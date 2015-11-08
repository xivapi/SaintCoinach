using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text.Nodes {
    public interface INodeVisitor<T> {
        T Visit(XivString xivString);
        T Visit(OpenTag openTag);
        T Visit(CloseTag closeTag);
        T Visit(IfElement ifElement);
        T Visit(GenericElement genericElement);
        T Visit(EmptyElement emptyElement);
        T Visit(DefaultElement defaultElement);
        T Visit(Comparison comparison);
        T Visit(TopLevelParameter topLevelParameter);
        T Visit(SwitchElement switchElement);
        T Visit(StaticString staticString);
        T Visit(StaticInteger staticInteger);
        T Visit(StaticByteArray staticByteArray);
        T Visit(Parameter parameter);

    }
}
