using GeneralizableMoebooruAPI;
using GeneralizableMoebooruAPI.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var option = new APIWrapperOption()
            {
                ApiBaseUrl = "https://yande.re/",
                PasswordSalts = "choujin-steiner--your-password--"
            };

            var wrapper = new APIWrapper(option);

            var i = await wrapper.AccountManager.LoginAsync("MikiraSora","q6523230");

            var u = (await wrapper.ImageFetcher.GetImageInfoAsync(298297)).DetailUrl;

            await foreach (var tag in wrapper.TagSearcher.SearchTagsAsync("stock"))
            {
                Console.WriteLine(tag.Name);
            }

            Console.ReadLine();
        }
    }
}
