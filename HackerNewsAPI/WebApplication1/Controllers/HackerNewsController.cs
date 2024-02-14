using HackerNewsAPI.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace HackerNewsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private static readonly HttpClient _client = new HttpClient();
        string projectDirectory = Directory.GetCurrentDirectory();

        [HttpGet]
        [Route("GetBestStories")]
        public async Task<IActionResult> GetBestStoriesAsync(int n)
        {
            DataRepository dataRepository = new DataRepository(projectDirectory + "/Repo/Data.json");

            string apiUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
            List<StoryItem> BestStories = new List<StoryItem>();
            // Make GET request to the API
            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                List<int> bestStoriesIds = JsonConvert.DeserializeObject<List<int>>(responseBody);
                List<StoryItem> stories = dataRepository.ReadData();

                foreach (var item in bestStoriesIds)
                {
                    if (!stories.Any(x => x.id == item))
                    {
                        string apiDetailUrl = "https://hacker-news.firebaseio.com/v0/item/" + item + ".json";

                        HttpResponseMessage responseDetails = await _client.GetAsync(apiDetailUrl);
                        if (responseDetails.IsSuccessStatusCode)
                        {
                            string rsBody = await responseDetails.Content.ReadAsStringAsync();
                            StoryItem storyItem = JsonConvert.DeserializeObject<StoryItem>(rsBody);
                            stories.Add(storyItem);
                            BestStories.Add(storyItem);
                        }
                    }
                    else
                    {
                        StoryItem storyItem = stories.Where(x => x.id == item).First();
                        BestStories.Add(storyItem);
                    }
                }
                dataRepository.WriteData(stories);  
            }
            if (BestStories.Count > 0)
            {
                return new JsonResult(BestStories.OrderByDescending(x => x.score).Take(n).ToList());
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
