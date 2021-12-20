using MySqlConnector;
using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server.Model
{
  public class PledgerQuery
  {
    public readonly AppDb Db;
    public PledgerQuery(AppDb db)
    {
      Db = db;
    }

    public async Task<List<Pledger>> ReadAllAsync()
    {
      using (DbCommand cmd = Db.Connection.CreateCommand()) {
        cmd.CommandText = @"SELECT id, name, weeklyAmount, qbName FROM pledgers";
        return await ReadAllAsync(await cmd.ExecuteReaderAsync());
      }
    }

    private async Task<List<Pledger>> ReadAllAsync(DbDataReader dbDataReader)
    {
      List<Pledger> ret = new List<Pledger>();

      using (dbDataReader)
      {
        try {
        while (await dbDataReader.ReadAsync())
        {
          int amount = 0;
          if (!dbDataReader.IsDBNull(2)) amount = dbDataReader.GetInt32(2);
          Pledger pledger = new Pledger()
          {
            ID = dbDataReader.GetInt32(0),
            Name = dbDataReader.GetString(1),
            Amount = amount,
            QBName = dbDataReader.GetString(3)
          };
          ret.Add(pledger);
        } 
        } catch(Exception excp)
        {
          Console.WriteLine("Exception: " + excp.Message);
        }
      }

      return ret;
    }

    public async Task<Pledger> ReadByIndexAsync(int index)
    {
      using (var cmd = Db.Connection.CreateCommand())
      {
        cmd.CommandText = @"SELECT id, name, weeklyAmount, qbName FROM pledgers WHERE id = @id";
        BindId(cmd, index);
        List<Pledger> pledgerList = await ReadAllAsync(await cmd.ExecuteReaderAsync());
        return pledgerList.FirstOrDefault();
      }
    }

    public async Task<int> InsertAsync(Pledger pledger)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"INSERT INTO `pledgers` (`name`, `weeklyamount`, `qbname`) VALUES (@name, @weeklyamount, @qbname);";
      BindParams(cmd, pledger);
      await cmd.ExecuteNonQueryAsync();
      int Id = (int)cmd.LastInsertedId;
      return Id;
    }

    public async Task UpdateAsync(Pledger pledger)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"UPDATE `pledgers` SET `name` = @name, `weeklyamount` = @weeklyamount, `qbname` = @qbname WHERE `Id` = @id;";
      BindParams(cmd, pledger);
      BindId(cmd, pledger.ID);
      await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"DELETE FROM `pledgers` WHERE `Id` = @id;";
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

    private void BindParams(MySqlCommand cmd, Pledger pledger)
    {
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@name",
        DbType = System.Data.DbType.String,
        Value = pledger.Name,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@weeklyamount",
        DbType = System.Data.DbType.Int32,
        Value = pledger.Amount,
      });
      cmd.Parameters.Add(new MySqlParameter
      {
        ParameterName = "@qbname",
        DbType = System.Data.DbType.Int32,
        Value = pledger.QBName,
      });
    }
  }
}

