using System;
using System.Linq;
using System.Linq.Expressions;

namespace thu6_pvo_dictionary.iRepositories
{
    interface IRespositoryBase<T>
    {

        /// <summary>
        ///     Retrieve all data of repository
        /// </summary>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> GetAllEntities();

        /// <summary>
        ///     Retrieve all data of repository by Condition
        /// </summary>
        /// <param name="expression">Condition</param>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> GetEntitiesByCondition(Expression<Func<T, bool>> expression);

        /// <summary>
        ///     Find data by id
        /// </summary>
        /// <param name="id">Table's primary key</param>
        /// <returns>Object</returns>
        T FindEntityById(object id);

        /// <summary>
        ///     Save a new entity in repository
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Object</returns>
        T CreateEntity(T entity);

        /// <summary>
        ///     Update a entity in repository by entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>T</returns>
        T UpdateEntity(T entity);

        /// <summary>
        ///    Delete a entity in repository by entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Bool</returns>
        bool DeleteEntity(T entity);
    }
}
