import {PictureModel} from "./PictureModel";

export interface PicturePagedResult {
  items: PictureModel[];
  totalPages: number;
  pageSize: number;
  page: number;
}
