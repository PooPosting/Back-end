import {AccountPreviewDto} from "./AccountPreviewDto";

export interface LikeDto {
  id: number;
  pictureId: string;
  isLike: boolean;
  accountPreview: AccountPreviewDto
}
