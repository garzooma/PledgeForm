using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server
{
  public class InstallmentsRepository : IInstallmentsRepository
  {
    private readonly string _connectionString;

    public InstallmentsRepository() : this (AppConfig.Config["Data:ConnectionString"])
    {

    }

    public InstallmentsRepository(string connectionString)
    {
      this._connectionString = connectionString;
    }

    public int Create(Installment installment)
    {
      using (AppDb db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.InstallmentQuery(db);
        var result = query.InsertAsync(installment);
        result.Wait();
        return result.Result;
      }
    }

    public void Delete(int index)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Installment> FindAll()
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.InstallmentQuery(db);
        var result = query.ReadAllAsync();
        result.Wait();
        return result.Result;
      }
    }

    /// <summary>
    /// Find donations between start and end, inclusive
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public IEnumerable<Installment> FindByDates(DateTime start, DateTime end)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.InstallmentQuery(db);
        var result = query.ReadByDatesAsync(start, end.AddDays(1));
        result.Wait();
        return result.Result;
      }
    }

    public Installment FindByIndex(int index)
    {
      throw new NotImplementedException();
    }

    public void Update(Installment entity)
    {
      throw new NotImplementedException();
    }
  }
}
