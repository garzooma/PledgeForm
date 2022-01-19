using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Server.Model
{
  public class DisplayEnvelopeQuery
  {
    public readonly AppDb Db;
    public DisplayEnvelopeQuery(AppDb db)
    {
      Db = db;
    }

    public async Task<List<DisplayEnvelope>> ReadAllAsync()
    {
      using (DbCommand cmd = Db.Connection.CreateCommand())
      {
        cmd.CommandText = @"SELECT e.pledgerId, e.envelopeNum, e.year, p.name, p.weeklyAmount, p.qbName FROM envelopes e join pledgers p on p.id = e.pledgerId";
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    private async Task<List<DisplayEnvelope>> ReadAllAsync(DbDataReader dbDataReader)
    {
      List<DisplayEnvelope> ret = new List<DisplayEnvelope>();

      using (dbDataReader)
      {
        try
        {
          while (await dbDataReader.ReadAsync())
          {
            int id = dbDataReader.GetInt32(0);
            int envNum = dbDataReader.GetInt32(1);
            int year = dbDataReader.GetInt32(2);
            string name = dbDataReader.GetString(3);
            int amount = dbDataReader.GetInt32(4);
            string qbName = dbDataReader.GetString(5);
            Pledger pledger = new Pledger()
            {
              ID = id,
              Name = name,
              Amount = amount,
              QBName = qbName
            };
            DisplayEnvelope envelope = new DisplayEnvelope()
            {
              PledgerId = id,
              EnvelopeNum = envNum,
              Year = year,
              Pledger = pledger
            };
            ret.Add(envelope);
          }
        }
        catch (Exception excp)
        {
          Console.WriteLine("Exception: " + excp.Message);
        }
      }

      return ret;
    }

  }
}
