using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace PledgeFormApp.Server.Model
{
  public class InstallmentQuery
  {
    public readonly AppDb Db;
    public InstallmentQuery(AppDb db)
    {
      Db = db;
    }

    public async Task<List<Installment>> ReadByDatesAsync(DateTime start, DateTime end)
    {
      using (DbCommand cmd = Db.Connection.CreateCommand())
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT pd.id, pd.date, pd.amount, pd.pledger, p.name, p.qbName, ev.envelopeNum, ev.year ");
        sb.Append(@"FROM pledgedonations pd  ");
        sb.Append(@"JOIN pledgers p on p.id = pd.pledger ");
        sb.Append(@"JOIN envelopes ev on ev.pledgerId = p.id ");
        sb.Append(@"WHERE pd.date >= @start and pd.date < @end");
        cmd.CommandText = sb.ToString();
        DbParameter parm = cmd.CreateParameter();
        parm.ParameterName = "@start";
        parm.Value = start;
        cmd.Parameters.Add(parm);
        parm = cmd.CreateParameter();
        parm.ParameterName = "@end";
        parm.Value = end;
        cmd.Parameters.Add(parm);
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    public async Task<List<Installment>> ReadAllAsync()
    {
      using (DbCommand cmd = Db.Connection.CreateCommand())
      {
        cmd.CommandText = @"SELECT pd.id, pd.date, pd.amount, pd.pledger, p.name, p.qbName, ev.envelopeNum, ev.year FROM pledgedonations pd JOIN pledgers p on p.id = pd.pledger JOIN envelopes ev on ev.pledgerId = p.id";
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    private async Task<List<Installment>> ReadAllAsync(DbDataReader dbDataReader)
    {
      List<Installment> ret = new List<Installment>();

      using (dbDataReader)
      {
        try
        {
          while (await dbDataReader.ReadAsync())
          {
            Installment installment = new Installment()
            {
              ID = dbDataReader.GetInt32(0),
              Date = dbDataReader.GetDateTime(1),
              Amount = dbDataReader.GetInt32(2),
              PledgerId = dbDataReader.GetInt32(3),
              Pledger = dbDataReader.GetString(4),
              QBName = dbDataReader.GetString(5),
              EnvelopeNumber = dbDataReader.GetInt32(6),
              Year = dbDataReader.GetInt32(7)
            };
            ret.Add(installment);
          }
        }
        catch (Exception excp)
        {
          Console.WriteLine("Exception: " + excp.Message);
        }
      }

      return ret;
    }


    public async Task<List<Installment>> ReadAllAsyncByDate(DateTime date)
    {
      using (DbCommand cmd = Db.Connection.CreateCommand())
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT pd.id, pd.date, pd.amount, pd.pledger, p.name, p.qbName, ev.envelopeNum, ev.year ");
        sb.Append(@"FROM pledgedonations pd ");
        sb.Append(@"JOIN pledgers p on p.id = pd.pledger ");
        sb.Append(@"JOIN envelopes ev on ev.pledgerId = p.id ");
        sb.Append(@"WHERE pd.date = @date");
        cmd.CommandText = sb.ToString();
        DbParameter parm = cmd.CreateParameter();
        parm.ParameterName = "@date";
        parm.Value = date;
        cmd.Parameters.Add(parm);
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    public async Task<List<Installment>> ReadAllAsyncByYear(int year)
    {
      using (DbCommand cmd = Db.Connection.CreateCommand())
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(@"SELECT pd.id, pd.date, pd.amount, pd.pledger, p.name, p.qbName, ev.envelopeNum, ev.year ");
        sb.Append(@"FROM pledgedonations pd ");
        sb.Append(@"JOIN pledgers p on p.id = pd.pledger ");
        sb.Append(@"JOIN envelopes ev on ev.pledgerId = p.id ");
        sb.Append(@"WHERE ev.year = @year");
        cmd.CommandText = sb.ToString();
        DbParameter parm = cmd.CreateParameter();
        parm.ParameterName = "@year";
        parm.Value = year;
        cmd.Parameters.Add(parm);
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    public async Task<int> InsertAsync(Installment donation)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"INSERT INTO `pledgedonations` (`date`, `amount`, `pledger`) VALUES (@date, @amount, @pledger);";
      BindParams(cmd, donation);
      await cmd.ExecuteNonQueryAsync();
      int Id = (int)cmd.LastInsertedId;
      return Id;
    }

    public async Task DeleteAsync(int id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"DELETE FROM `pledgedonations` WHERE `Id` = @id;";
      BindId(cmd, id);
      await cmd.ExecuteNonQueryAsync();
    }

    private void BindId(MySqlCommand cmd, int Id)
    {
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@id",
        DbType = System.Data.DbType.Int32,
        Value = Id,
      });
    }

    private void BindParams(MySqlConnector.MySqlCommand cmd, Installment installment)
    {
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@date",
        DbType = System.Data.DbType.String,
        Value = installment.Date,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@amount",
        DbType = System.Data.DbType.Int32,
        Value = installment.Amount,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@pledger",
        DbType = System.Data.DbType.Int32,
        Value = installment.PledgerId,
      });
    }
  }
}
