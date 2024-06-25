<script setup>
const props = defineProps({
  selectedRadio: {
    type: String,
    required: true,
  },
  radioContent: {
    type: Array,
    required: true,
  },
  gridColumn: {
    type: null,
    required: false,
  },
  checkoutData: {
    type: Array,
    required: true,
  },
});

const emit = defineEmits(["update:selectedRadio"]);

const selectedOption = ref(structuredClone(toRaw(props.selectedRadio)));

watch(selectedOption, () => {
  emit("update:selectedRadio", selectedOption.value);
});
</script>

<template>
  <VRadioGroup>
    <!-- <VRow>
      <VCol v-for="item in props.radioContent" :key="item">
        <VLabel
          class="custom-input custom-radio rounded cursor-pointer"
          :class="selectedOption === item.id ? 'active' : ''"
        >
          <div>
            <VRadio :value="item.id" />
          </div>
          <div class="flex-grow-1">
            <div class="d-flex align-center mb-1">
              <VImg
                :src="item.src"
                style="width: 200px; height: 250px"
                alt="designs"
              />

              <p>
                {{ item.src }}fffff
              </p>
            </div>
            <div class="mt-4">
              <h4 class="mb-2">
                Người tạo:
                {{ item.user }}
              </h4>
              <h4 class="mb-2">
                Ngày tạo:
                {{ item.time }}
              </h4>
              <h4 class="mb-2">
                Trạng thái:
                {{ item.status }}
              </h4>
            </div>
          </div>
        </VLabel>
      </VCol>
    </VRow> -->
  </VRadioGroup>
</template>

<style lang="scss" scoped>
.custom-radio {
  display: flex;
  align-items: flex-start;
  gap: 0.375rem;

  .v-radio {
    margin-block-start: -0.25rem;
  }

  .cr-title {
    font-weight: 500;
  }
}
</style>
