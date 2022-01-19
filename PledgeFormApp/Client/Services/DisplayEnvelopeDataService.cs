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
  public class DisplayEnvelopeDataService : IDisplayEnvelopeDataService
  {
    private readonly HttpClient _client;
    public DisplayEnvelopeDataService(HttpClient client)
    {
      _client = client;
    }
    
    public async Task AddEnvelope(DisplayEnvelope envelope)
    {
      HttpResponseMessage response = await _client.PostAsJsonAsync<DisplayEnvelope>("/displayenvelopes/create", envelope);
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }
      DisplayEnvelope retVal = JsonSerializer.Deserialize<DisplayEnvelope>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return;
    }

    public Task DeleteEnvelope(int envelopeId)
    {
      throw new NotImplementedException();
    }

    public async Task<IEnumerable<DisplayEnvelope>> GetAllEnvelopes()
    {
      HttpResponseMessage response = await _client.GetAsync("DisplayEnvelopes");
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }

      DisplayEnvelope[] envelopeList = JsonSerializer.Deserialize<DisplayEnvelope[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return envelopeList;
    }

    public Task<DisplayEnvelope> GetEnvelopeDetails(int envelopeId)
    {
      throw new NotImplementedException();
    }
  }
}
