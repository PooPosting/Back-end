import {AccountModel} from "./AccountModel";

export interface AccountPagedResult {
  items: AccountModel[];
  totalPages: number;
  pageSize: number;
  page: number;
}
