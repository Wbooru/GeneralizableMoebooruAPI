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
                ApiBaseUrl = "https://yande.re/",
                PasswordSalts = "choujin-steiner--your-password--"
            };

            var wrapper = new APIWrapper(option);

            var i = wrapper.AccountManager.Login("MikiraSora","");

            var u = wrapper.ImageFetcher.GetImageInfo(298297);

            var a = wrapper.TagSearcher.SearchTags("stock").ToArray();

            Console.ReadLine();
        }
    }
}
