import axios from "axios";
import { defineStore } from "pinia";
// Định nghĩa baseURL cho axios
axios.defaults.baseURL = "https://localhost:44389/api";
const authorization = localStorage.getItem("accessToken")
  ? localStorage.getItem("accessToken")
  : "";
export const projectApi = defineStore("project", {
  actions: {
    getAllsProject() {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetAllProject", {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    addProject(params) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateProject",
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
    createDeviler(params) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "Admin/CreateDelivery",
            { ...params },
            {
              headers: {
                Authorization: `Bearer ${authorization}`,
              },
            }
          )
          .then((response) => resolve(response))
          .catch((error) => reject(error));
      });
    },
    updateProject(id, params) {
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/Admin/UpdateProject",
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
    createPrintJob(params) {
      return new Promise((resolve, reject) => {
        axios
          .post("/Admin/CreatePrintJob", params, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    confirmDonePrintJob(id) {
      return new Promise((resolve, reject) => {
        axios
          .put(`/Admin/ConfirmDonePrintJob/${id}`, null, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    createDesign(params) {
      return new Promise((resolve, reject) => {
        axios
          .post(
            "/Admin/CreateDesign",
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
    approvalDesign(params) {
      return new Promise((resolve, reject) => {
        axios
          .put(
            "/Admin/ApprovalDesign",
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
    deleteProject(id) {
      console.log(id);
      return new Promise((resolve, reject) => {
        axios
          .delete(`/Admin/DeleteProject/${id}`, {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getByIdProject(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/Admin/GetProjectById/${id}`, {
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
          .get("/Admin/GetAllProject", {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
            params: {
              ProjectName: param.projectName,
              StartDate: param.startDate,
              EndDate: param.endDate,
              LeaderId: param.leaderId,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getAllPrintJobs() {
      return new Promise((resolve, reject) => {
        axios
          .get("/Admin/GetAllPrintJobs", {
            headers: {
              Authorization: `Bearer ${authorization}`,
            },
          })
          .then((res) => resolve(res))
          .catch((error) => reject(error));
      });
    },
    getByIdPrintJobs(id) {
      return new Promise((resolve, reject) => {
        axios
          .get(`/Admin/GetPrintJobById/${id}`, {
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
