using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Noisette.Obfuscation;
using System;
using System.Collections.Generic;
using System.IO;

namespace NoisetteCore.Obfuscation
{
    public class ObfuscationProcess
    {
        public ModuleDefMD _module;

        public static Dictionary<Tuple<MethodDef>, bool> SetObfuscateMethod;

        public static NoisetteCore.Helper.ConsoleHelper CH;

        public static Protection.Renaming.RenamingProtection RP;

        // Core.Property.module =
        public ObfuscationProcess(ModuleDefMD module)
        {
            _module = module;
            CH = new NoisetteCore.Helper.ConsoleHelper();
            CH.begin = DateTime.Now;
            CH.Debug("Instancing Tuples and ModuleWriters");
            SetObfuscateMethod = new Dictionary<Tuple<MethodDef>, bool>();
            Core.Property.opts = new ModuleWriterOptions(_module);
            CH.Debug("Analyzing module...");
            ObfuscationAnalyzer AZ = new ObfuscationAnalyzer(_module);
            AZ.PerformAnalyze();
        }

        public void DoObfusction()
        {
            CH.Debug("Starting the Obfusction routine");
            RP = new Protection.Renaming.RenamingProtection(_module);

            //CH.Debug("Obfuscating Constants ...");
            ////Constants
            //NoisetteCore.Protection.Constant.ConstantProtection CP =
            //    new NoisetteCore.Protection.Constant.ConstantProtection(_module);
            //CP.DoProcess();

            //Renaming
            CH.Debug("Processing Renaming ...");
            RP.RenameModule();
            ////Inject Antitamper class
            //Protection.AntiTampering.AntiTamperingProtection ATP = new Protection.AntiTampering.AntiTamperingProtection(_module);
            //CH.Debug("Processing AntiTamper ...");
            //ATP.Process();

            //Melt Constant
            Protection.ConstantMelting.ConstantMeltingProtection.MeltConstant(_module);

            //invalid metadata
            //Protection.InvalidMetadata.InvalidMD.InsertInvalidMetadata(module);
            //todo : something is wrong

            CH.Debug("Saving File");

            SaveAssembly();

            //post-stage antitamper
            //todo : make a proper post-process class

            //CH.Info("Injecting AntiTamper value");
            //Protection.AntiTampering.AntiTamperingProtection.Md5(Path.GetDirectoryName(_module.Location) + @"\" + Path.GetFileNameWithoutExtension(_module.Location) + "_nutsed.exe");
        }

        public void SaveAssembly()
        {
            Core.Property.opts.Logger = DummyLogger.NoThrowInstance;
            _module.Write(Path.GetDirectoryName(_module.Location) + @"\" + Path.GetFileNameWithoutExtension(_module.Location) + "_nutsed.exe", Core.Property.opts);
            CH.Debug("File Saved ! ");
            CH.Finish();
        }
    }
}