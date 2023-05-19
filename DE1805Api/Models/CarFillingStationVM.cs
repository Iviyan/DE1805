namespace DE1805Api.Models;

public class CarFillingStationVM
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;

    public List<CarFillingStationDataVM> Data { get; set; } = new();

    public CarFillingStation ToOrigin() => new() { Id =  Id, Address = Address, Data = Data.Select(d => d.ToOrigin()).ToList() };
}

public class CarFillingStationDataVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int AmountOfFuel { get; set; }

    public CarFillingStationData ToOrigin() => new() { Id = Id, Name = Name, Price = Price, AmountOfFuel = AmountOfFuel };
}