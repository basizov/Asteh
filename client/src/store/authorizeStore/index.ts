import { InferActionType } from "..";
import { FullInfo } from "../../api/models/FullInfo";

const initialState = {
  loading: false,
  loadingInitial: false,
  filterModel: null as FullInfo | null
};

enum AuthorizeActionTypes {
  SET_LOADING = 'AUTHORIZE_SET_LOADING',
  SET_LOADING_INITIAL = 'AUTHORIZE_SET_LOADING_INITIAL',
  SET_FULL_INFO = 'AUTHORIZE_SET_FULL_INFO'
};

export const authorizeReducer = (state = initialState, action: AuthorizeAction) => {
  switch (action.type) {
    case AuthorizeActionTypes.SET_LOADING:
      return {...state, loading: action.payload};
      case AuthorizeActionTypes.SET_LOADING_INITIAL:
      return {...state, loadingInitial: action.payload};
    case AuthorizeActionTypes.SET_FULL_INFO:
      return {...state, filterModel: action.payload};
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
  setFullInfo: (payload: FullInfo) => ({
    type: AuthorizeActionTypes.SET_FULL_INFO,
    payload
  }) as const
};
