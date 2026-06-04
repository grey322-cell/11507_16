namespace FiveSeven;
public class DeliveryModule
{
    private static Random _random = new Random();
    
    public async Task<bool> SendToDeliveryAsync(Order order)
    {
        if (order.Status != OrderStatus.Paid)
            throw new InvalidOperationException($"Заказ {order.Id} не оплачен. Текущий статус: {order.Status}");
        await Task.Delay(500);
        
        if (_random.Next(100) == 0) 
        {
            Console.WriteLine($"Ошибка внешнего API при отправке заказа {order.Id}");
            return false;
        }
        order.Status = OrderStatus.Sent;
        return true;
    }
}