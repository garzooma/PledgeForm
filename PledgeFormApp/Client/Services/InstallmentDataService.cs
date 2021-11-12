using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PledgeFormApp.Client.Services
{
  public class InstallmentDataService : IInstallmentDataService
  {
    private readonly HttpClient _client;
    public InstallmentDataService(HttpClient client)
    {
      _client = client;
    }

    public async Task<IEnumerable<Installment>> GetAllInstallments()
    {
      HttpResponseMessage response = await _client.GetAsync("Installments");
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }

      Installment[] installmentList = JsonSerializer.Deserialize<Installment[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return installmentList;
    }

    public Task<Installment> GetInstallmentDetails(int donationId)
    {
      throw new NotImplementedException();
    }
  }
}
