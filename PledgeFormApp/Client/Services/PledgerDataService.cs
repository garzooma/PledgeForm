using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PledgeFormApp.Client.Services
{
  public class PledgerDataService : IPledgerDataService
  {
    private readonly HttpClient _client;
    public PledgerDataService(HttpClient client)
    {
      _client = client;
    }

    public async Task AddPledger(Pledger pledger)
    {
      HttpResponseMessage response = await _client.PostAsJsonAsync<Pledger>("/pledgers/create", pledger);
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
      Pledger retVal = JsonSerializer.Deserialize<Pledger>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return;
    }

    public async Task DeletePledger(int id)
    {
      HttpResponseMessage response = await _client.PostAsJsonAsync<int>("/pledgers/delete", id);
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
      return;
    }

    public async Task<IEnumerable<Pledger>> GetAllPledgers()
    {
      HttpResponseMessage response = await _client.GetAsync("Pledgers");
      if (!response.IsSuccessStatusCode)
      {
        string result = response.Content.ReadAsStringAsync().Result;

        throw new ApplicationException(result);
      }

      string content = await response.Content.ReadAsStringAsync();
      Pledger[] pledgerList = JsonSerializer.Deserialize<Pledger[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return pledgerList;
    }

    public async Task<Pledger> GetPledgerDetails(int pledgerId)
    {
      //Pledger pledgerDetail = await _client.GetFromJsonAsync<Pledger>("Pledgers/" + pledgerId);
      HttpResponseMessage response = await _client.GetAsync("Pledgers/" + pledgerId);
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
      Pledger pledgerDetail = JsonSerializer.Deserialize<Pledger>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return pledgerDetail;
    }

    public async Task UpdatePledger(Pledger pledger)
    {
      HttpResponseMessage response = await _client.PutAsJsonAsync<Pledger>("/pledgers/update", pledger);
      if (!response.IsSuccessStatusCode)
      {
        string errorMessage = response.ReasonPhrase;
        throw new ApplicationException("Error: " + errorMessage);
      }
      Pledger retVal = await response.Content.ReadFromJsonAsync<Pledger>();
      return;
    }
  }
}
