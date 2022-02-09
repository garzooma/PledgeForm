using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Client.Services
{
  interface IDisplayEnvelopeDataService
  {
    Task<IEnumerable<DisplayEnvelope>> GetAllEnvelopes();
    Task<DisplayEnvelope> GetEnvelopeDetails(int envelopeId);
    Task<DisplayEnvelope> GetEnvelopeDetails(int year, int envelopeNum);
    Task AddEnvelope(DisplayEnvelope envelope);
    Task DeleteEnvelope(int envelopeId);
  }
}
