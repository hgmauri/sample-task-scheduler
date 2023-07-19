using Microsoft.Extensions.DependencyInjection;
using Sample.Scheduler.Core.Extensions;

namespace Sample.Scheduler.Core.TimerSchedulers;

public class TimerCheckDatabase : CronJobExtensions
{
	public TimerCheckDatabase(IScheduleConfig<TimerCheckDatabase> config, IServiceProvider serviceProvider)
		: base(config.CronExpression, config.TimeZoneInfo, serviceProvider)
	{
	}

	public override Task DoWork(IServiceScope scope, CancellationToken cancellationToken)
	{
		Serilog.Log.Information("Verified Database!");
		return Task.CompletedTask;
	}
}
