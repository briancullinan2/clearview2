using EPIC.DataLayer.Entities;
using System;
using System.Data;
using System.Linq;

namespace EPIC.DataLayer.Extensions
{
    public static class IEntityExtensions
    {
        public static void Save<T>(this T ent, bool? recurse = false) where T : class, IEntity
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

        public static int Update<T>(this T entity, IDbConnection conn, string keyName = "Id") where T : IEntity
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
