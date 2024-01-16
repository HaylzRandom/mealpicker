using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

class Program
{
    class Meal
    {
        public string? Name { get; set; }
        public string? CookingMethod { get; set; }
        public string? Location { get; set; }

        public string? MainIngredient { get; set; } // New property for main ingredient
    }

    static void Main()
    {
        // Load meals from a JSON file
        List<Meal> meals = LoadMealsFromJson("meals.json");

        if (meals == null)
        {
            Console.WriteLine("Error loading meals from the JSON file.");
            return;
        }

        // Get user input for the number of meals and preferred cooking method
        Console.Write("Enter the number of meals you'd like: ");
        int numberOfMeals = int.Parse(Console.ReadLine());

        Console.Write("Enter the main method of cooking (Hob, Air Fryer, Oven, Slow Cooker) or press Enter to skip: ");
        string? preferredCookingMethod = Console.ReadLine();

        Console.Write("Enter the main ingredient (optional, press Enter to skip): ");
        string? mainIngredient = Console.ReadLine();

        // Filter meals based on user input
        List<Meal> filteredMeals = meals
            .Where(meal => (string.IsNullOrEmpty(preferredCookingMethod) || meal.CookingMethod == preferredCookingMethod)
                && (string.IsNullOrEmpty(mainIngredient) || meal.MainIngredient == mainIngredient))
            .ToList();

        // Check if there are enough meals based on user input
        if (filteredMeals.Count < numberOfMeals)
        {
            Console.WriteLine($"Sorry, not enough meals available for the specified criteria.");
            return;
        }

        // Randomly select unique meals
        Random random = new Random();
        List<Meal> selectedMeals = new List<Meal>();
        for (int i = 0; i < numberOfMeals; i++)
        {
            int randomIndex = random.Next(filteredMeals.Count);
            Meal selectedMeal = filteredMeals[randomIndex];
            selectedMeals.Add(selectedMeal);
            filteredMeals.RemoveAt(randomIndex); // Ensure uniqueness
        }

        // Write the selected meals to a text file
        WriteSelectedMealsToFile(selectedMeals, "selected_meals.txt");
    }

    static List<Meal> LoadMealsFromJson(string filePath)
    {
        try
        {
            // Read JSON from file
            string json = File.ReadAllText(filePath);

            // Deserialize JSON into a list of meals
            List<Meal>? meals = JsonConvert.DeserializeObject<List<Meal>>(json);

            return meals;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading meals from JSON: {ex.Message}");
            return null;
        }
    }

    static void WriteSelectedMealsToFile(List<Meal> selectedMeals, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Selected meals:");

                foreach (var meal in selectedMeals)
                {
                    writer.WriteLine($"{meal.Name} - Cooking Method: {meal.CookingMethod} - Location: {meal.Location}");
                }
            }

            Console.WriteLine($"Selected meals have been written to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing selected meals to file: {ex.Message}");
        }
    }
}
