using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PledgeFormApp.Shared
{
  public class Installment
  {
    public int ID { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
    public int PledgerId { get; set; }
    public string Pledger { get; set; }
    public string QBName { get; set; }
    public override string ToString()
    {
      return string.Format("{0} {1}", Date.Date, Pledger);
    }
  }
}
