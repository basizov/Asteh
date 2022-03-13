import { ThemeProvider } from "@emotion/react";
import { createTheme, CssBaseline, Paper, styled, useMediaQuery } from "@mui/material";
import React, { useEffect, useMemo } from "react";
import { useDispatch } from "react-redux";
import { Route, Routes } from "react-router-dom";
import { useTypedSelector } from "../hooks/useTypedSelector";
import { AuthorizetionPage } from "../pages/AuthorizationPage";
import { UsersPage } from "../pages/UsersPage";
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
    components: {
      MuiCssBaseline: {
        styleOverrides: {
          body: {
            scrollbarColor: "#6b6b6b #2b2b2b",
            "&::-webkit-scrollbar, & *::-webkit-scrollbar": {
              backgroundColor: "transparent",
              height: 7,
              width: 7
            },
            "&::-webkit-scrollbar-thumb, & *::-webkit-scrollbar-thumb": {
              borderRadius: 8,
              backgroundColor: "#6b6b6b"
            },
            "&::-webkit-scrollbar-thumb:focus, & *::-webkit-scrollbar-thumb:focus": {
              backgroundColor: "#959595",
            },
            "&::-webkit-scrollbar-thumb:active, & *::-webkit-scrollbar-thumb:active": {
              backgroundColor: "#959595",
            },
            "&::-webkit-scrollbar-thumb:hover, & *::-webkit-scrollbar-thumb:hover": {
              backgroundColor: "#959595",
            },
            "&::-webkit-scrollbar-corner, & *::-webkit-scrollbar-corner": {
              backgroundColor: "#2b2b2b",
            },
          }
        }
      }
    }
  }), [prefersDarkMode]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline/>
      <RootPaper>
        <Loading loading={loadingInitial}/>
        {!loadingInitial && <React.Fragment>
          <Routes>
            <Route path="/" element={<UsersPage/>}/>
            <Route path="/auth" element={<AuthorizetionPage/>}/>
          </Routes>  
        </React.Fragment>}
      </RootPaper>
    </ThemeProvider>)
};
