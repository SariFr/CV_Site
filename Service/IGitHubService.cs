using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Service.DataEntities;

namespace Service;

public interface IGitHubService
{

    public Task<List<Portfolio>> GetPortfolio();
    public Task<List<Repository>> SearchRepositoriesAsync(string? userName, string? repoName, string? language);



}
