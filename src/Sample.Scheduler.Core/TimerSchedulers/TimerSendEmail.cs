using Microsoft.Extensions.DependencyInjection;
using Sample.Scheduler.Core.Extensions;

namespace Sample.Scheduler.Core.TimerSchedulers;

public class TimerSendEmail : CronJobExtensions
{
	public TimerSendEmail(IScheduleConfig<TimerSendEmail> config, IServiceProvider serviceProvider)
		: base(config.CronExpression, config.TimeZoneInfo, serviceProvider)
	{
	}

	public override Task DoWork(IServiceScope scope, CancellationToken cancellationToken)
	{
		Serilog.Log.Information("Email sent!");
		return Task.CompletedTask;
	}
}
