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

		public static Tuple<int, int> initDB()
		{
			int pledgerId = -1;
			int pledgerId2 = -1;
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
						cmd.CommandText = @"insert into  pledgers (name, weeklyamount, qbname) values ('test name2', '22', 'qbtest2')";
						cmd.ExecuteNonQuery();
						cmd.CommandText = @"select id, name from pledgers";
						using (DbDataReader dbDataReader = cmd.ExecuteReader())
						{
              while (dbDataReader.Read()) {
								string name = dbDataReader.GetString(1);
								if (name == "test name") pledgerId = dbDataReader.GetInt32(0);
								if (name == "test name2") pledgerId2 = dbDataReader.GetInt32(0);
							}
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

			return new Tuple<int, int>(pledgerId, pledgerId2);
		}

		[TestMethod]
		public async Task TestRead()
		{
			int pledgerId = initDB().Item1;
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

		[TestMethod]
		public async Task TestReadByIndex()
		{
			Tuple<int, int> intRet = initDB();
			int pledgerId = intRet.Item1;
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

					int index = Envelope.GetIndex(envelope.Year, envelope.EnvelopeNum);
					Envelope retEnvelope = await query.ReadByIndexAsync(index);
					Assert.AreEqual(envelope.PledgerId, retEnvelope.PledgerId);
					Assert.AreEqual(envelope.EnvelopeNum, retEnvelope.EnvelopeNum);
					Assert.AreEqual(envelope.Year, retEnvelope.Year);
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
			Tuple<int, int> pledgersList = initDB();
			int pledgerId = pledgersList.Item1;
			int pledgerId2 = pledgersList.Item2;

			using (var db = new AppDb(TestConnectionString))
			{
				await db.Connection.OpenAsync();
				EnvelopeQuery dQuery = new EnvelopeQuery(db);
				try
				{
					Envelope envelope = new Envelope()
					{
						PledgerId = pledgerId2,
						EnvelopeNum = 2,
						Year = 2021
					};
					await dQuery.InsertAsync(envelope);
					var result = await dQuery.ReadAllAsync();
					Assert.IsNotNull(result);
					Assert.AreEqual(2, result.Count);
					Envelope retEnvelope = result.FirstOrDefault(e => e.PledgerId == pledgerId2 && e.Year == 2021);
					Assert.IsNotNull(retEnvelope);
					Assert.AreEqual(envelope.Year, retEnvelope.Year);
					Assert.AreEqual(envelope.EnvelopeNum, retEnvelope.EnvelopeNum);
					Assert.AreEqual(envelope.PledgerId, retEnvelope.PledgerId);
				}
				catch (Exception excp)
				{
					Assert.Fail("Exception: " + excp.Message);
				}
			}
		}
	}
}
