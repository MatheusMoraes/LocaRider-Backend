using LocaRider.Application.Interfaces.Storages;

namespace LocaRider.Application.Services.LocalStorages
{

    public class LocalStorageService : IStoragesService
    {
        private readonly string _basePath;

        public LocalStorageService()
        {
            // Define a pasta "photos" na raiz do projeto
            _basePath = Path.Combine(AppContext.BaseDirectory, "photos");

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveAsync(string fileName, byte[] fileBytes)
        {
            var safeFileName = Path.GetFileName(fileName);

            var filePath = Path.Combine(_basePath, safeFileName);

            await File.WriteAllBytesAsync(filePath, fileBytes);

            return filePath;
        }
    }
}
