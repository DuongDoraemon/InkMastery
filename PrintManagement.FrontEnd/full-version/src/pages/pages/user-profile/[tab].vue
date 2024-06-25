<script setup>
import { useRoute } from "vue-router";
import UserConnections from "@/views/pages/user-profile/connections/index.vue";
import UserProfile from "@/views/pages/user-profile/profile/index.vue";
import UserProjects from "@/views/pages/user-profile/projects/index.vue";
import UserTeam from "@/views/pages/user-profile/team/index.vue";
import UserProfileHeader from "@/views/pages/user-profile/UserProfileHeader.vue";
import ChartJsLineChart from "@/views/charts/chartjs/ChartJsLineChart.vue";
import ApexChartBalance from "@/views/charts/apex-chart/ApexChartBalance.vue";
import ChartJsScatterChart from "@/views/charts/chartjs/ChartJsScatterChart.vue";
import { thongKeApi } from "@/api/thongKe/thongKeApi";
import { ref, onMounted } from "vue";
import congoImg from "@images/illustrations/congo-illustration.png";
import ECharts from "vue-echarts";
import * as echarts from "echarts";
const chartJsCustomColors = {
  white: "#fff",
  yellow: "#ffe802",
  primary: "#836af9",
  areaChartBlue: "#2c9aff",
  barChartYellow: "#ffcf5c",
  polarChartGrey: "#4f5d70",
  polarChartInfo: "#299aff",
  lineChartYellow: "#d4e157",
  polarChartGreen: "#28dac6",
  lineChartPrimary: "#9e69fd",
  lineChartWarning: "#ff9800",
  horizontalBarInfo: "#26c6da",
  polarChartWarning: "#ff8131",
  scatterChartGreen: "#28c76f",
  warningShade: "#ffbd1f",
  areaChartBlueLight: "#84d0ff",
  areaChartGreyLight: "#edf1f4",
  scatterChartWarning: "#ff9f43",
};
const route = useRoute();
const activeTab = ref(route.params.tab);
const thongKeApiInstance = thongKeApi();
const kpiData = ref([]);
const statistics = [
  {
    title: "Sales",
    stats: "230k",
    icon: "tabler-chart-pie-2",
    color: "primary",
  },
  {
    title: "Customers",
    stats: "8.549k",
    icon: "tabler-users",
    color: "info",
  },
  {
    title: "Products",
    stats: "1.423k",
    icon: "tabler-shopping-cart",
    color: "error",
  },
  {
    title: "Revenue",
    stats: "$9745",
    icon: "tabler-currency-dollar",
    color: "success",
  },
];
const chartOption = ref({
  tooltip: {
    trigger: "axis",
  },
  legend: {
    data: ["Lương"],
  },
  toolbox: {
    show: true,
    feature: {
      dataView: { show: true, readOnly: false },
      magicType: { show: true, type: ["line", "bar"] },
      restore: { show: true },
      saveAsImage: { show: true },
    },
  },
  calculable: true,
  xAxis: [
    {
      type: "category",
      data: [
        "Tháng 1",
        "Tháng 2",
        "Tháng 3",
        "Tháng 4",
        "Tháng 5",
        "Tháng 6",
        "Tháng 7",
        "Tháng 8",
        "Tháng 9",
        "Tháng 10",
        "Tháng 11",
        "Tháng 12",
      ],
    },
  ],
  yAxis: [
    {
      type: "value",
    },
  ],
  series: [
    {
      name: "Lương",
      type: "bar",
      data: [10000, 60030],
      markPoint: {
        data: [
          { type: "max", name: "Max" },
          { type: "min", name: "Min" },
        ],
      },
      markLine: {
        data: [{ type: "average", name: "Avg" }],
      },
    },
  ],
});
const salaryData = ref([]);
const kpi = async () => {
  const res = await thongKeApiInstance.getKpiUser();
  kpiData.value = res.data;
  console.log(kpiData.value);
};
const thongKeLuong = async () => {
  try {
    const response = await thongKeApiInstance.thongKeLuong();
    const salesData = response.data;
    salesData.forEach((item) => {
      salaryData.value.push({
        month: item.month,
        salary: item.salary,
      });
    });
    const salarySeriesData = [];
    for (let i = 0; i < 12; i++) {
      const monthSalary = salaryData.value.find((item) => item.month === i + 1);
      if (monthSalary) {
        salarySeriesData.push(monthSalary.salary);
      } else {
        salarySeriesData.push(null);
      }
    }
    chartOption.value.series[0].data = salarySeriesData;
    chartOption.value.xAxis[0].data = [
      "Tháng 1",
      "Tháng 2",
      "Tháng 3",
      "Tháng 4",
      "Tháng 5",
      "Tháng 6",
      "Tháng 7",
      "Tháng 8",
      "Tháng 9",
      "Tháng 10",
      "Tháng 11",
      "Tháng 12",
    ];
  } catch (error) {
    console.error("Error fetching sales data:", error);
  }
};

onMounted(() => {
  thongKeLuong();
  kpi();
});
</script>

<template>
  <div>
    <UserProfileHeader class="mb-5" />
    <VRow id="chartjs-wrapper">
      <div></div>
      <v-col cols="12">
        <v-card color="deep-purple-accent-4" variant="tonal">
          <VRow no-gutters>
            <VCol cols="8">
              <VCardText v-for="item in kpiData" :key="item.id">
                <h6 class="text-lg text-no-wrap font-weight-medium">
                  {{ item.indicatorName }} 🎉
                </h6>
                <!-- <p class="mb-2 text-h5">
                  {{
                    item.achieveKPI == true
                      ? "Chưa hoàn thành KPI "
                      : "Đã hoàn thành KPI 🎉"
                  }}
                </p> -->
                <v-dialog max-width="500">
                  <template v-slot:activator="{ props: activatorProps }">
                    <VBtn class="mt-15" v-bind="activatorProps"
                      >Xem chi tiết</VBtn
                    >
                  </template>

                  <template v-slot:default="{ isActive }">
                    <v-card
                      :title="
                        item.achieveKPI == true
                          ? 'Chưa hoàn thành KPI '
                          : 'Đã hoàn thành KPI 🎉'
                      "
                    >
                      <v-card-text>
                        Chỉ tiêu được đề ra: {{ item.target }} Project
                      </v-card-text>
                      <v-card-text>
                        Đã hoàn thành: {{ item.actuallyAchieved }} Project
                      </v-card-text>
                      <v-card-actions>
                        <v-spacer></v-spacer>

                        <v-btn
                          text="Thoát"
                          variant="flat"
                          @click="isActive.value = false"
                        ></v-btn>
                      </v-card-actions>
                    </v-card>
                  </template>
                </v-dialog>
              </VCardText>
            </VCol>

            <VCol cols="4">
              <VCardText class="pb-0 px-0 position-relative h-100">
                <VImg
                  :src="congoImg"
                  height="147"
                  class="congo-john-img w-100"
                />
              </VCardText>
            </VCol>
          </VRow>
        </v-card>
      </v-col>
      <VCol cols="12" md="12">
        <VCard>
          <v-row>
            <v-col>
              <v-card-title class="text-h3 mt-5"> Thống kê lương </v-card-title>
            </v-col>
          </v-row>

          <div class="heightChart mt-5">
            <ECharts ref="chart" :option="chartOption" autoresize />
          </div>
        </VCard>
      </VCol>
      <VCol cols="12">
        <VCard>
          <v-card-title class="text-h3 mt-5 mb-5">
            Thống kê lương hoa hồng
          </v-card-title>

          <v-row class="pr-4 pl-4">
            <v-col cols="12" sm="6" md="5">
              <div class="date-picker-wrapper mr-2">
                <AppDateTimePicker
                  style="width: 100%"
                  placeholder="Ngày bắt đầu"
                  prepend-inner-icon="tabler-calendar"
                  density="compact"
                  :config="{ position: 'auto right' }"
                />
              </div>
            </v-col>
            <v-col cols="12" sm="6" md="5">
              <div class="date-picker-wrapper mr-2">
                <AppDateTimePicker
                  style="width: 100%"
                  placeholder="Ngày kết thúc"
                  prepend-inner-icon="tabler-calendar"
                  density="compact"
                  :config="{ position: 'auto right' }"
                />
              </div>
            </v-col>
            <v-col
              cols="12"
              sm="12"
              md="2"
              class="d-flex justify-center align-center"
            >
              <v-btn>Thống kê</v-btn>
            </v-col>
          </v-row>

          <VCardText>
            <ChartJsLineChart :colors="chartJsCustomColors" />
          </VCardText>
        </VCard>
      </VCol>

      <!-- 👉 Latest Statistics -->
    </VRow>
  </div>
</template>
<style scoped>
.heightChart {
  height: 570px;
  border-end-end-radius: 5px;
  background: rgb(255, 255, 255);
  padding: 10px;
}
</style>
<route lang="yaml">
meta:
  navActiveLink: pages-user-profile-tab
</route>
