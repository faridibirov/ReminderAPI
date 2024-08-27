
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ReminderAPI.Data;
using ReminderAPI.Helper;
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
			var reminders = await context.Reminders
				.Where(r => r.SendAt <= DateTime.Now && !r.IsSent).ToListAsync();

			foreach(var reminder in reminders)
			{
				if(reminder.Method.ToLower() == "email")
				{
					await SendEmailReminder(scope, reminder);
				}

				else if (reminder.Method.ToLower()=="telegram")
				{
					await SendTelegramReminder(scope, reminder);
				}

				reminder.IsSent = true;
			}
			await context.SaveChangesAsync();
		}
	}

	private async Task SendTelegramReminder(IServiceScope scope, Reminder reminder)
	{
		var telegramService = scope.ServiceProvider.GetRequiredService<TelegramSender>();

		await telegramService.SendTelegramReminderAsync(reminder.To, reminder.Content);
	}

	private async Task SendEmailReminder(IServiceScope scope, Reminder reminder)
	{
		var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
		await emailSender.SendEmailAsync(reminder.To, "Reminder", reminder.Content);
	}


	public Task StopAsync(CancellationToken cancellationToken)
	{
		_timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}
}
