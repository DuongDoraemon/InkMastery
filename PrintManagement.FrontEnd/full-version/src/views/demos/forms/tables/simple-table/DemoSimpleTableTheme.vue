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
    <div class="mb-4">
      <v-row>
        <v-col cols="12" md="4">
          <v-text-field
            v-model="filterUser.name"
            label="Tìm kiếm nhân viên"
            prepend-inner-icon="mdi-magnify"
            variant="outlined"
            hide-details
            single-line
          ></v-text-field>
        </v-col>
        <v-col cols="12" md="4">
          <v-select
            clearable
            label="Lọc theo phòng ban"
            v-model="filterUser.teamId"
            item-value="id"
            item-title="name"
            :items="dataTeam"
            variant="outlined"
          >
          </v-select>
        </v-col>
        <v-col cols="12" md="2">
          <v-btn @click="filterUsers">Tìm kiếm</v-btn>
        </v-col>
      </v-row>
    </div>
    <div>
      <v-table>
        <thead>
          <tr>
            <th class="text-left">Họ và tên</th>
            <th class="text-left">Email</th>
            <th class="text-left">Số điện thoại</th>
            <th class="text-left">Nhóm</th>
            <th class="text-left">Thao tác</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(user, index) in paginatedData" :key="index">
            <td>{{ user.fullName }}</td>
            <td>{{ user.email }}</td>
            <td>{{ user.phoneNumber }}</td>
            <td>{{ user.teamName }}</td>
            <td>
              <v-dialog max-width="400">
                <template v-slot:activator="{ props: activatorProps }">
                  <v-btn
                    v-bind="activatorProps"
                    style="font-size: 20px"
                    density="comfortable"
                    @click="findByIdUser(user.id)"
                    icon
                    class="mr-4"
                  >
                    <v-icon icon="tabler-users-group"></v-icon>
                    <v-tooltip activator="parent" location="top">
                      Cập nhật phòng ban nhân viên
                    </v-tooltip>
                  </v-btn>
                </template>

                <template v-slot:default="{ isActive }">
                  <v-card class="pa-4">
                    <div class="text-center mb-4">
                      <h2>Cập nhật phòng ban nhân viên</h2>
                      <h4 class="mt-3 mb-3" style="color: white">
                        Nhân viên: {{ user.fullName }}
                      </h4>
                    </div>
                    <v-select
                      class="mb-6"
                      clearable
                      label="Phòng ban"
                      :items="dataTeam"
                      v-model="updateChangeUser.teamId"
                      item-title="name"
                      item-value="id"
                      variant="outlined"
                    ></v-select>
                    <v-card-actions>
                      <v-spacer></v-spacer>

                      <v-btn
                        text="Cập nhật"
                        variant="flat"
                        @click="capNhatNhanVien"
                      ></v-btn>
                      <v-btn
                        text="Thoát"
                        variant="outlined"
                        @click="isActive.value = false"
                      ></v-btn>
                    </v-card-actions>
                  </v-card>
                </template>
              </v-dialog>
              <v-dialog max-width="400">
                <template v-slot:activator="{ props: activatorProps }">
                  <v-btn
                    v-bind="activatorProps"
                    color="success"
                    style="font-size: 20px"
                    density="comfortable"
                    @click="findByIdUser(user.id)"
                    icon
                    class="mr-4"
                  >
                    <v-icon icon="tabler-user-edit"></v-icon>
                    <v-tooltip activator="parent" location="top">
                      Cập nhật quyền hạn
                    </v-tooltip>
                  </v-btn>
                </template>

                <template v-slot:default="{ isActive }">
                  <v-card class="pa-4">
                    <div class="text-center mb-4">
                      <h2>Cập nhật quyền hạn</h2>
                      <h4 class="mt-3 mb-3" style="color: white">
                        Nhân viên: {{ user.fullName }}
                      </h4>
                    </div>
                    <VSelect
                      class="mb-6"
                      clearable
                      v-model="selectedRoles"
                      label="Quyền hạn"
                      :items="dataRoles"
                      item-title="roleName"
                      :chips="true"
                      multiple
                      v-if="updateUserRoles.length > 0"
                    >
                    </VSelect>
                    <v-card-actions>
                      <v-spacer></v-spacer>

                      <v-btn
                        text="Cập nhật"
                        variant="flat"
                        @click="addRolesToUser(user.id)"
                      ></v-btn>
                      <v-btn
                        text="Thoát"
                        variant="outlined"
                        @click="isActive.value = false"
                      ></v-btn>
                    </v-card-actions>
                  </v-card>
                </template>
              </v-dialog>
              <v-dialog max-width="300">
                <template v-slot:activator="{ props: activatorProps }">
                  <v-btn
                    density="comfortable"
                    style="font-size: 20px"
                    v-bind="activatorProps"
                    color="error"
                    icon
                  >
                    <v-icon icon="mdi-delete-outline"></v-icon>
                    <v-tooltip activator="parent" location="top">
                      Xóa nhân viên
                    </v-tooltip></v-btn
                  >
                </template>

                <template v-slot:default="{ isActive }">
                  <v-card class="pa-4 text-center">
                    <h2>Bạn có muốn xóa</h2>
                    <v-card-actions class="mt-4">
                      <div>
                        <v-btn
                          text="Xóa"
                          :loading="loading"
                          @click="deleteCustomer(item.id)"
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
            </td>
          </tr>
        </tbody>
      </v-table>
    </div>
    <div class="text-center mt-9">
      <v-pagination
        v-model="currentPage"
        :length="totalPages"
        rounded="circle"
      ></v-pagination>
    </div>
    <v-snackbar
      v-model="snackbar"
      color="blue-grey"
      rounded="pill"
      class="mb-5"
    >
      {{ text }}
      <template v-slot:actions>
        <v-btn color="green" variant="text" @click="snackbar = false">
          Đóng
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>
<script>
import { teamApi } from "../../../../../api/Team/teamApi";
import { userApi } from "../../../../../api/User/userApi";
import { authApi } from "../../../../../api/Auth/authApi";
export default {
  data() {
    return {
      userApi: userApi(),
      teamApi: teamApi(),
      authApi: authApi(),
      dataUser: [],
      text: "",
      snackbar: false,
      dataTeam: [],
      selectedRoles: [],
      perPage: 8,
      currentPage: 1, // Current page
      dataRoles: [],
      roleAdd: "",
      updateUser: {},
      updateChangeUser: {
        employeeId: "",
        teamId: "",
      },
      updateUserRoles: [],
      searchQuery: "",
      filterUser: {
        email: "",
        teamId: "",
        name: "",
        phoneNumber: "",
      },
      isLoading: true,
    };
  },
  async mounted() {
    console.log(this.dataUser);

    console.log(this.dataTeam);

    console.log(this.dataRoles);
  },
  methods: {
    async filterUsers() {
      const res = await this.userApi.filterUser(this.filterUser);
      this.dataUser = res.data;
    },
    async findByIdUser(id) {
      try {
        const res = await this.userApi.getUserById(id);
        this.updateUser = res.data;
        this.updateChangeUser.employeeId = this.updateUser.id;
        console.log(this.updateChangeUser.employeeId);
        console.log(this.updateUser);
        const getByRolesUserId = await this.userApi.getRolesByIdUser(id);
        this.updateUserRoles = getByRolesUserId.data;

        this.updateUserRoles.forEach((role) => {
          const foundRole = this.dataRoles.find((item) => item.id === role.id);
          if (foundRole) {
            const test = this.selectedRoles.push(foundRole);
            console.log(test);
          }
        });

        console.log("update");
        console.log(this.updateUserRoles);
      } catch (error) {
        console.error("Error while fetching user data:", error);
      }
    },
    initializeSelectedRoles() {
      this.selectedRoles = this.updateUserRoles.map((role) => role.id);
      this.roleAdd = this.selectedRoles;
    },
    async getDataUser() {
      this.isLoading = true;
      try {
        const dataUser = await this.userApi.getAllUsers();
        this.dataUser = dataUser.data;
      } catch (error) {
        console.error("fetching data failed:", error);
      } finally {
        this.isLoading = false;
      }
    },
    capNhatNhanVien() {
      this.updateChangeManagerUser();
      // this.addRole();
    },
    async updateChangeManagerUser() {
      try {
        const res = await this.userApi.updateChangeTeamForUser(
          this.updateChangeUser
        );
        console.log(res);
        if (res.status === 200) {
          this.text = res.data;
          this.snackbar = true;
          setTimeout(() => {
            location.reload();
          }, 1500);
        } else {
          this.text = res.data;
          this.snackbar = true;
        }
      } catch (e) {
        this.text = "Đã xảy ra lỗi khi cập nhật";
        this.snackbar = true;
      }
    },
    async addRolesToUser(id) {
      try {
        const res = await this.userApi.addRoleUser(id, this.selectedRoles);
        console.log(res);
        if (res.data.status === 200) {
          this.text = res.data.message;
          this.snackbar = true;
        } else {
          this.text = res.data.message;
          this.snackbar = true;
        }
      } catch (error) {
        this.text = "Đã xảy ra lỗi khi cập nhật";
        this.snackbar = true;
        console.error("Error adding roles:", error);
      }
    },
    async getDataTeam() {
      try {
        const dataTeam = await this.teamApi.getAllTeams();
        this.dataTeam = dataTeam.data;
      } catch (error) {
        console.error("fetching data failed:", error);
      }
    },
    async getDataRoles() {
      try {
        const dataRoles = await this.authApi.getAllRoles();
        this.dataRoles = dataRoles.data;
      } catch (error) {
        console.error("fetching data failed:", error);
      }
    },
  },
  created() {
    this.getDataUser();
    this.getDataRoles();
    this.getDataTeam();
    this.initializeSelectedRoles();
  },
  watch: {
    updateUserRoles: {
      handler() {
        this.selectedRoles = this.updateUserRoles;
      },
      immediate: true,
    },
  },
  // watch: {
  //   updateUserRoles: {
  //     handler(newRoles) {
  //       this.selectedRoles = newRoles.map((role) => role.id);

  //     },
  //     immediate: true,
  //   },
  // },
  computed: {
    paginatedData() {
      const start = (this.currentPage - 1) * this.perPage;
      const end = start + this.perPage;
      return this.dataUser.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.dataUser.length / this.perPage);
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
.obligatory {
  color: rgb(255, 96, 96);
}
</style>
