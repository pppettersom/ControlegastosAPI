import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5014/api"
});

export default api;