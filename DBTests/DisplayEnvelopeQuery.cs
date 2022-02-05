using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;
using PledgeFormApp.Shared;
using PledgeFormApp.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using PledgeFormApp.Server;

namespace DBTests
{
  [TestClass]
  public class DisplayEnvelopeQueryTest
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
						cmd.CommandText = @"insert into  pledgers (name, weeklyamount, qbname) values ('test name2', '22', 'qbtest2')";
						cmd.ExecuteNonQuery();
						cmd.CommandText = @"select id, name from pledgers";
						using (DbDataReader dbDataReader = cmd.ExecuteReader())
						{
							while (dbDataReader.Read())
							{
								string name = dbDataReader.GetString(1);
								if (name == "test name") pledgerId = dbDataReader.GetInt32(0);
								//if (name == "test name2") pledgerId2 = dbDataReader.GetInt32(0);
							}
						}
						//cmd.CommandText = string.Format("insert into pledgedonations (date, amount, pledger) values ('2021-08-14', '10', {0})", pledgerId);
						//cmd.ExecuteNonQuery();
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
				DisplayEnvelopeQuery query = new DisplayEnvelopeQuery(db);
				try
				{
					var result = await query.ReadAllAsync();
					Assert.IsNotNull(result);
					Assert.AreEqual(1, result.Count);
					DisplayEnvelope envelope = result[0];
					Assert.AreEqual(pledgerId, envelope.PledgerId);
					Assert.AreEqual(1, envelope.EnvelopeNum);
					Assert.AreEqual(2021, envelope.Year);
					Assert.AreEqual("test name", envelope.Pledger.Name);
					Assert.AreEqual(123, envelope.Pledger.Amount);
					Assert.AreEqual("qbtest", envelope.Pledger.QBName);
				}
				catch (Exception excp)
				{
					Assert.Fail("Exception: " + excp.Message);
				}
			}
		}

		[TestMethod]
		public async Task TestReadById()
		{
			int pledgerId = initDB();
			using (var db = new AppDb(TestConnectionString))
			{
				await db.Connection.OpenAsync();
				DisplayEnvelopeQuery query = new DisplayEnvelopeQuery(db);
				try
				{
					int envelopeNum = 1;
					int year = 2021;
					int id = Envelope.GetIndex(year, envelopeNum);
					DisplayEnvelope envelope = await query.ReadByIndexAsync(id);
					Assert.IsNotNull(envelope);
					Assert.AreEqual(pledgerId, envelope.PledgerId);
					Assert.AreEqual(1, envelope.EnvelopeNum);
					Assert.AreEqual(2021, envelope.Year);
					Assert.AreEqual("test name", envelope.Pledger.Name);
					Assert.AreEqual(123, envelope.Pledger.Amount);
					Assert.AreEqual("qbtest", envelope.Pledger.QBName);
				}
				catch (Exception excp)
				{
					Assert.Fail("Exception: " + excp.Message);
				}
			}
		}
	}
}
