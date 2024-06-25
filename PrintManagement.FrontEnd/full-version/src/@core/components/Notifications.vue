<template>
  <IconBtn id="notification-btn">
    <VBadge
      v-bind="badgeProps"
      :model-value="notifications.some((n) => !n.isSeen)"
      color="error"
      :content="totalUnseenNotifications"
      class="notification-badge"
    >
      <VIcon size="26" icon="tabler-bell" />
    </VBadge>

    <VMenu
      activator="parent"
      width="380px"
      :location="location"
      offset="14px"
      :close-on-content-click="false"
    >
      <VCard class="d-flex flex-column">
        <!-- 👉 Header -->
        <VCardItem class="notification-section">
          <VCardTitle class="text-lg"> Thông báo </VCardTitle>
        </VCardItem>

        <VDivider />

        <!-- 👉 Notifications list -->
        <PerfectScrollbar
          :options="{ wheelPropagation: false }"
          style="max-block-size: 23.75rem"
        >
          <VList class="notification-list rounded-0 py-0">
            <template
              v-for="(item, index) in displayedNotifications"
              :key="item"
            >
              <VDivider v-if="index > 0" />
              <v-dialog max-width="400">
                <template v-slot:activator="{ props: activatorProps }">
                  <VListItem
                    v-bind="activatorProps"
                    link
                    lines="one"
                    min-height="66px"
                    class="list-item-hover-class"
                    @click="updateSeen(item.id)"
                  >
                    <VListItemTitle
                      ><v-icon icon="tabler-mail" class="mr-3"></v-icon
                      >{{ item.content }}</VListItemTitle
                    >

                    <span class="text-xs text-disabled"> </span>

                    <!-- Slot: Append -->
                    <template #append>
                      <div class="d-flex flex-column align-center gap-4">
                        <VBadge
                          dot
                          :color="!item.isSeen ? 'primary' : '#a8aaae'"
                          :class="`${
                            item.isSeen ? 'visible-in-hover' : ''
                          } ms-1`"
                          @click.stop="
                            $emit(item.isSeen ? 'unread' : 'read', [item.id])
                          "
                        />

                        <div style="block-size: 28px; inline-size: 28px">
                          <IconBtn
                            size="small"
                            class="visible-in-hover"
                            @click="$emit('remove', item.id)"
                          >
                            <VIcon size="20" icon="tabler-x" />
                          </IconBtn>
                        </div>
                      </div>
                    </template>
                  </VListItem>
                </template>

                <template v-slot:default="{ isActive }">
                  <v-card>
                    <v-card-text class="text-center text-h4">
                      Thông tin
                    </v-card-text>
                    <v-card-text class="text-center">
                      {{ item.content }}
                    </v-card-text>
                    <v-card-text class="text-center">
                      <router-link to="/apps/roles">
                        <v-btn variant="outlined"
                          >Xem dự án bạn được nhận</v-btn
                        >
                      </router-link>
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
            </template>
          </VList>
        </PerfectScrollbar>
        <VCardActions
          v-show="dataNotification.length && !showAll"
          class="notification-footer"
        >
          <VBtn block @click="showAllNotifications">
            View All Notifications
          </VBtn>
        </VCardActions>
      </VCard>
    </VMenu>
  </IconBtn>
</template>
<script>
import { notificationApi } from "../../api/Notification/notificationApi";
import { PerfectScrollbar } from "vue3-perfect-scrollbar";
import { avatarText } from "@core/utils/formatters";

export default {
  data() {
    return {
      notificationApi: notificationApi(),
      dataNotification: [],
      showAll: false,
      notSeen: null,
    };
  },
  methods: {
    async getDataNotification() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      const res = await this.notificationApi.getAllNotification(userInfo.Id);
      this.dataNotification = res.data;
    },
    async updateSeen(id) {
      const res = await this.notificationApi.updateSeenNotification(id);
      this.notSeen = res.data;
      this.totalUnseenNotifications();
    },
    markAllReadOrUnread() {
      const allNotificationsIds = this.dataNotification.map((item) => item.id);
      const isAllMarkRead = this.dataNotification.some(
        (item) => item.isSeen === false
      );
      if (!isAllMarkRead) this.$emit("unread", allNotificationsIds);
      else this.$emit("read", allNotificationsIds);
    },
    showAllNotifications() {
      this.showAll = true;
    },
  },
  created() {
    this.getDataNotification();
  },

  computed: {
    isAllMarkRead() {
      return this.dataNotification.some((item) => item.isSeen === false);
    },
    totalUnseenNotifications() {
      return this.dataNotification.filter((item) => item.isSeen === false)
        .length;
    },
    displayedNotifications() {
      return this.showAll
        ? this.dataNotification
        : this.dataNotification.slice(0, 5);
    },
  },
  props: {
    notifications: {
      type: Array,
      required: true,
    },
    badgeProps: {
      type: null,
      required: false,
      default: undefined,
    },
    location: {
      type: null,
      required: false,
      default: "bottom end",
    },
  },
  emits: ["read", "unread", "remove", "click:notification"],
};
</script>

<style lang="scss">
.notification-section {
  padding: 14px !important;
}

.notification-footer {
  padding: 6px !important;
}

.list-item-hover-class {
  .visible-in-hover {
    display: none;
  }

  &:hover {
    .visible-in-hover {
      display: block;
    }
  }
}

.notification-list.v-list {
  .v-list-item {
    border-radius: 0 !important;
    margin: 0 !important;
  }
}

// Badge Style Override for Notification Badge
.notification-badge {
  .v-badge__badge {
    /* stylelint-disable-next-line liberty/use-logical-spec */
    min-width: 18px;
    padding: 0;
    block-size: 18px;
  }
}
</style>
