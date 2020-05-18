import React from 'react'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import Login from './components/Login/Login'
import Main from './container/Main'
import { CookiesProvider } from 'react-cookie'
import { createStore } from 'redux'
import { Provider } from 'react-redux'
import filterApp from './redux/reducers/main'
import './custom.css';
import 'antd/dist/antd.css';
import UserStatus from './components/UserStatus'
import { useHubConnection } from './components/useHubConnection'

const App = () => {
  let store = createStore(filterApp);
  const hubConnection = useHubConnection('UserConnectionHub');

  return (
    <Router>
      <Switch>
        <CookiesProvider>
          <Provider store={store}>
            <UserStatus hubConnection={hubConnection}></UserStatus>
            <Route exact path="/" component={() => <Login hubConnection={hubConnection} />} ></Route>
            <Route path="/Main" component={Main}></Route>
          </Provider>
        </CookiesProvider>
      </Switch>
    </Router>
  );
}

export default App;