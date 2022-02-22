using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Client.Pages
{
  /// <summary>
  /// Class to display display of donations in Installments page
  /// </summary>
  public class DonationDisplayModel
  {
    public enum DonationsListMode {year, today, custom}

    public DonationsListMode ListMode = DonationsListMode.today;
    public int Year;
    public DateTime FromDate = DateTime.Today;
    public DateTime ToDate = DateTime.Today;
  }
}
