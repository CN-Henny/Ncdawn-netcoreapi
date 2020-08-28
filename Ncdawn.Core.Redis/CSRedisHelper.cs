using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ncdawn.Core.Redis
{
    public class CSRedisHelpers
    {
        private readonly CSRedisClient csredis;

        public CSRedisHelpers(string conn, int DbNum = 0)
        {
            csredis = CSRedisConnectionHelp.GetConnectionMultiplexer(conn + ",defaultDatabase=" + DbNum);
        }

        #region 获取整个hash的数据
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public List<T> GetHashAll<T>(string hashId)
        {
            var result = new List<T>();
            var result1 = csredis.HGetAll(hashId);
            foreach (var item in result1)
            {
                var model = ConvertObj<T>(item.Value);
                result.Add(model);
            }
            return result;
        }
        #endregion

        #region 存储数据到hash表
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool HashSet<T>(string key, string dataKey, T t)
        {
            string json = ConvertJson(t);
            return csredis.HSet(key, dataKey, json);
        }
        #endregion

        #region 移除hash中的某值
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool HashDelete(string key, string dataKey)
        {
            return csredis.HDel(key, dataKey) > 0;
        }
        #endregion

        #region 删除单个key
        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public long KeyDelete(string[] key)
        {
            return csredis.Del(key);
        }
        #endregion

        #region 判斷Key是否存在
        /// <summary>
        /// 判斷Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return csredis.Exists(key);
        }
        #endregion

        #region 判斷Key是否存在
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            return csredis.LLen(key);
        }
        #endregion

        #region 从hash表获取数据
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string dataKey)
        {
            string value = csredis.HGet(key, dataKey);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 从hash表获取JSON数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public string HashGetJson(string key, string dataKey)
        {
            return csredis.HGet(key, dataKey);
        }
        #endregion

        #region 判断某个数据是否已经被缓存
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public bool Exists(string key, string dataKey)
        {
            return csredis.HExists(key, dataKey);
        }
        #endregion

        #region 判断某个Key是否已经被缓存
        /// <summary>
        /// 判断某个Key是否已经被缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HashExists(string key)
        {
            bool flag = false;
            try
            {
                flag = csredis.Exists(key);
            }
            catch (Exception)
            {
            }
            return flag;
        }
        #endregion

        #region 保存单个key value
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间 秒(默认15分钟)</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, int second = 900)
        {
            return csredis.Set(key, value, second);
        }
        #endregion

        #region 获取单个key的值
        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public string StringGet(string key)
        {
            return csredis.Get(key);
        }
        #endregion

        #region 入队(往后屁股添加)
        /// <summary>
        /// 入队(往后屁股添加)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListRightPush<T>(string key, T value)
        {
            csredis.RPush(key, ConvertJson(value));
        }
        #endregion

        #region 出队(移出并获取列表的第一个元素)
        /// <summary>
        /// 出队(移出并获取列表的第一个元素)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            var value = csredis.LPop(key);
            return ConvertObj<T>(value);
        }
        #endregion

        #region 入栈(往头顶加)
        /// <summary>
        /// 入栈(往头顶加)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void StackLeftPush<T>(string key, T value)
        {
            csredis.LPush(key, ConvertJson(value));
        }
        #endregion

        #region 出栈(从头出)
        /// <summary>
        /// 出栈(从头出)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T StackLeftPop<T>(string key)
        {
            var value = csredis.LPop(key);
            return ConvertObj<T>(value);
        }
        #endregion

        #region 转换
        /// <summary>
        /// ConvertJson
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }
        /// <summary>
        /// ConvertObj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private T ConvertObj<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// ConvetList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        private List<T> ConvetList<T>(string[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        #endregion
    }
}
