import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {VoteType} from "../../utils/enums/voteType";
import {LikeState} from "../../utils/enums/likeState";

@Component({
  selector: 'app-vote-button',
  templateUrl: './vote-button.component.html',
  styleUrls: ['./vote-button.component.scss']
})
export class VoteButtonComponent  {

  @Output() voted: EventEmitter<VoteType> = new EventEmitter<VoteType>();
  @Input() voteType: VoteType = VoteType.LIKE;
  @Input() likeState: LikeState = LikeState.LIKED;
  @Input() isLoggedOn: boolean = false;
  @Input() likeCount: number = 0;
  @Input() dislikeCount: number = 0;

  vote() {
    this.voted.emit(this.voteType);
  }

}
