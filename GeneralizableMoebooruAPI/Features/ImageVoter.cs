using GeneralizableMoebooruAPI.Bases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralizableMoebooruAPI.Features
{
    public class ImageVoter : FeatureBase
    {
        public ImageVoter(APIWrapperOption option) : base(option)
        {

        }

        public ValueTask<bool> IsVotedAsync(ImageInfo info) => IsVotedAsync(info.Id);

        public async ValueTask<bool> IsVotedAsync(int id)
        {
            if (Option.CurrentUser == null)
                throw new Exception("投票功能需要事先用户登录.");

            var result = await (await HttpRequest.CreateRequestAsync($"{Option.ApiBaseUrl}favorite/list_users.json?id={id}")).GetJsonObjectAsync();
            var user_list = result["favorited_users"].ToString().Split(',');

            return user_list
                .Any(x => x.Equals(Option.CurrentUser.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public ValueTask SetVoteValueAsync(ImageInfo info, bool vote_up) => SetVoteValueAsync(info.Id, vote_up);
        public async ValueTask SetVoteValueAsync(int id,bool vote_up)
        {
            var value = vote_up ? Option.VoteValue : 0;

            if (Option.CurrentUser == null)
                throw new Exception("投票功能需要事先用户登录.");

            var url = $"{Option.ApiBaseUrl}post/vote.json?" + $"score={value}&id={id}&password_hash={Option.CurrentUser.PasswordHash}&login={Option.CurrentUser.Name}";

            var response = await HttpRequest.CreateRequestAsync(url, req => req.Method = "POST");
            using var reader = new StreamReader(response.GetResponseStream());
            var t = await reader .ReadToEndAsync();

            if (JsonConvert.DeserializeObject(t) is JObject result)
            {
                if (!result["success"].ToObject<bool>())
                    throw new Exception(result["reason"].ToString());
                else
                    Log.Info($"Voted item {id} , score {value}");
            }
            else
                Log.Error($"Can't get json object from response.");
        }
    }
}