using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace Service;

public interface IGitHubService
{

    public Task<int> GetUserFollowersAsync(string userName);
    public Task<List<Repository>> SearchRepositoriesInCSharp();



}
