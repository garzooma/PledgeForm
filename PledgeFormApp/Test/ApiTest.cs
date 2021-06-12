namespace Test
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using PledgeFormApp.Server;
  using PledgeFormApp.Server.Controllers;
  using PledgeFormApp.Shared;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net.Http;
  using System.Net.Http.Json;
  using System.Threading.Tasks;
  using System.Web.Http;

  [TestClass]
  public class ApiTest
  {
    [TestMethod]
    public void TestGetAll()
    {
      PledgersController controller = new PledgersController(new PledgersControllerTest.TestRepository());

      //controller.Request = new HttpRequestMessage();
      //controller.Configuration = new HttpConfiguration();
      HttpClient http = new HttpClient();
      http.BaseAddress = new Uri("https://localhost:5000");
      Task<Pledger[]> task = http.GetFromJsonAsync<Pledger[]>("Pledgers");
      task.Wait();
      Pledger[] pledgerList = task.Result;

      return;
    }
  }
}
