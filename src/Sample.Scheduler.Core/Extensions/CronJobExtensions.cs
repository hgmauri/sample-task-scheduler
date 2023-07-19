using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Sample.Scheduler.Core.Extensions;

public abstract class CronJobExtensions : BackgroundService
{
	private readonly CronExpression _expression;
	private readonly TimeZoneInfo _timeZoneInfo;
	private readonly IServiceProvider _serviceProvider;

	protected CronJobExtensions(string cronExpression, TimeZoneInfo timeZoneInfo, IServiceProvider serviceProvider)
	{
		_expression = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
		_timeZoneInfo = timeZoneInfo;
		_serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			var now = DateTimeOffset.Now;
			var next = _expression.GetNextOccurrence(now, _timeZoneInfo);
			if (!next.HasValue) continue;

			var delay = next.Value - now;
			await Task.Delay(delay, cancellationToken);

			if (cancellationToken.IsCancellationRequested) continue;

			try
			{
				using var scope = _serviceProvider.CreateScope();
				await DoWork(scope, cancellationToken);
			}
			catch (Exception ex)
			{
				Log.Error(ex, nameof(ExecuteAsync));
			}
		}
	}

	public abstract Task DoWork(IServiceScope scope, CancellationToken cancellationToken);
}

public interface IScheduleConfig<T>
{
	string CronExpression { get; set; }
	TimeZoneInfo TimeZoneInfo { get; set; }
}

public class ScheduleConfig<T> : IScheduleConfig<T>
{
	public string CronExpression { get; set; }
	public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
}