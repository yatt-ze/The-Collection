using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Noisette.Core
{
    class Helper
    {
        /// <summary>
        /// A simple Boolean function to check if choosen method
        /// can be subject to outlinning or not
        /// </summary>
        /// <param name="method"><see cref="MethodDef"/></param>
        /// <returns></returns>
        public static bool IsValidMethod(MethodDef method)
        {
            return method.HasBody && (method.Body.HasInstructions && (!method.IsNative && !method.IsPinvokeImpl));
        }


        /// <summary>
        /// Create a <see cref="MethodDef"/> which will return the Constant
        ///  </summary>
        /// <param name="constantvalue">The constant value, can be any ldci or ldcr8</param>
        /// <param name="source_method">the method where the constant value comes from</param>
        /// <returns></returns>
        public static MethodDef CreateReturnMethodDef(object constantvalue, MethodDef source_method)
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

            var meth = new MethodDefUser("_" + source_method.Name + "_" + constantvalue.ToString(), MethodSig.CreateStatic(corlib),
                       MethodImplAttributes.IL | MethodImplAttributes.Managed,
MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);

            meth.Body = new CilBody();
            Local return_value = new Local(corlib);
            meth.Body.Variables.Add(return_value);

            //Method body 
            meth.Body.Instructions.Add(OpCodes.Nop.ToInstruction());
            if (constantvalue is int)
            {
                if ((int) constantvalue != 0)
                {
                    meth.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, (Int32) constantvalue));
                }
                else
                {
                    meth.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
                }
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


    }
}
