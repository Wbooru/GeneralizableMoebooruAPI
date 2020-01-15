using System;
using System.Collections.Generic;

namespace GeneralizableMoebooruAPI.Bases
{
    public class ImageInfo
    {
        public int Id { get; set; }
        public Rating Rating { get; internal set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int Score { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public ImageUrl ThumbnailImageUrl { get; set; }
        public IEnumerable<ImageUrl> ImageUrls { get; set; }
    }
}