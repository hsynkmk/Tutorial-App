namespace App.Application.DTOs;
public class UpdateUserDto
{
    public string FullName { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
