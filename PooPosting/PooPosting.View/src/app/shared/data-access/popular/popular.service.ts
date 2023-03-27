import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {PopularDto} from "../../utils/dtos/PopularDto";
import {environment} from "../../../../environments/environment";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class PopularService {

  constructor(
    private httpClient: HttpClient
  ) { }

  getPopularPictures(): Observable<PopularDto> {
    return this.httpClient
      .get<PopularDto>(
        `${environment.picturesApiUrl}/api/popular`,
        { responseType: "json" }
      );
  }
}
