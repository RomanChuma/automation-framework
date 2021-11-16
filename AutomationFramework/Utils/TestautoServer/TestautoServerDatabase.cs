
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utils.TestautoServer.Models.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AutomationFramework.Core.Utils.TestautoServer
{
	public class TestautoServerDatabase
	{
		private readonly string _server;
		private readonly MySqlConnection _dbConn;

		public TestautoServerDatabase(string database, string user, string password)
		{
			_server = GlobalSettings.Framework.DatabaseServer;
			_dbConn = ConnectToDb(database, user, password);
		}

		~TestautoServerDatabase()
		{
			if (_dbConn.State != ConnectionState.Closed)
			{
				CloseConnection();
			}
		}

		private MySqlConnection ConnectToDb(string database, string user, string password)
		{
			string connectionStr = $"Server={_server};Database={database};Uid={user};Pwd={password};";
			
			return new MySqlConnection(connectionStr);
		}

		private MySqlCommand PrepareCommand(SqlCommandBuilder sqlCmdBuilder)
		{
			MySqlCommand sql = null;
			try
			{
				if (_dbConn.State != ConnectionState.Open)
				{
					_dbConn.Open();
				}

				if (_dbConn.State == ConnectionState.Open)
				{
					sql = new MySqlCommand(sqlCmdBuilder.ToString(), _dbConn);
					foreach (var tuple in sqlCmdBuilder.Parameters)
					{
						sql.Parameters.AddWithValue(tuple.Key, tuple.Value);
					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return sql;
		}

		public void CloseConnection()
		{
			_dbConn.Close();
		}

		public long ExecuteNonQueryAndReturnLastId(SqlCommandBuilder sqlCmdBuilder)
		{
			var sql = PrepareCommand(sqlCmdBuilder);
			sql.ExecuteNonQuery();
			return sql.LastInsertedId; // TODO : this is not thread safe, should try to figure a way to get this another way
		}


		/// <summary>
		///	Execute command
		/// </summary>
		/// <returns>
		/// The amount of line affected by the command
		/// </returns>
		public int ExecuteNonQuery(SqlCommandBuilder sqlCmdBuilder)
		{
			var sql = PrepareCommand(sqlCmdBuilder);
			return sql.ExecuteNonQuery();
		}

		/// <summary>
		/// Execute command with return value
		/// </summary>
		/// <returns>
		/// Return a list of T
		/// </returns>
		public List<T> ExecuteReader<T>(SqlCommandBuilder sqlCmdBuilder) where T : class, new()
		{
			List<T> result = new List<T>();
			var sql = PrepareCommand(sqlCmdBuilder);
			
			var reader = sql.ExecuteReader();

			while (reader.Read())
			{
				var entry = MySqlDataReaderMapping.ReflectType<T>(reader);
				result.Add(entry);
			}

			reader.Close();


			return result;
		}

		/// <summary>
		/// Execute command with return value
		/// </summary>
		/// <returns>
		/// A list of object array
		/// </returns>
		public List<object[]> ExecuteReader(SqlCommandBuilder sqlCmdBuilder)
		{
			List<object[]> result = new List<object[]>();
			var sql = PrepareCommand(sqlCmdBuilder);

			var reader = sql.ExecuteReader();

			while (reader.Read())
			{
				var entry = new object[reader.FieldCount];
				reader.GetValues(entry);
				result.Add(entry);
			}

			reader.Close();

			return result;
		}
	}
}
