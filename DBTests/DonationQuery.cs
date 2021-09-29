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
  public class DonationQueryTest
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
		DonationQuery query = new DonationQuery(db);
		try
		{
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Donation donation = result[0];
		  Assert.AreEqual(8, donation.Date.Month);
		  Assert.AreEqual(10, donation.Amount);
		  Assert.AreEqual(pledgerId, donation.PledgerId);
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
		DonationQuery query = new DonationQuery(db);
		try
		{
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Donation donation = result[0];
		  Assert.AreEqual(new DateTime(2021, 8,14), donation.Date);
		  Assert.AreEqual(10, donation.Amount);

		  Donation retDonation = await query.ReadByIndexAsync(donation.ID);
		  Assert.AreEqual(donation.ID, retDonation.ID);
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

	[TestMethod]
	public async Task TestInsert()
	{
	  int pledgerId = initDB();

	  using (var db = new AppDb(TestConnectionString))
	  {
		await db.Connection.OpenAsync();
		DonationQuery dQuery = new DonationQuery(db);
		try
		{
		  DateTime testDate = new DateTime(2021, 9, 1);
		  Donation donation = new Donation() {
			Amount = 12,
			Date = testDate,
			PledgerId = pledgerId
		  };
		  int donationId = await dQuery.InsertAsync(donation);
		  var result = await dQuery.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(2, result.Count);
		  Assert.IsTrue(result.Any(d => d.Date == testDate));
		  Donation retDonation = result.FirstOrDefault(d => d.Date == testDate);
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
		DonationQuery dQuery = new DonationQuery(db);
		try
		{
		  int pledgerId = await query.InsertAsync(pledger);
		  Donation donation = new Donation()
		  {
			Amount = 12,
			Date = new DateTime(2021, 9, 1),
			PledgerId = pledgerId
		  };
		  int donationId = await dQuery.InsertAsync(donation);
		  Donation retDonation = await dQuery.ReadByIndexAsync(donationId);
		  Assert.AreEqual(donationId, retDonation.ID);
		  Assert.AreEqual(donation.Date, retDonation.Date);
		  Assert.AreEqual(donation.Amount, retDonation.Amount);
		  Assert.AreEqual(donation.PledgerId, retDonation.PledgerId);

		  Donation donationLater = new Donation()
		  {
			Amount = 12,
			Date = new DateTime(2021, 9, 1 + 7),
			PledgerId = pledgerId
		  };
		  int donationLaterId = await dQuery.InsertAsync(donationLater);
		  retDonation = await dQuery.ReadByIndexAsync(donationLaterId);
		  Assert.AreEqual(donationLaterId, retDonation.ID);
		  Assert.AreEqual(donationLater.Date, retDonation.Date);
		  Assert.AreEqual(donationLater.Amount, retDonation.Amount);
		  Assert.AreEqual(donationLater.PledgerId, retDonation.PledgerId);

		  retDonation = await dQuery.ReadByIndexAsync(donationId);
		  Assert.AreEqual(donationId, retDonation.ID);
		  Assert.AreEqual(donation.Date, retDonation.Date);
		  Assert.AreEqual(donation.Amount, retDonation.Amount);
		  Assert.AreEqual(donation.PledgerId, retDonation.PledgerId);

		}
		catch (AssertFailedException)
        {
		  throw;
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
		DonationQuery query = new DonationQuery(db);
		try
		{
		  var result = await query.ReadAllAsync();
		  Assert.IsNotNull(result);
		  Assert.AreEqual(1, result.Count);
		  Donation donation = result[0];
		  Assert.AreEqual(10, donation.Amount);

		  await query.DeleteAsync(donation.ID);
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
