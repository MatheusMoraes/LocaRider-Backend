using AutoMapper;
using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Application.DTO.Rental;
using LocaRider.Application.Interfaces.Rentals;
using LocaRider.Domain.Entities.Motorcycles;
using LocaRider.Domain.Entities.Rental;
using LocaRider.Domain.Interfaces.Drivers;
using LocaRider.Domain.Interfaces.Motorcycles;
using LocaRider.Domain.Interfaces.RentalPlans;
using LocaRider.Domain.Interfaces.Rentals;
using Microsoft.Extensions.Logging;

namespace LocaRider.Application.Services.Rentals
{
    public class RentalsService : IRentalsService
    {
        private readonly IRentalsRepository _rentalsRepository;
        private readonly IRentalPlansRepository _rentalPlansRepository;
        private readonly IMotorcyclesRepository _motorcyclesRepository;
        private readonly IDriversRepository _driversRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RentalsService> _logger;

        public RentalsService(
            IRentalsRepository rentalsRepository,
            IRentalPlansRepository rentalPlansRepository,
            IMotorcyclesRepository motorcyclesRepository,
            IDriversRepository driversRepository,
            IMapper mapper,
            ILogger<RentalsService> logger)
        {
            _rentalsRepository = rentalsRepository;
            _motorcyclesRepository = motorcyclesRepository;
            _driversRepository = driversRepository;
            _rentalPlansRepository = rentalPlansRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RentalDTO> CreateRentalAsync(RentalDTO rentalDTO)
        {
            _logger.LogInformation("Iniciando criação de locação. Entregador: {DriverId}, Moto: {MotorcycleId}, Plano: {PlanDays}",
                rentalDTO.entregador_id, rentalDTO.moto_id, rentalDTO.plano);

            try
            {
                var driver = await _driversRepository.GetByIdAsync(rentalDTO.entregador_id);
                if (driver is null)
                {
                    _logger.LogWarning("Driver não encontrado. ID: {DriverId}", rentalDTO.entregador_id);
                    throw new KeyNotFoundException("Driver not found.");
                }

                if (driver.CnhType.ToUpper() != "A")
                {
                    _logger.LogWarning("Driver {DriverId} não possui carteira A, não pode alugar moto.", rentalDTO.entregador_id);
                    throw new InvalidOperationException("Somente motoristas com a carteira A podem alugar motos.");
                }

                var motorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(rentalDTO.moto_id);
                if (motorcycle is null)
                {
                    _logger.LogWarning("Moto não encontrada. ID: {MotorcycleId}", rentalDTO.moto_id);
                    throw new KeyNotFoundException("Moto não encontrada para a chave identificada");
                }

                var activeRental = await _rentalsRepository.GetActiveRentalByMotorcycleAsync(rentalDTO.moto_id);
                if (activeRental != null)
                {
                    _logger.LogWarning("Moto {MotorcycleId} já possui locação ativa.", rentalDTO.moto_id);
                    throw new InvalidOperationException("This motorcycle already has an active rental.");
                }

                var rentalPlan = await _rentalPlansRepository.GetRentalPlanByPeriodInDaysAsync(rentalDTO.plano);
                if (rentalPlan is null)
                {
                    _logger.LogWarning("Plano de locação não encontrado para {PlanDays} dias.", rentalDTO.plano);
                    throw new KeyNotFoundException("Plano de locação não encontrado para o período informado.");
                }

                var entity = _mapper.Map<Rental>(rentalDTO);
                entity.TotalPrice = 0;
                entity.DailyPrice = rentalPlan.DailyPrice;
                entity.StartDate = rentalDTO.data_inicio.AddDays(1);
                entity.EstimatedCompletionDate = rentalDTO.data_inicio.AddDays(rentalDTO.plano);

                await _rentalsRepository.AddAsync(entity);

                _logger.LogInformation("Locação criada com sucesso. Entregador: {DriverId}, Moto: {MotorcycleId}, ID da Locação: {RentalId}",
                    rentalDTO.entregador_id, rentalDTO.moto_id, entity.RentalId);

                return rentalDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar locação. Entregador: {DriverId}, Moto: {MotorcycleId}", rentalDTO.entregador_id, rentalDTO.moto_id);
                throw;
            }
        }

        public async Task<RentalDTO> GetRentalByIdAsync(string rentalId)
        {
            _logger.LogInformation("Buscando locação pelo ID: {RentalId}", rentalId);

            try
            {
                var rental = await _rentalsRepository.GetByIdAsync(rentalId);
                if (rental == null)
                {
                    _logger.LogWarning("Locação não encontrada. ID: {RentalId}", rentalId);
                    throw new KeyNotFoundException("Locacao não encontrada.");
                }

                _logger.LogInformation("Locação encontrada. ID: {RentalId}", rentalId);
                return _mapper.Map<RentalDTO>(rental);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar locação pelo ID: {RentalId}", rentalId);
                throw;
            }
        }

        public async Task<bool> SetReturnDateAsync(string rentalId, RentalDevolutionDTO rentalDevolutionDTO)
        {
            _logger.LogInformation("Atualizando data de devolução para locação {RentalId} com data {ReturnDate}", rentalId, rentalDevolutionDTO.data_devolucao);

            try
            {
                var rental = await _rentalsRepository.GetByIdAsync(rentalId);

                if (rental == null)
                {
                    _logger.LogWarning("Locação não encontrada para atualização. ID: {RentalId}", rentalId);
                    return false;
                }

                if (rental.DevolutionDate != null)
                {
                    _logger.LogWarning("Locação {RentalId} já possui data de devolução definida.", rentalId);
                    return false;
                }

                rental.DevolutionDate = rentalDevolutionDTO.data_devolucao;
                rental.TotalPrice = CalculateTotalPrice(rental, rentalDevolutionDTO.data_devolucao);

                _logger.LogInformation("Total calculado para locação {RentalId}: {TotalPrice}", rentalId, rental.TotalPrice);

                await _rentalsRepository.UpdateAsync(rental);

                _logger.LogInformation("Data de devolução atualizada com sucesso para locação {RentalId}", rentalId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar devolução da locação {RentalId}", rentalId);
                throw;
            }
        }

        private decimal CalculateTotalPrice(Rental rental, DateTime returnDate)
        {
            _logger.LogInformation("Calculando valor total para locação {RentalId}. Data de devolução: {ReturnDate}", rental.RentalId, returnDate);

            var totalDays = (returnDate.Date - rental.StartDate.Date).Days + 1;
            var dailyRate = rental.DailyPrice;
            var planDays = rental.Plan;
            var expectedEndDate = rental.EstimatedCompletionDate.Date;

            decimal total = 0;

            if (returnDate < expectedEndDate)
            {
                total = totalDays * dailyRate;
                var remainingDays = (expectedEndDate - returnDate).Days;

                if (planDays == 7)
                    total += remainingDays * dailyRate * 0.2m; // multa 20%
                else if (planDays == 15)
                    total += remainingDays * dailyRate * 0.4m; // multa 40%
            }
            else if (returnDate > expectedEndDate)
            {
                var extraDays = (returnDate - expectedEndDate).Days;
                total = planDays * dailyRate + (extraDays * 50m);
            }
            else
            {
                total = planDays * dailyRate;
            }

            _logger.LogInformation("Valor total calculado para locação {RentalId}: {Total}", rental.RentalId, total);

            return total;
        }
    }
}
