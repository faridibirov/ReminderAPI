using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReminderAPI.Data;
using ReminderAPI.Models;

namespace ReminderAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RemindersController : ControllerBase
{
	private readonly ReminderDbContext _db;

	public RemindersController(ReminderDbContext db)
	{
		_db = db;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders()
	{
		return await _db.Reminders.ToListAsync();
	}

	[HttpGet("{id:int}", Name = "GetReminder")]
	public async Task<ActionResult<Reminder>> GetReminder(int id)
	{
		var reminder = await _db.Reminders.FirstOrDefaultAsync(r => r.Id == id);

		if (reminder == null)
		{
			return NotFound();
		}

		return reminder;
	}

	[HttpPost]
	public async Task<ActionResult<Reminder>> CreateReminder ([FromBody] Reminder reminder)
	{
		if (reminder.SendAt<=DateTime.Now.ToLocalTime())
		{
			return BadRequest("The send date must be in the future.");
		}

		if(reminder.Method.ToLower()!="email" && reminder.Method.ToLower() != "telegram")

		{
			return BadRequest("The method can be only email or telegram");
		}

		_db.Reminders.Add(reminder);

		await _db.SaveChangesAsync();

		return CreatedAtRoute(nameof(GetReminder), new {id=reminder.Id }, reminder);
	}

	[HttpPut("{id:int}", Name = "UpdateReminder")]
	public async Task<ActionResult<Reminder>> UpdateReminder(int id, [FromBody] Reminder updateReminder)
	{
		if(updateReminder==null || id!=updateReminder.Id)
		{
			return BadRequest();
		}

		if (updateReminder.SendAt <= DateTime.Now.ToLocalTime())
		{
			return BadRequest("The send date must be in the future.");
		}

		if (updateReminder.Method.ToLower() != "email" && updateReminder.Method.ToLower() != "telegram")

		{
			return BadRequest("The method can be only email or telegram");
		}

		_db.Update(updateReminder);
		_db.SaveChanges();

		return Ok();
	}

	[HttpDelete("{id:int}", Name = "DeleteReminder")]
	public async Task<ActionResult<Reminder>> DeleteReminder(int id)
	{
		if(id==0)
		{
			return BadRequest();
		}

		var reminder = await _db.Reminders.FirstOrDefaultAsync(r=>r.Id==id);

		if(reminder==null)
		{
			return NotFound();
		}

		 _db.Remove(reminder);
		_db.SaveChanges();


		return Ok();

	}

}
