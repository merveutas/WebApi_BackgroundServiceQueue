using Queue.WebApi;
using Queue.WebApi.Queues;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(i => { i.AddConsole(); i.AddDebug(); });

builder.Services.AddSingleton(typeof(IBackgroundTaskQueue<string>), typeof(NamesQueue)); //tek 1 tane oluşsun istiyoruz program boyunca

builder.Services.AddHostedService<QueueHostedService>();

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
