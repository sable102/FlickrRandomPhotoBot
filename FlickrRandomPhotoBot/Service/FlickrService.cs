using FlickrRandomPhotoBot.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace FlickrRandomPhotoBot.Service
{
	public class FlickrService
	{
		private static string _accessToken;

		public FlickrService(string accessToken)
		{
			_accessToken = accessToken;
		}

		public async Task<List<string>> GetRandomImagesFromFlickr(int numberOfImages)
		{
			var imageUrls = await GetImageUrlsFromFlickr();

			// Проверяем, что получено достаточно изображений
			if (imageUrls.Count < numberOfImages)
			{
				throw new InvalidOperationException("Не хватает изображений для выбора.");
			}

			// Выбираем случайные изображения
			var random = new Random();
			var randomImages = imageUrls.OrderBy(x => random.Next()).Take(numberOfImages).ToList();

			return randomImages;
		}

		private static async Task<List<string>> GetImageUrlsFromFlickr()
		{
			var imageUrls = new List<string>();

			using (var client = new HttpClient())
			{
				string requestUri = $"https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key={_accessToken}&tags=nature&format=json&nojsoncallback=1&per_page=100"; // Запрос для получения изображений

				var response = await client.GetAsync(requestUri);

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var contentData = JsonConvert.DeserializeObject<FlickrResponseContent>(content);
					foreach (var photo in contentData.Photos.Photo)
					{
						var photoUrl = $"https://live.staticflickr.com/{photo.Server}/{photo.Id}_{photo.Secret}_b.jpg";
						imageUrls.Add(photoUrl);
					}
				}
				else
				{
					throw new Exception("Не удалось получить изображения: " + response.ReasonPhrase);
				}
			}

			return imageUrls;
		}
	}
}
