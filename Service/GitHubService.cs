using Octokit;
using static Service.GitHubService;

namespace Service;

public class GitHubService : IGitHubService
{ 
    private readonly GitHubClient _client;
    private readonly GitHubIntegrationOptions _options;
    public GitHubService()
    {
            _client =new GitHubClient(new ProductHeaderValue("my-cool-app"));

    }


public async Task<int> GetUserFollowersAsync(string userName)

        {

            var user = await _client.User.Get(userName);

            return user.Followers;

        }


public async Task<List<Repository>> SearchRepositoriesInCSharp()

        {

            var request = new SearchRepositoriesRequest("repo-name") { Language =  Language.CSharp };

            var result = await _client.Search.SearchRepo(request);

            return result.Items.ToList();

        }

    }


