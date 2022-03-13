import { AxiosResponse } from "axios";
import { instance } from "./instance";

enum Paths {
  USERS = '/Users' 
};

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => instance.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) => instance.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => instance.put<T>(url, body).then(responseBody),
  delete: <T>(url: string, body?: {}) => instance.delete<T>(url, body).then(responseBody)
};

export const API = {};
