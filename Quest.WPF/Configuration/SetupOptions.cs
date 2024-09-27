namespace Quest.WPF.Configuration;
public class SetupOptions
{
    public const string ConfigKey = "SETUP_OPTIONS";
    public List<string> Folders { get; set; } = new List<string>();
    public int CheckInterval { get; set; }
    public int MaximumConcurentProcessingJobs { get; set; }
}
