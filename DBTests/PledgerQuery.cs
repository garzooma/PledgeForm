using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;
using PledgeFormApp.Server;
using PledgeFormApp.Server.Model;
using PledgeFormApp.Shared;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DBTests
{
  [TestClass]
  public class PledgerQueryTest
  {
    public static readonly string TestConnectionString = "server=localhost;user id=garzooma;password=sts.varts;port=3306;database=TESTstsvarts;";

	[TestMethod]
	public void TestCtor()
    {
      AppDb db = new AppDb(TestConnectionString);

	}

	public static void initDB()
    {
	  string[] connParts = TestConnectionString.Split(";");
	  string[] dbParts = connParts[4].Split("=");
	  string dbName = dbParts[1];
	  if (!dbName.StartsWith("TEST"))
      {
		throw new Exception("Not a test db");
      }
	  using (MySqlConnection conn = new MySqlConnection(TestConnectionString))
      {
		try
        {
		  conn.Open();
		  using (DbCommand cmd = conn.CreateCommand())
		  {
			cmd.CommandText = @"delete FROM pledgers";
			cmd.ExecuteNonQuery();
		  }

		  using (DbCommand cmd = conn.CreateCommand())
		  {
			cmd.CommandText = @"insert into  pledgers (name, weeklyamount, qbname) values ('test name', '123', 'qbtest')";
			cmd.ExecuteNonQuery();
		  }
		} 
		catch(Exception excp)
        {
		  throw new Exception("Bad sql: " + excp.Message);
        }
        finally
        {
		  conn.Close();
        }
	  }
    }

    [TestMethod]
    public async Task TestRead()
    {
	  initDB();
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		PledgerQuery query = new PledgerQuery(db);
		try {
		  var result = await query.ReadAllAsync(); 
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Pledger pledger = result[0];
		  Assert.AreEqual("test name", pledger.Name);
		  Assert.AreEqual(123, pledger.Amount);
		  Assert.AreEqual("qbtest", pledger.QBName);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
        }
	  }
	}

	[TestMethod]
	public async Task TestReadByIndex()
    {
	  initDB();
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		PledgerQuery query = new PledgerQuery(db);
		try {
		  var result = await query.ReadAllAsync(); 
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Pledger pledger = result[0];
		  Assert.AreEqual("test name", pledger.Name);
		  Assert.AreEqual(123, pledger.Amount);
		  Assert.AreEqual("qbtest", pledger.QBName);

		  Pledger retPledger = await query.ReadByIndexAsync(pledger.ID);
		  Assert.AreEqual(pledger.ID, retPledger.ID);
		  Assert.AreEqual(pledger.Name, retPledger.Name);
		  Assert.AreEqual(pledger.Amount, retPledger.Amount);
		  Assert.AreEqual(pledger.QBName, retPledger.QBName);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
        }
	  }
    }

	[TestMethod]
	public async Task TestInsert()
	{
	  initDB();
	  Pledger pledger = new Pledger()
	  {
		Name = "test2",
		Amount = 345,
		QBName = "qbtest"
	  };
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		PledgerQuery query = new PledgerQuery(db);
		try
		{
		  await query.InsertAsync(pledger);
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(2, result.Count);
		  Pledger ret_pledger = result.Find(p => p.Name == "test2");
		  Assert.IsNotNull(ret_pledger);
		  Assert.AreEqual("test2", ret_pledger.Name);
		  Assert.AreEqual(345, ret_pledger.Amount);
		  Assert.AreEqual("qbtest", ret_pledger.QBName);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}

	[TestMethod]
	public async Task TestInsertRead()
	{
	  initDB();
	  Pledger pledger = new Pledger()
	  {
		Name = "test2",
		Amount = 345,
		QBName = "qbtest"
	  };
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		PledgerQuery query = new PledgerQuery(db);
		try
		{
		  await query.InsertAsync(pledger);
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(2, result.Count);
		  Pledger ret_pledger = result.Find(p => p.Name == "test2");
		  Assert.IsNotNull(ret_pledger);
		  Assert.AreEqual("test2", ret_pledger.Name);
		  Assert.AreEqual(345, ret_pledger.Amount);
		  Assert.AreEqual("qbtest", ret_pledger.QBName);

		  Pledger ret_pledger2 = await query.ReadByIndexAsync(ret_pledger.ID);
		  Assert.AreEqual(ret_pledger.ID, ret_pledger2.ID);
		  Assert.AreEqual(ret_pledger.Name, ret_pledger2.Name);
		  Assert.AreEqual(ret_pledger.Amount, ret_pledger2.Amount);
		  Assert.AreEqual(ret_pledger.QBName, ret_pledger2.QBName);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}

	[TestMethod]
	public async Task TestUpdate()
    {
	  initDB();
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		PledgerQuery query = new PledgerQuery(db);
		try
		{
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Pledger pledger = result[0];
		  Assert.AreEqual("test name", pledger.Name);
		  Assert.AreEqual(123, pledger.Amount);
		  Assert.AreEqual("qbtest", pledger.QBName);

		  Pledger updatedPledger = new Pledger()
		  {
			ID = pledger.ID,
			Name = pledger.Name + "X",
			Amount = pledger.Amount + 1,
			QBName = pledger.QBName + "Y"
		  };
		  await query.UpdateAsync(updatedPledger);

		  result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  pledger = result[0];
		  Assert.AreEqual(updatedPledger.Name, pledger.Name);
		  Assert.AreEqual(updatedPledger.Amount, pledger.Amount);
		  Assert.AreEqual(updatedPledger.QBName, pledger.QBName);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}

	[TestMethod]
	public async Task TestDelete()
    {
	  initDB();
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		PledgerQuery query = new PledgerQuery(db);
		try
		{
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Pledger pledger = result[0];
		  Assert.AreEqual("test name", pledger.Name);
		  Assert.AreEqual(123, pledger.Amount);
		  Assert.AreEqual("qbtest", pledger.QBName);

		  await query.DeleteAsync(pledger.ID);
		  result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(0, result.Count);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}
  }
}
