using System.Security.Claims;
using System.Text;
using Application.Adapter;
using ExpenseReport.ApplicationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = "Data Source=.\\..\\blog.db";
builder.Services.AddDbContext<ExpensesDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AllRegisteredUsers", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
});

builder.Services.AddControllers();
builder.Services.AddTransient<IDateProvider, RealDateProvider>();
builder.Services.AddTransient<IExistingExpensesRepository, ExistingExpensesRepository>();
builder.Services.AddTransient<IExpenseService, ExpensesService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class ApiProgram { }