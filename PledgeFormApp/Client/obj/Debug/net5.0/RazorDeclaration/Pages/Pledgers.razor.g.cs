// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace PledgeFormApp.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using PledgeFormApp.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\_Imports.razor"
using PledgeFormApp.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\Pages\Pledgers.razor"
using PledgeFormApp.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/pledgerspage")]
    public partial class Pledgers : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 51 "F:\ga\proj\dotnet\blazor\pledgeform\PledgeFormApp\Client\Pages\Pledgers.razor"
       

  private Pledger[] pledgerList;

  [Inject]
  Services.IPledgerDataService PledgerDataService { get; set; }

  [Inject]
  public NavigationManager NavigationManager { get; set; }

  protected override async Task OnInitializedAsync()
  {
      //pledgerList = await Http.GetFromJsonAsync<Pledger[]>("Pledgers");
      IEnumerable<Pledger> list = await PledgerDataService.GetAllPledgers();
      pledgerList = list.ToArray();
  }

  protected async Task AddPledger()
  {
      //HttpResponseMessage returnMsg = await Http.PostAsJsonAsync<Pledger>("/pledgers/create", new Pledger
      //{
      //    Name = "Tom",
      //    Amount = 88,
      //    QBName = "QTom"
      //});
      await Task.Run(() => { NavigationManager.NavigateTo("/addpledger"); });
  }

  protected async Task DeletePledger(Pledger pledger)
  {
      await PledgerDataService.DeletePledger(pledger.ID);
      IEnumerable<Pledger> list = await PledgerDataService.GetAllPledgers();
      pledgerList = list.ToArray();
  }

  private int currentCount = 0;


  private void IncrementCount()
  {
      currentCount++;
  }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient Http { get; set; }
    }
}
#pragma warning restore 1591
