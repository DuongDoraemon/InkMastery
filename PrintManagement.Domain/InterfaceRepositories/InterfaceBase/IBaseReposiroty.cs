using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PrintManagement.Domain.InterfaceRepositories.InterfaceBase
{
    public interface IBaseReposiroty<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<IQueryable<TEntity>> CreateAsync(IQueryable<TEntity> entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(int id, TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity, params Object[] keyValues);
        Task<IQueryable<TEntity>> UpdateAsync(IQueryable<TEntity> entities);
        Task<TEntity> GetByIDAsync(Guid id);
        Task<List<TEntity>> ExecuteStoredProcedureAsync(string spName, params object[] parameters);
        Task<List<TEntity>> ExecuteStoredProcedureWithSqlParamListAsync(string spName, List<SqlParameter> parameters);
        Task<IQueryable<TEntity>> SqlQueryAsync(string query, params object[] parameters);
        void ClearTrackedChanges();
        Task ExecuteStoredProcedureNoReturnAsync(string spName, params object[] parameters);
        Task<T> ExecuteStoredProcedureScalarAsync<T>(string procedureName, List<SqlParameter> parameters);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> CountAsync(List<string> includes, Expression<Func<TEntity, bool>> predicate = null);
        Task<int> CountAsync(string include, Expression<Func<TEntity, bool>> predicate = null);
    }
}
