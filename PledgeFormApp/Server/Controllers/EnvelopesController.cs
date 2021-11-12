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
  public class EnvelopesController : Controller
  {
    private readonly IEnvelopesRepository _repository;

    public EnvelopesController(IEnvelopesRepository repository)
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

    public IActionResult Index()
    {
      return View();
    }
  }
}
