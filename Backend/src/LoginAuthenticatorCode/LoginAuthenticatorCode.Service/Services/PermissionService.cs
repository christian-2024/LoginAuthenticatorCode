using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Repository;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using System.Linq.Expressions;

namespace LoginAuthenticatorCode.Service.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<Permission> _validator;

    public PermissionService(IPermissionRepository permissionRepository, IMapper mapper, IValidator<Permission> validator)
    {
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }
    public async Task<Permission> AddAsync(Permission entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        entity.Name = entity.Name.Trim();

        var listErrors = await this.Validate(entity);
        if (listErrors.Any())
            throw new ValidationException(listErrors);

        await _permissionRepository.AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IList<Permission> entities)
    {
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            entity.Name = entity.TypeAccess.ToString();

            var listErrors = await this.Validate(entity);
            if (listErrors.Any())
                throw new ValidationException(listErrors);
        }
        await _permissionRepository.AddRangeAsync(entities);
    }

    public async Task<Permission> DeleteAsync(Permission entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        await _permissionRepository.DeleteAsync(entity);
        return entity;
    }

    public async Task<Permission> DeleteAsyncById(long Id)
    {
        var entity = await _permissionRepository.GetByIdAsync(Id);

        return await _permissionRepository.DeleteAsyncById(Id);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync(Expression<Func<Permission, bool>> predicate)
    {
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        return await _permissionRepository.GetAllAsync(predicate);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync(Permission entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        return await _permissionRepository.GetAllAsync(entity);
    }

    public async Task<PagedListDto<PermissionResponseListDto>> GetAllPermissionsAdminAsync(PermissionRequestListDto requestDto)
    {
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        var result = await _permissionRepository.GetAllPermissionsAdminAsync(requestDto);

        IEnumerable<PermissionResponseListDto> responseList = _mapper.Map<IEnumerable<Permission>, IEnumerable<PermissionResponseListDto>>(result);

        return new PagedListDto<PermissionResponseListDto>(responseList, requestDto.PageNumber, requestDto.PageSize, result.TotalCount);
    }

    public async Task<PagedListDto<PermissionResponseListDto>> GetAllPermissionsByFilterAsync(PermissionRequestListDto requestDto)
    {
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        var result = await _permissionRepository.GetAllPermissionsByFilterAsync(requestDto);

        IEnumerable<PermissionResponseListDto> responseList = _mapper.Map<IEnumerable<Permission>, IEnumerable<PermissionResponseListDto>>(result);

        return new PagedListDto<PermissionResponseListDto>(responseList, requestDto.PageNumber, requestDto.PageSize, result.TotalCount);
    }

    public async Task<PagedListDto<PermissionResponseListDto>> GetAllPermissionsClientAsync(PermissionRequestListDto requestDto)
    {
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        var result = await _permissionRepository.GetAllPermissionsByFilterAsync(requestDto);

        IEnumerable<PermissionResponseListDto> responseList = _mapper.Map<IEnumerable<Permission>, IEnumerable<PermissionResponseListDto>>(result);

        return new PagedListDto<PermissionResponseListDto>(responseList, requestDto.PageNumber, requestDto.PageSize, result.TotalCount);
    }

    public async Task<Permission> GetAsync(Expression<Func<Permission, bool>> predicate)
    {
        _ = predicate ?? throw new ArgumentNullException(nameof(predicate));

        var entity = await _permissionRepository.GetAsync(predicate);

        return entity;
    }

    public async Task<Permission> GetByIdAsync(long Id)
    {
        var entity = await _permissionRepository.GetByIdAsync(Id);

        return entity;
    }

    public async Task<Permission> UpdateAsync(Permission entity)
    {
        entity.Name = entity.TypeAccess.ToString(); //numero inteiro passa a ser string

        var listErrors = await this.Validate(entity);
        if (listErrors.Any())
            throw new ValidationException(listErrors);

        return await _permissionRepository.UpdateAsync(entity);
    }

    public async Task UpdateRangeAsync(IEnumerable<Permission> entities)
    {
        foreach (var entity in entities)
        {
            entity.Name = entity.TypeAccess.ToString(); 

            var listErrors = await this.Validate(entity);
            if (listErrors.Any())
                throw new ValidationException(listErrors);
        }
        await _permissionRepository.UpdateRangeAsync(entities);
    }

    public async Task<List<ValidationFailure>> Validate(Permission entity)
    {
        var listErrors = new List<ValidationFailure>();

        var validaton = _validator.Validate(entity);

        if (!validaton.IsValid)
             listErrors.AddRange(validaton.Errors);

        var isValidationPermission = await _permissionRepository.GetAsync(u => u.TypeAccess == entity.TypeAccess);
        if (isValidationPermission != null)
             listErrors.Add(new ValidationFailure(nameof(entity.TypeAccess), "Já existe uma permissão com este tipo de acesso."));

        return listErrors;
    }

    public Task<List<ValidationFailure>> ValidateUserPermission(Permission entity)
    {
        throw new NotImplementedException();
    }
}