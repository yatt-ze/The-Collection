using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

public class GeoIP
{
    public MemoryStream MS;
    private string res = Application.StartupPath + @"\Data\GeoIP.dat";
    private static long CountryBegin = 16776960;
    private static string[] CountryName = new[] { "Unknown", "Asia/Pacific Region", "Europe", "Andorra", "United Arab Emirates", "Afghanistan", "Antigua and Barbuda", "Anguilla", "Albania", "Armenia", "Netherlands Antilles", "Angola", "Antarctica", "Argentina", "American Samoa", "Austria", "Australia", "Aruba", "Azerbaijan", "Bosnia and Herzegovina", "Barbados", "Bangladesh", "Belgium", "Burkina Faso", "Bulgaria", "Bahrain", "Burundi", "Benin", "Bermuda", "Brunei Darussalam", "Bolivia", "Brazil", "Bahamas", "Bhutan", "Bouvet Island", "Botswana", "Belarus", "Belize", "Canada", "Cocos (Keeling) Islands", "Congo, The Democratic Republic of the", "Central African Republic", "Congo", "Switzerland", "Cote D'Ivoire", "Cook Islands", "Chile", "Cameroon", "China", "Colombia", "Costa Rica", "Cuba", "Cape Verde", "Christmas Island", "Cyprus", "Czech Republic", "Germany", "Djibouti", "Denmark", "Dominica", "Dominican Republic", "Algeria", "Ecuador", "Estonia", "Egypt", "Western Sahara", "Eritrea", "Spain", "Ethiopia", "Finland", "Fiji", "Falkland Islands (Malvinas)", "Micronesia, Federated States of", "Faroe Islands", "France", "France, Metropolitan", "Gabon", "United Kingdom", "Grenada", "Georgia", "French Guiana", "Ghana", "Gibraltar", "Greenland", "Gambia", "Guinea", "Guadeloupe", "Equatorial Guinea", "Greece", "South Georgia and the South Sandwich Islands", "Guatemala", "Guam", "Guinea-Bissau", "Guyana", "Hong Kong", "Heard Island and McDonald Islands", "Honduras", "Croatia", "Haiti", "Hungary", "Indonesia", "Ireland", "Israel", "India", "British Indian Ocean Territory", "Iraq", "Iran, Islamic Republic of", "Iceland", "Italy", "Jamaica", "Jordan", "Japan", "Kenya", "Kyrgyzstan", "Cambodia", "Kiribati", "Comoros", "Saint Kitts and Nevis", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait", "Cayman Islands", "Kazakstan", "Lao People's Democratic Republic", "Lebanon", "Saint Lucia", "Liechtenstein", "Sri Lanka", "Liberia", "Lesotho", "Lithuania", "Luxembourg", "Latvia", "Libyan Arab Jamahiriya", "Morocco", "Monaco", "Moldova, Republic of", "Madagascar", "Marshall Islands", "Macedonia, the Former Yugoslav Republic of", "Mali", "Myanmar", "Mongolia", "Macao", "Northern Mariana Islands", "Martinique", "Mauritania", "Montserrat", "Malta", "Mauritius", "Maldives", "Malawi", "Mexico", "Malaysia", "Mozambique", "Namibia", "New Caledonia", "Niger", "Norfolk Island", "Nigeria", "Nicaragua", "Netherlands", "Norway", "Nepal", "Nauru", "Niue", "New Zealand", "Oman", "Panama", "Peru", "French Polynesia", "Papua New Guinea", "Philippines", "Pakistan", "Poland", "Saint Pierre and Miquelon", "Pitcairn", "Puerto Rico", "Palestinian Territory, Occupied", "Portugal", "Palau", "Paraguay", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saudi Arabia", "Solomon Islands", "Seychelles", "Sudan", "Sweden", "Singapore", "Saint Helena", "Slovenia", "Svalbard and Jan Mayen", "Slovakia", "Sierra Leone", "San Marino", "Senegal", "Somalia", "Suriname", "Sao Tome and Principe", "El Salvador", "Syrian Arab Republic", "Swaziland", "Turks and Caicos Islands", "Chad", "French Southern Territories", "Togo", "Thailand", "Tajikistan", "Tokelau", "Turkmenistan", "Tunisia", "Tonga", "Timor-Leste", "Turkey", "Trinidad and Tobago", "Tuvalu", "Taiwan, Province of China", "Tanzania, United Republic of", "Ukraine", "Uganda", "United States Minor Outlying Islands", "United States", "Uruguay", "Uzbekistan", "Holy See (Vatican City State)", "Saint Vincent and the Grenadines", "Venezuela", "Virgin Islands, British", "Virgin Islands, U.S.", "Vietnam", "Vanuatu", "Wallis and Futuna", "Samoa", "Yemen", "Mayotte", "Yugoslavia", "South Africa", "Zambia", "Montenegro", "Zimbabwe", "Anonymous Proxy", "Satellite Provider", "Other", "Aland Islands", "Guernsey", "Isle of Man", "Jersey", "Saint Barthelemy", "Saint Martin" };
    private static string[] CountryCode = new[] { "--", "AP", "EU", "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AZ", "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BM", "BN", "BO", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CR", "CU", "CV", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EE", "EG", "EH", "ER", "ES", "ET", "FI", "FJ", "FK", "FM", "FO", "FR", "FX", "GA", "GB", "GD", "GE", "GF", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT", "HU", "ID", "IE", "IL", "IN", "IO", "IQ", "IR", "IS", "IT", "JM", "JO", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "ST", "SV", "SY", "SZ", "TC", "TD", "TF", "TG", "TH", "TJ", "TK", "TM", "TN", "TO", "TL", "TR", "TT", "TV", "TW", "TZ", "UA", "UG", "UM", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "YE", "YT", "SAU", "RS", "ZA", "ZM", "ME", "ZW", "A1", "A2", "O1", "AX", "GG", "IM", "JE", "BL", "MF" };
    private static string[] CountryNump = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "100", "101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "111", "112", "113", "114", "115", "116", "117", "118", "119", "120", "121", "122", "123", "124", "125", "126", "127", "128", "129", "130", "131", "132", "133", "134", "135", "136", "137", "138", "139", "140", "141", "142", "143", "144", "145", "146", "147", "148", "149", "150", "151", "152", "153", "154", "155", "156", "157", "158", "159", "160", "161", "162", "163", "164", "165", "166", "167", "168", "169", "170", "171", "172", "173", "174", "175", "176", "177", "178", "179", "180", "181", "182", "183", "184", "185", "186", "187", "188", "189", "190", "191", "192", "193", "194", "195", "196", "197", "198", "199", "200", "201", "202", "203", "204", "205", "206", "207", "208", "209", "210", "211", "212", "213", "214", "215", "216", "217", "218", "219", "220", "221", "222", "223", "224", "225", "226", "227", "228", "229", "230", "231", "232", "233", "234", "235", "236", "237", "238", "239", "240", "241", "242", "243", "244", "245", "246", "247", "248", "249", "250", "251", "252", "253", "254" };

    public GeoIP(MemoryStream mss)
    {
        MS = mss;
    }

    public GeoIP()
    {
        FileStream FS = new FileStream(res, FileMode.Open, FileAccess.Read);
        MS = new MemoryStream();
        byte[] BY = new byte[257];
        while (FS.Read(BY, 0, BY.Length) != 0)
            MS.Write(BY, 0, BY.Length);
        FS.Close();
    }

    public new string LookupCountryNump(string IPA)
    {
        IPAddress ADR;
        try
        {
            ADR = IPAddress.Parse(IPA);
        }
        catch (FormatException ex)
        {
            return "--";
        }
        return LookupCountryNump(ADR + "");
    }

    public new string LookupCountryCode(string IPA)
    {
        IPAddress ADR;
        try
        {
            ADR = IPAddress.Parse(IPA);
        }
        catch (FormatException ex)
        {
            return "--";
        }
        return LookupCountryCodeA(ADR);
    }

    public new string LookupCountryName(string IPA)
    {
        IPAddress ADR;
        try
        {
            ADR = IPAddress.Parse(IPA);
        }
        catch (FormatException e)
        {
            return "N/A";
        }
        return LookupCountryNameA(ADR);
    }

    public new string LookupCountryCodeA(IPAddress IPA)
    {
        return CountryCode[System.Convert.ToInt32(SeekCountry(0, ConvertIPAddressToNumber(IPA), 31))];
    }

    public new string LookupCountryNameA(IPAddress addr)
    {
        return CountryName[System.Convert.ToInt32(SeekCountry(0, ConvertIPAddressToNumber(addr), 31))];
    }

    private long SeekCountry(long StartOffset, long IPNumber, int SearchDepth)
    {
        byte[] BUF_bytes = new byte[7];
        long[] x_long = new long[3];
        if (SearchDepth == 0)
        {
        }
        try
        {
            MS.Seek(6 * StartOffset, 0);
            MS.Read(BUF_bytes, 0, 6);
        }
        catch (IOException ex)
        {
        }
        int ITR;
        for (ITR = 0; ITR <= 1; ITR++)
        {
            x_long[ITR] = 0;
            int j;
            for (j = 0; j <= 2; j++)
            {
                int PRT = BUF_bytes[(ITR * 3 + j)];
                if (PRT < 0)
                    PRT += 256;
                x_long[ITR] += vbShiftLeft(PRT, (j * 8));
            }
        }
        if ((IPNumber & vbShiftLeft(1, SearchDepth)) > 0)
        {
            if (x_long[1] >= CountryBegin)
                return x_long[1] - CountryBegin;
            return SeekCountry(x_long[1], IPNumber, SearchDepth - 1);
        }
        else
        {
            if (x_long[0] >= CountryBegin)
                return x_long[0] - CountryBegin;
            return SeekCountry(x_long[0], IPNumber, SearchDepth - 1);
        }
    }

    private long vbShiftLeft(long Value, int Count)
    {
        int ITR;
        long xxxx;
        xxxx = Value;
        for (ITR = 1; ITR <= Count; ITR++)
            xxxx = xxxx * 2;

        return xxxx;
    }

    private long vbShiftRight(long Value, int Count)
    {
        int ITR;
        long xyz;
        xyz = Value;
        for (ITR = 1; ITR <= Count; ITR++)
            xyz = xyz / 2;

        return xyz;
    }

    private long ConvertIPAddressToNumber(IPAddress IPA)
    {
        try
        {
            string ipar = IPA.ToString();
            string[] ADR = ipar.Split('.');

            int[] values = new int[4];
            int i = 0;

            foreach (string s in ADR)
            {
                values[i] = int.Parse(s);
                i++;
            }
            return System.Convert.ToInt64(16777216 * values[0] + 65536 * values[1] + 256 * values[2] + values[3]);
        }
        catch
        {
            return 0;
        }
    }
}