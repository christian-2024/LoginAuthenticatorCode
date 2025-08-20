using FluentValidation;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.Validation;

public class PermissionValidator : AbstractValidator<Permission>
{
    public PermissionValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("É necessário atribuir um nome")
            .MaximumLength(150).WithMessage("O nome pode ter no máximo 150 caracteres");
        RuleFor(c => c.TypeAccess)
            .NotEmpty().WithMessage("É necessário atribuir o tipo de acesso")
            .IsInEnum().WithMessage("O tipo de acesso fornecido não é válido.");
        RuleFor(c => c.Description)
            .MaximumLength(250).WithMessage("A descrição pode ter no máximo 250 caracteres");
        RuleFor(c => c.Situation)
            .Must(s => s == Situation.Active || s == Situation.Inactive)
            .WithMessage("Situação inválida! Apenas os valores 1 e 2 são permitidos.");
    }
}