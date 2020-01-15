using GeneralizableMoebooruAPI.Features;
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

        private ImageFetcher fetcher;
        public ImageFetcher ImageFetcher => fetcher ?? (fetcher = new ImageFetcher(option));

        private ImageVoter voter;
        public ImageVoter ImageVoter => voter ?? (voter = new ImageVoter(option));

        private AccountManager account_manager;
        public AccountManager AccountManager => account_manager ?? (account_manager = new AccountManager(option));

        private TagSearcher tag_searcher;
        public TagSearcher TagSearcher => tag_searcher ?? (tag_searcher = new TagSearcher(option));
    }
}
