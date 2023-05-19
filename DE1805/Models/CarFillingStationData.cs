namespace DE1805.Models;

public class CarFillingStationData
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int AmountOfFuel { get; set; }
}