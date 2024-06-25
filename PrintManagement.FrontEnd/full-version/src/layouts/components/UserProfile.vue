<script setup>
import { PerfectScrollbar } from "vue3-perfect-scrollbar";
import { initialAbility } from "@/plugins/casl/ability";
import { useAppAbility } from "@/plugins/casl/useAppAbility";

const router = useRouter();
const ability = useAppAbility();
let logoutTimer = null; 
const userData = JSON.parse(localStorage.getItem("userInfo") || "null");
const accessTokenPhien = localStorage.getItem("accessToken");
const logout = () => {
  clearTimeout(logoutTimer); 
  localStorage.removeItem("userInfo");
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
    localStorage.removeItem("pageReloaded");
  router.push("/login");
};
const startSessionTimer = () => {
  logoutTimer = setTimeout(() => {
    logout();
  }, 60 * 60 * 1000); 
};
if (accessTokenPhien) {
  startSessionTimer(); 
}
const userProfileList = [
  { type: "divider" },
  {
    type: "navItem",
    icon: "tabler-user",
    title: "Trang cá nhân",
    to: {
      path: "/pages/user-profile/profile",
    },
  },
  {
    type: "navItem",
    icon: "tabler-settings",
    title: "Cài đặt",
    to: {
      name: "pages-account-settings-tab",
      params: { tab: "account" },
    },
  },
  { type: "divider" },
  {
    type: "navItem",
    icon: "tabler-logout",
    title: "Logout",
    onClick: logout,
  },
];
</script>

<template>
  <VBadge
    dot
    bordered
    location="bottom right"
    offset-x="3"
    offset-y="3"
    color="success"
  >
    <VAvatar
      class="cursor-pointer"
      :color="!(userData && userData.Avatar) ? 'primary' : undefined"
      :variant="!(userData && userData.Avatar) ? 'tonal' : undefined"
    >
      <VImg v-if="userData && userData.Avatar" :src="userData.Avatar" />
      <VIcon v-else icon="tabler-user" />

      <!-- SECTION Menu -->
      <VMenu activator="parent" width="230" location="bottom end" offset="14px">
        <VList>
          <VListItem>
            <template #prepend>
              <VListItemAction start>
                <VBadge
                  dot
                  location="bottom right"
                  offset-x="3"
                  offset-y="3"
                  color="success"
                  bordered
                >
                  <VAvatar
                    :color="
                      !(userData && userData.Avatar) ? 'primary' : undefined
                    "
                    :variant="
                      !(userData && userData.Avatar) ? 'tonal' : undefined
                    "
                  >
                    <VImg
                      v-if="userData && userData.Avatar"
                      :src="userData.Avatar"
                    />
                    <VIcon v-else icon="tabler-user" />
                  </VAvatar>
                </VBadge>
              </VListItemAction>
            </template>

            <VListItemTitle class="font-weight-medium">
              {{ userData.FullName || userData.Email }}
            </VListItemTitle>
            <VListItemSubtitle style="font-size: 11px">{{
              userData.Email
            }}</VListItemSubtitle>
          </VListItem>

          <PerfectScrollbar :options="{ wheelPropagation: false }">
            <template v-for="item in userProfileList" :key="item.title">
              <VListItem
                v-if="item.type === 'navItem'"
                :to="item.to"
                @click="item.onClick && item.onClick()"
              >
                <template #prepend>
                  <VIcon class="me-2" :icon="item.icon" size="22" />
                </template>

                <VListItemTitle>{{ item.title }}</VListItemTitle>

                <template v-if="item.badgeProps" #append>
                  <VBadge v-bind="item.badgeProps" />
                </template>
              </VListItem>

              <VDivider v-else class="my-2" />
            </template>
          </PerfectScrollbar>
        </VList>
      </VMenu>
      <!-- !SECTION -->
    </VAvatar>
  </VBadge>
</template>
