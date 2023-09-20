import axios from "axios";

const url = "http://localhost:5160";

const promiseWithErrorHandling = (promise) => {
  return promise.catch((err) => {
    if (err.response && err.response.status === 500) {
      window.location.assign("/error");
    } else {
      throw err;
    }
  });
};

export default {
  post: async (path, payload) => {
    return promiseWithErrorHandling(axios.post(`${url}/${path}`, payload));
  },

  get: async (path) => {
    return promiseWithErrorHandling(axios.get(`${url}/${path}`));
  },

  getWithParameter: async (path, parameter) => {
    return promiseWithErrorHandling(axios.get(`${url}/${path}`, parameter));
  },

  postWithoutErrorHandling: async (path, payload) => {
    return axios.post(`${url}/${path}`, payload);
  },

  postSignup: async (path, payload) => {
    return promiseWithErrorHandling(axios.post(`${url}/${path}`, payload));
  },

  put: async (path, payload) => {
    return promiseWithErrorHandling(axios.put(`${url}/${path}`, payload));
  },
};
