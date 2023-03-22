import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CommentModel} from "../../../../../Models/ApiModels/Get/CommentModel";

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent {
  @Input() comment!: CommentModel;
  @Output() commentDeleted: EventEmitter<void> = new EventEmitter<void>();

  onCommentDelete() {
    this.commentDeleted.emit();
  }

}
