using System;
using System.Collections.Generic;
using System.Text;

namespace Library.SuperSocket.Extension
{
    /// <summary>
    /// 初始CRC值
    /// </summary>
    public enum InitialCrcValue
    {
        Zeros,
        NonZero1 = 0xffff,
        NonZero2 = 0x1D0F
    }
    /// <summary>
    /// 初始CRC值
    /// </summary>
    public enum CrcLength
    {
        B8 = 8,
        B16 = 16
    }

    /// <summary>
    /// CRC16-CCITT 的校验值
    /// </summary>
    public class CrcCcitt
    {
        const ushort poly = 4129;
        ushort[] table = new ushort[256];
        public InitialCrcValue initialCrcValue;
        ushort initialValue = 0;
        public CrcLength crcLength;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">位数</param>
        /// <param name="initialValue">初始值</param>
        public CrcCcitt(CrcLength length, InitialCrcValue initialValue = InitialCrcValue.Zeros)
        {
            crcLength = length;
            if (crcLength == CrcLength.B16)
            {
                this.initialCrcValue = initialValue;
                this.initialValue = (ushort)initialValue;
                ushort temp, a;
                for (int i = 0; i < table.Length; i++)
                {
                    temp = 0;
                    a = (ushort)(i << 8);
                    for (int j = 0; j < 8; j++)
                    {
                        if (((temp ^ a) & 0x8000) != 0)
                        {
                            temp = (ushort)((temp << 1) ^ poly);
                        }
                        else
                        {
                            temp <<= 1;
                        }
                        a <<= 1;
                    }
                    table[i] = temp;
                }
            }
        }

        /// <summary>
        /// 计算分片数
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public ushort ComputeChecksum_16(byte[] bytes)
        {
            ushort crc = this.initialValue;
            for (int i = 0; i < bytes.Length; i++)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }

        /// <summary>
        /// 计算分片数
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte ComputeChecksum_8(byte[] bytes)
        {
            byte crc = (byte)(bytes[0] ^ bytes[1]);
            for (int i = 2; i < bytes.Length; i++)
            {
                crc ^= bytes[i];
            }
            return crc;
        }

        public object ComputeChecksum(byte[] bytes)
        {
            switch (crcLength)
            {
                case CrcLength.B8:
                    return ComputeChecksum_8(bytes);
                case CrcLength.B16:
                    return ComputeChecksum_16(bytes);
                default:
                    throw new Exception("计算分片数 : 没有指定位数");
            }
        }

        /// <summary>
        /// 计算分片数
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            switch (crcLength)
            {
                case CrcLength.B8:
                    byte crc_B8 = ComputeChecksum_8(bytes);
                    return new byte[] { crc_B8 };
                case CrcLength.B16:
                    ushort crc_B16 = ComputeChecksum_16(bytes);
                    return new byte[] { (byte)(crc_B16 >> 8), (byte)(crc_B16 & 0x00ff) };
                default:
                    throw new Exception("计算分片数 : 没有指定位数");
            }
        }
    }
}
