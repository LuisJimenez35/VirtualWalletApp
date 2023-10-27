using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace VirtualWalletApp.Database
{
    public class DatabaseHelper
    {
        const string server = @"localhost";
        const string database = "VirtualWalletDB";
        private static string connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", server, database);

        // Select
        public static DataTable ExecuteQuery(string procedureName, List<SqlParameter> param)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;

                    if (param != null)
                    {
                        foreach (SqlParameter p in param)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }

                    // Utiliza un SqlDataReader para ejecutar el procedimiento almacenado que devuelve un conjunto de resultados
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update - Delete - Insert
        public static void ExecuteNonQuery(string procedureName, List<SqlParameter> param)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;

                    if (param != null)
                    {
                        foreach (SqlParameter p in param)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }

                    // Ejecuta el comando para procedimientos almacenados que no devuelven resultados
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Select con mapeo a lista del tipo T
        public static List<T> ExecuteQuery<T>(string procedureName, List<SqlParameter> param)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;

                    if (param != null)
                    {
                        foreach (SqlParameter p in param)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }

                    // Utiliza un SqlDataReader para ejecutar el procedimiento almacenado que devuelve un conjunto de resultados
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<T> result = new List<T>();

                        // Mapea los resultados del lector al tipo T
                        while (reader.Read())
                        {
                            T item = Activator.CreateInstance<T>();

                            foreach (var prop in typeof(T).GetProperties())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                                {
                                    prop.SetValue(item, reader[prop.Name], null);
                                }
                            }

                            result.Add(item);
                        }

                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
