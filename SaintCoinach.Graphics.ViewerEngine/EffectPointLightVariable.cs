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

        #region Constructor
        public EffectPointLightVariable(EffectVariable structureVar) {
            _StructureVariable = structureVar;

            _DirectionMember = structureVar.GetMemberByName("Direction").AsVector();
            _DiffuseMember = structureVar.GetMemberByName("Diffuse").AsVector();
            _SpecularMember = structureVar.GetMemberByName("Specular").AsVector();
        }
        #endregion

        public void Set(PointLight light) {
            _DirectionMember.Set(light.Direction);
            _DiffuseMember.Set(light.Diffuse);
            _SpecularMember.Set(light.Specular);
        }

        public PointLight Get() {
            PointLight light;

            light.Direction = _DirectionMember.GetVector<Vector3>();
            light.Diffuse = _DiffuseMember.GetVector<Vector3>();
            light.Specular = _SpecularMember.GetVector<Vector3>();

            return light;
        }
    }
}
