using FYPIBDPatientApp.Models;

namespace FYPIBDPatientApp.Dtos
{
    public class DietaryLogDto
    {
        public DateTime Date { get; set; }

        public string FoodItem { get; set; }

        public int Healthiness { get; set; }

        public string Notes { get; set; }
    }
}
