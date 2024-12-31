namespace App.Application.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
    public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

}
