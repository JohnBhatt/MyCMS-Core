using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Core.Interfaces;
using MyCMS.Data;

namespace MyCMS.Services
{
    public class MenuService : IMenuService
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public MenuService(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
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
            
            await _auditService.LogAsync("Menus", menu.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { menu.MenuName, menu.Position }), "Menu created");
            return menu;
        }

        public async Task<Menu> UpdateMenuAsync(Menu menu)
        {
            var existing = await _context.Menus.FindAsync(menu.Id);
            if (existing == null) throw new InvalidOperationException("Menu not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.MenuName, existing.Position });
            
            menu.ModifiedOn = DateTime.UtcNow;
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { menu.MenuName, menu.Position });
            await _auditService.LogAsync("Menus", menu.Id.ToString(), "Updated", oldValues, newValues, "Menu updated");
            return menu;
        }

        public async Task DeleteMenuAsync(Guid id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                var oldValues = JsonSerializer.Serialize(new { menu.MenuName, menu.Position });
                menu.IsDeleted = true;
                menu.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("Menus", id.ToString(), "Deleted", oldValues, null, "Menu deleted");
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
            
            await _auditService.LogAsync("MenuItems", menuItem.Id.ToString(), "Created", null,
                JsonSerializer.Serialize(new { menuItem.ItemName, menuItem.MenuURL, menuItem.MenuId }), "Menu item created");
            return menuItem;
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
        {
            var existing = await _context.MenuItems.FindAsync(menuItem.Id);
            if (existing == null) throw new InvalidOperationException("Menu item not found");
            
            var oldValues = JsonSerializer.Serialize(new { existing.ItemName, existing.MenuURL, existing.ItemPosition });
            
            menuItem.ModifiedOn = DateTime.UtcNow;
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
            
            var newValues = JsonSerializer.Serialize(new { menuItem.ItemName, menuItem.MenuURL, menuItem.ItemPosition });
            await _auditService.LogAsync("MenuItems", menuItem.Id.ToString(), "Updated", oldValues, newValues, "Menu item updated");
            return menuItem;
        }

        public async Task DeleteMenuItemAsync(Guid id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem != null)
            {
                var oldValues = JsonSerializer.Serialize(new { menuItem.ItemName, menuItem.MenuURL });
                menuItem.IsDeleted = true;
                menuItem.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                await _auditService.LogAsync("MenuItems", id.ToString(), "Deleted", oldValues, null, "Menu item deleted");
            }
        }
    }
}
