﻿using PooPosting.Application.Models.Dtos.Account;

namespace PooPosting.Api.Models.Dtos.Like;

public class LikeDto
{
    public int Id { get; set; }
    public string PictureId { get; set; }
    public AccountDto Account { get; set; }
}