import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
const authorization = localStorage.getItem("accessToken")
  ? localStorage.getItem("accessToken")
  : "";

// const login = async (params) => {
//   return await axios.post('', {...params})
// }
// export default AuthService = {
//   login: login,
// }
export const authApi = defineStore("auth", {
  actions: {
    login(params) {
      return new Promise((resolve, reject) => {
        axios
          .post("/Auth/Login", { ...params })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    register(params) {
      return new Promise((resolve, reject) => {
        console.log(params);
        // params.DateOfBirth = format(new Date(params.DateOfBirth), "yyyy-MM-dd");
        axios
          .post(
            "/Auth/Register",
            { ...params },
            {
              headers: {
                "Content-Type": "multipart/form-data",
              },
            }
          )
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },

    async changePassword(params) {
      const authToken = `Bearer ${authorization}`;
      console.log(authToken);
      const res = await axios.put(
        `/Auth/ChangePassword`,
        { ...params },
        {
          headers: {
            Authorization: authToken,
          },
        }
      );
    },
    forgotPassword(param) {
      return new Promise((resolve, reject) => {
        axios
          .post("/Auth/ForgotPassword", null, {
            params: {
              email: param.email,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getAllRoles() {
      return new Promise((resolve, reject) => {
        axios
          .get("/Auth/GetAllRoles", {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    confirmCreateNewPassword(params) {
      return new Promise((resolve, reject) => {
        axios
          .put("/Auth/ConfirmCreateNewPassword", { ...params })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
  },
});
