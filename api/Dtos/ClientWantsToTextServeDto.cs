using System.ComponentModel.DataAnnotations;
using lib;

namespace socketAPIFirst.Dtos;

public class ClientWantsToTextServeDto : BaseDto
{
    [MinLength(1)]
    public string message { get; set; }
    public bool isUser { get; set; }
}