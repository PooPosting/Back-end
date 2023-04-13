export interface AccountDto {
  id: string;
  roleId: string;
  nickname: string;
  email: string;
  profilePicUrl: string;
  backgroundPicUrl: string;
  accountDescription: string;
  accountCreated: string;

  isModifiable: boolean;
  isAdminModifiable: boolean;
}
