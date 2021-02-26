using System;

namespace Microservice.Library.Cache.Model
{
    /// <summary>
    /// Redis值信息
    /// </summary>
    public struct RedisValueInfo
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 值类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan? ExpireTime { get; set; }

        /// <summary>
        /// 过期类型
        /// </summary>
        public ExpireType? ExpireType { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(RedisValueInfo))
                return false;

            var _obj = (RedisValueInfo)obj;
            if (!_obj.Value.Equals(this.Value))
                return false;
            if (!_obj.TypeName.Equals(this.TypeName))
                return false;
            if (!_obj.ExpireTime.Equals(this.ExpireTime))
                return false;
            if (!_obj.ExpireType.Equals(this.ExpireType))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(RedisValueInfo left, RedisValueInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RedisValueInfo left, RedisValueInfo right)
        {
            return !(left == right);
        }
    }
}
