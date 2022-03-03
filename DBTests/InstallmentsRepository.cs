using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PledgeFormApp.Server;
using PledgeFormApp.Shared;
using MySqlConnector;using System.Data.Common;

namespace DBTests
{
  [TestClass]
  public class InstallmentsRepositoryTest
  {
    const string TestConnectionString = "server=localhost;user id=garzooma;password=sts.varts;port=3306;database=TESTstsvarts;";


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
						cmd.CommandText = @"insert into  pledgers (name, weeklyamount, qbname) values ('test name', '5', 'qbtest')";
						cmd.ExecuteNonQuery();
						cmd.CommandText = @"select id from pledgers";
						using (DbDataReader dbDataReader = cmd.ExecuteReader())
						{
							dbDataReader.Read();
							pledgerId = dbDataReader.GetInt32(0);
						}
						cmd.CommandText = @"insert into  pledgers (name, weeklyamount, qbname) values ('test name2', '15', 'qbtest2')";
						cmd.ExecuteNonQuery();
						cmd.CommandText = @"select id from pledgers where name = 'test name2'";
						int pledgerId2;
						using (DbDataReader dbDataReader = cmd.ExecuteReader())
						{
							dbDataReader.Read();
							pledgerId2 = dbDataReader.GetInt32(0);
						}

						cmd.CommandText = string.Format("INSERT INTO `envelopes` (`pledgerId`, `envelopeNum`, `year`) VALUES ({0}, 1, 2021)", pledgerId);
						cmd.ExecuteNonQuery();

						cmd.CommandText = string.Format("INSERT INTO `envelopes` (`pledgerId`, `envelopeNum`, `year`) VALUES ({0}, 2, 2021)", pledgerId2);
						cmd.ExecuteNonQuery();

						cmd.CommandText = string.Format("insert into pledgedonations (date, amount, pledger) values ('2021-08-14', '5', {0})", pledgerId);
						cmd.ExecuteNonQuery();

						cmd.CommandText = string.Format("insert into pledgedonations (date, amount, pledger) values ('2021-08-14', '30', {0})", pledgerId2);
						cmd.ExecuteNonQuery();

						cmd.CommandText = string.Format("insert into pledgedonations (date, amount, pledger) values ('2021-08-21', '5', {0})", pledgerId);
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
    public void TestCtor()
    {
      InstallmentsRepository repository = new InstallmentsRepository(TestConnectionString);
    }

    [TestMethod]
    public void TestFindAll()
    {
      int pledgerId = initDB();
      InstallmentsRepository repository = new InstallmentsRepository(TestConnectionString);
      List<Installment> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
			Assert.AreEqual(3, ret.Count);
			//Installment installment = result[0];
			Installment installment = ret.FirstOrDefault(i => i.Date == new DateTime(2021, 8, 14) && i.EnvelopeNumber == 1);
			Assert.IsNotNull(installment);
			Assert.AreEqual(8, installment.Date.Month);
			Assert.AreEqual(5, installment.Amount);
			Assert.AreEqual(pledgerId, installment.PledgerId);
			Assert.AreEqual(2021, installment.Year);
		}

		[TestMethod]
    public void TestFindByDates()
    {
      int pledgerId = initDB();
      InstallmentsRepository repository = new InstallmentsRepository(TestConnectionString);
			DateTime testDate = new DateTime(2021, 8, 14);
      List<Installment> ret = repository.FindByDates(testDate, testDate.AddDays(7)).ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(3, ret.Count);
			Installment installment = ret.FirstOrDefault(i => i.Date == new DateTime(2021, 8, 14) && i.EnvelopeNumber == 1);
			Assert.IsNotNull(installment);
			Assert.AreEqual(8, installment.Date.Month);
			Assert.AreEqual(5, installment.Amount);
			Assert.AreEqual(pledgerId, installment.PledgerId);
			Assert.AreEqual(2021, installment.Year);
		}

		[TestMethod]
		public void TestFindByYear()
		{
			int pledgerId = initDB();
			InstallmentsRepository repository = new InstallmentsRepository(TestConnectionString);
			DateTime testDate = new DateTime(2021, 8, 14);
			List<Installment> ret = repository.FindByYear(2021).ToList();
			Assert.IsNotNull(ret);
			Assert.AreEqual(3, ret.Count);
			//Installment installment = result[0];
			Installment installment = ret.FirstOrDefault(i => i.Date == new DateTime(2021, 8, 14) && i.EnvelopeNumber == 1);
			Assert.IsNotNull(installment);
			Assert.AreEqual(8, installment.Date.Month);
			Assert.AreEqual(5, installment.Amount);
			Assert.AreEqual(pledgerId, installment.PledgerId);
			Assert.AreEqual(2021, installment.Year);

			ret = repository.FindByYear(2022).ToList();
			Assert.IsNotNull(ret);
			Assert.AreEqual(0, ret.Count);

		}
	}
}
