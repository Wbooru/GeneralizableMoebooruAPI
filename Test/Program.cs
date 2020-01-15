using GeneralizableMoebooruAPI;
using System;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var option = new APIWrapperOption()
            {
                ApiBaseUrl = "http://konachan.net/",
                PasswordSalts = "So-I-Heard-You-Like-Mupkids-?--your-password--"
            };

            var wrapper = new APIWrapper(option);

            var u = wrapper.ImageFetcher.GetImageInfo(298297);

            var a = wrapper.TagSearcher.SearchTags("stock").ToArray();

            Console.ReadLine();
        }
    }
}
