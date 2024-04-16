using Newtonsoft.Json;

namespace Services.Dtos;

public class AuthResponseDtoOutput
{
    public Jwt Authentication { get; set; }

    public UserOutputDto User { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }
}
