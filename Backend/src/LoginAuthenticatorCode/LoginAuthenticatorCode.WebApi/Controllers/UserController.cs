using AutoMapper;
using FluentValidation;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.Auth;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;

namespace LoginAuthenticatorCode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IPermissionService _permissionService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IAuthenticationService authenticationService, IPermissionService permissionService, IMapper mapper)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Cria um usuário")]
    [ProducesResponseType(typeof(UserResponseFormDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] UserRequestInsertDto requestDto)
    {
        try
        {
            if (requestDto.PermissionId <= 0)
                return BadRequest("Permissão não encontrada.");

            if (requestDto.PhoneNumber is not null)
                requestDto.PhoneNumber = Regex.Replace(requestDto.PhoneNumber, @"[^a-zA-Z0-9]", string.Empty);

            var permission = await _permissionService.GetByIdAsync(requestDto.PermissionId);
            if (permission is null)
                return BadRequest("Permissão não encontrada.");

            var user = _mapper.Map<User>(requestDto);
            var vUser = await _userService.AddAsync(user);

            var response = _mapper.Map<UserResponseFormDto>(vUser);

            return Ok(response);
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }

    [HttpGet("get")]
    [Authorize]
    [SwaggerOperation(Summary = "Obtém um usuário de acordo com o id.")]
    [SwaggerResponse(200, "O usuário foi retornada com sucesso.")]
    [SwaggerResponse(404, "Nenhum item encontrado.")]
    [ProducesResponseType(typeof(UserResponseListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserResponseListDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUser([FromQuery] long id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Usuário não localizado.");

            var user = await _userService.GetByIdAsync(id);
            var permission = await _permissionService.GetByIdAsync(user.PermissionId);

            var result = _mapper.Map<UserResponseListDto>(user);
            result.Permission = _mapper.Map<PermissionResponseListDto>(permission);

            return Ok(result);
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }

    [HttpGet("get-all")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Obtém todos os usuários de acordo com a paginação informada e filtros.")]
    [SwaggerResponse(200, "Os usuários foram retornados com sucesso.")]
    [SwaggerResponse(404, "Nenhum item encontrado.")]
    [ProducesResponseType(typeof(UserResponseListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserResponseListDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserRequestListDto requestDto)
    {
        try
        {
            var result = await _userService.GetAllUsersByFilterAsync(requestDto);
            return Ok(new PaginationHeaderDto<UserResponseListDto>(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages, result));
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }

    [HttpPut("update-profile")]
    [Authorize]
    [SwaggerOperation(Summary = "Atualiza o perfil do usuário.")]
    [ProducesResponseType(typeof(UserResponseFormDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUserProfile([FromQuery] long id, [FromBody] UserProfileRequestUpdateDto requestDto)
    {
        try
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

            if (id <= 0)
                return BadRequest("Usuário não localizado.");

            if (requestDto.PhoneNumber is not null)
                requestDto.PhoneNumber = Regex.Replace(requestDto.PhoneNumber, @"[^a-zA-Z0-9]", string.Empty);

            var userAuthenticationId = _authenticationService.GetCurrentUser().Id;

            if (userAuthenticationId != id)
                return BadRequest("Você não tem permissão para atualizar esse usuário.");

            User user = await _userService.GetByIdAsync(id);
            if (user is null)
                return BadRequest("Usuário não localizado.");

            var vUser = _mapper.Map(requestDto, user);

            if (!string.IsNullOrEmpty(requestDto.NewPassword))
            {
                bool verifityHash = _userService.VerifyHashPassword(requestDto.CurrentPassword, user.Password);
                if (!verifityHash)
                    return BadRequest("Senha atual incorreta.");

                var newHash = _userService.GenerateNewHashPassword(requestDto.NewPassword);
                vUser.Password = newHash;
            }
            vUser.UserIdModified = userAuthenticationId;
            var result = await _userService.UpdateAsync(vUser);

            var response = _mapper.Map<UserResponseFormDto>(result);
            return Ok(response);
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }

    [HttpPut("update-user")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Atualiza um usuário.")]
    [ProducesResponseType(typeof(UserResponseFormDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromQuery] long id, [FromBody] UserRequestUpdateDto requestDto)
    {
        try
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

            if (id <= 0)
                return BadRequest("Usuário não localizado.");

            var userAuthenticationId = _authenticationService.GetCurrentUser().Id;
            if (userAuthenticationId == id)
                return BadRequest("Você não tem permissão para atualizar esse usuário.");

            User User = await _userService.GetByIdAsync(id);
            if (User is null)
                return BadRequest("Usuário não localizado.");

            if (!string.IsNullOrEmpty(requestDto.Password))
            {
                var newHash = _userService.GenerateNewHashPassword(requestDto.Password);
                requestDto.Password = newHash;
            }
            else
            {
                requestDto.Password = User.Password;
            }
            var vUser = _mapper.Map(requestDto, User);

            vUser.UserIdModified = userAuthenticationId;
            var result = await _userService.UpdateAsync(vUser);

            var response = _mapper.Map<UserResponseFormDto>(result);
            return Ok(response);
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }

    [HttpPut("update-situation")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Atualiza a situação do usuário.")]
    [ProducesResponseType(typeof(UserResponseFormDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUserSituation([FromQuery] long id, [FromBody] UserRequestUpdateSituationDto requestDto)
    {
        try
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

            if (id <= 0)
                return BadRequest("Usuário não localizado.");

            User User = await _userService.GetByIdAsync(id);
            if (User is null)
                return BadRequest("Usuário não localizado.");

            var vUser = _mapper.Map(requestDto, User);
            vUser.UserIdModified = _authenticationService.GetCurrentUser().Id;

            await _userService.UpdateAsync(vUser);

            return Ok();
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Admin, Manager")]
    [SwaggerOperation(Summary = "Deleta um usuário.")]
    [ProducesResponseType(typeof(UserResponseListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromQuery] long id)
    {
        try
        {
            if (id <= 0)
                return BadRequest("Usuário não localizado.");

            User User = await _userService.GetByIdAsync(id);
            if (User is null)
                return BadRequest("Usuário não localizado.");
            if (User is not null)
            {
                User.UserIdDeleted = 1;
                await _userService.DeleteAsync(User);
            }
            return Ok();
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Faz a autenticação do usuário.")]
    [ProducesResponseType(typeof(AuthenticateResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromQuery] AuthenticateRequestDto requestDto)
    {
        try
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));
            var result = await _userService.LoginAsync(requestDto);

            return Ok(result);
        }
        catch (ValidationException e)
        {
            var vErros = e.Errors.Select(x => x.ErrorMessage).ToList();
            if (vErros is not null && vErros.Any())
                return BadRequest(new
                {
                    Errors = vErros
                });
        }
        catch (InvalidOperationException e)
        {
            // Erro de conflito (usuário com e-mail/cpf já existente)
            return Conflict(new { message = e.Message });
        }
        catch (Exception e)
        {
            // Captura de qualquer outro erro não tratado
            Log.Error(e, "Erro: {Message} - {StackTrace} - {InnerException}", e.Message, e.StackTrace, e.InnerException?.ToString());
            return BadRequest(new { Errors = e.Message });
        }
        return Ok();
    }
}