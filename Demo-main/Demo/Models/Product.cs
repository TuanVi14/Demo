namespace Demo.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Image { get; set; } = string.Empty;
    // Category used by the views (e.g. "Flash Sale", "Trend Luxe", "Hot Trend")
    public string Category { get; set; } = string.Empty;
    // Number of reviews shown in the UI
    public int Reviews { get; set; }
}