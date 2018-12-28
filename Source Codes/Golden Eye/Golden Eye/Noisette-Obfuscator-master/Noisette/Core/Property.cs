using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisetteCore.Core
{
    /// <summary>
    ///     Provide every used fields and other needed ojects
    /// </summary>
    internal class Property
    {
        /// <summary>
        ///     The modulewriter used when saving / writting modified assembly
        /// </summary>
        public static ModuleWriterOptions opts { get; set; }

        /// <summary>
        /// Directory path where the file is in
        /// </summary>
        public static string DirectoryName { get; set; }

        /// <summary>
        /// A list which will contains <see cref="MethodDef"/> who may contains
        /// <see cref="System.Reflection"/> references
        /// </summary>
        public static List<MethodDef> ContainsReflectionReference = new List<MethodDef>();

        /// <summary>
        /// A list which will contains <see cref="MethodDef"/> who may containns
        /// <see cref="System.Windows.Forms"/> references
        /// </summary>
        public static List<MethodDef> ContainsWinformReference = new List<MethodDef>();
    }
}