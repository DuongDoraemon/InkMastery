<script setup>
import ApexChartAreaChart from "@/views/charts/apex-chart/ApexChartAreaChart.vue";
import ApexChartBalance from "@/views/charts/apex-chart/ApexChartBalance.vue";
import ApexChartDailySalesStates from "@/views/charts/apex-chart/ApexChartDailySalesStates.vue";
import ApexChartDataScience from "@/views/charts/apex-chart/ApexChartDataScience.vue";
import ApexChartExpenseRatio from "@/views/charts/apex-chart/ApexChartExpenseRatio.vue";
import ApexChartHorizontalBar from "@/views/charts/apex-chart/ApexChartHorizontalBar.vue";
import ApexChartMobileComparison from "@/views/charts/apex-chart/ApexChartMobileComparison.vue";
import ApexChartNewTechnologiesData from "@/views/charts/apex-chart/ApexChartNewTechnologiesData.vue";
import ApexChartStatistics from "@/views/charts/apex-chart/ApexChartStatistics.vue";
import ApexChartStocksPrices from "@/views/charts/apex-chart/ApexChartStocksPrices.vue";
</script>

<template>
  <div v-if="isLoading" class="text-center mt-15">
    <a-space>
      <a-spin size="large" />
    </a-space>
  </div>
  <div v-else>
    <v-row>
      <v-col cols="12" md="4">
        <v-text-field
          label="Tìm kiếm phòng ban"
          prepend-inner-icon="mdi-magnify"
          variant="outlined"
          hide-details
          v-model="filters.name"
          single-line
        ></v-text-field>
      </v-col>
      <v-col cols="10" md="4">
        <v-btn @click="filter">Tìm kiếm</v-btn>
      </v-col>
      <v-col cols="2" md="4" class="text-right">
        <v-dialog max-width="500">
          <template v-slot:activator="{ props: activatorProps }">
            <v-btn density="comfortable" icon v-bind="activatorProps">
              <v-icon icon="mdi-plus"></v-icon>
              <v-tooltip activator="parent" location="top">
                Thêm phòng ban
              </v-tooltip>
            </v-btn>
          </template>

          <template v-slot:default="{ isActive }">
            <v-card>
              <div class="text-center mb-4 mt-4"><h2>Thêm phòng ban</h2></div>

              <v-form ref="refVForm" @submit.prevent="onSubmit">
                <div class="pa-3">
                  <v-row class="mb-1">
                    <v-col>
                      <span class="obligatory">(*)</span>

                      <v-text-field
                        v-model="inputAddTeam.name"
                        :rules="[requiredValidator]"
                        label="Tên phòng ban"
                        variant="outlined"
                      ></v-text-field>
                    </v-col>
                    <v-col>
                      <span class="obligatory">(*)</span>
                      <v-select
                        v-model="inputAddTeam.managerId"
                        label="Quản lý"
                        :rules="[requiredValidator]"
                        :items="dataUserRoles"
                        clearable
                        item-value="id"
                        item-title="fullName"
                        variant="outlined"
                      ></v-select>
                    </v-col>
                  </v-row>
                  <span class="obligatory">(*)</span>
                  <v-textarea
                    label="Mô tả"
                    variant="outlined"
                    :rules="[requiredValidator]"
                    v-model="inputAddTeam.description"
                  ></v-textarea>
                </div>
                <v-card-actions>
                  <v-spacer></v-spacer>

                  <v-btn
                    text="Thêm mới"
                    variant="flat"
                    @click="createTeam"
                    type="submit"
                  ></v-btn>
                  <v-btn
                    text="Thoát"
                    variant="outlined"
                    @click="isActive.value = false"
                  ></v-btn>
                </v-card-actions>
              </v-form>
            </v-card>
          </template>
        </v-dialog>
      </v-col>
    </v-row>
    <VRow>
      <VCol
        cols="12"
        lg="4"
        md="6"
        v-for="(item, index) in paginatedData"
        :key="index"
      >
        <VCard>
          <v-card-title class="mt-3 mb-2">
            Tên phòng ban: {{ item.name }} ({{ item.description }})
          </v-card-title>
          <VCardText>
            Số thành viên: {{ item.numberOfMember }}
            <v-icon class="ml-1" icon="mdi-account-group"></v-icon>
          </VCardText>

          <VCardText> Quản lý: {{ item.managerName }} </VCardText>
          <v-divider class="mb-3"></v-divider>
          <VCardActions>
            <v-dialog max-width="500">
              <template v-slot:activator="{ props: activatorProps }">
                <VBtn
                  variant="flat"
                  class="mr-2"
                  v-bind="activatorProps"
                  color="success"
                  density="comfortable"
                  @click="getByTeamId(item.id)"
                  icon
                >
                  <v-icon icon="mdi-pencil" style="font-size: 22px"></v-icon>
                  <v-tooltip activator="parent" location="top"
                    >Sửa phòng ban</v-tooltip
                  ></VBtn
                >
              </template>

              <template v-slot:default="{ isActive }">
                <v-card>
                  <div class="text-center mb-4 mt-4">
                    <h2>Sửa phòng ban</h2>
                  </div>

                  <v-form ref="refVForm" @submit.prevent="onSubmit">
                    <div class="pa-3">
                      <v-row class="mb-1">
                        <v-col>
                          <span class="obligatory">(*)</span>

                          <v-text-field
                            label="Tên phòng ban"
                            :rules="[requiredValidator]"
                            v-model="update.name"
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                        <v-col>
                          <span class="obligatory">(*)</span>
                          <v-select
                            v-model="update.managerId"
                            label="Quản lý"
                            :rules="[requiredValidator]"
                            :items="dataUserRoles"
                            item-value="id"
                            item-title="fullName"
                            variant="outlined"
                          ></v-select>
                        </v-col>
                      </v-row>
                      <span class="obligatory">(*)</span>
                      <v-textarea
                        label="Mô tả"
                        v-model="update.description"
                        class="mb-4"
                        variant="outlined"
                        :rules="[requiredValidator]"
                      ></v-textarea>
                    </div>
                    <v-card-actions>
                      <v-spacer></v-spacer>

                      <v-btn
                        text="Cập nhật"
                        variant="flat"
                        type="submit"
                        @click="updateTeam(update.id)"
                      ></v-btn>
                      <v-btn
                        text="Thoát"
                        variant="outlined"
                        @click="isActive.value = false"
                      ></v-btn>
                    </v-card-actions>
                  </v-form>
                </v-card>
              </template>
            </v-dialog>
            <v-dialog max-width="700">
              <template v-slot:activator="{ props: activatorProps }">
                <VBtn
                  variant="flat"
                  class="mr-2"
                  v-if="userManager && salesTeam && salesTeam.name === 'Sales'"
                  v-bind="activatorProps"
                  density="comfortable"
                  @click="getUserTeam(item.id)"
                  icon
                >
                  <v-icon
                    icon="mdi-account-file-text-outline"
                    style="font-size: 22px"
                  ></v-icon>
                  <v-tooltip activator="parent" location="top"
                    >Giao KPI cho nhân viên</v-tooltip
                  ></VBtn
                >
              </template>

              <template v-slot:default="{ isActive }">
                <v-card>
                  <v-form ref="refVForm" @submit.prevent="onSubmit">
                    <div class="pa-3">
                      <div class="text-center mb-4 mt-4">
                        <h2>Giao KPI dự án</h2>
                      </div>
                      <v-row class="mb-1">
                        <v-col cols="6">
                          <span class="obligatory">(*)</span>

                          <v-text-field
                            label="Tên chỉ tiêu"
                            :rules="[requiredValidator]"
                            v-model="inputKpi.IndicatorName"
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                        <v-col cols="6">
                          <span class="obligatory">(*)</span>
                          <v-select
                            clearable
                            v-model="inputKpi.EmployeeId"
                            label="Nhân viên"
                            :rules="[requiredValidator]"
                            :items="dataLocUsers"
                            item-value="id"
                            item-title="fullName"
                            variant="outlined"
                          >
                          </v-select>
                        </v-col>
                        <v-col cols="6">
                          <span class="obligatory">(*)</span>
                          <v-select
                            clearable
                            v-model="inputKpi.Period"
                            label="Thời gian làm"
                            :rules="[requiredValidator]"
                            :items="dataPeriod"
                            item-value="value"
                            item-title="name"
                            variant="outlined"
                          ></v-select>
                        </v-col>
                        <v-col cols="6">
                          <span class="obligatory">(*)</span>

                          <v-text-field
                            label="Chỉ tiêu"
                            type="number"
                            :rules="[requiredValidator]"
                            v-model="inputKpi.Target"
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                      </v-row>
                    </div>
                    <v-card-actions>
                      <v-spacer></v-spacer>

                      <v-btn
                        text="Giao KPI"
                        variant="flat"
                        type="submit"
                        @click="createKpi"
                      ></v-btn>
                      <v-btn
                        text="Thoát"
                        variant="outlined"
                        @click="isActive.value = false"
                      ></v-btn>
                    </v-card-actions>
                  </v-form>
                </v-card>
              </template>
            </v-dialog>
            <v-dialog max-width="300">
              <template v-slot:activator="{ props: activatorProps }">
                <VBtn
                  variant="flat"
                  class="mr-2"
                  color="error"
                  v-bind="activatorProps"
                  density="comfortable"
                  icon
                >
                  <v-icon
                    icon="mdi-delete-outline"
                    style="font-size: 22px"
                  ></v-icon>
                  <v-tooltip activator="parent" location="top"
                    >Xóa phòng ban</v-tooltip
                  ></VBtn
                >
              </template>

              <template v-slot:default="{ isActive }">
                <v-card class="pa-4 text-center">
                  <h2>Bạn có muốn xóa</h2>
                  <v-card-actions class="mt-4">
                    <div>
                      <v-btn
                        text="Xóa"
                        @click="deleteTeam(item.id)"
                        color="error"
                        variant="flat"
                        class="ml-13"
                      ></v-btn>
                      <v-btn
                        text="Thoát"
                        variant="outlined"
                        @click="isActive.value = false"
                      ></v-btn>
                    </div>
                  </v-card-actions>
                </v-card>
              </template>
            </v-dialog>
          </VCardActions>
        </VCard>
      </VCol>
    </VRow>
    <div class="text-center mt-5 mb-15">
      <v-pagination
        v-model="currentPage"
        :length="totalPages"
        rounded="circle"
      ></v-pagination>
    </div>
  </div>
  <v-snackbar v-model="snackbar" color="blue-grey" rounded="pill" class="mb-5">
    {{ text }}
    <template v-slot:actions>
      <v-btn color="green" variant="text" @click="snackbar = false">
        Đóng
      </v-btn>
    </template>
  </v-snackbar>
</template>
<script>
import { teamApi } from "../../api/Team/teamApi";
import { userApi } from "../../api/User/userApi";
import {
  emailValidator,
  requiredValidator,
  usernameValidator,
} from "@validators";

export default {
  data() {
    return {
      page: 1,
      text: "",
      inputKpi: {
        EmployeeId: "",
        IndicatorName: "",
        Target: null,
        Period: null,
      },
      dataPeriod: [
        { name: "Theo ngày", value: "Quater" },
        { name: "Theo tháng", value: "Month" },
        { name: "Theo năm", value: "Year" },
      ],
      snackbar: false,
      isLoading: true,
      teamApi: teamApi(),
      userApi: userApi(),
      userManager: false,
      dataUsers: [],
      userById: {},
      salesTeam: null,
      dataLocUsers: [],
      dataTeams: [],
      filters: { name: "" },
      dataUserRoles: [],
      inputAddTeam: {
        name: "",
        description: "",
        managerId: "",
      },
      perPage: 6,
      currentPage: 1,
      update: {},
    };
  },
  async mounted() {
    await this.CheckManager();
    await this.getDataUserById();
    await this.getDataTeams();
  },
  methods: {
    async getByTeamId(id) {
      const getById = await this.teamApi.getTeamById(id);
      this.update = getById.data;
    },
    async CheckManager() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));

      if (userInfo && userInfo.Permission) {
        this.userManager = userInfo.Permission.includes("Manager");
      } else {
        this.userManager = false;
      }
    },
    async createTeam() {
      const res = await this.teamApi.createTeams(this.inputAddTeam);
      // debugger;
      if (res.data.status === 200) {
        setTimeout(() => {
          this.reloadPage();
        }, 2000);
        this.text = res.data.message;
        this.snackbar = true;
      } else {
        this.text = res.data.message;
        this.snackbar = true;
      }
      console.log(res.data.status);
    },
    async getUserTeam(id) {
      const res = await this.teamApi.getTeamUserById(id);
      this.dataLocUsers = res.data;
      console.log(res);
    },
    async deleteTeam(id) {
      const res = await this.teamApi.deleteTeams(id);
      console.log(res);
      if (res.status === 200) {
        this.text = res.data;
        this.snackbar = true;
        setTimeout(() => {
          this.reloadPage();
        }, 2000);
      } else {
        this.text = res.data;
        this.snackbar = true;
      }
    },
    async createKpi() {
      const res = await this.teamApi.createKpiEmployee(
        this.inputKpi.EmployeeId,
        this.inputKpi.IndicatorName,
        this.inputKpi.Period,
        this.inputKpi.Target
      );
      if (res.data.status === 200) {
        this.text = res.data.message;
        this.snackbar = true;
      } else {
        this.text = res.data.message;
        this.snackbar = true;
      }
    },
    async updateTeam(id) {
      const update = await this.teamApi.updateTeams(id, this.update);
      if (update.data.status === 200) {
        this.text = update.data.message;
        this.snackbar = true;
        setTimeout(() => {
          this.reloadPage();
        }, 2000);
      } else {
        this.text = update.data.message;
        this.snackbar = true;
      }
    },
    onSubmit() {
      refVForm.value?.validate().then(({ valid: isValid }) => {
        if (isValid) login();
      });
    },
    async getUserRole() {
      const res = await this.userApi.getAllUseHaveRoleManager();
      this.dataUserRoles = res.data;
      console.log(this.dataUserRoles);
    },
    async filter() {
      const res = await this.teamApi.filterTeam(this.filters);
      this.dataTeams = res.data;
    },
    async getDataUser() {
      try {
        const dataUser = await this.userApi.getAllUsers();
        this.dataUsers = dataUser.data;
        console.log(this.salesTeam);
      } catch (e) {
        console.error("Error fetching data:", error);
      }
    },
    async getDataUserById() {
      try {
        const userId = JSON.parse(localStorage.getItem("userInfo"));
        const dataUser = await this.userApi.getUserById(userId.Id);
        this.userById = dataUser.data;
      } catch (e) {
        console.error("Error fetching data:", error);
      }
    },
    reloadPage() {
      location.reload();
    },
    async getDataTeams() {
      this.isLoading = true;
      try {
        const data = await this.teamApi.getAllTeams();
        this.dataTeams = data.data;
        this.salesTeam = this.dataTeams.find((team) => team.name === "Sales");
      } catch (e) {
        console.error("Error fetching data:", error);
      } finally {
        this.isLoading = false;
      }
    },
  },
  created() {
    this.getDataUser();
    this.getUserRole();
    this.filter();
  },
  computed: {
    paginatedData() {
      const start = (this.currentPage - 1) * this.perPage;
      const end = start + this.perPage;
      return this.dataTeams.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.dataTeams.length / this.perPage);
    },
  },
};
</script>
<style lang="scss">
@use "@core/scss/template/libs/apex-chart.scss";

.date-picker-wrapper {
  inline-size: 10.5rem;
}

#apex-chart-wrapper {
  .v-card-item__append {
    padding-inline-start: 0;
  }
}
.obligatory {
  color: rgb(252, 57, 57);
}
</style>
