namespace FiveSeven;
using System.Reflection;

public interface IDiscountRule
{
    string RuleName { get; }
    decimal ApplyDiscount(Order order, decimal currentTotal);
}

public class DiscountModule
{
    public void ApplyAllDiscounts(Order order)
    {
        decimal total = order.TotalAmount;
        string appliedRules = "";
        
        var discountTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IDiscountRule).IsAssignableFrom(t));
        
        foreach (var type in discountTypes)
        {
            IDiscountRule rule = (IDiscountRule)Activator.CreateInstance(type);
            decimal newTotal = rule.ApplyDiscount(order, total);
            
            if (newTotal < total)  // скидка применилась
            {
                total = newTotal;
                appliedRules += (string.IsNullOrEmpty(appliedRules) ? "" : ", ") + rule.RuleName;
            }
        }
        
        order.TotalAmount = total;
        order.AppliedDiscountRule = appliedRules;
    }
}