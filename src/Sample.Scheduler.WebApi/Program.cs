using Sample.Scheduler.Core.Extensions;
using Sample.Scheduler.Core.TimerSchedulers;

var builder = WebApplication.CreateBuilder(args);
builder.AddSerilog();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCronJob<TimerSendEmail>(c => c.CronExpression = @"0 */1 * * * *");
builder.Services.AddCronJob<TimerCheckDatabase>(c => c.CronExpression = @"* * * * * *");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();