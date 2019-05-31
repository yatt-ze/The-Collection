using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoisetteCore.Protection.AntiTampering
{
    public class AntiTamperingProtection
    {
        private ModuleDefMD _module;

        public AntiTamperingProtection(ModuleDefMD module)
        {
            _module = module;
        }

        public void Process()
        {
            AddCall(_module);
        }

        public static void Md5(string filePath)
        {
            //We get the md5 as byte, of the target
            byte[] md5bytes = System.Security.Cryptography.MD5.Create().ComputeHash(System.IO.File.ReadAllBytes(filePath));
            //Let's use FileStream to edit the file's byte
            using (var stream = new FileStream(filePath, FileMode.Append))
            {
                //Append md5 in the end
                stream.Write(md5bytes, 0, md5bytes.Length);
            }
        }

        private void AddCall(ModuleDef module)
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(EOFAntitamp).Module);
            MethodDef cctor = module.GlobalType.FindOrCreateStaticConstructor();
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(EOFAntitamp).MetadataToken));
            IEnumerable<IDnlibDef> members = Inject_Helper.InjectHelper.Inject(typeDef, module.GlobalType, module);

            var init = (MethodDef)members.Single(method => method.Name == "Initialize");

            cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));

            foreach (TypeDef type in module.Types)
            {
                if (type.IsGlobalModuleType) continue;
                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    if (method.IsConstructor)
                    {
                        method.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Nop));
                        method.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));
                    }
                }
            }

            foreach (MethodDef md in module.GlobalType.Methods)
            {
                if (md.Name != ".ctor") continue;
                module.GlobalType.Remove(md);
                break;
            }
        }
    }
}