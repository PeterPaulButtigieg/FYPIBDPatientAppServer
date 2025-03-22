using FYPIBDPatientApp.Models;

namespace FYPIBDPatientApp.Dtos
{
    public class DietaryLogDto
    {
        public DateTime Date { get; set; }

        public string FoodItem { get; set; }

        public string CookingMethod { get; set; }

        public string Notes { get; set; }
    }
}
