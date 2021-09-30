using System;
using System.Net;

namespace WeatherSvr
{
    class NoKeepAliveWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (request is HttpWebRequest)
                ((HttpWebRequest)request).KeepAlive = false;

            return request;
        }
    }
}
