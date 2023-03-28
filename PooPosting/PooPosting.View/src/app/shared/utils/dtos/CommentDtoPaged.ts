import {CommentDto} from "./CommentDto";

export interface CommentDtoPaged {
  items: CommentDto[]
  totalPages: number;
  totalItems: number;
  page: number;
}
