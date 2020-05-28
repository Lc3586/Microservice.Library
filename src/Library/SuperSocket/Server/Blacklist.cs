﻿using Library.Cache;
using Library.Extension;
using SuperSocket;
using SuperSocket.WebSocket.Server;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Library.SuperSocket.Server
{
    /// <summary>
    /// 黑名单扩展类
    /// 默认使用缓存(<see cref="Library.Cache.CacheHelper"/>)存储数据
    /// </summary>
    public class Blacklist
    {
        public Blacklist()
        {
            _storage = new CacheStorage();
        }

        public Blacklist(IStorage storage)
        {
            _storage = storage;
        }

        private static IStorage _storage;

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns>是否处于冻结中</returns>
        public static bool Verification(IAppSession session, out BlackInfo info)
        {
            info = _storage.Get(GetId(session));
            return info == null ? true : info.UnfreezeTime > DateTime.Now;
        }

        /// <summary>
        /// 新增
        /// </summary>
        public static void Freeze(IAppSession session)
        {
            var info = _storage.Get(GetId(session));
            if (info != null)
                _storage.Remove(info.Id);
            else
                info = new BlackInfo
                {
                    Id = GetId(session),
                    RemoteEndPoint = session.RemoteEndPoint.ToString().Split(':')[0],
                    LocalEndPoint = session.LocalEndPoint.ToString().Split(':')[0],
                    UnfreezeTime = DateTime.Now,
                    FfreezeTimes = 0
                };

            info.FfreezeTimes += 1;
            info.UnfreezeTime.AddMinutes(5 * info.FfreezeTimes);
            _storage.Add(info);
        }

        /// <summary>
        /// 移除
        /// </summary>
        public static void Unfreeze(IAppSession session)
        {
            _storage.Remove(GetId(session));
        }

        /// <summary>
        /// 获取Id
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private static string GetId(IAppSession session)
        {
            return (session.LocalEndPoint.ToString().Split(':')[0] + session.RemoteEndPoint.ToString().Split(':')[0]).ToMD5String();
        }
    }

    /// <summary>
    /// 黑名单信息
    /// </summary>
    public class BlackInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 远程 IP 地址
        /// </summary>
        public string RemoteEndPoint { get; set; }

        /// <summary>
        /// 服务器 IP 地址
        /// </summary>
        public string LocalEndPoint { get; set; }

        /// <summary>
        /// 解除限制的时间
        /// </summary>
        public DateTime UnfreezeTime { get; set; }

        /// <summary>
        /// 违规次数
        /// </summary>
        public int FfreezeTimes { get; set; }
    }

    /// <summary>
    /// 数据存储方式
    /// </summary>
    public enum StorageMode
    {
        /// <summary>
        /// 缓存
        /// </summary>
        Cache,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }

    /// <summary>
    /// 存储器接口
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        public BlackInfo Get(string id);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data">会话黑名单信息</param>
        public void Add(BlackInfo data);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id);
    }

    public class CacheStorage : IStorage
    {
        public BlackInfo Get(string id)
        {
            return CacheHelper.Cache.GetCache<BlackInfo>(id);
        }

        public void Add(BlackInfo data)
        {
            CacheHelper.Cache.SetCache(data.Id, data, data.UnfreezeTime - DateTime.Now, ExpireType.Absolute);
        }

        public void Remove(string id)
        {
            CacheHelper.Cache.RemoveCache(id);
        }
    }
}
