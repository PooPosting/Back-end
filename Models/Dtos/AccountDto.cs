using System;
using System.Collections.Generic;
using PicturesAPI.Entities;

namespace PicturesAPI.Models.Dtos;

public class AccountDto
{
    public Guid Id { get; set; }
        
    public string Nickname { get; set; }
    public string Email { get; set; }

    public virtual List<PictureDto> Pictures { get; set; }
    public DateTime AccountCreated { get; set; }
}