namespace FiveSeven;

public enum OrderStatus
{
    New,        // новый
    Paid,       // оплачен
    Rejected,   // отклонён
    Sent        // отправлен в доставку
}
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute { }
public class Order
{
    public int Id { get; set; }
    
    [Required]
    public string UserEmail { get; set; }
    
    [Required]
    public decimal TotalAmount { get; set; }
    
    public OrderStatus Status { get; set; } = OrderStatus.New;
    public decimal Discount { get; set; }
    public string AppliedDiscountRule { get; set; }
}