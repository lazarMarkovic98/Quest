namespace Quest.Infrastructure.Helper;
public class CheckFilesInputDto
{
    public List<string> Folders { get; set; } = new List<string>();
    public int MaximumConcurentProcessingJobs { get; set; }
}
