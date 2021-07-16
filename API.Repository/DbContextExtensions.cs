using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Repository
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Same Sequene Result Set/Properties with DB  Sequence Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// 
        public static async Task<T> ExecuteReaderMultipleDSAsync<T>(this DbContext db, string commandText, SqlParameter[] param) where T : ISPComplexType, new()
        {
            T ResultData = new T();
            try
            {
                var connection = (SqlConnection)db.Database.GetDbConnection();
                {
                    if (connection != null && connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();

                    var command = connection.CreateCommand();
                    command.CommandText = commandText;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(param);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        foreach (PropertyInfo propInfo in ResultData.GetType().GetProperties())
                        {
                            Type elementType = propInfo.PropertyType.GetGenericArguments()[0];
                            MethodInfo method = typeof(Common).GetMethod("ConvertToObjectAsync");
                            MethodInfo generic = method.MakeGenericMethod(elementType);
                             
                            // for async
                            var task = (Task)generic.Invoke(null, new object[] { reader });
                            await task.ConfigureAwait(false);
                            var resultProperty = task.GetType().GetProperty("Result");
                            propInfo.SetValue(ResultData, resultProperty.GetValue(task));

                            reader.NextResult();
                        }
                    }

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                ResultData = default(T);
            }

            return ResultData;
        }

        public static async Task<T> ExecuteNonQueryAsync<T>(this DbContext db, string commandText, SqlParameter[] param) where T : new()
        {
            T newObj = new T();
            try
            {
                var connection = (SqlConnection)db.Database.GetDbConnection();
                {
                    if (connection != null && connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();

                    var command = connection.CreateCommand();
                    command.CommandText = commandText;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(param);
                    await command.ExecuteNonQueryAsync();

                    SqlParameter[] output = param.Where(x => x.Direction == ParameterDirection.Output).ToArray();

                    var entity = typeof(T);
                    var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    var propDict = new Dictionary<string, PropertyInfo>();
                    propDict = props.ToDictionary(x => x.Name.ToUpper(), x => x);

                    foreach (SqlParameter item in output)
                    {
                        if (propDict.ContainsKey(item.ParameterName.ToString().ToUpper().Replace("@", "")))
                        {
                            var info = propDict[item.ParameterName.ToString().ToUpper().Replace("@", "")];
                            if ((info != null) && info.CanWrite)
                            {
                                var val = command.Parameters[item.ParameterName].Value;
                                info.SetValue(newObj, (val == DBNull.Value ? null : val), null);
                            }
                        }
                    }

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                newObj = default(T);
            }

            return newObj;
        }

        public static async Task<List<T>> ExecuteReaderSingleDSAsync<T>(this DbContext db, string commandText, SqlParameter[] param) where T : ISPComplexType, new()
        {
            T newObj = new T();
            List<T> res = null;
            try
            {
                var connection = (SqlConnection)db.Database.GetDbConnection();
                {
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        await connection.OpenAsync();
                    }

                    var command = connection.CreateCommand();

                    command.CommandText = commandText;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(param);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Type elementType = newObj.GetType();
                        MethodInfo method = typeof(Common).GetMethod("ConvertToObjectAsync");
                        MethodInfo generic = method.MakeGenericMethod(elementType);

                        //Simple
                        //object obj = generic.Invoke(null, new object[] { reader });

                        //For Async
                        var task = (Task)generic.Invoke(null, new object[] { reader });
                        await task.ConfigureAwait(false);
                        var resultProperty = task.GetType().GetProperty("Result");
                        object obj = resultProperty.GetValue(task);

                        IList objectList = obj as IList;

                        res = new List<T>();

                        if (objectList.Count > 0)
                        {
                            res = objectList.Cast<T>().ToList();
                        }
                    }

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                res = null;
            }
            finally
            {
            }

            return res;
        }
    }

    public static class Common
    {
        public static async Task<List<T>> ConvertToObjectAsync<T>(this SqlDataReader dataReader) where T : class, new()
        {
            List<T> data = new();

            if (dataReader.HasRows)
            {
                while (await dataReader.ReadAsync())
                {
                    var newObject = new T();

                    dataReader.MapDataToObject(newObject);

                    data.Add(newObject);
                }
            } 

            return data;
        }


        /// <summary>
        /// Maps a SqlDataReader record to an object. Ignoring case.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <param name="newObject"></param>
        /// <remarks>https://stackoverflow.com/a/52918088</remarks>
        private static void MapDataToObject<T>(this SqlDataReader dataReader, T newObject)
        {
            if (newObject == null) throw new ArgumentNullException(nameof(newObject));

            // Fast Member Usage
            var objectMemberAccessor = FastMember.TypeAccessor.Create(newObject.GetType());
            var propertiesHashSet =
                    objectMemberAccessor
                    .GetMembers()
                    .Select(mp => mp.Name)
                    .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                var name = propertiesHashSet.FirstOrDefault(a => a.Equals(dataReader.GetName(i), StringComparison.InvariantCultureIgnoreCase));
                if (!String.IsNullOrEmpty(name))
                {
                    objectMemberAccessor[newObject, name]
                        = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);
                }
            }
        }
    }
}
