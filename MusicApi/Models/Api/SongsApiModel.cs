using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models.Api
{
    public class SongsApiModel
    {
        public string Title { get; set; }
        public string Duration { get; set; }
        public bool IsFeatured { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        [NotMapped]
        public IFormFile AudioFile { get; set; }
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }
    }
}
