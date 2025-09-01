using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Models;

[Index(nameof(Name), IsUnique = true)]
public class Genre
{
    public Guid Id { get; set; }

    [Required] [StringLength(20)] public required string Name { get; set; }
}