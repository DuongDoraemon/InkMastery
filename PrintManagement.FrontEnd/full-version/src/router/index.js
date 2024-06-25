import { setupLayouts } from "virtual:generated-layouts";
import { createRouter, createWebHistory } from "vue-router";
import routes from "~pages";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      redirect: (to) => {
        const userData = JSON.parse(localStorage.getItem("userInfo") || "{}");
        const permissions =
          userData && userData.permissions ? userData.permissions : [];

        if (permissions.includes("Admin")) {
          location.reload();
          return { name: "dashboards-analytics" };
        }
        if (permissions.includes("Leader")) return { name: "leader-dashboard" };
        // Thêm các điều kiện kiểm tra cho các quyền hạn khác ở đây

        return { name: "login", query: to.query };
      },
    },
    {
      path: "/pages/user-profile",
      redirect: () => ({
        name: "pages-user-profile-tab",
        params: { tab: "profile" },
      }),
    },
    // {
    //   path: "/",
    //   redirect: () => ({
    //     name: "login",
    //     // params: { tab: "profile" },
    //   }),
    // },
    {
      path: "/pages/account-settings",
      redirect: () => ({
        name: "pages-account-settings-tab",
        params: { tab: "account" },
      }),
    },
    {
      path: "/update-password",
      name: "update-password",
      component: () => import("../pages/ConfirmCreateNewPassword.vue"),
    },
    {
      path: "/test",
      name: "test",
      component: () => import("../pages/wizard-examples/test.vue"),
    },
    {
      path: "/wizard-examples/checkout/:id",
      name: "checkout",
      component: () => import("../pages/wizard-examples/checkout.vue"),
    },
    {
      path: "/tables/warehouse",
      // name: "warehouse",
      // component: () => import("../pages/tables/warehouse.vue"),
      redirect: () => ({
        name: "pages-table-warehouse",
      }),
    },
    ...setupLayouts(routes),
  ],
});

// router.beforeEach((to) => {
//   const isLoggedIn = isUserLoggedIn();
//   if (canNavigate(to)) {
//     if (to.meta.redirectIfLoggedIn && isLoggedIn) return "/";
//   } else {
//     if (isLoggedIn) return { name: "not-authorized" };
//     else
//       return {
//         name: "login",
//         query: { to: to.name !== "index" ? to.fullPath : undefined },
//       };
//   }
// });
export default router;
