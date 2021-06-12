using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server.Controllers
{
  public class HelloWorldController : Controller
  {
    // 
    // GET: /HelloWorld/
    public string Index()
    {
      return "This is my default action...";
    }

    // Get: /HellowWorld/Welcome
    public string Welcome()
    {
      return "This is the Welcome action method...";
    }
  }
}
