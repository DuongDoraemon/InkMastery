<script setup>
import { VForm } from "vuetify/components/VForm";
import { useAppAbility } from "@/plugins/casl/useAppAbility";
import AuthProvider from "@/views/pages/authentication/AuthProvider.vue";
import axios from "@axios";
import { useGenerateImageVariant } from "@core/composable/useGenerateImageVariant";
import authV2LoginIllustrationBorderedDark from "@images/pages/auth-v2-login-illustration-bordered-dark.png";
import authV2LoginIllustrationBorderedLight from "@images/pages/auth-v2-login-illustration-bordered-light.png";
import authV2LoginIllustrationDark from "@images/pages/auth-v2-login-illustration-dark.png";
import authV2LoginIllustrationLight from "@images/pages/auth-v2-login-illustration-light.png";
import authV2MaskDark from "@images/pages/misc-mask-dark.png";
import authV2MaskLight from "@images/pages/misc-mask-light.png";
import { VNodeRenderer } from "@layouts/components/VNodeRenderer";
import { themeConfig } from "@themeConfig";
import {
  emailValidator,
  requiredValidator,
  usernameValidator,
} from "@validators";

const authThemeImg = useGenerateImageVariant(
  authV2LoginIllustrationLight,
  authV2LoginIllustrationDark,
  authV2LoginIllustrationBorderedLight,
  authV2LoginIllustrationBorderedDark,
  true
);
const authThemeMask = useGenerateImageVariant(authV2MaskLight, authV2MaskDark);
const isPasswordVisible = ref(false);
const route = useRoute();
const router = useRouter();
const ability = useAppAbility();

const errors = ref({
  email: undefined,
  password: undefined,
});

const refVForm = ref();
const email = ref("admin@demo.com");
const password = ref("admin");
const rememberMe = ref(false);
</script>

<template>
  <VRow no-gutters class="auth-wrapper bg-surface">
    <VCol lg="8" class="d-none d-lg-flex">
      <div class="position-relative bg-background rounded-lg w-100 ma-8 me-0">
        <div class="d-flex align-center justify-center w-100 h-100">
          <VImg
            max-width="505"
            :src="authThemeImg"
            class="auth-illustration mt-16 mb-2"
          />
        </div>

        <VImg :src="authThemeMask" class="auth-footer-mask" />
      </div>
    </VCol>

    <VCol
      cols="12"
      lg="4"
      class="auth-card-v2 d-flex align-center justify-center"
    >
      <VCard flat :max-width="500" class="mt-12 mt-sm-0 pa-4">
        <VCardText>
          <img src="../assets/images/logoPrint.png" alt="" width="150" />

          <h5 class="text-h5 mb-1">
            <span class="text-capitalize"> Chào mừng đến với InkMastery </span>!
            👋🏻
          </h5>
          <p class="mb-0">
            Vui lòng đăng nhập vào tài khoản của bạn và bắt đầu cuộc phiêu lưu
          </p>
        </VCardText>
        <!--  -->
        <VCardText>
          <VForm ref="refVForm" @submit.prevent="onSubmit">
            <VRow>
              <!-- email -->
              <VCol cols="12">
                <AppTextField
                  v-model="inputLogin.username"
                  label="Tài khoản"
                  autofocus
                  :rules="[requiredValidator, usernameValidator]"
                  :error-messages="errors.email"
                />
              </VCol>

              <!-- password -->
              <VCol cols="12">
                <AppTextField
                  v-model="inputLogin.password"
                  label="Mật khẩu"
                  :rules="[requiredValidator]"
                  :type="isPasswordVisible ? 'text' : 'password'"
                  :error-messages="errors.password"
                  :append-inner-icon="
                    isPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'
                  "
                  @click:append-inner="isPasswordVisible = !isPasswordVisible"
                />

                <div
                  class="d-flex align-center flex-wrap justify-space-between mt-2 mb-4"
                >
                  <VCheckbox v-model="rememberMe" label="Nhớ mật khẩu" />
                  <RouterLink
                    class="text-primary ms-2 mb-1"
                    :to="{ name: 'forgot-password' }"
                  >
                    Quên mật khẩu?
                  </RouterLink>
                </div>

                <VBtn block type="submit" :loading="loading" @click="login">
                  Đăng nhập
                </VBtn>
                <!-- <VBtn block class="mt-3" @click="logout"> Xóa location </VBtn> -->
              </VCol>

              <!-- create account -->
              <VCol cols="12" class="text-center">
                <span>Bạn chưa có tài khoản?</span>
                <RouterLink
                  class="text-primary ms-2"
                  :to="{ name: 'register' }"
                >
                  Đăng ký tài khoản
                </RouterLink>
              </VCol>
              <VCol cols="12" class="d-flex align-center">
                <VDivider />
                <span class="mx-4">Hoặc</span>
                <VDivider />
              </VCol>

              <!-- auth providers -->
              <VCol cols="12" class="text-center">
                <AuthProvider />
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
      </VCard>
    </VCol>
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
      <!-- <template v-slot:activator="{ props }">
        <v-btn class="ma-2" color="blue-grey" rounded="pill" v-bind="props"
          >open</v-btn
        >
      </template> -->
    </v-snackbar>
  </VRow>
</template>
<script>
import { authApi } from "../api/Auth/authApi";
import { useRoute } from "vue-router";
export default {
  data() {
    return {
      authApi: authApi(),
      loading: false,
      snackbar: false,
      text: "",
      router: useRoute(),
      route: useRouter(),
      inputLogin: {
        username: "",
        password: "",
      },
    };
  },
  methods: {
    async login() {
      this.loading = true;
      try {
        const result = (await this.authApi.login(this.inputLogin)).data;
        console.log(result);
        if (result && result.status === 200) {
          const res = result.data;
          if (!localStorage.getItem("accessToken")) {
            console.log(localStorage.getItem("accessToken"));
            localStorage.setItem("accessToken", res.accessToken);
            localStorage.setItem("refreshToken", res.refreshToken);
            const accessToken = localStorage.getItem("accessToken");
            const decoded = this.parseJwt(res.accessToken);
            console.log(decoded);
            localStorage.setItem("userInfo", JSON.stringify(decoded));
          }

          const userInfo = JSON.parse(localStorage.getItem("userInfo"));
          this.text = result.message;
          if (userInfo && userInfo.Permission) {
            const allRoles = ["Admin", "Leader", "Designer", "Employee"];
            const userRoles = userInfo.Permission;
            let routeName;
            if (
              userRoles.length === allRoles.length ||
              userRoles.includes("Admin")
            ) {
              routeName = "pages-cards-card-basic";
            } else if (userRoles.includes("Employee")) {
              routeName = "pages-dialog-examples";
            } else {
              const toPath = this.$route.query.to;
              routeName = toPath ? toPath : "defaultRouteName";
            }

            // Check if page has been reloaded once
            if (!localStorage.getItem("pageReloaded")) {
              localStorage.setItem("pageReloaded", "true");
              this.$router.push({ name: routeName }).then(() => {
                location.reload();
              });
            } else {
              this.$router.push({ name: routeName });
            }

            this.text = result.message;
            this.snackbar = true;
          }
          this.snackbar = true;
        } else {
          this.text = result.message || "Unknown error occurred";
          this.snackbar = true;
        }
      } catch (error) {
        console.error("Error during login:", error);
        this.text = "Đã xảy ra lỗi trong quá trình đăng nhập";
        this.snackbar = true;
      } finally {
        this.loading = false;
      }
    },
    parseJwt(token) {
      var base64Url = token.split(".")[1];
      var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      var jsonPayload = decodeURIComponent(
        window
          .atob(base64)
          .split("")
          .map(function (c) {
            return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
          })
          .join("")
      );

      return JSON.parse(jsonPayload);
    },
    reloadPage() {
      location.reload();
    },
    onSubmit() {
      refVForm.value?.validate().then(({ valid: isValid }) => {
        if (isValid) login();
      });
    },
    // logout() {
    //   localStorage.removeItem("accessToken");
    //   localStorage.removeItem("refreshToken");
    //   localStorage.removeItem("userInfo");
    //   this.reloadPage();
    // },
  },
  // mounted() {
  //   localStorage.removeItem("userInfo");
  //   localStorage.removeItem("accessToken");
  //   localStorage.removeItem("refreshToken");
  // },
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
