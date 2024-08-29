using KuSonchik.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace KuSonchik.Utilities
{
    internal class DBAPI
    {
        // Scaffold-DbContext "Host=localhost;Port=5432;Database=course_work_crm;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -Force -Context CRMContext -OutputDir Models
        private CRMContext Context { get; set; } = new();

        private IQueryable<T> GetTable<T>() where T : class => Context.Set<T>();


        internal async Task LoadTable<T>(IList table) where T : class
        {
            var list = await GetTable<T>().ToListAsync();
            if (list.Count == 0) return;
            table.Clear();
            for (int i = 0; i < list.Count; ++i) table.Add(list[i]);
        }

        #region Добавление записей
        internal async Task AddOrder(Order item)
        {
            using (CRMContext db = new())
            {
                await db.Orders.AddAsync(item);
                db.SaveChanges();
            }
        }
        internal async Task AddDelievery(Delievery item)
        {
            using (CRMContext db = new())
            {
                await db.Delieveries.AddAsync(item);
                db.SaveChanges();
            }
        }
        internal async Task AddProduct(Product item)
        {
            using (CRMContext db = new())
            {
                await db.Products.AddAsync(item);
                db.SaveChanges();
            }
        }
        internal async Task AddClient(Client item)
        {
            using (CRMContext db = new())
            {
                await db.Clients.AddAsync(item);
                db.SaveChanges();
            }
        }
        internal async Task AddEmployee(Supplier item)
        {
            using (CRMContext db = new())
            {
                await db.Suppliers.AddAsync(item);
                db.SaveChanges();
            }
        }
        #endregion
        #region Редактирование записей
        internal async Task EditOrder(Order item)
        {
            Context.Orders.Update(item);
            Context.SaveChanges();
        }
        internal async Task EditDelievery(Delievery item)
        {
            Context.Delieveries.Update(item);
            Context.SaveChanges();
        }
        internal async Task EditProduct(Product item)
        {
            Context.Products.Update(item);
            Context.SaveChanges();
        }
        internal async Task EditClient(Client item)
        {
            Context.Clients.Update(item);
            Context.SaveChanges();
        }
        internal async Task EditEmployee(Supplier item)
        {
            Context.Suppliers.Update(item);
            Context.SaveChanges();
        }
        #endregion
        #region Удаление записей
        internal async Task DeleteOrder(Order item)
        {
            Context.Orders.Remove(item);
            Context.SaveChanges();
        }
        internal async Task DeleteDelievery(Delievery item)
        {
            Context.Delieveries.Remove(item);
            Context.SaveChanges();
        }
        internal async Task DeleteProduct(Product item)
        {
            Context.Products.Remove(item);
            Context.SaveChanges();
        }
        internal async Task DeleteClient(Client item)
        {
            Context.Clients.Remove(item);
            Context.SaveChanges();
        }
        internal async Task DeleteEmployee(Supplier item)
        {
            Context.Suppliers.Remove(item);
            Context.SaveChanges();
        }
        #endregion
    }
}
