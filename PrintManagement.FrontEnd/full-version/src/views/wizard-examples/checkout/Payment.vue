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
const checkoutPaymentDataLocal = ref(prop.checkoutData);

console.log(checkoutPaymentDataLocal);

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
  emit("update:checkout-data", checkoutPaymentDataLocal.value);
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
      <VRow>
        <VCol cols="6">
          <VLabel class="mb-2"> Mã đơn hàng </VLabel>
          <VTextField label="Mã đơn hàng" />
        </VCol>
        <VCol cols="6">
          <VLabel class="mb-2"> Tên sản phẩm </VLabel>

          <VTextField label="Tên sản phẩm" />
        </VCol>
        <VCol cols="6">
          <VLabel class="mb-2"> Kích thước </VLabel>
          <VTextField label="Kích thước" />
        </VCol>
        <VCol cols="6">
          <VLabel class="mb-2"> Quản lí </VLabel>
          <VTextField label="Quản lí" disabled />
        </VCol>
        <VCol cols="6">
          <VLabel class="mb-2"> Đơn vị tính </VLabel>
          <VTextField label="Đơn vị tính" type="number" />
        </VCol>
        <VCol cols="6">
          <VLabel class="mb-2"> Loại sản phẩm </VLabel>

          <VSelect
            clearable
            label="Loại sản phẩm"
            :items="['Hộp giấy', 'Thiệp cưới', 'Banner', 'Bìa']"
            variant="outlined"
          />
        </VCol>
        <VCol cols="6">
          <VLabel class="mb-2"> Loại giấy </VLabel>
          <VSelect
            clearable
            label="Loại giấy"
            :items="['Cát tông', 'A4', 'Giấy cứng', 'Giấy màu']"
            variant="outlined"
          />
        </VCol>

        <VCol cols="6">
          <VLabel class="mb-2"> Số lượng </VLabel>

          <VTextField label="Số lượng" type="number" />
        </VCol>
        <VCol cols="6">
          <VLabel class="mb-2"> Ngày đặt </VLabel>

          <AppDateTimePicker
            clearable
            placeholder="Ngày đặt"
            class="date-picker-input"
            prepend-inner-icon="tabler-calendar"
          />
        </VCol>
        <VCol cols="6" />
        <VCol>
          <VTextarea label="Ghi chú" variant="outlined" />
        </VCol>
      </VRow>
    </VCol>

    <VCol cols="12" md="4">
      <VCard flat variant="outlined">
        <VCardText>
          <h6 class="text-base font-weight-medium mb-4">Thông tin dự án</h6>

          <div class="d-flex justify-space-between text-base mb-2">
            <span class="text-high-emphasis">Giá dự án</span>
            <span>4.000.090 đ</span>
          </div>
          <div class="d-flex justify-space-between text-base mb-2">
            <span class="text-high-emphasis">Tên dự án</span>
            <span>In ấn bàn thảo</span>
          </div>
        </VCardText>

        <VDivider />

        <VCardText>
          <div class="d-flex justify-space-between text-base mb-2">
            <span class="text-high-emphasis font-weight-medium"
              >Thành tiền:
            </span>
          </div>
          <VBtn block class="mt-4" @click="nextStep">
            Hoàn thành
            <VIcon icon=" mdi-arrow-right" class="ml-3" />
          </VBtn>
        </VCardText>
      </VCard>
    </VCol>
  </VRow>
</template>
