import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CommentDto} from "../../../shared/utils/dtos/CommentDto";

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent {
  @Input() comment!: CommentDto;
  @Output() commentDeleted: EventEmitter<void> = new EventEmitter<void>();

  onCommentDelete() {
    this.commentDeleted.emit();
  }

}
