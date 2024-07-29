using System.Diagnostics;

namespace FlickrRandomPhotoBot.Service
{
	public class BotHostedService : IHostedService, IDisposable
	{
		private TelegramBot _telegramBot;

		public BotHostedService(TelegramBot telegramBot)
		{
			_telegramBot = telegramBot;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_telegramBot.Start();
			Debug.WriteLine("Bot started");
			return Task.CompletedTask;

		}

		public Task StopAsync(CancellationToken cancellationToken)
		{

			_telegramBot.Stop();
			Debug.WriteLine("Bot stoped");
			return Task.CompletedTask;
		}

		public void Dispose()
		{

		}
	}
}
