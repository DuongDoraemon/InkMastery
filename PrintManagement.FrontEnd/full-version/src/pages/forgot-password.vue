<script setup>
import { useGenerateImageVariant } from "@core/composable/useGenerateImageVariant";
import { VNodeRenderer } from "@layouts/components/VNodeRenderer";
import { themeConfig } from "@themeConfig";
import authV2ForgotPasswordIllustrationDark from "@images/pages/auth-v2-forgot-password-illustration-dark.png";
import authV2ForgotPasswordIllustrationLight from "@images/pages/auth-v2-forgot-password-illustration-light.png";
import authV2MaskDark from "@images/pages/misc-mask-dark.png";
import authV2MaskLight from "@images/pages/misc-mask-light.png";
import {
  emailValidator,
  requiredValidator,
  usernameValidator,
} from "@validators";
const email = ref("");
const refVForm = ref();

const authThemeImg = useGenerateImageVariant(
  authV2ForgotPasswordIllustrationLight,
  authV2ForgotPasswordIllustrationDark
);
const authThemeMask = useGenerateImageVariant(authV2MaskLight, authV2MaskDark);
</script>

<template>
  <VRow class="auth-wrapper bg-surface" no-gutters>
    <VCol lg="8" class="d-none d-lg-flex">
      <div class="position-relative bg-background rounded-lg w-100 ma-8 me-0">
        <div class="d-flex align-center justify-center w-100 h-100">
          <VImg
            max-width="368"
            :src="authThemeImg"
            class="auth-illustration mt-16 mb-2"
          />
        </div>

        <VImg class="auth-footer-mask" :src="authThemeMask" />
      </div>
    </VCol>

    <VCol cols="12" lg="4" class="d-flex align-center justify-center">
      <VCard flat :max-width="500" class="mt-12 mt-sm-0 pa-4">
        <VCardText>
          <!-- <VNodeRenderer :nodes="themeConfig.app.logo" class="mb-6" />
           -->
          <img
            src="../assets/images/logoPrint.png"
            alt=""
            width="150"
            class="mb-3"
          />
          <h5 class="text-h5">Quên mật khẩu? 🔒</h5>
        </VCardText>

        <VCardText>
          <VForm ref="refVForm" @submit.prevent="onSubmit">
            <VRow>
              <!-- email -->
              <VCol cols="12">
                <AppTextField
                  v-model="inputForgotPassword.email"
                  autofocus
                  :rules="[requiredValidator, emailValidator]"
                  label="Email"
                  type="email"
                />
              </VCol>

              <!-- Reset link -->
              <VCol cols="12">
                <VBtn
                  block
                  :loading="loading"
                  type="submit"
                  @click="fogotPassword"
                >
                  Xác nhận
                </VBtn>
              </VCol>

              <!-- back to login -->
              <VCol cols="12">
                <RouterLink
                  class="d-flex align-center justify-center"
                  :to="{ name: 'login' }"
                >
                  <VIcon icon="tabler-chevron-left" class="flip-in-rtl" />
                  <span>Quay lại đăng nhập</span>
                </RouterLink>
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
      </VCard>
    </VCol>
  </VRow>
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
import { authApi } from "../api/Auth/authApi";
export default {
  data() {
    return {
      text: "",
      loading: false,
      snackbar: false,
      authApi: authApi(),
      inputForgotPassword: {
        email: "",
      },
    };
  },
  methods: {
    async fogotPassword() {
      console.log(this.inputForgotPassword);
      const result = await this.authApi.forgotPassword(
        this.inputForgotPassword
      );
      console.log(result);
      if (result.status === 200) {
        (this.text = result.data), (this.snackbar = true);
        setTimeout(() => {
          this.$router.push({ path: "/update-password" });
        }, 2000);
      } else {
        (this.text = "Sending failed"), (this.snackbar = true);
      }
    },
  },
};
</script>
<style lang="scss">
@use "@core/scss/template/pages/page-auth.scss";
</style>

<route lang="yaml">
meta:
  layout: blank
  action: read
  subject: Auth
  redirectIfLoggedIn: true
</route>
