using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DE1805.Models;

[Table("car_filling_stations")]
public class CarFillingStation
{
    [Column("id"), JsonPropertyName("Station_ID")] public int Id { get; set; }
    [Column("address"), JsonPropertyName("Address")] public string Address { get; set; } = null!;

    [JsonPropertyName("data")]
    public List<CarFillingStationData> Data { get; set; } = new();
}
