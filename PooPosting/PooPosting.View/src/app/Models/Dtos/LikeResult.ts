import {LikeDto} from "./LikeDto";
import {LikeState} from "../../Enums/LikeState";

export interface LikeResult {
  likeCount: number,
  dislikeCount: number,
  likes: LikeDto[],
  likeState: LikeState
}
