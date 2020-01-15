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
        public ImageFetcher ImageFetcher => _fetcher ?? (_fetcher = new ImageFetcher(option));

        private ImageVoter _voter;
        public ImageVoter ImageVoter => _voter ?? (_voter = new ImageVoter(option));

        private AccountManager _account_manager;
        public AccountManager AccountManager => _account_manager ?? (_account_manager = new AccountManager(option));
    }
}
