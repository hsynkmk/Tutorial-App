using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Order;
public class CreateOrderDto
{
    public int CourseId { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TransactionId { get; set; }
    public List<CreateOrderDetailDto>? OrderDetails { get; set; }
}
