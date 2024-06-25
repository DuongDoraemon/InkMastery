<template>
  <div v-if="isLoading" class="text-center mt-15">
    <a-space>
      <a-spin size="large" />
    </a-space>
  </div>
  <div class="table-giao-hang" v-else>
    <div class="mb-5">
      <v-row>
        <v-col cols="12" md="3">
          <v-select
            clearable
            label="Lọc theo trạng thái"
            :items="dataFilter"
            v-model="selectedStatus"
            @change="handleStatusChange"
            variant="outlined"
          ></v-select>
        </v-col>
      </v-row>
    </div>
    <v-table>
      <thead>
        <tr>
          <th class="text-left">tên đơn hàng</th>
          <th class="text-left">Khách hàng</th>
          <th class="text-left">Địa chỉ</th>
          <th class="text-left">ptvc</th>
          <th class="text-left">Vận chuyển</th>
          <th class="text-left">Trạng thái</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in paginatedData" :key="index">
          <td>{{ item.project.projectName }}</td>
          <td>{{ item.project.customer }}</td>
          <td>{{ item.deliveryAddress }}</td>
          <td>{{ item.shippingMethodName }}</td>
          <td>{{ item.deliver.fullName }}</td>
          <td>
            {{ item.deliveryStatus }}
          </td>
          <td>
            <v-menu>
              <template v-slot:activator="{ props }">
                <v-btn
                  v-if="userById.teamName === 'Delivery'"
                  icon="mdi-dots-vertical"
                  density="comfortable"
                  variant="text"
                  v-bind="props"
                ></v-btn>
              </template>

              <v-list>
                <v-dialog max-width="500">
                  <template v-slot:activator="{ props: activatorProps }">
                    <v-btn v-bind="activatorProps" variant="text"
                      >Xem chi tiết</v-btn
                    >
                  </template>

                  <template v-slot:default="{ isActive }">
                    <v-card>
                      <div class="text-center mt-3">
                        <h2>Chi tiết đơn hàng</h2>
                      </div>
                      <div class="pa-4">
                        <v-list lines="one" class="text-h6">
                          <v-list-item>
                            Mã đơn hàng: {{ item.id }}
                          </v-list-item>
                          <v-list-item>
                            Giá đơn hàng:
                            {{ formatCurrency(item.project.startingPrice) }}
                          </v-list-item>
                          <v-list-item>
                            Tên đơn hàng: {{ item.project.projectName }}
                          </v-list-item>
                          <v-list-item>
                            Khách hàng: {{ item.customer.fullName }}
                          </v-list-item>
                          <v-list-item>
                            Số điện thoại: {{ item.customer.phoneNumber }}
                          </v-list-item>
                          <v-list-item>
                            Địa chỉ giao hàng: {{ item.deliveryAddress }}
                          </v-list-item>
                          <v-list-item>
                            Phương thức vận chuyển:
                            {{ item.shippingMethodName }}
                          </v-list-item>
                          <v-list-item>
                            Người vẫn chuyển: {{ item.deliver.fullName }}
                          </v-list-item>
                          <v-list-item>
                            Trạng thái đơn hàng: {{ item.deliveryStatus }}
                          </v-list-item>
                        </v-list>
                      </div>
                      <div
                        v-if="item.deliveryStatus === 'Delivering'"
                        class="pl-9 pr-9 mb-7"
                      >
                        <v-select
                          label="Xác nhận giao hàng"
                          clearable
                          :items="items"
                          :rules="[requiredValidator]"
                          item-title="label"
                          v-model="inputDevilery.ConfirmStatus"
                          item-value="value"
                          variant="outlined"
                        ></v-select>
                      </div>
                      <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                          variant="flat"
                          v-if="item.deliveryStatus === 'Waiting'"
                          @click="ShipperConfirmOrder(item.id)"
                          >Xác nhận đơn hàng</v-btn
                        >
                        <v-btn
                          v-if="item.deliveryStatus === 'Delivering'"
                          variant="flat"
                          @click="shipDone(item.id)"
                          text="Xác nhận"
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
              </v-list>
            </v-menu>
          </td>
        </tr>
      </tbody>
    </v-table>
    <div class="text-center mt-4">
      <v-pagination
        v-model="currentPage"
        :length="totalPages"
        rounded="circle"
      ></v-pagination>
    </div>
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
    </v-snackbar>
  </div>
</template>
<script>
import { giaoHangApi } from "@/api/giaoHang/giaoHangApi";
import { userApi } from "@/api/User/userApi";

export default {
  data() {
    return {
      giaoHangApi: giaoHangApi(),
      userApi: userApi(),
      dataGiaoHang: [],
      refVForm: "",
      userById: {},
      inputDevilery: {
        DeliveryId: "",
        ConfirmStatus: "",
      },
      dataFilter: ["Delivered", "Delivering", "Waiting"],
      requiredValidator: (value) =>
        !!value || "Vui lòng chọn nhân viên giao hàng",
      isLoading: true,
      text: "",
      snackbar: false,
      idDelivery: {},
      items: [
        {
          value: "NotReceived",
          label: "Không nhận",
        },
        {
          value: "Reject",
          label: "Từ chối",
        },
        {
          value: "Received",
          label: "Đã giao",
        },
      ],
      currentPage: 1,
      perPage: 10,
      selectedStatus: null,
      isConfirmed: false,
    };
  },
  watch: {
    "item.id": function (newVal) {
      this.deliveryId = newVal;
    },
  },
  async mounted() {
    await this.getUserById();
  },
  methods: {
    async getAllGiaoHang() {
      this.isLoading = true;
      try {
        const res = await this.giaoHangApi.getAllGiaoHang();
        this.dataGiaoHang = res.data;
      } catch (error) {
        console.error("Error fetching projects:", error);
      } finally {
        this.isLoading = false;
      }
    },
    handleStatusChange() {
      this.currentPage = 1;
    },
    async getUserById() {
      const idUser = JSON.parse(localStorage.getItem("userInfo"));
      const res = await this.userApi.getUserById(idUser.Id);
      this.userById = res.data;
      console.log(this.userById);
    },
    async ShipperConfirmOrder(deliveryId) {
      try {
        const res = await this.giaoHangApi.shipperOder({ deliveryId });
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
        this.isConfirmed = true;
      } catch (error) {
        console.error("Error in ShipperConfirmOrder:", error);
        this.text = "An error occurred";
        this.snackbar = true;
      }
    },
    formatCurrency(value) {
      const intValue = parseInt(value);
      return intValue.toLocaleString("vi-VN", {
        style: "currency",
        currency: "VND",
      });
    },
    async shipDone(id) {
      try {
        const res = await this.giaoHangApi.shipperConfirm(
          (this.inputDevilery.DeliveryId = id),
          this.inputDevilery.ConfirmStatus
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
      } catch (error) {
        this.text = "Lỗi hệ thống";
        this.snackbar = true;
      }
    },
    onSubmit() {
      this.refVForm.validate().then(({ valid: isValid }) => {
        if (isValid) this.shipDone();
      });
    },
  },
  created() {
    this.getAllGiaoHang();
  },
  computed: {
    filteredData() {
      if (this.selectedStatus) {
        return this.dataGiaoHang.filter(
          (item) => item.deliveryStatus === this.selectedStatus
        );
      }
      return this.dataGiaoHang;
    },
    paginatedData() {
      const start = (this.currentPage - 1) * this.perPage;
      const end = start + this.perPage;
      return this.filteredData.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.filteredData.length / this.perPage);
    },
  },
};
</script>

<style lang="scss">
.text-capitalize {
  text-transform: capitalize;
}

.user-list-name:not(:hover) {
  color: rgba(var(--v-theme-on-background), var(--v-medium-emphasis-opacity));
}
</style>
