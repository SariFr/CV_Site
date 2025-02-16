using Microsoft.Extensions.Options;
using Octokit;
using Service.DataEntities;
using static Service.GitHubService;

namespace Service;

public class GitHubService : IGitHubService
{ 
    private readonly GitHubClient _client;
    private readonly GitHubIntegrationOptions _options;
    //public GitHubService(IOptions<GitHubIntegrationOptions> options) 
    //{
    //    _client =new GitHubClient(new ProductHeaderValue("my-cool-app"));
    //    _options = options.Value;

    //}
    public GitHubService(IOptions<GitHubIntegrationOptions> options)
    {
        _options = options.Value;

        _client = new GitHubClient(new ProductHeaderValue("my-cool-app"))
        {
            Credentials = new Credentials(_options.Token)
        };
    }




    //public async Task<int> GetUserFollowersAsync(string userName)

    //        {

    //            var user = await _client.User.Get(userName);

    //            return user.Followers;

    //        }

    public async Task<List<RepositoryPortfolio>> GetPortfolio()
    {
        var repositories = await _client.Repository.GetAllForCurrent();
        var portfolio = new List<RepositoryPortfolio>();

        foreach (var repo in repositories)
        {
            var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);
            var languageList = languages.Select(lang => lang.Name).ToList();

            var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
            int pullRequestCount = pullRequests.Count;

            portfolio.Add(new RepositoryPortfolio
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


    //public async Task<List<Repository>> SearchRepositoriesAsync(string? userName, string? repoName, string? language)
    //{
    //    Language? languageEnum = null;

    //    if (!string.IsNullOrWhiteSpace(language) && Enum.TryParse(language, true, out Language parsedLanguage))
    //    {
    //        languageEnum = parsedLanguage;
    //    }

    //    var request = new SearchRepositoriesRequest(repoName ?? string.Empty)
    //    {
    //        Language = languageEnum,
    //        User = userName
    //    };


    //    var result = await _client.Search.SearchRepo(request);
    //    return result.Items.ToList();
    //}
    //public async Task<List<Repository>> SearchRepositoriesAsync(string? userName, string? repoName, string? language)
    //{
    //    // אם כל הפרמטרים ריקים, נחזיר רשימה ריקה
    //    if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(repoName) && string.IsNullOrWhiteSpace(language))
    //    {
    //        return new List<Repository>();
    //    }

    //    Language? languageEnum = null;
    //    if (!string.IsNullOrWhiteSpace(language) && Enum.TryParse(language, true, out Language parsedLanguage))
    //    {
    //        languageEnum = parsedLanguage;
    //    }

    //    // בדיקה: אם repoName ריק, ניתן ערך ברירת מחדל תקף
    //    var request = new SearchRepositoriesRequest(string.IsNullOrWhiteSpace(repoName) ? "github" : repoName)
    //    {
    //        Language = languageEnum,
    //        User = userName
    //    };

    //    var result = await _client.Search.SearchRepo(request);
    //    return result.Items.ToList();
    //}

    public async Task<List<Repository>> SearchRepositoriesAsync(string? userName, string? repoName, string? language)
    {
        Language? languageEnum = null;
        if (!string.IsNullOrWhiteSpace(language) && Enum.TryParse(language, true, out Language parsedLanguage))
        {
            languageEnum = parsedLanguage;
        }

        // אם יש שם משתמש - נקבל ישירות את הריפוזיטוריז שלו
        if (!string.IsNullOrWhiteSpace(userName))
        {
            var userRepositories = await _client.Repository.GetAllForUser(userName);

            // סינון לפי שם ריפוזיטורי ושפה אם סופקו
            var filteredRepos = userRepositories
                .Where(repo =>
                    (string.IsNullOrWhiteSpace(repoName) || repo.Name.Contains(repoName, StringComparison.OrdinalIgnoreCase)) &&
                    (languageEnum == null || repo.Language?.Equals(languageEnum.ToString(), StringComparison.OrdinalIgnoreCase) == true))
                .ToList();

            return filteredRepos;
        }

        // חיפוש כללי כאשר אין שם משתמש
        var request = new SearchRepositoriesRequest(string.IsNullOrWhiteSpace(repoName) ? "github" : repoName)
        {
            Language = languageEnum
        };

        var result = await _client.Search.SearchRepo(request);
        return result.Items.ToList();
    }


}


