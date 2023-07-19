using Microsoft.Extensions.DependencyInjection;

namespace Sample.Scheduler.Core.Extensions;
public static class ScheduledServiceExtensions
{
	public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : CronJobExtensions
	{
		var config = new ScheduleConfig<T>();
		options.Invoke(config);

		services.AddSingleton<IScheduleConfig<T>>(config);

		services.AddHostedService<T>();

		return services;
	}
}
