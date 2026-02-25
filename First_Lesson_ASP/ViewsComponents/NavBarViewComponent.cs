using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace First_Lesson_ASP.ViewsComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly RestDBContext _context;

        public NavBarViewComponent(RestDBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allNavigations = await _context.Navigations
                .OrderBy(n => n.Order)
                .ToListAsync();

            var rootItems = allNavigations
                .Where(n => n.ParentId == null)
                .OrderBy(n => n.Order)
                .ToList();

            foreach (var parent in rootItems)
            {
                BuildHierarchy(parent, allNavigations);
            }

            return View("NavBar", rootItems);
        }

        private void BuildHierarchy(Navigate parent, List<Navigate> allItems)
        {
            var children = allItems
                .Where(n => n.ParentId == parent.Id)
                .OrderBy(n => n.Order)
                .ToList();

            parent.Childs = children;

            foreach (var child in children)
            {
                BuildHierarchy(child, allItems);
            }
        }
    }
}