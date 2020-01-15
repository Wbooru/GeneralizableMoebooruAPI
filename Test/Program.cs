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
            wrapper.AccountManager.Login("MikiraSora","q6523230");

            wrapper.ImageVoter.SetVoteValue(298297,true);
            var is_vote = wrapper.ImageVoter.IsVoted(298297);

            foreach (var info in wrapper.ImageFetcher.GetImages(new[] { "stockings", "penis" }).Take(20))
            {
                Console.WriteLine(info.Id);
            }

            Console.ReadLine();
        }
    }
}
