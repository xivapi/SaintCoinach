using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;

namespace SaintCoinach.Graphics.Viewer {
    public class EffectPointLightVariable {
        #region Fields
        private EffectVariable _StructureVariable;
        private EffectVectorVariable _DirectionMember;
        private EffectVectorVariable _DiffuseMember;
        private EffectVectorVariable _SpecularMember;
        #endregion

        #region Properties
        public Vector3 Direction {
            get { return _DirectionMember.GetVector<Vector3>(); }
            set { _DirectionMember.Set(value); }
        }
        public Vector3 Diffuse {
            get { return _DiffuseMember.GetVector<Vector3>(); }
            set { _DiffuseMember.Set(value); }
        }
        public Vector3 Specular {
            get { return _SpecularMember.GetVector<Vector3>(); }
            set { _SpecularMember.Set(value); }
        }
        #endregion

        #region Constructor
        public EffectPointLightVariable(EffectVariable structureVar) {
            _StructureVariable = structureVar;

            _DirectionMember = structureVar.GetMemberByName("Direction").AsVector();
            _DiffuseMember = structureVar.GetMemberByName("Diffuse").AsVector();
            _SpecularMember = structureVar.GetMemberByName("Specular").AsVector();
        }
        #endregion

        public void Set(PointLight light) {
            this.Direction = light.Direction;
            this.Diffuse = light.Diffuse;
            this.Specular = light.Specular;
        }

        public PointLight Get() {
            PointLight light;

            light.Direction = this.Direction;
            light.Diffuse = this.Diffuse;
            light.Specular = this.Specular;

            return light;
        }
    }
}
