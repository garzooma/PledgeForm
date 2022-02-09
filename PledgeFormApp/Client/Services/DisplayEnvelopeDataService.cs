using Microsoft.AspNetCore.Components;
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
    private readonly NavigationManager _navMgr;
    public DisplayEnvelopeDataService(HttpClient client, NavigationManager navigationManager)
    {
      _client = client;
      _navMgr = navigationManager;
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

    public async Task<DisplayEnvelope> GetEnvelopeDetails(int envelopeId)
    {
      HttpResponseMessage response = await _client.GetAsync("DisplayEnvelopes");
      string content = await response.Content.ReadAsStringAsync();
      if (!response.IsSuccessStatusCode)
      {
        throw new ApplicationException(content);
      }

      DisplayEnvelope envelope = JsonSerializer.Deserialize<DisplayEnvelope>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return envelope;
    }

    public async Task<DisplayEnvelope> GetEnvelopeDetails(int year, int envelopeNum)
    {
      HttpResponseMessage response = await _client.GetAsync($"DisplayEnvelopes/{year}/{envelopeNum}");
      //response.EnsureSuccessStatusCode();
      if (!response.IsSuccessStatusCode)
      {
        //if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        //{
        //  _navMgr.NavigateTo("/500");
        //}
        //throw new ApplicationException($"Couldn't get envelope '{envelopeNum}' for year '{year}'; Reason: {response.ReasonPhrase}");
        return null;
      }
      string content = await response.Content.ReadAsStringAsync();
      DisplayEnvelope envelope = JsonSerializer.Deserialize<DisplayEnvelope>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      return envelope;
    }
  }
}
