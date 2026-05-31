namespace MyCMS.Data.Interfaces
{
    public interface IDbInitializer
    {
        Task SeedAsync();
    }
}
