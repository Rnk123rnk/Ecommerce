using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<int> CreateOrder(Order obj)
        {
            try
            {
                await _db.Orders.AddAsync(obj);
                await _db.SaveChangesAsync();
                return obj.OrderId;
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
         
        }
    }
}
