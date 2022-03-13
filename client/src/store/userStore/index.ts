import { InferActionType } from "..";
import { User } from "../../api/models/User";

const initialState = {
  loading: false,
  users: [] as User[],
  selectedUser: null as User | null
};

enum UserActionTypes {
  SET_LOADING = 'USER_SET_LOADING',
  SET_USERS = 'USER_SET_USERS',
  SET_SELECTED_USER = 'USER_SET_SELECTED_USER',
  ADD_USER = 'USER_ADD_USER'
};

export const userReducer = (state = initialState, action: UserAction) => {
  switch (action.type) {
    case UserActionTypes.SET_LOADING:
      return {...state, loading: action.payload};
    case UserActionTypes.SET_USERS:
      return {...state, users: action.payload};
    case UserActionTypes.SET_SELECTED_USER:
      return {...state, selectedUser: action.payload};
    case UserActionTypes.ADD_USER:
      return {...state, users: [...state.users, action.payload]};
    default:
      return {...state};
  }
};

export type UserAction = InferActionType<typeof userActions>;

export const userActions = {
  setLoading: (payload: boolean) => ({
    type: UserActionTypes.SET_LOADING,
    payload
  }) as const,
  setUsers: (payload: User[]) => ({
    type: UserActionTypes.SET_USERS,
    payload
  }) as const,
  setSelectedUser: (payload: User) => ({
    type: UserActionTypes.SET_SELECTED_USER,
    payload
  }) as const,
  addUser: (payload: User) => ({
    type: UserActionTypes.ADD_USER,
    payload
  }) as const
};
