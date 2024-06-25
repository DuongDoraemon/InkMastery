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

const prop = __props;
const checkoutShipingDataLocal = ref(prop.checkoutData);

console.log(checkoutShipingDataLocal);

// const selectedPaymentMethod = ref("card");

// const cardFormData = ref({
//   cardNumber: null,
//   cardName: "",
//   cardExpiry: "",
//   cardCvv: null,
//   isCardSave: true,
// });

// const giftCardFormData = ref({
//   giftCardNumber: null,
//   giftCardPin: null,
// });

// const selectedDeliveryAddress = computed(() => {
//   return checkoutPaymentDataLocal.value.addresses.filter((address) => {
//     return address.value === checkoutPaymentDataLocal.value.deliveryAddress;
//   });
// });

const updateCartData = () => {
  emit("update:checkout-data", checkoutShipingDataLocal.value);
};

const nextStep = () => {
  updateCartData();
  emit("update:currentStep", prop.currentStep ? prop.currentStep + 1 : 1);
};

watch(() => prop.currentStep, updateCartData);
</script>

<template>
  <VRow>
    <VCol cols="12" md="8">
      <!-- 👉 Offers alert -->
      <VAlert color="success" variant="tonal" class="mb-4">
        <template #prepend>
          <VIcon icon="tabler-bookmarks" size="34" />
        </template>
        <VAlertTitle class="text-success mb-3">
          Đơn hàng đã tạo thành công
        </VAlertTitle>

        <p class="mb-1">Đơn hàng đang được nhân viên giao hàng tiếp nhận</p>
      </VAlert>
      <VCard>
        <VTabs
          v-model="tab"
          align-tabs="center"
          bg-color="deep-purple-accent-4"
          stacked
        >
          <VTab value="tab-1">
            <VIcon icon="mdi-phone" />

            Giao hàng
          </VTab>

          <VTab value="tab-2">
            <VIcon icon="mdi-heart" />

            Trạng thái giao hàng
          </VTab>
        </VTabs>

        <VTabsWindow v-model="tab">
          <VTabsWindowItem v-for="i in 3" :key="i" :value="'tab-' + i">
            <VCard>
              <VCardText>{{ text }}</VCardText>
            </VCard>
          </VTabsWindowItem>
        </VTabsWindow>
      </VCard>
    </VCol>

    <VCol cols="12" md="4">
      <VCard flat variant="outlined">
        <VCardText>
          <h6 class="text-base font-weight-medium mb-4">Thông tin đơn hàng</h6>

          <div class="mb-2">
            <span>Mã đơn hàng: </span>
            <span>MMD0903HSDHJDF</span>
          </div>
          <div class="mb-2">
            <span>Tên đơn hàng: </span>
            <span>In ấn bản thảo</span>
          </div>
        </VCardText>

        <VDivider />

        <VCardText>
          <div>
            <div>
              <span class="text-high-emphasis">Khách hàng: </span>

              <span class="me-2">Đoàn Việt Tiến</span>
            </div>
            <div>
              <span class="text-high-emphasis">Số điện thoại: 0312345678</span>
            </div>
            <div />
            <span class="me-2">Địa chỉ: Hà Nội</span>
          </div>
          <div class="d-flex justify-space-between text-base mb-2">
            <span class="text-high-emphasis font-weight-medium"
              >Thành tiền: 500.000 đ</span
            >
          </div>
        </VCardText>
      </VCard>
    </VCol>
  </VRow>
</template>

<script>
export default {
  data() {
    return {
      tab: null,
      text: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
    };
  },
};
</script>
