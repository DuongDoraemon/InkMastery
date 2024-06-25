import router from "@/router";
import axios from "axios";

const axiosIns = axios.create({
  // You can add your headers here
  // ================================
  // baseURL: 'https://some-domain.com/api/',
  // timeout: 1000,
  // headers: {'X-Custom-Header': 'foobar'}
});

// ℹ️ Add request interceptor to send the authorization header on each subsequent request after login
axiosIns.interceptors.request.use((config) => {
  // Retrieve token from localStorage
  const token = localStorage.getItem("accessToken");

  // If token is found
  if (token) {
    // Get request headers and if headers is undefined assign blank object
    config.headers = config.headers || {};

    // Set authorization header
    // ℹ️ JSON.parse will convert token to string
    config.headers.Authorization = token ? `Bearer ${JSON.parse(token)}` : "";
  }

  // Return modified config
  return config;
});

// ℹ️ Add response interceptor to handle 401 response
axiosIns.interceptors.response.use(
  (response) => {
    console.log(response);
    return response;
  },
  (error) => {
    // Capture the response object
    const response = error.response;

    console.log(response);
    // Handle error
    if (response.status === 401) {
      // ℹ️ Logout user and redirect to login page
      // Remove "userData" from localStorage
      localStorage.removeItem("userInfo");

      // Remove "accessToken" from localStorage
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");

      // If 401 response returned from api
      router.push("/login");
    } else {
      return Promise.reject(error);
    }
  }
);
export default axiosIns;
