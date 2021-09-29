using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Server.Model
{
  public class DonationQuery
  {
    public readonly AppDb Db;
    public DonationQuery(AppDb db)
    {
      Db = db;
    }

    public async Task<List<Donation>> ReadAllAsync()
    {
      using (DbCommand cmd = Db.Connection.CreateCommand())
      {
        cmd.CommandText = @"SELECT pd.id, pd.date, pd.amount, pd.pledger FROM pledgedonations pd";
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    private async Task<List<Donation>> ReadAllAsync(DbDataReader dbDataReader)
    {
      List<Donation> ret = new List<Donation>();

      using (dbDataReader)
      {
        try
        {
          while (await dbDataReader.ReadAsync())
          {
            Donation donation = new Donation()
            {
              ID = dbDataReader.GetInt32(0),
              Date = dbDataReader.GetDateTime(1),
              Amount = dbDataReader.GetInt32(2),
              PledgerId = dbDataReader.GetInt32(3)
            };
            ret.Add(donation);
          }
        }
        catch (Exception excp)
        {
          Console.WriteLine("Exception: " + excp.Message);
        }
      }

      return ret;
    }

    public async Task<Donation> ReadByIndexAsync(int index)
    {
      using (var cmd = Db.Connection.CreateCommand())
      {
        cmd.CommandText = @"SELECT id, date, amount, pledger FROM pledgedonations WHERE id = @id";
        BindId(cmd, index);
        List<Donation> DonationList = await ReadAllAsync(await cmd.ExecuteReaderAsync());
        return DonationList.FirstOrDefault();
      }
    }

    public async Task<int> InsertAsync(Donation donation)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"INSERT INTO `pledgedonations` (`date`, `amount`, `pledger`) VALUES (@date, @amount, @pledger);";
      BindParams(cmd, donation);
      await cmd.ExecuteNonQueryAsync();
      int Id = (int)cmd.LastInsertedId;
      return Id;
    }

    // Never update a donation -- just delete and re-insert
    //public async Task UpdateAsync(Donation Donation)
    //{
    //  using var cmd = Db.Connection.CreateCommand();
    //  cmd.CommandText = @"UPDATE `pledgedonations` SET `date` = @date, `amount` = @amount, `pledger` = @pledger WHERE `Id` = @id;";
    //  BindParams(cmd, Donation);
    //  BindId(cmd, Donation.ID);
    //  await cmd.ExecuteNonQueryAsync();
    //}

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

    private void BindParams(MySqlCommand cmd, Donation donation)
    {
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@date",
        DbType = System.Data.DbType.String,
        Value = donation.Date,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@amount",
        DbType = System.Data.DbType.Int32,
        Value = donation.Amount,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@pledger",
        DbType = System.Data.DbType.Int32,
        Value = donation.PledgerId,
      });
    }
  }
}
