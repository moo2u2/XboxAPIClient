using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;
using XboxAPIClient.Models.V2;

namespace XboxAPIClient
{
    /// <summary>
    /// Client wrapper for the unofficial XboxAPI hosted at http://www.xboxapi.com/.
    /// </summary>
    public class XboxAPI
    {
        private const string baseUrl = "http://www.xboxapi.com/";

        private string apiKey;

        public XboxAPI(string apiKey)
        {
            this.apiKey = apiKey;
        }

        #region Public Methods

        public Task<AccountProfile> AccountProfile()
        {
            RestRequest request = new RestRequest("/v2/profile");
            return executeAndDeserialize<AccountProfile>(request);
        }

        public Task<AccountXuid> AccountXuid()
        {
            RestRequest request = new RestRequest("/v2/accountXuid");
            return executeAndDeserialize<AccountXuid>(request);
        }

        public Task<Gamercard> Gamercard(long xuid)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/gamercard");
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<Gamercard>(request);
        }

        public Task<GamertagXuid> GamertagXuid(string gamertag)
        {
            RestRequest request = new RestRequest("/v2/xuid/{gamertag}");
            request.AddUrlSegment("gamertag", gamertag);
            return executeAndDeserialize<GamertagXuid>(request);
        }

        public Task<GameStats> GameStats(long xuid, string titleId)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/game-stats/{titleId}");
            request.AddUrlSegment("xuid", xuid);
            request.AddUrlSegment("titleId", titleId);
            return executeAndDeserialize<GameStats>(request);
        }

        public Task<Presence> Presence(long xuid)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/presence");
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<Presence>(request);
        }

        public Task<Profile> Profile(long xuid)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/profile");
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<Profile>(request);
        }

        public Task<Xbox360Games> Xbox360Games(long xuid)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/xbox360games");
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<Xbox360Games>(request);
        }

        public Task<Xbox360GameAchievement[]> Xbox360GameAchievements(string titleId, long xuid)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/achievements/{titleId}");
            request.AddUrlSegment("titleId", titleId);
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<Xbox360GameAchievement[]>(request);
        }

        public Task<XboxOneGameAchievement[]> XboxOneGameAchievements(string titleId, long xuid)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/achievements/{titleId}");
            request.AddUrlSegment("titleId", titleId);
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<XboxOneGameAchievement[]>(request);
        }

        public Task<XboxOneGames> XboxOneGames(long xuid)
        {
            RestRequest request = new RestRequest("/v2/{xuid}/xboxonegames");
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<XboxOneGames>(request);
        }

        public Task<XuidGamertag> XuidGamertag(long xuid)
        {
            RestRequest request = new RestRequest("/v2/gamertag/{xuid}");
            request.AddUrlSegment("xuid", xuid);
            return executeAndDeserialize<XuidGamertag>(request);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Executes the given request using this object's API key.
        /// </summary>
        /// <param name="request">The request to be executed.</param>
        private async Task<RestResponse> execute(RestRequest request)
        {
            RestClient client = new RestClient(baseUrl);
            client.AddDefaultHeader("X-AUTH", apiKey);

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            return (RestResponse)(await taskCompletion.Task);
        }

        /// <summary>
        /// Executes the given request using this object's API key, and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type of object to which the response should be deserialized</typeparam>
        /// <param name="request">The request to be executed.</param>
        /// <returns>The deserialized response object.</returns>
        private Task<T> executeAndDeserialize<T>(RestRequest request)
        {
            return execute(request).ContinueWith(restResponse =>
            {
                return JsonConvert.DeserializeObject<T>(restResponse.Result.Content);
            });
        }

        #endregion Private Methods
    }
}