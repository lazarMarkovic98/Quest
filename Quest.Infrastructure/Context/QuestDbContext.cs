using Microsoft.EntityFrameworkCore;
using Quest.Infrastructure.Models;

namespace Quest.Infrastructure.Context;
public class QuestDbContext : DbContext
{
    public DbSet<Result> Results { get; set; }

    public QuestDbContext(DbContextOptions<QuestDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("QuestDb");
}
