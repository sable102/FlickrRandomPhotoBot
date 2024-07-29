namespace FlickrRandomPhotoBot.Models
{
	public class Photo
	{
		public string Id { get; set; }
		public string Owner { get; set; }
		public string Secret { get; set; }
		public string Server { get; set; }
		public int Farm { get; set; }
		public string Title { get; set; }
		public bool IsPublic { get; set; }
		public bool IsFriend { get; set; }
		public bool IsFamily { get; set; }
	}

	public class ContentData
	{
		public int Page { get; set; }
		public int Pages { get; set; }
		public int Perpage { get; set; }
		public int Total { get; set; }
		public List<Photo> Photo { get; set; }
	}

	public class FlickrResponseContent
	{
		public ContentData Photos { get; set; }
	}
}
