using lib;

namespace socketAPIFirst.Dtos;

public class ServerReturnsListOfLanguageNames: BaseDto
{
    public List<string> names { get; set; }
}