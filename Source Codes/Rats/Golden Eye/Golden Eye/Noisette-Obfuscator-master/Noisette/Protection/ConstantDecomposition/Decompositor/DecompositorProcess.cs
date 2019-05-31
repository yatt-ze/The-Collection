using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NoisetteCore.Protection.ConstantDecomposition.Decompositor
{
    internal class DecompositorProcess
    {
        public static string[] test_list = new string[] { };

        public DecompositorProcess()
        {
            List<string> list = new List<string>();
            list.Add("1");
            list.Add("2");
            test_list = list.ToArray();
            tok_ = "123";
        }

        public static string tok_;

        public void test()
        {
            string test = tok_;
            //var new_wstring = Assembly.GetExecutingAssembly().ManifestModule.ResolveField(0x0600003E).GetValue(typeof(string));
        }
    }
}