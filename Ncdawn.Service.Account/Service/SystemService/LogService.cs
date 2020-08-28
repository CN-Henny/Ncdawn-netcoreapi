using Microsoft.Extensions.Configuration;
using Pivotal.Discovery.Client;
using System;
using System.Threading.Tasks;
using Ncdawn.CommonService.Class.Models;
using Ncdawn.EFS.Models;
using Ncdawn.EFS.Response;
using Ncdawn.Core.Redis;

namespace Ncdawn.Service.Account
{
    public interface ILogService : IBaseService
    {
        Task DLOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result);
        Task ELOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result);
        Task ILOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result);
        Task JLOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result);
        Task PLOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result);
        Task ULOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result);
    }

    public class LogService : IBaseService, ILogService
    {
        private readonly CSRedisHelpers redisHelper;
        private readonly CSRedisHelpers redisInstructHelper;
        //ISysNcdawnLogRepository paasOLTNcdawnLogRepository;
        public LogService(IConfiguration configuration, IDiscoveryClient client /*,ISysNcdawnLogRepository _IpaasOLTNcdawnLogRepository*/)
        {
            string RedisConfig = configuration.GetSection("RedisConfig")["LogChannel"].ToString();
            redisHelper = new CSRedisHelpers(RedisConfig, 1);
            string RedisConfigInstruct = configuration.GetSection("RedisConfig")["Connection"].ToString();
            redisInstructHelper = new CSRedisHelpers(RedisConfigInstruct);
           // paasOLTNcdawnLogRepository = _IpaasOLTNcdawnLogRepository;
        }

        public Task DLOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result)
        {
            try
            {
                //SysNcdawnLog paasOltNcdawnLog = new SysNcdawnLog();
                //paasOltNcdawnLog.ObjectId = NewId();
                //paasOltNcdawnLog.CreateTime = DateTime.Now;
                //paasOltNcdawnLog.ModificationTime = DateTime.Now;
                //paasOltNcdawnLog.CreateUserName = "Sys";
                //paasOltNcdawnLog.ModificationUserName = "Sys";
                //paasOltNcdawnLog.Sort = 0;
                //paasOltNcdawnLog.LogType = "DLOG";
                //paasOltNcdawnLog.UserId = UserId;
                //paasOltNcdawnLog.UserName = userName;
                //paasOltNcdawnLog.ProjectName = projectName;
                //paasOltNcdawnLog.ModuleName = moduleName;
                //paasOltNcdawnLog.Method = method;
                //paasOltNcdawnLog.Messages = content;
                //paasOltNcdawnLog.Result = result;
                //paasOLTNcdawnLogRepository.Create(paasOltNcdawnLog);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task ELOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result)
        {
            try
            {
                //SysNcdawnLog paasOltNcdawnLog = new SysNcdawnLog();
                //paasOltNcdawnLog.ObjectId = NewId();
                //paasOltNcdawnLog.CreateTime = DateTime.Now;
                //paasOltNcdawnLog.ModificationTime = DateTime.Now;
                //paasOltNcdawnLog.CreateUserName = "Sys";
                //paasOltNcdawnLog.ModificationUserName = "Sys";
                //paasOltNcdawnLog.Sort = 0;
                //paasOltNcdawnLog.LogType = "ELOG";
                //paasOltNcdawnLog.UserId = UserId;
                //paasOltNcdawnLog.UserName = userName;
                //paasOltNcdawnLog.ProjectName = projectName;
                //paasOltNcdawnLog.ModuleName = moduleName;
                //paasOltNcdawnLog.Method = method;
                //paasOltNcdawnLog.Messages = content;
                //paasOltNcdawnLog.Result = result;
                //paasOLTNcdawnLogRepository.Create(paasOltNcdawnLog);
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public Task ILOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result)
        {
            try
            {
                //SysNcdawnLog paasOltNcdawnLog = new SysNcdawnLog();
                //paasOltNcdawnLog.ObjectId = NewId();
                //paasOltNcdawnLog.CreateTime = DateTime.Now;
                //paasOltNcdawnLog.ModificationTime = DateTime.Now;
                //paasOltNcdawnLog.CreateUserName = "Sys";
                //paasOltNcdawnLog.ModificationUserName = "Sys";
                //paasOltNcdawnLog.Sort = 0;
                //paasOltNcdawnLog.LogType = "ILOG";
                //paasOltNcdawnLog.UserId = UserId;
                //paasOltNcdawnLog.UserName = userName;
                //paasOltNcdawnLog.ProjectName = projectName;
                //paasOltNcdawnLog.ModuleName = moduleName;
                //paasOltNcdawnLog.Method = method;
                //paasOltNcdawnLog.Messages = content;
                //paasOltNcdawnLog.Result = result;
                //paasOLTNcdawnLogRepository.Create(paasOltNcdawnLog);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task JLOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result)
        {
            try
            {
                //SysNcdawnLog paasOltNcdawnLog = new SysNcdawnLog();
                //paasOltNcdawnLog.ObjectId = NewId();
                //paasOltNcdawnLog.CreateTime = DateTime.Now;
                //paasOltNcdawnLog.ModificationTime = DateTime.Now;
                //paasOltNcdawnLog.CreateUserName = "Sys";
                //paasOltNcdawnLog.ModificationUserName = "Sys";
                //paasOltNcdawnLog.Sort = 0;
                //paasOltNcdawnLog.LogType = "JLOG";
                //paasOltNcdawnLog.UserId = UserId;
                //paasOltNcdawnLog.UserName = userName;
                //paasOltNcdawnLog.ProjectName = projectName;
                //paasOltNcdawnLog.ModuleName = moduleName;
                //paasOltNcdawnLog.Method = method;
                //paasOltNcdawnLog.Messages = content;
                //paasOltNcdawnLog.Result = result;
                //paasOLTNcdawnLogRepository.Create(paasOltNcdawnLog);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task PLOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    //SysNcdawnLog paasOltNcdawnLog = new SysNcdawnLog();
                    //paasOltNcdawnLog.ObjectId = NewId();
                    //paasOltNcdawnLog.CreateTime = DateTime.Now;
                    //paasOltNcdawnLog.ModificationTime = DateTime.Now;
                    //paasOltNcdawnLog.CreateUserName = "Sys";
                    //paasOltNcdawnLog.ModificationUserName = "Sys";
                    //paasOltNcdawnLog.Sort = 0;
                    //paasOltNcdawnLog.LogType = "PLOG";
                    //paasOltNcdawnLog.UserId = UserId;
                    //paasOltNcdawnLog.UserName = userName;
                    //paasOltNcdawnLog.ProjectName = projectName;
                    //paasOltNcdawnLog.ModuleName = moduleName;
                    //paasOltNcdawnLog.Method = method;
                    //paasOltNcdawnLog.Messages = content;
                    //paasOltNcdawnLog.Result = result;
                    //paasOLTNcdawnLogRepository.Create(paasOltNcdawnLog);
                });
            }
            catch (Exception)
            {

            }
        }

        public async Task ULOG(string UserId, string userName, string projectName, string moduleName, string method, string content, string result)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    //SysNcdawnLog paasOltNcdawnLog = new SysNcdawnLog();
                    //paasOltNcdawnLog.ObjectId = NewId();
                    //paasOltNcdawnLog.CreateTime = DateTime.Now;
                    //paasOltNcdawnLog.ModificationTime = DateTime.Now;
                    //paasOltNcdawnLog.CreateUserName = "Sys";
                    //paasOltNcdawnLog.ModificationUserName = "Sys";
                    //paasOltNcdawnLog.Sort = 0;
                    //paasOltNcdawnLog.LogType = "ULOG";
                    //paasOltNcdawnLog.UserId = UserId;
                    //paasOltNcdawnLog.UserName = userName;
                    //paasOltNcdawnLog.ProjectName = projectName;
                    //paasOltNcdawnLog.ModuleName = moduleName;
                    //paasOltNcdawnLog.Method = method;
                    //paasOltNcdawnLog.Messages = content;
                    //paasOltNcdawnLog.Result = result;
                    //paasOLTNcdawnLogRepository.Create(paasOltNcdawnLog);
                });
            }
            catch (Exception)
            {

            }
        }

        private string FormatInstructFormat(string InstructMsg)
        {
            return "$," + InstructMsg + ",#";
        }
        public string NewId()
        {
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            id += guid.Substring(0, 10);
            return id;
        }
    }
}
