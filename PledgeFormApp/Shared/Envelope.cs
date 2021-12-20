using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PledgeFormApp.Shared
{
  public class Envelope
  {
    public int PledgerId { get; set; }
    public int EnvelopeNum { get; set; }
    public int Year { get; set; }

    [JsonIgnore]
    public int Index
    {
      get
      {
        return GetIndex(Year, EnvelopeNum);
      }
      set
      {
        EnvelopeNum = GetEnvelopeNum(value);
        Year = GetYear(value);
      }
    }

    // functions for turning envelope number/year into single int index
    // for IRepositoryBase<T>
    public static int GetIndex(int year, int envelopeNum)
    {
      return year * 1000 + envelopeNum;
    }

    public static int GetYear(int index)
    {
      return index / 1000;
    }

    public static int GetEnvelopeNum(int index)
    {
      return index % 1000;
    }
  }
}
