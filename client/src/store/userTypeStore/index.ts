import { InferActionType } from "..";
import { UserType } from "../../api/models/UserType";

const initialState = {
  loading: false,
  userTypes: [] as UserType[]
};

enum UserTypeActionTypes {
  SET_LOADING = 'USER_TYPE_SET_LOADING',
  SET_USER_TYPES = 'USER_TYPE_SET_USER_TYPES'
};

export const userTypeReducer = (state = initialState, action: UserTypeAction) => {
  switch (action.type) {
    case UserTypeActionTypes.SET_LOADING:
      return {...state, loading: action.payload};
    case UserTypeActionTypes.SET_USER_TYPES:
      return {...state, userTypes: action.payload};
    default:
      return {...state};
  }
};

export type UserTypeAction = InferActionType<typeof userTypeActions>;

export const userTypeActions = {
  setLoading: (payload: boolean) => ({
    type: UserTypeActionTypes.SET_LOADING,
    payload
  }) as const,
  setUserTypes: (payload: UserType[]) => ({
    type: UserTypeActionTypes.SET_USER_TYPES,
    payload
  }) as const
};
