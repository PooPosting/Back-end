﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PicturesAPI.Entities;

namespace PicturesAPI.Models.Dtos;

public class CreatePictureDto
{

    [Required] [MinLength(4)] [MaxLength(25)]
    public string Name { get; set; }
        
    [MaxLength(400)]
    public string Description { get; set; }
        
    [MaxLength(20)]
    public List<string> Tags { get; set; }
        
    [Required] [MinLength(16)] [MaxLength(500)]
    public string Url { get; set; }
}