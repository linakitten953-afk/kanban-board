using Blazored.LocalStorage;
using KanbanBoard.Client;
using KanbanBoard.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5143/")
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();