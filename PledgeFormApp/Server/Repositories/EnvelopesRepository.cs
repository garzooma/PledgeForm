using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server
{
  public class EnvelopesRepository : IEnvelopesRepository
  {
    private readonly string _connectionString;

    public EnvelopesRepository() : this(AppConfig.Config["Data:ConnectionString"])
    {

    }

    public EnvelopesRepository(string connectionString)
    {
      _connectionString = connectionString;
    }


    public int Create(Envelope envelope)
    {
      using (AppDb db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.EnvelopeQuery(db);
        var result = query.InsertAsync(envelope);
        result.Wait();
        return result.Result;
      }
    }

    public void Delete(int index)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Envelope> FindAll()
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.EnvelopeQuery(db);
        var result = query.ReadAllAsync();
        result.Wait();
        return result.Result;
      }
    }

    public Envelope FindByIndex(int index)
    {
      throw new NotImplementedException();
    }

    public void Update(Envelope entity)
    {
      throw new NotImplementedException();
    }
  }
}
