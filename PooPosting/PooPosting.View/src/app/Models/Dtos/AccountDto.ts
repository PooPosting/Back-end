import {PicturePreviewDto} from "./PicturePreviewDto";

export interface AccountDto {
  id: string;
  roleId: string;
  nickname: string;
  email: string;
  profilePicUrl: string;
  backgroundPicUrl: string;
  accountDescription: string;
  picturePreviews: PicturePreviewDto[];
  accountCreated: string;

  isModifiable: boolean;
  isAdminModifiable: boolean;
}
