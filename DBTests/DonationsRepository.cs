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
  public class DonationsRepositoryTest
  {
    const string TestConnectionString = "server=localhost;user id=garzooma;password=sts.varts;port=3306;database=TESTstsvarts;";

    [TestMethod]
    public void TestCtor()
    {
      DonationsRepository repository = new DonationsRepository(TestConnectionString);
    }

    [TestMethod]
    public void TestFindAll()
    {
      int pledgerId = DBTests.DonationQueryTest.initDB();
      DonationsRepository repository = new DonationsRepository(TestConnectionString);
      List<Donation> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(1, ret.Count);
      Donation donation = ret[0];
      Assert.AreEqual(10, donation.Amount);
      Assert.AreEqual(pledgerId, donation.PledgerId);
    }

    [TestMethod]
    public void TestFindByIndex()
    {
      int pledgerId = DBTests.DonationQueryTest.initDB();
      DonationsRepository repository = new DonationsRepository(TestConnectionString);
      List<Donation> ret = repository.FindAll().ToList();
      Assert.IsNotNull(ret);
      Assert.AreEqual(1, ret.Count);
      Donation donation = ret[0];
      Assert.AreEqual(10, donation.Amount);
      Donation retDonation = repository.FindByIndex(donation.ID);
      Assert.IsNotNull(retDonation);
      Assert.AreEqual(donation.Amount, retDonation.Amount);
    }

    [TestMethod]
    public void TestCreate()
    {
      int pledgerId = DBTests.DonationQueryTest.initDB();
      DonationsRepository repository = new DonationsRepository(TestConnectionString);
      List<Donation> donationsList = repository.FindAll().ToList();
      Assert.IsNotNull(donationsList);
      Assert.AreEqual(1, donationsList.Count);
      Donation donation = donationsList[0];

      Donation newDonation = new Donation()
      {
        Date = donation.Date.AddDays(7),
        Amount = 20,
        PledgerId = pledgerId
      };
      repository.Create(newDonation);
      donationsList = repository.FindAll().ToList();
      Assert.IsNotNull(donationsList);
      Assert.AreEqual(2, donationsList.Count);

      Donation testDonation = donationsList.FirstOrDefault(d => d.Date == newDonation.Date);
      Assert.IsNotNull(testDonation);
      Assert.AreEqual(newDonation.Amount, testDonation.Amount);
      Assert.AreEqual(newDonation.PledgerId, testDonation.PledgerId);
      Assert.IsTrue(testDonation.ID > 0);

    }

    [TestMethod]
    public void TestDelete()
    {
      int pledgerId = DBTests.DonationQueryTest.initDB();
      DonationsRepository repository = new DonationsRepository(TestConnectionString);
      List<Donation> donationsList = repository.FindAll().ToList();
      Assert.IsNotNull(donationsList);
      Assert.AreEqual(1, donationsList.Count);
      Donation donation = donationsList[0];

      repository.Delete(donation.ID);
      donationsList = repository.FindAll().ToList();
      Assert.IsNotNull(donationsList);
      Assert.AreEqual(0, donationsList.Count);

    }
  }
}
