using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace JDownloaderService
{
	class Decrypter
	{

        public static string DecryptLinks(String key, String data)
        {
            // decode key
            key = key.ToUpper();

            String decKey = "";
            for (int i = 0; i < key.Length; i += 2)
            {
                decKey += (char)Convert.ToUInt16(key.Substring(i, 2), 16);
            }

            // decode data
            byte[] dataByte = Convert.FromBase64String(data);

            // decrypt that shit!
            RijndaelManaged rDel = new RijndaelManaged();
            ASCIIEncoding aEc = new ASCIIEncoding();

            rDel.Key = aEc.GetBytes(decKey);
            rDel.IV = aEc.GetBytes(decKey);
            rDel.Mode = CipherMode.CBC;

            rDel.Padding = PaddingMode.None;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(dataByte, 0, dataByte.Length);

            String rawLinks = aEc.GetString(resultArray);

            // replace empty paddings
            Regex rgx = new Regex("\u0000+$");
            String cleanLinks = rgx.Replace(rawLinks, "");

            // replace newlines
            rgx = new Regex("\n+");
            cleanLinks = rgx.Replace(cleanLinks, "\r\n");

            return cleanLinks;
        }

    }
}
