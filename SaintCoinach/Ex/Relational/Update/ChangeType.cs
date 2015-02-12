using System;

namespace SaintCoinach.Ex.Relational.Update {
    [Flags]
    public enum ChangeType {
        None = 0x00,
        Structure = 0x01,
        Data = 0x02,
        Breaking = 0x04
    }
}
