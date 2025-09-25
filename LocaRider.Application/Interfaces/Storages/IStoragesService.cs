namespace LocaRider.Application.Interfaces.Storages
{
    public interface IStoragesService
    {
        Task<string> SaveAsync(string fileName, byte[] fileBytes);
    }
}
