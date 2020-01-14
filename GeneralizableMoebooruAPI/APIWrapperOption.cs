﻿using GeneralizableMoebooruAPI.Interfaces;
using System;

namespace GeneralizableMoebooruAPI
{
    public class APIWrapperOption
    {
        /// <summary>
        /// example : https://yande.re/
        /// </summary>
        public string ApiBaseUrl { get; set; }

        public ulong PicturesCountPerRequest { get; set; } = 20;

        public bool TryGetValidFileSize { get; set; } = false;

        public IHttpRequest HttpRequest { get; set; } = HttpRequestDefaultImplement.Default;

        public ILog Log { get; set; } = LogDefaultImplement.Default;

        internal void Check()
        {
            if (string.IsNullOrWhiteSpace(ApiBaseUrl))
                throw new Exception("proprty ApiBaseUrl must be set.");
        }
    }
}