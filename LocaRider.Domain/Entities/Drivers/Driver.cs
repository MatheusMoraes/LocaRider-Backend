using System.ComponentModel.DataAnnotations;

namespace LocaRider.Domain.Entities.Driver
{
    public class Driver
    {
        [Key]
        public string DriverId { get; set; }
        public string Name { get; private set; } = string.Empty;
        public string Cnpj { get; private set; } = string.Empty;
        public DateTime BirthDate { get; private set; }
        public string CnhNumber { get; private set; } = string.Empty;
        public string CnhType { get; private set; } = string.Empty;
        public string CnhImage { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public ICollection<LocaRider.Domain.Entities.Rental.Rental> Rentals { get; set; } = [];
        public Driver() { }
        public Driver(string name, string cnpj, DateTime birthDate, string cnhNumber, string cnhType, string cnhImage)
        {
            SetName(name);
            SetCnpj(cnpj);
            SetBirthDate(birthDate);
            SetCnhNumber(cnhNumber);
            SetCnhType(cnhType);
            SetCnhImage(cnhImage);

            CreatedAt = DateTime.UtcNow; // Garantindo UTC
        }

        // Métodos de negócio para manter invariantes
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome é obrigatório.");
            Name = name.Trim();
        }

        public void SetCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ é obrigatório.");
            Cnpj = cnpj.Trim();
        }

        public void SetBirthDate(DateTime birthDate)
        {
            if (birthDate > DateTime.UtcNow.AddYears(-18))
                throw new ArgumentException("Motorista deve ter pelo menos 18 anos.");
            BirthDate = birthDate.ToUniversalTime(); ;
        }

        public void SetCnhNumber(string cnhNumber)
        {
            if (string.IsNullOrWhiteSpace(cnhNumber))
                throw new ArgumentException("Número da CNH é obrigatório.");
            CnhNumber = cnhNumber.Trim();
        }

        public void SetCnhType(string cnhType)
        {
            if (cnhType != "A" && cnhType != "B" && cnhType != "A+B")
                throw new ArgumentException("Tipo da CNH inválido. Deve ser A, B ou A+B.");
            CnhType = cnhType;
        }

        public void SetCnhImage(string cnhImage)
        {
            if (string.IsNullOrWhiteSpace(cnhImage))
                throw new ArgumentException("Imagem da CNH é obrigatória.");
            CnhImage = cnhImage.Trim();
        }
    }
}
