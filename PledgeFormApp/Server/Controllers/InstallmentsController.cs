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
  public class InstallmentsController : ControllerBase
  {
    private readonly IInstallmentsRepository _repository;
    public InstallmentsController(IInstallmentsRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Installment>>> Get()
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

    [HttpGet("{start}/{end}")]
    public async Task<ActionResult<IEnumerable<Installment>>> Get(DateTime start, DateTime end)
    {
      try
      {
        return Ok(await Task.Run(() => _repository.FindByDates(start, end)));
      }
      catch (Exception excp)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, excp);
      }
    }

    [HttpGet("{year}")]
    public async Task<ActionResult<IEnumerable<Installment>>> Get(int year)
    {
      try
      {
        return Ok(await Task.Run(() => _repository.FindByYear(year)));
      }
      catch (Exception excp)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, excp);
      }
    }

    // GET: PledgersController/Create
    [HttpPost("create")]
    public async Task<ActionResult<Installment>> Create([FromBody] Installment installment)
    {
      try
      {
        int id = _repository.Create(installment);
        return Ok(await Task.Run(() => _repository.FindByIndex(id)));
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex);
      }
    }
  }
}
