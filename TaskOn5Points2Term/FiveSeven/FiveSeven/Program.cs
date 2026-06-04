namespace FiveSeven;

class Program
{
    static async Task Main()
    {
        //Быстрый тест
        //Создаём тестовые заказы
        var orders = new List<Order>
        {
            new Order { Id = 1, UserEmail = "ivan@mail.ru", TotalAmount = 5000, Status = OrderStatus.New },
            new Order { Id = 2, UserEmail = "vip@corp.com", TotalAmount = 15000, Status = OrderStatus.New },
            new Order { Id = 3, UserEmail = "", TotalAmount = 1000, Status = OrderStatus.New },        // невалидный
            new Order { Id = 4, UserEmail = "loyal@shop.ru", TotalAmount = 8000, Status = OrderStatus.New },
            new Order { Id = 5, UserEmail = "guest@mail.com", TotalAmount = 3000, Status = OrderStatus.New }
        };

        //логгер
        var batchProcessor = new BatchProcessor();
        var auditLogger = new AuditLogger();
        
        batchProcessor.OnOrderStateChanged += (sender, args) =>
        {
            Console.WriteLine($"[АУДИТ] Заказ {args.OrderId} прошел этап: {args.StageName} в {DateTime.Now:HH:mm:ss}");
        };

        //Oбработкa
        await batchProcessor.ProcessBatchAsync(orders);
        
        foreach (var order in orders)
        {
            Console.WriteLine($"Заказ {order.Id}: Статус = {order.Status}, Итог = {order.TotalAmount:F2}");
        }
    }
}