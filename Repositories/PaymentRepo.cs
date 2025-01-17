using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class PaymentRepo
    {
        private readonly GreenHiTechContext _context;
        public PaymentRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all payments
        public List<Payment> GetAll()
        {
            return _context.Payments.ToList();
        }

        // Get payment by id
        public Payment? Get(int id)
        {
            return _context.Payments.Find(id);

        }

        // Add payment
        public string Add(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                _context.SaveChanges();
                return $"success,Successfully created payment ID: " +
                       $"{payment.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create payment: {e.Message}";
            }
        }

        // Update payment
        public string Update(Payment payment)
        {
            if (Get(payment.PkId) != null)
            {
                try
                {
                    _context.Payments.Update(payment);
                    _context.SaveChanges();
                    return $"success,Successfully updated payment ID: {payment.PkId}";
                }
                catch (Exception e)
                {
                    return $"error,Failed to update payment: {e.Message}";
                }
            }
            return $"error,Payment ID: {payment.PkId} not found";
        }

        // Delete payment
        public string Delete(int id)
        {
            Payment? payment = Get(id);
            if (payment != null)
            {
                try
                {
                    _context.Payments.Remove(payment);
                    _context.SaveChanges();
                    return $"success,Successfully deleted payment ID: {id}";
                }
                catch (Exception e)
                {
                    return $"error,Failed to delete payment: {e.Message}";
                }
            }
            return $"error,Payment ID: {id} not found";
        }

        // Check if payment exists
        public bool Any(int id)
        {
            return _context.Payments.Any(p => p.PkId == id);
        }
    }
}
