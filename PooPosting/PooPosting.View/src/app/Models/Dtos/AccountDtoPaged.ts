import {AccountDto} from "./AccountDto";

export interface AccountDtoPaged {
  items: AccountDto[];
  totalPages: number;
  pageSize: number;
  page: number;
}
