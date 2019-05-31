using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoisetteCLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Noisette Obfuscator - The nuts-breaker obfuscator for .NET file.");
            Console.WriteLine("Copyright (c) 2016 - XenocodeRCE");
            Console.WriteLine("Latest version at https://github.com/XenocodeRCE/Noisette-Obfuscator");
            Console.WriteLine();
            try
            {
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("No input file");
                    return;
                }
                else
                {
                    var filename = args[0];
                    Console.WriteLine("[" + DateTime.Now + "] Reading file " + Path.GetFileNameWithoutExtension(filename));
                    NoisetteCore.Obfuscation.ObfuscationProcess obf =
                        new NoisetteCore.Obfuscation.ObfuscationProcess(ModuleDefMD.Load(filename));
                    obf.DoObfusction();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] " + ex.ToString());
            }

            Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}