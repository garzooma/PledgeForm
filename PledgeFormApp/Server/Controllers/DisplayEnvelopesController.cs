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
  public class DisplayEnvelopesController : Controller
  {
    private readonly IDisplayEnvelopesRepository _repository;

    public DisplayEnvelopesController(IDisplayEnvelopesRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Envelope>>> Get()
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

    [HttpGet("{year}/{envelopeNum}")]
    public async Task<ActionResult<Pledger>> Get(int year, int envelopeNum)
    {
      try
      {
        //Pledger pledger = await Task.Run(() => _repository.FindByIndex(id));
        DisplayEnvelope envelope = await Task.Run(() => _repository.Find(year, envelopeNum));
        if (envelope == null) return NotFound();
        return Ok(envelope);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex);
      }
    }

    // GET: PledgersController/Create
    [HttpPost("create")]
    public async Task<ActionResult<Envelope>> Create([FromBody] DisplayEnvelope envelope)
    {
      try
      {
        int id = _repository.Create(envelope);
        return Ok(await Task.Run(() => _repository.FindByIndex(id)));
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex);
      }
    }

    //public IActionResult Index()
    //{
    //  return View();
    //}
  }
}
