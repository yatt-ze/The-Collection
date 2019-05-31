using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NoisetteCore.Protection.ConstantMutation
{
    internal class ConstantMutationProtection
    {
        public static void MutateConstant(ModuleDefMD module)
        {
            //Collatz Conjecture first to replace 1s
            //Protection.ConstantMutation.Mutator.CollatzConjecture();
            ////Then replace 0s
            //Protection.ConstantMutation.Mutator.ZeroReplacer();
            ////Replace Boolean value with actuall code
            //Protection.ConstantMutation.Mutator.Booleanisator();
            ////Insert inline integer value set;
            //Protection.ConstantMutation.Mutator.InlineInteger();
            ////ListContainChecker
            ////Protection.ConstantMutation.Mutator.ListChecker();
            ////todo : fix this sh*t
        }
    }

    //public static class Mutator
    //{
    //    public static void CollatzConjecture()
    //    {
    //        //inject the collatz class
    //

    //        //check for ldci which value is '1'
    //        foreach (TypeDef type in Core.Property.module.Types)
    //        {
    //            if (type.IsGlobalModuleType) continue;
    //            foreach (MethodDef method in type.Methods)
    //            {
    //                if (!method.HasBody) continue;
    //
    //            }
    //        }
    //    }

    //    public static void Booleanisator()
    //    {
    //        foreach (TypeDef type in Core.Property.module.Types)
    //        {
    //            if (type.IsGlobalModuleType) continue;
    //            foreach (MethodDef method in type.Methods)
    //            {
    //                if (!method.HasBody) continue;
    //                var instr = method.Body.Instructions;
    //                for (int i = 0; i < instr.Count; i++)
    //                {
    //                    if (instr[i].OpCode == OpCodes.Callvirt)
    //                    {
    //                        if (instr[i].Operand.ToString().ToLower().Contains("bool"))
    //                        {
    //                            if (instr[i - 1].OpCode == OpCodes.Ldc_I4_0)
    //                            {
    //                                FieldInfo fieldZ = null;
    //                                Local bool_local = new Local(Core.Property.module.CorLibTypes.Boolean);
    //                                method.Body.Variables.Add(bool_local);

    //                                var r2 = new Instruction();

    //                                var fieldarray = typeof(System.String).GetFields();
    //                                foreach (var field in fieldarray)
    //                                {
    //                                    if (field.Name == "Empty")
    //                                    {
    //                                        r2 = new Instruction(OpCodes.Ldsfld, field);
    //                                        fieldZ = field;
    //                                        break;
    //                                    }
    //                                }

    //                                instr[i - 1].OpCode = OpCodes.Ldsfld;
    //                                instr[i - 1].Operand = method.Module.Import(fieldZ);
    //                                instr.Insert(i,
    //                                    Instruction.Create(OpCodes.Call,
    //                                        method.Module.Import(typeof(System.String).GetMethod("IsNullOrEmpty"))));
    //                                instr.Insert(i + 1, Instruction.Create(OpCodes.Stloc_S, bool_local));
    //                                instr.Insert(i + 2, Instruction.Create(OpCodes.Ldloc_S, bool_local));
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    /*
    //     * You may think you know what the following code does.
    //     * But you dont. Trust me.
    //     * Fiddle with it, and youll spend many a sleepless
    //     * night cursing the moment you thought youd be clever
    //     * enough to "optimize" the code below.
    //     * Now close this file and go play with something else.
    //     */

    //    public static void ZeroReplacer()
    //    {
    //        Random rnd = new Random();

    //        bool empty = false;
    //        bool list = false;

    //        ModuleDefMD typeModule = ModuleDefMD.Load(typeof(Runtime.NewArray).Module);
    //        TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.NewArray).MetadataToken));
    //        MethodDef cctor = Core.Property.module.GlobalType.FindOrCreateStaticConstructor();
    //        IEnumerable<IDnlibDef> members = Inject_Helper.InjectHelper.Inject(typeDef, Core.Property.module.GlobalType,
    //            Core.Property.module);
    //        var init = (FieldDef)members.Single(method2 => method2.Name == "noisette");
    //        if (init == null) throw new ArgumentNullException(nameof(init));
    //        Random rnd4 = new Random();
    //        init.Name = init.Name + "" + rnd4.Next(0, 99999999);

    //        foreach (TypeDef type in Core.Property.module.Types)
    //        {
    //            if (type.IsGlobalModuleType) continue;
    //            foreach (MethodDef method in type.Methods)
    //            {
    //                if (method.FullName.Contains("My.")) continue; //VB gives cancer anyway
    //                if (!method.HasBody) continue;
    //                if (method.Body.HasExceptionHandlers) continue;
    //                var instr = method.Body.Instructions;
    //                for (int i = 0; i < instr.Count; i++)
    //                {
    //                    if (!instr[i].IsLdcI4()) continue;

    //                    switch (rnd.Next(0, 2))
    //                    {
    //                        case 0:
    //                        if (empty)
    //                        {
    //                            goto case 1;
    //                        }
    //                        empty = true;
    //                        list = false;

    //                        break;

    //                        case 1:
    //                        if (list)
    //                        {
    //                            goto case 0;
    //                        }
    //                        list = true;
    //                        empty = false;

    //                        switch (rnd.Next(0, 2))
    //                        {
    //                            case 0:
    //                            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Add));
    //                            break;

    //                            case 1:
    //                            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sub));
    //                            break;
    //                        }
    //                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Conv_I4));
    //                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldlen));
    //                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Callvirt, method.Module.Import(typeof(List<String>).GetMethod("ToArray"))));

    //                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldsfld, init));

    //                        i += 5;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //

    //    public static void ListChecker()
    //    {
    //        List<FieldDef> CreatedFieldDef = new List<FieldDef>();

    //        foreach (TypeDef type in Core.Property.module.Types)
    //        {
    //            if (type.IsGlobalModuleType) continue;
    //            for (int index = 0; index < type.Methods.Count; index++)
    //            {
    //                MethodDef method = type.Methods[index];
    //                if (!method.HasBody) continue;
    //                if (Protection.ConstantOutlinning.ConstantOutlinningProtection.ProxyMethodConst.Contains(method)) continue;
    //                if (method.IsConstructor) continue;
    //                if (method.FullName.Contains("My.")) continue; //VB gives cancer anyway
    //                var instr = method.Body.Instructions;

    //                //var value = instr[i].GetLdcI4Value();

    //                //Create a new Field
    //                SZArraySig array2 = new SZArraySig(method.Module.CorLibTypes.String);
    //                ITypeDefOrRef type1 = array2.ToTypeDefOrRef();
    //                TypeSig type2 = type1.ToTypeSig();
    //                FieldDef new_list = new FieldDefUser(Protection.Renaming.RenamingProtection.GenerateNewName(),
    //                    new FieldSig(type2),
    //                    dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
    //                type.Fields.Add(new_list);

    //                //Go to staticconstructor cctor
    //                var cctor = type.FindOrCreateStaticConstructor();
    //                //Add the array initializer code

    //                cctor.Body.Instructions.Insert(0, new Instruction(OpCodes.Ldc_I4_0));

    //                cctor.Body.Instructions.Insert(1, new Instruction(OpCodes.Newarr, method.Module.Import(typeof(string))));
    //                cctor.Body.Instructions.Insert(2,
    //                    new Instruction(OpCodes.Stsfld, new_list));

    //                //Go to default constructor ctor
    //                var ctor = type.FindDefaultConstructor();
    //                if (ctor == null) continue;
    //                //add Local list variable
    //                var listGenericInstSig =
    //                    method.Module.ImportAsTypeSig(typeof(System.Collections.Generic.List<String>));
    //                //store it in method body
    //                Local loc = new Local(listGenericInstSig);
    //                ctor.Body.Variables.Add(loc);
    //                // Create TypeSpec from GenericInstSig
    //                var listTypeSpec = new TypeSpecUser(listGenericInstSig);
    //                // Create System.Collections.Generic.List<String>::.ctor method reference
    //                var listCtor = new MemberRefUser(method.Module, ".ctor",
    //                    MethodSig.CreateInstance(method.Module.CorLibTypes.Void), listTypeSpec);
    //                // Create Add(!0) method reference, !0 signifying first generic argument of declaring type
    //                // In this case, would be Add(String item)
    //                // (GenericMVar would be used for method generic argument, such as Add<!!0>(!!0))
    //                var listAdd = new MemberRefUser(method.Module, "Add",
    //                    MethodSig.CreateInstance(method.Module.CorLibTypes.Void, new GenericVar(0)), listTypeSpec);
    //                //Create ToArray() method reference
    //                var listToArray = new MemberRefUser(method.Module, "ToArray",
    //                    MethodSig.CreateInstance(method.Module.CorLibTypes.Void, new GenericVar(0)), listTypeSpec);

    //                //get ilbody
    //                var body = ctor.Body;
    //                //add instrutions
    //                body.Instructions.Insert(3, OpCodes.Newobj.ToInstruction(listCtor));
    //                body.Instructions.Insert(4, Instruction.Create(OpCodes.Stloc_S, loc)); // Store list to local[S]

    //                /**/
    //                body.Instructions.Insert(5, Instruction.Create(OpCodes.Ldloc_S, loc));
    //                body.Instructions.Insert(6, Instruction.Create(OpCodes.Ldstr, "1")); // Random string n1
    //                body.Instructions.Insert(7, OpCodes.Callvirt.ToInstruction(listAdd));
    //                body.Instructions.Insert(8, Instruction.Create(OpCodes.Nop));
    //                /**/
    //                body.Instructions.Insert(9, Instruction.Create(OpCodes.Ldloc_S, loc));
    //                body.Instructions.Insert(10, Instruction.Create(OpCodes.Ldstr, "2")); // Random string n2
    //                body.Instructions.Insert(11, OpCodes.Callvirt.ToInstruction(listAdd));
    //                body.Instructions.Insert(12, Instruction.Create(OpCodes.Nop));
    //                /**/
    //                body.Instructions.Insert(13, Instruction.Create(OpCodes.Ldloc_S, loc));
    //                body.Instructions.Insert(14, Instruction.Create(OpCodes.Callvirt,
    //                    method.Module.Import(typeof(List<String>).GetMethod("ToArray"))));
    //                body.Instructions.Insert(15, Instruction.Create(OpCodes.Stsfld, new_list));

    //                //Add the FieldDef to our list of FieldDef
    //                CreatedFieldDef.Add(new_list);

    //                for (int i = 0; i < instr.Count; i++)
    //                {
    //                    if (i + 10 > instr.Count)
    //                        break;
    //                    if (instr[i].IsLdcI4())
    //                    {
    //                        var value = instr[i].GetLdcI4Value();
    //                        var original = new Instruction(instr[i].OpCode, instr[i].Operand);
    //                        //true branch

    //                        //Add 2 Locals
    //                        Local int1 = new Local(method.Module.CorLibTypes.Int32);
    //                        Local int2 = new Local(method.Module.CorLibTypes.Int32);
    //                        method.Body.Variables.Add(int1);
    //                        method.Body.Variables.Add(int2);

    //                        method.Body.Instructions.Insert(i, OpCodes.Nop.ToInstruction());
    //                        method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldsfld, new_list));
    //                        method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldstr, "2")); //the string to search for
    //                        //public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
    //                        MethodInfo the_enumerable_method_ = null;
    //                        ParameterInfo param1 = null;
    //                        ParameterInfo param2 = null;

    //                        foreach (var m in typeof(System.Linq.Enumerable).GetMethods())
    //                        {
    //                            if (!m.Name.ToLower().Contains("contains")) continue;
    //                            var param = m.GetParameters();
    //                            //"Contains<TSource>(this IEnumerable<TSource> source, TSource value)"
    //                            if (param.Length != 2) continue;
    //                            the_enumerable_method_ = m;
    //                            param1 = param[0];
    //                            param2 = param[1];
    //                            break;
    //                        }

    //                        method.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Call, method.Module.Import(typeof(System.Linq.Enumerable).GetMethod(the_enumerable_method_.Name, new Type[] { param1.ParameterType, param2.ParameterType }))));
    //                        //method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Brtrue_S, instr[i + 6]));
    //                        method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, 14));//wrong ldci
    //                        // method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Br_S, instr[i + 6]));
    //                        method.Body.Instructions.Insert(i + 5, original);//right ldci
    //                        method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldloc_S, int1));

    //                        method.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Stloc_S, int1));
    //                        method.Body.Instructions.RemoveAt(i + 8);

    //                        method.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Brtrue_S, instr[i + 5]));
    //                        method.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Br_S, instr[i + 7]));

    //                        i += 20;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}