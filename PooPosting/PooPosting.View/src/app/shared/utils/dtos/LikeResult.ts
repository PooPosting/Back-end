import {LikeDto} from "./LikeDto";
import {LikeState} from "../enums/likeState";

export interface LikeResult {
  likeCount: number,
  dislikeCount: number,
  likes: LikeDto[],
  likeState: LikeState
}
