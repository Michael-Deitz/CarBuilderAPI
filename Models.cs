namespace CarBuilder.Models;

public class PaintColor
{
    public int Id { get; set; }
    public decimal Price {get; set; }
    public string Color { get; set; }
}

public class Interior
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Material { get; set; }
}

public class Technology
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Package { get; set; }
}

public class Wheels
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Style { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int WheelId { get; set; }
    public int TechnologyId { get; set; }
    public int PaintId { get; set; }
    public int InteriorId { get; set; }
    public Wheels Wheel { get; set; }
    public Technology Technology { get; set; }
    public Interior Interior { get; set; }
    public PaintColor PaintColor { get; set; }
    public decimal TotalCost => PaintColor.Price + Interior.Price + Technology.Price + Wheel.Price;
}

