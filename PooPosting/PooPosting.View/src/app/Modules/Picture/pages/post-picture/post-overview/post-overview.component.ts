import { Component, OnInit } from '@angular/core';
import {PostPictureServiceService} from "../../../../../Services/helpers/post-picture-service.service";
import {LocationServiceService} from "../../../../../Services/helpers/location-service.service";
import {CacheServiceService} from "../../../../../Services/data/cache-service.service";
import {HttpServiceService} from "../../../../../Services/http/http-service.service";
import {MessageService} from "primeng/api";
import {PictureDto} from "../../../../../Models/Dtos/PictureDto";

@Component({
  selector: 'app-post-overview',
  templateUrl: './post-overview.component.html',
  styleUrls: ['./post-overview.component.scss']
})
export class PostOverviewComponent implements OnInit {

  imgDataUrl: string = "";
  awaitSubmit = false;
  picturePreview?: PictureDto;

  constructor(
    private postPictureService: PostPictureServiceService,
    private locationService: LocationServiceService,
    private cacheService: CacheServiceService,
    private httpService: HttpServiceService,
    private messageService: MessageService,
  ) {
    this.picturePreview = {
      id: "LOCAL_PREVIEW",
      accountPreview: {
        id: this.cacheService.getCachedUserAccount().id,
        nickname: this.cacheService.getCachedUserAccount().nickname,
        profilePicUrl: this.cacheService.getCachedUserAccount().profilePicUrl
      },
      name: this.postPictureService.name ? this.postPictureService.name : '',
      description: this.postPictureService.description ? this.postPictureService.description : '',
      tags: this.postPictureService.tags ? this.postPictureService.tags : [],
      url: this.imgDataUrl,
      pictureAdded: Date.now().toString(),
      likes: [],
      comments: [],
      likeCount: Math.floor(Math.random() * 100) + 50,
      dislikeCount: Math.floor(Math.random() * 30),
      likeState: 0,
      isModifiable: false,
      isAdminModifiable: false
    }
  }

  ngOnInit() {
    const fileBlob = this.postPictureService.file;
    if (fileBlob) {
      const fileReader = new FileReader();
      fileReader.onloadend = () => {
        this.imgDataUrl = fileReader.result as string;
      }
      fileReader.readAsDataURL(fileBlob);
    }
  }

  post() {
    const formData: FormData = new FormData();
    formData.append('file', this.postPictureService.file!);
    formData.append('name', this.postPictureService.name!);

    if (this.postPictureService.description) {
      formData.append('description', this.postPictureService.description);
    }
    if (this.postPictureService.tags) {
      formData.append('tags', this.tagsToString(this.postPictureService.tags));
    }

    this.awaitSubmit = true;
    this.httpService.postPictureRequest(formData)
      .subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Sukces',
            detail: `"${this.postPictureService.name}" został zapostowany pomyślnie!`
          });
          this.awaitSubmit = false;
          this.locationService.goHomepage();
        },
        error: () => {
          this.awaitSubmit = false;
        }
      });
  }


  private tagsToString(tags: string[]): string{
    let result = ""
    tags.forEach(tag => result += (tag + " "));
    return result.slice(0, result.length - 1);
  }

  return() {
    this.locationService.goBack();
  }

}
