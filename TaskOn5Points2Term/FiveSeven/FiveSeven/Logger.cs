namespace FiveSeven;
public class OrderEventArgs : EventArgs
{
    public int OrderId { get; set; }
    public string StageName { get; set; } 
}
public class OrderProcessor
{
    public event EventHandler<OrderEventArgs> OnOrderStateChanged;
    
    protected virtual void RaiseOrderStateChanged(int orderId, string stageName)
    {
        OnOrderStateChanged?.Invoke(this, new OrderEventArgs { OrderId = orderId, StageName = stageName });
    }
}

public class AuditLogger
{
    public void Subscribe(OrderProcessor processor)
    {
        processor.OnOrderStateChanged += (sender, args) =>
        {
            Console.WriteLine($"[АУДИТ] Заказ {args.OrderId} прошел этап: {args.StageName} в {DateTime.Now:HH:mm:ss}");
        };
    }
}