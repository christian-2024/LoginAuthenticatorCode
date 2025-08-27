using AutoMapper;
using FluentValidation;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace LoginAuthenticatorCode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;
    private readonly IMapper _mapper;

    public PermissionController(IPermissionService permissionService, IMapper mapper)
    {
        _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("get")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Obtém uma permissão de acordo com o id.")]
    [SwaggerResponse(200, "A permissão foi retornada com sucesso.")]
    [SwaggerResponse(404, "Nenhum item encontrado.")]
    [ProducesResponseType(typeof(PermissionResponseListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PermissionResponseListDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPermission([FromBody] long id)
    {
        try
        {
            if (id > 0)
            {
                var permission = await _permissionService.GetByIdAsync(id);

                var permissionDto = _mapper.Map<PermissionRequestUpdateDto>(permission);
                return Ok(permissionDto);
            }
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
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Obtém todos as permissões de acordo com a paginação informada.")]
    [SwaggerResponse(200, "As permissões foram retornados com sucesso.")]
    [SwaggerResponse(404, "Nenhum item encontrado.")]
    [ProducesResponseType(typeof(PermissionResponseListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PermissionResponseListDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllPermissions([FromBody] PermissionRequestListDto requestDto)
    {
        try
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

            var result = await _permissionService.GetAllPermissionsByFilterAsync(requestDto);
            return Ok(new PaginationHeaderDto<PermissionResponseListDto>(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages, result));
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

    [HttpGet("admin/get-all")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Obtém todos as permissões de acordo com o tipo de acesso.")]
    [SwaggerResponse(200, "As permissões foram retornados com sucesso.")]
    [SwaggerResponse(404, "Nenhum item encontrado.")]
    [ProducesResponseType(typeof(PermissionResponseListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PermissionResponseListDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllPermissionsAdmin([FromQuery] PermissionRequestListDto requestDto)
    {
        try
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

            var result = await _permissionService.GetAllPermissionsAdminAsync(requestDto);
            return Ok(new PaginationHeaderDto<PermissionResponseListDto>(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages, result));
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