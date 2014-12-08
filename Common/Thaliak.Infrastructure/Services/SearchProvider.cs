using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Logging;

namespace Thaliak.Services {
    using Interfaces;
    using Search;

    [Export(typeof(ISearchProvider))]
    public class SearchProvider : ISearchProvider {
        /*[ImportMany(AllowRecomposition = true)]
        public Lazy<ISearchQuery, Behaviors.ISearchFunctionRegistration>[] QueryFunctions { get; set; }*/
        [ImportMany(AllowRecomposition = true)]
        public ExportFactory<ISearchQuery, Behaviors.ISearchFunctionRegistration>[] QueryFunctions { get; set; }
        

        [ImportMany(AllowRecomposition = true)]
        public Lazy<ISearchDataSource>[] DataSources { get; set; }
        [Import]
        private ILoggerFacade _Logger;

        #region ISearchProvider Members
        const string OpPattern = @"(?<OP#>[\+\|])";
        const string MdPattern = @"(?<MD#>[~])";
        const string FnPattern = @"(?<FN#>[^: \+\|~]+):";
        const string PadPattern = @"\s*";

        private static readonly Regex SplitPattern;
        private static readonly int PossibleSplitPatternCount;

        static SearchProvider() {
            var combinations = new[]{
                new [] { OpPattern, MdPattern, FnPattern },
                new [] { OpPattern, MdPattern },
                new [] { OpPattern, FnPattern },
                new [] { MdPattern, FnPattern },
                new [] { OpPattern },
                new [] { MdPattern},
                new [] { FnPattern },
            };
            var pad = PadPattern;

            var patternBuilder = new StringBuilder();
            patternBuilder.Append(@"(?:""(?<Args>[^""]*)""\s*)?(?:");
            for (var i = 0; i < combinations.Length; ++i) {
                patternBuilder.AppendFormat("(?<P{0}>", i);
                patternBuilder.Append(string.Join(pad, combinations[i]).Replace("#", i.ToString()));
                patternBuilder.Append(")|");
            }
            patternBuilder.Append("(?<EOS>$))");

            SplitPattern = new Regex(patternBuilder.ToString(), RegexOptions.Compiled | RegexOptions.Singleline);
            PossibleSplitPatternCount = combinations.Length;
        }
        
        const string DefaultFunction = "text";

        public ISearchQuery Parse(string queryString) {
            if (string.IsNullOrWhiteSpace(queryString))
                return null;

            QueryCollection currentCollection = null;
            var currentFunc = DefaultFunction;
            var currentArgsStart = 0;
            var currentInverted = false;
            Match match = SplitPattern.Match(queryString);
            while (match.Success) {
                if (match.Index > currentArgsStart) {
                    string args;
                    if (match.Groups["Args"].Success)
                        args = match.Groups["Args"].Value;
                    else {
                        var argsLen = match.Index - currentArgsStart;
                        args = queryString.Substring(currentArgsStart, argsLen);
                    }

                    var q = GetQuery(currentFunc.Trim(), args.Trim());
                    if (currentInverted)
                        q = new InvertQuery { InnerQuery = q };

                    if (currentCollection == null)
                        currentCollection = new AndQuery();
                    currentCollection.Add(q);

                    if (match.Groups["EOS"].Success)  // End of string
                        break;
                }

                var isSuccess = false;
                Group gOp = null, gFn = null, gMd = null;
                for (var i = 0; i < PossibleSplitPatternCount && !isSuccess; ++i) {
                    var iStr = i.ToString();
                    if (match.Groups["P" + iStr].Success) {
                        gOp = match.Groups["OP" + iStr];
                        gFn = match.Groups["FN" + iStr];
                        gMd = match.Groups["MD" + iStr];
                        isSuccess = true;
                    }
                }

                if (!isSuccess) {   // Matched to end of string on first loop
                    string args;
                    if (match.Groups["Args"].Success)
                        args = match.Groups["Args"].Value;
                    else
                        args = queryString;

                    var q = GetQuery(DefaultFunction, args);

                    if (q != null) {
                        if (currentCollection == null)
                            currentCollection = new AndQuery();
                        currentCollection.Add(q);
                    }
                    break;
                }

                currentFunc = gFn.Success ? gFn.Value : DefaultFunction;
                currentInverted = gMd.Success ? (gMd.Value == "~") : false;
                currentArgsStart = match.Index + match.Length;

                var op = gOp != null && gOp.Success ? gOp.Value : "+";
                switch (op) {
                    case "|":
                        CheckCollectionType<OrQuery>(ref currentCollection);
                        break;
                    case "+":
                        CheckCollectionType<AndQuery>(ref currentCollection);
                        break;
                    default:
                        throw new NotSupportedException();  // Did somebody forget to add operator here after changing the regex?!
                }

                match = match.NextMatch();
            }

            return currentCollection;
        }
        private static void CheckCollectionType<T>(ref QueryCollection collection) where T : QueryCollection, new() {
            if (collection == null || collection.GetType() != typeof(T)) {
                var prev = collection;
                collection = new T();
                if (prev != null)
                    collection.Add(prev);
            }
        }

        public ISearchQuery GetQuery(string function, string args) {
            var funcFactory = QueryFunctions.FirstOrDefault(_ => string.Equals(_.Metadata.Function, function, StringComparison.OrdinalIgnoreCase));
            if (funcFactory == null)
                return null;
            var func = funcFactory.CreateExport().Value;
            func.Set(args);
            return func;
        }

        public System.Collections.IEnumerable Search(Interfaces.ISearchQuery query) {
            var queryTypes = query.MatchedTypes.Where(_ => !_.IsInterface).ToArray();
            IEnumerable<Lazy<ISearchDataSource>> sources;
            if (!queryTypes.Any() || queryTypes.Any(_ => _ == typeof(object)))
                sources = DataSources.Where(s => s.Value.IncludeByDefault);
            else
                sources = DataSources.Where(s => s.Value.ContainedTypes.Any(t => queryTypes.Any(qt => t.IsAssignableFrom(qt) || qt.IsAssignableFrom(t))));
            var sourceEnum = sources.SelectMany(s => s.Value.GetEnumerable().Cast<object>());

            return sourceEnum.Where(o => query.IsMatch(o));
        }
        #endregion
    }
}
