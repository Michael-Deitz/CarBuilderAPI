namespace CarBuilder.Models.DTOs;

public class PaintColorDTO
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; }
}

public class InteriorDTO
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Material { get; set; }
}

public class TechnologyDTO
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Package { get; set; }
}

public class WheelsDTO
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Style { get; set; }
}

public class OrderDTO
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int WheelId { get; set; }
    public int TechnologyId { get; set; }
    public int PaintId { get; set; }
    public int InteriorId { get; set; }
    public WheelsDTO Wheel { get; set; }
    public TechnologyDTO Technology { get; set; }
    public InteriorDTO Interior { get; set; }
    public PaintColorDTO PaintColor { get; set; }
     public decimal TotalCost
    {
        get
        {
            decimal total = 0;
            total += Wheel.Price + Technology.Price + PaintColor.Price + Interior.Price;
            return total;
        }
    }
    
}