<script setup>
const props = defineProps({
  currentStep: {
    type: Number,
    required: false,
  },
  checkoutData: {
    type: null,
    required: true,
  },
});

const emit = defineEmits(["update:currentStep", "update:checkout-data"]);

const checkoutCartDataLocal = ref(props.checkoutData);
console.log(checkoutCartDataLocal);
//  cart total
// const totalCost = computed(() => {
//   return (checkoutCartDataLocal.value.orderAmount =
//     checkoutCartDataLocal.value.cartItems.reduce((acc, item) => {
//       return acc + item.price * item.quantity;
//     }, 0));
// });

const updateCartData = () => {
  emit("update:checkout-data", checkoutCartDataLocal);
};

const nextStep = () => {
  // debugger;
  updateCartData();
  emit("update:currentStep", props.currentStep ? props.currentStep + 1 : 1);
};

watch(() => props.currentStep, updateCartData);
</script>

<template>
  <VRow>
    <VCol cols="12" md="8">
      <VCard hover>
        <div>
          <v-row>
            <v-col cols="5">
              <div class="ma-auto pa-5">
                <VImg :src="checkoutCartDataLocal.imageDescription" />
              </div>
            </v-col>

            <VDivider :vertical="$vuetify.display.mdAndUp" />
            <v-col
              ><div>
                <VCardItem>
                  <VCardTitle class="text-h3">
                    {{ checkoutCartDataLocal.projectName }}
                  </VCardTitle>
                </VCardItem>
                <VCardText class="text-subtitle-1">
                  <span>Ngày tạo: </span>
                  <span class="font-weight-medium">
                    {{ formatDate(checkoutCartDataLocal.startDate) }}
                  </span>
                </VCardText>
                <VCardText class="text-subtitle-1">
                  <span>Ngày dự kiến: </span>
                  <span class="font-weight-medium">
                    {{ formatDate(checkoutCartDataLocal.expectedEndDate) }}
                  </span>
                </VCardText>
                <VCardText class="text-subtitle-1">
                  <span style="font-weight: bold"
                    >Yêu cầu của khách hàng:
                  </span>
                  <span class="font-weight-medium">
                    {{ checkoutCartDataLocal.requestDescriptionFromCustomer }}
                  </span>
                </VCardText>
                <VCardText class="text-subtitle-1">
                  <span style="font-weight: bold">Mô tả: </span>
                  <span class="font-weight-medium">
                    {{ checkoutCartDataLocal.description }}
                  </span>
                </VCardText>
              </div></v-col
            >
          </v-row>
        </div>
      </VCard>
    </VCol>

    <VCol cols="12" md="4">
      <VCard flat variant="outlined">
        <!-- 👉 payment offer -->
        <VCardText>
          <h4 class="text-h4 font-weight-medium mb-3">Thông tin dự án</h4>

          <div class="d-flex align-center gap-4"></div>

          <!-- 👉 Gift wrap banner -->
          <div class="bg-var-theme-background rounded pa-5 mt-4">
            <h6 class="text-base font-weight-medium mb-5">
              Người phụ trách: {{ checkoutCartDataLocal.leader }}
            </h6>
            <p>Số điện thoại: {{ checkoutCartDataLocal.phoneLeader }}</p>
            <p>Email: {{ checkoutCartDataLocal.emailLeader }}</p>
          </div>
          <div class="bg-var-theme-background rounded pa-5 mt-4">
            <h6 class="text-base font-weight-medium mb-5">
              Khách hàng: {{ checkoutCartDataLocal.customer }}
            </h6>
            <p>Số điện thoại: {{ checkoutCartDataLocal.phoneCustomer }}</p>
            <p>Email: {{ checkoutCartDataLocal.emailCustomer }}</p>
            <p>Địa chỉ: {{ checkoutCartDataLocal.addressCustomer }}</p>
          </div>
        </VCardText>

        <VDivider />

        <VCardText class="d-flex justify-space-between py-4">
          <h6 class="text-base font-weight-medium">Giá dự án</h6>
          <h6 class="text-base font-weight-medium">
            {{ formatCurrency(checkoutCartDataLocal.startingPrice) }}
          </h6>
        </VCardText>
      </VCard>

      <VBtn block class="mt-4" @click="nextStep">
        Thiết kế <v-icon icon="mdi-arrow-right" class="ml-2"></v-icon>
      </VBtn>
    </VCol>
  </VRow>
</template>
<script>
import { projectApi } from "../../../api/Project/projectApi";
export default {
  data() {
    return {
      projectApi: projectApi(),
      dataGetIdProject: {},
    };
  },
  async mounted() {
    // const id = this.$route.params.id;
    // const res = await this.projectApi.getByIdProject(id);
    // this.dataGetIdProject = res.data;
  },
  methods: {
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
    formatCurrency(value) {
      // Chuyển đổi giá trị sang kiểu số nguyên
      const intValue = parseInt(value);

      // Sử dụng hàm toLocaleString để định dạng giá tiền theo tiêu chuẩn của quốc gia
      return intValue.toLocaleString("vi-VN", {
        style: "currency",
        currency: "VND",
      });
    },
  },
};
</script>
<style lang="scss" scoped>
.checkout-item-remove-btn {
  position: absolute;
  inset-block-start: 10px;
  inset-inline-end: 10px;
}
</style>
