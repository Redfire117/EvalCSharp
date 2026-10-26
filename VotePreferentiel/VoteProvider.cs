using System.Net.Http.Json;

namespace VotePreferentiel;

public class VoteProvider
{
    private readonly HttpClient _httpClient;

    public VoteProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VoteModel> GetVotes()
    {
        string url = "https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.json";

        var votes = await _httpClient.GetFromJsonAsync<VoteModel>(url);

        return votes ?? throw new Exception("Impossible de récupérer les votes.");
    }
}