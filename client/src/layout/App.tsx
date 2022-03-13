import { ThemeProvider } from "@emotion/react";
import { createTheme, CssBaseline, Paper, styled, useMediaQuery } from "@mui/material";
import React, { useEffect, useMemo } from "react";
import { useDispatch } from "react-redux";
import { Route, Routes } from "react-router-dom";
import { useTypedSelector } from "../hooks/useTypedSelector";
import { getFullInfoAsync } from "../store/authorizeStore/asyncActions";
import { Loading } from "./Loading";

const RootPaper = styled(Paper)({
  position: 'absolute',
  top: 0,
  left: 0,
  width: '100%',
  height: '100vh',
  borderRadius: 0
});

export const App: React.FC = () => {
  const dispatch = useDispatch();
  const {loadingInitial} = useTypedSelector(s => s.authorization);

  useEffect(() => {
    dispatch(getFullInfoAsync());
  }, []);

  const prefersDarkMode = useMediaQuery('(prefers-color-scheme: dark)');
  const theme = useMemo(() => createTheme({
    palette: {
      mode: prefersDarkMode ? 'dark' : 'light',
    },
  }), [prefersDarkMode]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline/>
      <RootPaper>
        <Loading loading={loadingInitial}/>
        {!loadingInitial && <React.Fragment>
          <Routes>
          </Routes>  
        </React.Fragment>}
      </RootPaper>
    </ThemeProvider>)
};
