using LocaRider.Application.DTO.Driver;
using Swashbuckle.AspNetCore.Filters;

namespace LocaRider.API.Utils.Drivers
{
    public class DriversExample : IExamplesProvider<DriverDTO>
    {

        public DriverDTO GetExamples()
        {
            return new DriverDTO
            {
                identificador = "entregador123",
                nome = "João da Silva",
                cnpj = "12345678901234",
                data_nascimento = new DateTime(2000, 02, 23),
                numero_cnh = "12345678900",
                tipo_cnh = "A",
                imagem_cnh = "base64string"
            };
        }
    }
    public class DriversCnhImageExample : IExamplesProvider<DriverBase64ImageDTO>
    {

        public DriverBase64ImageDTO GetExamples()
        {
            return new DriverBase64ImageDTO
            {
                imagem_cnh = "base64string"
            };
        }
    }
}
