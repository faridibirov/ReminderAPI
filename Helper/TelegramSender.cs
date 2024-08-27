namespace ReminderAPI.Helper;

public class TelegramSender
{
	private readonly HttpClient _httpClient;

	private readonly string _botToken;

	public TelegramSender(HttpClient httpClient, IConfiguration _config)
	{
		_httpClient = httpClient;
		_botToken = _config.GetValue<string>("Telegram:TelegramBotToken");
	}

	public async Task SendTelegramReminderAsync(string chatId, string message)
	{
		var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("chat_id", chatId),
			new KeyValuePair<string, string>("text", message),

		});

		var response = await _httpClient.PostAsync(apiUrl, content);

		response.EnsureSuccessStatusCode();
	}
}
