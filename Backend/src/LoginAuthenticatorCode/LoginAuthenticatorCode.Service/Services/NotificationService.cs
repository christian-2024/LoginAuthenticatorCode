using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto.List;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Interfaces.Repository;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using LoginAuthenticatorCode.Shared.Jwt;
using System.Linq.Expressions;

namespace LoginAuthenticatorCode.Service.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly AppSetting _appSetting;
    private readonly IValidator<Notification> _validator;

    public NotificationService(INotificationRepository notificationRepository,
                               IAuthenticationService authenticationService,
                               IUserService userService,
                               IMapper mapper,
                               AppSetting appSetting,
                               IValidator<Notification> validator)
    {
        _notificationRepository = notificationRepository;
        _authenticationService = authenticationService;
        _userService = userService;
        _mapper = mapper;
        _appSetting = appSetting;
        _validator = validator;
    }

    public async Task<Notification> AddAsync(Notification entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        var listErrors = await this.Validate(entity);
        if (listErrors.Any())
            throw new ValidationException(listErrors);

        await _notificationRepository.AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IList<Notification> entities)
    {
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            var listErrorsPermission = await this.ValidateUserPermission(entity);
            if (listErrorsPermission.Any())
                throw new ValidationException(listErrorsPermission);

            var listErrors = await this.Validate(entity);
            if (listErrors.Any())
                throw new ValidationException(listErrors);
        }
        await _notificationRepository.AddRangeAsync(entities);
    }

    public async Task<Notification> DeleteAsync(Notification entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        await _notificationRepository.DeleteAsync(entity);
        return entity;
    }

    public async Task<Notification> DeleteAsyncById(long Id)
    {
        var entity = await _notificationRepository.GetByIdAsync(Id);

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        await _notificationRepository.DeleteAsyncById(Id);
        return entity;
    }

    public async Task<IEnumerable<Notification>> GetAllAsync(Expression<Func<Notification, bool>> predicate)
    {
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));
        return await _notificationRepository.GetAllAsync(predicate);
    }

    public async Task<IEnumerable<Notification>> GetAllAsync(Notification entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        return await _notificationRepository.GetAllAsync(entity);
    }

    public async Task<PagedListDto<NotificationResponseListDto>> GetAllNotificationsByFilterAsync(NotificationRequestListDto requestDto)
    {
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        var result = await _notificationRepository.GetAllNotificationsByFilterAsync(requestDto);

        IEnumerable<NotificationResponseListDto> NpsClientDto = _mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationResponseListDto>>(result);

        return new PagedListDto<NotificationResponseListDto>(NpsClientDto, requestDto.PageNumber, requestDto.PageSize, result.TotalCount);
    }

    public async Task<Notification> GetAsync(Expression<Func<Notification, bool>> predicate)
    {
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        var entity = await _notificationRepository.GetAsync(predicate);

        var user = await _userService.GetByIdAsync(entity.UserId);
        var authRole = _authenticationService.IsAdmin();
        //var authClient = _authenticationService.UserClient();

        //if (authRole == false && user.u != authClient)
        //    throw new ValidationException("Registro não encontrado");

        return entity;
    }

    public async Task<Notification> GetByIdAsync(long Id)
    {
        var entity = await _notificationRepository.GetByIdAsync(Id);

        var user = await _userService.GetByIdAsync(entity.UserId);
        var authRole = _authenticationService.IsAdmin();
        //var authClient = _authenticationService.UserClient();

        //if (authRole == false && user.u != authClient)
        //    throw new ValidationException("Registro não encontrado");

        return entity;
    }

    public async Task<Notification> UpdateAsync(Notification entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        var listErrors = await this.Validate(entity);
        if (listErrors.Any())
            throw new ValidationException(listErrors);

        return await _notificationRepository.UpdateAsync(entity);
    }

    public async Task UpdateRangeAsync(IEnumerable<Notification> entities)
    {
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            var listErrorsPermission = await this.ValidateUserPermission(entity);
            if (listErrorsPermission.Any())
                throw new ValidationException(listErrorsPermission);

            var listErrors = await this.Validate(entity);
            if (listErrors.Any())
                throw new ValidationException(listErrors);
        }
        await _notificationRepository.UpdateRangeAsync(entities);
    }

    public async Task<List<ValidationFailure>> Validate(Notification entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));
        var listErrors = new List<ValidationFailure>();
        var validation = _validator.Validate(entity);

        if (!validation.IsValid)
            listErrors.AddRange(validation.Errors);

        return listErrors;
    }

    public async Task<List<ValidationFailure>> ValidateUserPermission(Notification entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));
        var listErrors = new List<ValidationFailure>();

        var authRole = _authenticationService.IsAdmin();
        var authClient = _authenticationService.UserClient();

        //Impede que usuários comuns criem/deletem notificações de outros clientes
        if (authRole == false)
            listErrors.Add(new ValidationFailure("Notificação", $"Registro não encontrado."));

        return listErrors;
    }
}