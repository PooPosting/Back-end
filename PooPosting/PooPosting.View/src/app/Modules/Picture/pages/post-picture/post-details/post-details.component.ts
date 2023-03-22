import {Component, ViewChild} from '@angular/core';
import {LocationServiceService} from "../../../../../Services/helpers/location-service.service";
import {UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import {ItemName} from "../../../../../Regexes/ItemName";
import {PostPictureServiceService} from "../../../../../Services/helpers/post-picture-service.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.scss']
})
export class PostDetailsComponent {

  tags: string[] = [];
  form: UntypedFormGroup = this.formBuilder.group({
    name: new UntypedFormControl(this.postPictureService.name, [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(40),
      Validators.pattern(ItemName)
    ]),
    description: new UntypedFormControl(this.postPictureService.description, [
      Validators.maxLength(500)
    ]),
    tags: new UntypedFormControl(this.postPictureService.tags, [
      Validators.maxLength(500)
    ]),
  });
  @ViewChild('pChips') ChipsInput: any;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private locationService: LocationServiceService,
    private postPictureService: PostPictureServiceService,
    private router: Router,
  ) {
  }

  // p-chips custom logic
  trimChips() {
    let tags: string[] = this.form.get('tags')?.value;
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
    this.form.get('tags')?.setValue(uniqueTagsTrimmed);
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

  submit(){
    this.postPictureService.name = this.form.getRawValue().name;
    this.postPictureService.description = this.form.getRawValue().description;
    this.postPictureService.tags = this.form.getRawValue().tags;

    this.router.navigate(['/picture/post/overview']);
  }

  return() {
    this.locationService.goBack();
  }

}
