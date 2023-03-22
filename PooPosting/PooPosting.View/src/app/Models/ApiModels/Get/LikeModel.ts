import {AccountPreviewModel} from "./AccountPreviewModel";

export interface LikeModel {
  id: number;
  pictureId: string;
  isLike: boolean;
  accountPreview: AccountPreviewModel
}
