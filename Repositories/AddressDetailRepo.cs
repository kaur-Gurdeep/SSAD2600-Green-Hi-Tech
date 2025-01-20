using GreenHiTech.Models;

namespace GreenHiTech.Repositories
{
    public class AddressDetailRepo
    {
        private readonly GreenHiTechContext _context;
        public AddressDetailRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all address details
        public List<AddressDetail> GetAll()
        {
            return _context.AddressDetails.ToList();
        }

        // Get address detail by id
        public AddressDetail? GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else if (!_context.AddressDetails.Any(ad => ad.PkId == id))
            {
                return null;
            }
            return _context.AddressDetails.Find(id);
        }

        // Add address detail
        public string Add(AddressDetail addressDetail)
        {
            try
            {
                _context.AddressDetails.Add(addressDetail);
                _context.SaveChanges();

                return $"success,Successfully created address detail ID: {addressDetail.PkId}";
            }
            catch (Exception e)
            {
                return $"error,Failed to create address detail: {e.Message}";
            }
        }

        // Update address detail
        public string Update(AddressDetail addressDetail)
        {
            if (Any(addressDetail.PkId))
            {
                try
                {
                    _context.AddressDetails.Update(addressDetail);
                    _context.SaveChanges();

                    return $"success,Successfully updated address detail ID: {addressDetail.PkId}";
                }
                catch (Exception ex)
                {
                    return $"error,Address detail could not be updated: {ex.Message}";
                }
            }
            else
            {
                return $"warning,Unable to find address detail ID: {addressDetail.PkId}";
            }
        }

        // Delete address detail
        public string Delete(int id)
        {
            AddressDetail? addressDetail = GetById(id);
            if (addressDetail == null)
            {
                return $"warning,Unable to find address detail ID: {id}";
            }

            try
            {
                _context.AddressDetails.Remove(addressDetail);
                _context.SaveChanges();
                return $"success,Successfully deleted address detail ID: {id}";
            }
            catch (Exception e)
            {
                return $"error,Failed to delete address detail: {e.Message}";
            }
        }

        // if address detail exists
        public bool Any(int id)
        {
            return _context.AddressDetails.Any(ad => ad.PkId == id);
        }
    }
}
