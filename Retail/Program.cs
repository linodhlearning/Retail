using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Retail.AutoMapper;
using Retail.Model;
using Retail.Repository;
using Retail.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ErrorModel).Assembly));


builder.Services.AddDbContext<RetailDbContext>(options =>
  
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString")));

 
builder.Services.AddScoped<RepoUow>();

builder.Services.AddAutoMapperSetup();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  

}

app.DbMigrate(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
