using Quest.Infrastructure.Helper;
using Quest.Infrastructure.Models;

namespace Quest.Engine.Interfaces;
public interface IEngine
{
    Task<List<Result>> GenerateReports(CheckFilesInputDto request);
}