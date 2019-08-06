using Bitspco.Identity.Common.Enums;
using Bitspco.Identity.Data.Common.Interfaces;
using Bitspco.Identity.Data.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bitspco.Identity.Data
{
    public class DataAdapter : IDataAdapter
    {
        private IdentityDBContext identityDbContext;

        public DataAdapter(IdentityDBContext identityDbContext)
        {
            this.identityDbContext = identityDbContext;
        }
        private DbContext GetContext<T>()
        {
            var name = typeof(T).Namespace;
            if (name.Contains("ExternalSystems"))
            {
            }
            return identityDbContext;
        }
        private DbContext GetContext(SystemCode system = SystemCode.Identity)
        {
            return identityDbContext;
        }
        private object GetKey(object obj)
        {
            var type = obj.GetType();
            foreach (var item in type.GetProperties())
            {
                if (item.Name == "Id" || item.Name == type.Name + "Id" || item.GetCustomAttribute<KeyAttribute>() != null || item.GetCustomAttribute<DatabaseGeneratedAttribute>() != null)
                    return item.GetValue(obj);
            }
            return 0;
        }
        public IQueryable<T> Select<T>() where T : class
        {
            return GetContext<T>().Set<T>();
        }
        public int Count<T>() where T : class
        {
            return GetContext<T>().Set<T>().Count();
        }
        public T GetById<T>(object id) where T : class
        {
            return GetContext<T>().Set<T>().Find(id);
        }
        public List<T> GetAll<T>() where T : class
        {
            return GetContext<T>().Set<T>().ToList();
        }
        public T Insert<T>(T obj) where T : class
        {
            return GetContext<T>().Set<T>().Add(obj);
        }
        public T InsertAndSave<T>(T entity) where T : class
        {
            try { return Insert(entity); }
            finally { SaveChanges<T>(); }
        }
        public T Update<T>(T obj) where T : class
        {
            var id = GetKey(obj);
            GetContext<T>().Entry(GetById<T>(id)).CurrentValues.SetValues(obj);
            return obj;
        }

        public T UpdateAndSave<T>(T entity) where T : class
        {
            try { return Update(entity); }
            finally { SaveChanges<T>(); }
        }
        public T Delete<T>(T entity) where T : class
        {
            return GetContext<T>().Set<T>().Remove(entity);
        }
        public T DeleteAndSave<T>(T entity) where T : class
        {
            try { return Delete(entity); }
            finally { SaveChanges<T>(); }
        }
        public DbContextTransaction BeginTransaction(SystemCode system = SystemCode.Identity)
        {
            return GetContext(system).Database.BeginTransaction();
        }
        public void SaveChanges(SystemCode system = SystemCode.Identity)
        {
            try { GetContext(system).SaveChanges(); }
            catch (DbEntityValidationException e) {
                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
                throw new Exception(sb.ToString());
            }
        }
        public void SaveChanges<T>()
        {
            try { GetContext<T>().SaveChanges(); }
            catch (DbEntityValidationException e) { throw e; }
        }
    }
}
