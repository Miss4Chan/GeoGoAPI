namespace GeoGoAPI._models.dtos.twin;

public class DigitalTwinProfileDto
{
    public int UserTwinId { get; set; }

    public List<RawWeightDto> RawWeights { get; set; } = new();
    public List<NormalizedWeightDto> NormalizedWeights { get; set; } = new();
}

public class RawWeightDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public double Score { get; set; }
}

public class NormalizedWeightDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public double NormalizedScore { get; set; }
}
