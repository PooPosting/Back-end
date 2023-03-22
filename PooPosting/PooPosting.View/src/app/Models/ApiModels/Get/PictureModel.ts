import {LikeModel} from "./LikeModel";
import {CommentModel} from "./CommentModel";
import {AccountPreviewModel} from "./AccountPreviewModel";

export interface PictureModel {
  id: string;
  accountPreview: AccountPreviewModel,

  name: string;
  description: string;
  tags: string[];
  url: string;
  pictureAdded: string;

  likes: LikeModel[];
  comments: CommentModel[];

  likeCount: number;
  dislikeCount: number;

  likeState: number;
  isModifiable: boolean;
  isAdminModifiable: boolean;

  isAdv?: boolean;
}
