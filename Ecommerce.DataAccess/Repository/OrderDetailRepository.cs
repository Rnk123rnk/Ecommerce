using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
   
        public async Task CreateOrderDetail(OrderDetail obj)
        {
            await _db.OrderDetails.AddAsync(obj);
            await _db.SaveChangesAsync();
        }
    }
}
