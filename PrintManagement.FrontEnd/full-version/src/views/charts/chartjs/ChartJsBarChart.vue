<template>
  <BarChart
    style="height: 500px"
    :chart-data="data"
    :chart-options="chartOptions"
  />
</template>

<script>
import { useTheme } from "vuetify";
import { getLatestBarChartConfig } from "@core/libs/chartjs/chartjsConfig";
import BarChart from "@core/libs/chartjs/components/BarChart";
import { thongKeApi } from "@/api/thongKe/thongKeApi";

export default {
  name: "SalaryStatistics",
  components: {
    BarChart,
  },
  props: {
    colors: {
      type: null,
      required: true,
    },
  },
  data() {
    return {
      vuetifyTheme: useTheme(),
      chartOptions: null,
      data: {
        labels: [
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
        datasets: [
          {
            maxBarThickness: 15,
            backgroundColor: this.colors.barChartYellow,
            borderColor: "transparent",
            borderRadius: {
              topRight: 15,
              topLeft: 15,
            },
            data: [],
          },
        ],
      },
    };
  },
  created() {
    this.fetchSalaryStatistics();
  },
  methods: {
    async fetchSalaryStatistics() {
      try {
        const thongKe = thongKeApi();
        const response = await thongKe.thongKeLuong();
        const salaries = response.data;

        // Extract data and labels from the response
        const salaryData = salaries.map((item) => item.salary);
        const labels = salaries.map((item) => `Tháng ${item.month}`);

        // Find the maximum salary for setting y-axis max value
        const maxSalary = Math.max(...salaryData);

        // Update the chart's data
        this.data.labels = labels;
        this.data.datasets[0].data = salaryData;

        // Get the latest chart options and update the y-axis settings
        this.chartOptions = getLatestBarChartConfig(this.vuetifyTheme.current);
        this.chartOptions.scales.y.max = maxSalary;
        this.chartOptions.scales.y.ticks.stepSize = 100000;
        this.chartOptions.scales.y.ticks.callback = (value) => {
          return new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(value);
        };

        console.log(this.data);
      } catch (error) {
        console.error("Error fetching salary statistics:", error);
      }
    },
  },
};
</script>

<style lang="scss">
@use "@core/scss/template/libs/apex-chart.scss";

</style>
