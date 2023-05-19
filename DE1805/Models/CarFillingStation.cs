using System.Collections.Generic;

namespace DE1805.Models;

public class CarFillingStation
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;

    public List<CarFillingStationData> Data { get; set; } = new();
}
