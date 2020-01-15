using GeneralizableMoebooruAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeneralizableMoebooruAPI.Features
{
    public class FeatureBase
    {
        protected APIWrapperOption Option { get; private set; }

        protected ILog Log => Option.Log;
        protected IHttpRequest HttpRequest => Option.HttpRequest;

        public FeatureBase(APIWrapperOption option)
        {
            this.Option = option;

            Log.Info($"Created new {this.GetType().Name} for {option.ApiBaseUrl}");
        }
    }
}
