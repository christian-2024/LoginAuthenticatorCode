using FluentValidation;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.Validation;

public class UserValidador : AbstractValidator<User>
{
    public UserValidador()
    {
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("O nome do usuario não pode ser vazio")
            .MinimumLength(150).WithMessage("O nome do usuário tem no maximo 150 caracteres");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("O campo Email não pode ser vazio.")
            .MinimumLength(80).WithMessage("O campo Email tem no maximo 80 caracteres");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("O campo Email não pode ser vazio");

        RuleFor(user => user.PhoneNumber)
            .MinimumLength(16).WithMessage("O campo telefone tem no maximo 16 caracteres");

        RuleFor(x => x.PermissionId)
               .GreaterThan(0).WithMessage("É necessário incluir o tipo de acesso");

        RuleFor(user => user.Cpf)
            .MinimumLength(16).WithMessage("O campo Cpf tem no maximo 16 caracteres");

        RuleFor(user => user.Situation)
            .Must(s => s == Situation.Active || s == Situation.Inactive)
            .WithMessage("A situação inválida! Apenas os valores 1 e 2 são permitidos.");
    }
}