using AutoMapper;
using LocaRider.Application.DTO.Driver;
using LocaRider.Application.Interfaces.Drivers;
using LocaRider.Application.Interfaces.Storages;
using LocaRider.Domain.Entities.Driver;
using LocaRider.Domain.Interfaces.Drivers;
using Microsoft.Extensions.Logging;

namespace LocaRider.Application.Services.Drivers
{
    public class DriversService : IDriversService
    {
        private readonly IDriversRepository _driversRepository;
        private readonly IStoragesService _storagesService;
        private readonly IMapper _mapper;
        private readonly ILogger<DriversService> _logger;

        public DriversService(
            IDriversRepository driversRepository,
            IStoragesService storagesService,
            IMapper mapper,
            ILogger<DriversService> logger)
        {
            _driversRepository = driversRepository;
            _storagesService = storagesService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> UpdateDriverCnhImageAsync(string driverId, DriverBase64ImageDTO driverBase64ImageDTO)
        {
            _logger.LogInformation("Iniciando atualização da imagem CNH para motorista {DriverId}", driverId);

            try
            {
                var driver = await _driversRepository.GetByIdAsync(driverId);
                if (driver is null)
                {
                    _logger.LogWarning("Motorista inexistente: {DriverId}", driverId);
                    throw new KeyNotFoundException("Motorista não encontrado.");
                }

                _logger.LogInformation("Motorista encontrado: {DriverId}", driverId);

                var base64 = driverBase64ImageDTO.imagem_cnh?.Replace("\r", "").Replace("\n", "").Trim();
                if (string.IsNullOrWhiteSpace(base64))
                {
                    _logger.LogWarning("Base64 da CNH está vazio ou nulo para motorista {DriverId}", driverId);
                    throw new InvalidOperationException("Imagem CNH inválida.");
                }

                var commaIndex = base64.IndexOf(',');
                if (commaIndex >= 0)
                    base64 = base64[(commaIndex + 1)..];

                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(base64);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Falha ao decodificar Base64 para motorista {DriverId}", driverId);
                    throw new InvalidOperationException("A string fornecida não é uma Base64 válida.");
                }

                string extension = DetermineImageExtension(fileBytes, driverId);
                var fileName = $"{driver.DriverId}{extension}";
                _logger.LogInformation("Nome de arquivo gerado: {FileName} para motorista {DriverId}", fileName, driverId);

                var relativePath = await _storagesService.SaveAsync(fileName, fileBytes);
                _logger.LogInformation("Imagem CNH salva no storage: {Path} para motorista {DriverId}", relativePath, driverId);

                driver.SetCnhImage(relativePath);
                await _driversRepository.UpdateAsync(driver);

                _logger.LogInformation("Imagem CNH atualizada no banco para motorista {DriverId}", driverId);
                return relativePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar imagem CNH para motorista {DriverId}", driverId);
                throw;
            }
        }

        public async Task<DriverDTO?> CreateDriverAsync(DriverDTO driverDTO)
        {
            _logger.LogInformation("Iniciando criação de motorista com CNPJ {Cnpj}", driverDTO.cnpj);

            try
            {
                if (await _driversRepository.GetByIdAsync(driverDTO.identificador) is not null)
                {
                    _logger.LogWarning("Identificador já existe: {Identificador}", driverDTO.identificador);
                    throw new InvalidOperationException($"Já existe motorista com o identificador {driverDTO.identificador}");
                }

                if (await _driversRepository.ExistsByCNPJAsync(driverDTO.cnpj))
                {
                    _logger.LogWarning("CNPJ já existe: {Cnpj}", driverDTO.cnpj);
                    throw new InvalidOperationException($"Já existe motorista com CNPJ {driverDTO.cnpj}");
                }

                if (await _driversRepository.ExistsByCNHNumberAsync(driverDTO.numero_cnh))
                {
                    _logger.LogWarning("CNH já existe: {Cnh}", driverDTO.numero_cnh);
                    throw new InvalidOperationException($"Já existe motorista com CNH {driverDTO.numero_cnh}");
                }

                if (driverDTO.imagem_cnh is null)
                {
                    _logger.LogWarning("Imagem CNH não fornecida para motorista {Identificador}", driverDTO.identificador);
                    throw new InvalidOperationException("Imagem CNH é obrigatória para cadastro.");
                }

                _logger.LogInformation("Processando imagem CNH para motorista {Identificador}", driverDTO.identificador);
                var relativePath = await ProcessAndSaveImageAsync(driverDTO.identificador, new DriverBase64ImageDTO { imagem_cnh = driverDTO.imagem_cnh });

                var driverEntity = _mapper.Map<Driver>(driverDTO);
                driverEntity.SetCnhImage(relativePath);

                var createdDriver = await _driversRepository.AddAsync(driverEntity);
                _logger.LogInformation("Motorista criado com sucesso: {DriverId}", createdDriver.DriverId);

                return _mapper.Map<DriverDTO>(createdDriver);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar motorista com CNPJ {Cnpj}", driverDTO.cnpj);
                throw;
            }
        }

        private async Task<string> ProcessAndSaveImageAsync(string? driverId, DriverBase64ImageDTO imageDTO)
        {
            try
            {
                var base64 = imageDTO.imagem_cnh?.Replace("\r", "").Replace("\n", "").Trim();
                if (string.IsNullOrWhiteSpace(base64))
                {
                    _logger.LogWarning("Base64 da CNH inválido para motorista {DriverId}", driverId);
                    throw new InvalidOperationException("Imagem CNH inválida.");
                }

                var commaIndex = base64.IndexOf(',');
                if (commaIndex >= 0)
                    base64 = base64.Substring(commaIndex + 1);

                var fileBytes = Convert.FromBase64String(base64);
                string extension = DetermineImageExtension(fileBytes, driverId);

                var fileName = $"{driverId ?? ""}{extension}";
                var relativePath = await _storagesService.SaveAsync(fileName, fileBytes);

                _logger.LogInformation("Imagem CNH salva no storage em {Path} para motorista {DriverId}", relativePath, driverId);
                return relativePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar imagem CNH para motorista {DriverId}", driverId);
                throw;
            }
        }

        private string DetermineImageExtension(byte[] fileBytes, string? driverId)
        {
            if (fileBytes.Length >= 8 && fileBytes.Take(8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
                return ".png";
            if (fileBytes.Length >= 2 && fileBytes.Take(2).SequenceEqual(new byte[] { 0x42, 0x4D }))
                return ".bmp";

            _logger.LogWarning("Formato inválido de imagem para motorista {DriverId}", driverId);
            throw new InvalidOperationException("Formato de imagem inválido. Apenas PNG ou BMP são aceitos.");
        }
    }
}
