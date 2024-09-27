using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quest.Engine.Interfaces;
using Quest.Infrastructure.Context;
using Quest.Infrastructure.Helper;
using Quest.Infrastructure.Models;

namespace Quest.Engine.Implementations;
public class Engine(QuestDbContext dbContext) : IEngine
{
    private readonly QuestDbContext _dbContext = dbContext;
    private async Task ProcessFiles(CheckFilesInputDto request)
    {
        List<string> files = new List<string>();

        foreach (var folder in request.Folders)
        {
            files.AddRange(Directory.EnumerateFiles(folder!, "*.json"));
        }

        var options = new ParallelOptions { MaxDegreeOfParallelism = request.MaximumConcurentProcessingJobs };
        List<Result> results = new List<Result>();

        await Parallel.ForEachAsync(files, options, async (file, cancellationToken) =>
        {
            var result = await CheckFile(file);
            results.Add(result);
        });
        AddOrUpdate(results);
        _dbContext.SaveChanges();
    }
    private async Task<Result> CheckFile(string fullPath)
    {
        try
        {
            string jsonText = await File.ReadAllTextAsync(fullPath);
            var result = JsonConvert.DeserializeObject<List<Component>>(jsonText);
            return new Result(result!.Count, fullPath);
        }
        catch (Exception e)
        {
            return new Result(0,fullPath, e.Message);
        }
    }

    public async Task<List<Result>> GenerateReports(CheckFilesInputDto request)
    {
        await ProcessFiles(request);
        return await _dbContext.Results.ToListAsync();
    }

    public void AddOrUpdate(List<Result> results)
    {
        foreach (var result in results)
        {
            var file = _dbContext.Results.FirstOrDefault(x => x.FileName == result.FileName);
            if (file is null)
            {
                _dbContext.Results.Add(result);
            }
            else
            {
                file.Status = result.Status;
                file.NumberOfComponents = result.NumberOfComponents;
            }

        }
    }
}
