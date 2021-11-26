namespace MusicApi.Models.Api
{
    public class ArtistsApiModel
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public IFormFile Image { get; set; }
    }
}
