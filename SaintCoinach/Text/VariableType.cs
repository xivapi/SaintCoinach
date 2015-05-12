using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public enum VariableType : byte {
        LessThanOrEqualTo = 0xE0,       // Followed by two variables
        Value1 = 0xE1,                  // Followed by one variable
        GreaterThanOrEqualTo = 0xE2,    // Followed by two variables
        Invert = 0xE3,                  // Followed by one variable
        Equal = 0xE4,                   // Followed by two variables
        InputParameter = 0xE8,          // Followed by one variable
        PlayerParameter = 0xE9,         // Followed by one variable
        UnknownParameter1 = 0xEA,       // Followed by one variable
        UnknownParameter2 = 0xEB,       // Followed by one variable
        
        Byte = 0xF0,
        Int16_MinusOne = 0xF1,          // Followed by a Int16 that is one too high
        Int16_1 = 0xF2,                 // Followed by a Int16
        Int16_2 = 0xF4,                 // Followed by a Int16
        Int24_MinusOne = 0xF5,          // Followed by a Int24 that is one too high
        Int24 = 0xF6,                   // Followed by a Int24

        Decode = 0xFF,                  // Followed by length (inlcuding length) and data
    }
}
