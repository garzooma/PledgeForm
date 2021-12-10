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
  }
}
