using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FlickrRandomPhotoBot.Service
{
	public class TelegramBot
	{
		private readonly FlickrService _flickrService;
		private readonly CancellationTokenSource _cts;
		private readonly TelegramBotClient botClient;


		public TelegramBot(FlickrService flickrService, string botToken)
		{
			_flickrService = flickrService;
			_cts = new CancellationTokenSource();
			botClient = new TelegramBotClient(botToken);
		}

		public void Start()
		{
			
			botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new Telegram.Bot.Polling.ReceiverOptions(), _cts.Token);
		}

		public void Stop()
		{
			_cts.Cancel();
		}


		private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update messageEventArgs, CancellationToken cancellationToken)
		{
			if (messageEventArgs.Message is not { } message)
				return;

			if (message.Text is not { } messageText)
				return;


			if (message == null || message.Type != MessageType.Text) return;

			switch (message.Text)
			{
				case "/start":
					await botClient.SendTextMessageAsync(message.Chat.Id, "Привет! Я бот, который может присылать фото из flickr.");
					break;
				case "/photo":
					await SendPhotoFromFlickrPhotos(message.Chat.Id);
					break;
			}
		}

		private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
		{
			Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
			return Task.CompletedTask;
		}


		private async Task SendPhotoFromFlickrPhotos(long chatId)
		{
			// Получите список фотографий из Flickr.
			var photosList = await _flickrService.GetRandomImagesFromFlickr(3);
			var random = new Random();
			// Выберите фотографию, которую вы хотите отправить.
			var photoUrls = photosList.OrderBy(x => random.Next()).Take(3);

			foreach (var photoUrl in photoUrls)
			{
				// Отправьте фотографию.
				await botClient.SendPhotoAsync(chatId, InputFile.FromUri(photoUrl));
			}
		}
	}
}