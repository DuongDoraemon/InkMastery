<script setup>
import Shepherd from "shepherd.js";
import axios from "@axios";
import { useThemeConfig } from "@core/composable/useThemeConfig";

const { appContentLayoutNav } = useThemeConfig();

defineOptions({ inheritAttrs: false });

// 👉 Is App Search Bar Visible
const isAppSearchBarVisible = ref(false);

// 👉 Default suggestions
const suggestionGroups = [
  {
    title: "Popular Searches",
    content: [
      {
        icon: "tabler-chart-donut",
        title: "Thống kê",
        url: { name: "dashboards-analytics" },
      },
      {
        title: "Quản lý kho",
        icon: "tabler-packages",
        url: { name: "apps-invoice-list" },
      },
      {
        icon: "tabler-tir",
        title: "Quản lý giao hàng",
        url: { name: "apps-roles" },
      },
      {
        title: "Quản lý dự án",
        icon: "tabler-photo",
        url: { name: "pages-dialog-examples" },
      },
    ],
  },
  {
    title: "Apps & Pages",
    content: [
      {
        title: "Trang chủ",

        icon: "tabler-home",
        url: { name: "pages-cards-card-basic" },
      },
      {
        title: "Quản lý nhân viên",
        icon: "tabler-users-group",

        url: { name: "tables-simple-table" },
      },
      {
        title: "Quản lý khách hàng",
        icon: "tabler-user-shield",
        url: { name: "tables-data-table" },
      },
      {
        title: "Quản lý phòng ban",
        icon: "tabler-brand-teams",
        url: { name: "charts-apex-chart" },
      },
    ],
  },
];

// 👉 No Data suggestion
const noDataSuggestions = [
  {
    title: "Analytics Dashboard",
    icon: "tabler-shopping-cart",
    url: { name: "dashboards-analytics" },
  },
  {
    title: "Account Settings",
    icon: "tabler-user",
    url: {
      name: "pages-account-settings-tab",
      params: { tab: "account" },
    },
  },
  {
    title: "Pricing Page",
    icon: "tabler-cash",
    url: { name: "pages-pricing" },
  },
];

const searchQuery = ref("");
const searchResult = ref([]);
const router = useRouter();

// 👉 fetch search result API
watchEffect(() => {
  axios
    .get("/app-bar/search", { params: { q: searchQuery.value } })
    .then((response) => {
      searchResult.value = response.data;
    });
});

const redirectToSuggestedOrSearchedPage = (selected) => {
  router.push(selected.url);
  isAppSearchBarVisible.value = false;
  searchQuery.value = "";
};

const LazyAppBarSearch = defineAsyncComponent(() =>
  import("@core/components/AppBarSearch.vue")
);
</script>

<template>
  <div
    class="d-flex align-center cursor-pointer"
    v-bind="$attrs"
    style="user-select: none"
    @click="isAppSearchBarVisible = !isAppSearchBarVisible"
  >
    <!-- 👉 Search Trigger button -->
    <!-- close active tour while opening search bar using icon -->
    <IconBtn class="me-1" @click="Shepherd.activeTour?.cancel()">
      <VIcon size="26" icon="tabler-search" />
    </IconBtn>

    <span
      v-if="appContentLayoutNav === 'vertical'"
      class="d-none d-md-flex align-center text-disabled"
      @click="Shepherd.activeTour?.cancel()"
    >
      <span class="me-3">Search</span>
      <span class="meta-key">&#8984;K</span>
    </span>
  </div>

  <!-- 👉 App Bar Search -->
  <LazyAppBarSearch
    v-model:isDialogVisible="isAppSearchBarVisible"
    v-model:search-query="searchQuery"
    :search-results="searchResult"
    :suggestions="suggestionGroups"
    :no-data-suggestion="noDataSuggestions"
    @item-selected="redirectToSuggestedOrSearchedPage"
  >
    <!--
      <template #suggestions>
      use this slot if you want to override default suggestions
      </template>
    -->

    <!--
      <template #noData>
      use this slot to change the view of no data section
      </template>
    -->

    <!--
      <template #searchResult="{ item }">
      use this slot to change the search item
      </template>
    -->
  </LazyAppBarSearch>
</template>

<style lang="scss" scoped>
@use "@styles/variables/_vuetify.scss";

.meta-key {
  border: thin solid rgba(var(--v-border-color), var(--v-border-opacity));
  border-radius: 6px;
  block-size: 1.5625rem;
  line-height: 1.3125rem;
  padding-block: 0.125rem;
  padding-inline: 0.25rem;
}
</style>
