using System;

// Blueprint Method Pattern - Simple Example
// Making different types of beverages with the same steps

public abstract class BeverageBlueprint 
{
    // TEMPLATE METHOD - This is the blueprint/recipe
    // It defines the steps that EVERY beverage must follow
    public void MakeBeverage() 
    {
        Console.WriteLine("🍵 Starting beverage preparation...\n");
        
        // Step 1: Always boil water first
        BoilWater();
        
        // Step 2: Add the main ingredient (different for each beverage)
        AddMainIngredient();  // ← Abstract method
        
        // Step 3: Pour into cup
        PourInCup();
        
        // Step 4: Add extras only if customer wants them
        if (CustomerWantsExtras()) 
        {
            AddExtras();  // ← Abstract method
        }
        
        Console.WriteLine("✅ Beverage ready to serve!\n");
    }

    // Concrete methods - same for all beverages
    private void BoilWater() 
    {
        Console.WriteLine("💧 Boiling water...");
    }
    
    private void PourInCup() 
    {
        Console.WriteLine("☕ Pouring into cup...");
    }

    // Abstract methods - each beverage implements differently
    protected abstract void AddMainIngredient();
    protected abstract void AddExtras();
    
    // Hook method - can be overridden if needed
    protected virtual bool CustomerWantsExtras() 
    {
        return true;  // Default: customer wants extras
    }
}

// Concrete implementation 1: Tea
public class Tea : BeverageBlueprint 
{
    protected override void AddMainIngredient() 
    {
        Console.WriteLine("🍃 Steeping tea bag...");
    }
    
    protected override void AddExtras() 
    {
        Console.WriteLine("🍋 Adding lemon and sugar...");
    }
}

// Concrete implementation 2: Coffee
public class Coffee : BeverageBlueprint 
{
    protected override void AddMainIngredient() 
    {
        Console.WriteLine("☕ Brewing coffee grounds...");
    }
    
    protected override void AddExtras() 
    {
        Console.WriteLine("🥛 Adding milk and sugar...");
    }
}

// Concrete implementation 3: Hot Chocolate
public class HotChocolate : BeverageBlueprint 
{
    protected override void AddMainIngredient() 
    {
        Console.WriteLine("🍫 Mixing chocolate powder...");
    }
    
    protected override void AddExtras() 
    {
        Console.WriteLine("🍦 Adding whipped cream and marshmallows...");
    }
    
    // Override hook method - hot chocolate always gets extras!
    protected override bool CustomerWantsExtras() 
    {
        return true;  // Hot chocolate always gets extras
    }
}

// Concrete implementation 4: Green Tea (minimalist)
public class GreenTea : BeverageBlueprint 
{
    protected override void AddMainIngredient() 
    {
        Console.WriteLine("🌱 Steeping green tea leaves...");
    }
    
    protected override void AddExtras() 
    {
        Console.WriteLine("🍯 Adding a touch of honey...");
    }
    
    // Override hook method - green tea purists don't want extras
    protected override bool CustomerWantsExtras() 
    {
        return false;  // Keep it pure and simple
    }
}

class Program 
{
    public static void Main() 
    {
        Console.WriteLine("☕ COFFEE SHOP - BLUEPRINT METHOD DEMO ☕");
        Console.WriteLine("=" + new string('=', 45) + "\n");

        // Create different beverages
        BeverageBlueprint[] beverages = {
            new Tea(),
            new Coffee(), 
            new HotChocolate(),
            new GreenTea()
        };

        // Each beverage follows the SAME blueprint/recipe
        // But implements the steps differently
        foreach (var beverage in beverages) 
        {
            Console.WriteLine($"Making {beverage.GetType().Name}:");
            Console.WriteLine("-" + new string('-', 30));
            
            beverage.MakeBeverage();  // ← This calls the blueprint method
        }
    }
}
