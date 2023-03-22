import {LikeDto} from "./LikeDto";
import {CommentDto} from "./CommentDto";
import {AccountPreviewDto} from "./AccountPreviewDto";

export interface PictureDto {
  id: string;
  accountPreview: AccountPreviewDto,

  name: string;
  description: string;
  tags: string[];
  url: string;
  pictureAdded: string;

  likes: LikeDto[];
  comments: CommentDto[];

  likeCount: number;
  dislikeCount: number;

  likeState: number;
  isModifiable: boolean;
  isAdminModifiable: boolean;

  isAdv?: boolean;
}
