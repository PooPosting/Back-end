import {AccountDto} from "./AccountDto";
import {PictureDto} from "./PictureDto";

export interface PopularDto {
  mostPostedAccounts: AccountDto[],
  mostLikedAccounts: AccountDto[],

  mostLikedPictures: PictureDto[],
  mostCommentedPictures: PictureDto[],
  mostVotedPictures: PictureDto[]
}
