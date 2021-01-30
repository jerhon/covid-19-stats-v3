import React, {useState} from 'react';
import './App.css';
import {AppBar, Drawer, IconButton, Toolbar, Typography} from "@material-ui/core";
import MenuIcon from "@material-ui/icons/Menu";
import {makeStyles} from "@material-ui/core/styles";
import clsx from "clsx";
import {Switch, Route, Redirect} from "react-router-dom"
import {NationPage} from "./features/nation-page";


const useStyles = makeStyles({
    drawerContent: {
        width: 250,
    },
    root: {
        display: 'flex',
        flexDirection: 'column',
        backgroundColor: '#f5f5f5'
    },
    page: {
        margin: 16,
        minHeight: '100vh'
    }
})

function App() {
    let [drawerOpen, setDrawerOpen] = useState(false);
    let classes = useStyles();

    return (
        <div className={classes.root}>
            <AppBar position="static">
                <Toolbar>
                    <IconButton edge="start" color="inherit" aria-label="menu"
                                onClick={() => setDrawerOpen(!drawerOpen)}>
                        <MenuIcon/>
                    </IconButton>
                    <Typography variant="h6">
                        COVID-19 Statistics
                    </Typography>
                </Toolbar>
            </AppBar>
            <Drawer open={drawerOpen} anchor="left" onClose={() => setDrawerOpen(false)}>
                <div className={clsx(classes.drawerContent)}>Drawer</div>
            </Drawer>
            <div className={classes.page}>
                <Switch>
                    <Route exact path="/" component={NationPage} />
                    <Route>
                        <Redirect to="/" />
                    </Route>
                </Switch>
            </div>

        </div>
    );
}

export default App;
