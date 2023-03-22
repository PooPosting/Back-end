import {AccountModel} from "./AccountModel";
import {PictureModel} from "./PictureModel";

export interface PopularModel {
  mostPostedAccounts: AccountModel[],
  mostLikedAccounts: AccountModel[],

  mostLikedPictures: PictureModel[],
  mostCommentedPictures: PictureModel[],
  mostVotedPictures: PictureModel[]
}
