using FluentValidation;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.Validation;

public class NotificationValidator : AbstractValidator<Notification>
{
    public NotificationValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("O titulo não pode ser vazio")
            .MinimumLength(255).WithMessage("O titulo pode ter no máximo 255 caracteres");
        RuleFor(c => c.Description)
             .NotEmpty().WithMessage("A descrição não pode ser vazio")
            .MinimumLength(500).WithMessage("A descrição pode ter no máximo 500 caracteres");

        RuleFor(c => c.UserId)
            .GreaterThan(0).WithMessage("É necessário atribuir o Id de um cliente");
        RuleFor(c => c.Situation)
            .Must(s => s == Situation.Active || s == Situation.Inactive)
            .WithMessage("Situação inválida! Apenas os valores 1 e 2 são permitidos.");
    }
}