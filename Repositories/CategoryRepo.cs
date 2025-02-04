using GreenHiTech.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GreenHiTech.Repositories
{
    public class CategoryRepo
    {
        private readonly GreenHiTechContext _context;
        public CategoryRepo(GreenHiTechContext context)
        {
            _context = context;
        }

        // Get all categories
        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        // Get category by id
        public Category? GetById(int id)
        {
            if(id == 0)
            {
                return null;
            }
            else if(!_context.Categories.Any(c => c.PkId == id))
            {
                return null;
            }
            return _context.Categories.Find(id);
        }

        // Add category
        public string Add(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                return $"success,Successfully created category ID: {category.PkId}";
            }
            catch(Exception e)
            {
                return $"error,Failed to create category: {e.Message}";
            }
        }

        // Update category
        public string Update(Category category)
        {
            if(Any(category.PkId))
            {
                try
                {
                    _context.Categories.Update(category);
                    _context.SaveChanges();

                    return $"success,Successfully updated category ID: {category.PkId}";
                }
                catch(Exception ex)
                {
                    return $"error,Category could not be updated: {ex.Message}";
                }
            }
            else
            {
                return $"warning,Unable to find category ID: {category.PkId}";
            }
        }

        // Delete category
        public string Delete(int id)
        {
            Category? category = GetById(id);
            if(category == null)
            {
                return $"warning,Unable to find category ID: {id}";
            }

            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return $"success,Successfully deleted category ID: {id}";
            }
            catch(Exception e)
            {
                return $"error,Failed to delete category: {e.Message}";
            }
        }

        // If category exists
        public bool Any(int id)
        {
            return _context.Categories.Any(c => c.PkId == id);
        }

        // return a list of all available Categories as a SelectListItem
        public List<SelectListItem> GetSelectListItems()
        {
            List<SelectListItem>? categoryItems = GetAll().Select(c => new SelectListItem
            {
                Value = c.PkId.ToString(),
                Text = c.Name
            }).ToList();

            return categoryItems;
        }
    }
}
