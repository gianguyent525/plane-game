using System.ComponentModel.DataAnnotations;

namespace PlaneGameApi.Models;

public class SubmitScoreRequestDto
{
    [Range(0, int.MaxValue)]
    public int Score { get; set; }
}
