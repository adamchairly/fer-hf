namespace FerHf.API.DTOs;

public class PhotoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}
