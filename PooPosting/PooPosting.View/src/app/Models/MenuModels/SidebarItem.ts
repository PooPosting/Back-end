import {MenuItem} from "./MenuItem";

export interface SidebarItem {
  label: string;
  class: string;
  showWhileLoggedIn: boolean;
  showWhileLoggedOut: boolean;
  menuItems: MenuItem[];
}
