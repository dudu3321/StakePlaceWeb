import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { CookiesProvider } from 'react-cookie';
import filterApp from './redux/reducers/main/index';
import { Provider } from 'react-redux';
import { createStore } from 'redux';


const rootElement = document.getElementById('root');
let store = createStore(filterApp);

ReactDOM.render(
  <Provider store={store}>
    <CookiesProvider>
      <App />
    </CookiesProvider>
  </Provider>,
  rootElement
);

registerServiceWorker();
