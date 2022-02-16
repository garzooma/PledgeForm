using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server
{
  public interface IInstallmentsRepository : IRepositoryBase<Installment>
  {
    IEnumerable<Installment> FindByDates(DateTime start, DateTime end);

  }
}
