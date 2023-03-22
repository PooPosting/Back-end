import { SortSearchBy } from "../../Enums/SortSearchBy";

export interface SearchQuery {
  searchPhrase: string,
  pageNumber: number,
  pageSize: number,
  sortBy?: SortSearchBy,
}
