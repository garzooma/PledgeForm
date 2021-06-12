using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PledgerController : ControllerBase
  {

    private readonly ILogger<Pledger> _logger;

    public PledgerController(ILogger<Pledger> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Pledger> Get()
    {
      using (var db = new AppDb())
      {
        db.Connection.Open();
        var query = new Model.PledgerQuery(db);
        Task<List<Pledger>> result = query.ReadAllAsync();
        result.Wait();
        return result.Result;
      }
    }
  }
}
