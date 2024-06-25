import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
function getAccessToken() {
  return localStorage.getItem("accessToken") || "";
}
console.log(getAccessToken());
export const teamApi = defineStore("team", {
  actions: {
    getAllTeams() {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetAllTeams", {
            headers: {
              Authorization: `Bearer ${getAccessToken()}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getTeamById(id) {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetTeamById", {
            params: {
              teamId: id,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getTeamUserById(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/User/GetAllUserByTeam/${id}`, null)
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    createTeams(params) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateTeam",
            {
              ...params,
            },
            {
              headers: {
                Authorization: `Bearer ${getAccessToken()}`,
              },
            }
          )
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    deleteTeams(id) {
      console.log(id);
      console.log(getAccessToken());
      return new Promise((resolve, reject) => {
        axios
          .delete(`/Admin/DeleteTeam/${id}`, {
            headers: {
              Authorization: `Bearer ${getAccessToken()}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    filterTeam(param) {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetAllTeams", {
            params: {
              name: param.name,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    createKpiEmployee(employee, IndicatorName, peroid, target) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateKPIForEmployee",
            {
              EmployeeId: employee,
              IndicatorName: IndicatorName,
              Target: target,
              Period: peroid,
            },
            {
              headers: {
                "Content-Type": "multipart/form-data",
                Authorization: `Bearer ${getAccessToken()}`,
              },
            }
          )
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    updateTeams(id, param) {
      // console.log(params);
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/Admin/UpdateTeam",

            {
              // params: {
              //   teamId: id,
              //   name: param.name,
              //   description: param.description,
              //   manager: param.manager,
              // }
              ...param,
            },
            {
              headers: {
                Authorization: `Bearer ${getAccessToken()}`,
              },
            }
          )
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
  },
});
