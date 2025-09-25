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

namespace LocaRider.Application.Services.Rentals
{
    public class RentalsService : IRentalsService
    {   
        private readonly IRentalsRepository _rentalsRepository;
        private readonly IRentalPlansRepository _rentalPlansRepository;
        private readonly IMotorcyclesRepository _motorcyclesRepository;
        private readonly IDriversRepository _driversRepository;
        private readonly IMapper _mapper;
        public RentalsService(
            IRentalsRepository rentalsRepository, 
            IRentalPlansRepository rentalPlansRepository,
            IMotorcyclesRepository motorcyclesRepository,
            IDriversRepository driversRepository,
            IMapper mapper)
        {
            _rentalsRepository = rentalsRepository;
            _motorcyclesRepository = motorcyclesRepository;
            _driversRepository = driversRepository;
            _rentalPlansRepository = rentalPlansRepository; 
            _mapper = mapper;
        }
        public async Task<RentalDTO> CreateRentalAsync(RentalDTO rentalDTO)
        {
            var driver = await _driversRepository.GetByIdAsync(rentalDTO.entregador_id);
            if (driver is null)
                throw new KeyNotFoundException("Driver not found.");

            if (driver.CnhType.ToUpper() != "A")
                throw new InvalidOperationException("Somente motoristas com a carteira A podem alugar motos.");

            var motorcycle = await _motorcyclesRepository.GetMotorcycleByIdAsync(rentalDTO.moto_id);
            if (motorcycle is null)
                throw new KeyNotFoundException("Moto não encontrada para a chave identificada");

            var activeRental = await _rentalsRepository.GetActiveRentalByMotorcycleAsync(rentalDTO.moto_id);
            if (activeRental != null)
                throw new InvalidOperationException("This motorcycle already has an active rental.");

            var rentalPlan = await _rentalPlansRepository.GetRentalPlanByPeriodInDaysAsync(rentalDTO.plano);
            if (rentalPlan is null)
                throw new KeyNotFoundException("Plano de locação não encontrado para o período informado.");
            var entity = _mapper.Map<Rental>(rentalDTO);

            entity.TotalPrice = 0;
            entity.DailyPrice = rentalPlan.DailyPrice;
            entity.StartDate = rentalDTO.data_inicio.AddDays(1);
            entity.EstimatedCompletionDate = rentalDTO.data_inicio.AddDays(rentalDTO.plano);

            await _rentalsRepository.AddAsync(entity);
            return rentalDTO;
        }

        public async Task<RentalDTO> GetRentalByIdAsync(string rentalId)
        {
            return await _rentalsRepository.GetByIdAsync(rentalId) is Rental rental
                ? _mapper.Map<RentalDTO>(rental)
                : throw new KeyNotFoundException("Locacao não encontrada.");
        }

        public async Task<bool> SetReturnDateAsync(string rentalId, RentalDevolutionDTO rentalDevolutionDTO)
        {
            var rental = await _rentalsRepository.GetByIdAsync(rentalId);

            if (rental == null)
                return false; // locação não encontrada

            if (rental.DevolutionDate != null)
                return false; // já foi devolvida

            // Setando data de devolução
            rental.DevolutionDate = rentalDevolutionDTO.data_devolucao;

            // Calcula valor total da locação
            rental.TotalPrice = CalculateTotalPrice(rental, rentalDevolutionDTO.data_devolucao);

            await _rentalsRepository.UpdateAsync(rental);

            return true; // atualização realizada com sucesso
        }

        private decimal CalculateTotalPrice(Rental rental, DateTime returnDate)
        {
            var totalDays = (returnDate.Date - rental.StartDate.Date).Days + 1;
            var dailyRate = rental.DailyPrice;
            var planDays = rental.Plan;
            var expectedEndDate = rental.EstimatedCompletionDate.Date;

            decimal total = 0;

            if (returnDate < expectedEndDate)
            {
                // devolução antecipada → cobra diárias + multa sobre dias restantes
                total = totalDays * dailyRate;
                var remainingDays = (expectedEndDate - returnDate).Days;

                if (planDays == 7)
                    total += remainingDays * dailyRate * 0.2m; // multa 20%
                else if (planDays == 15)
                    total += remainingDays * dailyRate * 0.4m; // multa 40%
            }
            else if (returnDate > expectedEndDate)
            {
                // devolução atrasada → cobra diárias + adicional
                var extraDays = (returnDate - expectedEndDate).Days;
                total = planDays * dailyRate + (extraDays * 50m);
            }
            else
            {
                // devolução no prazo → cobra só as diárias do plano
                total = planDays * dailyRate;
            }

            return total;
        }
    }
}
