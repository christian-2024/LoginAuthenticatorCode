namespace LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;

public class PermissionResponseListDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<string>? Errors { get; set; } = new List<string>();
}