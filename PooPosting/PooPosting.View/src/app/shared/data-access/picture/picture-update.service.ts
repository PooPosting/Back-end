import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UpdatePictureNameDto} from "../../utils/dtos/UpdatePictureNameDto";
import {PictureDto} from "../../utils/dtos/PictureDto";
import {environment} from "../../../../environments/environment";
import {UpdatePictureDescriptionDto} from "../../utils/dtos/UpdatePictureDescriptionDto";
import {UpdatePictureTagsDto} from "../../utils/dtos/UpdatePictureTagsDto";

@Injectable({
  providedIn: 'root'
})
export class PictureUpdateService {

  constructor(
    private httpClient: HttpClient
  ) { }

  updatePictureName(data: UpdatePictureNameDto, id: string) {
    return this.httpClient
      .patch<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/update/name`,
        data,
        { responseType: "json" }
      );
  }

  updatePictureDescription(data: UpdatePictureDescriptionDto, id: string) {
    return this.httpClient
      .patch<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/update/description`,
        data,
        { responseType: "json" }
      );
  }

  updatePictureTags(data: UpdatePictureTagsDto, id: string) {
    return this.httpClient
      .patch<PictureDto>(
        `${environment.picturesApiUrl}/api/picture/${id}/update/tags`,
        data,
        { responseType: "json" }
      );
  }

}
