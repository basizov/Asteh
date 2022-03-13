import { AxiosResponse } from "axios";
import { instance } from "./instance";
import { FullInfo } from "./models/FullInfo";
import { AuthorizeModel } from "./models/request/AuthorizeModel";
import { CreateUserModel } from "./models/request/CreateUserModel";
import { FilterModel } from "./models/request/FilterModel";
import { UpdateUserModel } from "./models/request/UpdateUserModel";
import { User } from "./models/User";
import { UserType } from "./models/UserType";

enum Paths {
  USERS = '/Users',
  FILTER_USERS = '/Users/find',
  USER_TYPES = '/UserType',
  AUTHORIZE = '/Authorize'
};

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => instance.get<T>(url).then(responseBody),
  getWithParams: <T>(url: string, params: URLSearchParams) =>
    instance.get<T>(url, {params}).then(responseBody),
  post: <T>(url: string, body: {}) => instance.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => instance.put<T>(url, body).then(responseBody),
  delete: <T>(url: string, body?: {}) => instance.delete<T>(url, body).then(responseBody)
};

const user = {
  getUsers: () => requests.get<User[]>(Paths.USERS),
  getUserById: (id: number) => requests.get<User>(`${Paths.USERS}/${id}`),
  filterUsers: (payload: URLSearchParams) =>
    requests.getWithParams<User>(Paths.FILTER_USERS, payload),
  createUser: (payload: CreateUserModel) => requests.post<User>(Paths.USERS, payload),
  updateUser: (
    id: number,
    payload: UpdateUserModel) => requests.put(`${Paths.USERS}/${id}`, payload),
  deleteUser: (id: number) => requests.delete(`${Paths.USERS}/${id}`)
};

const userType = {
  getUserTypes: () => requests.get<UserType[]>(Paths.USER_TYPES)
};

const authorize = {
  getFullInfo: () => requests.get<FullInfo>(Paths.AUTHORIZE),
  login: (payload: AuthorizeModel) => requests.post<FullInfo>(Paths.AUTHORIZE, payload) 
};

export const API = {
  USERS: user,
  USER_TYPES: userType,
  AUTHORIZATION: authorize
};
