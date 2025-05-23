using Newtonsoft.Json;
using Questao2;
using System.Text.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();

        /*Output expected:
          Team Paris Saint - Germain scored 109 goals in 2013
          Team Chelsea scored 92 goals in 2014*/
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        using HttpClient client = new HttpClient();

        totalGoals += GetGoalsForTeam(client, team, year, isTeam1: true).Result;

        totalGoals += GetGoalsForTeam(client, team, year, isTeam1: false).Result;

        return totalGoals;
    }

    public static async Task<int> GetGoalsForTeam(HttpClient client, string team, int year, bool isTeam1)
    {
        int page = 1;
        int totalPages = 1;
        int totalGoals = 0;
        string teamParam = isTeam1 ? "team1" : "team2";

        while (page <= totalPages)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamParam}={Uri.EscapeDataString(team)}&page={page}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(json, options);

            totalPages = result.Total_Pages;

            foreach (var match in result.Data)
            {
                string goalStr = isTeam1 ? match.Team1Goals : match.Team2Goals;
                if (int.TryParse(goalStr, out int goals))
                    totalGoals += goals;
            }

            page++;
        }

        return totalGoals;
    }

}