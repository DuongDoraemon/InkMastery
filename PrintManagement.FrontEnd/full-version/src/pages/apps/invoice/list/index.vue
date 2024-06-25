<script setup>
import {
  alphaDashValidator,
  emailValidator,
  requiredValidator,
  phoneNumberValidator,
  usernameValidator,
} from "@validators";
const refVForm = ref();
</script>
<template>
  <div v-if="isLoading" class="text-center mt-15">
    <a-space>
      <a-spin size="large" />
    </a-space>
  </div>
  <div v-else>
    <v-row>
      <v-col cols="12" md="4">
        <v-text-field
          label="Tìm kiếm theo tên"
          prepend-inner-icon="mdi-magnify"
          variant="outlined"
          v-model="filterResource"
          hide-details
          single-line
        ></v-text-field>
      </v-col>
      <v-col>
        <v-btn @click="filter"> Tìm kiếm </v-btn>
      </v-col>
      <v-col class="text-right">
        <v-dialog max-width="500">
          <template v-slot:activator="{ props: activatorProps }">
            <v-btn
              icon="mdi-plus"
              v-if="isAdmin"
              active
              v-bind="activatorProps"
              density="comfortable"
            >
              <v-icon icon="mdi-plus" style="font-size: 20px"></v-icon>
              <v-tooltip activator="parent" location="top"
                >Thêm tài nguyên</v-tooltip
              >
            </v-btn>
          </template>

          <template v-slot:default="{ isActive }">
            <v-card class="pa-4">
              <h2 class="mb-4 text-center">Thêm sản phẩm</h2>
              <v-form ref="refVForm" @submit.prevent="onSubmit">
                <div class="clearfix mb-4 text-center">
                  <div>
                    <v-row>
                      <v-col cols="3">
                        <label
                          for="fileInput"
                          style="
                            border: 1px dashed #dddddd;
                            display: block;
                            border-radius: 6px;
                            width: 100px;
                            height: 100px;
                            cursor: pointer;
                            overflow: hidden;
                          "
                        >
                          <img
                            v-if="imageUrl"
                            :src="imageUrl"
                            style="object-fit: cover; width: 100%; height: 100%"
                            alt="avatar"
                          />
                          <div v-else style="height: 100%">
                            <div
                              class="ant-upload-text"
                              style="
                                height: 100%;
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                flex-direction: column;
                              "
                            >
                              <div style="margin-top: 8px; color: grey">
                                <v-icon
                                  style="font-size: 20px"
                                  icon="mdi-tray-arrow-up"
                                ></v-icon>
                                <p class="mt-1">Tải ảnh lên</p>
                              </div>
                            </div>
                          </div>
                        </label>
                      </v-col>
                      <span class="red">(*)</span>
                      <v-col>
                        <v-text-field
                          type="file"
                          id="fileInput"
                          ref="fileInput"
                          hidden
                          :rules="[requiredValidator]"
                          v-model="inputAddResource.Image"
                          @change="handleImageChange"
                          accept="image/*"
                      /></v-col>
                    </v-row>
                  </div>
                </div>
                <v-row class="mb-4">
                  <v-col cols="12">
                    <span class="red">(*)</span>

                    <v-text-field
                      label="Tên sản phẩm"
                      :rules="[requiredValidator]"
                      v-model="inputAddResource.ResourceName"
                      variant="outlined"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12">
                    <span class="red">(*)</span>

                    <v-select
                      clearable
                      label="Thể loại kho"
                      :items="dataResourceType"
                      :rules="[requiredValidator]"
                      v-model="inputAddResource.ResourceTypeId"
                      item-value="id"
                      item-title="nameOfResourceType"
                      variant="outlined"
                    ></v-select>
                  </v-col>
                </v-row>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    text="Thêm mới"
                    type="submit"
                    variant="flat"
                    @click="addResource"
                  ></v-btn>
                  <v-btn
                    variant="outlined"
                    text="Thoát"
                    @click="isActive.value = false"
                  ></v-btn>
                </v-card-actions>
              </v-form>
            </v-card>
          </template>
        </v-dialog>
      </v-col>
    </v-row>
    <VRow>
      <!-- 👉 Apple iPhone 11 Pro -->
      <VCol
        sm="4"
        cols="12"
        v-for="(item, index) in paginatedData"
        :key="index"
      >
        <VCard>
          <div
            class="d-flex justify-space-between flex-wrap flex-md-nowrap flex-column flex-md-row"
          >
            <div class="ma-auto pa-5">
              <VImg width="137" height="176" :src="item.image" />
            </div>

            <VDivider :vertical="$vuetify.display.mdAndUp" />

            <div>
              <VCardItem>
                <VCardTitle>{{ item.resourceName }}</VCardTitle>
              </VCardItem>
              <VCardText class="text-subtitle-1">
                <span>Số lượng tồn: </span>
                <span class="font-weight-medium">{{
                  item.availableQuantity
                }}</span>
              </VCardText>
              <VCardText class="text-subtitle-1">
                <span>Loại sản phẩm: </span>
                <span class="font-weight-medium">{{
                  item.resourceTypeName
                }}</span>
              </VCardText>
              <VCardText class="text-subtitle-1">
                <span>Trạng thái: </span>
                <span class="font-weight-medium">{{
                  item.resourceStatus
                }}</span>
              </VCardText>
              <v-dialog max-width="700">
                <template v-slot:activator="{ props: activatorProps }">
                  <v-btn
                    class="ma-3"
                    density="comfortable"
                    v-if="isAdmin"
                    v-bind="activatorProps"
                    @click="inputCreatePropertyResource.resourceId = item.id"
                    icon
                  >
                    <v-icon icon="mdi-plus" style="font-size: 20px"></v-icon>
                    <v-tooltip activator="parent" location="top"
                      >Thêm sản phẩm</v-tooltip
                    >
                  </v-btn>
                </template>
                <template v-slot:default="{ isActive }">
                  <v-form ref="refVForm" @submit.prevent="onSubmit">
                    <v-card class="pa-4">
                      <h2 class="mb-4 text-center">
                        {{ item.resourceName }}
                      </h2>
                      <div class="clearfix mb-4 text-center"></div>
                      <input
                        type="hidden"
                        v-model="inputCreatePropertyResource.resourceId"
                        readonly
                      />
                      <v-row class="mb-4">
                        <v-col cols="6">
                          <span class="red">(*)</span>
                          <v-text-field
                            label="Tên tài nguyên"
                            :rules="[requiredValidator]"
                            v-model="
                              inputCreatePropertyResource.requests[0].name
                            "
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                        <v-col cols="6">
                          <span class="red">(*)</span>
                          <v-text-field
                            label="Giá sản phẩm"
                            type="number"
                            :rules="[requiredValidator]"
                            v-model="
                              inputCreatePropertyResource.requests[0].price
                            "
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                        <v-col cols="12">
                          <span class="red">(*)</span>
                          <v-textarea
                            label="Mô tả"
                            variant="outlined"
                            :rules="[requiredValidator]"
                            v-model="
                              inputCreatePropertyResource.resourcePropertyName
                            "
                          ></v-textarea>
                        </v-col>
                      </v-row>

                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          text="Thêm mới"
                          variant="flat"
                          type="submit"
                          @click="createPropertyInformation"
                        ></v-btn>
                        <v-btn
                          variant="outlined"
                          text="Thoát"
                          @click="isActive.value = false"
                        ></v-btn>
                      </v-card-actions>
                    </v-card>
                  </v-form>
                </template>
              </v-dialog>
              <v-dialog max-width="700">
                <template v-slot:activator="{ props: activatorProps }">
                  <v-btn
                    class="mr-3"
                    color="info"
                    density="comfortable"
                    v-bind="activatorProps"
                    v-if="isAdmin"
                    icon
                    @click="getByIdResource(item.id)"
                  >
                    <v-icon icon="mdi-eye" style="font-size: 20px"></v-icon>
                    <v-tooltip activator="parent" location="top"
                      >Chi tiết tài nguyên</v-tooltip
                    >
                  </v-btn>
                </template>

                <template v-slot:default="{ isActive }">
                  <v-card class="pa-4">
                    <v-table>
                      <thead>
                        <tr>
                          <th class="text-left">Tên tài nguyên</th>
                          <th class="text-left">Giá</th>
                          <th class="text-left">Số lượng</th>
                          <th class="text-left">Thao tác</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="item in dataResourceProperties" :key="item">
                          <td>{{ item.name }}</td>
                          <td>{{ formatCurrency(item.price) }}</td>
                          <td>{{ item.quantity }}</td>
                          <td>
                            <v-dialog max-width="300">
                              <template
                                v-slot:activator="{ props: activatorProps }"
                              >
                                <v-btn
                                  class="ma-3"
                                  density="comfortable"
                                  v-bind="activatorProps"
                                  @click="
                                    inputQuantity.resourcePropertyDetailId =
                                      item.id
                                  "
                                  icon
                                >
                                  <v-icon
                                    icon="mdi-plus"
                                    style="font-size: 20px"
                                  ></v-icon>
                                  <v-tooltip activator="parent" location="top"
                                    >Thêm số lượng</v-tooltip
                                  >
                                </v-btn>
                              </template>

                              <template v-slot:default="{ isActive }">
                                <v-form
                                  ref="refVForm"
                                  @submit.prevent="onSubmit"
                                >
                                  <v-card class="pa-4">
                                    <div
                                      class="clearfix mb-4 text-center"
                                    ></div>
                                    <v-row class="mb-4">
                                      <v-col cols="12">
                                        <input
                                          type="hidden"
                                          v-model="
                                            inputQuantity.resourcePropertyDetailId
                                          "
                                          readonly
                                        />
                                        <span class="red">(*)</span>
                                        <v-text-field
                                          v-model="inputQuantity.quantity"
                                          label="Số lượng"
                                          type="number"
                                          :rules="[
                                            quantityGreaterThanZero,
                                            requiredValidator,
                                          ]"
                                          variant="outlined"
                                        ></v-text-field>
                                      </v-col>
                                    </v-row>

                                    <v-card-actions>
                                      <v-spacer></v-spacer>
                                      <v-btn
                                        text="Thêm mới"
                                        type="submit"
                                        variant="flat"
                                        @click="createQuantity"
                                      ></v-btn>
                                      <v-btn
                                        variant="outlined"
                                        text="Thoát"
                                        @click="isActive.value = false"
                                      ></v-btn>
                                    </v-card-actions>
                                  </v-card>
                                </v-form>
                              </template>
                            </v-dialog>

                            <v-dialog max-width="300">
                              <template
                                v-slot:activator="{ props: activatorProps }"
                              >
                                <v-btn
                                  density="comfortable"
                                  style="font-size: 20px"
                                  v-bind="activatorProps"
                                  color="error"
                                  class="ml-3"
                                  icon
                                >
                                  <v-icon icon="mdi-delete-outline"></v-icon>
                                  <v-tooltip activator="parent" location="top"
                                    >Xóa sản phẩm</v-tooltip
                                  >
                                </v-btn>
                              </template>

                              <template v-slot:default="{ isActive }">
                                <v-card class="pa-4 text-center">
                                  <h2>Bạn có muốn xóa</h2>
                                  <v-card-actions class="mt-4">
                                    <div>
                                      <v-btn
                                        text="Xóa"
                                        @click="deleteCoupon(item.id)"
                                        color="error"
                                        variant="flat"
                                        class="ml-13"
                                      ></v-btn>
                                      <v-btn
                                        text="Thoát"
                                        variant="outlined"
                                        @click="isActive.value = false"
                                      ></v-btn>
                                    </div>
                                  </v-card-actions>
                                </v-card>
                              </template>
                            </v-dialog>
                          </td>
                        </tr>
                      </tbody>
                    </v-table>

                    <v-card-actions>
                      <v-spacer></v-spacer>

                      <v-btn
                        class="mt-4"
                        variant="outlined"
                        text="Thoát"
                        @click="isActive.value = false"
                      ></v-btn>
                    </v-card-actions>
                  </v-card>
                </template>
              </v-dialog>
              <v-dialog max-width="300">
                <template v-slot:activator="{ props: activatorProps }">
                  <v-btn
                    v-if="isAdmin"
                    density="comfortable"
                    style="font-size: 20px"
                    v-bind="activatorProps"
                    color="error"
                    icon
                  >
                    <v-icon icon="mdi-delete-outline"></v-icon>
                    <v-tooltip activator="parent" location="top"
                      >Xóa tài nguyên</v-tooltip
                    >
                  </v-btn>
                </template>

                <template v-slot:default="{ isActive }">
                  <v-card class="pa-4 text-center">
                    <h2>Bạn có muốn xóa</h2>
                    <v-card-actions class="mt-4">
                      <div>
                        <v-btn
                          text="Xóa"
                          @click="deleteResources(item.id)"
                          color="error"
                          variant="flat"
                          class="ml-13"
                        ></v-btn>
                        <v-btn
                          text="Thoát"
                          variant="outlined"
                          @click="isActive.value = false"
                        ></v-btn>
                      </div>
                    </v-card-actions>
                  </v-card>
                </template>
              </v-dialog>
            </div>
          </div>
        </VCard>
      </VCol>
    </VRow>
    <div class="text-center mt-9">
      <v-pagination
        v-model="currentPage"
        :length="totalPages"
        rounded="circle"
      ></v-pagination>
    </div>
  </div>

  <v-snackbar v-model="snackbar" color="blue-grey" rounded="pill" class="mb-5">
    {{ text }}
    <template v-slot:actions>
      <v-btn color="green" variant="text" @click="snackbar = false">
        Đóng
      </v-btn>
    </template>
  </v-snackbar>
</template>
>
<script>
import { resourceTypeApi } from "../../../../api/resource/resourceTypeApi";
import { resourceApi } from "../../../../api/resource/resourceApi";
import { set } from "@vueuse/core";
import { ref } from "vue";
export default {
  data() {
    return {
      resourceTypeApi: resourceTypeApi(),
      resourceApi: resourceApi(),
      dataResourceType: [],
      dataResource: [],
      text: "",
      snackbar: false,
      currentPage: 1,
      isLoading: true,
      refVForm: "",
      perPage: 6,
      dataResourceProperties: [],
      dataById: [],
      imageUrl: "",
      filterResource: "",
      idCoupon: "",
      inputAddResource: {
        ResourceName: "",
        Image: "",
        ResourceTypeId: "",
      },
      inputQuantity: {
        resourcePropertyDetailId: "",
        quantity: null,
      },
      inputCreatePropertyResource: {
        resourceId: "",
        resourcePropertyName: "",
        requests: [
          {
            name: "",
            price: null,
          },
        ],
      },
      page: 1,
    };
  },
  methods: {
    async getAllResourceTypes() {
      const data = await this.resourceTypeApi.getAllsResourceType();
      this.dataResourceType = data.data;
      console.log(this.dataResourceType);
    },
    async filter() {
      const res = await this.resourceApi.fillterData(this.filterResource);
      this.dataResource = res.data;
      console.log(this.dataResource);
    },
    async addResource() {
      try {
        const res = await this.resourceApi.createResouce(this.inputAddResource);
        if (res.data.status === 200) {
          this.text = res.data.message;
          this.snackbar = true;
          setTimeout(() => {
            location.reload();
          }, 1500);
        } else {
          this.text = res.data.message;
          this.snackbar = true;
        }
      } catch (e) {
        this.text = "Lỗi khi thêm dữ liệu";
        this.snackbar = true;
      }
    },
    quantityGreaterThanZero(value) {
      return value > 0 || "Số lượng phải lớn hơn 0";
    },
    async createPropertyInformation() {
      try {
        const res = await this.resourceApi.createResourcePropertyInFormation(
          this.inputCreatePropertyResource
        );
        if (res.data.status === 200) {
          this.text = res.data.message;
          this.snackbar = true;
          setTimeout(() => {
            location.reload();
          }, 1500);
        } else {
          this.text = res.data.message;
          this.snackbar = true;
        }
      } catch (e) {
        this.text = "Lỗi khi thêm dữ liệu";
        this.snackbar = true;
      }
    },
    async getAllsResource() {
      this.isLoading = true;
      try {
        const data = await this.resourceApi.getAllsResource();
        this.dataResource = data.data;
      } catch (e) {
      } finally {
        this.isLoading = false;
      }
    },
    onSubmit() {
      refVForm.value?.validate().then(({ valid: isValid }) => {
        if (isValid) login();
      });
    },
    async getByIdResource(id) {
      try {
        const res = await this.resourceApi.getByIdResource(id);
        this.dataById = res.data;
        console.log(this.dataById);
        const dataPropertyDetails = this.dataById.resourceProperties
          .map((property) => property.resourcePropertyDetails)
          .flat();

        this.dataResourceProperties = dataPropertyDetails;
        console.log(this.dataResourceProperties);
      } catch (error) {
        console.error("Error fetching resource by ID:", error);
      }
    },
    async createQuantity() {
      const res = await this.resourceApi.createImportCoupon(this.inputQuantity);
      if (res.data.status === 200) {
        setTimeout(() => {
          location.reload();
        }, 2000);
        this.text = res.data.message;
        this.snackbar = true;
      } else {
        this.text = res.data.messeage;
        console.log(this.text);
        this.snackbar = true;
      }
      console.log(res);
    },
    async deleteCoupon(id) {
      const res = await this.resourceApi.deleteCoupon(id);
      if (res.data === 200) {
        setTimeout(() => {
          location.reload();
        }, 1500);
        this.text = res.data;
        this.snackbar = true;
      } else {
        this.text = res.data;
        this.snackbar = true;
      }
    },
    async deleteResources(id) {
      try {
        const res = await this.resourceApi.deleteResource(id);
        console.log(res);
        if (res.status === 200) {
          setTimeout(() => {
            location.reload();
          }, 1500);
          this.text = res.data;
          this.snackbar = true;
        } else {
          this.text = res.data;
          this.snackbar = true;
        }
      } catch (e) {
        this.text = "Token bạn đã hết hạn vui lòng đăng nhập lại";
        this.snackbar = true;
      }
    },
    handleImageChange(event) {
      const file = event.target.files[0];
      const maxSizeInBytes = 2 * 1024 * 1024; // 2MB
      const allowedExtensions = [".jpg", ".jpeg", ".png"];

      if (!file) {
        return;
      }

      if (file.size > maxSizeInBytes) {
        this.text = "Kích thước ảnh không được vượt quá 2MB";
        this.snackbar = true;
        return;
      }

      const fileName = file.name;
      const fileExtension = fileName.split(".").pop().toLowerCase();
      if (!allowedExtensions.includes("." + fileExtension)) {
        this.text = "Hệ thống chỉ hỗ trợ file ảnh dạng: jpg, png, jpeg";
        this.snackbar = true;
        return;
      }

      // Lưu file vào inputAddResource
      this.inputAddResource.Image = file;

      // Hiển thị ảnh lên giao diện (nếu cần)
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    },
    formatCurrency(value) {
      const intValue = parseInt(value);
      return intValue.toLocaleString("vi-VN", {
        style: "currency",
        currency: "VND",
      });
    },
  },
  created() {
    this.getAllResourceTypes();
    this.getAllsResource();
    this.filter();
  },
  computed: {
    paginatedData() {
      const start = (this.currentPage - 1) * this.perPage;
      const end = start + this.perPage;
      return this.dataResource.slice(start, end);
    },
    isAdmin() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      return (
        userInfo && userInfo.Permission && userInfo.Permission.includes("Admin")
      );
    },
    totalPages() {
      return Math.ceil(this.dataResource.length / this.perPage);
    },
  },
};
</script>
<style lang="scss" scoped>
.avatar-center {
  position: absolute;
  border: 3px solid rgb(var(--v-theme-surface));
  inset-block-start: -2rem;
  inset-inline-start: 1rem;
}
.text-width {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  width: 270px;
}
// membership pricing
.member-pricing-bg {
  position: relative;
  background-color: rgba(var(--v-theme-on-surface), var(--v-hover-opacity));
}

.membership-pricing {
  sup {
    inset-block-start: 9px;
  }
}
.red {
  color: rgb(253, 75, 75);
}
.v-btn {
  transform: none;
}
.ant-upload-select-picture-card i {
  font-size: 32px;
  color: #999;
}

.ant-upload-select-picture-card .ant-upload-text {
  margin-top: 8px;
  color: #d9d9d9;
}
</style>
