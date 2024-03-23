namespace PixHub.Dtos;

public class WebhookDTO(string origin, string destiny)
{
  public string Origin { get; } = origin;
  public string Destiny { get; } = destiny;
}