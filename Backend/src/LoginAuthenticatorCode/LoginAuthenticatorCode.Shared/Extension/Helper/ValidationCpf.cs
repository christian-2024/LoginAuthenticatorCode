namespace SmartDigitalWorkshop.Shared.Extensions.Helpers;

public static class ValidationCpf
{
    public static bool IdentifyInput(string input)
    {
        return input.Length switch
        {
            11 when ValidateCpf(input) => true,
            10 or 11 => false,
            _ => throw new NotImplementedException()
        };
    }

    private static bool ValidateCpf(string cpf)
    {
        if (cpf.Length != 11) return false;

        // Verifica se todos os dígitos são iguais (CPF inválido)
        if (new string(cpf[0], cpf.Length) == cpf) return false;

        // Calcula os dígitos verificadores
        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(cpf[i].ToString()) * multiplicadores1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(cpf[i].ToString()) * multiplicadores2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digito1}{digito2}");
    }
}