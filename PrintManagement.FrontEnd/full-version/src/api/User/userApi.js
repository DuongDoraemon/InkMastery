import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
const authorization = localStorage.getItem("accessToken")
  ? localStorage.getItem("accessToken")
  : "";
const userInfo = JSON.parse(localStorage.getItem("userInfo"));
const id = userInfo.Id;
export const userApi = defineStore("user", {
  actions: {
    getAllUsers() {
      return new Promise((resolve, reject) => {
        axios
          .get("/User/GetAllUsers")
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    updateUserAccount(id, params) {
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/User/UpdateUser",
            { ...params },
            {
              headers: {
                "Content-Type": "multipart/form-data",
                Authorization: `Bearer ${authorization}`,
              },
            }
          )
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    updateChangeTeamForUser(params) {
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/Admin/ChangeDepartmentForUser",
            { ...params },
            {
              headers: {
                Authorization: `Bearer ${authorization}`,
              },
            }
          )
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    addRoleUser(id, roles) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            `/Admin/AddRoleToUser?userId=${id}`,

            roles,

            {
              headers: {
                Authorization: `Bearer ${authorization}`,
              },
            }
          )
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getAllUseHaveRoleManager() {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetAllUserHaveRoleManager")
          .then((res) => {
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      });
    },
    filterUser(param) {
      return new Promise((resolve, reject) => {
        // https://localhost:7070/api/User/GetAllUsers?Name=D%C6%B0%C6%A1ng&Email=duongtv280703%40gmail.com&PhoneNumber=0388049008
        axios
          .get("/User/GetAllUsers", {
            params: {
              email: param.email,
              name: param.name,
              phoneNumber: param.phoneNumber,
              teamId: param.teamId,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getUserById(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/User/GetUserById/${id}`)
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getRolesByIdUser(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/Admin/GetRolesByUserId/${id}`, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getAllUserContainsLeaderRole() {
      return new Promise((resolve, reject) => {
        axios
          .get("/User/GetAllUserContainsLeaderRole")
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
  },
});
