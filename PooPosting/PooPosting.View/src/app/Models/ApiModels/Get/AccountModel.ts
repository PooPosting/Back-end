import {PicturePreviewModel} from "./PicturePreviewModel";

export interface AccountModel {
  id: string;
  roleId: string;
  nickname: string;
  email: string;
  profilePicUrl: string;
  backgroundPicUrl: string;
  accountDescription: string;
  picturePreviews: PicturePreviewModel[];
  accountCreated: string;

  isModifiable: boolean;
  isAdminModifiable: boolean;
}
