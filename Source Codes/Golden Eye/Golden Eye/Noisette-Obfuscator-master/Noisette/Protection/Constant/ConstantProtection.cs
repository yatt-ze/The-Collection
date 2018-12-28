using dnlib.DotNet;
using dnlib.DotNet.Emit;
using NoisetteCore.Helper;
using NoisetteCore.Obfuscation;
using NoisetteCore.Protection.AntiTampering;
using NoisetteCore.Protection.Renaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MethodAttributes = dnlib.DotNet.MethodAttributes;
using MethodImplAttributes = dnlib.DotNet.MethodImplAttributes;

namespace NoisetteCore.Protection.Constant
{
    internal class ConstantProtection
    {
        private ModuleDefMD _module;
        public MethodDef CollatzCtor;
        public SafeRandom random;

        public List<MethodDef> ProxyMethodConst = new List<MethodDef>();
        public List<MethodDef> ProxyMethodStr = new List<MethodDef>();

        public RenamingProtection RP;

        public ConstantProtection(ModuleDefMD module)
        {
            RP = Obfuscation.ObfuscationProcess.RP;
            _module = module;
            random = new SafeRandom();
            InitializeCollatz();
        }

        public void DoProcess()
        {
            foreach (TypeDef type in _module.Types)
            {
                ExplodeMember(type);
            }
        }

        public void ExplodeMember(TypeDef type)
        {
            for (int index = 0; index < type.Methods.Count; index++)
            {
                var method = type.Methods[index];
                if (CanObfuscate(method))
                    ProcessProtection(method);
            }
        }

        public void ExplodeMember()
        {
        }

        public void ProcessProtection(MethodDef method)
        {
            //running tests for the moment

            var instr = method.Body.Instructions;
            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Ldc_I4)
                {
                    ProtectIntegers(method, i);
                    i += 10;
                }
            }
            StringHidding(method);
            CtorCallProtection(method);
            CallvirtProtection(method);
            LdfldProtection(method);
        }

        public void ProtectIntegers(MethodDef method, int i)
        {
            //InlineInteger(method, i);
            ReplaceValue(method, i);
            //OutelineValue(method, i);
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public void StringHidding(MethodDef method)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            var instr = method.Body.Instructions;

            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Ldstr)
                {
                    //if (instr[i + 1].OpCode == OpCodes.Call)
                    //{
                    var original_str_value = instr[i].Operand.ToString();
                    var modulector = method.Module.GlobalType;

                    FieldDefUser original_str_value_field = new FieldDefUser("str" + instr[i].Operand + "_" + method.Name,
                        new FieldSig(method.Module.CorLibTypes.String),
                        dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
                    modulector.Fields.Add(original_str_value_field);

                    var cctor = modulector.FindOrCreateStaticConstructor();

                    cctor.Body.Instructions.Insert(cctor.Body.Instructions.Count - 1,
                        OpCodes.Ldstr.ToInstruction(original_str_value));
                    cctor.Body.Instructions.Insert(cctor.Body.Instructions.Count - 1,
                        OpCodes.Stsfld.ToInstruction(original_str_value_field));

                    instr[i].OpCode = OpCodes.Ldsfld;
                    instr[i].Operand = original_str_value_field;
                    //instr.Insert(i - 1, .ToInstruction(original_str_value_field));

                    //var md = (MemberRef)instr[i + 1].Operand;
                    //if (md?.MethodSig.Params.Count == 1)
                    //{
                    //    var original_str_value = instr[i].Operand.ToString();
                    //    var modulector = method.Module.GlobalType;

                    //    FieldDefUser original_str_value_field = new FieldDefUser("str" + instr[i].Operand + "_" + method.Name,
                    //        new FieldSig(method.Module.CorLibTypes.String),
                    //        dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
                    //    modulector.Fields.Add(original_str_value_field);

                    //    var cctor = modulector.FindOrCreateStaticConstructor();

                    //    cctor.Body.Instructions.Insert(cctor.Body.Instructions.Count - 1,
                    //        OpCodes.Ldstr.ToInstruction(original_str_value));
                    //    cctor.Body.Instructions.Insert(cctor.Body.Instructions.Count - 1,
                    //        OpCodes.Stsfld.ToInstruction(original_str_value_field));

                    //    Local local_str = new Local(method.Module.CorLibTypes.String);
                    //    method.Body.Variables.Add(local_str);
                    //    Local local_bool = new Local(method.Module.CorLibTypes.Boolean);
                    //    method.Body.Variables.Add(local_bool);

                    //    instr[i].OpCode = OpCodes.Ldloc_S;
                    //    instr[i].Operand = local_str;

                    //    instr.Insert(i - 1, OpCodes.Nop.ToInstruction());
                    //    instr.Insert(i - 1, OpCodes.Ldloc_S.ToInstruction(local_bool));
                    //    instr.Insert(i - 1, OpCodes.Stloc_S.ToInstruction(local_bool));
                    //    instr.Insert(i - 1, Instruction.Create(OpCodes.Call, method.Module.Import(typeof(String).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }))));
                    //    instr.Insert(i - 1, OpCodes.Ldstr.ToInstruction("56s4df65sd4f6s5d4f")); // a differant string from original_str_value_field
                    //    instr.Insert(i - 1, OpCodes.Ldloc_S.ToInstruction(local_str));
                    //    instr.Insert(i - 1, OpCodes.Stloc_S.ToInstruction(local_str));
                    //    instr.Insert(i - 1, OpCodes.Ldsfld.ToInstruction(original_str_value_field));
                    //    instr.Insert(i + 6, OpCodes.Brfalse_S.ToInstruction(instr[i + 11]));
                    //    i += 12;
                    //}
                    //}
                    //instr.RemoveAt(i);
                }
            }
        }

        public void InlineInteger(MethodDef method, int i)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            var instr = method.Body.Instructions;

            if (instr[i - 1].OpCode == OpCodes.Callvirt)
            {
                if (instr[i + 1].OpCode == OpCodes.Call)
                {
                    return;
                }
            }
            if (instr[i + 4].IsBr())
            {
                return;
            }
            bool is_valid_inline = true;
            switch (random.Next(0, 2))
            {
                //true
                case 0:
                is_valid_inline = true;
                break;
                //false
                case 1:
                is_valid_inline = false;
                break;
            }

            Local new_local = new Local(method.Module.CorLibTypes.String);
            method.Body.Variables.Add(new_local);
            Local new_local2 = new Local(method.Module.CorLibTypes.Int32);
            method.Body.Variables.Add(new_local2);
            var value = instr[i].GetLdcI4Value();
            var first_ldstr = RP.GenerateNewName(RP);

            instr.Insert(i, Instruction.Create(OpCodes.Ldloc_S, new_local2));

            instr.Insert(i, Instruction.Create(OpCodes.Stloc_S, new_local2));
            if (is_valid_inline)
            {
                instr.Insert(i, Instruction.Create(OpCodes.Ldc_I4, value));
                instr.Insert(i, Instruction.Create(OpCodes.Ldc_I4, value + 1));
            }
            else
            {
                instr.Insert(i, Instruction.Create(OpCodes.Ldc_I4, value + 1));
                instr.Insert(i, Instruction.Create(OpCodes.Ldc_I4, value));
            }
            instr.Insert(i,
                Instruction.Create(OpCodes.Call,
                    method.Module.Import(typeof(System.String).GetMethod("op_Equality",
                        new Type[] { typeof(string), typeof(string) }))));
            instr.Insert(i, Instruction.Create(OpCodes.Ldstr, first_ldstr));
            instr.Insert(i, Instruction.Create(OpCodes.Ldloc_S, new_local));
            instr.Insert(i, Instruction.Create(OpCodes.Stloc_S, new_local));
            if (is_valid_inline)
            {
                instr.Insert(i, Instruction.Create(OpCodes.Ldstr, first_ldstr));
            }
            else
            {
                instr.Insert(i,
                    Instruction.Create(OpCodes.Ldstr,
                        RP.GenerateNewName(RP)));
            }
            instr.Insert(i + 5, Instruction.Create(OpCodes.Brtrue_S, instr[i + 6]));
            instr.Insert(i + 7, Instruction.Create(OpCodes.Br_S, instr[i + 8]));

            instr.RemoveAt(i + 10);

            if (ProxyMethodConst.Contains(method))
            {
                var last = method.Body.Instructions.Count;
                if (!instr[last - 2].IsLdloc()) return;
                instr[last - 2].OpCode = OpCodes.Ldloc_S;
                instr[last - 2].Operand = new_local2;
            }
        }

        public void CtorCallProtection(MethodDef method)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            var instr = method.Body.Instructions;

            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Call)
                {
                    if (instr[i].Operand.ToString().ToLower().Contains("void"))
                    {
                        if (instr[i - 1].IsLdarg())
                        {
                            Local new_local = new Local(method.Module.CorLibTypes.Int32);
                            method.Body.Variables.Add(new_local);

                            instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                            instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
                            instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
                            instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                            //---------------------------------------------------bne.un.s +3
                            instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
                            //---------------------------------------------------br.s +4
                            instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
                            //---------------------------------------------------br.s +1
                            instr.Insert(i + 6, OpCodes.Nop.ToInstruction());

                            instr.Insert(i + 3, new Instruction(OpCodes.Bne_Un_S, instr[i + 4]));
                            instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
                            instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
                        }
                    }
                }
            }
        }

        public void CallvirtProtection(MethodDef method)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            var instr = method.Body.Instructions;

            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Callvirt)
                {
                    if (instr[i].Operand.ToString().ToLower().Contains("int32"))
                    {
                        if (instr[i - 1].IsLdloc())
                        {
                            Local new_local = new Local(method.Module.CorLibTypes.Int32);
                            method.Body.Variables.Add(new_local);

                            instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                            instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
                            instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
                            instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                            //---------------------------------------------------bne.un.s +3
                            instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
                            //---------------------------------------------------br.s +4
                            instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
                            //---------------------------------------------------br.s +1
                            instr.Insert(i + 6, OpCodes.Nop.ToInstruction());

                            instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
                            instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
                            instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
                        }
                    }
                }
            }
        }

        public void LdfldProtection(MethodDef method)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            var instr = method.Body.Instructions;

            for (int i = 0; i < instr.Count; i++)
            {
                if (instr[i].OpCode == OpCodes.Ldfld)
                {
                    //if (instr[i].Operand.ToString().ToLower().Contains("class"))
                    //{
                    if (instr[i - 1].IsLdarg())
                    {
                        Local new_local = new Local(method.Module.CorLibTypes.Int32);
                        method.Body.Variables.Add(new_local);

                        instr.Insert(i - 1, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                        instr.Insert(i, OpCodes.Stloc_S.ToInstruction(new_local));
                        instr.Insert(i + 1, OpCodes.Ldloc_S.ToInstruction(new_local));
                        instr.Insert(i + 2, OpCodes.Ldc_I4.ToInstruction(random.Next()));
                        //---------------------------------------------------bne.un.s +3
                        instr.Insert(i + 3, OpCodes.Ldarg_0.ToInstruction());
                        //---------------------------------------------------br.s +4
                        instr.Insert(i + 4, OpCodes.Nop.ToInstruction());
                        //---------------------------------------------------br.s +1
                        instr.Insert(i + 6, OpCodes.Nop.ToInstruction());

                        instr.Insert(i + 3, new Instruction(OpCodes.Beq_S, instr[i + 4]));
                        instr.Insert(i + 5, new Instruction(OpCodes.Br_S, instr[i + 8]));
                        instr.Insert(i + 8, new Instruction(OpCodes.Br_S, instr[i + 9]));
                    }
                    //}
                }
            }
        }

        public void ReplaceValue(MethodDef method, int i)
        {
            var instr = method.Body.Instructions;
            if (instr[i].OpCode != OpCodes.Ldc_I4) return;
            var value = instr[i].GetLdcI4Value();
            if (value == 1)
                CollatzConjecture(method, i);
            if (value == 0)
                EmptyTypes(method, i);
        }

        public void CollatzConjecture(MethodDef method, int i)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            var instr = method.Body.Instructions;
            instr[i].Operand = random.Next(1, 15); //the created logic three should be little enough here
            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Call, CollatzCtor));
        }

        public void EmptyTypes(MethodDef method, int i)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            switch (random.Next(0, 2))
            {
                case 0:
                method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Add));
                break;

                case 1:
                method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Sub));
                break;
            }
            method.Body.Instructions.Insert(i + 1,
                Instruction.Create(OpCodes.Ldsfld,
                    method.Module.Import((typeof(Type).GetField("EmptyTypes")))));
            method.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldlen));
        }

        public void OutelineValue(MethodDef method, int i)
        {
            if (method.DeclaringType.IsGlobalModuleType) return;
            if ((!ProxyMethodConst.Contains(method)))
            {
                for (int index = 0; index < method.Body.Instructions.Count; index++)
                {
                    Instruction instr = method.Body.Instructions[index];
                    if (instr.OpCode == OpCodes.Ldc_I4)
                    {
                        MethodDef proxy_method = CreateReturnMethodDef(instr.GetLdcI4Value(), method);
                        method.DeclaringType.Methods.Add(proxy_method);
                        ProxyMethodConst.Add(proxy_method);
                        instr.OpCode = OpCodes.Call;
                        instr.Operand = proxy_method;
                    }
                    else if (instr.OpCode == OpCodes.Ldc_R4)
                    {
                        MethodDef proxy_method = CreateReturnMethodDef(instr, method);
                        method.DeclaringType.Methods.Add(proxy_method);
                        ProxyMethodConst.Add(proxy_method);
                        instr.OpCode = OpCodes.Call;
                        instr.Operand = proxy_method;
                    }
                    else if (instr.Operand is string && instr.OpCode == OpCodes.Ldstr)
                    {
                        MethodDef proxy_method = CreateReturnMethodDef(instr, method);
                        method.DeclaringType.Methods.Add(proxy_method);
                        ProxyMethodConst.Add(proxy_method);
                        instr.OpCode = OpCodes.Call;
                        instr.Operand = proxy_method;
                    }
                }
            }
        }

        public void InitializeCollatz()
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(Runtime.CollatzConjecture).Module);
            MethodDef cctor = _module.GlobalType.FindOrCreateStaticConstructor();
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.CollatzConjecture).MetadataToken));
            IEnumerable<IDnlibDef> members = Inject_Helper.InjectHelper.Inject(typeDef, _module.GlobalType, _module);
            CollatzCtor = (MethodDef)members.Single(method => method.Name == "ConjetMe");
        }

        public MethodDef CreateReturnMethodDef(object constantvalue, MethodDef source_method)
        {
            CorLibTypeSig corlib = null;

            if (constantvalue is int)
            {
                corlib = source_method.Module.CorLibTypes.Int32;
            }
            else
            {
                if (constantvalue is Instruction)
                {
                    var abecede = constantvalue as Instruction;
                    constantvalue = abecede.Operand;
                }
            }
            if (constantvalue is float)
            {
                corlib = source_method.Module.CorLibTypes.Single;
            }
            if (constantvalue is string)
            {
                corlib = source_method.Module.CorLibTypes.String;
            }

            var meth = new MethodDefUser("_" + source_method.Name + "_" + constantvalue.ToString(),
                MethodSig.CreateStatic(corlib),
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig)
            { Body = new CilBody() };

            Local return_value = new Local(corlib);
            meth.Body.Variables.Add(return_value);

            //Method body
            meth.Body.Instructions.Add(OpCodes.Nop.ToInstruction());
            if (constantvalue is int)
            {
                meth.Body.Instructions.Add((int)constantvalue != 0
                    ? Instruction.Create(OpCodes.Ldc_I4, (Int32)constantvalue)
                    : Instruction.Create(OpCodes.Ldc_I4_0));
            }
            if (constantvalue is float)
            {
                meth.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_R4, (Single)constantvalue));
            }
            if (constantvalue is string)
            {
                meth.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, (string)constantvalue));
            }
            meth.Body.Instructions.Add(OpCodes.Stloc_0.ToInstruction());
            var test_ldloc = new Instruction(OpCodes.Ldloc_0);
            meth.Body.Instructions.Add(test_ldloc);
            meth.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            Instruction target = meth.Body.Instructions[3];
            meth.Body.Instructions.Insert(3, Instruction.Create(OpCodes.Br_S, target));
            return meth;
        }

        public bool CanObfuscate(MethodDef method)
        {
            foreach (var lst in ObfuscationProcess.SetObfuscateMethod)
            {
                if (lst.Key.Item1.Name == method.Name)
                    return false;
            }
            return true;
        }
    }
}