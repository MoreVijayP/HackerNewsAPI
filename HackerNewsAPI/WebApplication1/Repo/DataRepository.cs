using Newtonsoft.Json;

namespace HackerNewsAPI.Repo
{
    public class DataRepository
    {
        private readonly string _filePath;

        public DataRepository(string filePath)
        {
            _filePath = filePath;
        }
        public List<StoryItem> ReadData()
        {
            if (!File.Exists(_filePath))
                return new List<StoryItem>();

            string jsonContent = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<StoryItem>>(jsonContent);
        }

        // Write data to JSON file
        public void WriteData(List<StoryItem> data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            File.WriteAllText(_filePath, jsonContent);
        }
    }
}
