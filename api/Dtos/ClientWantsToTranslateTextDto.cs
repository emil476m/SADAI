using lib;

namespace socketAPIFirst.Dtos;

public class ClientWantsToTranslateTextDto : BaseDto
{
    public string text { get; set; }
    public string toLan { get; set; }
    public string fromLan { get; set; }
}