using System.ComponentModel.DataAnnotations.Schema;

namespace DE1805Api.Models;

[Table("car_filling_stations")]
public class CarFillingStation
{
    [Column("id")] public int Id { get; set; }
    [Column("address")] public string Address { get; set; } = null!;

    public List<CarFillingStationData> Data { get; set; } = new();
}
