using System.Threading.Tasks;

namespace Dewey.GitHub
{
    public static class GitHubClient
    {
        public static string ClientName { get; set; }
        public static string ClientIssueLabelTitle { get; set; }
        public static string RepositoryOwner { get; set; }
        public static string RepositoryName { get; set; }
        public static string AccessToken { get; set; }

        public static async Task LogIssueAsync(GitHubLog log)
        {
            var client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue(ClientName))
            {
                Credentials = new Octokit.Credentials(AccessToken)
            };

            Octokit.IIssuesClient issuesClient = client.Issue;

            Octokit.NewIssue newIssue = new Octokit.NewIssue(log.Title)
            {
                Body = log.Description
            };

            if (ClientIssueLabelTitle != null && !string.IsNullOrWhiteSpace(ClientIssueLabelTitle)) {
                newIssue.Labels.Add(ClientIssueLabelTitle);
            }

            await issuesClient.Create(RepositoryOwner, RepositoryName, newIssue);
        }
    }
}
