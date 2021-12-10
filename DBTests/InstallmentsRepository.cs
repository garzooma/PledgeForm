using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PledgeFormApp.Server;
using PledgeFormApp.Shared;

namespace DBTests
{
  [TestClass]
  public class InstallmentsRepositoryTest
  {
    const string TestConnectionString = "server=localhost;user id=garzooma;password=sts.varts;port=3306;database=TESTstsvarts;";

    [TestMethod]
    public void TestCtor()
    {
      InstallmentsRepository repository = new InstallmentsRepository(TestConnectionString);
    }

    [TestMethod]
    public void TestFindAll()
    {
      int pledgerId = DBTests.InstallmentQueryTest.initDB();
      InstallmentsRepository repository = new InstallmentsRepository(TestConnectionString);
      List<Installment> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(1, ret.Count);
      Installment installment = ret[0];
      Assert.AreEqual(10, installment.Amount);
      Assert.AreEqual(pledgerId, installment.PledgerId);
    }
  }
}
