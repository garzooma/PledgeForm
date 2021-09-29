using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PledgeFormApp.Shared
{
  public class Donation
  {
    public int ID { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
    public int PledgerId { get; set; }
    public Pledger Pledger { get; set; }
    public Donation() { }

    public override string ToString()
    {
      return string.Format("{0}:{1}", PledgerId, Date.ToShortDateString());
    }
  }
}
