import axios from "axios";

const paramsFile =  new URLSearchParams();
paramsFile.append('fromDatabase', 'false');

export const fileInstance = axios.create({
  baseURL: 'http://localhost:5000',
  params: paramsFile,
  withCredentials: true
});
