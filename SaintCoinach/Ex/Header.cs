using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Ex {
    public class Header {
        protected static readonly Dictionary<int, Language> LanguageMap = new Dictionary<int, Language> {
            { 0, Language.None },
            { 1, Language.Japanese },
            { 2, Language.English },
            { 3, Language.German },
            { 4, Language.French },
            { 5, Language.Unsupported /*"chs"*/ },
            { 6, Language.Unsupported /*"cht"*/ },
            { 7, Language.Unsupported /*"ko"*/ },
        };

        #region Fields
        private ExCollection _Collection;
        private IO.File _File;
        private string _Name;
        private Column[] _Columns;
        private Range[] _DataFileRanges;
        private Language[] _AvailableLanguages;
        private int _FixedSizeDataLength;
        #endregion

        #region Properties
        public ExCollection Collection { get { return _Collection; } }
        public IO.File File { get { return _File; } }
        public string Name { get { return _Name; } }
        public IEnumerable<Column> Columns { get { return _Columns; } }
        public IEnumerable<Range> DataFileRanges { get { return _DataFileRanges; } }
        public IEnumerable<Language> AvailableLanguages { get { return _AvailableLanguages; } }
        public int FixedSizeDataLength { get { return _FixedSizeDataLength; } }

        public Column GetColumn(int index) { return _Columns[index]; }
        #endregion

        #region Constructor
        public Header(ExCollection collection, string name, IO.File file) {
            _Collection = collection;
            _Name = name;
            _File = file;

            Build();
        }
        #endregion

        #region Build
        private void Build() {
            const uint Magic = 0x46485845;  // EXHF
            const int MinimumLength = 0x2E;
            const int FixedSizeDataLengthOffset = 0x06;
            const int DataOffset = 0x20;

            var buffer = File.GetData();

            if (buffer.Length < MinimumLength)
                throw new InvalidDataException("EXH file is too short");
            if (OrderedBitConverter.ToUInt32(buffer, 0, false) != Magic)
                throw new InvalidDataException("File not a EX header");

            _FixedSizeDataLength = OrderedBitConverter.ToUInt16(buffer, FixedSizeDataLengthOffset, true);

            int currentPosition = DataOffset;
            ReadColumns(buffer, ref currentPosition);
            ReadPartialFiles(buffer, ref currentPosition);
            ReadSuffixes(buffer, ref currentPosition);
        }
        private void ReadColumns(byte[] buffer, ref int position) {
            const int CountOffset = 0x08;
            const int Length = 0x04;

            var count = OrderedBitConverter.ToUInt16(buffer, CountOffset, true);
            _Columns = new Column[count];
            for (int i = 0; i < count; ++i) {
                _Columns[i] = CreateColumn(i, buffer, position);
                position += Length;
            }
        }
        private void ReadPartialFiles(byte[] buffer, ref int position) {
            const int CountOffset = 0x0A;
            const int Length = 0x08;

            var count = OrderedBitConverter.ToUInt16(buffer, CountOffset, true);
            _DataFileRanges = new Range[count];
            for (int i = 0; i < count; ++i) {
                var min = OrderedBitConverter.ToInt32(buffer, position + 0x00, true);
                var len = OrderedBitConverter.ToInt32(buffer, position + 0x04, true);

                _DataFileRanges[i] = new Range(min, len);

                position += Length;
            }
        }
        private void ReadSuffixes(byte[] buffer, ref int position) {
            const int CountOffset = 0x0C;
            const int Length = 0x02;

            var count = OrderedBitConverter.ToUInt16(buffer, CountOffset, true);
            var langs = new List<Language>();
            for (int i = 0; i < count; ++i) {
                langs.Add(LanguageMap[buffer[position]]);
                position += Length;
            }
            _AvailableLanguages = langs.Where(_ => _ != Language.Unsupported).ToArray();
        }
        #endregion

        #region Factory
        protected virtual Column CreateColumn(int index, byte[] data, int offset) {
            return new Column(this, index, data, offset);
        }
        #endregion
    }
}
