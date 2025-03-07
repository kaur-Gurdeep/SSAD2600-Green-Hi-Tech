using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class OrderRepo
    {
        private readonly GreenHiTechContext _context;
        public OrderRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all orders
        public List<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        //public Order? GetByUserId(int userId)
        //{
        //    return _context.Orders.FirstOrDefault(
        //        o => o.FkUserId == userId
        //        );
        //}

        // Get order by id
        public Order? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else if (!_context.Orders.Any(o => o.PkId == id))
            {
                return null;
            }
            return _context.Orders.Find(id);
        }

        // Add order
        public string Add(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

                return $"success,Successfully created order ID: {order.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create order: {e.Message}";
            }
        }

        // Update order
        public string Update(Order order)
        {
            if (Any(order.PkId))
            {
                try
                {
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                    return $"success,Successfully updated order ID: {order.PkId}";
                }
                catch (Exception ex)
                {
                    return $"error,Order could not be updated: {ex.Message}";
                }
            }
            else
            {
                return $"warning,Unable to find order ID: {order.PkId}";
            }
        }

        // Delete order
        public string Delete(int id)
        {
            Order? order = GetById(id);
            if (order == null)
            {
                return $"warning,Unable to find order ID: {id}";
            }

            try
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                return $"success,Successfully deleted order ID: {id}";
            }
            catch (Exception e)
            {
                return $"error,Failed to delete order: {e.Message}";
            }
        }

        // if order exists
        public bool Any(int id)
        {
            return _context.Orders.Any(o => o.PkId == id);
        }
    }
}
