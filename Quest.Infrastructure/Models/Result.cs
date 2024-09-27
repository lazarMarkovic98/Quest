namespace Quest.Infrastructure.Models;
public class Result
{
    public int Id { get; set; }
    public int NumberOfComponents { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Result(int numberOfComponents, string fileName, string status = "Ok")
    {
        NumberOfComponents = numberOfComponents;
        FileName = fileName;
        Status = status;
    }
}
