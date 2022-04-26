﻿using System.ComponentModel.DataAnnotations;

namespace Shoping.Data.Entities
{
    public class State
    {
        public int Id { get; set; }
        [Display(Name = "Departamento/Estado")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        public Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
        [Display(Name = "Ciudades")]
        public int CityNumber => Cities == null ? 0 : Cities.Count;
    }
}
