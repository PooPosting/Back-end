using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace PooPosting.Api.Models.Dtos.Picture;

public class CreatePictureDto
{
    [MaxLength(40)]
    public string Name { get; set; }
    
    [MaxLength(500)]
    [AllowNull] 
    public string Description { get; set; }
    
    [MaxLength(4)]
    [AllowNull] 
    public string[] Tags { get; set; }

    [MaxLength(4194304)]
    public string FileBase64 { get; set; }
}