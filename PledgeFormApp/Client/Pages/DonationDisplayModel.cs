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
    private DateTime fromDate = DateTime.Today;
    public DateTime FromDate
    {
      get
      {
        return fromDate;
      }
      set
      {
        fromDate = value;
        PropertyChanged(this, new PropertyChangedEventArgs(nameof(FromDate)));
      }
    }

    private DateTime toDate = DateTime.Today;
    public DateTime ToDate
    {
      get
      {
        return toDate;
      }
      set
      {
        toDate = value;
        PropertyChanged(this, new PropertyChangedEventArgs(nameof(ToDate)));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged = (s,e) => {};
  }
}
