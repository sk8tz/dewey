using System.ComponentModel.DataAnnotations;

namespace Dewey.GitHub
{
    public class GitHubLog
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
