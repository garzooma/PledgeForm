using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Client.Services
{
  interface IInstallmentDataService
  {
    Task<IEnumerable<Installment>> GetAllInstallments();
    Task<Installment> GetInstallmentDetails(int donationId);
    Task<IEnumerable<Installment>> GetInstallmentsByDates(DateTime from, DateTime to);
    Task<IEnumerable<Installment>> GetInstallmentsByYear(int year);
  }
}
