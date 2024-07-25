using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandDiscordBot
{
    public static class Structures
    {
        public class Response
        {
            [JsonProperty("player_count")]
            public int PlayerCount;

            [JsonProperty("result")]
            public int Result;
        }

        public class Root
        {
            [JsonProperty("response")]
            public Response Response;
        }


    }
}
