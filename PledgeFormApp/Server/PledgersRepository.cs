using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server
{
  public class PledgersRepository : IPledgersRepository
  {
    private readonly string _connectionString;

    public PledgersRepository() : this(AppConfig.Config["Data:ConnectionString"])
    {

    }

    public PledgersRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    public void Create(Pledger pledger)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.PledgerQuery(db);
        var result = query.InsertAsync(pledger);
        result.Wait();
      }
    }

    public void Delete(Pledger pledger)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.PledgerQuery(db);
        var result = query.DeleteAsync(pledger.ID);
        result.Wait();
      }
    }

    public IEnumerable<Pledger> FindAll()
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.PledgerQuery(db);
        var result = query.ReadAllAsync();
        result.Wait();
        return result.Result;
      }
    }

    public Pledger FindByIndex(int index)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.PledgerQuery(db);
        var result = query.ReadByIndexAsync(index);
        result.Wait();
        return result.Result;
      }
    }

    public void Update(Pledger pledger)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.PledgerQuery(db);
        var result = query.UpdateAsync(pledger);
        result.Wait();
        return;
      }
    }
  }
}
