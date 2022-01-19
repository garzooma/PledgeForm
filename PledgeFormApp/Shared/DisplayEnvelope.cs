using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PledgeFormApp.Shared
{
  /// <summary>
  /// Version of Envelope suitable for display, i.e. has pledger info
  /// </summary>
  public class DisplayEnvelope : Envelope
  {
    public Pledger Pledger { get; set; }
  }
}
