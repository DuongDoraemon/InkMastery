import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
const authorization = localStorage.getItem("accessToken")
  ? localStorage.getItem("accessToken")
  : "";
export const notificationApi = defineStore("notification", {
  actions: {
    getAllNotification(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/User/GetNotificationsByUser/${id}`, {})
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },

    updateSeenNotification(id) {
      return new Promise((resolve, reject) => {
        axios
          .put(`/User/ConfirmIsSeenNotification/${id}`, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    updateCustomer(id, params) {
      return new Promise((resolve, reject) => {
        console.log(params);
        axios
          .put(
            "/Admin/UpdateCustomer",
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
    filterCustomer(param) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/Admin/GetAllCustomers`, {
            params: {
              Name: param.name,
              PhoneNumber: param.phoneNumber,
              Address: param.address,
            },
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
  },
});
