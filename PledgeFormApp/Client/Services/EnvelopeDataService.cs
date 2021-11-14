using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PledgeFormApp.Client.Services
{
  public class EnvelopeDataService : IEnvelopeDataService
  {
    private readonly HttpClient _client;
    public EnvelopeDataService(HttpClient client)
    {
      _client = client;
    }
    
    public Task AddEnvelope(Envelope envelope)
    {
      throw new NotImplementedException();
    }

    public Task DeleteEnvelope(int envelopeId)
    {
      throw new NotImplementedException();
    }

    public async Task<IEnumerable<Envelope>> GetAllEnvelopes()
    {
      HttpResponseMessage response = await _client.GetAsync("Envelopes");
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }

      Envelope[] envelopeList = JsonSerializer.Deserialize<Envelope[]>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return envelopeList;
    }

    public Task<Envelope> GetEnvelopeDetails(int envelopeId)
    {
      throw new NotImplementedException();
    }
  }
}
