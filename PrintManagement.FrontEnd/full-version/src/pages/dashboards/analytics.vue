<template>
  <VRow class="match-height">
    <VCol cols="12" md="3">
      <AppDateTimePicker
        clearable
        :format="dateFormat"
        v-model="inputData.StartTime"
        placeholder="Ngày bắt đầu"
        :rules="[startDateRule]"
        prepend-inner-icon="tabler-calendar"
        class="date-picker-input"
      ></AppDateTimePicker>
    </VCol>
    <VCol cols="12" md="3">
      <AppDateTimePicker
        clearable
        :format="dateFormat"
        v-model="inputData.EndTime"
        placeholder="Ngày kết thúc"
        prepend-inner-icon="tabler-calendar"
        :rules="[endDateRule]"
        class="date-picker-input"
      ></AppDateTimePicker>
    </VCol>
    <VCol cols="12" md="1">
      <v-btn block @click="thongKeSales"
        ><v-icon icon="mdi-filter-outline" class="text-h2"></v-icon>
        <v-tooltip activator="parent" location="top">Thống kê</v-tooltip></v-btn
      >
    </VCol>
    <v-col cols="12">
      <h2 class="mb-4">Thống kê doanh thu</h2>
      <div class="heightChart">
        <ECharts ref="chart" :option="chartOption" autoresize />
      </div>
    </v-col>
  </VRow>
</template>

<script setup>
import { useTheme } from "vuetify";
import { thongKeApi } from "@/api/thongKe/thongKeApi";
import { ref, onMounted } from "vue";
import ECharts from "vue-echarts";
import * as echarts from "echarts";

const vuetifyTheme = useTheme();
const currentTheme = vuetifyTheme.current.value.colors;
const dateFormat = "yyyy-MM-dd";
const thongKeApiInstance = thongKeApi();
const startDateRule = (value) => {
  const startTime = new Date(value);
  const endTime = new Date(inputData.value.EndTime);
  if (endTime && startTime >= endTime) {
    return "Ngày bắt đầu phải nhỏ hơn ngày kết thúc";
  }
  return true;
};

const endDateRule = (value) => {
  const endTime = new Date(value);
  const startTime = new Date(inputData.value.StartTime);
  if (startTime && startTime >= endTime) {
    return "Ngày kết thúc phải lớn hơn ngày bắt đầu";
  }
  return true;
};
const chartOption = ref({
  title: {
    text: "",
  },
  tooltip: {
    trigger: "axis",
  },
  legend: {
    data: ["Doanh thu"],
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
      name: "Doanh thu",
      type: "bar",
      data: [], // Initially empty
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

const salesData = ref([]);
const inputData = ref({ StartTime: "", EndTime: "" });

const thongKeSales = async () => {
  try {
    const params = {
      StartTime: inputData.value.StartTime,
      EndTime: inputData.value.EndTime,
    };

    const response = await thongKeApiInstance.thongKeDoanhSo(params);
    const responseData = response.data;
    console.log(responseData);

    const salesData = ref([]);

    responseData.forEach((item) => {
      salesData.value.push({
        month: item.month,
        sales: item.sales,
      });
    });

    const salesSeriesData = [];
    for (let i = 0; i < 12; i++) {
      const monthSales = salesData.value.find((item) => item.month === i + 1);
      if (monthSales) {
        salesSeriesData.push(monthSales.sales);
      } else {
        salesSeriesData.push(null);
      }
    }

    chartOption.value.series[0].data = salesSeriesData;
  } catch (error) {
    console.error("Error fetching sales data:", error);
  }
};

onMounted(() => {
  thongKeSales();
});
</script>

<style lang="scss">
@use "@core/scss/template/libs/apex-chart.scss";
.heightChart {
  height: 570px;
  border-radius: 5px;
  background: rgb(255, 255, 255);
  padding: 10px;
}
</style>
