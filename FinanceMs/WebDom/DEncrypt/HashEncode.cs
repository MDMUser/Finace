using System;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;
namespace WebDom.DEncrypt
{
	/// <summary>
	/// 得到随机安全码（哈希加密）。
	/// </summary>
	public class HashEncode
	{
        const int kRandomBytesCount = 32;
        const int kPBKDF2BytesCount = 32;

		public HashEncode()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		/// <summary>
		/// 得到随机哈希加密字符串
		/// </summary>
		/// <returns></returns>
		public static string GetSecurity()
		{			
			string Security = HashEncoding(GetRandomValue());		
			return Security;
		}
		/// <summary>
		/// 得到一个随机数值
		/// </summary>
		/// <returns></returns>
		public static string GetRandomValue()
		{			
			Random Seed = new Random();
			string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
			return RandomVaule;
		}

        /// <summary>
        /// 得到一个 64 字节的随机字符串
        /// </summary>
        /// <returns></returns>
        /// <remarks>使用 CSPRNG 生成</remarks>
        public static string GetRandomString64()
        {
            byte[] random = new Byte[kRandomBytesCount];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(random);
            rng.Dispose();

            return GetHexString(random);
        }

        /// <summary>
        /// 生成一个字符串的 SHA256 hash
        /// </summary>
        /// <param name="toBeHashed"></param>
        /// <returns>十六进制的 hash 字符串</returns>
        public static string Sha256(string toBeHashed)
        {
            SHA256 mySHA256 = SHA256Managed.Create();   
            byte[] hashValue = mySHA256.ComputeHash(GetBytesFromString(toBeHashed));
            return GetHexString(hashValue);
        }

        /// <summary>
        /// 使用 PBKDF2 算法加密一个字符串
        /// </summary>
        /// <param name="toBeEncrypted">需要加密的字符串</param>
        /// <param name="salt">盐</param>
        /// <returns>长度为 64 字节的十六进制加密字符串</returns>
        public static string PBKDF2(string toBeEncrypted, string salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(toBeEncrypted, GetBytesFromHexString(salt));
            return GetHexString(pbkdf2.GetBytes(kPBKDF2BytesCount));
        }

		/// <summary>
		/// 哈希加密一个字符串
		/// </summary>
		/// <param name="Security"></param>
		/// <returns></returns>
		public static string HashEncoding(string Security)
		{						
			byte[] Value;
			UnicodeEncoding Code = new UnicodeEncoding();
			byte[] Message = Code.GetBytes(Security);
			SHA512Managed Arithmetic = new SHA512Managed();
			Value = Arithmetic.ComputeHash(Message);
			Security = "";
			foreach(byte o in Value)
			{
				Security += (int) o + "O";
			}
			return Security;
		}

        /// <summary>
        /// 将字符串转化为 byte 数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] GetBytesFromString(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// 将十六进制的字符串转化为 byte 数组
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        private static byte[] GetBytesFromHexString(string hexStr)
        {
            Debug.Assert(hexStr != null);
            Debug.Assert(hexStr.Length > 0);

            return Enumerable.Range(0, hexStr.Length)
                 .Where(x => x % 2 == 0)
                 .Select(x => Convert.ToByte(hexStr.Substring(x, 2), 16))
                 .ToArray();
        }

        /// <summary>
        /// 将 byte 数组转换为十六进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string GetHexString(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString("x2"));

            return result.ToString();
        }
	}
}
