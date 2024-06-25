<script setup>
import axios from "@axios";
const userData = JSON.parse(localStorage.getItem("userInfo") || "null");

const profileHeaderData = ref();

const fetchHeaderData = () => {
  axios.get("/pages/profile-header").then((response) => {
    profileHeaderData.value = response.data;
  });
};

fetchHeaderData();
</script>

<template>
  <VCard>
    <VImg
      src="https://i.pinimg.com/originals/b0/0d/0c/b00d0cdbf06fab3b2d8cc6847a014495.jpg"
      min-height="125"
      max-height="400"
      cover
    />

    <VCardText
      class="d-flex align-bottom flex-sm-row flex-column justify-center gap-x-5"
    >
      <div class="d-flex h-0">
        <VAvatar
          rounded
          size="120"
          :image="userData.Avatar"
          class="user-profile-avatar mx-auto"
        />
      </div>

      <div class="user-profile-info w-100 mt-16 pt-6 pt-sm-0 mt-sm-0">
        <h6 class="text-h4 text-center text-sm-start font-weight-medium mb-3">
          {{ userData.FullName }}
        </h6>

        <div
          class="d-flex align-center justify-center justify-sm-space-between flex-wrap gap-4"
        >
          <div
            class="d-flex flex-wrap justify-center justify-sm-start flex-grow-1 gap-2"
          >
            {{ userData.Email }}
          </div>
        </div>
      </div>
    </VCardText>
  </VCard>
</template>

<style lang="scss">
.user-profile-avatar {
  border: 5px solid rgb(var(--v-theme-surface));
  background-color: rgb(var(--v-theme-surface)) !important;
  inset-block-start: -3rem;

  .v-img__img {
    border-radius: 0.125rem;
  }
}
</style>
