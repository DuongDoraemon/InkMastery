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
})

const emit = defineEmits(["update:currentStep", "update:checkout-data"])

const checkoutAddressDataLocal = ref(props.checkoutData)

console.log(checkoutAddressDataLocal)

const deliveryOptions = [
  {
    icon: { icon: "tabler-user" },
    title: "Standard",
    desc: "Get your product in 1 Week.",
    value: "free",
  },
  {
    icon: { icon: "tabler-crown" },
    title: "Express",
    desc: "Get your product in 3-4 days.",
    value: "express",
  },
  {
    icon: { icon: "tabler-rocket" },
    title: "Overnight",
    desc: "Get your product in 1 day.",
    value: "overnight",
  },
]

const resolveAddressBadgeColor = {
  home: "primary",
  office: "success",
}

const resolveDeliveryBadgeData = {
  free: {
    color: "success",
    price: "Free",
  },
  express: {
    color: "secondary",
    price: 10,
  },
  overnight: {
    color: "secondary",
    price: 15,
  },
}

const updateAddressData = () => {
  emit("update:checkout-data", checkoutAddressDataLocal.value)
}

const nextStep = () => {
  updateAddressData()
  emit("update:currentStep", props.currentStep ? props.currentStep + 1 : 1)
}

watch(() => props.currentStep, updateAddressData)
</script>

<template>
  <VRow>
    <VCol
      cols="12"
      md="8"
    >
      <!-- 👉 Address custom input -->
      <div
        class="mx-auto"
        width="1000"
      >
        <VSlideGroup
          v-model="model"
          class="pa-4"
          selected-class="bg-success"
          show-arrows
        >
          <VSlideGroupItem v-slot="{ toggle }">
            <VCard
              v-for="item in checkoutAddressDataLocal.designs"
              :key="item"
              class="ma-4"
              color="grey-lighten-1"
              height="400"
              width="270"
              @click="toggle"
            >
              <CustomRadios
                v-model:selected-radio="
                  checkoutAddressDataLocal.deliveryAddress
                "
                :radio-content="item"
              />
            </VCard>
          </VSlideGroupItem>
        </VSlideGroup>
      </div>

      <!-- 👉 Add New Address -->

      <VBtn
        variant="tonal"
        class="mt-5 mb-8"
      >
        <label for="fileProject">
          <VIcon
            icon="mdi-folder-upload-outline"
            style="font-size: 25px"
            class="mr-2"
          />
          Tải file
        </label>
      </VBtn>
      <input
        id="fileProject"
        type="file"
        style="display: none"
      >
    </VCol>

    <VCol
      cols="12"
      md="4"
    >
      <VCard
        flat
        variant="outlined"
      >
        <!-- 👉 Delivery estimate date -->
        <VCardText>
          <h6 class="text-base font-weight-medium mb-5">
            Thông tin dự án
          </h6>

          <VList class="card-list bg-var-theme-background rounded pa-5">
            <VListItem>
              <VListItemSubtitle class="text-h6 mb-4">
                Dự án:
                {{ checkoutAddressDataLocal.projectName }}
              </VListItemSubtitle>
              <VListItemSubtitle class="text-h6 mb-4">
                Khách hàng
                {{ checkoutAddressDataLocal.customer }}
              </VListItemSubtitle>
              <VListItemSubtitle class="text-h6 mb-4">
                Quản lý:
                {{ checkoutAddressDataLocal.leader }}
              </VListItemSubtitle>
              <VListItemSubtitle class="text-h6 mb-4">
                Mô tả:
                {{ checkoutAddressDataLocal.description }}
              </VListItemSubtitle>
              <VListItemSubtitle class="text-h6 mb-4">
                Yêu cầu của khách hàng:

                {{ checkoutAddressDataLocal.requestDescriptionFromCustomer }}
              </VListItemSubtitle>
            </VListItem>
          </VList>
        </VCardText>
      </VCard>

      <VBtn
        block
        class="mt-4"
        @click="nextStep"
      >
        In ấn <VIcon
          icon=" mdi-arrow-right"
          class="ml-3"
        />
      </VBtn>
      <VBtn
        block
        variant="outlined"
        class="mt-4"
        @click="nextStep"
      >
        Không phê duyệt
      </VBtn>
    </VCol>
  </VRow>
</template>

<script>
export default {
  data() {
    return {
      model: null,
    }
  },
}
</script>
