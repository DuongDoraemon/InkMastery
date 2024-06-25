import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
const authorization = localStorage.getItem("accessToken")
  ? localStorage.getItem("accessToken")
  : "";
export const customerApi = defineStore("customer", {
  actions: {
    getAllCustomer() {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetAllCustomers", {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    addCustomer(params) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateCustomer",
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

    deleteCustomer(id) {
      console.log(id);
      return new Promise((resolve, reject) => {
        axios
          .delete(`/Admin/DeleteCustomer/${id}`, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getByIdCustomer(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/Admin/GetCustomerById/${id}`, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    updateCustomer(id, params){
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
