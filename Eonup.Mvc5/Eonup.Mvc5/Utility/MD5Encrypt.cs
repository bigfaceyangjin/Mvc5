using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Eonup.Mvc5.Utility
{
	/// <summary>
	/// 不可逆加密 限于字母和数字
	/// </summary>
	public class MD5Encrypt
	{
		/// <summary>
		/// MD5加密
		/// </summary>
		/// <param name="source">待加密字符</param>
		/// <param name="length">加密位</param>
		/// <returns>加密后的字符</returns>
		public static string Encrypt(string source,int length=32)
		{
			//创建加密实例 HashAlgorithm(哈希加密算法基类)
			HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;
			if (string.IsNullOrEmpty(source))
			{
				return string.Empty;
			}
			//将原文编码为一个指定的字节序列
			byte[] bytes= Encoding.ASCII.GetBytes(source);
			//计算原文序列的哈希值
			byte[] hashvalue= provider.ComputeHash(bytes);
			StringBuilder sb = new StringBuilder();
			switch (length)
			{
				case 16://16位密文是32位密文的9到24位
					for (int i = 4; i < 12; i++)
					{
						sb.Append(hashvalue[i].ToString("x2"));//转换为小写的16进制 每次输出两位
					}
					break;
				case 32:
					for (int i = 0; i < 16; i++)
					{
						sb.Append(hashvalue[i].ToString("x2"));
					}
					break;
				default:
					for (int i = 0; i < length; i++)
					{
						sb.Append(hashvalue[i].ToString("x2"));
					}
					break;
			}
			return sb.ToString();
		}
	}
}