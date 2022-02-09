using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server
{
  public class DisplayEnvelopesRepository : IDisplayEnvelopesRepository
  {
    private readonly string _connectionString;

    public DisplayEnvelopesRepository() : this(AppConfig.Config["Data:ConnectionString"])
    {

    }

    public DisplayEnvelopesRepository(string connectionString)
    {
      _connectionString = connectionString;
    }


    public int Create(DisplayEnvelope envelope)
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

    public DisplayEnvelope Find(int year, int envelopeNum)
    {
      int idx = Envelope.GetIndex(year, envelopeNum);
      return FindByIndex(idx);
    }

    public IEnumerable<DisplayEnvelope> FindAll()
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.DisplayEnvelopeQuery(db);
        var result = query.ReadAllAsync();
        result.Wait();
        return result.Result;
      }
    }

    public DisplayEnvelope FindByIndex(int index)
    {
      using (var db = new AppDb(_connectionString))
      {
        Task open = db.Connection.OpenAsync();
        open.Wait();
        var query = new Model.DisplayEnvelopeQuery(db);
        var result = query.ReadByIndexAsync(index);
        result.Wait();
        return result.Result;
      }
    }


    public void Update(DisplayEnvelope entity)
    {
      throw new NotImplementedException();
    }

    DisplayEnvelope IRepositoryBase<DisplayEnvelope>.FindByIndex(int index)
    {
      throw new NotImplementedException();
      //using (var db = new AppDb(_connectionString))
      //{
      //  Task open = db.Connection.OpenAsync();
      //  open.Wait();
      //  var query = new Model.DisplayEnvelopeQuery(db);
      //  var result = query.ReadByIndexAsync(index);
      //  result.Wait();
      //  return result.Result;
      //}
      //
      }
    }
  }
