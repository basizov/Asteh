import { User } from "./User";
import { UserType } from "./UserType";

export type FullInfo = {
  userId: number;
  isAccessEnabled: boolean;
  users: User[];
  userTypes: UserType[];
};