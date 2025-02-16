using Microsoft.Extensions.Options;
using Octokit;
using Service.DataEntities;
using static Service.GitHubService;

namespace Service;

public class GitHubService : IGitHubService
{ 
    private readonly GitHubClient _client;
    private readonly GitHubIntegrationOptions _options;

    public GitHubService(IOptions<GitHubIntegrationOptions> options)
    {
        _options = options.Value;

        _client = new GitHubClient(new ProductHeaderValue("my-cool-app"))
        {
            Credentials = new Credentials(_options.Token)
        };
    }


    public async Task<List<Portfolio>> GetPortfolio()
    {
        var repositories = await _client.Repository.GetAllForCurrent();
        var portfolio = new List<Portfolio>();

        foreach (var repo in repositories)
        {
            var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);
            var languageList = languages.Select(lang => lang.Name).ToList();

            var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
            int pullRequestCount = pullRequests.Count;

            portfolio.Add(new Portfolio
            {
                Name = repo.Name,
                HtmlUrl = repo.HtmlUrl,
                Stars = repo.StargazersCount,
                LastCommitDate = repo.PushedAt?.DateTime,
                Languages = languageList,
                PullRequestCount = pullRequestCount
            });
        }

        return portfolio;
    }


    public async Task<List<Repository>> SearchRepositoriesAsync(string? userName, string? repoName, string? language)
    {
        Language? languageEnum = null;
        if (!string.IsNullOrWhiteSpace(language) && Enum.TryParse(language, true, out Language parsedLanguage))
        {
            languageEnum = parsedLanguage;
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            var userRepositories = await _client.Repository.GetAllForUser(userName);

            var filteredRepos = userRepositories
                .Where(repo =>
                    (string.IsNullOrWhiteSpace(repoName) || repo.Name.Contains(repoName, StringComparison.OrdinalIgnoreCase)) &&
                    (languageEnum == null || repo.Language?.Equals(languageEnum.ToString(), StringComparison.OrdinalIgnoreCase) == true))
                .ToList();

            return filteredRepos;
        }

        var request = new SearchRepositoriesRequest(string.IsNullOrWhiteSpace(repoName) ? "github" : repoName)
        {
            Language = languageEnum
        };

        var result = await _client.Search.SearchRepo(request);
        return result.Items.ToList();
    }


}


