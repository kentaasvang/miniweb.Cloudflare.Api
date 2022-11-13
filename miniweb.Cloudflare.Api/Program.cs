using CloudFlare.Client.Api.Zones.DnsRecord;
using Microsoft.EntityFrameworkCore;
using miniweb.Cloudflare.Api;
using miniweb.Cloudflare.Api.Data;
using miniweb.Cloudflare.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCloudflareClient(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddServices();

builder.Host.UseSystemd();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
  
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Api}/{action=Index}/{id?}");

app.Run();