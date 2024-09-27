using Microsoft.EntityFrameworkCore;
using Quest.Infrastructure.Context;
using Quest.Infrastructure.Helper;
using Quest.Infrastructure.Models;

namespace Quest.Engine.Tests.Tests;
public class EngineTests
{
    private readonly Implementations.Engine _engine;
    private readonly QuestDbContext _dbContext;

    public EngineTests()
    {
        var options = new DbContextOptionsBuilder<QuestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestQuestDb")
            .Options;
        _dbContext = new QuestDbContext(options);

        _engine = new Implementations.Engine(_dbContext);
    }

    [Fact]
    public async Task GenerateReports_ShouldProcessFilesAndReturnResults()
    {
        // Arrange
        var inputDto = new CheckFilesInputDto
        {
            Folders = new List<string> { "TestFolder" },
            MaximumConcurentProcessingJobs = 4
        };

        Directory.CreateDirectory("TestFolder");
        string testFilePath = Path.Combine("TestFolder", "test1.json");
        await File.WriteAllTextAsync(testFilePath, "[{\"Id\": 1, \"Name\": \"Component1\"}, {\"Id\": 2, \"Name\": \"Component2\"}]");

        // Act
        var result = await _engine.GenerateReports(inputDto);

        // Assert
        Assert.NotNull(result);

        Directory.Delete("TestFolder", true);
    }

    [Fact]
    public void AddOrUpdate_ShouldAddOrUpdateResults()
    {
        // Arrange
        var results = new List<Result>
            {
                new Result(2, "File1.json") { Status = "Ok" },
                new Result(3, "File2.json") { Status = "Ok" }
            };

        // Act
        _ = _engine.GenerateReports(new CheckFilesInputDto
        {
            Folders = new List<string> { "TestFolder" },
            MaximumConcurentProcessingJobs = 2
        });

        _engine.AddOrUpdate(results);
        _dbContext.SaveChanges();

        // Assert
        var allResults = _dbContext.Results.ToList();
        Assert.Equal(2, allResults.Count);
        Assert.Equal("Ok", allResults.First(x => x.FileName == "File1.json").Status);
        Assert.Equal(3, allResults.First(x => x.FileName == "File2.json").NumberOfComponents);
    }
}
