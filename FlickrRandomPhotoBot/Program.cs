
using FlickrRandomPhotoBot.Service;


var builder = Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration((context, config) =>
	{
		var environment = context.HostingEnvironment.EnvironmentName;

		config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
					.AddEnvironmentVariables();
	})
	.ConfigureServices((context, services) =>
	{
		// Получение конфигурации
		var configuration = context.Configuration;
		var flickrToken = configuration["AppSettings:FlickrToken"];
		var telegramBotToken = configuration["AppSettings:TelegramBotToken"];

		// Добавление сервисов в контейнер зависимостей
		services.AddSingleton<IHostedService>(sp =>
		{
			return new BotHostedService(new TelegramBot(new FlickrService(flickrToken),telegramBotToken));
		});
	});


var app = builder.Build();




app.Run();
