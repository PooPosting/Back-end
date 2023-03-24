import {PictureDto} from "./PictureDto";

export interface PictureDtoPaged {
  items: PictureDto[];
  totalPages: number;
  pageSize: number;
  page: number;
}
