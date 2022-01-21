using System.Collections.Generic;
using PicturesAPI.Entities;

namespace PicturesAPI.Repos.Interfaces;

public interface ILikeRepo
{
    List<Like> GetLikesByLiker(Account liker);
    List<Like> GetLikesByLiked(Picture picture);
    int RemoveLikes(List<Like> likes);
    public Like GetLikeByLikerAndLiked(Account account, Picture picture);
    bool AddLike(Like like);
    bool RemoveLike(Like like);
    bool ChangeLike(Like like);

}