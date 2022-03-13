import { LocalizationProvider, DateRangePicker } from "@mui/lab";
import DateFnsAdapter from '@mui/lab/AdapterDateFns';
import { Button, CircularProgress, FormControl, Grid, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { format, fromUnixTime } from "date-fns";
import { ru } from "date-fns/locale";
import { Form, Formik } from "formik";
import React, { useState } from "react";
import { useMemo } from "react";
import { useDispatch } from "react-redux";
import { FilterModel } from "../../api/models/request/FilterModel";
import { useTypedSelector } from "../../hooks/useTypedSelector";
import { filterUsersAsync } from "../../store/userStore/asyncActions";

export const FilterUsers: React.FC = () => {
  const dispatch = useDispatch();
  const initialValues = useMemo(() => ({
    name: '',
    typeName: '',
    beginDate: '',
    endDate: ''
  } as FilterModel), []);
  const {userTypes} = useTypedSelector(s => s.userTypes);
  const {from} = useTypedSelector(s => s.authorization);
  const {loading: usersLoading} = useTypedSelector(s => s.users);
  const [dateRange, setDateRange] = useState<[Date | null, Date | null]>([null, null]);

  return <Formik
    initialValues={initialValues}
    onSubmit={async values => {
      await dispatch(filterUsersAsync(values, from));
    }}
  >
    {({
      handleSubmit,
      handleBlur,
      handleChange,
      values,
      setFieldValue
    }) => (
      <Form onSubmit={handleSubmit}>
        <Grid
          container
          alignItems='center'
          sx={{width: '100%', marginTop: '.7rem'}}
        >
          <Grid item xs={3}>
            <TextField
              id='name'
              type='name'
              variant='outlined'
              fullWidth
              onBlur={handleBlur}
              onChange={handleChange}
              onFocus={(e) => e.target.select()}
              value={values.name}
              label="ФИО"
            />
          </Grid>
          <Grid item xs={3}>
            <FormControl fullWidth>
              <InputLabel id="typeName-label">Тип пользователя</InputLabel>
              <Select
                id="typeName"
                labelId="typeName-label"
                value={values.typeName}
                label="Тип пользователя"
                onChange={(e) => setFieldValue('typeName', e.target.value)}
              >
                {userTypes.map(userType => <MenuItem
                  key={userType.id}
                  value={userType.name}
                >{userType.name}</MenuItem>)}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={5}>
            <LocalizationProvider
              dateAdapter={DateFnsAdapter} 
              locale={ru}
            >
              <DateRangePicker
                startText="С"
                endText="По"
                value={dateRange}
                onChange={(newValue) => {
                  setDateRange(newValue);
                  if (newValue[0] && newValue[1]) {
                    setFieldValue('beginDate', format(newValue[0], 'dd.MM.yyyy'));
                    setFieldValue('endDate', format(newValue[1], 'dd.MM.yyyy'));
                  }
                }}
                renderInput={(startProps, endProps) => (
                  <React.Fragment>
                    <TextField {...startProps} />
                    <TextField {...endProps} />
                  </React.Fragment>
                )}
              />
            </LocalizationProvider>
          </Grid>
          <Grid item xs={1}>
            <Button type='submit'>{usersLoading
              ? <CircularProgress size={10} color="inherit"/>
              : '✅'}</Button>
          </Grid>
        </Grid>
      </Form>
    )}
  </Formik>
};