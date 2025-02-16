using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using Service;
using Service.DataEntities;

[Route("api/github")]
[ApiController]
public class GitHubController : ControllerBase
{
    private readonly IGitHubService _gitHubService;

    public GitHubController(IGitHubService gitHubService)
    {
        _gitHubService = gitHubService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRepositories([FromQuery] string? username , [FromQuery] string? repoName, [FromQuery] string? language)
    {
         var repositories = await _gitHubService.SearchRepositoriesAsync(username,repoName, language);
        return Ok(repositories);
    }

    [HttpGet("portfolio")]
    public async Task<IActionResult> GetPortfolio()
    {
        var repositories = await _gitHubService.GetPortfolio();
        return Ok(repositories);
    }

}
