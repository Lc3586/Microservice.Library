using Microservice.Library.FreeSql.Application;

namespace Microservice.Library.FreeSql.Extention
{
    public static class CodeFirstExtension
    {
        /// <summary>
        /// 获取指定字符串的缩写形式
        /// </summary>
        /// <remarks>如果数据长度没有超过指定的长度限制, 则返回原始数据</remarks>
        /// <param name="value">数据</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="maxBlockLength">最大的块长度</param>
        /// <returns></returns>
        public static string GetAbbreviation(this string value, int maxLength, int maxBlockLength = 3)
        {
            if (value.Length <= maxLength)
                return value;

            for (int i = maxBlockLength; i > 0; i--)
            {
                var result = Abbreviation(i);
                if (result.Length <= maxLength)
                    return result;
            }

            throw new FreeSqlException($"数据: {value}, 获取到的缩写超过了最大长度的限制, 请缩短该数据的长度.");

            string Abbreviation(int blockLength)
            {
                var result = string.Empty;

                for (int i = 0, j = blockLength; i < value.Length; i++)
                {
                    if (j == 0)
                        j = blockLength;
                    else if (i == 0 || j < blockLength - 1)
                    {
                        result += value[i];
                        blockLength--;
                    }
                    else if (char.IsUpper(value[i]))
                    {
                        result += value[i];
                        if (value.Length >= i + 2 && $"{value[i]}{value[i + 1]}".ToLower() == "id")
                        {
                            result += value[i + 1];
                            blockLength = 1;
                        }
                        else
                            blockLength = 2;
                    }
                    else if (value[i] == '_')
                    {
                        result += $"_{value[i + 1]}";
                        blockLength = 2;
                        i++;
                    }
                    else
                        continue;
                }

                return result;
            }
        }
    }
}
