using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Shared
{
  public class Pledger
  {
    public int ID { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public string QBName { get; set; }

    public Pledger() { }


    public override string ToString()
    {
      return Name;
    }
  }
}
