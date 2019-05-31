using dnlib.DotNet;
using NoisetteCore.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NoisetteCore.Protection.Renaming
{
    public class RenamingProtection
    {
        public ModuleDefMD _module;

        public ModuleDefMD mscorlib;

        public List<string> UsedNames;

        public RenamingProtection(ModuleDefMD module)
        {
            mscorlib = ModuleDefMD.Load(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "mscorlib.dll"));
            UsedNames = new List<string>();
            _module = module;
        }

        public void RenameModule()
        {
            Obfuscation.RenameAnalyzer RA = new Obfuscation.RenameAnalyzer(_module);
            RA.PerformAnalyze();
        }

        public string GenerateNewName(RenamingProtection RP)
        {
            string str = null;
            foreach (TypeDef type in RP.mscorlib.Types)
            {
                if (!RP.UsedNames.Contains(type.Name))
                {
                    RP.UsedNames.Add(type.Name);
                    str = type.Name;
                    break;
                }
            }
            return str;
        }
    }
}