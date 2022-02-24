using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microservice.Library.Extension.Helper
{
    /// <summary>
    /// 密码帮助类
    /// </summary>
    public static class CryptographyHelper
    {
        /// <summary>
        /// 计算SHA1摘要
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string data, Encoding encoding = null)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = (encoding ?? Encoding.UTF8).GetBytes(data);
            byte[] outputBytes = sha1.ComputeHash(inputBytes);
            return outputBytes;
        }

        /// <summary>
        /// 转为SHA1哈希
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="toLower">转为小写</param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static string ToSHA1String(this string data, bool toLower = true, Encoding encoding = null)
        {
            byte[] sha1Bytes = ToSHA1Bytes(data, encoding);
            string resStr = BitConverter.ToString(sha1Bytes).Replace("-", "");
            return toLower ? resStr.ToLower() : resStr;
        }

        /// <summary>
        /// 获取MD5值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="lower">小写</param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static string GetMD5(this string data, bool lower, Encoding encoding = null)
        {
            return lower ? data.ToMD5String(encoding)
                 : data.ToMD5String(encoding).ToLower();
        }

        /// <summary>
        /// 解密
        /// <para>AES-256-GCM</para>
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="nonce">长度为12个字节的随机字符串</param>'
        /// <param name="associatedData">长度小于16个字节的字符串</param>
        /// <returns></returns>
        public static string DecryptWithAES_256_GCM(this string ciphertext, string nonce, string associatedData, string key, Encoding encoding = null)
        {
            var _encoding = (encoding ?? Encoding.UTF8);
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            AeadParameters aeadParameters = new AeadParameters(
                new KeyParameter(_encoding.GetBytes(key)),
                128,
                _encoding.GetBytes(nonce),
                _encoding.GetBytes(associatedData));

            gcmBlockCipher.Init(false, aeadParameters);

            byte[] bytes = Convert.FromBase64String(ciphertext);
            byte[] plaintext = new byte[gcmBlockCipher.GetOutputSize(associatedData.Length)];
            int length = gcmBlockCipher.ProcessBytes(bytes, 0, associatedData.Length, plaintext, 0);
            gcmBlockCipher.DoFinal(plaintext, length);
            return _encoding.GetString(plaintext);
        }

        /// <summary>
        /// 解密
        /// <para>AES-256-ECB</para>
        /// <para>填充方案: PKCS7Padding</para>
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="key"></param>
        /// <param name="fPKCS7Padding">
        /// <para>true: PKCS7填充方案</para>
        /// <para>false: PKCS5填充方案（用于解密在java中加密的数据）</para>
        /// </param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static string DecryptWithAES_256(this string ciphertext, string key, bool fPKCS7Padding = true, Encoding encoding = null)
        {
            var _encoding = (encoding ?? Encoding.UTF8);
            var ciphertextA = ciphertext;
            var ciphertextB = Convert.FromBase64String(ciphertextA);

            RijndaelManaged decryptor;
            if (fPKCS7Padding)
            {
                decryptor = new RijndaelManaged
                {
                    //BlockSize = 128,
                    Padding = PaddingMode.PKCS7,
                    Mode = CipherMode.ECB,
                    Key = _encoding.GetBytes(GetMD5(key, true))
                };
            }
            else
            {
                var sr = SecureRandom.GetInstance("SHA1PRNG", false);
                sr.SetSeed(_encoding.GetBytes(key));

                var sr_key = new byte[128];
                sr.NextBytes(sr_key, 0, 128);

                decryptor = new RijndaelManaged
                {
                    BlockSize = 128,
                    Padding = PaddingMode.PKCS7,
                    Mode = CipherMode.CBC,
                    KeySize = 128,
                    Key = sr_key
                };
            }

            ICryptoTransform cTransform = decryptor.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(ciphertextB, 0, ciphertextB.Length);
            return _encoding.GetString(resultArray);
        }

        /// <summary>
        /// RSA公钥加密
        /// </summary>
        /// <param name="data">明文</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="padding">填充方案（默认SHA256）</param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static string EncryptWithRSA(this string data, string publicKey, RSAEncryptionPadding padding = null, Encoding encoding = null)
        {
            var rsa_PublicKey = RSAHelper.CreateRsaProviderFromPublicKey(publicKey);

            var bytes = rsa_PublicKey.Encrypt((encoding ?? Encoding.UTF8).GetBytes(data), padding);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// RSA私钥解密
        /// </summary>
        /// <param name="data">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="padding">填充方案（默认SHA256）</param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static string DecryptWithRSA(this string data, string privateKey, RSAEncryptionPadding padding = null, Encoding encoding = null)
        {
            var rsa_PrivateKey = RSAHelper.CreateRsaProviderFromPrivateKey(privateKey);

            var bytes = rsa_PrivateKey.Decrypt(Convert.FromBase64String(data), padding);

            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

        /// <summary>
        /// RSA公钥加密
        /// </summary>
        /// <param name="data">明文</param>
        /// <param name="pemFile">pem公钥文件地址</param>
        /// <param name="fOAEP">
        /// <para>true: OAEP填充方案(Optimal Asymmetric Encryption Padding)</para>
        /// <para>false: PKCS填充方案（PKCS#1 v1.5 padding）</para></param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static string EncryptWithRSA(this string data, string pemFile, bool fOAEP, Encoding encoding = null)
        {
            var publicKey = File.ReadAllBytes(pemFile);
            var publicKeyProvider = (RSACryptoServiceProvider)(new X509Certificate2(publicKey).PublicKey.Key);
            var bytes = publicKeyProvider.Encrypt((encoding ?? Encoding.UTF8).GetBytes(data), fOAEP);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// RSA私钥解密
        /// </summary>
        /// <param name="data">密文</param>
        /// <param name="certFile">p12证书地址</param>
        /// <param name="certPassword">证书密码</param>
        /// <param name="fOAEP">
        /// <para>true: OAEP填充方案(Optimal Asymmetric Encryption Padding)</para>
        /// <para>false: PKCS填充方案（PKCS#1 v1.5 padding）</para></param>
        /// <param name="encoding">编码（默认UTF8）</param>
        /// <returns></returns>
        public static string DecryptWithRSA(this string data, string certFile, string certPassword, bool fOAEP, Encoding encoding = null)
        {
            var privateCert = new X509Certificate2(certFile, certPassword, X509KeyStorageFlags.Exportable);
            var privateKeyProvider = (RSACryptoServiceProvider)privateCert.PrivateKey;
            var bytes = privateKeyProvider.Decrypt(Convert.FromBase64String(data), fOAEP);
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }
    }
}
