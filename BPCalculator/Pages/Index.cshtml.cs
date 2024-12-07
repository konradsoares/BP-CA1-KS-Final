using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        [BindProperty]  // bound on POST
        public BloodPressure BP { get; set; }

        // setup initial data
        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };
        }

        // POST, validate
        public IActionResult OnPost()
        {
            // extra validation
            if (!(BP.Systolic > BP.Diastolic))
            {
                ModelState.AddModelError("", "Systolic must be greater than Diastolic");
            }

            // If the model is valid, calculate the category and return the page.
            return Page();
        }

        // Helper method to determine category color class
        public string GetCategoryColorClass()
        {
            switch (BP.Category)
            {
                case BPCategory.Low:
                    return "bg-blue";   // Blue for Low BP
                case BPCategory.Ideal:
                    return "bg-green";  // Green for Ideal BP
                case BPCategory.PreHigh:
                    return "bg-orange"; // Orange for Pre-High BP
                case BPCategory.High:
                    return "bg-red";    // Red for High BP
                default:
                    return "";
            }
        }
    }
}
