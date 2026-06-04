namespace FiveSeven;
public class BatchProcessor
{
    private readonly CheckModule _validator = new CheckModule();
    private readonly DiscountModule _discounter = new DiscountModule();
    private readonly DeliveryModule _delivery = new DeliveryModule();
    
    public async Task ProcessBatchAsync(List<Order> orders)
    {
        using var semaphore = new SemaphoreSlim(5);
        
        var tasks = orders.Select(order => ProcessOrderAsync(order, semaphore));
        await Task.WhenAll(tasks);
    }
    
    private async Task ProcessOrderAsync(Order order, SemaphoreSlim semaphore)
    {
        await semaphore.WaitAsync();  
        try
        {
            await ProcessSingleOrder(order);
        }
        finally
        {
            semaphore.Release(); 
        }
    }
    
    private async Task ProcessSingleOrder(Order order)
    {
        Console.WriteLine($"\nОбработка заказа {order.Id}");
        
        if (!_validator.ValidateOrder(order))
        {
            order.Status = OrderStatus.Rejected;
            return;
        }
        RaiseOrderStateChanged(order.Id, "Валидация");
        order.Status = OrderStatus.Paid;
        RaiseOrderStateChanged(order.Id, "Оплата");
        
        _discounter.ApplyAllDiscounts(order);
        RaiseOrderStateChanged(order.Id, "Применение скидок");
        
        bool deliverySuccess = await _delivery.SendToDeliveryAsync(order);
        if (deliverySuccess)
            RaiseOrderStateChanged(order.Id, "Отправка в доставку");
        else
            Console.WriteLine($"Внешнее API отклонило заказ {order.Id}");
    }
    
    public event EventHandler<OrderEventArgs> OnOrderStateChanged;
    protected virtual void RaiseOrderStateChanged(int orderId, string stageName)
    {
        OnOrderStateChanged?.Invoke(this, new OrderEventArgs { OrderId = orderId, StageName = stageName });
    }
}


