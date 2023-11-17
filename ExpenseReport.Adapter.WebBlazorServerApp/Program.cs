using Application.Adapter;
using ExpenseReport.Adapter.WebBlazorServerApp.Data;
using ExpenseReport.ApplicationServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ExpenseReportService>();
builder.Services.AddTransient<IDateProvider, RealDateProvider>();
builder.Services.AddTransient<IExistingExpensesRepository, ExistingExpensesRepository>();
builder.Services.AddTransient<IExpenseService, ExpensesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

public partial class BlazorProgram { }
