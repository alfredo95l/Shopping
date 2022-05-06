using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shoping.Data;
using Shoping.Data.Entities;

namespace Shoping.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync()
        {
            List<SelectListItem> list = await _context.Categorias.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
                .OrderBy(x => x.Text)
                .ToListAsync();
            list.Insert(0, new SelectListItem { Text = "[Seleccione una categoria...]", Value = "0" });
            
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync(IEnumerable<Category> filter)
        {
            List<Category> categories = await _context.Categorias.ToListAsync();
            List<Category> categoriesFiltered = new();
            foreach (Category category in categories)
            {
                if (!filter.Any(c => c.Id == category.Id))
                {
                    categoriesFiltered.Add(category);
                }
            }
            List<SelectListItem> list = categoriesFiltered.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
               .OrderBy(x => x.Text)
               .ToList();
            list.Insert(0, new SelectListItem { Text = "[Seleccione un pais...]", Value = "0" });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            List<SelectListItem> list = await _context.Cities
               .Where(s => s.State.Id == stateId)
               .Select(c => new SelectListItem
               {
                   Text = c.Name,
                   Value = c.Id.ToString()
               })
              .OrderBy(x => x.Text)
              .ToListAsync();
            list.Insert(0, new SelectListItem { Text = "[Seleccione una ciudad...]", Value = "0" });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
        {
            List<SelectListItem> list = await _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
               .OrderBy(x => x.Text)
               .ToListAsync();
            list.Insert(0, new SelectListItem { Text = "[Seleccione una pais...]", Value = "0" });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId)
        {
            List<SelectListItem> list = await _context.States
                .Where(s => s.Country.Id == countryId)
                .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
               .OrderBy(x => x.Text)
               .ToListAsync();
            list.Insert(0, new SelectListItem { Text = "[Seleccione una departamento/estado...]", Value = "0" });

            return list;
        }
    }
}
