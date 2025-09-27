using LocaRider.Application.Interfaces.Storages;
using Microsoft.Extensions.Logging;

namespace LocaRider.Application.Services.LocalStorages
{
    public class LocalStorageService : IStoragesService
    {
        private readonly string _basePath;
        private readonly ILogger<LocalStorageService> _logger;

        public LocalStorageService(ILogger<LocalStorageService> logger)
        {
            _logger = logger;

            // Define a pasta "photos" na raiz do projeto
            _basePath = Path.Combine(AppContext.BaseDirectory, "photos");
            _logger.LogInformation("Caminho base para armazenamento local definido: {BasePath}", _basePath);

            try
            {
                if (!Directory.Exists(_basePath))
                {
                    Directory.CreateDirectory(_basePath);
                    _logger.LogInformation("Diretório criado com sucesso: {BasePath}", _basePath);
                }
                else
                {
                    _logger.LogInformation("Diretório já existe: {BasePath}", _basePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar diretório de armazenamento local: {BasePath}", _basePath);
                throw;
            }
        }

        public async Task<string> SaveAsync(string fileName, byte[] fileBytes)
        {
            _logger.LogInformation("Iniciando salvamento do arquivo {FileName}", fileName);

            try
            {
                var safeFileName = Path.GetFileName(fileName);
                var filePath = Path.Combine(_basePath, safeFileName);

                await File.WriteAllBytesAsync(filePath, fileBytes);

                _logger.LogInformation("Arquivo salvo com sucesso: {FilePath}", filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar arquivo {FileName} no diretório {BasePath}", fileName, _basePath);
                throw;
            }
        }
    }
}
