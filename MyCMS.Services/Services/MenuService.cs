using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class MenuService : IMenuService
    {
        private readonly AppDbContext _context;

        public MenuService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus.ToListAsync();
        }

        public async Task<Menu?> GetMenuByIdAsync(Guid id)
        {
            return await _context.Menus.FindAsync(id);
        }

        public async Task<Menu> CreateMenuAsync(Menu menu)
        {
            menu.Id = Guid.NewGuid();
            menu.CreatedOn = DateTime.UtcNow;
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task<Menu> UpdateMenuAsync(Menu menu)
        {
            menu.ModifiedOn = DateTime.UtcNow;
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task DeleteMenuAsync(Guid id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                menu.IsDeleted = true;
                menu.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MenuItem>> GetMenuItemsByMenuIdAsync(Guid menuId)
        {
            return await _context.MenuItems
                .Where(m => m.MenuId == menuId)
                .OrderBy(m => m.ItemPosition)
                .ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(Guid id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem)
        {
            menuItem.Id = Guid.NewGuid();
            menuItem.CreatedOn = DateTime.UtcNow;
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
        {
            menuItem.ModifiedOn = DateTime.UtcNow;
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task DeleteMenuItemAsync(Guid id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem != null)
            {
                menuItem.IsDeleted = true;
                menuItem.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
