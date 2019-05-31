#pragma once
#include <fstream>
class Config
{
	public:
		std::string pastebinUrl = "ITS A PASTEBIN RAT HAHA - Enter URL";
		//std::string BitcoinAddress = "123 - or longer";
		//std::string EthereumAddress = "123 - abc";
		std::string useragent = "somesecret - you're paying for shit";
		bool startup = false;
		char key[3] = { 'K', 'C', 'Q' }; //Any chars will work
};

