using AppointmentDesk.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using AppointmentDesk.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBusinessLogic();
builder.Services.AddDataAccess(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/appointment", async (
    [FromBody] CreateAppointmentRequest body,
    [FromServices] IAppointmentService service 
    ) =>
{
    await service.Create(body);
    return Results.Ok();
    //return Results.Created($"/todoitems/{todoItem.Id}", new TodoItemDTO(todoItem));
})
.WithName("CreateAppointment")
.WithOpenApi();

app.Run();

