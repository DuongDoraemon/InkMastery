<script setup>
import { useGenerateImageVariant } from "@core/composable/useGenerateImageVariant";
import { VNodeRenderer } from "@layouts/components/VNodeRenderer";
import { themeConfig } from "@themeConfig";
import authV2ForgotPasswordIllustrationDark from "@images/pages/auth-v2-forgot-password-illustration-dark.png";
import authV2ForgotPasswordIllustrationLight from "@images/pages/auth-v2-forgot-password-illustration-light.png";
import authV2MaskDark from "@images/pages/misc-mask-dark.png";
import authV2MaskLight from "@images/pages/misc-mask-light.png";

const authThemeImg = useGenerateImageVariant(
  authV2ForgotPasswordIllustrationLight,
  authV2ForgotPasswordIllustrationDark
);
const authThemeMask = useGenerateImageVariant(authV2MaskLight, authV2MaskDark);
const isPasswordVisible = ref(false);
const isPasswordVisibleOne = ref(false);
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
          <img
            src="../assets/images/logoPrint.png"
            alt=""
            width="150"
            class="mb-3"
          />
          <h5 class="text-h5 mb-3">Cập nhật mật khẩu mới🔒</h5>
          <p class="mb-0">Vui lòng kiểm tra Email để lấy mã xác minh</p>
          <p v-if="countdownFinished">Hết thời gian!</p>
          <p class="mt-4" v-else>
            Thời gian còn lại:
            <span style="color: #c7ff00">{{ seconds }}</span> giây
          </p>
        </VCardText>

        <VCardText>
          <VForm>
            <VRow>
              <VCol cols="12">
                <AppTextField
                  v-model="inputConfirmUpdate.confirmCode"
                  autofocus
                  label="Mã xác minh"
                  type="text"
                />
              </VCol>
              <VCol cols="12">
                <AppTextField
                  autofocus
                  v-model="inputConfirmUpdate.password"
                  label="Mật khẩu mới"
                  :type="isPasswordVisible ? 'text' : 'password'"
                  :append-inner-icon="
                    isPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'
                  "
                  @click:append-inner="isPasswordVisible = !isPasswordVisible"
                />
              </VCol>
              <VCol cols="12">
                <AppTextField
                  autofocus
                  v-model="inputConfirmUpdate.confirmPassword"
                  label="Nhập lại mật khẩu"
                  :type="isPasswordVisibleOne ? 'text' : 'password'"
                  :append-inner-icon="
                    isPasswordVisibleOne ? 'tabler-eye-off' : 'tabler-eye'
                  "
                  @click:append-inner="
                    isPasswordVisibleOne = !isPasswordVisibleOne
                  "
                />
              </VCol>
              <VCol cols="12">
                <VBtn block @click="createNewPassword"> Xác nhận </VBtn>
              </VCol>
              <VCol cols="12">
                <RouterLink
                  class="d-flex align-center justify-center"
                  :to="{ path: '/forgot-password' }"
                >
                  <VIcon icon="tabler-chevron-left" class="flip-in-rtl" />
                  <span>Quay lại quên mật khẩu</span>
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
  </v-snackbar>
</template>
<script>
import { authApi } from "../api/Auth/authApi";
export default {
  data() {
    return {
      text: "",
      snackbar: false,
      loading: false,
      authApi: authApi(),
      inputConfirmUpdate: {
        password: "",
        confirmCode: "",
        confirmPassword: "",
      },
      countdownFinished: false,
      minutes: 0,
      seconds: 0,
      endTime: new Date().getTime() + 60000,
    };
  },
  created() {
    this.updateCountdown();
    this.countdownInterval = setInterval(this.updateCountdown, 1000);
  },

  methods: {
    async createNewPassword() {
      console.log(this.inputConfirmUpdate);
      const result = await this.authApi.confirmCreateNewPassword(
        this.inputConfirmUpdate
      );
      console.log(result);
      if (result) {
        (this.text = result.data), (this.snackbar = true);
        setTimeout(() => {
          this.$router.push({ path: "/login" });
        }, 1500);
      } else {
        (this.text = "Password update failed"), (this.snackbar = true);
      }
    },
    updateCountdown() {
      const currentTime = new Date().getTime();
      const distance = this.endTime - currentTime;

      this.minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
      this.seconds = Math.floor((distance % (1000 * 60)) / 1000);

      if (distance < 0) {
        clearInterval(this.countdownInterval);
        this.countdownFinished = true;
        if (this.$route.path === "/update-password") {
          this.$router.push({ path: "/forgot-password" });
        }
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
