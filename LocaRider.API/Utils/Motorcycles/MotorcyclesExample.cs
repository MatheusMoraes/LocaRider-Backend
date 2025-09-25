using LocaRider.Application.DTO.Motorcycle;
using Swashbuckle.AspNetCore.Filters;

namespace LocaRider.API.Utils.Motorcycles
{
    public class MotorcyclesObjectExample: IExamplesProvider<MotorcycleDTO>
    {
        public MotorcycleDTO GetExamples()
        {
            return new MotorcycleDTO
            {
                identificador = "moto123",
                ano = 2020,
                modelo = "Mottu Sport",
                placa = "CDX-0101"
            };
        }
    }

    public class MotorcyclesPlateExample : IExamplesProvider<MotorcyclePlateDTO>
    {
        public MotorcyclePlateDTO GetExamples()
        {
            return new MotorcyclePlateDTO
            {
               placa = "ABC-1234"
            };
        }
    }
}
