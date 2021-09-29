using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PledgeFormApp.Client.Services
{
  public class DonationDataService : IDonationDataService
  {
    private readonly HttpClient _client;
    public DonationDataService(HttpClient client)
    {
      _client = client;
    }

    public async Task AddDonation(Donation donation)
    {
      HttpResponseMessage response = await _client.PostAsJsonAsync<Donation>("/donations/create", donation);
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
      Donation retVal = JsonSerializer.Deserialize<Donation>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return;
    }

    public Task DeleteDonation(int donationId)
    {
      throw new NotImplementedException();
    }

    public async Task<IEnumerable<Donation>> GetAllDonations()
    {
      HttpResponseMessage response = await _client.GetAsync("Donations");
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }

      Donation[] donationList = JsonSerializer.Deserialize<Donation[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return donationList;
    }

    public Task<Donation> GetDonationDetails(int donationId)
    {
      throw new NotImplementedException();
    }
  }
}
