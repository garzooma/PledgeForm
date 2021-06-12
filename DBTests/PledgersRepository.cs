using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PledgeFormApp.Server;
using PledgeFormApp.Shared;

namespace DBTests
{
  [TestClass]
  public class PledgersRepositoryTest
  {
    const string TestConnectionString = "server=localhost;user id=garzooma;password=sts.varts;port=3306;database=TESTstsvarts;";

    [TestMethod]
    public void TestCtor()
    {
      PledgersRepository repository = new PledgersRepository(TestConnectionString);
    }

    [TestMethod]
    public void TestFindAll()
    {
      DBTests.PledgerQueryTest.initDB();
      PledgersRepository repository = new PledgersRepository(TestConnectionString);
      List<Pledger> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(1, ret.Count);
      Pledger pledger = ret[0];
      Assert.AreEqual("test name", pledger.Name);
    }

    [TestMethod]
    public void TestFindByIndex()
    {
      DBTests.PledgerQueryTest.initDB();
      PledgersRepository repository = new PledgersRepository(TestConnectionString);
      List<Pledger> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(1, ret.Count);
      Pledger pledger = ret[0];
      Assert.AreEqual("test name", pledger.Name);
      Pledger retPledger = repository.FindByIndex(pledger.ID);
      Assert.IsNotNull(retPledger);
      Assert.AreEqual(pledger.Name, retPledger.Name);
    }

    [TestMethod]
    public void TestCreate()
    {
      DBTests.PledgerQueryTest.initDB();
      PledgersRepository repository = new PledgersRepository(TestConnectionString);
      List<Pledger> pledgerList = repository.FindAll().ToList();
      Assert.IsNotNull(pledgerList);
      Assert.AreEqual(1, pledgerList.Count);

      Pledger newPledger = new Pledger()
      {
        Name = "New Pledger",
        Amount = 234,
        QBName = "New QBName"
      };
      repository.Create(newPledger);
      pledgerList = repository.FindAll().ToList();
      Assert.IsNotNull(pledgerList);
      Assert.AreEqual(2, pledgerList.Count);

      Pledger testPledger = pledgerList.FirstOrDefault(p => p.Name == newPledger.Name);
      Assert.IsNotNull(testPledger);
      Assert.AreEqual(newPledger.Name, testPledger.Name);
      Assert.AreEqual(newPledger.Amount, testPledger.Amount);
      Assert.AreEqual(newPledger.QBName, testPledger.QBName);
      Assert.IsTrue(testPledger.ID > 0);

    }
  

    [TestMethod]
    public void TestDelete()
    {
      DBTests.PledgerQueryTest.initDB();
      PledgersRepository repository = new PledgersRepository(TestConnectionString);
      List<Pledger> pledgerList = repository.FindAll().ToList();
      Assert.IsNotNull(pledgerList);
      Assert.AreEqual(1, pledgerList.Count);

      Pledger pledger = pledgerList[0];

      repository.Delete(pledger);
      pledgerList = repository.FindAll().ToList();
      Assert.IsNotNull(pledgerList);
      Assert.AreEqual(0, pledgerList.Count);

    }

    [TestMethod]
    public void TestUpdate()
    {
      DBTests.PledgerQueryTest.initDB();
      PledgersRepository repository = new PledgersRepository(TestConnectionString);
      List<Pledger> pledgerList = repository.FindAll().ToList();
      Assert.IsNotNull(pledgerList);
      Assert.AreEqual(1, pledgerList.Count);

      Pledger pledger = pledgerList[0];
      int oldAmount = pledger.Amount;
      pledger.Amount += 10;

      repository.Update(pledger);
      pledgerList = repository.FindAll().ToList();
      Assert.IsNotNull(pledgerList);
      Assert.AreEqual(1, pledgerList.Count);
      Pledger updatedPledger = pledgerList[0];
      Assert.AreEqual(oldAmount + 10, pledger.Amount);

    }
  }
}
