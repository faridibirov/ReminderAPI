namespace ReminderAPI.Models;

public class Reminder
{
	public int Id { get; set; }
	public string To { get; set; }
	public string Content { get; set; }
	public DateTime SendAt { get; set; }
	public string Method { get; set; }

	public bool IsSent { get; set; } = false;
}
