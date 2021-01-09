using Library.WeChat.Application;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Senparc.CO2NET.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Library.WeChat.Extension
{
    /// <summary>
    /// 安全措施帮助类
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options">微信服务配置</param>
        public SecurityHelper(WeChatServiceOptions options)
        {
            Options = options;

            if (!string.IsNullOrWhiteSpace(Options.CertFilePath))
            {
                PrivateCert = new X509Certificate2(Options.CertFilePath, Options.CertPassword, X509KeyStorageFlags.Exportable);

                PrivateKey = (RSACryptoServiceProvider)PrivateCert.PrivateKey;

                //需要重新导入参数
                var parameters = PrivateKey.ExportParameters(true);

                PrivateKey.ImportParameters(parameters);
            }

            if (!string.IsNullOrWhiteSpace(Options.PemFilePath))
            {
                var publicKey = File.ReadAllBytes(Options.PemFilePath);
                PublicKey = (RSACryptoServiceProvider)(new X509Certificate2(publicKey).PublicKey.Key);
            }
        }

        #region 私有成员

        /// <summary>
        /// 微信服务配置
        /// </summary>
        WeChatServiceOptions Options { get; }

        #region RAS

        /// <summary>
        /// 证书系列号
        /// </summary>
        string CertSN { get; }

        /// <summary>
        /// p12证书
        /// </summary>
        X509Certificate2 PrivateCert { get; }

        /// <summary>
        /// 私钥
        /// </summary>
        RSACryptoServiceProvider PrivateKey { get; }

        /// <summary>
        /// 公钥
        /// </summary>
        RSACryptoServiceProvider PublicKey { get; }

        #endregion

        #region AES

        //string ALGORITHM = "AES/GCM/NoPadding";

        //int TAG_LENGTH_BIT = 128;

        //int NONCE_LENGTH_BYTE = 12;

        #endregion

        #endregion

        /// <summary>
        /// 获取签名
        /// <para>SHA256-RSA</para>
        /// </summary>
        /// <param name="data">验证签名字符串</param>
        /// <returns></returns>
        public byte[] GetSignByteWithSHA256_RSA(string data)
        {
            byte[] bytes = Options.Encoding.GetBytes(data);

            byte[] signature = PrivateKey.SignData(bytes, "SHA256");

            return signature;
        }

        /// <summary>
        /// 获取签名
        /// <para>SHA256-RSA</para>
        /// </summary>
        /// <param name="data">验证签名字符串</param>
        /// <returns></returns>
        public string GetSignBase64WithSHA256_RSA(string data)
        {
            return Convert.ToBase64String(GetSignByteWithSHA256_RSA(data));
        }

        /// <summary>
        /// 获取MD5值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="lower">小写</param>
        /// <returns></returns>
        public string GetMD5(string data, bool lower)
        {
            return lower ? EncryptHelper.GetLowerMD5(data, Options.Encoding)
                 : EncryptHelper.GetMD5(data);
        }

        /// <summary>
        /// 解密
        /// <para>AES-256-GCM</para>
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="nonce">长度为12个字节的随机字符串</param>'
        /// <param name="associatedData">长度小于16个字节的字符串</param>
        /// <returns></returns>
        public string DecryptWithAES_256_GCM(string ciphertext, string nonce, string associatedData)
        {
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            AeadParameters aeadParameters = new AeadParameters(
                new KeyParameter(Options.Encoding.GetBytes(Options.Key)),
                128,
                Options.Encoding.GetBytes(nonce),
                Options.Encoding.GetBytes(associatedData));

            gcmBlockCipher.Init(false, aeadParameters);

            byte[] bytes = Convert.FromBase64String(ciphertext);
            byte[] plaintext = new byte[gcmBlockCipher.GetOutputSize(associatedData.Length)];
            int length = gcmBlockCipher.ProcessBytes(bytes, 0, associatedData.Length, plaintext, 0);
            gcmBlockCipher.DoFinal(plaintext, length);
            return Options.Encoding.GetString(plaintext);
        }

        /// <summary>
        /// 解密
        /// <para>AES-256-ECB</para>
        /// <para>填充方案: PKCS7Padding</para>
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <returns></returns>
        public string DecryptWithAES_256_ECB(string ciphertext)
        {
            var ciphertextA = ciphertext;
            var ciphertextB = Convert.FromBase64String(ciphertextA);
            var key_MD5_Lower = GetMD5(Options.Key, true);

            var decryptor = new RijndaelManaged();
            decryptor.BlockSize = 128;
            decryptor.Padding = PaddingMode.PKCS7;
            decryptor.Mode = CipherMode.ECB;
            decryptor.Key = Options.Encoding.GetBytes(key_MD5_Lower);
            ICryptoTransform cTransform = decryptor.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(ciphertextB, 0, ciphertextB.Length);
            return Options.Encoding.GetString(resultArray);
        }

        /// <summary>
        /// 加密
        /// <para>RSA</para>
        /// <para>填充方案: RSAES-OAEP(Optimal Asymmetric Encryption Padding)</para>
        /// </summary>
        /// <param name="data">明文</param>
        /// <returns></returns>
        public string EncryptWithRSA(string data)
        {
            var bytes = PublicKey.Encrypt(Encoding.UTF8.GetBytes(data), true);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 解密
        /// <para>RSA</para>
        /// <para>填充方案: RSAES-OAEP(Optimal Asymmetric Encryption Padding)</para>
        /// </summary>
        /// <param name="data">密文</param>
        /// <returns></returns>
        public string DecryptWithRSA(string data)
        {
            var bytes = PrivateKey.Decrypt(Encoding.UTF8.GetBytes(data), true);

            return Convert.ToBase64String(bytes);
        }
    }
}
