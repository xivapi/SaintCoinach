using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintCoinach.Text {
    public enum ExpressionType : byte {
        GreaterThanOrEqualTo = 0xE0,    // Followed by two variables
        UnknownComparisonE1 = 0xE1,     // Followed by one variable
        LessThanOrEqualTo = 0xE2,       // Followed by two variables
        NotEqual = 0xE3,                // Followed by one variable
        Equal = 0xE4,                   // Followed by two variables
        InputParameter = 0xE8,          // Followed by one variable
        PlayerParameter = 0xE9,         // Followed by one variable
        UnknownParameterEA = 0xEA,      // Followed by one variable
        UnknownParameterEB = 0xEB,      // Followed by one variable
        
        Byte = 0xF0,
        Int16_MinusOne = 0xF1,          // Followed by a Int16 that is one too high
        Int16_1 = 0xF2,                 // Followed by a Int16
        Int16_2 = 0xF4,                 // Followed by a Int16
        Int24_MinusOne = 0xF5,          // Followed by a Int24 that is one too high
        Int24 = 0xF6,                   // Followed by a Int24

        Int24_Color = 0xFA,             // Followed by a Int24, but converted to Int32 with upper 8 bits set to 1
        Int32 = 0xFE,                   // Followed by a Int32

        Decode = 0xFF,                  // Followed by length (inlcuding length) and data
    }
}
