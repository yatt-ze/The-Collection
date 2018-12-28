using dnlib.DotNet;
using NoisetteCore.Obfuscation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noisette.Obfuscation
{
    internal class ObfuscationAnalyzer
    {
        public ModuleDefMD _module;

        public ObfuscationAnalyzer(ModuleDefMD module)
        {
            _module = module;
        }

        public void PerformAnalyze()
        {
            foreach (TypeDef type in _module.Types)
            {
                ExplodeMember(type);
            }
        }

        private void ExplodeMember(TypeDef type)
        {
            if (!type.IsGlobalModuleType)
                ExplodeMember(type.Methods);
        }

        private void ExplodeMember(IList<MethodDef> method_list)
        {
            var mtlist = method_list;
            foreach (MethodDef method in mtlist)
            {
                if (!CanObfuscate(method))
                    ObfuscationProcess.SetObfuscateMethod.Add(Tuple.Create(method), false);
            }
        }

        public bool CanObfuscate(MethodDef item)
        {
            if (!item.HasBody)
                return false;
            if (!item.Body.HasInstructions)
                return false;
            if (item.FullName.Contains(".My")) //VB.NET gives Cancer and Ebola. Twice.
                return false;
            if (ContainsReflection(item))
                return false;
            if (item.DeclaringType.IsGlobalModuleType)
                return false;
            if (item.FullName.Contains("<Module>"))
                return false;
            return true;
        }

        /// <summary>
        /// Check for <see cref="System.Reflection"/> references in the file
        /// </summary>
        /// <param name="module">the file to protect</param>
        private bool ContainsReflection(MethodDef method)
        {
            var mbody = method.Body.Instructions;
            foreach (var instruction in mbody)
            {
                if (instruction.Operand != null && instruction.Operand.ToString().ToLower().Contains("reflection"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}