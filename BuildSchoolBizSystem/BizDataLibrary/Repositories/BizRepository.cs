using BizDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BizDataLibrary.Repositories
{
    public class BizRepository<T> where T : class //約束型別
    {
        private BizModel _context;

        //protected BizModel Context //讓子類別可以使用_Context欄位
        //{
        //    get { return _context; }
        //}

        public BizRepository(BizModel context)
        {
            if(context == null)
            {
                throw new ArgumentNullException();
            }
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        //public abstract IQueryable<T> GetAll(); //內容不同，給虛擬型別

        //public IQueryable<T> GetAll<TKey>(Expression<Func<T, TKey>> keySelector)
        //{
        //    return _context.Set<T>().OrderBy(keySelector);
        //}

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}
