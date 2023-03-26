import {LikeDto} from "./LikeDto";

export interface LikeDtoPaged {
  items: LikeDto[];
  totalPages: number;
  pageSize: number;
  page: number;
}
