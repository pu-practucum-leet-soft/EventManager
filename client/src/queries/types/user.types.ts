export type UserViewModel = {
  Id: number;
  userName: string;
  email: string;
  role: "User" | "Admin";
};

export type Token = {
  access: string;
  refresh: string;
};
