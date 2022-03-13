import { Button, Grid, TextField } from "@mui/material";
import { Form, Formik } from "formik";
import React, { useMemo } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { object, string } from "yup";
import { SchemaOptions } from "yup/lib/schema";
import { AuthorizeModel } from "../api/models/request/AuthorizeModel";
import { StyledPaper } from "../components/Styled/StyledPaper";
import { useTypedSelector } from "../hooks/useTypedSelector";
import { authorizeUserAsync } from "../store/authorizeStore/asyncActions";

export const AuthorizetionPage: React.FC = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const {from} = useTypedSelector(s => s.authorization);
  const initialState = useMemo(() => ({
    login: '',
    password: ''
  } as AuthorizeModel), []);
  const validationSchema: SchemaOptions<AuthorizeModel> = useMemo(() => object({
    login: string().required(),
    password: string().required()
  }), []);
  
  return <StyledPaper>
    <Formik
      initialValues={initialState}
      validationSchema={validationSchema}
      onSubmit={async values => {
        await dispatch(authorizeUserAsync(values, from));
        navigate('/');
      }}
    >
     {({
      handleSubmit,
      handleBlur,
      handleChange,
      values,
      errors
     }) => (
       <Form onSubmit={handleSubmit}>
       <Grid
         container
         sx={{padding: '1rem', minWidth: '20rem'}}
         direction='column'
         spacing={1}
       >
         <Grid item>
           <TextField
             id='login'
             type='text'
             variant='outlined'
             fullWidth
             onBlur={handleBlur}
             onChange={handleChange}
             onFocus={(e) => e.target.select()}
             value={values.login}
             error={!!errors.login}
             label="Логин"
           />
         </Grid>
         <Grid item>
           <TextField
             id='password'
             type='password'
             variant='outlined'
             fullWidth
             onBlur={handleBlur}
             onChange={handleChange}
             onFocus={(e) => e.target.select()}
             value={values.password}
             error={!!errors.password}
             label="Пароль"
           />
         </Grid>
         <Grid item>
          <Button
            variant="outlined"
            type='submit'
            fullWidth
          >Сменить</Button>
         </Grid>
        </Grid>
       </Form>
     )} 
    </Formik>
  </StyledPaper>;
};