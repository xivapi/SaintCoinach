using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;
using SaintCoinach.IO;
using Directory = System.IO.Directory;

namespace SaintCoinach.Graphics.Viewer.Interop
{
    public static class FbxExport
    {
        static class Interop
        {
            [DllImport("fbxInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int exportFbx([In, Out] IntPtr[] meshes, int numMeshes,
                                                [In, Out] byte[] skeleton, int skeletonSize,
                                                [In, Out] IntPtr[] anims, int totalAnims,
                                                [In, Out] int[] boneMap, int mapLength,
                                                string filename, int mode);
        }

        [HandleProcessCorruptedStateExceptions]
        public static int ExportFbx(string fileName,
                                        Mesh[] ma,
                                        Skeleton skele,
                                        List<PapFile> paps,
                                        int mode = 0)
        {
            ModelDefinition thisDefinition = ma[0].Model.Definition;

            // Create bonemap in the same manner that hkAnimationInterop does
            var nameMap = new Dictionary<string, int>();
            for (var i = 0; i < skele.BoneNames.Length; ++i)
                nameMap.Add(skele.BoneNames[i], i);
            var boneMap = thisDefinition.BoneNames.Select(n => nameMap[n]).ToArray();

            // Get mesh ptrs
            InteropMesh[] managedMeshes = new InteropMesh[ma.Length];
            for (int i = 0; i < ma.Length; i++)
                managedMeshes[i] = new InteropMesh(ma[i]);

            // Get animations ptrs
            InteropAnimation[] managedAnims = new InteropAnimation[paps.Count];
            int numPaps = paps.Count;
            if (paps.Count > 0)
            {
                for (int i = 0; i < paps.Count; i++)
                    managedAnims[i] = new InteropAnimation(paps[i]);
            }
            else
            {
                managedAnims = new InteropAnimation[0];
            }

            int result = 0;

            try
            {
                result = Interop.exportFbx(managedMeshes.Select(_ => _._UnmanagedPtr).ToArray(), managedMeshes.Length,
                    skele.File.HavokData, skele.File.HavokData.Length,
                    managedAnims.Select(_ => _._UnmanagedPtr).ToArray(), numPaps,
                    boneMap, boneMap.Length,
                    fileName, mode);
            }
            catch (Win32Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Native error code " + e.NativeErrorCode + " encountered.");
                return 1;
            }
            catch (Exception e)
            {
                if (e is AccessViolationException || e is SEHException)
                {
                    System.Diagnostics.Debug.WriteLine($"Access violation with\n" +
                                                       $"Model:\t {thisDefinition.File.Path}\n" +
                                                       $"Anims:\t {numPaps}\n" +
                                                       $"Skele:\t {skele.File.File.Path}\n");
                }

                return 1;
            }
            
            // Prevent collection of mesh and anim resources during unmanaged code
            GC.KeepAlive(managedMeshes);
            GC.KeepAlive(managedAnims);

            return result;
        }

        // This is monster-specific because the loading of actual materials assumes b0000 for some reason?
        public static void ExportMonsterMaterials(ARealmReversed realm, string folder, MaterialDefinition[] thisDefinitionMaterials, ImcVariant variant)
        {
            string format = "chara/monster/m{0}/obj/body/b{1}/material/v{2:D4}{3}";
            
            foreach (var material in thisDefinitionMaterials)
            {
                string path = material.Name;
                string m = path.Substring(path.IndexOf("_m") + 2, 4);
                string b = path.Substring(path.IndexOf("_m") + 7, 4);

                Material mat = new Material(material, realm.Packs.GetFile(String.Format(format, m, b, variant.Variant, path)), variant);
                foreach (var tex in mat.TexturesFiles)
                {
                    string texFile = tex.Path.Substring(tex.Path.LastIndexOf('/')).Replace(".tex", ".png");
                    string output = folder + '\\' + texFile;
                    tex.GetImage().Save(output);
                }
            }
        }
    }
}
