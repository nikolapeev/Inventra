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
    }
}
