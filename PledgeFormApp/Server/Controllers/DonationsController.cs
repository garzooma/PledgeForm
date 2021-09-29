using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PledgeFormApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class DonationsController : ControllerBase
  {
    private readonly IDonationsRepository _repository;

    public DonationsController(IDonationsRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Donation>>> Get()
    {
      try
      {
        return Ok(await Task.Run(() => _repository.FindAll()));
      }
      catch (Exception excp)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, excp);
      }
    }

    // GET: PledgersController/Create
    [HttpPost("create")]
    public async Task<ActionResult<Donation>> Create([FromBody] Donation donation)
    {
      try
      {
        int id = _repository.Create(donation);
        return Ok(await Task.Run(() => _repository.FindByIndex(id)));
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex);
      }
    }

  }
}
