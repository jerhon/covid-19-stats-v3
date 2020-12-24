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
            light: '#5472d3',
            main: '#0d47a1',
            dark: '#002171',
            contrastText: '#fff',
        },
        secondary: {
            light: '#ffffb0',
            main: '#ffcc80',
            dark: '#ca9b52',
            contrastText: '#000',
        },
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
