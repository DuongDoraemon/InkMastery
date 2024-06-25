<template>
  <VRow>
    <VCol cols="12">
      <VCard title="Thông tin cá nhân">
        <VCardText class="d-flex">
          <!-- 👉 Avatar -->
          <v-avatar
            ref="imagePreview"
            rounded
            size="100"
            class="me-6"
            :image="accountDataLocal.avatarImg"
          />

          <!-- 👉 Upload Photo -->
          <form class="d-flex flex-column justify-center gap-4">
            <div class="d-flex flex-wrap gap-2">
              <VBtn color="primary">
                <v-icon icon="mdi-tray-arrow-up" class="mr-3" />
                <label for="inputFile"> Sửa hình ảnh </label>
              </VBtn>
              <input
                id="inputFile"
                type="file"
                name="file"
                accept=".jpeg,.png,.jpg,GIF"
                hidden
                @change="handleImageChange"
              />
              <!-- <img
                :src="accountDataLocal.avatarImg"
                alt="Avatar"
                style="max-width: 100px; max-height: 100px"
              /> -->

              <!-- <VBtn
                type="reset"
                color="secondary"
                variant="tonal"
                @click="resetAvatar"
              >
                <span class="d-none d-sm-block">Ảnh mặc định</span>
                <VIcon icon="tabler-refresh" class="d-sm-none" />
              </VBtn> -->
            </div>

            <p class="text-body-1 mb-0">
              Được phép JPG, GIF hoặc PNG. Kích thước tối đa 2MB
            </p>
          </form>
        </VCardText>

        <VDivider />

        <VCardText class="pt-2">
          <!-- 👉 Form -->
          <VForm class="mt-6">
            <VRow>
              <!-- 👉 Username -->
              <VCol md="6" cols="12">
                <AppTextField v-model="UserNames" label="Tài khoản" disabled />
              </VCol>

              <!-- 👉 FullName-->
              <VCol md="6" cols="12">
                <AppTextField v-model="updateUser.fullName" label="Họ và tên" />
              </VCol>

              <!-- 👉 Email -->
              <VCol cols="12" md="6">
                <AppTextField
                  v-model="updateUser.email"
                  label="Email"
                  type="email"
                />
              </VCol>

              <!-- 👉 Date of birth -->
              <VCol cols="12" md="6">
                <AppTextField
                  v-model="updateUser.phoneNumber"
                  label="Số điện thoại"
                />
              </VCol>

              <!-- 👉 phone -->
              <VCol cols="12" md="6">
                <AppDateTimePicker
                  label="Ngày sinh"
                  v-model="updateUser.dateOfBirth"
                  :format="dateFormat"
                  class="date-picker-input"
                />
              </VCol>

              <!-- 👉 Gender -->
              <VCol cols="12" md="6">
                <AppSelect
                  v-model="updateUser.gender"
                  label="Giới tính"
                  :items="currencies"
                  :menu-props="{ maxHeight: 200 }"
                />
              </VCol>

              <!-- 👉 Form Actions -->
              <VCol cols="12" class="d-flex flex-wrap gap-4">
                <VBtn @click="updateUsers">Cập nhật</VBtn>
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
      </VCard>
    </VCol>
  </VRow>

  <!-- Confirm Dialog -->
  <ConfirmDialog
    v-model:isDialogVisible="isConfirmDialogOpen"
    confirmation-question="Are you sure you want to deactivate your account?"
    confirm-title="Deactivated!"
    confirm-msg="Your account has been deactivated successfully."
    cancel-title="Cancelled"
    cancel-msg="Account Deactivation Cancelled!"
  />
  <v-snackbar v-model="snackbar" color="blue-grey" rounded="pill" class="mb-5">
    {{ text }}
    <template v-slot:actions>
      <v-btn color="green" variant="text" @click="snackbar = false">
        Đóng
      </v-btn>
    </template>
  </v-snackbar>
</template>
<script>
import { userApi } from "../../../api/User/userApi";
import avatar1 from "@images/avatars/avatar-14.png";
import { ref } from "vue";
export default {
  computed: {
    dateFormat: "yyyy-MM-dd",
  },
  data() {
    const userData = JSON.parse(localStorage.getItem("userInfo") || "null");
    const accountData = {
      avatarImg: userData.Avatar,
    };
    return {
      usersApi: userApi(),
      updateUser: {},
      text: "",
      snackbar: false,
      accountData,
      UserNames: "",
      refInputEl: null,
      isConfirmDialogOpen: false,
      accountDataLocal: structuredClone(accountData),
      isAccountDeactivated: false,
      currencies: ["Male", "Female", "UnKnown"],
      validateAccountDeactivation: [
        (v) => !!v || "Please confirm account deactivation",
      ],
    };
  },
  async mounted() {
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    this.UserNames = userInfo.UserName;
    const user = await this.usersApi.getUserById(userInfo.Id);
    this.updateUser = user.data;
  },
  methods: {
    handleImageChange(event) {
      const file = event.target.files[0];
      const maxSizeInBytes = 2 * 1024 * 1024; // 2MB
      const allowedExtensions = [".jpg", ".jpeg", ".png"];
      if (!file) {
        return;
      }
      if (file.size > maxSizeInBytes) {
        this.text = "Kích thước ảnh không được vượt quá 2MB";
        this.snackbar = true;
        return;
      }
      const fileName = file.name;
      const fileExtension = fileName.split(".").pop();
      if (!allowedExtensions.includes("." + fileExtension.toLowerCase())) {
        this.text = "Hệ thống chỉ hỗ trợ file ảnh dạng: jpg, png, jpeg";
        this.snackbar = true;
        return;
      }
      const fileReader = new FileReader();
      fileReader.readAsDataURL(file);
      fileReader.onload = () => {
        if (typeof fileReader.result == "string") {
          this.accountDataLocal.avatarImg = fileReader.result;
          this.$refs.imagePreview.src = fileReader.result;
          this.updateUser.avatar = file;
        }
      };
    },

    resetForm() {
      this.accountDataLocal = structuredClone(this.accountData);
    },
    resetAvatar() {
      this.accountDataLocal.avatarImg = this.accountData.avatarImg;
    },
    async updateUsers() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      const update = await this.usersApi.updateUserAccount(
        userInfo.Id,
        this.updateUser
      );

      if (update.data.status === 200) {
        this.text = update.data.message;
        this.snackbar = true;
      } else if (update.data.status === 400) {
        this.text = update.data.message;
        this.snackbar = true;
      } else {
        this.text = "Lỗi hệ thống không cập nhật được!";
        this.snackbar = true;
      }
    },
  },
};
</script>
