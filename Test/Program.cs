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
                ApiBaseUrl = "http://konachan.net/"
            };

            var wrapper = new APIWrapper(option);

            var images = wrapper.GetImageFetcher.GetImages(null).Take(20).ToArray();

            foreach (var image in images)
            {
                Console.WriteLine(image.Id);
            }

            Console.ReadLine();
        }
    }
}
