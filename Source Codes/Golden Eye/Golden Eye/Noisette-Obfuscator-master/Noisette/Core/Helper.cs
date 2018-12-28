using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoisetteCore.Core
{
    internal class Helper
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
    }
}