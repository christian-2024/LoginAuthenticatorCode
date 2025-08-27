using AutoMapper;
using FluentValidation;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace LoginAuthenticatorCode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public NotificationController(INotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpPost("create")]
    [Authorize]
    [SwaggerOperation(Summary = "Cria uma notificação")]
    [ProducesResponseType(typeof(NotificationResponseListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateNotification([FromBody] NotificationRequestInsertDto requestDto)
    {
        try
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));
            var notification = _mapper.Map<Notification>(requestDto);
            var result = await _notificationService.AddAsync(notification);

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