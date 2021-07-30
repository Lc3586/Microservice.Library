using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microservice.Library.Extension.Helper
{
    /// <summary>
    /// RSA帮助类
    /// </summary>
    public class RSAHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pemFile">pem公钥文件地址</param>
        /// <param name="certFile">p12证书地址</param>
        /// <param name="certPassword">证书密码</param>
        public RSAHelper(string pemFile, string certFile, string certPassword)
        {
            if (!File.Exists(pemFile))
                throw new ApplicationException($"未找到指定的证书文件: {pemFile}.");

            if (!File.Exists(certFile))
                throw new ApplicationException($"未找到指定的证书文件: {certFile}.");

            var publicKey = File.ReadAllBytes(pemFile);
            PrivateKeyProvider = (RSACryptoServiceProvider)(new X509Certificate2(publicKey).PublicKey.Key);

            var privateCert = new X509Certificate2(certFile, certPassword, X509KeyStorageFlags.Exportable);

            PublicKeyProvider = (RSACryptoServiceProvider)privateCert.PrivateKey;

            //需要重新导入参数
            var parameters = PrivateKey.ExportParameters(true);

            PrivateKey.ImportParameters(parameters);

            UseProvider = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="privateKey">私钥</param>
        public RSAHelper(string publicKey, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ApplicationException("未设置公钥.");

            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ApplicationException("未设置私钥.");

            PublicKey = CreateRsaProviderFromPublicKey(publicKey);

            PrivateKey = CreateRsaProviderFromPrivateKey(privateKey);

            UseProvider = false;
        }

        #region 私有成员

        readonly bool UseProvider;

        /// <summary>
        /// 私钥
        /// </summary>
        readonly RSACryptoServiceProvider PrivateKeyProvider;

        /// <summary>
        /// 公钥
        /// </summary>
        readonly RSACryptoServiceProvider PublicKeyProvider;

        /// <summary>
        /// 私钥
        /// </summary>
        readonly RSA PrivateKey;

        /// <summary>
        /// 公钥
        /// </summary>
        readonly RSA PublicKey;

        #region 此部分代码来源于：https://www.cnblogs.com/stulzq/p/7757915.html 作者：晓晨Master（李志强）

        RSA CreateRsaProviderFromPrivateKey(string privateKey)
        {
            var privateKeyBits = Convert.FromBase64String(privateKey);

            var rsa = RSA.Create();
            var rsaParameters = new RSAParameters();

            using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new ApplicationException("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new ApplicationException("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new ApplicationException("Unexpected value read binr.ReadByte()");

                rsaParameters.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.D = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.P = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Q = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DP = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DQ = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(rsaParameters);
            return rsa;
        }

        RSA CreateRsaProviderFromPublicKey(string publicKeyString)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            var x509Key = Convert.FromBase64String(publicKeyString);

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (MemoryStream mem = new MemoryStream(x509Key))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    var rsa = RSA.Create();
                    RSAParameters rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);

                    return rsa;
                }

            }
        }

        int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
            if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        #endregion

        #endregion

        /// <summary>
        /// 默认编码（默认UTF8）
        /// </summary>
        public Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// 默认加密算法类型（默认SHA256）
        /// </summary>
        public HashAlgorithmName DefaultHashAlgorithm = HashAlgorithmName.SHA256;

        /// <summary>
        /// 默认签名填充方案（默认Pkcs1）
        /// </summary>
        public RSASignaturePadding DefaultSignaturePadding = RSASignaturePadding.Pkcs1;

        /// <summary>
        /// 默认加密填充方案（默认SHA256）
        /// </summary>
        public RSAEncryptionPadding DefaultEncryptionPadding = RSAEncryptionPadding.OaepSHA256;

        /// <summary>
        /// 计算SHA1摘要
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public byte[] ToSHA1Bytes(string data, Encoding encoding = null)
        {
            encoding = encoding ?? DefaultEncoding;

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = encoding.GetBytes(data);
            byte[] outputBytes = sha1.ComputeHash(inputBytes);
            return outputBytes;
        }

        /// <summary>
        /// 转为SHA1哈希
        /// </summary>
        /// <param name="data">字符串</param>
        /// <param name="toLower">转为小写</param>
        /// <returns></returns>
        public string ToSHA1String(string data, bool toLower = true)
        {
            byte[] sha1Bytes = ToSHA1Bytes(data);
            string resStr = BitConverter.ToString(sha1Bytes).Replace("-", "");
            return toLower ? resStr.ToLower() : resStr;
        }

        /// <summary>
        /// 获取签名
        /// <para>SHA256-RSA</para>
        /// </summary>
        /// <param name="data">验证签名字符串</param>
        /// <param name="hashAlgorithm">加密算法类型</param>
        /// <param name="padding">填充方案（默认Pkcs1）</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public byte[] GetSignByte(string data, HashAlgorithmName? hashAlgorithm = null, RSASignaturePadding padding = null, Encoding encoding = null)
        {
            hashAlgorithm = hashAlgorithm ?? DefaultHashAlgorithm;
            padding = padding ?? DefaultSignaturePadding;
            encoding = encoding ?? DefaultEncoding;

            byte[] bytes = encoding.GetBytes(data);

            byte[] signature = UseProvider
                ? PrivateKeyProvider.SignData(bytes, hashAlgorithm)
                : PrivateKey.SignData(bytes, hashAlgorithm.Value, padding);

            return signature;
        }

        /// <summary>
        /// 获取签名
        /// <para>SHA256-RSA</para>
        /// </summary>
        /// <param name="data">验证签名字符串</param>
        /// <param name="hashAlgorithm">加密算法类型</param>
        /// <param name="padding">填充方案（默认Pkcs1）</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string GetSignBase64(string data, HashAlgorithmName? hashAlgorithm = null, RSASignaturePadding padding = null, Encoding encoding = null)
        {
            return Convert.ToBase64String(GetSignByte(data, hashAlgorithm, padding, encoding));
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sign"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="padding">填充方案（默认Pkcs1）</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public bool VerifySign(string data, byte[] sign, HashAlgorithmName? hashAlgorithm = null, RSASignaturePadding padding = null, Encoding encoding = null)
        {
            return VerifySign(data, Convert.ToBase64String(sign), hashAlgorithm, padding, encoding);
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sign"></param>
        /// <param name="hashAlgorithm"></param>
        /// <param name="padding">填充方案（默认Pkcs1）</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public bool VerifySign(string data, string sign, HashAlgorithmName? hashAlgorithm = null, RSASignaturePadding padding = null, Encoding encoding = null)
        {
            hashAlgorithm = hashAlgorithm ?? DefaultHashAlgorithm;
            padding = padding ?? DefaultSignaturePadding;
            encoding = encoding ?? DefaultEncoding;

            byte[] dataBytes = encoding.GetBytes(data);
            byte[] signBytes = Convert.FromBase64String(sign);

            var verify = UseProvider
                ? PublicKeyProvider.VerifyData(dataBytes, hashAlgorithm.Value, signBytes)
                : PublicKey.VerifyData(dataBytes, signBytes, hashAlgorithm.Value, padding);

            return verify;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">明文</param>
        /// <param name="padding">填充方案（默认SHA256）</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string Encrypt(string data, RSAEncryptionPadding padding = null, Encoding encoding = null)
        {
            padding = padding ?? DefaultEncryptionPadding;
            encoding = encoding ?? DefaultEncoding;

            var bytes = encoding.GetBytes(data);

            var result = UseProvider
                ? (padding == null
                    ? PublicKeyProvider.Encrypt(bytes, true)
                    : PublicKeyProvider.Encrypt(bytes, padding))
                : PublicKey.Encrypt(bytes, padding);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">密文</param>
        /// <param name="padding">填充方案（默认SHA256）</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public string Decrypt(string data, RSAEncryptionPadding padding = null, Encoding encoding = null)
        {
            padding = padding ?? DefaultEncryptionPadding;
            encoding = encoding ?? DefaultEncoding;

            var bytes = Convert.FromBase64String(data);

            var result = UseProvider
                ? (padding == null
                    ? PrivateKeyProvider.Decrypt(bytes, true)
                    : PrivateKeyProvider.Decrypt(bytes, padding))
                : PrivateKey.Decrypt(bytes, padding);

            return encoding.GetString(result);
        }
    }
}
