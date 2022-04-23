using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Asha.Util
{
    public class IdUtil
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];
        internal static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-=".ToCharArray();


        public static string GetId()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetUniqueKey()
        {
            return GetId32();
        }

        public static string GetId32()
        {
            string toFill = "00000000000000000000000000000000";
            string radom = Convert.ToString(Next(1, 1000000 - 1)) + Convert.ToString(Next(1, 1000000000 - 1));
            string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string id = time + toFill.Substring(0, toFill.Length - time.Length - radom.Length) + radom;
            return id;
        }

        public static string GetId12()
        {
            string toFill = "000000000000";
            string radom = Convert.ToString(Next(1, 1000 - 1));
            string time = DateTime.Now.ToString("yyyyMMdd");
            string id = time + toFill.Substring(0, toFill.Length - time.Length - radom.Length) + radom;
            return id;
        }

        public static string GetId16()
        {
            string toFill = "0000000000000000";
            string radom = Convert.ToString(Next(1, 10000000 - 1));
            string time = DateTime.Now.ToString("yyyyMMdd");
            string id = time + toFill.Substring(0, toFill.Length - time.Length - radom.Length) + radom;
            return id;
        }

        /// <summary>
        /// 產生一個非負數且最大值 max (含)以下的亂數
        /// </summary>
        /// <param name="max">最大值</param>
        public static int Next(int max)
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0)
            {
                value = -value;
            }
            return value;
        }

        /// <summary>
        /// 產生一個非負數且最小值在 min 以上最大值在 max (含)以下的亂數
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }


        public static string GetRandomCode(int Number)
        {
            string uid = Guid.NewGuid().ToString();
            return uid.Substring(uid.Length - 8);
        }
    }
}
