
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ReminderAPI.Data;
using ReminderAPI.Models;

namespace ReminderAPI;

public class ReminderService : IHostedService
{
	private readonly IServiceProvider _serviceProvider;


	private Timer _timer;

	public ReminderService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}



	public Task StartAsync(CancellationToken cancellationToken)
	{
		_timer = new Timer(SendReminders, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

		return Task.CompletedTask;
	}

	private async void SendReminders(object? state)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ReminderDbContext>();
			var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

			var reminders = await context.Reminders
				.Where(r => r.SendAt <= DateTime.UtcNow && !r.IsSent).ToListAsync();

			foreach(var reminder in reminders)
			{
				if(reminder.Method.ToLower() == "email")
				{
					emailSender.SendEmailAsync(reminder.To, "Reminder", reminder.Content);
				}

				else if (reminder.Method.ToLower()=="telegram")
				{
					await SendTelegramReminder(reminder);
				}

				reminder.IsSent = true;
			}
			await context.SaveChangesAsync();
		}
	}

	private async Task SendTelegramReminder(Reminder reminder)
	{
		throw new NotImplementedException();
	}

	//private async Task SendEmailReminder(Reminder reminder)
	//{
	//	_emailSender.SendEmailAsync(reminder.To, "Reminder", reminder.Content);
	//}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}
}
