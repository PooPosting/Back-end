import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {HttpServiceService} from "../../../../Services/http/http-service.service";
import {MessageService} from "primeng/api";
import {ItemName} from "../../../../Regexes/ItemName";
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import {Clipboard} from "@angular/cdk/clipboard";
import {SelectOption} from "../../../../Models/QueryModels/SelectOption";
import {PictureDetailsServiceService} from "../../../../Services/data/picture-details-service.service";
import {CacheServiceService} from "../../../../Services/data/cache-service.service";
import {environment} from "../../../../../environments/environment";
import {PictureDto} from "../../../../Models/Dtos/PictureDto";
import {CommentDto} from "../../../../Models/Dtos/CommentDto";

@Component({
  selector: 'app-picture-details-modal',
  templateUrl: './picture-details-modal.component.html',
  styleUrls: ['./picture-details-modal.component.scss']
})
export class PictureDetailsModalComponent implements OnInit {
  @Input() picture: PictureDto | null = null;
  @Input() isLoggedOn!: boolean;

  @Output() pictureChanged: EventEmitter<PictureDto> = new EventEmitter<PictureDto>();
  @Output() modalClose: EventEmitter<void> = new EventEmitter<void>()

  shareUrl!: string;
  showSettings: boolean = false;
  deletePhrase: string = "";

  selectValue: SelectOption = {name: "none", class: "none"};
  editValue: SelectOption = {name: "none", class: "none"};
  editOptions: SelectOption[] = [
    { name: "Tagi", class: "bi bi-tag"},
    { name: "Nazwa", class: "bi bi-balloon"},
    { name: "Opis", class: "bi bi-file-earmark-text"},
  ]
  selectOptions: SelectOption[] = [
    { name: "Edytuj", class: "bi bi-pen"},
    { name: "Usuń", class: "bi bi-trash3"},
  ]

  awaitSubmit: boolean = false;

  tags: string[] = [];
  changeTags: UntypedFormGroup = new UntypedFormGroup({
    tags: new UntypedFormControl("", [
      Validators.required
    ])
  });
  changeName: UntypedFormGroup = new UntypedFormGroup({
    name: new UntypedFormControl("", [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(40),
      Validators.pattern(ItemName)
    ])
  });
  changeDesc: UntypedFormGroup = new UntypedFormGroup({
    desc: new UntypedFormControl("", [
      Validators.required,
      Validators.maxLength(500),
    ])
  })
  commentForm: UntypedFormGroup = new UntypedFormGroup({
    text: new UntypedFormControl("", [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(250),
      Validators.pattern(ItemName)
    ]),
  });

  constructor(
    private httpService: HttpServiceService,
    private messageService: MessageService,
    private pictureDetailsService: PictureDetailsServiceService,
    private clipboard: Clipboard,
    private cacheService: CacheServiceService,
  ) {

  }

  ngOnInit(): void {
    this.pictureDetailsService.modalTriggerSubject.subscribe({
      next: () => {
        this.showSettings = false;
        this.selectValue = {name: "none", class: "none"};
        this.editValue = {name: "none", class: "none"};
        this.resetForms();
      }
    });
    this.pictureDetailsService.showModalSubject.subscribe({
      next: (val) => {
        this.shareUrl = `${environment.appWebUrl}/picture/${val!.id}`;
        this.isLoggedOn = this.cacheService.getUserLoggedOnState();
        this.picture = val;
      }
    });
  }


  copyUrl(textToCopy: string) {
    this.messageService.clear();
    this.clipboard.copy(textToCopy);
    this.messageService.add({
      severity: 'success',
      summary: 'Sukces!',
      detail: 'Pomyślnie skopiowano adres obrazka!',
    })
  }
  comment() {
    this.awaitSubmit = true;
    this.httpService.postCommentRequest(this.picture!.id, this.commentForm.getRawValue())
      .subscribe(this.commentObserver);
  }
  deleteComment(commId: string) {
    this.httpService.deleteCommentRequest(this.picture!.id, commId)
      .subscribe({
        next: () => {
          this.picture!.comments = this.picture!.comments.filter(c => c.id != commId);
          this.messageService.add({
            severity:'warn',
            summary:'Sukces',
            detail:'Pomyślnie usunięto komentarz!'
          });
        }
      });
  }

  trimChips() {
    let tags: string[] = this.changeTags.get('tags')?.value;
    let tagsToTrim: string[] = [];
    let tagsTrimmed: string[] = [];
    let uniqueTagsTrimmed: string[] = [];
    tags.forEach(val => {
      tagsToTrim = val.split(" ")
      tagsToTrim.forEach(tag => {
        tagsTrimmed.push(tag)
        tagsTrimmed.forEach((c) => {
          if (!uniqueTagsTrimmed.includes(c)) {
            uniqueTagsTrimmed.push(c);
          }
          if (uniqueTagsTrimmed.length > 4){
            uniqueTagsTrimmed.pop();
          }
        });
      });
    });
    this.changeTags.get('tags')?.setValue(uniqueTagsTrimmed);
    this.tags = uniqueTagsTrimmed;
  }
  popChips() {
    this.tags.pop();
  }
  onKeyDown(event: any) {
    if (event.key === " ") {
      event.preventDefault();
      const element = event.target as HTMLElement;
      element.blur();
      element.focus();
    }
  }

  submitDelete() {
    this.awaitSubmit = true;
    this.deletePicture();
    this.messageService.clear();
  }

  submitTags(){
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append("tags", this.tags.join(' '));
    this.messageService.clear();
    this.postChanges(formData);
  }
  submitName(){
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append("name", this.changeName.get('name')?.value)
    this.messageService.clear();
    this.postChanges(formData);
  }
  submitDesc(){
    this.awaitSubmit = true;
    let formData = new FormData();
    formData.append("description", this.changeDesc.get('desc')?.value);
    this.messageService.clear();
    this.postChanges(formData);
  }

  like(){
    this.httpService.patchPictureLikeRequest(this.picture!.id)
      .subscribe(this.likeObserver)
  }
  dislike(){
    this.httpService.patchPictureDislikeRequest(this.picture!.id)
      .subscribe(this.likeObserver)
  }

  closeModal(event: any) {
    if (!event.ctrlKey) {
      this.modalClose.emit();
    }
  }

  likeObserver = {
    next: (v: PictureDto) => {
      this.picture = v;
      this.pictureChanged.emit(v);
    },
  }
  commentObserver = {
    next: (v: CommentDto) => {
      this.picture!.comments.unshift(v);
      this.commentForm.reset();
      this.awaitSubmit = false;
      this.messageService.add({
        severity:'success',
        summary:'Sukces',
        detail:'Pomyślnie skomentowano!'
      });
      this.commentForm.reset();
    }
  }

  private postChanges(model: FormData) {
    this.httpService.updatePictureRequest(model, this.picture!.id).subscribe({
      next: (val) => {
        this.picture!.name = val.name;
        if (this.tags) this.picture!.tags = val.tags;
        this.picture!.description = val.description;
        this.messageService.add({severity:'success', summary: 'Sukces', detail: 'Zmiany zostały wprowadzone'});
        this.resetForms();
        this.awaitSubmit = false;
        this.pictureDetailsService.pictureChangedSubject.next(val);
      },
      error: () => {
        this.awaitSubmit = false;
      }
    });
  }
  deletePicture() {
    this.httpService.deletePictureRequest(this.picture!.id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'warn',
          summary: 'Sukces',
          detail: 'Post został usunięty'
        });
        this.pictureDetailsService.pictureDeletedSubject.next(this.picture!.id);
        this.modalClose.emit();
        this.awaitSubmit = false;
      },
      error: () => {
        this.awaitSubmit = false;
      }
    });
  }

  private resetForms() {
    this.changeTags.reset();
    this.changeName.reset();
    this.changeDesc.reset();
    this.deletePhrase = "";
  }

}
