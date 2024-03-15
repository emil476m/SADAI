using System.ComponentModel.DataAnnotations;
using lib;

namespace socketAPIFirst.Dtos;

public class ServerRespondsToUser : BaseDto
{
    [MinLength(1)]
    public string message { get; set; }
    public bool isUser { get; set; }
}