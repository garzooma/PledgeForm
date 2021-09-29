using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Server.Model
{
  public class EnvelopeQuery
  {
    public readonly AppDb Db;
    public EnvelopeQuery(AppDb db)
    {
      Db = db;
    }

    public async Task<List<Envelope>> ReadAllAsync()
    {
      using (DbCommand cmd = Db.Connection.CreateCommand())
      {
        cmd.CommandText = @"SELECT pledgerId, envelopeNum, year FROM envelopes";
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    private async Task<List<Envelope>> ReadAllAsync(DbDataReader dbDataReader)
    {
      List<Envelope> ret = new List<Envelope>();

      using (dbDataReader)
      {
        try
        {
          while (await dbDataReader.ReadAsync())
          {
            Envelope envelope = new Envelope()
            {
              PledgerId = dbDataReader.GetInt32(0),
              EnvelopeNum = dbDataReader.GetInt32(1),
              Year = dbDataReader.GetInt32(2),
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

    public async Task<int> InsertAsync(Envelope envelope)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"INSERT INTO `envelopes` (`pledgerId`, `envelopeNum`, `year`) VALUES (@pledgerId, @envelopeNum, @year);";
      BindParams(cmd, envelope);
      await cmd.ExecuteNonQueryAsync();
      int Id = (int)cmd.LastInsertedId;
      return Id;
    }

    private void BindParams(MySqlCommand cmd, Envelope envelope)
    {
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@pledgerId",
        DbType = System.Data.DbType.Int32,
        Value = envelope.PledgerId,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@envelopeNum",
        DbType = System.Data.DbType.Int32,
        Value = envelope.EnvelopeNum,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@year",
        DbType = System.Data.DbType.Int32,
        Value = envelope.Year,
      });
    }
  }
}
