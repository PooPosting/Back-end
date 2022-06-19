﻿namespace PicturesAPI.Models.Dtos;

public class PictureDto
{
    public string Id { get; set; }
    public string AccountId { get; set; }
    public string AccountNickname { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Tags { get; set; }

    public string Url { get; set; }
    public DateTime PictureAdded { get; set; }

    public ICollection<LikeDto> Likes { get; set; }
    public ICollection<CommentDto> Comments { get; set; }
}