using GeneralizableMoebooruAPI.Bases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneralizableMoebooruAPI.Features
{
    public class ImageFetcher:FeatureBase
    {
        public ImageFetcher(APIWrapperOption option) : base(option)
        {
        }

        public IEnumerable<ImageInfo> GetImages(IEnumerable<string> tags=null, int page = 1)
        {
            var base_url = $"{Option.ApiBaseUrl}post.json?limit={Option.PicturesCountPerRequest}&";

            if (tags?.Any() ?? false)
                base_url += $"tags={string.Join("+", tags)}&";

            while (true)
            {
                JArray json;

                try
                {
                    var actual_url = $"{base_url}page={page}";

                    var response = HttpRequest.CreateRequest(actual_url);
                    using var reader = new StreamReader(response.GetResponseStream());

                    json = JsonConvert.DeserializeObject(reader.ReadLine()) as JArray;

                    if (json.Count == 0)
                        break;

                    page++;
                }
                catch (Exception e)
                {
                    Log.Error($"Get image failed {e.Message}, but It still continue to fetch..");
                    json = null;
                }

                if (json != null)
                    foreach (var item in json.Select(x => BuildItem(x)))
                    {
                        yield return item;
                    }
            }
        }

        private ImageInfo BuildItem(JToken pic_info)
        {
            ImageInfo item = new ImageInfo();

            item.Rating = pic_info["rating"].ToString() switch
            {
                "s" => Rating.Safe,
                "q" => Rating.Questionable,
                "e" => Rating.Explicit,
                _ => Rating.Questionable
            };

            item.Id = pic_info["id"].ToObject<int>();
            item.ThumbnailImageUrl = new ImageUrl
            {
                Url = pic_info["preview_url"].ToString(),
                ImageWidth = pic_info["preview_width"].ToObject<int>(),
                ImageHeight = pic_info["preview_height"].ToObject<int>(),
                UrlDescription = "Thumbnail"
            };

            item.Tags = pic_info["tags"].ToString().Split(' ').ToList();
            item.Author = pic_info["creator_id"].ToString();
            item.Source = pic_info["source"].ToString();
            item.CreateDateTime = DateTimeOffset.FromUnixTimeSeconds(pic_info["created_at"].ToObject<long>()).DateTime;
            item.Author = pic_info["author"].ToString();
            item.Score = pic_info["score"].ToObject<int>();

            List<ImageUrl> downloads = new List<ImageUrl>();

            downloads.Add(new ImageUrl()
            {
                UrlDescription = "Jpeg",
                ImageWidth = pic_info["jpeg_width"].ToObject<int>(),
                ImageHeight = pic_info["jpeg_height"].ToObject<int>(),
                FileLength = pic_info["jpeg_file_size"].ToObject<int>(),
                Url = pic_info["jpeg_url"].ToString(),
            });

            downloads.Add(new ImageUrl()
            {
                UrlDescription = "Preview",
                ImageWidth = pic_info["preview_width"].ToObject<int>(),
                ImageHeight = pic_info["preview_height"].ToObject<int>(),
                FileLength = 0,
                Url = pic_info["preview_url"].ToString(),
            });

            downloads.Add(new ImageUrl()
            {
                UrlDescription = "Sample",
                ImageWidth = pic_info["sample_width"].ToObject<int>(),
                ImageHeight = pic_info["sample_height"].ToObject<int>(),
                FileLength = pic_info["sample_file_size"].ToObject<int>(),
                Url = pic_info["sample_url"].ToString(),
            });

            downloads.Add(new ImageUrl()
            {
                UrlDescription = "File",
                ImageWidth = pic_info["width"].ToObject<int>(),
                ImageHeight = pic_info["height"].ToObject<int>(),
                FileLength = pic_info["file_size"].ToObject<int>(),
                Url = pic_info["file_url"].ToString(),
            });

            foreach (var info in downloads.Where(x => Option.TryGetValidFileSize && x.FileLength <= 0))
                info.FileLength = HttpRequest.CreateRequest(info.Url, req => req.Method = "HEAD").ContentLength;

            item.ImageUrls = downloads;

            return item;
        }

        public ImageInfo GetImageInfo(int id)
        {
            try
            {
                var response = HttpRequest.CreateRequest($"{Option.ApiBaseUrl}post/show/{id}");

                using var reader = new StreamReader(response.GetResponseStream());
                var content = reader.ReadToEnd();

                const string CONTENT_HEAD = "Post.register_resp(";
                var start_index = content.LastIndexOf(CONTENT_HEAD);

                if (start_index < 0)
                {
                    Log.Warn($"Can't get any information with id {id}.");
                    return null;
                }

                start_index += CONTENT_HEAD.Length;
                StringBuilder builder = new StringBuilder(1024);
                int stack = 1;

                foreach (var ch in content.Skip(start_index))
                {
                    if (ch == ')')
                    {
                        stack--;
                        if (stack == 0)
                            break;
                    }

                    if (ch == '(')
                        stack++;

                    builder.Append(ch);
                }

                var result = JsonConvert.DeserializeObject(builder.ToString()) as JObject;
                return BuildItem((result["posts"] as JArray).FirstOrDefault());
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return null;
            }
        }
    }
}