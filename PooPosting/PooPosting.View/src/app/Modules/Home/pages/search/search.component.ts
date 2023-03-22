import {Component, OnInit, ViewChild} from '@angular/core';
import { HttpServiceService } from 'src/app/Services/http/http-service.service';
import { HttpParamsServiceService } from 'src/app/Services/http/http-params-service.service';
import {MessageService} from "primeng/api";
import {AccountDtoPaged} from "../../../../Models/Dtos/AccountDtoPaged";
import {ScrollServiceService} from "../../../../Services/helpers/scroll-service.service";
import {Title} from "@angular/platform-browser";
import {CacheServiceService} from "../../../../Services/data/cache-service.service";
import {PictureDtoPaged} from "../../../../Models/Dtos/PictureDtoPaged";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  @ViewChild('picPaginator') picPaginator: any;
  @ViewChild('accPaginator') accPaginator: any;
  isLoggedOn: boolean = false;
  picPage: number = 1;
  accPage: number = 1;

  picturesResult: PictureDtoPaged = {
    items:[],
    page:0,
    pageSize: 0,
    totalPages:0
  }
  accountsResult: AccountDtoPaged = {
    items:[],
    page:0,
    pageSize: 0,
    totalPages:0
  }

  constructor(
    private httpService: HttpServiceService,
    private paramsService: HttpParamsServiceService,
    private messageService: MessageService,
    private scrollService: ScrollServiceService,
    private cacheService: CacheServiceService,
    private title: Title
  ) {
    this.title.setTitle('PicturesUI - Panel wyszukiwania');
  }

  ngOnInit(): void {
    this.scrollService.loadScrollState();
    this.paramsService.setSearchPageNumber(1);
    this.isLoggedOn = this.cacheService.getUserLoggedOnState();
  }

  searchPictures() {
    this.clearSearch();
    this.httpService.searchPicturesRequest().subscribe({
      next: (val: PictureDtoPaged) => {
        console.log(val);
        this.picturesResult = val;
        this.messageService.add({
          severity:'success',
          summary: 'Sukces',
          detail: `Wyświelanie wyników dla "${this.paramsService.SearchQuery.searchPhrase}"`
        });
        this.clearAccountsResult();
        this.picPaginator.updateCurrentPage(val.page);
        this.picPaginator.updatePages(val.totalPages);
        this.scrollService.loadScrollState();
      },
      error: () => {
        this.messageService.add({
          severity:'error',
          summary: 'Niepowodzenie',
          detail: `Nie znaleziono żadnych wyników dla "${this.paramsService.SearchQuery.searchPhrase}"`});
      }
    });
  }
  searchAccounts() {
    this.clearSearch();
    this.httpService.searchAccountsRequest().subscribe({
      next: (val: AccountDtoPaged) => {
        this.accountsResult = val;
        this.messageService.add({
          severity:'success',
          summary: 'Sukces',
          detail: `Wyświelanie wyników dla "${this.paramsService.SearchQuery.searchPhrase}"`
        });
        this.clearPictureResult();
        this.accPaginator.updateCurrentPage(val.page);
        this.accPaginator.updatePages(val.totalPages);
        this.scrollService.loadScrollState();
      },
      error: () => {
        this.messageService.add({
          severity:'error',
          summary: 'Niepowodzenie',
          detail: `Nie znaleziono żadnych wyników dla "${this.paramsService.SearchQuery.searchPhrase}"`});
      }
    });
  }
  clearSearch() {
    this.scrollService.resetScrollState();
    this.scrollService.loadScrollState();
    this.clearAccountsResult();
    this.clearPictureResult();
  }

  paginate(val: any): void {
    this.scrollService.resetScrollState();
    this.scrollService.loadScrollState();
    this.paramsService.setSearchPageNumber(val+1);
    this.updatePage();
    this.fetchPictures();
  }
  paginateAccs(val: any): void {
    this.scrollService.resetScrollState();
    this.scrollService.loadScrollState();
    this.paramsService.setSearchPageNumber(val+1);
    this.updatePage();
    this.fetchAccounts();
  }

  private clearPictureResult() {
    this.picturesResult = {
      items:[],
      page:0,
      pageSize: 0,
      totalPages:0
    }
  }
  private clearAccountsResult() {
    this.accountsResult = {
      items:[],
      page:0,
      pageSize: 0,
      totalPages:0
    }
  }

  private fetchPictures(): void {
    this.httpService.searchPicturesRequest().subscribe({
      next: (val: PictureDtoPaged) => {
        this.picturesResult = val;
        document.body.scrollTop = 0;
        this.picPaginator.updateCurrentPage(val.page);
        this.picPaginator.updatePages(val.totalPages);
      }
    });
  }
  private fetchAccounts(): void {
    this.httpService.searchAccountsRequest().subscribe({
      next: (val: AccountDtoPaged) => {
        this.accountsResult = val;
        document.body.scrollTop = 0;
        this.accPaginator.updateCurrentPage(val.page);
        this.accPaginator.updatePages(val.totalPages);
      }
    });
  }

  private updatePage(): void {
    this.picPage = this.paramsService.getPageNumber();
    this.accPage = this.paramsService.getPageNumber();
  }

}
