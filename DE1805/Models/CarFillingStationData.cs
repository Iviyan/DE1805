using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DE1805.Models;

[Table("car_filling_stations_data")]
public class CarFillingStationData
{
    [Column("id"), JsonIgnore] public int Id { get; set; }
    [Column("car_filling_station_id"), JsonIgnore] public int CarFillingStationId { get; set; }
    [Column("name"), JsonPropertyName("Name")] public string Name { get; set; } = null!;
    [Column("price"), JsonPropertyName("Price")] public decimal Price { get; set; }
    [Column("amount_of_fuel"), JsonPropertyName("AmountOfFuel")] public int AmountOfFuel { get; set; }

    [ForeignKey(nameof(CarFillingStationId))]
    public CarFillingStation CarFillingStation { get; set; } = null!;
}