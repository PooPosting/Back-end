import {PictureDto} from "./PictureDto";

export interface PictureDtoPaged {
  items: PictureDto[];
  totalPages: number;
  totalItems: number;
  page: number;
}
