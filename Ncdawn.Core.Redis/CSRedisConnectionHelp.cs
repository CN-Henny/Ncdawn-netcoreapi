using CSRedis;
using System.Collections.Concurrent;

namespace Ncdawn.Core.Redis
{
    /// <summary>
    /// ConnectionMultiplexer对象管理帮助类
    /// </summary>
    public static class CSRedisConnectionHelp
    {
        //系统自定义Key前缀
        public static readonly string SysCustomKey = "";
        private static readonly object Locker = new object();
        private static CSRedisClient _instance;
        private static readonly ConcurrentDictionary<string, CSRedisClient> ConnectionCache = new ConcurrentDictionary<string, CSRedisClient>();

        /// <summary>
        /// 缓存获取
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static CSRedisClient GetConnectionMultiplexer(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetManager(connectionString);
            }
            return ConnectionCache[connectionString];
        }
        /// <summary>
        /// 单例获取
        /// </summary>
        public static CSRedisClient Instance(string readWriteHosts)
        {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = GetManager(readWriteHosts);
                        }
                    }
                }
                return _instance;
        }

        private static CSRedisClient GetManager(string connectionString = null)
        {
            var connect = new CSRedisClient(connectionString);
            return connect;
        }
    }
}