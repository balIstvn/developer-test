using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Clients;
using Taxually.TechnicalTest.Handlers;
using Taxually.TechnicalTest.Handlers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register Clients
builder.Services.AddSingleton<ITaxuallyHttpClient, TaxuallyHttpClient>();
builder.Services.AddSingleton<ITaxuallyQueueClient, TaxuallyQueueClient>();

// Register IVatRegistration handlers
builder.Services.AddSingleton<IVatRegistrationHandler, UkVatRegistrationHandler>();
builder.Services.AddSingleton<IVatRegistrationHandler, FranceVatRegistrationHandler>();
builder.Services.AddSingleton<IVatRegistrationHandler, GermanyVatRegistrationHandler>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
