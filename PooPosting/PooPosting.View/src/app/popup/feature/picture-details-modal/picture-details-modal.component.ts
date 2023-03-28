import {Component,OnInit} from '@angular/core';
import {HttpServiceService} from "../../../shared/data-access/http-service.service";
import {MessageService} from "primeng/api";
import {BlockSpaceOnStartEnd} from "../../../shared/utils/regexes/blockSpaceOnStartEnd";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Clipboard} from "@angular/cdk/clipboard";
import {SelectOption} from "../../../shared/utils/models/selectOption";
import {PictureDetailsServiceService} from "../../../shared/state/picture-details-service.service";
import {AppCacheService} from "../../../shared/state/app-cache.service";
import {environment} from "../../../../environments/environment";
import {PictureDto} from "../../../shared/utils/dtos/PictureDto";
import {CommentDto} from "../../../shared/utils/dtos/CommentDto";
import {map, Observable, Subscription} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {LocationServiceService} from "../../../shared/helpers/location-service.service";
import {PictureLikesService} from "../../../shared/data-access/picture/picture-likes.service";

@Component({
  selector: 'app-picture-details-modal',
  templateUrl: './picture-details-modal.component.html',
  styleUrls: ['./picture-details-modal.component.scss']
})
export class PictureDetailsModalComponent implements OnInit {

  id: Observable<string>;
  picture: PictureDto | null = null;
  comments: CommentDto[] = [];
  visible: boolean = true;
  isLoggedOn: boolean = false;
  recentlyRefreshed: boolean = false;

  shareUrl: string = `${environment.appWebUrl}/picture/`;
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
  changeTags: FormGroup = new FormGroup({
    tags: new FormControl<string>("", [
      Validators.required
    ])
  });
  changeName: FormGroup = new FormGroup({
    name: new FormControl<string>("", [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(40),
      Validators.pattern(BlockSpaceOnStartEnd)
    ])
  });
  changeDesc: FormGroup = new FormGroup({
    desc: new FormControl<string>("", [
      Validators.required,
      Validators.maxLength(500),
    ])
  })

  constructor(
    private httpService: HttpServiceService,
    private likeService: PictureLikesService,
    private messageService: MessageService,
    private pictureDetailsService: PictureDetailsServiceService,
    private clipboard: Clipboard,
    private cacheService: AppCacheService,
    private route: ActivatedRoute,
    private locationService: LocationServiceService
  ) {
    this.id = route.params.pipe(map(p => p['id']));
  }

  ngOnInit(): void {
    this.isLoggedOn = this.cacheService.cachedUserInfo != undefined;
    this.refreshPicture();
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

  like() {
    this.likeService.likePicture(this.picture!.id)
      .subscribe(this.likeObserver)
  }
  dislike() {
    this.likeService.dislikePicture(this.picture!.id)
      .subscribe(this.likeObserver)
  }

  likeObserver = {
    next: (v: PictureDto) => {
      this.picture = v;
    },
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
        this.awaitSubmit = false;
      },
      error: () => {
        this.awaitSubmit = false;
      }
    });
  }

  refreshPicture() {
    this.recentlyRefreshed = true;
    this.fetchPicture();
    setTimeout(() => {
      this.recentlyRefreshed = false;
    }, 10000)
  }

  fetchPicture() {
    let sub: Subscription = this.id.subscribe({
      next: (id) => {
        this.shareUrl = `${environment.appWebUrl}/picture/${id}`;
        let sub: Subscription = this.httpService.getPictureRequest(id)
          .subscribe({
            next: (pic) => {
              this.picture = pic;
            },
            error: () => this.locationService.goError404(),
            complete: () => sub.unsubscribe()
          });
      },
      error: () => this.locationService.goError404(),
      complete: () => sub.unsubscribe()
    });
  }

  private resetForms() {
    this.changeTags.reset();
    this.changeName.reset();
    this.changeDesc.reset();
    this.deletePhrase = "";
  }

}
