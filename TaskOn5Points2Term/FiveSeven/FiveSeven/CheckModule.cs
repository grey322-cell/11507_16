namespace FiveSeven;
using System.Reflection;

public class CheckModule
{
    public bool ValidateOrder(Order order)
    {
        var properties = order.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var prop in properties)
        {
            if (Attribute.IsDefined(prop, typeof(RequiredAttribute)))
            {
                object value = prop.GetValue(order);
                
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    Console.WriteLine($"Ошибка: поле {prop.Name} обязательно для заполнения");
                    return false;
                }
                
                if (value is decimal dec && dec == 0)
                {
                    Console.WriteLine($"Ошибка: поле {prop.Name} не может быть равно 0");
                    return false;
                }
            }
        }
        
        return true;
    }
}