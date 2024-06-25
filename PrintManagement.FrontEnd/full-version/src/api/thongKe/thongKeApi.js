import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
function getAccessToken() {
  return localStorage.getItem("accessToken") || "";
}
const userInfo = JSON.parse(localStorage.getItem("userInfo"));
export const thongKeApi = defineStore("thongke", {
  actions: {
    thongKeLuong() {
      return new Promise((resolve, reject) => {
        axios
          .get("/User/GetstatisticSalary", {
            params: {
              userId: userInfo.Id,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getKpiUser() {
      return new Promise((resolve, reject) => {
        axios
          .get("/User/GetAllKpi", {
            params: {
              userId: userInfo.Id,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    thongKeDoanhSo(param) {
      return new Promise((resolve, reject) => {
        axios
          .get("/User/GetStatisticSales", { params: param })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
  },
});
