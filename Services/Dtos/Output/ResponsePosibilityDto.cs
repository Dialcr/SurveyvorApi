using DataAcces.Entities;

namespace Services.Dtos.Output;

public class ResponsePosibilityDto
{
    public int Id { get; set; }

    public string ResponseValue { get; set; } = "";
}

public static class ResponsePosibilityExtention
{
    public static ResponsePosibilityDto ToResponsePosibilityDto(
        this ResponsePosibility responsePosibility
    )
    {
        return new ResponsePosibilityDto()
        {
            Id = responsePosibility.Id,
            ResponseValue = responsePosibility.ResponseValue,
        };
    }
}
