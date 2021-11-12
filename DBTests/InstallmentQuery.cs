using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;
using PledgeFormApp.Server;
using PledgeFormApp.Server.Model;
using PledgeFormApp.Shared;

namespace DBTests
{
  [TestClass]
  public class InstallmentQueryTest
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
		InstallmentQuery query = new InstallmentQuery(db);
		try
		{
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Installment installment = result[0];
		  Assert.AreEqual(8, installment.Date.Month);
		  Assert.AreEqual(10, installment.Amount);
		  Assert.AreEqual(pledgerId, installment.PledgerId);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}

	[TestMethod]
	public async Task TestReadByDate()
	{
	  int pledgerId = initDB();
	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		InstallmentQuery query = new InstallmentQuery(db);
		try
		{
		  DateTime testDate = DateTime.Parse("2021-08-14");
		  var result = await query.ReadAllAsyncByDate(testDate);
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Installment installment = result[0];
		  Assert.AreEqual(8, installment.Date.Month);
		  Assert.AreEqual(10, installment.Amount);
		  Assert.AreEqual(pledgerId, installment.PledgerId);

		  testDate = testDate.AddDays(1);
		  result = await query.ReadAllAsyncByDate(testDate);
		  Assert.IsNotNull(result);
		  Assert.AreEqual(0, result.Count);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}


	[TestMethod]
	[Ignore]
	public async Task TestInsert()
	{
	  int pledgerId = initDB();

	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		InstallmentQuery query = new InstallmentQuery(db);
		try
		{
		  DateTime testDate = new DateTime(2021, 9, 1);
		  Installment donation = new Installment()
		  {
			Amount = 12,
			Date = testDate,
			PledgerId = pledgerId
		  };
		  int donationId = await query.InsertAsync(donation);
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(2, result.Count);
		  Assert.IsTrue(result.Any(d => d.Date == testDate));
		  Installment retDonation = result.FirstOrDefault(d => d.Date == testDate);
		  Assert.AreEqual(donationId, retDonation.ID);
		  Assert.AreEqual(donation.Date, retDonation.Date);
		  Assert.AreEqual(donation.Amount, retDonation.Amount);
		  Assert.AreEqual(donation.PledgerId, retDonation.PledgerId);
		}
		catch (Exception excp)
		{
		  Assert.Fail("Exception: " + excp.Message);
		}
	  }
	}

  }
}
