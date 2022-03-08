using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Client.Pages
{
  /// <summary>
  /// Class to display display of donations in Installments page
  /// </summary>
  public class DonationDisplayModel : INotifyPropertyChanged
  {
    public DonationDisplayModel()
    {
      listMode = DonationsListMode.today;
    }
    public enum DonationsListMode {all, year, today, custom}

    private DonationsListMode listMode;
    public DonationsListMode ListMode
    {
      get { return listMode; }
      set { 
        listMode = value;
        PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListMode)));
      }
    }
    public int Year = DateTime.Today.Year;
    public DateTime FromDate = DateTime.Today;
    public DateTime ToDate = DateTime.Today;

    public event PropertyChangedEventHandler PropertyChanged = (s,e) => {};
  }
}
