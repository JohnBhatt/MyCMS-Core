using MyCMS.Core.Entities;

namespace MyCMS.Core.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAllMenusAsync();
        Task<Menu?> GetMenuByIdAsync(Guid id);
        Task<Menu> CreateMenuAsync(Menu menu);
        Task<Menu> UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(Guid id);
        Task<List<MenuItem>> GetMenuItemsByMenuIdAsync(Guid menuId);
        Task<MenuItem?> GetMenuItemByIdAsync(Guid id);
        Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
        Task DeleteMenuItemAsync(Guid id);
    }
}
