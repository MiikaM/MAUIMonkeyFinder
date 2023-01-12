using System.Net.Http.Json;

namespace MAUIMonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpClient;
    List<Monkey> monkeyList = new();

    public MonkeyService()
    {
        httpClient = new HttpClient();
    }
    public async Task<List<Monkey>> GetMonkeys()
    {
        if (monkeyList?.Count > 0)
            return monkeyList;

        string url = "https://www.montemagno.com/monkeys.json";

        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
        }
        return monkeyList;
    }
}