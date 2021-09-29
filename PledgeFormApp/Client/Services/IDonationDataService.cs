using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Client.Services
{
  interface IDonationDataService
  {
    Task<IEnumerable<Donation>> GetAllDonations();
    Task<Donation> GetDonationDetails(int donationId);
    Task AddDonation(Donation donation);
    Task DeleteDonation(int donationId);
  }
}
