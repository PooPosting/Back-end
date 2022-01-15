using System;
using System.Collections.Generic;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;

namespace PicturesAPI.Interfaces;

public interface IPictureService
{
    PagedResult<PictureDto> GetAll(PictureQuery query);
    IEnumerable<PictureDto> GetAllOdata();
    PictureDto GetById(Guid id);
    Guid Create(CreatePictureDto dto);
    void Put(Guid id, PutPictureDto dto);
    void Delete(Guid id);
}