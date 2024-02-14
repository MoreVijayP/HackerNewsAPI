using HackerNewsAPI.Repo;
using Newtonsoft.Json;

namespace HackerNewsAPI
{
    public static class StartupLoadData
    {
        private static readonly HttpClient _client = new HttpClient();
        public static void InvokeAsync()
        {
            string projectDirectory = Directory.GetCurrentDirectory();

            DataRepository dataRepository = new DataRepository(projectDirectory + "/Repo/Data.json");
            string apiUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";

            // Make GET request to the API
            HttpResponseMessage response =  _client.GetAsync(apiUrl).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                string responseBody =  response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                List<int> bestStories = JsonConvert.DeserializeObject<List<int>>(responseBody);
                List<StoryItem> stories = new List<StoryItem>();
                foreach (var item in bestStories)
                {
                    string apiDetailUrl = "https://hacker-news.firebaseio.com/v0/item/" + item + ".json";

                    // Make GET request to the API
                    HttpResponseMessage responseDetails =  _client.GetAsync(apiDetailUrl).GetAwaiter().GetResult();
                    if (responseDetails.IsSuccessStatusCode)
                    {
                        string rsBody =  responseDetails.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        StoryItem storyItem = JsonConvert.DeserializeObject<StoryItem>(rsBody);
                        stories.Add(storyItem);
                    }
                }
                dataRepository.WriteData(stories);
            }

        }
    }
}
