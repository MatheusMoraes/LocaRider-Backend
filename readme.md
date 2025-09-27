# LocaRider API

## Descrição
LocaRider é um sistema de gestão de motos, entregadores e locações, utilizando Clean Architecture e boas práticas em .NET 10, com persistência em PostgreSQL e MongoDB. A API permite:
- Cadastro e gerenciamento de entregadores
- Cadastro e gerenciamento de motos
- Criação e gerenciamento de locações
- Upload de imagens de CNH
- Consulta e atualização de dados

## Tecnologias Utilizadas
- .NET 10
- EF Core
- PostgreSQL
- MongoDB
- Docker e Docker Compose
- AutoMapper
- Swashbuckle (Swagger)
- xUnit para testes unitários
- ILogger para logs

## Estrutura do Projeto
- **API**: LocaRider.API
- **Application**: lógica de serviços, DTOs e interfaces
- **Domain**: entidades e regras de negócio
- **Infrastructure**: persistência, repositórios e integrações externas
- **Tests**: testes unitários e de integração

## Como Rodar
1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/LocaRider.git
   cd LocaRider
   ```

3. Inicie os containers (ou só inicialize utilizando o docker compose no visual studio):
   ```bash
   docker-compose up --build
   ```

4. Acesse a API via:
   - HTTP: `http://localhost:8080`
   - HTTPS: `https://localhost:8081`

5. A documentação Swagger estará disponível em:
   - `https://localhost:8081/swagger/

## Endpoints Principais

## Drivers

### 1. Criar Entregador
- **URL:** `/entregadores`
- **Método:** POST
- **Request Body (JSON):
```json
{
  "identificador": "1",
  "nome": "João Silva",
  "cpf": "12345678900",
  "cnpj": "11111111000100",
  "cnhNumero": "CNH123",
  "cnhTipo": "A",
  "birthDate": "1990-01-01"
}
```
- **Responses:**
  - `201 Created` → Criado com sucesso  
  - `409 Conflict` → CNPJ ou CNH já cadastrado  
  - `400 BadRequest` → Dados inválidos

### 2. Enviar Foto da CNH
- **URL:** `/entregadores/{id}/cnh`
- **Método:** POST
- **Request Body (JSON):
```json
{
  "imagem_cnh": "base64string..."
}
```
- **Responses:**
  - `201 Created` → Imagem salva, retorna caminho relativo  
  - `404 NotFound` → Motorista não encontrado  
  - `400 BadRequest` → Dados inválidos

## Motorcycles

### 1. Criar Moto
- **URL:** `/motos`
- **Método:** POST
- **Request Body (JSON):
```json
{
  "identificador": "1",
  "placa": "ABC1234",
  "modelo": "CB 600",
  "ano": 2010
}
```
- **Responses:**
  - `201 Created` → Criada com sucesso  
  - `409 Conflict` → Placa ou identificador já cadastrado  
  - `400 BadRequest` → Dados inválidos

### 2. Consultar todas as motos
- **URL:** `/motos?placa=ABC1234` *(opcional)*
- **Método:** GET
- **Response Body (JSON):
```json
[
  {
    "identificador": "1",
    "placa": "ABC1234",
    "modelo": "CB 600",
    "ano": 2010
  }
]
```

### 3. Consultar moto por ID
- **URL:** `/motos/{id}`
- **Método:** GET
- **Response Body (JSON):
```json
{
  "identificador": "1",
  "placa": "ABC1234",
  "modelo": "CB 600",
  "ano": 2010
}
```

### 4. Atualizar placa da moto
- **URL:** `/motos/{id}/placa`
- **Método:** PUT
- **Request Body (JSON):
```json
{
  "placa": "XYZ5678"
}
```
- **Responses:**
  - `200 OK` → Placa modificada com sucesso  
  - `404 NotFound` → Moto não encontrada ou placa já cadastrada  
  - `400 BadRequest` → Dados inválidos

### 5. Deletar moto
- **URL:** `/motos/{id}`
- **Método:** DELETE
- **Responses:**
  - `200 OK` → Moto deletada com sucesso  
  - `404 NotFound` → Moto não encontrada  
  - `400 BadRequest` → Dados inválidos

## Rentals

### 1. Criar locação
- **URL:** `/locacao`
- **Método:** POST
- **Request Body (JSON):
```json
{
  "rentalId": "1",
  "motorcycleId": "1",
  "driverId": "1",
  "startDate": "2025-09-27",
  "estimatedCompletionDate": "2025-10-04",
  "plan": 7,
  "dailyPrice": 30
}
```
- **Responses:**
  - `201 Created` → Locação criada com sucesso  
  - `400 BadRequest` → Dados inválidos

### 2. Consultar locação por ID
- **URL:** `/locacao/{id}`
- **Método:** GET
- **Response Body (JSON):
```json
{
  "rentalId": "1",
  "motorcycleId": "1",
  "driverId": "1",
  "startDate": "2025-09-27",
  "estimatedCompletionDate": "2025-10-04",
  "plan": 7,
  "dailyPrice": 30
}
```

### 3. Informar data de devolução
- **URL:** `/locacao/{id}/devolucao`
- **Método:** PUT
- **Request Body (JSON):
```json
{
  "returnDate": "2025-10-02"
}
```
- **Responses:**
  - `200 OK` → Data de devolução informada com sucesso  
  - `404 NotFound` → Locação não encontrada  
  - `400 BadRequest` → Dados inválidos



## Logs
Os logs do sistema são gravados via `ILogger`. Visualize os logs no output ou dentro dos logs do docker, alternativamente

## Testes
Execute os testes com xUnit:
```bash
dotnet test
```

## Observações
- As portas HTTP/HTTPS estão mapeadas no Docker Compose (`8080:8080` e `8081:8081`) para manter consistência.
- Para desenvolvimento, a variável `ASPNETCORE_ENVIRONMENT` está configurada como `Development`.

## Contribuição
Sinta-se à vontade para abrir issues ou pull requests. Utilize a mesma estrutura de camadas e padrões do projeto.

---
**LocaRider - Gestão de motos, entregadores e locações**