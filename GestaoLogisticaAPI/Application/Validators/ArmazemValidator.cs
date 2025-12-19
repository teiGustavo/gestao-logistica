using FluentValidation;
using GestaoLogisticaAPI.Application.DTOs;

namespace GestaoLogisticaAPI.Application.Validators;

public class ArmazemValidator : AbstractValidator<ArmazemDto>
{
    public ArmazemValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome muito grande, seu animal.");
    }
}