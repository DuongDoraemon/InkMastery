<script setup>
const shortcuts = [
  {
    icon: "tabler-chart-donut",

    title: "Thống kê",
    subtitle: "Statistical",
    to: { name: "dashboards-analytics" },
  },
  {
    icon: "tabler-packages",
    title: "Quản lý kho",
    subtitle: "Inventory Management",
    to: { name: "apps-invoice-list" },
  },
  {
    icon: "tabler-home",
    title: "Trang chủ",
    subtitle: "Home",
    to: { name: "pages-cards-card-basic" },
  },
  {
    icon: "tabler-photo",
    title: "Quản lý dự án",
    subtitle: "Dashboard Analytics",
    to: { name: "pages-dialog-examples" },
  },
  {
    icon: "tabler-settings",
    title: "Cài đặt",
    subtitle: "Account Settings",
    to: {
      name: "pages-account-settings-tab",
      params: { tab: "account" },
    },
  },
  {
    icon: "tabler-tir",
    title: "Quản lý giao hàng",
    subtitle: "Delivery Management",
    to: { name: "apps-roles" },
  },
];
</script>

<template>
  <Shortcuts :shortcuts="shortcuts" />
</template>
