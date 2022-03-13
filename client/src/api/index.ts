import { AxiosResponse } from "axios";
import { dbInstance } from "./dbInstance";
import { fileInstance } from "./fileInstance";
import { FullInfo } from "./models/FullInfo";
import { AuthorizeModel } from "./models/request/AuthorizeModel";
import { CreateUserModel } from "./models/request/CreateUserModel";
import { FilterModel } from "./models/request/FilterModel";
import { UpdateUserModel } from "./models/request/UpdateUserModel";
import { User } from "./models/User";
import { UserType } from "./models/UserType";

enum Paths {
  USERS = '/User',
  FILTER_USERS = '/User/find',
  USER_TYPES = '/UserType',
  AUTHORIZE = '/Authorize'
};

const responseBody = <T>(response: AxiosResponse<T>) => response.data;
const requests = (fromDatabase = true) => {
  const instance = fromDatabase ? dbInstance : fileInstance;

  return {
    get: <T>(url: string) =>
      instance.get<T>(url).then(responseBody),
    getWithParams: <T>(url: string, params: URLSearchParams) =>
      instance.get<T>(url, {params}).then(responseBody),
    post: <T>(url: string, body: {}) =>
      instance.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) =>
      instance.put<T>(url, body).then(responseBody),
    delete: <T>(url: string, body?: {}) =>
      instance.delete<T>(url, body).then(responseBody)
  };
};

const user = (fromDatabase = true) => {
  return {
    getUsers: () => requests(fromDatabase).get<User[]>(Paths.USERS),
    getUserById: (id: number) => requests(fromDatabase).get<User>(`${Paths.USERS}/${id}`),
    filterUsers: (payload: URLSearchParams) =>
      requests(fromDatabase).getWithParams<User[]>(Paths.FILTER_USERS, payload),
    createUser: (payload: CreateUserModel) => requests(fromDatabase).post<User>(Paths.USERS, payload),
    updateUser: (
      id: number,
      payload: UpdateUserModel) => requests(fromDatabase).put(`${Paths.USERS}/${id}`, payload),
    deleteUser: (id: number) => requests(fromDatabase).delete(`${Paths.USERS}/${id}`)
  };
};

const userType = (fromDatabase = true) => {
  return {
    getUserTypes: () => requests(fromDatabase).get<UserType[]>(Paths.USER_TYPES)
  };
};

const authorize = (fromDatabase = true) => {
  return {
    getFullInfo: () => requests(fromDatabase).get<FullInfo>(Paths.AUTHORIZE),
    login: (payload: AuthorizeModel) => requests(fromDatabase).post<FullInfo>(Paths.AUTHORIZE, payload) 
  };
};

export const API = (fromDatabase = true) => {
  return {
    USERS: user(fromDatabase),
    USER_TYPES: userType(fromDatabase),
    AUTHORIZATION: authorize(fromDatabase)
  };
};
