using GreenHiTech.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GreenHiTech.Repositories
{
    public class OrderDetailRepo
    {
        private readonly GreenHiTechContext _context;

        public OrderDetailRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        public List<OrderDetail> GetAll()
        {
            return _context.OrderDetails
                .Include(od => od.FkOrder)
                .Include(od => od.FkProduct)
                .ToList();
        }

        public OrderDetail GetById(int id)
        {
            return _context.OrderDetails
                .Include(od => od.FkOrder)
                .Include(od => od.FkProduct)
                .FirstOrDefault(od => od.PkId == id);
        }

        public List<OrderDetail> GetByOrderId(int orderId)
        {
            return _context.OrderDetails
                .Include(od => od.FkOrder)
                .Include(od => od.FkProduct)
                .Where(od => od.FkOrderId == orderId)
                .ToList();
        }

        public string Add(OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
                return $"success,Successfully created order detail ID: {orderDetail.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create order detail: {e.Message}";
            }
        }

        public string Update(OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Update(orderDetail);
                _context.SaveChanges();
                return $"success,Successfully updated order detail ID: {orderDetail.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to update order detail: {e.Message}";
            }
        }

        public string Delete(int id)
        {
            var orderDetail = _context.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return $"warning,Unable to find order detail ID: {id}";
            }

            try
            {
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();
                return $"success,Successfully deleted order detail ID: {id}";
            }
            catch (Exception e)
            {
                return $"error,Failed to delete order detail: {e.Message}";
            }
        }
    }
}
