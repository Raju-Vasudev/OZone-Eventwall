import apiService from "../Hooks/apiService";

export default {
  fetchEvents: async (date) => {
    const response = await apiService.get(`shows?date=${date}`);
    return response.data;
  },

  getSuggestions: async (payload) => {
    const response = await apiService.post("events/improve", payload);
    return response.data;
  },

  create: async (payload) => {
    const response = await apiService.post("events", payload);
    return response.data;
  },
  subscribe: async (payload) => {
    const response = await apiService.post("Subscriptions", payload);
    return response.data;
  },
  suggestEvent: async (payload) => {
    const response = await apiService.post("events/suggest", payload);
    return response.data;
  },
  upload: async (payload) => {
    const response = await apiService.post("Artifacts/upload", payload);
    return response.data;
  },
  download: async (id) => {
    const response = await apiService.get(`Artifacts/download/${id}`);
    return response.data;
  },
};
