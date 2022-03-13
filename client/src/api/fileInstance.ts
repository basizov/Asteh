import axios from "axios";

const paramsFile =  new URLSearchParams();
paramsFile.append('fromDatabase', 'false');

export const fileInstance = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
  params: paramsFile,
  withCredentials: true
});
