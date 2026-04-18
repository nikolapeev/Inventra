using Inventra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventra.Data
{
    public class InventraDbSeeder
    {
        public static void Seed(InventraDbContext context)
        {
            // Прилагане на миграциите преди сийдване
            context.Database.Migrate();

            // Проверка дали вече има данни (за да не се дублират при всяко стартиране)
            if (context.Categories.Any() || context.Products.Any() || context.Customers.Any())
            {
                return;
            }

            // 1. Categories (5 броя)
            var categories = new[]
            {
                new Category { CategoryId = Guid.NewGuid(), Name = "Електроника" },
                new Category { CategoryId = Guid.NewGuid(), Name = "Компютърна периферия" },
                new Category { CategoryId = Guid.NewGuid(), Name = "Мрежово оборудване" },
                new Category { CategoryId = Guid.NewGuid(), Name = "Офис консумативи" },
                new Category { CategoryId = Guid.NewGuid(), Name = "Софтуер" }
            };
            context.Categories.AddRange(categories);

            // 2. Suppliers (5 броя)
            var suppliers = new[]
            {
                new Supplier { SupplierId = Guid.NewGuid(), Name = "ТехноПлюс ООД", EIK = "123456789", PhoneNumber = "+359888111222", Email = "sales@technoplus.bg" },
                new Supplier { SupplierId = Guid.NewGuid(), Name = "СмартТрейд АД", EIK = "987654321", PhoneNumber = "+359899333444", Email = "office@smarttrade.bg" },
                new Supplier { SupplierId = Guid.NewGuid(), Name = "ХардуерБГ ЕООД", EIK = "112233445", PhoneNumber = "+359877555666", Email = "contact@hardware.bg" },
                new Supplier { SupplierId = Guid.NewGuid(), Name = "Глобал Нет ООД", EIK = "554433221", PhoneNumber = "+359888999000", Email = "info@globalnet.bg" },
                new Supplier { SupplierId = Guid.NewGuid(), Name = "МегаСофт АД", EIK = "998877665", PhoneNumber = "+359898123456", Email = "sales@megasoft.bg" }
            };
            context.Suppliers.AddRange(suppliers);

            // 3. Couriers (5 броя)
            var couriers = new[]
            {
                new Courier { CourierId = Guid.NewGuid(), Name = "Еконт Експрес", Phone = "070017300" },
                new Courier { CourierId = Guid.NewGuid(), Name = "Спиди АД", Phone = "070017001" },
                new Courier { CourierId = Guid.NewGuid(), Name = "DHL България", Phone = "070017700" },
                new Courier { CourierId = Guid.NewGuid(), Name = "Лео Експрес", Phone = "080018100" },
                new Courier { CourierId = Guid.NewGuid(), Name = "Интерлогистика", Phone = "070012222" }
            };
            context.Couriers.AddRange(couriers);

            // 4. Customers (5 броя)
            var customers = new[]
            {
                new Customer { CustomerId = Guid.NewGuid(), FullName = "Иван Иванов", PhoneNumber = "0888123456", CompanyName="ЕлектроХолд АД",Email = "ivan.ivanov@abv.bg", Country = "България", County = "Стара Загора", City = "Казанлък", Address = "бул. Розова долина 15", PostalCode = "6100", EIK = "123123123", ZDDS = false },
                new Customer { CustomerId = Guid.NewGuid(), FullName = "Петър Петров", PhoneNumber = "0899654321",CompanyName="ХидроПрофИнвест ООД", Email = "peter.p@gmail.com", Country = "България", County = "Стара Загора", City = "Стара Загора", Address = "бул. Цар Симеон Велики 100", PostalCode = "6000", EIK = "321321321", ZDDS = false },
                new Customer { CustomerId = Guid.NewGuid(), FullName = "Мария Георгиева", PhoneNumber = "0877112233",CompanyName="ЕТ Мария Георгиева", Email = "maria.g@yahoo.com", Country = "България", County = "Пловдив", City = "Пловдив", Address = "ул. Главна 5", PostalCode = "4000", EIK = "111222333", ZDDS = true },
                new Customer { CustomerId = Guid.NewGuid(), FullName = "Елена Димитрова", PhoneNumber = "0888998877",CompanyName="Елена и сие ЕООД", Email = "elena.dimitrova@mail.bg", Country = "България", County = "София", City = "София", Address = "бул. Витоша 20", PostalCode = "1000", EIK = "444555666", ZDDS = false},
                new Customer { CustomerId = Guid.NewGuid(), FullName = "Стоян Стоянов", PhoneNumber = "0898445566",CompanyName="Еко Радо АД", Email = "stoyan.s@abv.bg", Country = "България", County = "Бургас", City = "Бургас", Address = "ул. Александровска 10", PostalCode = "8000", EIK = "777888999", ZDDS = true }
            };
            context.Customers.AddRange(customers);

            // 5. Products (5 броя)
            var products = new[]
            {
                new Product { Id = Guid.NewGuid(), Name = "Лаптоп Lenovo ThinkPad T14", CategoryId = categories[0].CategoryId, Description = "Бизнес лаптоп с Intel Core i7", Price = 2499.00m, StockQuantity = 15, ImageURL = "/images/thinkpad.jpg", SupplierId = suppliers[0].SupplierId, BatchNumber = "B-2026-01", AddedBy = "admin@inventra.com", WarehouseLocationId = "A1-S1" },
                new Product { Id = Guid.NewGuid(), Name = "Мишка Logitech MX Master 3S", CategoryId = categories[1].CategoryId, Description = "Ергономична безжична мишка", Price = 199.50m, StockQuantity = 40, ImageURL = "/images/mxmaster.jpg", SupplierId = suppliers[1].SupplierId, BatchNumber = "B-2026-02", AddedBy = "admin@inventra.com", WarehouseLocationId = "B3-S2" },
                new Product { Id = Guid.NewGuid(), Name = "Рутер TP-Link Archer AX73", CategoryId = categories[2].CategoryId, Description = "Wi-Fi 6 Gigabit Router", Price = 245.00m, StockQuantity = 25, ImageURL = "/images/tplink.jpg", SupplierId = suppliers[2].SupplierId, BatchNumber = "B-2026-03", AddedBy = "admin@inventra.com", WarehouseLocationId = "C2-S1" },
                new Product { Id = Guid.NewGuid(), Name = "Офис стол Ergonomic Pro", CategoryId = categories[3].CategoryId, Description = "Ергономичен офис стол с лумбална опора", Price = 350.00m, StockQuantity = 10, ImageURL = "/images/chair.jpg", SupplierId = suppliers[3].SupplierId, BatchNumber = "B-2026-04", AddedBy = "admin@inventra.com", WarehouseLocationId = "D4-S1" },
                new Product { Id = Guid.NewGuid(), Name = "Microsoft Office 2026 License", CategoryId = categories[4].CategoryId, Description = "Доживотен лиценз за Office пакет", Price = 499.99m, StockQuantity = 100, ImageURL = "/images/office.jpg", SupplierId = suppliers[4].SupplierId, BatchNumber = "B-2026-05", AddedBy = "admin@inventra.com", WarehouseLocationId = "Digital" }
            };
            context.Products.AddRange(products);

            // 6. Orders (5 броя)
            var orders = new[]
            {
                new Order { Id = Guid.NewGuid(), CustomerId = customers[0].CustomerId, CourierId = couriers[0].CourierId, TrackingNumber = "EC100000001BG", TotalPrice = 2499.00m, AdditionalInfo = "Доставка след 14:00 часа" },
                new Order { Id = Guid.NewGuid(), CustomerId = customers[1].CustomerId, CourierId = couriers[1].CourierId, TrackingNumber = "SP200000002BG", TotalPrice = 199.50m, AdditionalInfo = "Обаждане преди доставка" },
                new Order { Id = Guid.NewGuid(), CustomerId = customers[2].CustomerId, CourierId = couriers[2].CourierId, TrackingNumber = "DH300000003BG", TotalPrice = 490.00m, AdditionalInfo = "Оставете на рецепция" },
                new Order { Id = Guid.NewGuid(), CustomerId = customers[3].CustomerId, CourierId = couriers[0].CourierId, TrackingNumber = "EC100000004BG", TotalPrice = 2849.00m, AdditionalInfo = "Чупливо" },
                new Order { Id = Guid.NewGuid(), CustomerId = customers[4].CustomerId, CourierId = couriers[1].CourierId, TrackingNumber = "SP200000005BG", TotalPrice = 499.99m, AdditionalInfo = "Изпращане на лиценза по имейл" }
            };
            context.Orders.AddRange(orders);

            // 7. Order Details (Най-малко 5 броя, свързани с поръчките)
            var orderDetails = new[]
            {
                // Поръчка 1: 1 Лаптоп
                new OrderDetails { OrderId = orders[0].Id, ProductId = products[0].Id, QTY = 1, Subtotal = 2499.00m },
                
                // Поръчка 2: 1 Мишка
                new OrderDetails { OrderId = orders[1].Id, ProductId = products[1].Id, QTY = 1, Subtotal = 199.50m },
                
                // Поръчка 3: 2 Рутера (2 * 245 = 490)
                new OrderDetails { OrderId = orders[2].Id, ProductId = products[2].Id, QTY = 2, Subtotal = 490.00m },
                
                // Поръчка 4: 1 Лаптоп и 1 Офис стол (2499 + 350 = 2849)
                new OrderDetails { OrderId = orders[3].Id, ProductId = products[0].Id, QTY = 1, Subtotal = 2499.00m },
                new OrderDetails { OrderId = orders[3].Id, ProductId = products[3].Id, QTY = 1, Subtotal = 350.00m },
                
                // Поръчка 5: 1 Софтуерен лиценз
                new OrderDetails { OrderId = orders[4].Id, ProductId = products[4].Id, QTY = 1, Subtotal = 499.99m }
            };
            context.OrderDetails.AddRange(orderDetails);

            // Запазване на всички промени в базата
            context.SaveChanges();
        }
    }
}
