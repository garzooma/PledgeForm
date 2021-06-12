using MySqlConnector;
using System;

namespace PledgeFormApp.Server
{
  public class AppDb : IDisposable
  {

	public MySqlConnection Connection;

	public AppDb() : this (AppConfig.Config["Data:ConnectionString"])
	{
	}

	public AppDb(string connectionString)
	{
	  Connection = new MySqlConnection(connectionString);
	}

	public void Dispose()
	{
	  Connection.Close();
	}
  }
}