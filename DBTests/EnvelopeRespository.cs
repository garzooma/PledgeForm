using Microsoft.VisualStudio.TestTools.UnitTesting;
using PledgeFormApp.Server;
using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTests
{
  [TestClass]
  public class EnvelopeRespositoryTest
  {
    const string TestConnectionString = "server=localhost;user id=garzooma;password=sts.varts;port=3306;database=TESTstsvarts;";

    [TestMethod]
    public void TestCtor()
    {
      EnvelopesRepository repository = new EnvelopesRepository(TestConnectionString);
    }

    [TestMethod]
    public void TestFindAll()
    {
      Tuple<int, int> result = DBTests.EnvelopeQueryTest.initDB();
      int pledgerId = result.Item1;
      EnvelopesRepository repository = new EnvelopesRepository(TestConnectionString);
      List<Envelope> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(1, ret.Count);
      Envelope envelope = ret[0];
      Assert.AreEqual(1, envelope.EnvelopeNum);
      Assert.AreEqual(pledgerId, envelope.PledgerId);
    }

    [TestMethod]
    public void TestCreate()
    {
      Tuple<int, int> result = DBTests.EnvelopeQueryTest.initDB();
      int pledgerId = result.Item1;
      EnvelopesRepository repository = new EnvelopesRepository(TestConnectionString);
      List<Envelope> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(1, ret.Count);

      PledgersRepository pledgerRepository = new PledgersRepository(TestConnectionString);
      Pledger newPledger = new Pledger()
      {
        Name = "New Pledger",
        Amount = 234,
        QBName = "New QBName"
      };
      pledgerRepository.Create(newPledger);
      List<Pledger> pledgerList = pledgerRepository.FindAll().ToList();
      Assert.IsNotNull(pledgerList);
      Assert.AreEqual(3, pledgerList.Count);
      Pledger testPledger = pledgerList.FirstOrDefault(p => p.Name == newPledger.Name);
      Assert.IsNotNull(testPledger);

      Envelope newEnvelope = new Envelope()
      {
        EnvelopeNum = 2,
        PledgerId = testPledger.ID,
        Year = 2021
      };

      repository.Create(newEnvelope);
      List<Envelope> envelopeList = repository.FindAll().ToList();
      Assert.AreEqual(2, envelopeList.Count);
      Envelope testEnvelope = envelopeList.FirstOrDefault(e => e.EnvelopeNum == 2);
      Assert.IsNotNull(testEnvelope);
      Assert.AreEqual(newEnvelope.PledgerId, testEnvelope.PledgerId);
      Assert.AreEqual(newEnvelope.EnvelopeNum, testEnvelope.EnvelopeNum);
      Assert.AreEqual(newEnvelope.Year, testEnvelope.Year);

      return;
    }
  }
}
