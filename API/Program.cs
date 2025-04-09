using API.Extensions;
using APPLICATION;
using Carter;
using INFRAESTRUCTURE;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfraestructure(builder.Configuration);



builder.Services.AddProblemDetails()
                .AddCarter();

var app = builder.Build();

app.ApplyMigration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapCarter();
}

app.UseHttpsRedirection();



app.Run();

