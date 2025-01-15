using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class OrderDetailRepo
    {
        private readonly GreenHiTechContext _context;
        public OrderDetailRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all order details
        public List<OrderDetail> GetAll()
        {
            return _context.OrderDetails.ToList();
        }

        // Get order detail by id
        public OrderDetail? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else if (!_context.OrderDetails.Any(od => od.PkId == id))
            {
                return null;
            }
            return _context.OrderDetails.Find(id);
        }

        // Add order detail
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

        // Update order detail
        public string Update(OrderDetail orderDetail)
        {
            if (Any(orderDetail.PkId))
            {
                try
                {
                    _context.OrderDetails.Update(orderDetail);
                    _context.SaveChanges();

                    return $"success,Successfully updated order detail ID: {orderDetail.PkId}";
                }
                catch (Exception ex)
                {
                    return $"error,Order detail could not be updated: {ex.Message}";
                }
            }
            else
            {
                return $"warning,Unable to find order detail ID: {orderDetail.PkId}";
            }
        }

        // Delete order detail
        public string Delete(int id)
        {
            OrderDetail? orderDetail = GetById(id);
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

        // if order detail exists
        public bool Any(int id)
        {
            return _context.OrderDetails.Any(od => od.PkId == id);
        }
    }
}
