import axios from "axios";

const paramsDatabase =  new URLSearchParams();
paramsDatabase.append('fromDatabase', 'true');

export const dbInstance = axios.create({
  baseURL: 'http://localhost:5000',
  params: paramsDatabase,
  withCredentials: true
});
