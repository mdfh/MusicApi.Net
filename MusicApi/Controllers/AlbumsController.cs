﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Database;
using MusicApi.Helper;
using MusicApi.Models;
using MusicApi.Models.Api;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] AlbumApiModel apiModel)
        {
            var imageUrl = await FileHelper.UploadImage(apiModel.Image);

            var album = new Album();
            album.Name = apiModel.Name;
            album.ArtistId = apiModel.ArtistId;
            album.ImageUrl = imageUrl;

            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await (from album in _dbContext.Albums
                                 select new
                                 {
                                     Id = album.Id,
                                     Name = album.Name,
                                     ImageUrl = album.ImageUrl,
                                 }).ToListAsync();

            return Ok(albums);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Details(int albumId)
        {
            var artistDetails = await _dbContext.Albums
                .Where(album => album.Id == albumId)
                .Include(a => a.Songs)
                .ToListAsync();
            return Ok(artistDetails);
        }
    }
}
