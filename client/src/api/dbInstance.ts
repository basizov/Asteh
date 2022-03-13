import axios from "axios";

const paramsDatabase =  new URLSearchParams();
paramsDatabase.append('fromDatabase', 'true');

export const dbInstance = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
  params: paramsDatabase,
  withCredentials: true
});
