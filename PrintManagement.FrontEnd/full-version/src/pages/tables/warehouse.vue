<script setup>
import { ref } from "vue";
function getBase64(file) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });
}
const previewVisible = ref(false);
const previewImage = ref("");
const previewTitle = ref("");
const fileList = ref([]);
const handleCancel = () => {
  previewVisible.value = false;
  previewTitle.value = "";
};
const handlePreview = async (file) => {
  if (!file.url && !file.preview) {
    file.preview = await getBase64(file.originFileObj);
  }
  previewImage.value = file.url || file.preview;
  previewVisible.value = true;
  previewTitle.value =
    file.name || file.url.substring(file.url.lastIndexOf("/") + 1);
};
</script>
<template>
  <v-row>
    <v-col>
      <v-text-field
        label="Tìm kiếm theo tên"
        prepend-inner-icon="mdi-magnify"
        variant="outlined"
        hide-details
        single-line
      ></v-text-field>
    </v-col>
    <v-col class="text-right">
      <v-dialog max-width="500">
        <template v-slot:activator="{ props: activatorProps }">
          <v-btn
            icon="mdi-plus"
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
            <div class="clearfix mb-4 text-center">
              <a-upload
                v-model:file-list="fileList"
                action="https://www.mocky.io/v2/5cc8019d300000980a055e76"
                list-type="picture-card"
                @preview="handlePreview"
              >
                <div v-if="fileList.length < 1">
                  <plus-outlined />
                  <div style="margin-top: 8px; color: grey">
                    <v-icon
                      style="font-size: 20px"
                      icon="mdi-tray-arrow-up"
                    ></v-icon>
                    <p class="mt-1">Tải ảnh lên</p>
                  </div>
                </div>
              </a-upload>
              <a-modal
                :open="previewVisible"
                :title="previewTitle"
                :footer="null"
                @cancel="handleCancel"
              >
              </a-modal>
            </div>
            <v-row class="mb-4">
              <v-col cols="12">
                <v-text-field
                  label="Tên sản phẩm"
                  variant="outlined"
                ></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-select
                  clearable
                  label="Thể loại kho"
                  :items="dataResourceType"
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
                variant="flat"
                @click="isActive.value = false"
              ></v-btn>
              <v-btn
                variant="outlined"
                text="Thoát"
                @click="isActive.value = false"
              ></v-btn>
            </v-card-actions>
          </v-card>
        </template>
      </v-dialog>
    </v-col>
  </v-row>
  <VRow>
    <!-- 👉 Apple iPhone 11 Pro -->
    <VCol sm="4" cols="12" v-for="item in dataResource" :key="item">
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
              <span class="font-weight-medium">{{ item.resourceStatus }}</span>
            </VCardText>
            <v-dialog max-width="700">
              <template v-slot:activator="{ props: activatorProps }">
                <v-btn
                  class="ma-3"
                  density="comfortable"
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
                        v-model="inputCreatePropertyResource.requests.name"
                        variant="outlined"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="6">
                      <span class="red">(*)</span>
                      <v-text-field
                        label="Giá sản phẩm"
                        type="number"
                        v-model="inputCreatePropertyResource.requests.price"
                        variant="outlined"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12">
                      <span class="red">(*)</span>
                      <v-textarea
                        label="Mô tả"
                        variant="outlined"
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
                      @click="createPropertyInformation"
                    ></v-btn>
                    <v-btn
                      variant="outlined"
                      text="Thoát"
                      @click="isActive.value = false"
                    ></v-btn>
                  </v-card-actions>
                </v-card>
              </template>
            </v-dialog>
            <v-dialog max-width="700">
              <template v-slot:activator="{ props: activatorProps }">
                <v-btn
                  class="mr-3"
                  color="info"
                  density="comfortable"
                  v-bind="activatorProps"
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
                              <v-card class="pa-4">
                                <div class="clearfix mb-4 text-center"></div>
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
                                      :rules="[quantityGreaterThanZero]"
                                      variant="outlined"
                                    ></v-text-field>
                                  </v-col>
                                </v-row>

                                <v-card-actions>
                                  <v-spacer></v-spacer>
                                  <v-btn
                                    text="Thêm mới"
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
                                      @click="deleteCustomer(item.id)"
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
                        @click="deleteCustomer(item.id)"
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
  <div class="text-center mt-6">
    <v-pagination v-model="page" :length="4" rounded="circle"></v-pagination>
  </div>
</template>
>
<script>
import { resourceTypeApi } from "../../api/resource/resourceTypeApi";
import { resourceApi } from "../../api/resource/resourceApi";
export default {
  data() {
    return {
      resourceTypeApi: resourceTypeApi(),
      resourceApi: resourceApi(),
      dataResourceType: [],
      dataResource: [],
      dataResourceProperties: [],
      dataById: [],
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
    quantityGreaterThanZero(value) {
      return value > 0 || "Số lượng phải lớn hơn 0";
    },
    async createPropertyInformation() {
      const res = await this.resourceApi.createResourcePropertyInFormation(
        this.inputCreatePropertyResource
      );
      console.log(res);
    },
    async getAllsResource() {
      const data = await this.resourceApi.getAllsResource();
      this.dataResource = data.data;

      console.log(this.dataResource);
      console.log(this.dataResourceProperties);
    },
    async getByIdResource(id) {
      try {
        const res = await this.resourceApi.getByIdResource(id);
        this.dataById = res.data;
        console.log(res);
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
      console.log(res);
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
  created() {
    this.getAllResourceTypes();
    this.getAllsResource();
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
