using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Client.Services
{
  public interface IPledgerDataService
  {
    Task<IEnumerable<Pledger>> GetAllPledgers();
    Task<Pledger> GetPledgerDetails(int pledgerId);
    Task AddPledger(Pledger pledger);
    Task UpdatePledger(Pledger pledger);
    Task DeletePledger(int pledgerId);
  }
}
