using GeneralizableMoebooruAPI.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace GeneralizableMoebooruAPI
{
    public class ImageVoter : FeatureBase
    {
        public ImageVoter(APIWrapperOption option) : base(option)
        {

        }

        public bool IsVoted(ImageInfo info) => IsVoted(info.Id);

        public bool IsVoted(int id)
        {
            if (Option.CurrentUser == null)
                throw new Exception("投票功能需要事先用户登录.");

            var result = HttpRequest.CreateRequest($"{Option.ApiBaseUrl}favorite/list_users.json?id={id}").GetJsonObject();
            var user_list = result["favorited_users"].ToString().Split(',');

            return user_list
                .Any(x => x.Equals(Option.CurrentUser.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public void SetVoteValue(ImageInfo info, bool vote_up) => SetVoteValue(info.Id, vote_up);
        public void SetVoteValue(int id,bool vote_up)
        {
            var value = vote_up ? Option.VoteValue : 0;

            if (Option.CurrentUser == null)
                throw new Exception("投票功能需要事先用户登录.");

            var url = $"{Option.ApiBaseUrl}post/vote.json?" + $"score={value}&id={id}&password_hash={Option.CurrentUser.PasswordHash}&login={Option.CurrentUser.Name}";

            var response = HttpRequest.CreateRequest(url, req => req.Method = "POST");
            using var reader = new StreamReader(response.GetResponseStream());
            var t = reader.ReadToEnd();

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