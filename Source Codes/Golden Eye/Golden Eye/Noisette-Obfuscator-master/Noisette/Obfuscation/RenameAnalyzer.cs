using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisetteCore.Obfuscation
{
    internal class RenameAnalyzer
    {
        private ModuleDefMD _module;

        public NoisetteCore.Protection.Renaming.RenamingProtection RP;

        public RenameAnalyzer(ModuleDefMD module)
        {
            RP = Obfuscation.ObfuscationProcess.RP;
            _module = module;
        }

        public void PerformAnalyze()
        {
            ExplodeMember(_module);
        }

        private void ExplodeMember(ModuleDefMD item)
        {
            foreach (TypeDef type in item.Types)
            {
                ExplodeMember(type);
            }
        }

        private void ExplodeMember(TypeDef item)
        {
            if (CanRename(item))
            {
                item.Name = RP.GenerateNewName(RP);
            }
            ExplodeMember(item.Methods);
            ExplodeMember(item.Properties);
            ExplodeMember(item.Fields);
        }

        private void ExplodeMember(IList<MethodDef> item_list)
        {
            foreach (var item in item_list)
            {
                if (CanRename(item))
                {
                    item.Name = RP.GenerateNewName(RP);
                }
                ExplodeMember(item.Parameters);
            }
        }

        private void ExplodeMember(ParameterList item_list)
        {
            foreach (var item in item_list.Where(CanRename))
            {
                item.Name = RP.GenerateNewName(RP); ;
            }
        }

        private void ExplodeMember(IList<PropertyDef> item_list)
        {
            foreach (var item in item_list.Where(CanRename))
            {
                item.Name = RP.GenerateNewName(RP); ;
            }
        }

        private void ExplodeMember(IList<FieldDef> item_list)
        {
            foreach (var item in item_list.Where(CanRename))
            {
                item.Name = RP.GenerateNewName(RP); ;
            }
        }

        public bool CanRename(object memberToRename)
        {
            var rename = true;
            if (memberToRename is MethodDef)
                rename = AnalyzeMember((MethodDef)memberToRename);
            if (memberToRename is TypeDef)
                rename = AnalyzeMember((TypeDef)memberToRename);
            if (memberToRename is FieldDef)
                rename = AnalyzeMember((FieldDef)memberToRename);
            if (memberToRename is EventDef)
                rename = AnalyzeMember((EventDef)memberToRename);
            return rename;
        }

        public bool AnalyzeMember(TypeDef type)
        {
            if (type.IsRuntimeSpecialName)
                return false;
            if (type.IsGlobalModuleType)
                return false;
            return true;
        }

        public bool AnalyzeMember(MethodDef method)
        {
            if (method.IsRuntimeSpecialName)
                return false;
            if (method.IsConstructor || method.IsStaticConstructor)
                return false;
            if (method.DeclaringType.IsForwarder)
                return false;
            return true;
        }

        public bool AnalyzeMember(FieldDef field)
        {
            if (field.IsRuntimeSpecialName)
                return false;
            if (field.IsLiteral && field.DeclaringType.IsEnum)
                return false;
            return true;
        }

        public bool AnalyzeMember(EventDef ev)
        {
            if (ev.IsRuntimeSpecialName)
                return false;
            return true;
        }
    }
}