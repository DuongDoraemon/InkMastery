<template>
  <div v-if="isLoading" class="text-center mt-15">
    <a-space>
      <a-spin size="large" />
    </a-space>
  </div>
  <div v-else>
    <v-row>
      <v-col cols="12">
        <v-row>
          <v-col cols="12" md="2">
            <v-text-field
              clearable
              label="Tìm kiếm project"
              v-model="fillterProject.projectName"
              prepend-inner-icon="mdi-magnify"
              variant="outlined"
              hide-details
              single-line
            ></v-text-field>
          </v-col>
          <v-col cols="12" md="2">
            <v-select
              clearable
              label="Lọc leader"
              variant="outlined"
              v-model="fillterProject.leaderId"
              item-value="id"
              item-title="fullName"
              :items="dataUser"
            ></v-select>
          </v-col>
          <v-col cols="12" md="2">
            <AppDateTimePicker
              clearable
              :format="dateFormat"
              v-model="fillterProject.startDate"
              placeholder="Ngày bắt đầu"
              prepend-inner-icon="tabler-calendar"
              class="date-picker-input"
              :rules="[startDateRule]"
            >
            </AppDateTimePicker>
          </v-col>
          <v-col cols="12" md="2">
            <AppDateTimePicker
              clearable
              :format="dateFormat"
              v-model="fillterProject.endDate"
              placeholder="Ngày kết thúc"
              class="date-picker-input"
              prepend-inner-icon="tabler-calendar"
              :rules="[endDateRule]"
            >
            </AppDateTimePicker>
          </v-col>
          <v-col cols="12" md="2">
            <v-select
              clearable
              label="Lọc tiến độ"
              variant="outlined"
              v-model="tienDo"
              item-value="value"
              item-title="name"
              :items="dataTienDo"
              @change="currentPage = 1"
            ></v-select>
          </v-col>
          <v-col cols="10" md="1">
            <v-btn @click="fillter">Tìm kiếm</v-btn>
          </v-col>
          <v-col cols="2" md="1" class="text-right">
            <v-dialog max-width="700" persistent>
              <template v-slot:activator="{ props: activatorProps }">
                <v-btn
                  icon
                  v-if="userCheckRole.teamName === 'Sales' && userEmployee"
                  v-bind="activatorProps"
                  density="comfortable"
                  variant="flat"
                >
                  <v-icon icon="mdi-plus"></v-icon>
                  <v-tooltip activator="parent" location="top">
                    Thêm dự án
                  </v-tooltip>
                </v-btn>
              </template>

              <template v-slot:default="{ isActive }">
                <v-card>
                  <h2 class="text-center mt-3">Thêm dự án</h2>
                  <v-form ref="refVForm" @submit.prevent="onSubmit">
                    <div class="pa-4">
                      <v-row class="mb-5">
                        <v-col cols="12" md="3">
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
                              style="
                                object-fit: cover;
                                width: 100%;
                                height: 100%;
                              "
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
                                <div
                                  style="margin-top: 8px; color: grey"
                                  class="text-center"
                                >
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
                        <v-col cols="12" md="9">
                          <v-text-field
                            type="file"
                            id="fileInput"
                            ref="fileInput"
                            hidden
                            :rules="[requiredValidator]"
                            @change="handleImageChange"
                            accept="image/*"
                          />
                        </v-col>
                      </v-row>
                      <v-row>
                        <v-col cols="12" md="6">
                          <v-text-field
                            label="Tên dự án"
                            :rules="[requiredValidator]"
                            v-model="inputAddProject.projectName"
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                        <v-col cols="12" md="6">
                          <AppDateTimePicker
                            :format="dateFormat"
                            v-model="inputAddProject.expectedEndDate"
                            placeholder="Ngày dự kiến"
                            :rules="[requiredValidator, dateValidator]"
                            prepend-inner-icon="tabler-calendar"
                            class="date-picker-input"
                          >
                          </AppDateTimePicker>
                        </v-col>
                        <v-col cols="12" md="6">
                          <v-select
                            clearable
                            v-model="inputAddProject.leaderId"
                            :rules="[requiredValidator]"
                            label="Người nhận dự án"
                            item-value="id"
                            item-title="fullName"
                            :items="dataUser"
                            variant="outlined"
                          ></v-select>
                        </v-col>
                        <v-col cols="12" md="6">
                          <v-select
                            clearable
                            v-model="inputAddProject.customerId"
                            item-value="id"
                            :rules="[requiredValidator]"
                            item-title="fullName"
                            label="Khách hàng"
                            :items="dataCustomer"
                            variant="outlined"
                          ></v-select>
                        </v-col>
                        <v-col cols="12" md="6">
                          <v-text-field
                            label="Giá dự án"
                            :rules="[requiredValidator]"
                            v-model="inputAddProject.startingPrice"
                            type="number"
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                        <v-col cols="12" md="6">
                          <v-text-field
                            label="Phần trăm hoa hồng nhân viên"
                            :rules="[
                              requiredValidator,
                              minValidator,
                              maxValidator,
                            ]"
                            type="number"
                            v-model="inputAddProject.commissionPercentage"
                            variant="outlined"
                          ></v-text-field>
                        </v-col>
                      </v-row>
                      <div class="mb-3">
                        <span class="red">(*)</span>
                        <span>Yêu cầu khách hàng</span>
                      </div>
                      <v-textarea
                        v-model="inputAddProject.requestDescriptionFromCustomer"
                        variant="outlined"
                        :rules="[requiredValidator]"
                        class="mb-5"
                      ></v-textarea>
                      <div class="mb-3">
                        <span class="red">(*)</span> <span>Mô tả dự án</span>
                      </div>
                      <v-textarea
                        v-model="inputAddProject.description"
                        :rules="[requiredValidator]"
                        variant="outlined"
                      ></v-textarea>
                    </div>

                    <v-card-actions>
                      <v-spacer></v-spacer>
                      <v-btn
                        variant="flat"
                        text="Thêm mới"
                        type="submit"
                        @click="saveProject"
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
      </v-col>
    </v-row>

    <VRow>
      <VCol
        cols="12"
        sm="6"
        md="3"
        v-for="(project, index) in paginatedData"
        :key="index"
      >
        <VCard>
          <VImg :src="project.imageDescription" cover height="25em" />

          <VCardItem>
            <VCardTitle>{{ project.projectName }}</VCardTitle>
          </VCardItem>
          <VCardText> Leader: {{ project.leader }} </VCardText>
          <v-card-text v-if="project.actualEndDate != null">
            Ngày tạo: {{ formatDate(project.actualEndDate) }}
          </v-card-text>
          <v-card-text v-else> Đang trong thời gian hoàn thành </v-card-text>
          <!-- <v-card-text>{{ confirmGiaoHang }}</v-card-text> -->
          <v-card-text>
            <label
              >Tiến độ:
              <strong style="font-size: 15px; color: #00ff0a"
                >{{ Math.ceil(project.progress) }}%</strong
              ></label
            >
            <v-progress-linear
              class="mt-2"
              color="light-blue"
              height="10"
              :model-value="project.progress"
              striped
            >
            </v-progress-linear>
          </v-card-text>

          <v-card-text>
            <v-dialog
              v-model="dialog"
              transition="dialog-bottom-transition"
              fullscreen
            >
              <template v-slot:activator="{ props: activatorProps }">
                <v-btn
                  icon
                  color="info"
                  style="font-size: 18px"
                  v-bind="activatorProps"
                  density="comfortable"
                  @click="handleButtonClick(project.id)"
                >
                  <v-icon icon="mdi-eye-outline"></v-icon>
                  <v-tooltip activator="parent" location="top">
                    Xem tiến độ dự án
                  </v-tooltip>
                </v-btn>
              </template>

              <v-card>
                <v-toolbar>
                  <v-toolbar-title>
                    {{ checkoutData.projectName }}
                  </v-toolbar-title>

                  <v-spacer></v-spacer>
                  <v-btn icon="mdi-close" @click="dialog = false"></v-btn>
                </v-toolbar>
                <VCard>
                  <!-- <VCardText>
                    <AppStepper
                      class="checkout-stepper"
                      :items="checkoutSteps"
                      :direction="
                        $vuetify.display.smAndUp ? 'horizontal' : 'vertical'
                      "
                    />
                  </VCardText> -->

                  <VCardText>
                    <AppStepper
                      class="checkout-stepper"
                      v-model:currentStep="currentStep"
                      :items="checkoutSteps"
                      :direction="
                        $vuetify.display.smAndUp ? 'horizontal' : 'vertical'
                      "
                    />
                  </VCardText>

                  <VDivider />

                  <VCardText>
                    <!-- 👉 stepper content -->
                    <VWindow
                      class="disable-tab-transition"
                      v-model="currentStep"
                    >
                      <VWindowItem :value="0">
                        <VRow>
                          <VCol cols="12" md="8">
                            <VCard hover>
                              <div>
                                <v-row>
                                  <v-col cols="5">
                                    <div class="ma-auto pa-5">
                                      <VImg
                                        style="border-radius: 10ypx"
                                        :src="checkoutData.imageDescription"
                                      />
                                    </div>
                                  </v-col>

                                  <VDivider
                                    :vertical="$vuetify.display.mdAndUp"
                                  />
                                  <v-col
                                    ><div>
                                      <VCardItem>
                                        <VCardTitle class="text-h3">
                                          {{ checkoutData.projectName }}
                                        </VCardTitle>
                                      </VCardItem>
                                      <VCardText class="text-subtitle-1">
                                        <span>Ngày tạo: </span>
                                        <span
                                          class="font-weight-medium"
                                          v-if="checkoutData.startDate != null"
                                        >
                                          {{
                                            formatDate(checkoutData.startDate)
                                          }}
                                        </span>
                                        <span class="font-weight-medium" v-else>
                                          Đang trong thời gian hoàn thành
                                        </span>
                                      </VCardText>
                                      <VCardText class="text-subtitle-1">
                                        <span>Ngày dự kiến: </span>
                                        <span class="font-weight-medium">
                                          {{
                                            formatDate(
                                              checkoutData.expectedEndDate
                                            )
                                          }}
                                        </span>
                                      </VCardText>
                                      <VCardText class="text-subtitle-1">
                                        <span style="font-weight: bold"
                                          >Yêu cầu của khách hàng:
                                        </span>
                                        <span class="font-weight-medium">
                                          {{
                                            checkoutData.requestDescriptionFromCustomer
                                          }}
                                        </span>
                                      </VCardText>
                                      <VCardText class="text-subtitle-1">
                                        <span style="font-weight: bold"
                                          >Mô tả:
                                        </span>
                                        <span class="font-weight-medium">
                                          {{ checkoutData.description }}
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
                                <h4 class="text-h4 font-weight-medium mb-3">
                                  Thông tin dự án
                                </h4>

                                <div class="d-flex align-center gap-4"></div>

                                <!-- 👉 Gift wrap banner -->
                                <div
                                  class="bg-var-theme-background rounded pa-5 mt-4"
                                >
                                  <h6 class="text-base font-weight-medium mb-5">
                                    Người phụ trách:
                                    {{ checkoutData.leader }}
                                  </h6>
                                  <p>
                                    Số điện thoại:
                                    {{ checkoutData.phoneLeader }}
                                  </p>
                                  <p>
                                    Email:
                                    {{ checkoutData.emailLeader }}
                                  </p>
                                </div>
                                <div
                                  class="bg-var-theme-background rounded pa-5 mt-4"
                                >
                                  <h6 class="text-base font-weight-medium mb-5">
                                    Khách hàng:
                                    {{ checkoutData.customer }}
                                  </h6>
                                  <p>
                                    Số điện thoại:
                                    {{ checkoutData.phoneCustomer }}
                                  </p>
                                  <p>
                                    Email:
                                    {{ checkoutData.emailCustomer }}
                                  </p>
                                  <p>
                                    Địa chỉ:
                                    {{ checkoutData.addressCustomer }}
                                  </p>
                                </div>
                              </VCardText>

                              <VDivider />

                              <VCardText
                                class="d-flex justify-space-between py-4"
                              >
                                <h6 class="text-base font-weight-medium">
                                  Giá dự án
                                </h6>
                                <h6 class="text-base font-weight-medium">
                                  {{
                                    formatCurrency(checkoutData.startingPrice)
                                  }}
                                </h6>
                              </VCardText>
                            </VCard>

                            <VBtn
                              block
                              v-if="userHasPermission"
                              :loading="loading"
                              class="mt-4"
                              :disabled="
                                isPrinting ||
                                progress === 75 ||
                                progress === 100
                              "
                              @click="goToStep(1)"
                            >
                              Thiết kế
                              <v-icon
                                icon="mdi-arrow-right"
                                class="ml-2"
                              ></v-icon>
                            </VBtn>
                          </VCol>
                        </VRow>
                        <!-- <CartContent
                          v-model:current-step="currentStep"
                          v-model:checkout-data="checkoutData"
                        /> -->
                      </VWindowItem>

                      <VWindowItem :value="1">
                        <VRow>
                          <VCol cols="12" md="8">
                            <v-row>
                              <v-col
                                cols="3"
                                v-for="(item, index) in paginatedDataDesign"
                                :key="index"
                              >
                                <VCard
                                  color="grey-lighten-1"
                                  height="450"
                                  width="270"
                                  @click="toggle"
                                >
                                  <VRadioGroup v-model="inputPheDuyet.designId">
                                    <VRow>
                                      <VCol>
                                        <VLabel
                                          class="custom-input custom-radio rounded cursor-pointer"
                                          :class="
                                            selectedOption === item.id
                                              ? 'active'
                                              : ''
                                          "
                                          @click="toggleSelection(item.id)"
                                        >
                                          <div class="flex-grow-1">
                                            <div
                                              class="d-flex align-center mb-1"
                                            >
                                              <VImg
                                                :src="item.designImage"
                                                cover
                                                style="
                                                  width: 200px;
                                                  height: 286px;
                                                "
                                                alt="designs"
                                              />
                                            </div>
                                            <div class="mt-4">
                                              <h4 class="mb-2">
                                                Người tạo:
                                                {{ item.designer }}
                                              </h4>
                                              <h4 class="mb-2">
                                                Ngày tạo:
                                                {{
                                                  formatDate(item.designTime)
                                                }}
                                              </h4>
                                              <h4 class="mb-2">
                                                Trạng thái:
                                                {{ item.designStatus }}
                                              </h4>
                                            </div>
                                          </div>
                                        </VLabel>
                                      </VCol>
                                    </VRow>
                                  </VRadioGroup>
                                </VCard>
                              </v-col>
                            </v-row>
                            <v-row>
                              <v-col>
                                <v-dialog max-width="500">
                                  <template
                                    v-slot:activator="{ props: activatorProps }"
                                  >
                                    <VBtn
                                      variant="tonal"
                                      v-if="userHasPermission"
                                      class="ml-4"
                                      v-bind="activatorProps"
                                      @click="
                                        inputCreateDesign.projectId =
                                          checkoutData.id
                                      "
                                      :disabled="
                                        isPrinting ||
                                        progress === 50 ||
                                        progress === 75 ||
                                        progress === 100
                                      "
                                    >
                                      <label>
                                        <VIcon
                                          icon="mdi-folder-upload-outline"
                                          style="font-size: 25px"
                                          class="mr-2"
                                        />
                                        Tải file
                                      </label>
                                    </VBtn>
                                  </template>

                                  <template v-slot:default="{ isActive }">
                                    <v-card title="Tải file ảnh ">
                                      <v-card-text>
                                        <input
                                          type="hidden"
                                          v-model="inputCreateDesign.projectId"
                                          readonly
                                        />
                                        <v-file-input
                                          label="File"
                                          variant="outlined"
                                          show-size
                                          accept="image/*"
                                          @change="handleImageChange"
                                        ></v-file-input>
                                      </v-card-text>

                                      <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn
                                          text="Thêm mới"
                                          variant="flat"
                                          :loading="loading"
                                          @click="
                                            createDesigns(checkoutData.id)
                                          "
                                        ></v-btn>
                                        <v-btn
                                          text="Thoát"
                                          variant="outlined"
                                          @click="isActive.value = false"
                                        ></v-btn>
                                      </v-card-actions>
                                    </v-card>
                                  </template>
                                </v-dialog>
                              </v-col>
                              <v-col class="text-right">
                                <div>
                                  <v-pagination
                                    v-model="currentPageDesign"
                                    :length="totalPagesDesign"
                                    rounded="circle"
                                  ></v-pagination>
                                </div>
                              </v-col>
                            </v-row>
                          </VCol>

                          <VCol cols="12" md="4">
                            <VCard flat variant="outlined">
                              <!-- 👉 Delivery estimate date -->
                              <VCardText>
                                <h6 class="text-base font-weight-medium mb-5">
                                  Thông tin dự án
                                </h6>

                                <VList
                                  class="card-list bg-var-theme-background rounded pa-5"
                                >
                                  <VListItem>
                                    <VListItemSubtitle class="text-h6 mb-4">
                                      Dự án:
                                      {{ checkoutData.projectName }}
                                    </VListItemSubtitle>
                                    <VListItemSubtitle class="text-h6 mb-4">
                                      Khách hàng
                                      {{ checkoutData.customer }}
                                    </VListItemSubtitle>
                                    <VListItemSubtitle class="text-h6 mb-4">
                                      Quản lý:
                                      {{ checkoutData.leader }}
                                    </VListItemSubtitle>
                                    <VListItemSubtitle class="text-h6 mb-4">
                                      Mô tả:
                                      {{ checkoutData.description }}
                                    </VListItemSubtitle>
                                    <VListItemSubtitle class="text-h6 mb-4">
                                      Yêu cầu của khách hàng:

                                      {{
                                        checkoutData.requestDescriptionFromCustomer
                                      }}
                                    </VListItemSubtitle>
                                  </VListItem>
                                </VList>
                              </VCardText>
                            </VCard>
                            <v-select
                              clearable
                              v-if="userHasPermission"
                              class="mt-5"
                              label="Mời chọn phê duyệt hoặc không phê duyệt"
                              :items="dataItemPheDuyet"
                              item-value="value"
                              item-title="label"
                              v-model="inputPheDuyet.DesignApproval"
                              variant="outlined"
                              :disabled="isSelectDisabled"
                            ></v-select>
                            <VBtn
                              block
                              class="mt-4"
                              :loading="loading"
                              @click="pheDuyet(checkoutData.id)"
                              v-if="inputPheDuyet.DesignApproval"
                            >
                              {{ buttonText }}
                              <VIcon icon=" mdi-arrow-right" class="ml-3" />
                            </VBtn>
                          </VCol>
                        </VRow>
                      </VWindowItem>

                      <VWindowItem :value="2">
                        <VRow>
                          <VCol cols="12" md="8">
                            <VRow>
                              <VCol cols="6">
                                <VLabel class="mb-2"> Mã đơn hàng </VLabel>
                                <VTextField
                                  v-model="dataDesign[0].id"
                                  disabled
                                />
                              </VCol>
                              <VCol cols="6">
                                <VLabel class="mb-2"> Tên đơn hàng </VLabel>

                                <VTextField
                                  v-model="checkoutData.projectName"
                                  disabled
                                />
                              </VCol>

                              <VCol cols="6">
                                <VLabel class="mb-2"> Quản lí </VLabel>
                                <VTextField
                                  v-model="checkoutData.leader"
                                  disabled
                                />
                              </VCol>
                              <VCol cols="6">
                                <VLabel class="mb-2"> Ngày đặt </VLabel>

                                <VTextField
                                  placeholder="Ngày đặt"
                                  class="date-picker-input"
                                  prepend-inner-icon="tabler-calendar"
                                  disabled
                                  v-model="formattedStartDate"
                                />
                              </VCol>
                              <VCol cols="6">
                                <VLabel class="mb-2"> Loại máy móc </VLabel>

                                <VSelect
                                  clearable
                                  v-model="selectedMachine"
                                  label="Loại máy móc"
                                  :items="resourcePropertyMachinesDetails"
                                  item-title="name"
                                  variant="outlined"
                                  :disabled="
                                    isPrinting ||
                                    progress === 75 ||
                                    progress === 100
                                  "
                                />
                              </VCol>

                              <v-col>
                                <v-table fixed-header height="385px">
                                  <thead>
                                    <tr>
                                      <th class="text-left">Tài nguyên</th>
                                      <!-- <th class="text-left">Đơn giá</th> -->
                                      <th class="text-left">Số lượng</th>
                                    </tr>
                                  </thead>
                                  <tbody>
                                    <tr
                                      v-for="(
                                        item, index
                                      ) in resourcePropertyDetails"
                                      :key="item.id"
                                    >
                                      <td>{{ item.name }}</td>
                                      <!-- <td>{{ item.price }}</td> -->
                                      <td>
                                        <div
                                          class="text-right"
                                          style="display: flex"
                                        >
                                          <v-btn
                                            @click="decrement(index)"
                                            class="btn-minus mr-2"
                                            :disabled="
                                              isPrinting ||
                                              progress === 75 ||
                                              progress === 100
                                            "
                                          >
                                            <v-icon
                                              icon="mdi-window-minimize"
                                            ></v-icon>
                                          </v-btn>
                                          <div style="width: 90px">
                                            <v-text-field
                                              type="number"
                                              v-model="item.quantity"
                                              :disabled="
                                                isPrinting ||
                                                progress === 75 ||
                                                progress === 100
                                              "
                                            />
                                          </div>
                                          <v-btn
                                            @click="increment(index)"
                                            class="btn-plus ml-2"
                                            :disabled="
                                              isPrinting ||
                                              progress === 75 ||
                                              progress === 100
                                            "
                                          >
                                            <v-icon icon="mdi-plus"></v-icon>
                                          </v-btn>
                                        </div>
                                      </td>
                                    </tr>
                                  </tbody>
                                </v-table>
                              </v-col>
                            </VRow>
                          </VCol>

                          <VCol cols="12" md="4">
                            <VCard flat variant="outlined">
                              <VCardText>
                                <h6 class="text-base font-weight-medium mb-4">
                                  Thông tin dự án
                                </h6>
                                <div class="text-center room">
                                  <v-img
                                    width="200"
                                    height="300"
                                    aspect-ratio="1/1"
                                    cover
                                    class="mb-4 box-shadow roomImg"
                                    :src="designsApprove[0].designImage"
                                  />
                                </div>

                                <div
                                  class="d-flex justify-space-between text-base mb-2"
                                >
                                  <span class="text-high-emphasis"
                                    >Giá dự án</span
                                  >
                                  <span>{{
                                    formatCurrency(checkoutData.startingPrice)
                                  }}</span>
                                </div>
                                <div
                                  class="d-flex justify-space-between text-base mb-2"
                                >
                                  <span class="text-high-emphasis"
                                    >Tên dự án</span
                                  >
                                  <span>{{ checkoutData.projectName }}</span>
                                </div>
                              </VCardText>

                              <VDivider />

                              <VCardText>
                                <div
                                  class="d-flex justify-space-between text-base mb-2"
                                >
                                  <span
                                    class="text-high-emphasis font-weight-medium"
                                    >Thành tiền:
                                  </span>
                                  <!-- <input
                                    type="text"
                                    v-model="checkoutData.startingPrice"
                                    hidden
                                  /> -->
                                  <span>{{
                                    isNaN(checkoutData.startingPrice) === true
                                      ? 0
                                      : formatCurrency(
                                          checkoutData.startingPrice
                                        )
                                  }}</span>
                                </div>
                                <VBtn
                                  v-if="
                                    isLeader &&
                                    !isPrintingCompleted &&
                                    progress !== 100
                                  "
                                  block
                                  class="mt-4"
                                  :disabled="!selectedMachine"
                                  @click="
                                    initiatePrintAndConfirm(
                                      designsApprove[0].id
                                    )
                                  "
                                >
                                  Bắt đầu in
                                </VBtn>
                              </VCardText>
                            </VCard>
                          </VCol>
                        </VRow>
                      </VWindowItem>

                      <VWindowItem :value="3">
                        <VRow>
                          <VCol cols="12" md="8">
                            <!-- 👉 Offers alert -->
                            <VAlert
                              color="success"
                              variant="tonal"
                              class="mb-4"
                            >
                              <template #prepend>
                                <VIcon icon="tabler-bookmarks" size="34" />
                              </template>
                              <VAlertTitle class="text-success mb-3">
                                Đơn hàng đã tạo thành công
                              </VAlertTitle>

                              <p class="mb-1">
                                Đơn hàng đang được giao cho nhân viên giao hàng
                              </p>
                            </VAlert>
                            <v-table>
                              <thead>
                                <tr>
                                  <th class="text-left">Tên đơn hàng</th>
                                  <th class="text-left">Khách hàng</th>
                                  <th class="text-left">Địa chỉ</th>
                                  <th class="text-left">Thao tác</th>
                                </tr>
                              </thead>
                              <tbody>
                                <tr>
                                  <td>{{ checkoutData.projectName }}</td>
                                  <td>{{ checkoutData.customer }}</td>
                                  <td>{{ checkoutData.addressCustomer }}</td>
                                  <td>
                                    <v-dialog max-width="500">
                                      <template
                                        v-slot:activator="{
                                          props: activatorProps,
                                        }"
                                      >
                                        <v-btn
                                          v-if="userCheckDelivery"
                                          v-bind="activatorProps"
                                          text="Giao hàng"
                                          variant="flat"
                                          :disabled="isDeliveryButtonDisabled"
                                        ></v-btn>
                                      </template>

                                      <template v-slot:default="{ isActive }">
                                        <v-card title="Giao hàng">
                                          <v-form
                                            ref="refVForm"
                                            @submit.prevent="onSubmitGiaoHang"
                                          >
                                            <input
                                              type="text"
                                              hidden
                                              v-model="
                                                inputCreateDiveler.customerId
                                              "
                                            />
                                            <input
                                              type="text"
                                              hidden
                                              v-model="
                                                inputCreateDiveler.projectId
                                              "
                                            />
                                            <div class="pa-4">
                                              <label>
                                                <span class="red mr-2">(*)</span
                                                >Ngày dự kiến giao hàng</label
                                              >
                                              <v-text-field
                                                class="mb-6 mt-3"
                                                placeholder="yyyy-mm-dd"
                                                :rules="[dateRule]"
                                                v-model="
                                                  inputCreateDiveler.estimateDeliveryTime
                                                "
                                              ></v-text-field>
                                              <label>
                                                <span class="red mr-2">(*)</span
                                                >Nhân viên giao hàng</label
                                              >
                                              <v-select
                                                clearable
                                                label="Nhân viên giao hàng"
                                                :items="dataUserDeviler"
                                                :rules="[requiredValidator]"
                                                class="mb-5 mt-3"
                                                v-model="
                                                  inputCreateDiveler.deliverId
                                                "
                                                item-title="fullName"
                                                item-value="id"
                                                variant="outlined"
                                              ></v-select>
                                            </div>
                                            <v-card-actions>
                                              <v-spacer></v-spacer>
                                              <v-btn
                                                variant="flat"
                                                text="Gửi"
                                                type="submit"
                                                @click="createDevilers"
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
                                  </td>
                                </tr>
                              </tbody>
                            </v-table>
                          </VCol>

                          <VCol cols="12" md="4">
                            <VCard flat variant="outlined">
                              <VCardText>
                                <h6 class="text-base font-weight-medium mb-4">
                                  Thông tin đơn hàng
                                </h6>

                                <div class="mb-2">
                                  <span>Mã đơn hàng: </span>
                                  <span>{{ checkoutData.id }}</span>
                                </div>
                                <div class="mb-2">
                                  <span>Tên đơn hàng: </span>
                                  <span>{{ checkoutData.projectName }}</span>
                                </div>
                              </VCardText>

                              <VDivider />

                              <VCardText>
                                <div>
                                  <div>
                                    <span class="text-high-emphasis"
                                      >Khách hàng:
                                    </span>

                                    <span class="me-2">{{
                                      checkoutData.customer
                                    }}</span>
                                  </div>
                                  <div>
                                    <span class="text-high-emphasis"
                                      >Số điện thoại:
                                      {{ checkoutData.phoneCustomer }}</span
                                    >
                                  </div>
                                  <div />
                                  <span class="me-2"
                                    >Địa chỉ:
                                    {{ checkoutData.addressCustomer }}</span
                                  >
                                </div>
                                <div
                                  class="d-flex justify-space-between text-base mb-2"
                                >
                                  <span
                                    class="text-high-emphasis font-weight-medium"
                                    >Thành tiền:
                                    {{
                                      formatCurrency(checkoutData.startingPrice)
                                    }}</span
                                  >
                                </div>
                              </VCardText>
                            </VCard>
                          </VCol>
                        </VRow>
                      </VWindowItem>
                    </VWindow>
                  </VCardText>
                </VCard>
              </v-card>
            </v-dialog>
            <!-- <v-dialog max-width="400">
              <template v-slot:activator="{ props: activatorProps }">
                <v-btn
                  v-bind="activatorProps"
                  style="font-size: 20px"
                  density="comfortable"
                  class="ml-4"
                  icon
                >
                  <v-icon icon="mdi-pencil-outline"></v-icon>
                  <v-tooltip activator="parent" location="top">
                    Cập nhật dự án
                  </v-tooltip>
                </v-btn>
              </template>

              <template v-slot:default="{ isActive }">
                <v-card class="pa-4">
                  <div class="text-center mb-4">
                    <h2>Cập nhật dự án</h2>
                  </div>
                  <h1 class="text-center mb-8">Tính năng đang phát triển</h1>
                  <v-card-actions>
                    <v-spacer></v-spacer>

                    <v-btn
                      text="Cập nhật"
                      variant="flat"
                      @click="isActive.value = false"
                    ></v-btn>
                    <v-btn
                      text="Thoát"
                      variant="outlined"
                      @click="isActive.value = false"
                    ></v-btn>
                  </v-card-actions>
                </v-card>
              </template>
            </v-dialog> -->
            <v-dialog max-width="300">
              <template v-slot:activator="{ props: activatorProps }">
                <v-btn
                  v-if="isAdmin"
                  density="comfortable"
                  style="font-size: 20px"
                  v-bind="activatorProps"
                  color="error"
                  class="ml-4"
                  icon
                >
                  <v-icon icon="mdi-delete-outline"></v-icon>
                  <v-tooltip activator="parent" location="top">
                    Xóa dự án
                  </v-tooltip>
                </v-btn>
              </template>

              <template v-slot:default="{ isActive }">
                <v-card class="pa-4 text-center">
                  <h2>Bạn có muốn xóa</h2>
                  <v-card-actions class="mt-4">
                    <div>
                      <v-btn
                        text="Xóa"
                        :loading="loading"
                        @click="deleteProjects(project.id)"
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
          </v-card-text>
        </VCard>
      </VCol>
    </VRow>
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
import { projectApi } from "../../../api/Project/projectApi";
import { customerApi } from "../../../api/customer/customerApi";
import { userApi } from "../../../api/User/userApi";
import { resourceApi } from "@/api/resource/resourceApi";
import { giaoHangApi } from "@/api/giaoHang/giaoHangApi";
import {
  alphaDashValidator,
  emailValidator,
  requiredValidator,
  phoneNumberValidator,
  usernameValidator,
} from "@validators";
export default {
  data() {
    return {
      tab: null,
      page: 1,
      tienDo: null,
      isPrinting: false,
      userHasPermission: false,
      giaoHangApi: giaoHangApi(),
      projectApi: projectApi(),
      customerApi: customerApi(),
      userApi: userApi(),
      userCheckDelivery: false,
      resourceApi: resourceApi(),
      dataCustomer: [],
      dataProject: [],
      dataDeviler: [],
      dataUser: [],
      isPrintingCompleted: false,
      isActive: true,
      findByProjectId: {},
      designsApprove: {},
      snackbar: false,
      dataItemPheDuyet: [
        {
          value: "Agree",
          label: "Phê duyệt",
        },
        {
          value: "Refuse",
          label: "Không phê duyệt",
        },
      ],
      dataMachines: [],
      confirmGiaoHang: "",
      dialog: false,
      notifications: false,
      sound: true,
      widgets: false,
      loading: false,
      perPage: 8,
      quantity: 0,
      totalPrice: 0,
      progress: 0,
      currentPageDesign: 1,
      perPageDesign: 4,
      user: null,
      dataDesign: [],
      dataResource: [],
      stepProgressRequirements: [0, 1, 2, 3],
      updatePrj: {
        startingPrice: 0,
      },
      resourceProperty: [],
      resourcePropertyDetails: [],
      resourcePropertyMachines: [],
      resourcePropertyMachinesDetails: [],
      checkoutData: {},
      refVForm: "",
      currentPage: 1,
      date: "",
      valid: false,
      dateRule: (value) => {
        const pattern = /^\d{4}-\d{2}-\d{2}$/;
        const isValidFormat = pattern.test(value);
        if (!isValidFormat) {
          return "Vui lòng nhập đúng định dạng (yyy-mm-dd)";
        }

        const inputDate = new Date(value);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (inputDate < today) {
          return "Vui lòng nhập lớn hơn ngày hiện tại";
        }

        return true;
      },
      dateValidator: (value) => {
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        const selectedDate = new Date(value);
        if (selectedDate < today) {
          return "Ngày dự kiến phải lớn hơn hoặc bằng ngày hiện tại.";
        }

        return true;
      },
      maxValidator: (value) => {
        if (value > 100) {
          return "Phần trăm hoa hồng không thể lớn hơn 100.";
        }
        return true;
      },
      minValidator: (value) => {
        if (value < 0) {
          return "Phần trăm hoa hồng không thể nhỏ hơn 0.";
        }
        return true;
      },
      selectedOption: null,
      filteredDesigns: [],
      dataUserDeviler: [],
      currentStep: null,
      text: "",
      checkRoleBtn: {},
      isLeader: false,
      userCheckRole: {},
      userEmployee: false,
      activatorProps: {},
      acc: JSON.parse(localStorage.getItem("userInfo")),
      knowledge: null,
      imageUrl: "",
      isAdmin: "",
      inputCreateDiveler: {
        shippingMethodId: "5d2439c1-d63d-4cc0-86c6-8f65d9c25f5a",
        customerId: "",
        deliverId: "",
        projectId: "",
        estimateDeliveryTime: "",
      },
      inputPrintJob: {
        designId: "",
        resourceForPrints: [
          {
            resourcePropertyDetailId: "",
            quantity: 0,
          },
        ],
      },
      inputPheDuyet: {
        designId: "",
        DesignApproval: null,
      },
      inputCreateDesign: {
        projectId: "",
        designImage: null,
      },
      fillterProject: {
        projectName: "",
        startDate: "",
        endDate: "",
        leaderId: "",
      },
      dataTienDo: [
        {
          name: "0%",
          value: 0,
        },
        {
          name: "25%",
          value: 25,
        },
        {
          name: "50%",
          value: 50,
        },
        // {
        //   name: "75%",
        //   value: 75,
        // },
        {
          name: "100%",
          value: 100,
        },
      ],
      checkoutSteps: [
        {
          title: "Dự án",
          icon: "custom-trending",
          value: 0,
          status: false,
        },
        {
          title: "Thiết kế",
          icon: "custom-address",
          value: 1,
          status: false,
        },
        {
          title: "In ấn",
          icon: "custom-payment",
          value: 2,
          status: false,
        },
        {
          title: "Giao hàng",
          icon: "custom-cart",
          value: 3,
          status: false,
        },
      ],
      selectedMachine: null,
      isDeliveryButtonDisabled: false,
      dataPrint: [],
      inputAddProject: {
        projectName: "",
        description: "",
        requestDescriptionFromCustomer: "",
        leaderId: "",
        expectedEndDate: "",
        customerId: "",
        imageDescription: "",
        startingPrice: "",
        commissionPercentage: "",
      },
      isLoading: true,
    };
  },
  async mounted() {
    const resDataCustomer = await this.customerApi.getAllCustomer();
    this.dataCustomer = resDataCustomer.data;
    const resUser = await this.userApi.getAllUserContainsLeaderRole();
    this.dataUser = resUser.data;
    await this.getCheckHienThi();
    this.CheckLeader();
    this.CheckDeliver();
    this.CheckEmployee();
  },
  watch: {
    resourcePropertyDetails: {
      handler: "calculateTotalPrice",
      deep: true,
    },
    totalPrice(newVal) {
      this.updatePrj.startingPrice = newVal;
    },
    "checkoutData.customerId": function (newVal) {
      this.inputCreateDiveler.customerId = newVal;
    },
    "checkoutData.id": function (newVal) {
      this.inputCreateDiveler.projectId = newVal;
    },
  },
  methods: {
    async getAllGiaoHang() {
      try {
        const res = await this.giaoHangApi.getAllGiaoHang();
        const deliveryData = res.data;
        const matchingDelivery = deliveryData.find(
          (delivery) => delivery.project.id === this.checkoutData.id
        );
        if (matchingDelivery) {
          this.deliveryData = matchingDelivery;
          const disableStatuses = ["Delivered", "Delivering", "Waiting"];
          this.isDeliveryButtonDisabled = disableStatuses.includes(
            matchingDelivery.deliveryStatus
          );
        } else {
          this.isDeliveryButtonDisabled = false;
        }
      } catch (error) {
        console.error("Error fetching delivery data:", error);
        this.isDeliveryButtonDisabled = false;
      }
    },
    startDateRule(value) {
      const startTime = new Date(value);
      const endTime = new Date(this.fillterProject.endDate);
      if (endTime && startTime >= endTime) {
        return "Ngày bắt đầu phải nhỏ hơn ngày kết thúc";
      }
      return true;
    },

    endDateRule(value) {
      const endTime = new Date(value);
      const startTime = new Date(this.fillterProject.startDate);
      if (startTime && startTime >= endTime) {
        return "Ngày kết thúc phải lớn hơn ngày bắt đầu";
      }
      return true;
    },
    formatDate() {
      if (this.date.length === 4 || this.date.length === 7) {
        this.date += "-";
      }
    },
    async getDataUser() {
      try {
        const res = await this.userApi.getAllUsers();
        this.dataUserDeviler = res.data.filter(
          (user) => user.teamName === "Delivery"
        );
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    },
    toggleSelection(optionId) {
      if (this.selectedOption === optionId) {
        this.selectedOption = null;
        this.inputPheDuyet.designId = "";
      } else {
        this.selectedOption = optionId;
        this.inputPheDuyet.designId = optionId;
      }
    },

    async pheDuyet(id) {
      this.loading = true;
      try {
        const res = await this.projectApi.approvalDesign(this.inputPheDuyet);
        if (res.status === 200) {
          const data = await this.projectApi.getByIdProject(id);
          this.dataDesign = data.data.designs;
          this.designsApprove = this.dataDesign.filter(
            (design) => design.designStatus === "HasBeenApproved"
          );
          this.text = res.data;
          this.snackbar = true;
          if (res.data === "Đã duyệt thiết kế") {
            this.goToStep(2);
          } else {
            console.log(
              "Triggering goToStep(2) is skipped because the design is not approved"
            );
          }
        } else {
          this.text = res.data;
          this.snackbar = true;
        }
      } catch (e) {
        console.error("fetching data failed:" + e);
      } finally {
        this.loading = false;
      }
    },
    async getAllTaiNguyen() {
      const res = await this.resourceApi.getAllsResource();
      this.dataResource = res.data.filter(
        (resourceType) => resourceType.resourceTypeName === "Stationery"
      );
      this.dataMachines = res.data.filter(
        (resourceType) => resourceType.resourceTypeName === "Machines"
      );
      this.resourceProperty = this.dataResource.flatMap(
        (resource) => resource.resourceProperties || []
      );
      this.resourcePropertyMachines = this.dataMachines.flatMap(
        (resource) => resource.resourceProperties || []
      );
      this.resourcePropertyDetails = this.resourceProperty.flatMap(
        (property) => property.resourcePropertyDetails || []
      );

      this.resourcePropertyMachinesDetails =
        this.resourcePropertyMachines.flatMap(
          (property) => property.resourcePropertyDetails || []
        );
      // console.log(this.resourceProperty);
      // this.resourcePropertyDetails.forEach((item) => {
      //   this.$set(item, "quantity", item.quantity || 0);
      //   this.$set(item, "maxQuantity", item.maxQuantity || 0); // Set maxQuantity for each item
      // });

      // this.resourcePropertyDetails.forEach((item) => {
      //   this.$set(item, "quantity", item.quantity || 0);
      // });
      // this.calculateTotalPrice();
    },
    increment(index) {
      this.resourcePropertyDetails[index].quantity++;
      // this.calculateTotalPrice();
    },
    decrement(index) {
      if (this.resourcePropertyDetails[index].quantity > 0) {
        this.resourcePropertyDetails[index].quantity--;
        // this.calculateTotalPrice();
      }
    },
    // calculateTotalPrice() {
    //   const resourceTotal = this.resourcePropertyDetails.reduce(
    //     (total, item) => {
    //       return total + item.price * item.quantity;
    //     },
    //     0
    //   );
    //   this.totalPrice = this.checkoutData.startingPrice + resourceTotal;
    // },
    checkRole() {
      const roleUser = JSON.parse(localStorage.getItem("userInfo"));
      return roleUser;
    },
    async saveProject() {
      this.loading = true;
      try {
        const res = await this.projectApi.addProject(this.inputAddProject);
        if (res.data.status === 200) {
          this.text = res.data.message;
          this.snackbar = true;
          setTimeout(() => {
            this.reloadPage();
          }, 2000);
        } else {
          this.text = res.data.message;
          this.snackbar = true;
        }
      } catch (e) {
        this.text = "Lỗi khi thêm dữ liệu";
        this.snackbar = true;
      } finally {
        this.loading = false;
      }
    },
    async getCheckHienThi() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      if (userInfo && userInfo.Permission) {
        console.log("Permissions:", userInfo.Permission); // Kiểm tra giá trị của userInfo.Permission
        const isAdmin = userInfo.Permission.includes("Admin");
        const isLeader = userInfo.Permission.includes("Leader");
        const isDesigner = userInfo.Permission.includes("Designer");

        // Kiểm tra các điều kiện riêng lẻ
        console.log("isAdmin:", isAdmin);
        console.log("isLeader:", isLeader);
        console.log("isDesigner:", isDesigner);
        if (isAdmin) {
          this.userHasPermission = isLeader || isDesigner;
        } else {
          this.userHasPermission = isLeader || isDesigner;
        }
      } else {
        this.userHasPermission = false;
      }
    },
    async CheckLeader() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      if (userInfo && userInfo.Permission) {
        this.isLeader = userInfo.Permission.includes("Leader");
        console.log("isLeader:", this.isLeader);
      } else {
        this.isLeader = false;
      }
    },
    async CheckDeliver() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));

      if (userInfo && userInfo.Permission) {
        this.userCheckDelivery =
          userInfo.Permission.includes("Leader") &&
          userInfo.Permission.includes("Manager");
      } else {
        this.userCheckDelivery = false;
      }
    },
    async CheckEmployee() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));

      if (userInfo && userInfo.Permission) {
        this.userEmployee = userInfo.Permission.includes("Employee");
      } else {
        this.userEmployee = false;
      }
    },
    async getByIdUser() {
      const idUser = JSON.parse(localStorage.getItem("userInfo"));
      if (idUser && idUser.Id) {
        const userId = await this.userApi.getUserById(idUser.Id);
        this.userCheckRole = userId.data;
      }
    },
    async deleteProjects(id) {
      try {
        const res = await this.projectApi.deleteProject(
          id,
          (this.loading = true)
        );
        if (res.status === 200) {
          this.text = res.data;
          this.snackbar = true;
          setTimeout(() => {
            this.reloadPage();
          }, 2000);
        } else {
          this.text = res.data.message;
          this.snackbar = true;
        }
      } catch (e) {
        console.error(e);
        this.text = "Bạn không phải Admin";
        this.loading = false;
        this.snackbar = true;
      }
    },
    onSubmit() {
      refVForm.value?.validate().then(({ valid: isValid }) => {
        if (isValid) register();
      });
    },
    reloadPage() {
      location.reload();
    },
    handleImageChange(event) {
      const file = event.target.files[0];
      const maxSizeInBytes = 2 * 1024 * 1024;
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

      this.inputAddProject.imageDescription = file;
      this.inputCreateDesign.designImage = file;
      const reader = new FileReader();
      reader.onload = (e) => {
        this.imageUrl = e.target.result;
      };
      reader.readAsDataURL(file);
    },
    async createDevilers() {
      const res = await this.projectApi.createDeviler(this.inputCreateDiveler);
      if (res.data.status === 200) {
        this.text = res.data.message;
        this.snackbar = true;
        setTimeout(() => {
          this.reloadPage();
        }, 1500);
      } else {
        this.text = res.data.message;
        this.snackbar = true;
      }
    },
    async fillter() {
      try {
        const res = await this.projectApi.fillterData(this.fillterProject);
        this.dataProject = res.data;
      } catch (e) {
        console.error("error fetching filtered data", e);
      }
    },
    onSubmitGiaoHang() {
      refVForm.value?.validate().then(({ valid: isValid }) => {
        if (isValid) this.createDevilers();
      });
    },
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
    async getAllCompareDesignPrint() {
      const print = await this.projectApi.getAllPrintJobs();
      this.dataPrint = print.data;
    },
    async createDesigns(id) {
      this.loading = true;
      try {
        const res = await this.projectApi.createDesign(this.inputCreateDesign);

        if (res.data.status === 200) {
          const data = await this.projectApi.getByIdProject(id);
          this.dataDesign = data.data.designs;
          this.isActive = false;
          this.text = res.data.message;
          this.snackbar = true;
        } else {
          this.text = res.data.message;
          this.snackbar = true;
        }
      } catch (e) {
        this.text = "Lỗi khi thêm dữ liệu";
        this.snackbar = true;
      } finally {
        this.loading = false;
      }
    },
    async getDataProjects() {
      this.isLoading = true;
      try {
        const res = await this.projectApi.getAllsProject();
        this.dataProject = res.data;
        // .filter(
        //   (project) => project.progress !== 100
        // );
      } catch (e) {
        console.error("error fetching data", e);
      } finally {
        this.isLoading = false;
      }
    },
    async getByIdProjects(id) {
      try {
        const res = await this.projectApi.getByIdProject(id);
        this.checkoutData = res.data;
        // this.updatePrj = res.data;
        this.dataDesign = this.checkoutData.designs;
        this.designsApprove = this.dataDesign.filter(
          (design) => design.designStatus === "HasBeenApproved"
        );
        this.progress = this.checkoutData.progress;
        const projectProgress = this.checkoutData.progress;
        const step = this.getStepFromProgress(projectProgress);
        this.goToStep(step);
        this.getAllGiaoHang();
        this;
      } catch (error) {
        console.error("Lỗi khi lấy dữ liệu dự án:", error);
      }
    },
    async printing(id) {
      this.isPrinting = true;
      this.updatePrj.startingPrice = this.totalPrice;

      try {
        // const updateProject = await this.projectApi.updateProject(
        //   id,
        //   this.updatePrj
        // );

        this.inputPrintJob.designId = id;
        this.inputPrintJob.resourceForPrints = this.resourcePropertyDetails.map(
          (item) => ({
            resourcePropertyDetailId: item.id,
            quantity: item.quantity,
          })
        );

        const res = await this.projectApi.createPrintJob(this.inputPrintJob);
        if (res.data.status === 200) {
          this.text = res.data.message;
          this.snackbar = true;
          this.isPrintingCompleted = true;
          const idPrint = res.data.data;
          return idPrint.id;
        } else {
          this.text = res.data.message;
          this.snackbar = true;
        }
      } catch (error) {
        console.error("Lỗi khi cập nhật và in dự án:", error);
      } finally {
        this.isPrinting = false;
      }
    },
    async ComfirmDonePrint(id) {
      const res = await this.projectApi.confirmDonePrintJob(id);
      this.goToStep(3);
    },
    async initiatePrintAndConfirm(id) {
      this.pro;
      try {
        const printJobId = await this.printing(id);
        if (printJobId) {
          await this.ComfirmDonePrint(printJobId);
        } else {
          console.error("Failed to retrieve printJobId");
        }
      } catch (error) {
        console.error("Error during print and confirm process:", error);
      }
    },
    handleButtonClick(id) {
      this.getByIdProjects(id);
    },
    loadUserFromLocalStorage() {
      const userJson = localStorage.getItem("userInfo");
      if (userJson) {
        this.user = JSON.parse(userJson);
      }
    },
    formatCurrency(value) {
      const intValue = parseInt(value);
      return intValue.toLocaleString("vi-VN", {
        style: "currency",
        currency: "VND",
      });
    },
    getStepFromProgress(progress) {
      let step;
      switch (true) {
        case progress === 0:
          step = 0;
          break;
        case progress > 0 && progress < 50:
          step = 1;
          break;
        case progress >= 50 && progress < 75:
          step = 2;
          break;
        case progress >= 75 && progress < 100:
          step = 3;
          break;
        case progress === 100:
          step = 3;
          break;
        default:
          step = 0;
      }
      return step;
    },
    goToStep(step) {
      // if (this.progress < 50 && step == 2) {
      //   console.log("vào đây rồi nhé");
      //   this.text = "Bạn phải hoàn thành bước trước đó!";
      //   this.snackbar = true;
      //   return step == 2;
      // }
      this.currentStep = step;
    },
  },
  created() {
    this.getDataProjects();
    this.getByIdUser();
    this.loadUserFromLocalStorage();
    this.getAllTaiNguyen();
    this.getDataUser();
    // this.getAllGiaoHang();
    // this.getAllCompareDesignPrint();
  },
  computed: {
    formattedStartDate: {
      get() {
        return this.formatDate(this.checkoutData.startDate);
      },
      set(value) {
        this.checkoutData.startDate = this.parseDate(value);
      },
    },
    isSelectDisabled() {
      return (
        !this.inputPheDuyet.designId ||
        this.isPrinting ||
        this.progress === 50 ||
        this.progress === 75 ||
        this.progress === 100
      );
    },
    filteredProjects() {
      if (this.tienDo === null) {
        return this.dataProject;
      } else {
        const percentage = parseInt(this.tienDo);
        return this.dataProject.filter(
          (project) => project.progress === percentage
        );
      }
    },
    isAdmin() {
      return (
        this.user &&
        this.user.Permission &&
        this.user.Permission.includes("Admin")
      );
    },
    isEmployee() {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      return (
        userInfo &&
        userInfo.Permission &&
        userInfo.Permission.includes("Employee")
      );
    },
    buttonText() {
      return this.inputPheDuyet.DesignApproval === "Agree"
        ? " Phê duyệt"
        : this.inputPheDuyet.DesignApproval === "Refuse"
        ? " Không phê duyệt"
        : "";
    },
    dateFormat: "yyyy-MM-dd",
    paginatedData() {
      const start = (this.currentPage - 1) * this.perPage;
      const end = start + this.perPage;
      return this.filteredProjects.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.filteredProjects.length / this.perPage);
    },
    paginatedDataDesign() {
      const start = (this.currentPageDesign - 1) * this.perPageDesign;
      const end = start + this.perPageDesign;
      return this.dataDesign.slice(start, end);
    },
    totalPagesDesign() {
      return Math.ceil(this.dataDesign.length / this.perPageDesign);
    },
    hasSaleRole() {
      return (
        this.acc.Permission &&
        this.acc.Permission.includes("Employee") &&
        Array.isArray(this.userCheckRole.teamName) &&
        this.userCheckRole.teamName.includes("Sales")
      );
    },
    isButtonDisabled() {
      return this.inputPheDuyet.DesignApproval === null;
    },
  },
};
</script>
<style lang="scss" scoped>
.room .roomImg {
  display: block;
  transition: all 0.4s ease;
}
.room .roomImg:hover {
  transform: scale(1.1);
  box-shadow: 1px 1px 15px 2px rgb(39, 39, 39);
}
.avatar-center {
  position: absolute;
  border: 3px solid rgb(var(--v-theme-surface));
  inset-block-start: -2rem;
  inset-inline-start: 1rem;
}
.red {
  color: rgb(255, 72, 72);
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
.box-shadow {
  border-radius: 10px;
}
.checkout-stepper {
  .stepper-icon-step {
    .step-wrapper + svg {
      margin-inline: 3.5rem !important;
    }
  }
}
.v-btn {
  transform: none;
}
</style>
