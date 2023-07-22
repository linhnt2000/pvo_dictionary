﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using thu6_pvo_dictionary.Common;
using EFCore.BulkExtensions;
using thu6_pvo_dictionary.Models.DataContext;
using thu6_pvo_dictionary.iRepositories;

namespace thu6_pvo_dictionary.Respositories
{
    public class BaseRespository<T> : IRespositoryBase<T> where T : class, new()
    {
        protected DbSet<T> Model { get; set; }
        protected AppDbContext DbContext { get; set; }
        protected ApiOption ApiConfig { get; set; }
        public BaseRespository(ApiOption apiConfig, AppDbContext databaseContext)
        {
            DbContext = databaseContext;
            Model = databaseContext.Set<T>();
            ApiConfig = apiConfig;
        }

        /// <summary>
        ///     Retrieve all data of repository
        /// </summary>
        /// <returns>IQueryable<T></returns>
        public IQueryable<T> GetAllEntities()
        {
            return Model.AsNoTracking();
        }

        /// <summary>
        ///     Retrieve all data of repository by Condition
        /// </summary>
        /// <param name="expression">Condition</param>
        /// <returns>IQueryable<T></returns>
        public IQueryable<T> GetEntitiesByCondition(Expression<Func<T, bool>> expression)
        {
            return Model.Where(expression).AsNoTracking();
        }

        /// <summary>
        ///     Find data by id
        /// </summary>
        /// <param name="id">Table's primary key</param>
        /// <returns>Object</returns>
        public T FindEntityById(object id)
        {
            try
            {
                var model = Model.Find(id);
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        /// <summary>
        ///     Save a new entity in repository
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Object</returns>
        public T CreateEntity(T entity)
        {
            try
            {
                var model = Model.Add(entity);
                return model.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Update a entity in repository by entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>T</returns>
        public T UpdateEntity(T entity)
        {
            try
            {
                Model.Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

      

        /// <summary>
        ///    Delete a entity in repository by entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Bool</returns>
        public bool DeleteEntity(T entity)
        {
            try
            {
                Model.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        ///     Save change to database
        /// </summary>
        public void SaveChange()
        {
            DbContext.SaveChanges();
        }

    }


}

