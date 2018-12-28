using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Noisette.Core
{
    class Type
    {

        


        /// <summary>
        /// The Type we use when we add our data to Lists
        /// </summary>
        internal class ReturnConstMethod
        {
            private object instr;
            private MethodDef method;
            public ReturnConstMethod(MethodDef meth, object inst)
            {
                this.instr = inst;
                this.method = meth;
            }

            public MethodDef Method
            {
                get { return method; }
                set { method = value; }
            }

            public object Instr
            {
                get { return instr; }
                set { instr = value; }
            }
        }
    }
}
