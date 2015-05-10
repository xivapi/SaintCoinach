using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Graphics {
    public class ModelAttribute {
        private static Dictionary<string, uint> MaskMap = new Dictionary<string, uint> {
            // TODO: Not actually confirmed that these are all correct.
            // tv_* seen on equipment > top
            { "atr_tv_a", 1 << 0 },
            { "atr_tv_b", 1 << 1 },
            { "atr_tv_c", 1 << 2 },
            { "atr_tv_d", 1 << 3 },
            { "atr_tv_e", 1 << 4 },
            { "atr_tv_f", 1 << 5 },
            { "atr_tv_g", 1 << 6 },
            { "atr_tv_h", 1 << 7 },
            { "atr_tv_i", 1 << 8 },
            { "atr_tv_j", 1 << 9 },

            // mv_* seen on equipment > met
            { "atr_mv_a", 1 << 0 },
            { "atr_mv_b", 1 << 1 },
            { "atr_mv_c", 1 << 2 },
            { "atr_mv_d", 1 << 3 },
            { "atr_mv_e", 1 << 4 },
            { "atr_mv_f", 1 << 5 },
            { "atr_mv_g", 1 << 6 },
            { "atr_mv_h", 1 << 7 },
            { "atr_mv_i", 1 << 8 },
            { "atr_mv_j", 1 << 9 },

            // bv_* seen on * > body
            { "atr_bv_a", 1 << 0 },
            { "atr_bv_b", 1 << 1 },
            { "atr_bv_c", 1 << 2 },
            { "atr_bv_d", 1 << 3 },
            { "atr_bv_e", 1 << 4 },
            { "atr_bv_f", 1 << 5 },
            { "atr_bv_g", 1 << 6 },
            { "atr_bv_h", 1 << 7 },
            { "atr_bv_i", 1 << 8 },
            { "atr_bv_j", 1 << 9 },

            // gv_* (probably) on equipment > glv
            { "atr_gv_a", 1 << 0 },
            { "atr_gv_b", 1 << 1 },
            { "atr_gv_c", 1 << 2 },
            { "atr_gv_d", 1 << 3 },
            { "atr_gv_e", 1 << 4 },
            { "atr_gv_f", 1 << 5 },
            { "atr_gv_g", 1 << 6 },
            { "atr_gv_h", 1 << 7 },
            { "atr_gv_i", 1 << 8 },
            { "atr_gv_j", 1 << 9 },

            // dv_* (probably) on equipment > dwn
            { "atr_dv_a", 1 << 0 },
            { "atr_dv_b", 1 << 1 },
            { "atr_dv_c", 1 << 2 },
            { "atr_dv_d", 1 << 3 },
            { "atr_dv_e", 1 << 4 },
            { "atr_dv_f", 1 << 5 },
            { "atr_dv_g", 1 << 6 },
            { "atr_dv_h", 1 << 7 },
            { "atr_dv_i", 1 << 8 },
            { "atr_dv_j", 1 << 9 },
            
            // sv_* (probably) on equipment > sho
            { "atr_sv_a", 1 << 0 },
            { "atr_sv_b", 1 << 1 },
            { "atr_sv_c", 1 << 2 },
            { "atr_sv_d", 1 << 3 },
            { "atr_sv_e", 1 << 4 },
            { "atr_sv_f", 1 << 5 },
            { "atr_sv_g", 1 << 6 },
            { "atr_sv_h", 1 << 7 },
            { "atr_sv_i", 1 << 8 },
            { "atr_sv_j", 1 << 9 },
        };

        #region Properties
        public ModelDefinition Definition { get; private set; }
        public string Name { get; private set; }
        public int Index { get; private set; }
        public uint AttributeMask { get; private set; }
        #endregion

        #region Constructor
        public ModelAttribute(ModelDefinition definition, int index) {
            this.Definition = definition;
            this.Index = index;
            this.Name = definition.AttributeNames[index];

            if (MaskMap.ContainsKey(Name))
                AttributeMask = MaskMap[Name];
            else
                AttributeMask = 0;
        }
        #endregion
    }
}
