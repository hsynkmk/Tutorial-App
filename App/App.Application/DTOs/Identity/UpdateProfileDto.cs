namespace App.Application.DTOs.Identity;
public class UpdateProfileDto
{
    public string Name { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
