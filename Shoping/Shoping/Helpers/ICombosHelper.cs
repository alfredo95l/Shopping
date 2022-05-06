using Microsoft.AspNetCore.Mvc.Rendering;
using Shoping.Data.Entities;

namespace Shoping.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync();
        Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync(IEnumerable<Category> filter);
        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();
        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId);
        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);
    }
}
