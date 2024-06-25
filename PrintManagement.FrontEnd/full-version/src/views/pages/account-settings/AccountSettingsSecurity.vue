<script setup>
import { VDataTable } from "vuetify/labs/VDataTable";
import {
  confirmPasswordValidator,
  requiredValidator,
  newPasswordValidator,
} from "@validators";
const isCurrentPasswordVisible = ref(false);
const isNewPasswordVisible = ref(false);
const isConfirmPasswordVisible = ref(false);

const refVForm = ref();
const passwordRequirements = [
  "Minimum 8 characters long - the more, the better",
  "At least one lowercase character",
  "At least one number, symbol, or whitespace character",
];

const isOneTimePasswordDialogVisible = ref(false);
</script>

<template>
  <VForm ref="refVForm" @submit.prevent="onSubmit">
    <VRow>
      <VCol cols="12">
        <VCard title="Đổi mật khẩu">
          <VForm>
            <VCardText class="pt-0">
              <!-- 👉 Current Password -->
              <VRow>
                <VCol cols="12" md="6">
                  <!-- 👉 current password -->
                  <AppTextField
                    v-model="inputChange.oldPassword"
                    :rules="[requiredValidator]"
                    :type="isCurrentPasswordVisible ? 'text' : 'password'"
                    :append-inner-icon="
                      isCurrentPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'
                    "
                    label="Mật khẩu cũ"
                    @click:append-inner="
                      isCurrentPasswordVisible = !isCurrentPasswordVisible
                    "
                  />
                </VCol>
              </VRow>

              <!-- 👉 New Password -->
              <VRow>
                <VCol cols="12" md="6">
                  <!-- 👉 new password -->
                  <AppTextField
                    v-model="inputChange.newPassword"
                    :rules="[requiredValidator]"
                    :type="isNewPasswordVisible ? 'text' : 'password'"
                    :append-inner-icon="
                      isNewPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'
                    "
                    label="Mật khẩu mới"
                    @click:append-inner="
                      isNewPasswordVisible = !isNewPasswordVisible
                    "
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <!-- 👉 confirm password -->
                  <AppTextField
                    v-model="inputChange.confirmPassword"
                    :rules="[requiredValidator]"
                    :type="isConfirmPasswordVisible ? 'text' : 'password'"
                    :append-inner-icon="
                      isConfirmPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'
                    "
                    label="Nhập lại mật khẩu"
                    @click:append-inner="
                      isConfirmPasswordVisible = !isConfirmPasswordVisible
                    "
                  />
                </VCol>
              </VRow>
            </VCardText>
            <VCardText class="d-flex flex-wrap gap-4">
              <VBtn type="submit" @click="changePassword">Cập nhật</VBtn>

              <VBtn type="reset" color="secondary" variant="tonal">
                Làm mới
              </VBtn>
            </VCardText>
          </VForm>
        </VCard>
      </VCol>
    </VRow>
  </VForm>
  <v-snackbar v-model="snackbar" color="blue-grey" rounded="pill" class="mb-5">
    {{ text }}
    <template v-slot:actions>
      <v-btn color="green" variant="text" @click="snackbar = false">
        Đóng
      </v-btn>
    </template>
    <!-- <template v-slot:activator="{ props }">
        <v-btn class="ma-2" color="blue-grey" rounded="pill" v-bind="props"
          >open</v-btn
        >
      </template> -->
  </v-snackbar>
</template>
<script>
import { authApi } from "../../../api/Auth/authApi";
export default {
  data() {
    return {
      text: "",
      snackbar: false,
      loading: false,
      authApi: authApi(),
      inputChange: {
        oldPassword: "",
        newPassword: "",
        confirmPassword: "",
      },
    };
  },
  methods: {
    async changePassword() {
      const changePass = await this.authApi.changePassword(this.inputChange);

      (this.text = "Updated password successfully"), (this.snackbar = true);
      // } else {
      //   (this.text = "Password update failed"), (this.snackbar = true);
      // }
    },

    onSubmit() {
      refVForm.value?.validate().then(({ valid: isValid }) => {
        if (isValid) this.changePassword();
      });
    },
  },
};
</script>
<style lang="scss" scoped>
.card-list {
  --v-card-list-gap: 5px;
}

.server-close-btn {
  inset-inline-end: 0.5rem;
}
</style>
