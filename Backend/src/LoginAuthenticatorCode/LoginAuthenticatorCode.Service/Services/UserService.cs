using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.Auth;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;
using LoginAuthenticatorCode.Domain.Enum;
using LoginAuthenticatorCode.Domain.Interfaces.Repository;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using System.Linq.Expressions;

namespace LoginAuthenticatorCode.Service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User> _validator;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;

    public UserService(IUserRepository userRepository, IValidator<User> validator, IMapper mapper, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    public async Task<User> AddAsync(User entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        var listErrors = await this.Validate(entity);
        if (listErrors.Any())
            throw new ValidationException(listErrors);

        entity.Password = this.GenerateNewHashPassword(entity.Password);

        await _userRepository.AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IList<User> entities)
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

            entity.Password = this.GenerateNewHashPassword(entity.Password);
        }
    }

    public async Task<User> DeleteAsync(User entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        return await _userRepository.DeleteAsync(entity);
    }

    public async Task<User> DeleteAsyncById(long Id)
    {
        var user = await _userRepository.GetByIdAsync(Id);

        var listErrorsPermission = await this.ValidateUserPermission(user);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        return await _userRepository.DeleteAsyncById(Id);
    }

    public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
    {
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));
        var entities = await _userRepository.GetAllAsync(predicate);

        return entities;
    }

    public async Task<IEnumerable<User>> GetAllAsync(User entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        return await _userRepository.GetAllAsync(entity);
    }

    public async Task<User> GetAsync(Expression<Func<User, bool>> predicate)
    {
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        var entity = await _userRepository.GetAsync(predicate);

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        return entity;
    }

    public async Task<User> GetByIdAsync(long Id)
    {
        var entity = await _userRepository.GetByIdAsync(Id);

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        return entity;
    }

    public async Task<User> UpdateAsync(User entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrorsPermission = await this.ValidateUserPermission(entity);
        if (listErrorsPermission.Any())
            throw new ValidationException(listErrorsPermission);

        var listErrors = await this.Validate(entity);
        if (listErrors.Any())
            throw new ValidationException(listErrors);

        return await _userRepository.UpdateAsync(entity);
    }

    public async Task UpdateRangeAsync(IEnumerable<User> entities)
    {
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            var listErrors = await this.Validate(entity);
            if (listErrors.Any())
                throw new ValidationException(listErrors);
        }
        await _userRepository.UpdateRangeAsync(entities);
    }

    public async Task<List<ValidationFailure>> Validate(User entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        var listErrors = await _validator.ValidateAsync(entity);

        var validation = _validator.Validate(entity);

        if (!validation.IsValid)
            listErrors.Errors.AddRange(validation.Errors);

        var isValidadeEmail = await _userRepository.GetAsync(x => x.Email == entity.Email && x.Id != entity.Id);
        if (isValidadeEmail != null)
            listErrors.Errors.Add(new ValidationFailure("Usuário", $"Já existe um usuário {(isValidadeEmail.Situation == Situation.Active ? "ativa" : "inativa")} cadastrado com esse e-mail."));

        var isValidadeCpf = await _userRepository.GetAsync(x => x.Cpf == entity.Cpf && x.Id != entity.Id);
        if (isValidadeCpf != null)
            listErrors.Errors.Add(new ValidationFailure("Usuário", $"Já existe um usuário {(isValidadeCpf.Situation == Situation.Active ? "ativa" : "inativa")} cadastrado com esse CPF."));

        return listErrors.Errors.ToList();
    }

    public string GenerateNewHashPassword(string password)
    {
        _ = password ?? throw new ArgumentNullException(nameof(password));
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyHashPassword(string password, string currentPassword)
    {
        _ = password ?? throw new ArgumentNullException(nameof(password));
        _ = currentPassword ?? throw new ArgumentNullException(nameof(currentPassword));

        return BCrypt.Net.BCrypt.Verify(password, currentPassword);
    }

    public async Task<PagedListDto<UserResponseListDto>> GetAllUsersByFilterAsync(UserRequestListDto requestDto)
    {
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        var user = _mapper.Map<User>(requestDto);

        //var listErrorsPermission = await this.ValidateUserPermission(user);
        //if (listErrorsPermission.Any())
        //    throw new ValidationException(listErrorsPermission);

        var result = await _userRepository.GetAllUserByFilterAsync(requestDto);

        IEnumerable<UserResponseListDto> userDto = _mapper.Map<IEnumerable<User>, IEnumerable<UserResponseListDto>>(result);

        return new PagedListDto<UserResponseListDto>(
            userDto,
            result.CurrentPage,
            result.TotalPages,
            result.PageSize,
            result.TotalCount);
    }

    public async Task<List<ValidationFailure>> ValidateUserPermission(User entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));
        var listErrors =  new List<ValidationFailure>();

        var authAdmin = _authenticationService.IsAdmin();

        if (!authAdmin)
        {
            var authUserId = _authenticationService.UserId();
            if (authUserId != entity.Id)
                listErrors.Add(new ValidationFailure("Usuário", "Você não tem permissão para acessar esse usuário."));
        }
        return listErrors;
    }

    public async Task<AuthenticateResponseDto> LoginAsync(AuthenticateRequestDto requestDto)
    {
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        var user = await _userRepository.GetAsync(x => x.Email == requestDto.Email);
        if (user == null)
            throw new ValidationException("Senha ou Email inválido(s)");

        var responseLogin = _mapper.Map<AuthenticateResponseDto>(user);

        responseLogin.Token = await _authenticationService.AuthenticateUserAsync(user);
        if (responseLogin.Token == null)
            throw new ValidationException("Erro ao gerar token");

        return responseLogin;
    }
}