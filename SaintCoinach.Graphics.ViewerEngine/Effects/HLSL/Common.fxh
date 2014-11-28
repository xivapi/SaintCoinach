// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// -----------------------------------------------------------------------------
// The following code is a port of XNA StockEffects http://xbox.create.msdn.com/en-US/education/catalog/sample/stock_effects
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//-----------------------------------------------------------------------------
// Common.fxh
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

void AddSpecular(inout float4 color, float3 specular)
{
    color.rgb += specular * color.a;
}

float3 Unpack(float2 vIn)
{
    float3 vOut;
    vOut.rg = vIn.rg * 2 - 1;
    vOut.b = sqrt(1 - vIn.r * vIn.r - vIn.g * vIn.g);
    return vOut;
}

float4 ComputeCommonPSOutput(float4 diffuse, float4 specular, ColorPair lightResult, float a)
{
    float4 color = float4(saturate(AmbientColor + lightResult.Diffuse) * diffuse.rgb, 1);
    float3 totalSpecular = lightResult.Specular * specular.rgb;

    color.a = a;
    AddSpecular(color, totalSpecular);

    return float4(color.rgb, a);
};

float3 CalculateNormalFromMap(float3 bumpyness, float3 normal, float3 tangent, float3 binorm, float3 sample) {
    normal = normalize(normal);
    tangent = normalize(tangent);
    binorm = normalize(binorm);

    float3 bumps = (bumpyness * 2) * (sample - (0.5).xxx);
    float3 result = (bumps.x * tangent + bumps.y * binorm + bumps.z * normal);
    return normalize(result);
}

VSOutputBg ComputeBgVSOutput(VSInputBg vin)
{
    VSOutputBg vout;

    vout.TexCoord0 = vin.TexCoord0;
    vout.TexCoord1 = vin.TexCoord1;
    vout.Blend = vin.Blend;

    vout.WorldNormal = mul(vin.Normal, WorldInverseTranspose);
    vout.WorldTangent = mul(vin.Tangent, WorldInverseTranspose);
    vout.WorldBinormal = mul(vin.Binormal, WorldInverseTranspose);
    vout.PositionPS = mul(vin.Position, WorldViewProj);
    vout.PositionWS = mul(vin.Position, World);

    return vout;
}

VSOutputCharacter ComputeCharacterVSOutput(VSInputCharacter vin)
{
    VSOutputCharacter vout;

    vout.TexCoord = vin.TexCoord;

    vout.WorldNormal = mul(vin.Normal, WorldInverseTranspose);
    vout.WorldTangent = mul(vin.Tangent, WorldInverseTranspose);
    vout.WorldBinormal = mul(vin.Binormal, WorldInverseTranspose);
    vout.PositionPS = mul(vin.Position, WorldViewProj);
    vout.PositionWS = mul(vin.Position, World);

    return vout;
}