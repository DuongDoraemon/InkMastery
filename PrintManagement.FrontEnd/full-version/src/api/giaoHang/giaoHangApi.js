import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
const authorization = localStorage.getItem("accessToken")
  ? localStorage.getItem("accessToken")
  : "";
export const giaoHangApi = defineStore("giaoHang", {
  actions: {
    getDevileryById(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/User/GetDeliveryById/${id}`)
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getAllGiaoHang() {
      return new Promise((resolve, reject) => {
        axios
          .post("/User/GetAllDelivery")
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },

    createDelivery(params) {
      return new Promise((resolve, reject) => {
        console.log(params);
        axios
          .post(
            "/Admin/CreateDelivery",
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
    shipperOder(param) {
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/User/ShipperConfirmOrderDelivery",
            { ...param },
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
    shipperConfirm(id, confirmStatus) {
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/User/ShipperConfirmDelivery",
            {
              DeliveryId: id,
              ConfirmStatus: confirmStatus,
            },
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
  },
});
