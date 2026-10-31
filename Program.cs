using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;
using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register application services here
// builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//error handling custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseMiddleware<TokenValidationMiddleware>();

app.UseAuthorization();

//custom logging middleware
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.MapControllers();

app.Run();

//copilot generated basic code and boilerplate