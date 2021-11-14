using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Client.Services
{
  interface IEnvelopeDataService
  {
    Task<IEnumerable<Envelope>> GetAllEnvelopes();
    Task<Envelope> GetEnvelopeDetails(int envelopeId);
    Task AddEnvelope(Envelope envelope);
    Task DeleteEnvelope(int envelopeId);
  }
}
