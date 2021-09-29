using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PledgeFormApp.Shared;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PledgersController : ControllerBase
  {
    private readonly IPledgersRepository _repository;

    //public PledgersController() : this(new PledgersRepository(AppConfig.Config["Data:ConnectionString"]) )
    //{

    //}

    public PledgersController(IPledgersRepository repository)
    {
      _repository = repository;
    }


    [HttpGet]
    public async Task <ActionResult<IEnumerable<Pledger>>> Get()
    {
      try
      { 
        return Ok( await Task.Run(() => _repository.FindAll()));
      } catch (Exception excp)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, excp);
      }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pledger>> Get(int id)
    {
      try
      {
        Pledger pledger = await Task.Run(() => _repository.FindByIndex(id));
        if (pledger == null) return NotFound();
        return Ok(pledger);
      } 
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex);
      }
    }

    [HttpGet("version")]
    public string GetVersion()
    {
      return "1.0";
    }


    //// GET: PledgersController/Details/5
    //public async Task<IActionResult> Details(int id)
    //{
    //  using (var db = new AppDb())
    //  {
    //    await db.Connection.OpenAsync();
    //    var query = new Model.PledgerQuery(db);
    //    var result = await query.ReadAllAsync();
    //    return new OkObjectResult(result);
    //  }
    //}

    // GET: PledgersController/Create
    [HttpPost("create")]
    public async Task<ActionResult<Pledger>> Create([FromBody] Pledger pledger)
    {
      try
      {
        int id = _repository.Create(pledger);
        return Ok(await Task.Run(() => _repository.FindByIndex(id)));
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex);
      }
    }

    //// POST: PledgersController/Create
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Create(IFormCollection collection)
    //{
    //  try
    //  {
    //    return RedirectToAction(nameof(Index));
    //  }
    //  catch
    //  {
    //    return View();
    //  }
    //}

    //// GET: PledgersController/Edit/5
    //public ActionResult Edit(int id)
    //{
    //  return View();
    //}

    //// POST: PledgersController/Edit/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Edit(int id, IFormCollection collection)
    //{
    //  try
    //  {
    //    return RedirectToAction(nameof(Index));
    //  }
    //  catch
    //  {
    //    return View();
    //  }
    //}

    // GET: PledgersController/Delete/5
    [HttpPost("delete")]
    public async Task<ActionResult> Delete([FromBody] int id)
    {
      await Task.Run(() => _repository.Delete(id));

      return Ok();
    }

    [HttpPut("update")]
    public async Task<ActionResult<Pledger>> Update([FromBody] Pledger pledger)
    {
      try
      {
        await Task.Run(() => _repository.Update(pledger));
        return Ok(await Task.Run(() => _repository.FindByIndex(pledger.ID)));
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, ex);
      }
    }

    //// POST: PledgersController/Delete/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Delete(int id, IFormCollection collection)
    //{
    //  try
    //  {
    //    return RedirectToAction(nameof(Index));
    //  }
    //  catch
    //  {
    //    return View();
    //  }
    //}
  }
}
