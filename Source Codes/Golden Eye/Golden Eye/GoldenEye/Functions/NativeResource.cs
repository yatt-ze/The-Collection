namespace GoldenEye.Functions
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class NativeResource
    {
        #region " Writing "

        public static void WriteResource(string filename, string name, byte[] data)
        {
            IntPtr handle = OpenFile(filename);
            IntPtr dataPtr = CopyToNative(data);
            if (!UpdateResource(handle, "RT_RCDATA", name, 0, dataPtr, Convert.ToUInt32(data.Length)))
            {
                FreeAlloc(dataPtr);
                throw new AccessViolationException("Failed to update resource");
            }

            if (!EndUpdateResource(handle, false))
            {
                FreeAlloc(dataPtr);
                throw new AccessViolationException("Failed to write resource");
            }
        }

        public static void WriteResource(string filename, string name, string data)
        {
            IntPtr handle = OpenFile(filename);
            StringBuilder sb = new StringBuilder(data);
            if (!UpdateResource(handle, "RT_STRING", name, 0, sb, Convert.ToUInt32(data.Length)))
                throw new AccessViolationException("Failed to update resource");
            if (!EndUpdateResource(handle, false))
                throw new AccessViolationException("Failed to write resource");
        }

        public static void ClearResources(string filename)
        {
            IntPtr handle = OpenFile(filename);
            if (!EndUpdateResource(handle, true))
                throw new AccessViolationException("Failed end update");
        }

        #endregion " Writing "

        #region " Reading "

        private static byte[] ReadFromPointer(IntPtr mHandle, string resourceName, string type)
        {
            IntPtr rHandle = FindResource(mHandle, resourceName, type);
            IntPtr dHandle = LoadResource(mHandle, rHandle);
            uint size = SizeofResource(mHandle, rHandle);
            return CopyToManaged(dHandle, size);
        }

        public static byte[] ReadResource(string resourceName)
        {
            IntPtr mHandle = CurrentModuleHandle();
            return ReadFromPointer(mHandle, resourceName, "RT_RCDATA");
        }

        public static string ReadResourceString(string resourceName)
        {
            IntPtr mHandle = CurrentModuleHandle();
            byte[] data = ReadFromPointer(mHandle, resourceName, "RT_STRING");
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] ReadResource(string filename, string resourcename)
        {
            IntPtr fHandle = OpenFile(filename);
            return ReadFromPointer(fHandle, resourcename, "RT_RCDATA");
        }

        public static string ReadResourceString(string filename, string resourcename)
        {
            IntPtr fHandle = OpenFile(filename);
            byte[] data = ReadFromPointer(fHandle, resourcename, "RT_STRING");
            return Encoding.UTF8.GetString(data);
        }

        #endregion " Reading "

        #region " Healpers "

        private static IntPtr GetResourceHandle(IntPtr mHandle, string rname, string type)
        {
            IntPtr rHandle = FindResource(mHandle, rname, type);
            if (rHandle == IntPtr.Zero)
                throw new InvalidOperationException("Cant find resource");
            return rHandle;
        }

        private static IntPtr CurrentModuleHandle()
        {
            IntPtr handle = GetModuleHandle(null);
            if (handle == IntPtr.Zero)
                throw new AccessViolationException("Failed to get current module handle");
            return handle;
        }

        private static IntPtr OpenFile(string filename)
        {
            IntPtr handle = BeginUpdateResource(filename, false);

            if (handle == IntPtr.Zero)
                throw new AccessViolationException("Failed to open file");
            return handle;
        }

        private static IntPtr CopyToNative(byte[] data)
        {
            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length);
            if (dataPtr == IntPtr.Zero)
                throw new AccessViolationException("Failed to open resource handle");
            Marshal.Copy(data, 0, dataPtr, data.Length);
            return dataPtr;
        }

        private static byte[] CopyToManaged(IntPtr ptr, uint size)
        {
            if (ptr == IntPtr.Zero)
                throw new ArgumentException("Pointer cant be zero");
            byte[] b = new byte[size];
            Marshal.Copy(ptr, b, 0, b.Length);
            return b;
        }

        private static void FreeAlloc(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
                Marshal.FreeHGlobal(ptr);
        }

        #endregion " Healpers "

        #region " WinApi "

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string module);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr FindResource(IntPtr hModule, string lpName, string lpType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr BeginUpdateResource(string pFileName,
           [MarshalAs(UnmanagedType.Bool)]bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool UpdateResource(IntPtr hUpdate, string lpType, string lpName, ushort wLanguage, IntPtr lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool UpdateResource(IntPtr hUpdate, string lpType, string lpName, ushort wLanguage, StringBuilder lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        #endregion " WinApi "
    }
}