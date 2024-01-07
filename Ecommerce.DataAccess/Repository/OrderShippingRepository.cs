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
    public class OrderShippingRepository : IOrderShippingRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderShippingRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateOrderShipping(OrderShipping obj)
        {
            await _db.OrderShippings.AddAsync(obj);
            await _db.SaveChangesAsync();
        }
    }
}
