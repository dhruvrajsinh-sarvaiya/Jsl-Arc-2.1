using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Infrastructure.Services
{
    public class WalletRepository<T> : IWalletRepository<T> where T : BizBase
    {
        private readonly AppDbContext _dbContext;

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
    }
}
