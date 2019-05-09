using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using Helpdesk.DAL;
using System.Linq.Expressions;
using System;
using Helpdesk.BLL;

namespace Helpdesk.BLL
{
    public class ManagerBase<TEntity> where TEntity : class
    {
        protected IRepository<TEntity> Repository;
        
        public ManagerBase()
        {
            Repository = new GenericRepository<TEntity>(ContextFactory.GetContext());         
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Repository.GetAll().Distinct();
        }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return Repository.GetQuery();
        }

        public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.GetQuery(predicate);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Single(predicate);
        }

        //public void Refresh(RefreshMode refreshMode, IEnumerable collection)
        //{
        //    Repository.Refresh(refreshMode, collection);
        //}

        //public void Refresh(RefreshMode refreshMode, object entity)
        //{
        //    Repository.Refresh(refreshMode, entity);
        //}

        /// <summary>
        /// Insert immediately to Database
        /// </summary>
        /// <param name="entityToInsert"></param>
        /// <returns></returns>
        public virtual void Insert(TEntity entityToInsert)
        {
            this.Repository.Add(entityToInsert);
            this.Repository.UnitOfWork.SaveChanges();
        }


        /// <summary>
        /// Update immediately to Database
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            this.Repository.Update(entityToUpdate);
            this.Repository.UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Delete immediately from Database
        /// </summary>
        /// <param name="entityToDelete"></param>
        public virtual void Delete(TEntity entityToDelete)
        {
            this.Repository.Delete(entityToDelete);
            this.Repository.UnitOfWork.SaveChanges();
        }
    }
}
