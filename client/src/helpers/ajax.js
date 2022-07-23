import axios from "axios";
export default {
  install: (app) => {
    const instance = axios.create({
      baseURL: "https://localhost:7272",
    });
    const customConfig = {
      headers: {
        'Content-Type': 'application/json'
      }
    };
    let ajax = {
      get: function (url) {
        return instance.get(url, customConfig);
      },
      post: function (url, data) {
        return instance.post(url, data, customConfig);
      },
      delete: function (url) {
        return instance.delete(url, customConfig);
      },
      put: function (url, data) {
        return instance.put(url, data, customConfig);
      },
    };
    app.config.globalProperties.$ajax = ajax;
  },
};