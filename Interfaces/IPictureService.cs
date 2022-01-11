using System;
using System.Collections.Generic;
using System.Security.Claims;
using PicturesAPI.Models;

namespace PicturesAPI.Interfaces;

public interface IPictureService
{
    IEnumerable<PictureDto> GetAllPictures();
    PictureDto GetSinglePictureById(Guid id);
    Guid CreatePicture(CreatePictureDto dto);
    void PutPicture(Guid id, PutPictureDto dto);
    void DeletePicture(Guid id);
}