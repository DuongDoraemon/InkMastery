import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
const authorization = localStorage.getItem("accessToken")
  ? localStorage.getItem("accessToken")
  : "";
export const resourceApi = defineStore("resource", {
  actions: {
    getAllsResource() {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetAll")
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    createResouce(params) {
      console.log(params);
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateResourceInformation",
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
    updateResouce(id, param) {
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/Admin/UpdateResourceInformation",
            { ...param },
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
    deleteResource(id) {
      return new Promise((resolve, reject) => {
        axios
          .delete(`/Admin/DeleteResource/${id}`, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getByIdResource(id) {
      console.log(id);
      return new Promise((resolve, reject) => {
        axios
          .get(`/Admin/GetById/${id}`, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    fillterData(param) {
      return new Promise((resolve, reject) => {
        axios
          .get(
            "/Admin/GetAll",
            {
              params: {
                resourceName: param,
              },
            },
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
    createImportCoupon(params) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateImportCoupon",
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
    createResourcePropertyInFormation(params) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateResourcePropertyInformation",
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
    deleteCoupon(id) {
      return new Promise((resolve, reject) => {
        axios
          .delete(`/Admin/DeleteResourcePropertyDetail/${id}`, {
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
