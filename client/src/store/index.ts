import { applyMiddleware, combineReducers, createStore } from "redux";
import { composeWithDevTools } from "redux-devtools-extension";
import thunk from "redux-thunk";
import { authorizeReducer } from "./authorizeStore";
import { userReducer } from "./userStore";
import { userTypeReducer } from "./userTypeStore";

type PropertiesType<T> = T extends ({ [key: string]: infer U }) ? U : never;
export type InferActionType<T extends {
  [key: string]: (...args: any[]) => any
}> = ReturnType<PropertiesType<T>>

const reducer = combineReducers({
  users: userReducer,
  userTypes: userTypeReducer,
  authorization: authorizeReducer
});
export type RootState = ReturnType<typeof reducer>;

export const store = createStore(
  reducer, composeWithDevTools(applyMiddleware(thunk)));
