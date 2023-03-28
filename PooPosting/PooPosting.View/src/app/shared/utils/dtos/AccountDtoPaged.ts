import {AccountDto} from "./AccountDto";

export interface AccountDtoPaged {
  items: AccountDto[];
  totalPages: number;
  totalItems: number;
  page: number;
}
