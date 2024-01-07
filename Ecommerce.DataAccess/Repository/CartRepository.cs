using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateCart(Cart obj)
        {
            int? productQuantityLeft = await _db.Products
                                      .Where(u => u.ProductId == obj.ProductId)
                                      .Select(u => u.Quantity)
                                      .FirstOrDefaultAsync();

            if (!productQuantityLeft.HasValue)
            {
                return false; // Product not found or quantity information not available
            }

            if (obj.UserId > 0)
            {
                var existingUserCartItem = await _db.Carts.FirstOrDefaultAsync(u => u.UserId == obj.UserId && u.ProductId == obj.ProductId);
                if (existingUserCartItem != null)
                {
                    int totalQuantity = existingUserCartItem.Quantity + obj.Quantity;

                    if (totalQuantity > productQuantityLeft.Value || totalQuantity < 0)
                    {
                        return false; // Quantity exceeds available product quantity or goes negative
                    }

                    existingUserCartItem.Quantity = totalQuantity; // Update quantity directly

                    if (totalQuantity == 0)
                    {
                        _db.Carts.Remove(existingUserCartItem); // Remove the item from the cart when quantity becomes 0
                    }
                }
                else
                {
                    if (obj.Quantity > productQuantityLeft.Value)
                    {
                        return false; // Quantity exceeds available product quantity
                    }
                    await _db.Carts.AddAsync(obj); // Add new cart item
                }
            }
            else if (!string.IsNullOrEmpty(obj.SessionId))
            {
                var existingSessionCartItem = await _db.Carts.FirstOrDefaultAsync(u => u.SessionId == obj.SessionId && u.ProductId == obj.ProductId);
                if (existingSessionCartItem != null)
                {
                    int totalQuantity = existingSessionCartItem.Quantity + obj.Quantity;

                    if (totalQuantity > productQuantityLeft.Value || totalQuantity < 0)
                    {
                        return false; // Quantity exceeds available product quantity or goes negative
                    }

                    existingSessionCartItem.Quantity = totalQuantity; // Update quantity directly

                    if (totalQuantity == 0)
                    {
                        _db.Carts.Remove(existingSessionCartItem); // Remove the item from the cart when quantity becomes 0
                    }
                }
                else
                {
                    if (obj.Quantity > productQuantityLeft.Value)
                    {
                        return false; // Quantity exceeds available product quantity
                    }
                    await _db.Carts.AddAsync(obj); // Add new cart item
                }
            }
            await _db.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> DeleteCart(int id)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(u => u.CartId == id);
            if (cart != null)
            {
                _db.Carts.Remove(cart);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task DeleteCartByUserId(int id)
        {
            var carts = await _db.Carts.Where(u => u.UserId == id).ToListAsync();
            if (carts != null && carts.Any())
            {
                _db.Carts.RemoveRange(carts);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<List<CartListDTO>> GetCartByIdList(int userId,string randomid)
        {
            if(userId > 0)
            {
                var cartList = await (from cart in _db.Carts
                                      join product in _db.Products
                                      on cart.ProductId equals product.ProductId
                                      where cart.UserId == userId
                                      select new CartListDTO
                                      {
                                          CartId = cart.CartId,
                                          Quantity = cart.Quantity,
                                          ProductId = product.ProductId,
                                          ProductName = product.ProductName,
                                          ProductPrice = product.ProductMRPPrice,
                                          ProductPriceDiscount = product.ProductDiscountedPrice != null ? (int)product.ProductDiscountedPrice : 0,
                                          ProductImage = product.ProductImage1,

                                      }).ToListAsync();
                return cartList;
            }
            else if(randomid != null)
            {
                var cartList = await (from cart in _db.Carts
                                      join product in _db.Products
                                      on cart.ProductId equals product.ProductId
                                      where cart.SessionId == randomid
                                      select new CartListDTO
                                      {
                                          CartId = cart.CartId,
                                          Quantity = cart.Quantity,
                                          ProductName = product.ProductName,
                                          ProductPrice = product.ProductMRPPrice,
                                          ProductPriceDiscount = product.ProductDiscountedPrice != null ? (int)product.ProductDiscountedPrice : 0,
                                          ProductImage = product.ProductImage1,

                                      }).ToListAsync();
                return cartList;
            }
            return null;
                
        }
        public async Task UpdateCartWhenLogin(int userId, string randomNumber)
        {
            var cartItems = await _db.Carts.Where(u => u.SessionId == randomNumber).ToListAsync();
            var userCartItems = await _db.Carts.Where(u => u.UserId == userId).ToListAsync();

            foreach (var item in cartItems)
            {
                var existingItem = userCartItems.FirstOrDefault(u => u.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                    if (product != null)
                    {
                        if (product.Quantity >= existingItem.Quantity + item.Quantity)
                        {
                            existingItem.Quantity += item.Quantity;
                        }
                        else
                        {
                            existingItem.Quantity = product.Quantity;
                        }
                    }
                }
                else
                {
                    var newCartItem = new Cart
                    {
                        UserId = userId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };
                    _db.Carts.Add(newCartItem);
                }
            }
            await _db.SaveChangesAsync();
        }

    }
}
