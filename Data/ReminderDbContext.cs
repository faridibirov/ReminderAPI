using Microsoft.EntityFrameworkCore;
using ReminderAPI.Models;

namespace ReminderAPI.Data;

public class ReminderDbContext : DbContext
{
	public ReminderDbContext(DbContextOptions<ReminderDbContext> options) : base(options) { }

	public DbSet<Reminder> Reminders { get; set; }

}
