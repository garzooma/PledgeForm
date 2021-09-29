using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;
using PledgeFormApp.Server;
using PledgeFormApp.Shared;
using PledgeFormApp.Server.Model;

namespace DBTests
{
  [TestClass]
  public class EnvelopeQueryTest
  {
    public static readonly string TestConnectionString = "server=localhost;user id=garzooma;password=sts.varts;port=3306;database=TESTstsvarts;";

    [TestMethod]
    public void TestCtor()
    {
      AppDb db = new AppDb(TestConnectionString);

      return;
    }

	public static int initDB()
	{
	  int pledgerId = -1;
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
			cmd.CommandText = @"delete FROM pledgedonations";
			cmd.ExecuteNonQuery();
			cmd.CommandText = @"delete FROM envelopes";
			cmd.ExecuteNonQuery();
		  }

		  using (DbCommand cmd = conn.CreateCommand())
		  {
			cmd.CommandText = @"insert into  pledgers (name, weeklyamount, qbname) values ('test name', '123', 'qbtest')";
			cmd.ExecuteNonQuery();
			cmd.CommandText = @"select id from pledgers";
			using (DbDataReader dbDataReader = cmd.ExecuteReader())
			{
			  dbDataReader.Read();
			  pledgerId = dbDataReader.GetInt32(0);
			}
			cmd.CommandText = string.Format("insert into pledgedonations (date, amount, pledger) values ('2021-08-14', '10', {0})", pledgerId);
			cmd.ExecuteNonQuery();
			cmd.CommandText = string.Format("INSERT INTO `envelopes` (`pledgerId`, `envelopeNum`, `year`) VALUES ({0}, 1, 2021)", pledgerId);
			cmd.ExecuteNonQuery();
		  }
		}
		catch (Exception excp)
		{
		  throw new Exception("Bad sql: " + excp.Message);
		}
		finally
		{
		  conn.Close();
		}
	  }

	  return pledgerId;
	}

	[TestMethod]
	public async Task TestRead()
	{
	  int pledgerId = initDB();
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		EnvelopeQuery query = new EnvelopeQuery(db);
		try
		{
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Envelope envelope = result[0];
		  Assert.AreEqual(pledgerId, envelope.PledgerId);
		  Assert.AreEqual(1, envelope.EnvelopeNum);
		  Assert.AreEqual(2021, envelope.Year);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}
  }
}
