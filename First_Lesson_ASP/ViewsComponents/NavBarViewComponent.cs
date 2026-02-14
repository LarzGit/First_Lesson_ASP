using Microsoft.AspNetCore.Mvc;
using First_Lesson_ASP.Entities;
using System.Collections.Generic;
using System.Linq;

namespace First_Lesson_ASP.ViewsComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var allNavigationItems = new List<Navigate>();

            // --- БАТЬКІВСЬКІ ЕЛЕМЕНТИ ---
            allNavigationItems.Add(new Navigate() { Id = 1, Title = "Home", Href = "/", Order = 1, ProductId = null });
            allNavigationItems.Add(new Navigate() { Id = 2, Title = "About", Href = "/About/About", Order = 2, ProductId = null });

            // Це батько для випадаючого списку (ID = 3)
            allNavigationItems.Add(new Navigate() { Id = 3, Title = "Menu", Href = "/Menu/Index", Order = 3, ProductId = null });

            allNavigationItems.Add(new Navigate() { Id = 4, Title = "Chefs", Href = "/Team/Index", Order = 4, ProductId = null });
            allNavigationItems.Add(new Navigate() { Id = 5, Title = "Contact Us", Href = "/About/ContactUs", Order = 5, ProductId = null });

            // --- ДІТИ (ПІДПУНКТИ) ---
            // ProductId = 3 означає, що вони будуть всередині "Menu"
            allNavigationItems.Add(new Navigate() { Id = 6, Title = "Hot Dishes", Href = "/Menu/Hot", Order = 1, ProductId = 3 });
            allNavigationItems.Add(new Navigate() { Id = 7, Title = "Cold Snacks", Href = "/Menu/Cold", Order = 2, ProductId = 3 });
            allNavigationItems.Add(new Navigate() { Id = 8, Title = "Drinks", Href = "/Menu/Drinks", Order = 3, ProductId = 3 });

            // Логіка побудови дерева
            var rootItems = allNavigationItems
                .Where(x => x.ProductId == null)
                .OrderBy(x => x.Order)
                .ToList();

            foreach (var parent in rootItems)
            {
                BuildHierarchy(parent, allNavigationItems);
            }

            return View("NavBar", rootItems);
        }

        private void BuildHierarchy(Navigate parent, List<Navigate> allItems)
        {
            var children = allItems
                .Where(x => x.ProductId == parent.Id)
                .OrderBy(x => x.Order)
                .ToList();

            parent.Childs = children;

            foreach (var child in children)
            {
                BuildHierarchy(child, allItems);
            }
        }
    }
}