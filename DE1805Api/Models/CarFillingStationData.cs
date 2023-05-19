using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DE1805Api.Models;

[Table("car_filling_stations_data")]
public class CarFillingStationData
{
    [Column("id")] public int Id { get; set; }
    [Column("car_filling_station_id"), JsonIgnore] public int CarFillingStationId { get; set; }
    [Column("name")] public string Name { get; set; } = null!;
    [Column("price")] public decimal Price { get; set; }
    [Column("amount_of_fuel")] public int AmountOfFuel { get; set; }

    [JsonIgnore]
    public CarFillingStation CarFillingStation { get; set; } = null!;
}