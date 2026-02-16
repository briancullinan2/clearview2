using EPIC.DataLayer.Entities;
using EPIC.DataLayer.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EPIC.DataLayer.Utilities.Extensions
{
    public static class IEntityExtensions
    {
        /// <summary>
        /// Rehydrates the entity by discarding local changes and fetching 
        /// the latest data from the database.
        /// </summary>
        public static void Refetch<T>(this IEntity<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var context = TranslationContext.Current;

            var entry = context.Entry(entity);

            // If the entity isn't being tracked, we need to attach it first
            if (entry.State == EntityState.Detached)
            {
                context.Attach(entity);
            }

            // This executes the SQL SELECT and updates the object's properties
            entry.Reload();
        }

        public static void Save<T>(this T ent, bool? recurse = false) where T : class, IEntity<T>
        {
            // Start the Transaction
            using (var transaction = TranslationContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1. Perform relational checks here (e.g., does the linked Facility exist?)
                    // if (!context.Facilities.Any(f => f.Id == messageEntity.FacilityId)) 
                    //    throw new Exception("Invalid Facility Link");

                    // 2. Add the primary entity
                    TranslationContext.Current.Set<T>().Add(ent);

                    // 3. Commit the changes
                    TranslationContext.Current.SaveChanges();

                    // 4. Finalize the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Arizona Compliance: Roll back to prevent data corruption
                    transaction.Rollback();
                    Log.Error($"Transaction Aborted: {ex.Message}");
                    throw; // Rethrow so the parent catch can handle the fallback
                }
            }
        }

        public static int Update<T>(this T entity, IDbConnection conn, string keyName = "Id") where T : IEntity<T>
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var tableName = type.Name; // Assumes Table Name = Class Name

            // 1. Build the SET clause (skipping the Primary Key)
            var setClauses = props
                .Where(p => p.Name != keyName)
                .Select(p => $"[{p.Name}] = @{p.Name}");

            string sql = $"UPDATE [{tableName}] SET {string.Join(", ", setClauses)} WHERE [{keyName}] = @{keyName}";

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            // 2. Map values to Parameters (Prevents SQL Injection)
            foreach (var prop in props)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = "@" + prop.Name;
                param.Value = prop.GetValue(entity) ?? DBNull.Value;
                cmd.Parameters.Add(param);
            }

            if (conn.State != ConnectionState.Open) conn.Open();
            return cmd.ExecuteNonQuery();
        }

        public static T AsSmart<T>(this T? x) where T : class, IEntity<T>, new()
        {
            // Logic to ensure returned entities are wrapped in your Proxy logic
            return Entities.Entity<T>.Wrap(x) ?? Entities.Entity<T>.Create(new T());
        }

        public static IQueryable<T> AsSmart<T>(this DbSet<T> set) where T : class, IEntity<T>
        {
            // Logic to ensure returned entities are wrapped in your Proxy logic
            return set.Select(x => Entities.Entity<T>.Wrap(x));
        }

        public static IEnumerable<T> AsSmart<T>(this IEnumerable<T> set) where T : class, IEntity<T>
        {
            // Logic to ensure returned entities are wrapped in your Proxy logic
            return set.Select(x => Entities.Entity<T>.Wrap(x));
        }

        public static IQueryable<T> AsSmart<T>(this IQueryable<T> set) where T : class, IEntity<T>
        {
            // Logic to ensure returned entities are wrapped in your Proxy logic
            return set.Select(x => Entities.Entity<T>.Wrap(x));
        }


        public static T Wrap<T>(this T target) where T : class, IEntity<T>
        {
            return T.Wrap(target);
        }


        /*
        public static List<T> ToList<T>(this IQueryable<T> query) where T : IEntity, new()
        {
            // 1. Convert the IQueryable to a raw SQL string
            // If using EF Core: string sql = query.ToQueryString();
            // If using a custom metadata provider, call its specific SQL generator
            string sql = query.ToString();

            var results = new List<T>();

            // 2. Execute via ADO.NET for high-performance medical-grade streaming
            using (var conn = DataFactory.CreateConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Use your reflection-based mapper to hydrate the entity
                            results.Add(DataMapper.Map<T>(reader));
                        }
                    }
                }
            }
            return results;
        }
        */

    }
}
