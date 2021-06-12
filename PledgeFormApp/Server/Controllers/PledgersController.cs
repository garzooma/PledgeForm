using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PledgeFormApp.Shared;
using System;
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
    public IEnumerable<Pledger> Get()
    {
      return _repository.FindAll();
    }

    [HttpGet("{id}")]
    public Pledger Get(int id)
    {
      return _repository.FindByIndex(id);
    }

    [HttpGet("version")]
    public string GetVersion()
    {
      return "1.0";
    }

    //// GET: PledgersController
    //public ActionResult Index()
    //{
    //  return View();
    //}

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
    public void Create([FromBody] Pledger pledger)
    {
      _repository.Create(pledger);
      return;
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
    [HttpPost]
    public void Delete(Pledger pledger)
    {
      _repository.Delete(pledger);

      return;
    }

    [HttpPost]
    public void Update(Pledger pledger)
    {
      _repository.Update(pledger);

      return;
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
