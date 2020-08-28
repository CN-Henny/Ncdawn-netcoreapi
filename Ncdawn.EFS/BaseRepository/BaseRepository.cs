using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using Ncdawn.EFS.Models;
using Microsoft.EntityFrameworkCore;
using Ncdawn.EFS.DBContext;
using Ncdawn.Core.Common;
using System.Data;

namespace Ncdawn.EFS.Response
{

    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public MSSqlDBContext db { get; set; }

        public BaseRepository(MSSqlDBContext context)
        {
            if (context != null)
            {
                this.db = context;
            }
        }
        public virtual bool Create(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges() > 0;
        }



        public virtual bool CreateBatch(List<T> models)
        {
            db.Set<T>().AddRange(models);
            return db.SaveChanges() > 0;
        }

        public virtual T CreateRtn(T model)
        {
            db.Set<T>().Add(model);
            if (db.SaveChanges() > 0)
            {
                return model;
            }
            else
            {
                return null;
            }
        }

        public virtual bool Edit(T model)
        {
            if (db.Entry<T>(model).State == EntityState.Modified)
            {
                return db.SaveChanges() > 0;
            }
            else if (db.Entry<T>(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry<T>(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    //T old = Find(model._ID);
                    //db.Entry<old>.CurrentValues.SetValues(model);
                    return false;
                }
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public virtual bool EditNotNull(T model)
        {
            db.Attach(model);
            foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
            {
                if (p.Name == "ObjectId" || p.Name == "CreateTime" || p.Name == "ModificationTime" || p.Name == "CreateUserName")
                {
                    if (p.Name == "CreateTime")
                    {
                        p.SetValue(model, p.GetValue(model), null);
                    }
                    if (p.Name == "ModificationTime")
                    {
                        object value = DateTime.Now;
                        p.SetValue(model, value, null);
                    }
                    continue;
                }

                if (p.PropertyType == typeof(int) && p.GetValue(model).ToString() == "0")
                {
                    // 如果是整型的变量，并且给的是0就不进行修改，因为这个时候说明没传这个值。
                    continue;
                }

                if (p.GetValue(model) != null)
                {
                    db.Entry<T>(model).Property(p.Name).IsModified = true;
                }
            }
            return db.SaveChanges() > 0;
        }

        public virtual bool EditBatch(List<T> models)
        {
            foreach (var item in models)
            {
                db.Set<T>().Attach(item);
                db.Entry(item).State = EntityState.Modified;
            }
            return db.SaveChanges() > 0;
        }

        public virtual bool Delete(T model)
        {
            db.Set<T>().Remove(model);
            return db.SaveChanges() > 0;
        }

        public virtual int Delete(params object[] keyValues)
        {
            foreach (var item in keyValues)
            {
                T model = GetById(item);
                if (model != null)
                {
                    db.Set<T>().Remove(model);
                }
            }
            return db.SaveChanges();
        }

        public virtual bool DeleteByList(List<string> keyValues)
        {
            foreach (var item in keyValues)
            {
                T model = GetById(item);
                if (model != null)
                {
                    db.Set<T>().Remove(model);
                }
            }
            return db.SaveChanges() > 0;
        }
        /// <summary>
        /// 如果没有IsDelete则真实删除
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual bool DeleteNotTrue(List<string> keyValues)
        {
            foreach (var item in keyValues)
            {
                T model = GetById(item);
                bool flag = true;
                foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
                {
                    if (p.Name == "IsDelete")
                    {
                        flag = false;
                        object value = "1";
                        p.SetValue(model, value, null);
                        db.Entry<T>(model).Property(p.Name).IsModified = true;
                    }
                    if (p.Name == "UnDelete")
                    {
                        object value = ResultHelper.NewId;
                        p.SetValue(model, value, null);
                        db.Entry<T>(model).Property(p.Name).IsModified = true;
                    }
                }
                if (flag)
                {
                    db.Set<T>().Remove(model);
                }
            }
            return db.SaveChanges() > 0;
        }

        public virtual T GetById(object keyValues)
        {
            return db.Set<T>().Find(keyValues);
        }

        public virtual IQueryable<T> GetList()
        {
            return db.Set<T>();
        }

        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).AsQueryable();
        }

        public virtual IQueryable<T> GetList<S>(int pageSize, int pageIndex, out int total
            , Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, bool>> orderByLambda)
        {
            var queryable = db.Set<T>().Where(whereLambda);
            total = queryable.Count();
            if (isAsc)
            {
                queryable = queryable.OrderBy(orderByLambda).Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
            else
            {
                queryable = queryable.OrderByDescending(orderByLambda).Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
            return queryable;
        }

        public virtual bool IsExist(object id)
        {
            return GetById(id) != null;
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> where)
        {
            IQueryable<T> queryable = db.Set<T>().Where(where);
            return queryable.Count() > 0 ? queryable.FirstOrDefault() : null;
        }
        public virtual void ChangeTracker()
        {
            var currentEntry = db.ChangeTracker.Entries().FirstOrDefault();
            if (currentEntry != null)
            {
                //设置实体State为EntityState.Detached，取消跟踪该实体，之后dbContext.ChangeTracker.Entries().Count()的值会减1
                currentEntry.State = EntityState.Detached;
            }
        }
        #region 事务
        public virtual bool CreateTransaction(T model)
        {
            db.Set<T>().Add(model);
            return true;
        }
        public virtual bool EditTransaction(T model)
        {
            if (db.Entry<T>(model).State == EntityState.Modified)
            {
                return true;
            }
            else if (db.Entry<T>(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry<T>(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public virtual bool DeleteTransaction(T model)
        {
            db.Set<T>().Remove(model);
            return true;
        }
        public virtual bool TransactionCommit()
        {
            try
            {
                return db.SaveChanges() > 0;
            }
            catch
            {
                var currentEntryList = db.ChangeTracker.Entries().ToList();
                for (int i = currentEntryList.Count() - 1; i >= 0; i--)
                {
                    if (currentEntryList[i] != null)
                    {
                        currentEntryList[i].State = EntityState.Detached;
                    }
                }
                return false;
            }
        }

        //public virtual Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction Transaction(IsolationLevel isolationLevel)
        //{
        //    var transaction = db.Database.BeginTransaction(isolationLevel);
        //    return transaction;
        //}
        #endregion
        /// <summary>
        /// 获取db
        /// </summary>
        /// <returns></returns>
        public virtual MSSqlDBContext GetDb()
        {
            return db;
        }
    }
}
