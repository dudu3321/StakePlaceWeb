import React, { Component } from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Login from './components/Login/Login';
import Main from './container/Main';
import { CookiesProvider } from 'react-cookie';

import './custom.css';
import 'antd/dist/antd.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Router>
        <Switch>
          <CookiesProvider>
            <Route exact path="/" component={Login}></Route>
            <Route path="/Main" component={Main}></Route>
          </CookiesProvider>
        </Switch>
      </Router>
    );
  }
}
