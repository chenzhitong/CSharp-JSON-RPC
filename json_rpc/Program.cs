using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace json_rpc
{
    class Program
    {
        /// <summary>
        /// API参考：https://github.com/AntShares/AntShares/wiki/API%E5%8F%82%E8%80%83
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var r = PostWebRequest("http://seed2.antshares.org:10332", "{'jsonrpc': '2.0', 'method': 'getblockcount', 'params': [],  'id': 1}");
            Console.WriteLine(ToGB2312(r));
            Console.ReadLine();

            r = PostWebRequest("http://seed2.antshares.org:10332", "{'jsonrpc': '2.0', 'method': 'getbestblockhash', 'params': [],  'id': 2}");
            Console.WriteLine(ToGB2312(r));
            Console.ReadLine();

            r = PostWebRequest("http://seed2.antshares.org:10332", "{'jsonrpc': '2.0', 'method': 'getrawtransaction', 'params': ['c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b', 1],  'id': 3}");
            Console.WriteLine(ToGB2312(r));
            Console.ReadLine();
        }

        public static string PostWebRequest(string postUrl, string paramData)
        {
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData);
                WebRequest webReq = WebRequest.Create(postUrl);
                webReq.Method = "POST";
                using (Stream newStream = webReq.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);
                }
                using (WebResponse response = webReq.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public static string ToGB2312(string str)
        {
            MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                str = str.Replace(m.ToString(), Encoding.Unicode.GetString(bts));
            }
            return str;
        }
    }
}
