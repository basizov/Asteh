import { InferActionType } from "..";
import { FullInfo } from "../../api/models/FullInfo";

const initialState = {
  loading: false,
  loadingInitial: false,
  userId: 0,
  isAccessEnabled: false
};

enum AuthorizeActionTypes {
  SET_LOADING = 'AUTHORIZE_SET_LOADING',
  SET_LOADING_INITIAL = 'AUTHORIZE_SET_LOADING_INITIAL',
  SET_USER_ID = 'AUTHORIZE_SET_USER_ID',
  SET_IS_ACCESS_ENABLED = 'AUTHORIZE_SET_IS_ACCESS_ENABLED'
};

export const authorizeReducer = (state = initialState, action: AuthorizeAction) => {
  switch (action.type) {
    case AuthorizeActionTypes.SET_LOADING:
      return {...state, loading: action.payload};
      case AuthorizeActionTypes.SET_LOADING_INITIAL:
      return {...state, loadingInitial: action.payload};
    case AuthorizeActionTypes.SET_USER_ID:
      return {...state, userId: action.payload};
    case AuthorizeActionTypes.SET_IS_ACCESS_ENABLED:
      return {...state, isAccessEnabled: action.payload};
    default:
      return {...state};
  }
};

export type AuthorizeAction = InferActionType<typeof authorizeActions>;

export const authorizeActions = {
  setLoading: (payload: boolean) => ({
    type: AuthorizeActionTypes.SET_LOADING,
    payload
  }) as const,
  setLoadingInitial: (payload: boolean) => ({
    type: AuthorizeActionTypes.SET_LOADING_INITIAL,
    payload
  }) as const,
  setUserId: (payload: number) => ({
    type: AuthorizeActionTypes.SET_USER_ID,
    payload
  }) as const,
  setIsAccessEnabled: (payload: boolean) => ({
    type: AuthorizeActionTypes.SET_IS_ACCESS_ENABLED,
    payload
  }) as const
};
