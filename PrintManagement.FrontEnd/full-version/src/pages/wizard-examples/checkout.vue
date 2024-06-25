<script setup>
import AddressContent from "@/views/wizard-examples/checkout/Address.vue";
import CartContent from "@/views/wizard-examples/checkout/Cart.vue";
import ConfirmationContent from "@/views/wizard-examples/checkout/Confirmation.vue";
import PaymentContent from "@/views/wizard-examples/checkout/Payment.vue";
import googleHome from "@images/pages/google-home.png";
import iphone11 from "@images/pages/iphone-11.png";

const checkoutSteps = [
  {
    title: "Dự án",
    icon: "custom-trending",
  },
  {
    title: "Thiết kế",
    icon: "custom-address",
  },
  {
    title: "In ấn",
    icon: "custom-payment",
  },
  {
    title: "Giao hàng",
    icon: "custom-cart",
  },
];

// const checkoutData = ref({
//   cartItems: [
//     {
//       id: 1,
//       name: "Google - Google Home - White",
//       seller: "Google",
//       inStock: true,
//       rating: 4,
//       price: 299,
//       discountPrice: 359,
//       image: googleHome,
//       quantity: 1,
//       estimatedDelivery: "18th Nov 2021",
//     },
//     {
//       id: 2,
//       name: "Apple iPhone 11 (64GB, Black)",
//       seller: "Apple",
//       inStock: true,
//       rating: 4,
//       price: 899,
//       discountPrice: 999,
//       image: iphone11,
//       quantity: 1,
//       estimatedDelivery: "20th Nov 2021",
//     },
//   ],
//   promoCode: "",
//   orderAmount: 1198,
//   deliveryAddress: "home",
//   deliverySpeed: "free",
//   deliveryCharges: 0,
//   thietKe: [
//     {
//       src: "https://cms.vietnamreport.net/source/BaoCao/sach_trang_kinh_te_vietnam_2024/files/mobile/1.jpg?240117171048",
//       user: "Nguyễn Bá Quang Huy",
//       time: "29-03-2024",
//       khachHang: "Nguyễn Khánh Huyền",
//       status: "Chờ duyệt",
//       moTa: " In báo cáo thường niên cho năm tài chính 2024, bao gồm các báo cáo tài chính và phân tích hoạt động.",
//       value: 1,
//     },
//     {
//       src: "https://thuthuatnhanh.com/wp-content/uploads/2019/06/anh-anime-girl-xinh-dep-cute-439x580.jpg",
//       user: "Trần Văn Dương",
//       time: "21-04-2024",
//       khachHang: "Thắm Nguyễn",
//       moTa: "In báo cáo thường niên cho năm tài chính 2024, bao gồm các báo cáo tài chính và phân tích hoạt động",
//       status: "Chờ duyệt",
//       value: 2,
//     },
//   ],
// });

const currentStep = ref(0);
</script>

<template>
  <VCard>
    <VCardText>
      <!-- 👉 Stepper -->
      <AppStepper
        v-model:current-step="currentStep"
        class="checkout-stepper"
        :items="checkoutSteps"
        :direction="$vuetify.display.smAndUp ? 'horizontal' : 'vertical'"
      />
    </VCardText>

    <VDivider />

    <VCardText>
      <!-- 👉 stepper content -->
      <VWindow v-model="currentStep" class="disable-tab-transition">
        <VWindowItem>
          <CartContent
            v-model:current-step="currentStep"
            v-model:checkout-data="checkoutData"
          />
        </VWindowItem>

        <VWindowItem>
          <AddressContent
            v-model:current-step="currentStep"
            v-model:checkout-data="checkoutData"
          />
        </VWindowItem>

        <VWindowItem>
          <PaymentContent
            v-model:current-step="currentStep"
            v-model:checkout-data="checkoutData"
          />
        </VWindowItem>

        <VWindowItem>
          <ConfirmationContent v-model:checkout-data="checkoutData" />
        </VWindowItem>
      </VWindow>
    </VCardText>
  </VCard>
</template>

<style lang="scss">
.checkout-stepper {
  .stepper-icon-step {
    .step-wrapper + svg {
      margin-inline: 3.5rem !important;
    }
  }
}
</style>
