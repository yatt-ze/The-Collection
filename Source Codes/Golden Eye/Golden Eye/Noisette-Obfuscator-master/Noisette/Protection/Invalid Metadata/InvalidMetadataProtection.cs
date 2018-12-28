using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoisetteCore.Protection.InvalidMetadata
{
    //public static class InvalidMD
    //{
    //    public static void InsertInvalidMetadata(ModuleDefMD module)
    //    {
    //        InvalidMD.process(module.Assembly);
    //    }

    //    public static void process(AssemblyDef asm)
    //    {
    //        ModuleDef manifestModule = asm.ManifestModule;
    //        manifestModule.Mvid = null;
    //        manifestModule.Name = Renaming.RenamingProtection.GenerateNewName();
    //        asm.ManifestModule.Import(new FieldDefUser(Renaming.RenamingProtection.GenerateNewName()));
    //        foreach (TypeDef current in manifestModule.Types)
    //        {
    //            if (current.IsGlobalModuleType) continue;
    //            TypeDef typeDef = new TypeDefUser(Renaming.RenamingProtection.GenerateNewName());
    //            typeDef.Methods.Add(new MethodDefUser());
    //            typeDef.NestedTypes.Add(new TypeDefUser(Renaming.RenamingProtection.GenerateNewName()));
    //            MethodDef item = new MethodDefUser();
    //            typeDef.Methods.Add(item);
    //            current.NestedTypes.Add(typeDef);
    //            current.Events.Add(new EventDefUser());
    //            MethodDef methodDef = new MethodDefUser();
    //            methodDef.MethodSig = new MethodSig();
    //            foreach (MethodDef current2 in current.Methods)
    //            {
    //                //If method has reflection assembly references
    //                if (Core.Property.ContainsReflectionReference.Contains(current2)) continue;
    //                if (current2.IsConstructor) continue;
    //                if (!current2.HasBody) continue;
    //                if (current2.FullName.Contains("My.")) continue; //VB gives cancer anyway

    //                if (current2.Body != null)
    //                {
    //                    current2.Body.SimplifyBranches();
    //                    if ((current2.ReturnType.FullName == "System.Void") && current2.HasBody && current2.Body.Instructions.Count != 0 && (!current2.Body.HasExceptionHandlers))
    //                    {
    //                        TypeSig typeSig = asm.ManifestModule.Import(typeof(int)).ToTypeSig();
    //                        Local local = new Local(typeSig);
    //                        TypeSig typeSig2 = asm.ManifestModule.Import(typeof(bool)).ToTypeSig();
    //                        Local local2 = new Local(typeSig2);
    //                        current2.Body.Variables.Add(local);
    //                        current2.Body.Variables.Add(local2);
    //                        Instruction operand = current2.Body.Instructions[current2.Body.Instructions.Count - 1];
    //                        Instruction instruction = new Instruction(OpCodes.Ret);
    //                        Instruction instruction2 = new Instruction(OpCodes.Ldc_I4_1);
    //                        current2.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldc_I4_0));
    //                        current2.Body.Instructions.Insert(1, new Instruction(OpCodes.Stloc, local));
    //                        current2.Body.Instructions.Insert(2, new Instruction(OpCodes.Br, instruction2));
    //                        Instruction instruction3 = new Instruction(OpCodes.Ldloc, local);
    //                        current2.Body.Instructions.Insert(3, instruction3);
    //                        current2.Body.Instructions.Insert(4, new Instruction(OpCodes.Ldc_I4_0));
    //                        current2.Body.Instructions.Insert(5, new Instruction(OpCodes.Ceq));
    //                        current2.Body.Instructions.Insert(6, new Instruction(OpCodes.Ldc_I4_1));
    //                        current2.Body.Instructions.Insert(7, new Instruction(OpCodes.Ceq));
    //                        current2.Body.Instructions.Insert(8, new Instruction(OpCodes.Stloc, local2));
    //                        current2.Body.Instructions.Insert(9, new Instruction(OpCodes.Ldloc, local2));
    //                        current2.Body.Instructions.Insert(10, new Instruction(OpCodes.Brtrue, current2.Body.Instructions[10]));
    //                        current2.Body.Instructions.Insert(11, new Instruction(OpCodes.Ret));
    //                        current2.Body.Instructions.Insert(12, new Instruction(OpCodes.Calli));
    //                        current2.Body.Instructions.Insert(13, new Instruction(OpCodes.Sizeof, operand));
    //                        current2.Body.Instructions.Insert(14, new Instruction(OpCodes.Nop));
    //                        current2.Body.Instructions.Insert(current2.Body.Instructions.Count, instruction2);
    //                        current2.Body.Instructions.Insert(current2.Body.Instructions.Count, new Instruction(OpCodes.Stloc, local2));
    //                        current2.Body.Instructions.Insert(current2.Body.Instructions.Count, new Instruction(OpCodes.Br, instruction3));
    //                        current2.Body.Instructions.Insert(current2.Body.Instructions.Count, instruction);
    //                        ExceptionHandler exceptionHandler = new ExceptionHandler(ExceptionHandlerType.Finally);
    //                        exceptionHandler.HandlerStart = current2.Body.Instructions[10];
    //                        exceptionHandler.HandlerEnd = current2.Body.Instructions[11];
    //                        exceptionHandler.TryEnd = current2.Body.Instructions[14];
    //                        exceptionHandler.TryStart = current2.Body.Instructions[12];
    //                        if (!current2.Body.HasExceptionHandlers)
    //                        {
    //                            current2.Body.ExceptionHandlers.Add(exceptionHandler);
    //                        }
    //                        operand = new Instruction(OpCodes.Br, instruction);
    //                        current2.Body.OptimizeBranches();
    //                        current2.Body.OptimizeMacros();
    //                    }
    //                }
    //            }
    //        }
    //        TypeDef typeDef2 = new TypeDefUser(Renaming.RenamingProtection.GenerateNewName()); // name cannot be null
    //        FieldDef item2 = new FieldDefUser(Renaming.RenamingProtection.GenerateNewName(), new FieldSig(manifestModule.Import(typeof(N0isette)).ToTypeSig()));
    //        typeDef2.Fields.Add(item2);
    //        typeDef2.BaseType = manifestModule.Import(typeof(N0isette));
    //        manifestModule.Types.Add(typeDef2);
    //        TypeDef typeDef3 = new TypeDefUser(Renaming.RenamingProtection.GenerateNewName());
    //        typeDef3.IsInterface = true;
    //        typeDef3.IsSealed = true;
    //        manifestModule.Types.Add(typeDef3);
    //        manifestModule.TablesHeaderVersion = new ushort?(257);

    //        Core.Property.opts.MetaDataOptions.TablesHeapOptions.ExtraData = 0xd4d;
    //        Core.Property.opts.MetaDataOptions.TablesHeapOptions.UseENC = false;
    //        Core.Property.opts.MetaDataOptions.MetaDataHeaderOptions.VersionString =
    //            Core.Property.opts.MetaDataOptions.MetaDataHeaderOptions.VersionString + "№";
    //        Core.Property.opts.MetaDataOptions.OtherHeapsEnd.Add(new RawHeap("#Noisette", new byte[1]));
    //    }

    //    private class RawHeap : HeapBase
    //    {
    //        private readonly byte[] content;
    //        private readonly string name;

    //        public RawHeap(string name, byte[] content)
    //        {
    //            this.name = name;
    //            this.content = content;
    //        }

    //        public override string Name
    //        {
    //            get { return name; }
    //        }

    //        public override uint GetRawLength()
    //        {
    //            return (uint)content.Length;
    //        }

    //        protected override void WriteToImpl(BinaryWriter writer)
    //        {
    //            writer.Write(content);
    //        }
    //    }
    //}
}