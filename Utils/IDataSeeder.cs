namespace DrzewaAPI.Utils;

public interface IDataSeeder
{
    Task SeedAsync(CancellationToken ct = default);
}