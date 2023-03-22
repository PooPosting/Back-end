import {AccountPreviewModel} from "./AccountPreviewModel";

export interface CommentModel {
  id: string;
  pictureId: string,
  text: string,
  isModifiable: boolean,
  isAdminModifiable: boolean,
  commentAdded: string
  accountPreview: AccountPreviewModel,
}
