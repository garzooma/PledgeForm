using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Server
{
  public interface IDisplayEnvelopesRepository : IRepositoryBase<DisplayEnvelope>
  {
    DisplayEnvelope Find(int year, int envelopeNum);
  }
}
