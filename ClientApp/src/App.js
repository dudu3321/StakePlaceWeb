import React, { Component } from 'react'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import Login from './components/Login/Login'
import Main from './container/Main'
import { CookiesProvider } from 'react-cookie'
import { createStore } from 'redux'
import { Provider } from 'react-redux'
import filterApp from './redux/reducers/main'
import './custom.css';
import 'antd/dist/antd.css';


const App = () => {
  let store = createStore(filterApp);
  return (
    <Router>
      <Switch>

        <CookiesProvider>
          <Provider store={store}>
            <Route exact path="/" component={Login}></Route>
            <Route path="/Main" component={Main}></Route>
          </Provider>
        </CookiesProvider>

      </Switch>
    </Router>

  );
}

export default App;