using Microsoft.VisualStudio.TestTools.UnitTesting;
using PledgeFormApp.Server;
using PledgeFormApp.Server.Controllers;
using PledgeFormApp.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
  [TestClass]
  public class PledgersControllerTest
  {
    [TestMethod]
    public void TestCtor()
    {
      PledgersController controller = new PledgersController(new TestRepository());
    }

    [TestMethod]
    public void TestRead()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      List<Pledger> results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);
    }

    [TestMethod]
    public void TestReadByIndex()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      List<Pledger> results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      Pledger retPledger = controller.Get(pledger.ID);
      Assert.IsNotNull(retPledger);
      Assert.AreEqual(pledger.ID, retPledger.ID);
      Assert.AreEqual(pledger.Name, retPledger.Name);
      Assert.AreEqual(pledger.Amount, retPledger.Amount);
      Assert.AreEqual(pledger.QBName, retPledger.QBName);
    }

    [TestMethod]
    public void TestCreate()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      List<Pledger> results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      Pledger newPledger = new Pledger() { Name = "New Pledger" };
      controller.Create(newPledger);
      results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Any(p => p.Name == "New Pledger"));

    }

    [TestMethod]
    public void TestDelete()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      List<Pledger> results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      Pledger newPledger = new Pledger() { Name = "New Pledger" };
      controller.Create(newPledger);
      results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Any(p => p.Name == "New Pledger"));
      Pledger returnedPledger = results.FirstOrDefault(p => p.Name == newPledger.Name);

      controller.Delete(returnedPledger);
      results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

    }

    [TestMethod]
    public void TestUpdate()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      List<Pledger> results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      Pledger newPledger = new Pledger() { Name = "New Pledger", Amount=25 };
      controller.Create(newPledger);
      results = controller.Get().ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Any(p => p.Name == "New Pledger"));
      Pledger returnedPledger = results.FirstOrDefault(p => p.Name == "New Pledger");
      Assert.AreEqual(newPledger.Name, returnedPledger.Name);
      Assert.AreEqual(newPledger.Amount, returnedPledger.Amount);

      Pledger modifiedPledger = new Pledger()
      {
        ID = returnedPledger.ID,
        Name = returnedPledger.Name,
        Amount = returnedPledger.Amount + 10,
        QBName = returnedPledger.QBName
      };
      controller.Update(modifiedPledger);
      pledger = controller.Get(modifiedPledger.ID);
      Assert.AreEqual(modifiedPledger.ID, pledger.ID);
      Assert.AreEqual(modifiedPledger.Name, pledger.Name);
      Assert.AreEqual(modifiedPledger.Amount, pledger.Amount);

    }

    internal class TestRepository : IPledgersRepository
    {
      private int nextID = 1;
      private List<Pledger> pledgersList = new List<Pledger>();

      public TestRepository()
      {
        pledgersList.Add(new Pledger() {
          ID = nextID++,
          Name = "Test Name",
          Amount = 123
        });
      }

      public void Create(Pledger pledger)
      {
        Pledger newPledger = new Pledger()
        {
          ID = nextID++,
          Name = pledger.Name,
          Amount = pledger.Amount,
          QBName = pledger.QBName
        };
        pledgersList.Add(newPledger);
      }

      public void Delete(Pledger pledger)
      {
        Pledger existingPledger = pledgersList.FirstOrDefault(p => p.ID == pledger.ID);
        pledgersList.Remove(existingPledger);
      }

      public IEnumerable<Pledger> FindAll()
      {
        return pledgersList;
      }

      public Pledger FindByIndex(int index)
      {
        return pledgersList.FirstOrDefault(p=>p.ID == index);
      }

      public void Update(Pledger pledger)
      {
        Pledger existingPledger = pledgersList.FirstOrDefault(p => p.ID == pledger.ID);
        pledgersList.Remove(existingPledger);
        pledgersList.Add(pledger);
      }
    }
  }
}
