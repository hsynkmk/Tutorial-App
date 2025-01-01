using App.Domain.Common;

namespace App.Domain.Entities;
public class CartItem : BaseEntity
{
    public int Id { get; set; }

    public ApplicationUser User { get; set; }

    public int CourseId { get; set; }

    public Course Course { get; set; }

}
