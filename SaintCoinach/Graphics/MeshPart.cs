using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class MeshPart {
        #region Properties
        public Mesh Mesh { get; private set; }
        public MeshPartHeader Header { get; private set; }
        public int IndexOffset { get { return Header.IndexOffset; } }
        public int IndexCount { get { return Header.IndexCount; } }

        public ModelAttribute[] Attributes { get; private set; }
        #endregion

        #region Constructor
        internal MeshPart(Mesh mesh, MeshPartHeader header, byte[] indexBuffer) {
            this.Mesh = mesh;
            this.Header = header;

            var attr = new List<ModelAttribute>();

            for (var i = 0; i < Mesh.Model.Definition.Attributes.Length; ++i) {
                if(((Header.AttributesMask >> i) & 1) == 1)
                    attr.Add(Mesh.Model.Definition.Attributes[i]);
            }

            Attributes = attr.ToArray();
        }
        #endregion
    }
}
