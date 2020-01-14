using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralizableMoebooruAPI
{
    public class APIWrapper
    {
        public APIWrapper(APIWrapperOption option)
        {
            option.Check();
            this.option = option;

            option.Log.Info($"Created new API wrapper for {option.ApiBaseUrl}");
        }

        private APIWrapperOption option;

        private ImageFetcher _fetcher;
        public ImageFetcher GetImageFetcher => _fetcher ?? (_fetcher = new ImageFetcher(option));
    }
}
