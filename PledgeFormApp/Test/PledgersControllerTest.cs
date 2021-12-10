using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PledgeFormApp.Server;
using PledgeFormApp.Server.Controllers;
using PledgeFormApp.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
  [TestClass]
  public class PledgersControllerTest
  {
    [TestMethod]
    public void TestCtor()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      Assert.IsNotNull(controller);
    }

    [TestMethod]
    public async Task TestRead()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      ActionResult<IEnumerable<Pledger>> result = await Task.Run(()=> controller.Get());
      List<Pledger> results = (((OkObjectResult)result.Result).Value as IEnumerable<Pledger>).ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.ToList().Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);
    }

    [TestMethod]
    public async void TestReadByIndex()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      ActionResult<IEnumerable<Pledger>> result = await controller.Get();
      List<Pledger> results = result.Value.ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      ActionResult<Pledger> result2 = await controller.Get(pledger.ID);
      Pledger retPledger = result2.Value;
      Assert.IsNotNull(retPledger);
      Assert.AreEqual(pledger.ID, retPledger.ID);
      Assert.AreEqual(pledger.Name, retPledger.Name);
      Assert.AreEqual(pledger.Amount, retPledger.Amount);
      Assert.AreEqual(pledger.QBName, retPledger.QBName);
    }

    [TestMethod]
    public async void TestCreate()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      ActionResult<IEnumerable<Pledger>> result = await controller.Get();
      List<Pledger> results = result.Value.ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      Pledger newPledger = new Pledger() { Name = "New Pledger" };
      await controller.Create(newPledger);
      result = await controller.Get();
      results = result.Value.ToList();
      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Any(p => p.Name == "New Pledger"));

    }

    [TestMethod]
    public async void TestDelete()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      ActionResult<IEnumerable<Pledger>> result = await controller.Get();
      List<Pledger> results = result.Value.ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      Pledger newPledger = new Pledger() { Name = "New Pledger" };
      await controller.Create(newPledger);
      result = await controller.Get();
      results = result.Value.ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(2, results.Count);
      Assert.IsTrue(results.Any(p => p.Name == "New Pledger"));
      Pledger returnedPledger = results.FirstOrDefault(p => p.Name == newPledger.Name);

      await controller.Delete(returnedPledger.ID);
      result = await controller.Get();
      results = result.Value.ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

    }

    [TestMethod]
    public async void TestUpdate()
    {
      PledgersController controller = new PledgersController(new TestRepository());
      ActionResult<IEnumerable<Pledger>> result = await controller.Get();
      List<Pledger> results = result.Value.ToList();
      Assert.IsNotNull(results);
      Assert.AreEqual(1, results.Count);
      Pledger pledger = results.First();
      Assert.AreEqual("Test Name", pledger.Name);

      Pledger newPledger = new Pledger() { Name = "New Pledger", Amount=25 };
      await controller.Create(newPledger);
      result = await controller.Get();
      results = result.Value.ToList();
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
      await controller.Update(modifiedPledger);

      ActionResult<Pledger> result2 = await controller.Get(pledger.ID);
      Pledger retPledger = result2.Value;
      Assert.AreEqual(modifiedPledger.ID, pledger.ID);
      Assert.AreEqual(modifiedPledger.Name, pledger.Name);
      Assert.AreEqual(modifiedPledger.Amount, pledger.Amount);

    }

    internal class TestRepository : IPledgersRepository
    {
      private int nextID = 1;
      private readonly List<Pledger> pledgersList = new List<Pledger>();

      public TestRepository()
      {
        pledgersList.Add(new Pledger() {
          ID = nextID++,
          Name = "Test Name",
          Amount = 123
        });
      }

      public int Create(Pledger pledger)
      {
        Pledger newPledger = new Pledger()
        {
          ID = nextID++,
          Name = pledger.Name,
          Amount = pledger.Amount,
          QBName = pledger.QBName
        };
        pledgersList.Add(newPledger);
        return newPledger.ID;
      }

      public void Delete(int index)
      {
        Pledger existingPledger = pledgersList.FirstOrDefault(p => p.ID == index);
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
