using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Duration { get; set; }

        public DateTime UploadedDate { get; set; }

        public bool IsFeatured { get; set; }

        public string ImageUrl { get; set; }

        public String AudioUrl;

        public int ArtistId { get; set; }

        public int? AlbumId { get; set; }
    }
}
