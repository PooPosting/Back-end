import {LikeModel} from "../Get/LikeModel";
import {LikeState} from "../../../Enums/LikeState";

export interface LikeResult {
  likeCount: number,
  dislikeCount: number,
  likes: LikeModel[],
  likeState: LikeState
}
