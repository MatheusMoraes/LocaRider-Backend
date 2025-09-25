using LocaRider.Application.DTO.Users;
using LocaRider.Application.Interfaces.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocaRider.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;

        // Injeção de dependência do Application Service (Use Case)
        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        /// <remarks>
        /// Possíveis respostas:
        /// - 200 → Lista de usuários (pode estar vazia).
        /// - 500 → Erro interno do servidor.
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retorna um usuário específico pelo seu ID.
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <remarks>
        /// Possíveis respostas:
        /// - 200 → Usuário encontrado.
        /// - 404 → Usuário não encontrado.
        /// - 500 → Erro interno.
        /// </remarks>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound("Usuário não encontrado");
            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="userCreateDto">Dados para criação</param>
        /// <remarks>
        /// Possíveis respostas:
        /// - 201 → Usuário criado com sucesso (retorna o objeto criado).
        /// - 400 → Dados inválidos.
        /// - 409 → Email já em uso.
        /// - 500 → Erro interno.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO userCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.CreateUserAsync(userCreateDto);

            if (createdUser == null)
                return Conflict("Email já está em uso.");

            return CreatedAtAction(nameof(GetUserById), new { userId = createdUser.UserId }, createdUser);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <param name="userUpdateDto">Novos dados</param>
        /// <remarks>
        /// Possíveis respostas:
        /// - 204 → Atualizado com sucesso (sem retorno no corpo).
        /// - 400 → Dados inválidos.
        /// - 404 → Usuário não encontrado.
        /// - 500 → Erro interno.
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserUpdateDTO userUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _userService.UpdateUserAsync(userId, userUpdateDto);
            if (!updated) return NotFound("Usuário não encontrado.");

            return NoContent();
        }

        /// <summary>
        /// Remove um usuário específico pelo ID.
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <remarks>
        /// Possíveis respostas:
        /// - 204 → Usuário removido com sucesso.
        /// - 404 → Usuário não encontrado.
        /// - 500 → Erro interno.
        /// </remarks>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var deleted = await _userService.DeleteUserAsync(userId);
            if (!deleted) return NotFound("Usuário não encontrado");

            return NoContent();
        }
    }
}