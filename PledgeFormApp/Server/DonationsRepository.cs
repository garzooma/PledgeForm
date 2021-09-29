using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PledgeFormApp.Shared;

namespace PledgeFormApp.Server
{
  public class DonationsRepository : IDonationsRepository
  {
    private readonly string _connectionString;

    public DonationsRepository() : this(AppConfig.Config["Data:ConnectionString"])
    {

    }

    public DonationsRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    public int Create(Donation donation)
    {
      using (AppDb db = new AppDb(_connectionString)) { 
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.DonationQuery(db);
        var result = query.InsertAsync(donation);
        result.Wait();
        return result.Result;
      }
    }

    public void Delete(int index)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.DonationQuery(db);
        var result = query.DeleteAsync(index);
        result.Wait();
      }
    }

    public IEnumerable<Donation> FindAll()
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.DonationQuery(db);
        var result = query.ReadAllAsync();
        result.Wait();
        return result.Result;
      }
    }

    public Donation FindByIndex(int index)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.DonationQuery(db);
        var result = query.ReadByIndexAsync(index);
        result.Wait();
        return result.Result;
      }
    }

    public void Update(Donation donation)
    {
      //using (var db = new AppDb(_connectionString))
      //{
      //  Task open = db.Connection.OpenAsync();
      //  open.Wait();
      //  var query = new Model.DonationQuery(db);
      //  var result = query.UpdateAsync(donation);
      //  result.Wait();
      //  return;
      //}
      throw new NotImplementedException();  // never update a donation -- delete and re-insert
    }
  }
}
