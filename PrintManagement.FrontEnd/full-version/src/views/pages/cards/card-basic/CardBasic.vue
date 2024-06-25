<script setup>
import avatar1 from "@images/avatars/avatar-1.png";
import avatar2 from "@images/avatars/avatar-2.png";
import avatar3 from "@images/avatars/avatar-3.png";
import avatar4 from "@images/avatars/avatar-4.png";
import eCommerce2 from "@images/eCommerce/2.png";
import pages1 from "@images/pages/1.png";
import pages2 from "@images/pages/2.png";
import pages3 from "@images/pages/3.png";
import pages5 from "@images/pages/5.jpg";
import pages6 from "@images/pages/6.jpg";

const avatars = [avatar1, avatar2, avatar3, avatar4];

const isCardDetailsVisible = ref(false);
</script>

<template>
  <div v-if="isLoading" class="text-center mt-15">
    <a-space>
      <a-spin size="large" />
    </a-space>
  </div>
  <div v-else>
    <v-row>
      <v-col cols="12">
        <v-row>
          <v-col cols="12" md="4">
            <v-text-field
              clearable
              label="Tìm kiếm project"
              v-model="fillterProject.projectName"
              prepend-inner-icon="mdi-magnify"
              variant="outlined"
              hide-details
              single-line
            ></v-text-field>
          </v-col>
          <v-col cols="12" md="3">
            <v-select
              clearable
              label="Lọc leader"
              variant="outlined"
              v-model="fillterProject.leaderId"
              item-value="id"
              item-title="fullName"
              :items="dataUser"
            ></v-select>
          </v-col>
          <v-col cols="12" md="2">
            <AppDateTimePicker
              clearable
              :format="dateFormat"
              v-model="fillterProject.startDate"
              placeholder="Ngày bắt đầu"
              prepend-inner-icon="tabler-calendar"
              class="date-picker-input"
            >
            </AppDateTimePicker>
          </v-col>
          <v-col cols="12" md="2">
            <AppDateTimePicker
              clearable
              :format="dateFormat"
              v-model="fillterProject.endDate"
              prepend-inner-icon="tabler-calendar"
              placeholder="Ngày kết thúc"
              class="date-picker-input"
              :rules="[endDateRule]"
            >
            </AppDateTimePicker>
          </v-col>
          <v-col cols="12" md="1">
            <v-btn @click="fillter">Tìm kiếm</v-btn>
          </v-col>
        </v-row>
      </v-col>
    </v-row>

    <VRow>
      <!-- 👉 Influencing The Influencer -->
      <VCol
        cols="12"
        sm="6"
        md="4"
        v-for="(project, index) in paginatedData"
        :key="index"
      >
        <v-dialog max-width="700">
          <template v-slot:activator="{ props: activatorProps }">
            <VCard v-bind="activatorProps">
              <VImg :src="project.imageDescription" cover height="25em" />

              <VCardItem>
                <VCardTitle>{{ project.projectName }}</VCardTitle>
              </VCardItem>
              <VCardText> Trưởng nhóm: {{ project.leader }} </VCardText>
              <v-card-text>
                Ngày hoàn thành: {{ formatDate(project.actualEndDate) }}
              </v-card-text>
            </VCard>
          </template>

          <template v-slot:default="{ isActive }">
            <v-card class="pa-0">
              <VImg :src="project.imageDescription" cover height="400" />
              <v-card-text>
                <h2>
                  {{ project.projectName }}
                </h2>
              </v-card-text>
              <VCardText
                ><h3>- <b>Trưởng nhóm:</b> {{ project.leader }}</h3>
              </VCardText>
              <VCardText
                ><h3>- <b>Khách hàng:</b> {{ project.customer }}</h3>
              </VCardText>
              <VCardText
                ><b>- Ngày hoàn thành:</b> {{ formatDate(project.startDate) }}
              </VCardText>
              <v-card-text class="text-h6">
                <b>- Yêu cầu của khách hàng:</b>
                {{ project.requestDescriptionFromCustomer }}
              </v-card-text>
              <v-card-text class="text-h6">
                <b>- Mô tả:</b>
                {{ project.description }}
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>

                <v-btn
                  variant="flat"
                  text="Thoát"
                  @click="isActive.value = false"
                ></v-btn>
              </v-card-actions>
            </v-card>
          </template>
        </v-dialog>
      </VCol>
    </VRow>
    <div class="text-center mt-4">
      <v-pagination
        v-model="currentPage"
        :length="totalPages"
        rounded="circle"
      ></v-pagination>
    </div>
  </div>
</template>
<script>
import { projectApi } from "../../../../api/Project/projectApi";
import { userApi } from "../../../../api/User/userApi";
export default {
  data() {
    return {
      page: 1,
      projectApi: projectApi(),
      dataProject: [],
      dataUser: [],
      perPage: 6, // Number of items per page (fixed)
      currentPage: 1, // Current page
      dataProgress: 0,
      userApi: userApi(),
      fillterProject: {
        projectName: "",
        startDate: "",
        endDate: "",
        leaderId: "",
      },
      isLoading: true,
      dateFormat: "YYYY-MM-DD",
    };
  },
  async mounted() {
    const resUser = await this.userApi.getAllUserContainsLeaderRole();
    this.dataUser = resUser.data;
  },
  methods: {
    async fillter() {
      const res = await this.projectApi.fillterData(this.fillterProject);
      this.dataProject = res.data.filter((project) => project.progress === 100);
    },

    formatDate(dateString) {
      const date = new Date(dateString);
      const day = date.getDate();
      const month = date.getMonth() + 1;
      const year = date.getFullYear();
      const hours = date.getHours();
      const minutes = date.getMinutes();
      const seconds = date.getSeconds();
      const formattedDay = day < 10 ? "0" + day : day;
      const formattedMonth = month < 10 ? "0" + month : month;
      const formattedHours = hours < 10 ? "0" + hours : hours;
      const formattedMinutes = minutes < 10 ? "0" + minutes : minutes;
      const formattedSeconds = seconds < 10 ? "0" + seconds : seconds;

      return `${formattedDay}/${formattedMonth}/${year}`;
    },
    async getDataProject() {
      this.isLoading = true;
      try {
        const res = await this.projectApi.getAllsProject();
        this.dataProject = res.data.filter(
          (project) => project.progress === 100
        );
      } catch (e) {
        console.error("fetching data:", e);
      } finally {
        this.isLoading = false;
      }
    },
  },
  created() {
    this.getDataProject();
  },
  computed: {
    paginatedData() {
      const start = (this.currentPage - 1) * this.perPage;
      const end = start + this.perPage;
      return this.dataProject.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.dataProject.length / this.perPage);
    },
    endDateRule() {
      return [
        (v) => !!v || "Ngày kết thúc là bắt buộc",
        (v) => {
          if (!v || !this.fillterProject.startDate) return true;
          const startDate = new Date(this.fillterProject.startDate).getTime();
          const endDate = new Date(v).getTime();
          return (
            endDate > startDate || "Ngày kết thúc phải lớn hơn ngày bắt đầu"
          );
        },
      ];
    },
  },
};
</script>
<style lang="scss" scoped>
.avatar-center {
  position: absolute;
  border: 3px solid rgb(var(--v-theme-surface));
  inset-block-start: -2rem;
  inset-inline-start: 1rem;
}

// membership pricing
.member-pricing-bg {
  position: relative;
  background-color: rgba(var(--v-theme-on-surface), var(--v-hover-opacity));
}

.membership-pricing {
  sup {
    inset-block-start: 9px;
  }
}

.v-btn {
  transform: none;
}
</style>
