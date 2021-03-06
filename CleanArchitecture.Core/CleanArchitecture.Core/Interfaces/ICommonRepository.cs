﻿using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface ICommonRepository<T> where T : BizBase
    {
        T GetById(long id);
        T GetActiveById(long id);
        List<T> List();
        List<T> GetAllList();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T AddProduct(T entity);
        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetSingle(int id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetSingleAsync(int id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> sumPredicate);
    }
}
