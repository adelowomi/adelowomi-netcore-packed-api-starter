using Adelowomi.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddConfiguration();
builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseStandardResponse();

app.MapIdentityEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();


