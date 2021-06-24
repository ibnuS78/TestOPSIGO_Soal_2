using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOPSIGO_Soal_2.Models;
using Newtonsoft.Json;

namespace TestOPSIGO_Soal_2.Controllers
{
    [Route("api/GitHub")]
    [ApiController]
    public class ApiGithubController : ControllerBase
    {
        [HttpGet,Route("ListRepo")]
        public async Task<IReadOnlyList<Repository>> ListRepoAsync()
        {
            var setting = GetSetting();
            var productInformation = new ProductHeaderValue(setting["Identity"]);
            var credentials = new Credentials(setting["AccessToken"]);
            var client = new GitHubClient(productInformation) { Credentials = credentials };
            var result = await client.Repository.GetAllForCurrent();

            return result;
        }

        [HttpGet, Route("ListIssue")]
        public async Task<IReadOnlyList<Issue>> ListIssueAsync(string RepoName)
        {
            var setting = GetSetting();
            var productInformation = new ProductHeaderValue(setting["Identity"]);
            var credentials = new Credentials(setting["AccessToken"]);
            var client = new GitHubClient(productInformation) { Credentials = credentials };

            var result = await client.Issue.GetAllForRepository(setting["Identity"], RepoName);

            return result;

        }

        private IConfigurationSection GetSetting()
        {
            GitHubSetting setting = new GitHubSetting();
            IConfiguration configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", true, true)
           .Build();
            var conn = configuration.GetSection(nameof(GitHubSetting));

            return conn;
        }
    }
}
