using GeneralizableMoebooruAPI.Bases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GeneralizableMoebooruAPI.Features
{
    public class TagSearcher : FeatureBase
    {
        public TagSearcher(APIWrapperOption option) : base(option)
        {

        }

        public IEnumerable<Tag> SearchTags(string keyword)
        {
            var arr = HttpRequest.CreateRequest($"{Option.ApiBaseUrl}tag.json?order=name&limit=0&name={keyword}")
                .GetJsonContainer<JArray>();

            foreach (var item in arr)
            {
                yield return new Tag()
                {
                    Name = item["name"].ToString(),
                    Type = item["type"].ToString() switch
                    {
                        "0" => TagType.General,
                        "1" => TagType.Artist,
                        //"2" => TagType.Character,
                        "3" => TagType.Copyright,
                        "4" => TagType.Character,
                        "5" => TagType.Circle,
                        "6" => TagType.Faults,
                        _ => TagType.Unknown
                    }
                };
            }
        }
    }
}