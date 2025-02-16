using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DataEntities
{
    public class RepositoryPortfolio
    {
        public string Name { get; set; }
        public string HtmlUrl { get; set; }
        public int Stars { get; set; }
        public DateTime? LastCommitDate { get; set; }
        public List<string> Languages { get; set; }
        public int PullRequestCount { get; set; }
    }
}

