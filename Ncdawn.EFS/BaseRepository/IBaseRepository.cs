using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ncdawn.EFS.DBContext;

namespace Ncdawn.EFS.Response
{
    public interface IBaseRepository<T> : IDependency
    {
        bool Create(T model);
        bool CreateBatch(List<T> models);
        T CreateRtn(T model);
        bool Edit(T model);
        bool EditNotNull(T model);
        bool EditBatch(List<T> models);
        bool Delete(T model);
        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="keyValues"></param>
        int Delete(params object[] keyValues);
        /// <summary>
        /// 按主键删除
        /// </summary>
        /// <param name="keyValues"></param>
        bool DeleteByList(List<string> keyValues);
        /// <summary>
        /// 如果没有IsDelete则真实删除
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        bool DeleteNotTrue(List<string> keyValues);
        T GetById(object keyValues);
        /// <summary>
        /// 获得所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetList();
        /// <summary>
        /// 根据表达式获取数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetList<S>(int pageSize, int pageIndex, out int total
            , Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, bool>> orderByLambda);

        bool IsExist(object id);
        int SaveChanges();
        T Find(Expression<Func<T, bool>> where);
        void ChangeTracker();
        bool CreateTransaction(T model);
        bool EditTransaction(T model);
        bool DeleteTransaction(T model);
        bool TransactionCommit();
        MSSqlDBContext GetDb();
    }
}
