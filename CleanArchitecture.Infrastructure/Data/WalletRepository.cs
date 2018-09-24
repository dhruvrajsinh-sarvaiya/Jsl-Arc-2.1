using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Infrastructure.Data
{
    public class WalletRepository<T> : IWalletRepository<T> where T : BizBase
    {
        private readonly CleanArchitectureContext _dbContext;

        readonly ILogger<WalletRepository<T>> _log;

        public WalletRepository(ILogger<WalletRepository<T>> log)
        {
            _log = log;
        }

        public T GetById(long id)
        {
            try
            {
                return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public List<T> List()
        {
            try
            {
                return _dbContext.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public T Add(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                _dbContext.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public T AddProduct(T entity)
        {
            try
            {
                _dbContext.Set<T>().Add(entity);
                _dbContext.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
