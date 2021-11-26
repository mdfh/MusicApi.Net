namespace MusicApi.Models.Api
{
    public class AlbumApiModel
    {
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public IFormFile Image { get; set; }
    }
}
