import React from 'react';
import "fontsource-roboto";
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import * as statementSlice from "./features/state.slice";
import { combineReducers } from 'redux';
import {configureStore} from "@reduxjs/toolkit";
import {Provider} from "react-redux";
import { BrowserRouter } from "react-router-dom";
import {createMuiTheme} from "@material-ui/core";
import {ThemeProvider} from "@material-ui/styles"
import * as nationSlice from "./features/nation.slice";
import {blue, red} from "@material-ui/core/colors";

const reducer = combineReducers({
    ...statementSlice.reducer, 
    ...nationSlice.reducer
});

const store = configureStore({
    reducer
});


const theme = createMuiTheme({
    palette: {
        primary: {
            main: blue[900]
        },
        secondary: {
            main: red[800]
        }
    },
    typography: {
        fontSize: 16
    },
    overrides: {
        MuiLink: {
            root: {
                color: red[900]
            }
        }
    }
})

ReactDOM.render(
  <React.StrictMode>
      <Provider store={store}>
          <ThemeProvider theme={theme}>
            <BrowserRouter>
                <App />
            </BrowserRouter>
          </ThemeProvider>
      </Provider>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
