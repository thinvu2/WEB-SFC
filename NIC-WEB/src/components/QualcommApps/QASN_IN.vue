<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="isShowForm ? ReturnForm() : BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Advance Shipment Notice</span>
      </div>
    </div>
    <!-- Search Box -->
    <div class="searchbox" v-if="!isShowForm">
      <div class="searchbox-content">
        <input
          :disabled="showTimeForm"
          v-on:keyup.enter="QuerySearch()"
          v-model="valueSearch"
          type="text"
          class="form-control"
          placeholder="Enter Pack Slip No... "
        />
        <button @click="QuerySearch()" class="btn-button">
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
      <!-- datepicker -->
      <div class="searchbox-timerange">
        <div class="showTimeForm">
          <input
            id="showTimeForm"
            type="checkbox"
            v-model="showTimeForm"
          />
        </div>
        <div class="datepicker-wrapper">
          <datepicker
            :disabled="!showTimeForm"
            class="form-control form-control-sm"
            v-model="dateFrom"
            :upperLimit="dateTo"
          />
        </div>
        <div class="datepicker-wrapper">
          <datepicker
            :disabled="!showTimeForm"
            class="form-control form-control-sm"
            v-model="dateTo"
            :lowerLimit="dateFrom"
          />
        </div>
      </div>
    </div>
    <div class="export-excel" v-if="!isShowForm">
      <img
        @click="exportexcelxlsx()"
        class="img-excel"
        src="assets/img/xlsx-icon.jpg"
        alt=""
      />
      <img
        @click="exportExcel()"
        class="img-excel"
        src="assets/img/excel_64.png"
        alt=""
      />
    </div>
    <!-- Form show data -->
    <div class="container" v-if="isShowForm === true">
      <div class="title-class">
        <p>Advance Shipment Notice</p>
      </div>
      <div class="form-row">
        <label for="msg_sender_name">From:</label>
        <input
          type="text"
          class="text-input"
          id="msg_sender_name"
          name="msg_sender_name"
          readonly
          v-model="model.MSG_SENDER_NAME"
        />
      </div>
      <div class="form-row">
        <label for="msg_receiver_name">To:</label>
        <input
          type="text"
          class="text-input"
          id="msg_receiver_name"
          name="msg_receiver_name"
          readonly
          v-model="model.MSG_RECEIVER_NAME"
        />
      </div>
      <div class="form-row">
        <label for="ship_mcmn_creattime">Time:</label>
        <input
          type="text"
          class="text-input"
          id="ship_mcmn_creattime"
          name="ship_mcmn_creattime"
          readonly
          v-model="model.SHIP_MCMN_CREATTIME"
        />
      </div>
      <div class="form-row">
        <label for="packslip_no">Pack Slip No:</label>
        <input
          type="text"
          class="text-input"
          id="packslip_no"
          name="packslip_no"
          readonly
          v-model="model.PACKSLIP_NO"
        />
      </div>
      <div class="form-row">
        <label for="airwaybill">Airway Bill:</label>
        <input
          type="text"
          class="text-input"
          id="airwaybill"
          name="airwaybill"
          readonly
          v-model="model.AIRWAYBILL"
        />
      </div>
      <div class="form-row">
        <label for="freightcarrier">FreightCarrier:</label>
        <input
          type="text"
          class="text-input"
          id="freightcarrier"
          name="freightcarrier"
          readonly
          v-model="model.FREIGHT_CARRIER_CODE"
        />
      </div>
      <div class="form-row">
        <label for="shipment">Shipment:</label>
        <input
          type="text"
          class="text-input"
          id="shipment"
          name="shipment"
          readonly
          v-model="model.SHIP_MCMN_CREATTIME"
        />
      </div>
      <div class="form-row">
        <label for="estimate-dock">Estimate Dock:</label>
        <input
          type="text"
          class="text-input"
          id="estimate-dock"
          name="estimate-dock"
          readonly
          v-model="model.SHIP_MCMN_ESTIMATEDDOCKTIME"
        />
      </div>
      <div class="form-row">
        <label for="supplier">Supplier:</label>
        <input
          type="text"
          class="text-input"
          id="supplier"
          name="supplier"
          readonly
          v-model="model.SUPPLIER_NAME"
        />
      </div>

      <div class="form-row">
        <label for="gross-weight">Gross Weight:</label>
        <input
          type="text"
          class="text-input"
          id="gross-weight"
          name="gross-weight"
          readonly
          v-model="model.GROSSWEIGHT"
        />
      </div>

      <div class="form-row">
        <label for="net-weight">Net Weight:</label>
        <input
          type="text"
          class="text-input"
          id="net-weight"
          name="net-weight"
          readonly
          v-model="model.NETWEIGHT"
        />
      </div>

      <div class="form-row">
        <label for="outer-container">Outer Container:</label>
        <input
          type="text"
          class="text-input"
          id="outer-container"
          name="outer-container"
          readonly
          v-model="model.SHIP_MCMN_TOTALOUTERCONTAINERS"
        />
      </div>
      <!-- <div class="form-row">
                    <label for="customer-drop-ship">Customer Ship:</label>
                    <input type="text"
                        class="text-input"
                        id="customer-drop-ship" 
                        name="customer-drop-ship"
                        readonly
                        v-model="model.AIRWAYBILL"
                    />
                </div> -->

      <div class="actual-ship-from">
        <span class="title-actual-ship"
          >Actual ship from: {{ model.SHIP_MCMN_LOCATIONNAME }}</span
        >
        <div class="form-row-actual">
          <label for="country-code">Country Code:</label>
          <input
            type="text"
            class="text-input"
            id="country-code"
            name="country-code"
            readonly
            v-model="model.COUNTRY_CODE"
          />
        </div>
        <div class="form-row-actual">
          <label for="ship-mcmn-city">City:</label>
          <input
            type="text"
            class="text-input"
            id="ship-mcmn-city"
            name="ship-mcmn-city"
            readonly
            v-model="model.SHIP_MCMN_CITY"
          />
        </div>
        <div class="form-row-actual">
          <label for="postal-code">Postal Code:</label>
          <input
            type="text"
            class="text-input"
            id="postal-code"
            name="postal-code"
            readonly
            v-model="model.POSTAL_CODE"
          />
        </div>

        <div class="form-row-actual-all">
          <div class="form-row-actual-all-label">
            <label for="packslip_no">Address:</label>
          </div>
          <div class="form-row-actual-all-input">
            <p>{{ model.SHIP_MCMN_STREET1 }}</p>
          </div>
          <div class="form-row-actual-all-input">
            <p>{{ model.SHIP_MCMN_STREET2 }}</p>
          </div>
          <div class="form-row-actual-all-input">
            <p>{{ model.SHIP_MCMN_STREET3 }}</p>
          </div>
        </div>
      </div>
      <div class="class-hr">
        <hr/>
      </div>
      <div class="form-row">
        <label for="ship-mcmn-lpn">Pallet No:</label>
        <input
          type="text"
          class="text-input"
          id="ship-mcmn-lpn"
          name="ship-mcmn-lpn"
          readonly
          v-model="model.SHIP_MCMN_LPN"
        />
      </div>
      <div class="form-row">
        <label for="pal-grossweight">GrossWeight:</label>
        <input
          type="text"
          class="text-input"
          id="pal-grossweight"
          name="pal-grossweight"
          readonly
          v-model="model.PAL_GROSSWEIGHT"
        />
      </div>
      <div class="form-row">
        <label for="pal-netweight">NetWeight:</label>
        <input
          type="text"
          class="text-input"
          id="pal-netweight"
          name="pal-netweight"
          readonly
          v-model="model.PAL_NETWEIGHT"
        />
      </div>
      <div class="form-row">
        <label for="pal-width">Width:</label>
        <input
          type="text"
          id="pal-width"
          class="text-input"
          name="pal-width"
          readonly
          v-model="model.PAL_WIDTH"
        />
      </div>

      <div class="form-row">
        <label for="pal-length">Length:</label>
        <input
          type="text"
          class="text-input"
          id="pal-length"
          name="pal-length"
          readonly
          v-model="model.PAL_LENGTH"
        />
      </div>
      <div class="form-row">
        <label for="pal-height">Height:</label>
        <input
          type="text"
          class="text-input"
          id="pal-height"
          name="pal-height"
          readonly
          v-model="model.PAL_HEIGHT"
        />
      </div>

      <div class="form-row">
        <label for="po-no">Po No:</label>
        <input
          type="text"
          class="text-input"
          id="po-no"
          name="po-no"
          readonly
          v-model="model.PO_NO"
        />
      </div>
      <div class="form-row">
        <label for="po-line">Po Line:</label>
        <input
          type="text"
          class="text-input"
          id="po-line"
          name="po-line"
          readonly
          v-model="model.POLINE_NO"
        />
      </div>
      <!-- <div class="form-row">
        <label for="lot-no">Lot No:</label>
        <input
          type="text"
          class="text-input"
          id="lot-no"
          name="lot-no"
          readonly
          v-model="model.LOT_NO"
        />
      </div> -->
      <div class="form-row">
        <label for="item-no">Items No:</label>
        <input
          type="text"
          id="item-no"
          class="text-input"
          name="item-no"
          readonly
          v-model="model.ITEM_NO"
        />
      </div>
      <div class="form-row">
        <label for="item-shipped-qty">QTY:</label>
        <input
          type="text"
          class="text-input"
          id="item-shipped-qty"
          name="item-shipped-qty"
          readonly
          v-model="model.ITEM_SHIPPEDQTY"
        />
      </div>
      <div class="form-row">
        <label for="item-mpn">MPN:</label>
        <input
          type="text"
          class="text-input"
          id="item-mpn"
          name="item-mpn"
          readonly
          v-model="model.ITEM_MPN"
        />
      </div>
      <div class="form-row-table">
        <table class="table-form">
          <template v-for="(item, index) in ShowDataTableOuterLPN" :key="index">
            <thead>
              <tr>
                <!-- <th></th> -->
                <th>LPN</th>
                <th>Gross Weight({{ item.OUTERBOX_WEIGHTUNITOFMEASURE }})</th>
                <th>Net Weight({{ item.INNERBOX_WEIGHTUNITOFMEASURE }})</th>
                <th>Quantity</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>{{ item.OUTERBOX_LPN }}</td>
                <!-- <td>LPN</td> -->
                <td>{{ item.OUTERBOX_GROSSWEIGHT }}</td>
                <td>{{ item.OUTERBOX_NETWEIGHT }}</td>
                <td>{{ item.OUTERBOX_QTY }}</td>
              </tr>

              <tr
                v-for="(itemInnerLPN, indexInnerLPN) in ShowDataTableInnerLPN"
                :key="indexInnerLPN"
              >
                <template
                  v-for="(itemInnerLPN1, indexInnerLPN1) in itemInnerLPN"
                  :key="indexInnerLPN1"
                >
                  <td
                    v-if="
                      indexInnerLPN1 == 'INNERBOX_LPN' ||
                        // indexInnerLPN1 == 'LPN' ||
                        indexInnerLPN1 == 'INNERBOX_GROSSWEIGHT' ||
                        indexInnerLPN1 == 'INNERBOX_NETWEIGHT' ||
                        indexInnerLPN1 == 'INNERBOX_QTY'
                    "
                  >
                    {{ itemInnerLPN1 }}
                  </td>
                </template>
              </tr>
            </tbody>
          </template>
        </table>
      </div>

      <div class="form-row">
        <label for="fab-site">Fab Site:</label>
        <input
          type="text"
          class="text-input"
          id="fab-site"
          name="fab-site"
          readonly
          v-model="model.FAB_SITE"
        />
      </div>
      <div class="form-row">
        <label for="assembly-site">Assembly Site:</label>
        <input
          type="text"
          class="text-input"
          id="assembly-site"
          name="assembly-site"
          readonly
          v-model="model.ASSEMBLY_SITE"
        />
      </div>
      <div class="form-row">
        <label for="test-site">Test Site:</label>
        <input
          type="text"
          class="text-input"
          id="test-site"
          name="test-site"
          readonly
          v-model="model.TEST_SITE"
        />
      </div>
      <div class="form-row">
        <label for="bump-site">Bump Site:</label>
        <input
          type="text"
          class="text-input"
          id="bump-site"
          name="bump-site"
          readonly
          v-model="model.BUMP_SITE"
        />
      </div>
      <div class="form-row">
        <label for="slt-site">Slt Site:</label>
        <input
          type="text"
          class="text-input"
          id="slt-site"
          name="slt-site"
          readonly
          v-model="model.SLT_SITE"
        />
      </div>
      <div class="form-row">
        <label for="probe-site">Probe Site:</label>
        <input
          type="text"
          class="text-input"
          id="probe-site"
          name="probe-site"
          readonly
          v-model="model.PROBE_SITE"
        />
      </div>
      <div class="form-row">
        <label for="be1-site">Be1 Site:</label>
        <input
          type="text"
          class="text-input"
          id="be1-site"
          name="be1-site"
          readonly
          v-model="model.BE1_SITE"
        />
      </div>
      <div class="form-row">
        <label for="be2-site">Be2 Site:</label>
        <input
          type="text"
          class="text-input"
          id="be2-site"
          name="be2-site"
          readonly
          v-model="model.BE2_SITE"
        />
      </div>
      <div class="form-row">
        <label for="lot-grade">Lot Grade:</label>
        <input
          type="text"
          class="text-input"
          id="lot-grade"
          name="lot-grade"
          readonly
          v-model="model.LOT_GRADE"
        />
      </div>
      <div class="form-row">
        <label for="manufacturer">Manufacturer:</label>
        <input
          type="text"
          class="text-input"
          id="manufacturer"
          name="manufacturer"
          readonly
          v-model="model.MANUFACTURER"
        />
      </div>
      <div class="form-row">
        <label for="date-code">Date Code:</label>
        <input
          type="text"
          class="text-input"
          id="date-code"
          name="date-code"
          readonly
          v-model="model.DATE_CODE"
        />
      </div>
      <div class="form-row">
        <label for="seal-date">Seal Date:</label>
        <input
          type="text"
          class="text-input"
          id="seal-date"
          name="seal-date"
          readonly
          v-model="model.SEAL_DATE"
        />
      </div>
      <div class="form-row">
        <label for="wafer-qty">Wafer Qty:</label>
        <input
          type="text"
          class="text-input"
          id="wafer-qty"
          name="wafer-qty"
          readonly
          v-model="model.WAFERQTY"
        />
      </div>
      <div class="form-row">
        <label for="subcontractors1">Subcontractors1:</label>
        <input
          type="text"
          class="text-input"
          id="subcontractors1"
          name="subcontractors1"
          readonly
          v-model="model.MODULE_ASSY_SUBCONTRACTORS1"
        />
      </div>
      <div class="form-row">
        <label for="subcontractors2">Subcontractors2:</label>
        <input
          type="text"
          class="text-input"
          id="subcontractors2"
          name="subcontractors2"
          readonly
          v-model="model.MODULE_ASSY_SUBCONTRACTORS2"
        />
      </div>
      <div class="form-row">
        <label for="subcontractors3">Subcontractors3:</label>
        <input
          type="text"
          class="text-input"
          id="subcontractors3"
          name="subcontractors3"
          readonly
          v-model="model.MODULE_ASSY_SUBCONTRACTORS3"
        />
      </div>
      <div class="form-row">
        <label for="country-fabrication">Country Fabrication:</label>
        <input
          type="text"
          class="text-input"
          id="country-fabrication"
          name="country-fabrication"
          readonly
          v-model="model.COUNTRY_OF_FABRICATION"
        />
      </div>
      <div class="form-row">
        <label for="country-assembly">Country Assembly:</label>
        <input
          type="text"
          class="text-input"
          id="country-assembly"
          name="country-assembly"
          readonly
          v-model="model.COUNTRY_OF_ASSEMBLY"
        />
      </div>
      <div class="form-row">
        <label for="country-origin">Country Origin:</label>
        <input
          type="text"
          class="text-input"
          id="country-origin"
          name="country-origin"
          readonly
          v-model="model.COUNTRY_OF_ORIGIN"
        />
      </div>
    </div>
    <div  class="button-areas" v-if="isShowForm === true && isShowSubmitForm === true">
      <div class="submit-btn">
        <input
          type="button"
          id="submit-btn"
          value="Submit"
          @click="SubmitBtn('actionSubmit')"
        />
      </div>
      <div class="reject-btn">
        <input
          type = "button"
          id = "reject-btn"
          value = "Reject"
          @click = "SubmitBtn('actionReject')"
        />
      </div>
    </div>
    <!-- table inside -->
    <div class="main-contain" v-if="isShowForm === false">
      <div class="row col-sm-12 div-content">
        <template v-if="DataTableHeader">
          <table id="tableMain" class="table mytable">
            <thead v-if="DataTable.length > 0">
              <tr>
                <template v-for="(item, index) in DataTableHeader" :key="index">
                  <th
                    v-if="
                      item == 'PACKSLIP_NO' ||
                      item== 'STATUS' ||
                      item == 'CREATE_TIME' ||
                      item == 'PO_NO' ||
                      item == 'PO_LINE' ||
                      item == 'ITEM_NO' ||
                      item == 'ITEM_SHIPPEDQTY' ||
                      item == 'ITEM_MPN' ||
                      item == 'ITEM_DESCRIPTION' ||
                      item == 'LOT_NO' ||
                      item == 'RECEIVER_LOCATION' ||
                      item == 'RECEIVER_NAME' ||
                      item == 'PALLET_LPN' ||
                      item == 'PAL_GROSSWEIGHT' ||
                      item == 'PAL_NETWEIGHT'
                    "
                  >
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <tbody>
              <template v-for="(item, index) in DataTable" :key="index">
                <tr>
                  <template v-for="(item1, index1) in item" :key="index1">
                    <td
                      @click="index1 == 'PACKSLIP_NO' && ShowDetail(index)"
                      :style="{ 
                        backgroundColor: (index1 === 'STATUS' && item1 ==='WAIT TO CONFIRM' ? 'rgb(236, 12, 12)' : '') ||
                        (index1 === 'STATUS' && item1 ==='WAIT TO RECEIVE' ? 'rgb(224 236 12)' : '') ||
                        (index1 === 'STATUS' && item1 ==='COMPLETED' ? 'rgb(43 226 66)' : '')
                        }"
                      v-if="
                        index1 == 'PACKSLIP_NO' ||
                        index1 == 'STATUS' ||
                        index1 == 'CREATE_TIME' ||
                        index1 == 'PO_NO' ||
                        index1 == 'PO_LINE' ||
                        index1 == 'ITEM_NO' ||
                        index1 == 'ITEM_SHIPPEDQTY' ||
                        index1 == 'ITEM_MPN' ||
                        index1 == 'ITEM_DESCRIPTION' ||
                        index1 == 'LOT_NO' ||
                        index1 == 'RECEIVER_LOCATION' ||
                        index1 == 'RECEIVER_NAME' ||
                        index1 == 'PALLET_LPN' ||
                        index1 == 'PAL_GROSSWEIGHT' ||
                        index1 == 'PAL_NETWEIGHT'
                      "
                    >
                      {{ item1 }}
                    </td>
                  </template>
                </tr>
              </template>
            </tbody>
          </table>
        </template>
      </div>
    </div>
  </div>
</template>
<script>
import xlsx from "xlsx";
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      showTimeForm: false,
      isShowSubmitForm: false,
      showError: false,
      dateFrom: new Date(),
      dateTo: new Date(),
      isShowForm: false,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      ShowDataDetail: [],
      ShowDataTableOuterLPN: [],
      ShowDataTableInnerLPN: [],
      columnName: [],
      valueSearch: "",
      model: {
        database_name: localStorage.databaseName,
        EMP_NO: localStorage.username,
        Total_Received_Qty: "",
        Total_Accepted_Qty: "",
        Inner_LPN: "",
        Box_Received_Qty: "",
        Box_Accepted_Qty: "",
        PACKSLIP_NO: "",
        SHIP_MCMN_CREATTIME: "",
        MSG_SENDER_NAME: "",
        MSG_RECEIVER_NAME: "",
        PO_NO: "",
        FREIGHT_CARRIER_CODE: "",
        SHIP_MCMN_TOTALOUTERCONTAINERS: "",
        NETWEIGHT: "",
        GROSSWEIGHT: "",
        SHIP_MCMN_ESTIMATEDDOCKTIME: "",
        SUPPLIER_NAME: "",
        POLINE_NO: "",
        ITEM_NO: "",
        ITEM_SHIPPEDQTY: "",
        ITEM_UNITOFMEASURE: "",
        SHIP_MCMN_LOCATIONNAME: "",
        ITEM_MPN: "",
        ITEM_DESCRIPTION: "",
        LOT_NO: "",
        RECEIVER_LOCATIONNAME: "",
        RECEIVER_NAME: "",
        FLAG: "",
        F_ID: "",
        AIRWAYBILL: "",
        COUNTRY_CODE: "",
        POSTAL_CODE: "",
        SHIP_MCMN_CITY: "",
        SHIP_MCMN_STREET1: "",
        SHIP_MCMN_STREET2: "",
        SHIP_MCMN_STREET3: "",
        SHIP_MCMN_LPN: "",
        PAL_GROSSWEIGHT: "",
        PAL_NETWEIGHT: "",
        PAL_WIDTH: "",
        PAL_LENGTH: "",
        PAL_HEIGHT: "",
        FAB_SITE: "",
        ASSEMBLY_SITE: "",
        TEST_SITE: "",
        BUMP_SITE: "",
        SLT_SITE: "",
        PROBE_SITE: "",
        BE1_SITE: "",
        BE2_SITE: "",
        DATE_CODE: "",
        SEAL_DATE: "",
        WAFERQTY: "",
        MODULE_ASSY_SUBCONTRACTORS1: "",
        MODULE_ASSY_SUBCONTRACTORS2: "",
        MODULE_ASSY_SUBCONTRACTORS3: "",
        LOT_GRADE: "",
        MANUFACTURER: "",
        COUNTRY_OF_FABRICATION: "",
        COUNTRY_OF_ASSEMBLY: "",
        COUNTRY_OF_ORIGIN: "",
      },
    };
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.isVisible = false;
      }
    });
  },
  computed: {},
  mounted() {
    this.LoadComponent();
  },
  methods: {
    async SubmitBtn(actionBtn) {
        let titleValue = "";
        let textValue = "";
        titleValue = "Are you sure edit?";
        textValue = "Once OK, data will be updated!";
        this.$swal({
          title: titleValue,
          text: textValue,
          icon: "warning",
          buttons: true,
          dangerMode: true,
        }).then(async (willDelete) => {
          if (willDelete.isConfirmed == false) return;
          let payload = {
            EMP_NO: localStorage.username,
            database_name: localStorage.databaseName,
            PACKSLIP_NO: this.model.PACKSLIP_NO,
            FLAG: this.model.FLAG,
            F_ID: this.model.F_ID,
            actionBtn: actionBtn
          };
          console.log("payload: ", payload);
          try {
            let { data } = await Repository.getRepo("InsertQasn_in", payload);
            console.log("data,", data);
            if (data.result == "ok") {
              this.ClearForm();
              this.$swal("", "Successfully applied", "success");
            } else if (data.result == "notPrivilege") {
              if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
              } else {
                this.$swal("", "Bạn không có quyền thêm sửa", "error");
              }
            }
             else {
              this.$swal("", data.result, "error");
            }
          } catch (error) {
            if (error.response && error.response.data) {
              this.$swal("", error.response.data.error, "error");
            } else {
              this.$swal("", error.Message, "error");
            }
          }
        });
    },
    async LoadComponent() {
      let databaseName = localStorage.databaseName;
      try {
        let { data } = await Repository.getApiServer(
          `GetLoadFormQASN_IN?database_name=${databaseName}`
        );
        this.DataTable = [];
        this.DataTableHeader = [];
        this.DataTable = data.data;
        if (this.DataTable.length > 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
        }
      } catch (error) {
        if (error.response && error.response.data) {
          this.$swal("", error.response.data.error, "error");
        } else {
          this.$swal("", error.Message, "error");
        }
      }
    },
    async QuerySearch() {
      const formatDate = (date) => {
        if (!date) return "";
        return `${this.pad(date.getFullYear())}${this.pad(
          date.getMonth() + 1
        )}${this.pad(date.getDate())}`;
      };
      const dateFrom = formatDate(this.dateFrom);
      const dateTo = formatDate(this.dateTo);
      let databaseName = localStorage.databaseName;
      let PACKSLIP_NO = this.valueSearch;
      let showTimeForm = this.showTimeForm;
      try {
        let { data } = await Repository.getApiServer(
          `GetQASN_IN?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}&dateFrom=${dateFrom}&dateTo=${dateTo}&showTimeForm=${showTimeForm}`
        );
        this.DataTable = [];
        this.DataTableHeader = [];
        this.DataTable = data.data;
        if (this.DataTable.length > 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
        }
      } catch (error) {
        if (error.response && error.response.data) {
          this.$swal("", error.response.data.error, "error");
        } else {
          this.$swal("", error.Message, "error");
        }
      }
    },
    async ShowDetail(index) {
      let databaseName = localStorage.databaseName;
      let PACKSLIP_NO = this.DataTable[index].PACKSLIP_NO;
      let FLAG = this.DataTable[index].FLAG;
      let F_ID = this.DataTable[index].F_ID;
      if (FLAG == "2") {
        this.isShowSubmitForm = true;
      }
      try {
        let responseData = await Repository.getApiServer(
          `GetShowDetail?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}&FLAG=${FLAG}&F_ID=${F_ID}`
        );
        this.ShowDataDetail = [];
        this.ShowDataDetail = responseData.data.data;
        this.ShowDataTableOuterLPN = responseData.data.dataTableOuterLPN;
        this.ShowDataTableInnerLPN = responseData.data.dataTableInnerLPN;
        console.log("ShowDataTableOuterLPN: ", this.ShowDataTableOuterLPN);
        if (this.ShowDataDetail.length > 0) {
          let firstItem = this.ShowDataDetail[0];
          this.model.PACKSLIP_NO = firstItem.PACKSLIP_NO;
          this.model.FLAG = firstItem.FLAG;
          this.model.F_ID = firstItem.F_ID;
          this.model.SHIP_MCMN_CREATTIME = firstItem.SHIP_MCMN_CREATTIME;
          this.model.MSG_SENDER_NAME = firstItem.MSG_SENDER_NAME;
          this.model.MSG_RECEIVER_NAME = firstItem.MSG_RECEIVER_NAME;
          this.model.PO_NO = firstItem.PO_NO;
          this.model.POLINE_NO = firstItem.POLINE_NO;
          this.model.ITEM_NO = firstItem.ITEM_NO;
          this.model.ITEM_SHIPPEDQTY = firstItem.ITEM_SHIPPEDQTY;
          this.model.ITEM_UNITOFMEASURE = firstItem.ITEM_UNITOFMEASURE;
          this.model.ITEM_MPN = firstItem.ITEM_MPN;
          this.model.ITEM_DESCRIPTION = firstItem.ITEM_DESCRIPTION;
          this.model.LOT_NO = firstItem.LOT_NO;
          this.model.RECEIVER_LOCATIONNAME = firstItem.RECEIVER_LOCATIONNAME;
          this.model.RECEIVER_NAME = firstItem.RECEIVER_NAME;
          this.model.SHIP_MCMN_LOCATIONNAME = firstItem.SHIP_MCMN_LOCATIONNAME;
          //new
          this.model.GROSSWEIGHT = firstItem.GROSSWEIGHT;
          this.model.NETWEIGHT = firstItem.NETWEIGHT;
          this.model.FAB_SITE = firstItem.FAB_SITE;
          this.model.ASSEMBLY_SITE = firstItem.ASSEMBLY_SITE;
          this.model.TEST_SITE = firstItem.TEST_SITE;
          this.model.BUMP_SITE = firstItem.BUMP_SITE;
          this.model.SLT_SITE = firstItem.SLT_SITE;
          this.model.PROBE_SITE = firstItem.PROBE_SITE;
          this.model.BE1_SITE = firstItem.BE1_SITE;
          this.model.BE2_SITE = firstItem.BE2_SITE;
          this.model.DATE_CODE = firstItem.DATE_CODE;
          this.model.SEAL_DATE = firstItem.SEAL_DATE;
          this.model.WAFERQTY = firstItem.WAFERQTY;
          this.model.MODULE_ASSY_SUBCONTRACTORS1 =
            firstItem.MODULE_ASSY_SUBCONTRACTORS1;
          this.model.MODULE_ASSY_SUBCONTRACTORS2 =
            firstItem.MODULE_ASSY_SUBCONTRACTORS2;
          this.model.MODULE_ASSY_SUBCONTRACTORS3 =
            firstItem.MODULE_ASSY_SUBCONTRACTORS3;
          this.model.LOT_GRADE = firstItem.LOT_GRADE;
          this.model.MANUFACTURER = firstItem.MANUFACTURER;
          this.model.COUNTRY_OF_FABRICATION = firstItem.COUNTRY_OF_FABRICATION;
          this.model.COUNTRY_OF_ASSEMBLY = firstItem.COUNTRY_OF_ASSEMBLY;
          this.model.COUNTRY_OF_ORIGIN = firstItem.COUNTRY_OF_ORIGIN;
          this.model.SHIP_MCMN_ESTIMATEDDOCKTIME =
            firstItem.SHIP_MCMN_ESTIMATEDDOCKTIME;
          this.model.SHIP_MCMN_TOTALOUTERCONTAINERS =
            firstItem.SHIP_MCMN_TOTALOUTERCONTAINERS;
          this.model.FREIGHT_CARRIER_CODE = firstItem.FREIGHT_CARRIER_CODE;
          this.model.SUPPLIER_NAME = firstItem.SUPPLIER_NAME;
          this.model.AIRWAYBILL = firstItem.AIRWAYBILL;
          this.model.COUNTRY_CODE = firstItem.COUNTRY_CODE;
          this.model.POSTAL_CODE = firstItem.POSTAL_CODE;
          this.model.SHIP_MCMN_CITY = firstItem.SHIP_MCMN_CITY;
          this.model.SHIP_MCMN_STREET1 = firstItem.SHIP_MCMN_STREET1;
          this.model.SHIP_MCMN_STREET2 = firstItem.SHIP_MCMN_STREET2;
          this.model.SHIP_MCMN_STREET3 = firstItem.SHIP_MCMN_STREET3;
          this.model.SHIP_MCMN_LPN = firstItem.SHIP_MCMN_LPN;
          this.model.PAL_GROSSWEIGHT = firstItem.PAL_GROSSWEIGHT;
          this.model.PAL_NETWEIGHT = firstItem.PAL_NETWEIGHT;
          this.model.PAL_WIDTH = firstItem.PAL_WIDTH;
          this.model.PAL_LENGTH = firstItem.PAL_LENGTH;
          this.model.PAL_HEIGHT = firstItem.PAL_HEIGHT;
          this.isShowForm = true;
        }
      } catch (error) {
        if (error.response && error.response.data) {
          this.$swal("", error.response.data.error, "error");
        } else {
          this.$swal("", error.Message, "error");
        }
      }
    },
    pad(number) {
      return number < 10 ? `0${number}` : `${number}`;
    },
    exportexcelxlsx() {
      const filteredData = this.DataTable.map((element) => {
        return Object.keys(element).reduce((acc, key) => {
          if (key !== "FLAG" && key !== "F_ID") {
            acc[key] = element[key];
          }
          return acc;
        }, {});
      });
      let ws = xlsx.utils.json_to_sheet(filteredData);
      /* add to workbook */
      let wb = xlsx.utils.book_new();
      xlsx.utils.book_append_sheet(wb, ws, "data");
      /* generate an XLSX file */
      xlsx.writeFile(wb, "download.xlsx");
    },
    exportExcel() {
      const filteredData = this.DataTable.map((element) => {
        return Object.keys(element).reduce((acc, key) => {
          if (key !== "FLAG" && key !== "F_ID") {
            acc[key] = element[key];
          }
          return acc;
        }, {});
      });
      const items = filteredData;
      const replacer = (key, value) => (value === null ? "" : value);
      const header = Object.keys(items[0]);
      const csv = [
        header.join(","), // header row first
        ...items.map((row) =>
          header
            .map((fieldName) => JSON.stringify(row[fieldName], replacer))
            .join(",")
        ),
      ].join("\r\n");
      let downloadLink = document.createElement("a");
      let blob = new Blob(["\ufeff", csv]);
      let url = URL.createObjectURL(blob);
      downloadLink.href = url;
      downloadLink.download = "data.csv";
      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);
    },
    ClearForm() {
      this.isShowSubmitForm = false;
    },
    ReturnForm() {
      this.isShowForm = false;
      this.isShowSubmitForm = false;
      this.model.PACKSLIP_NO = "";
      this.model.FLAG = "";
      this.model.F_ID = "";
      this.LoadComponent();
    },
    BackToParent() {
      this.$router.push({ path: "/Home/Qualcomm_Application" });
    },
  },
};
</script>

<style lang="scss" scoped>
.div-all {
  font-family: Arial, sans-serif;
  margin: 0;
  padding: 10px;
  background-color: #f9f9f9;
}
.searchbox-time {
  height: 40px;
}
.searchbox-time1 {
  margin-right: 5px;
}
.form-control:disabled,
.form-control[readonly] {
  background-color: #e9ecef;
  opacity: 1;
  height: 35px;
  border-radius: 3px;
}
.searchbox {
  display: inline-block;
  margin-bottom: 5px;
  width: 30%;
  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 10px;
  background-color: #f9f9f9;
}
.btn-button {
  border-radius: 5px;
  padding: 6px 20px;
  height: 40px;
  border: none;
  background: #f18436;
  color: #fff;
  box-sizing: 0;
  &:hover {
    background: #ec690c;
    transition: 0.5s;
  }
}
.searchbox-content {
  display: flex;
  gap: 5px;
  margin-bottom: 1px;
}
.searchbox-timerange {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px;
}
.datepicker-wrapper {
  flex: 1;
}
.export-excel {
  display: inline-block;
  right: 25px;
  position: absolute;
}
.img-excel {
  height: 40px;
  width: 40px;
  cursor: pointer;
}

.container {
  display: grid;
  grid-template-rows: 50px repeat(4, 35px) auto 5px repeat(4, 35px) auto repeat(7, 35px);
  grid-template-columns: 1fr 1fr 1fr;
  align-content: space-around;
  box-sizing: border-box;
  background: #e6e6e2;
  padding: 0 20px 0px 20px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  position: relative;
  font-size: 16px;
  border-radius: 5px;
  overflow-x: auto;
  width: 85%;
  row-gap: 5px;
  height: 600px;
  .text-input {
    width: 50%;
    padding: 5px;
    border: none;
    box-sizing: border-box;
    border: none;
    resize: vertical;
    background-color: #e6e6e2;
  }
  label {
    font-size: 16px;
    font-weight: 555;
    color: #141414;
    margin-left: 15px;
  }
  p {
    color: #141414;
  }
}
.title-class {
  grid-column-start: 1;
  grid-column-end: 4;
  text-align: center;
  font-size: 35px;
  color: #0e0d0d;
  font-family: serif;
  font-style: italic;
}
.title-class p {
  margin: 0;
}
//submit-form
.button-areas {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 45px;
}
// .submit-btn .reject-btn {
//   text-align: center;
// }
.submit-btn input {
  margin-right: 5px;
}
#submit-btn {
  font-weight: 555;
  margin-top: 5px;
  padding: 8px 11px;
  font-size: 17px;
  width: 80px;
  text-align: center;
  cursor: pointer;
  outline: none;
  color: #fff;
  background-color: #04aa6d;
  border: none;
  border-radius: 14px;
  box-shadow: 0 5px #999;
}
#submit-btn:hover {
  background-color: #3e8e41;
}
#submit-btn:active {
  background-color: #3e8e41;
  box-shadow: 0 5px #666;
  transform: translateY(4px);
}
//reject -button
#reject-btn {
  font-weight: 555;
  margin-top: 5px;
  padding: 8px 11px;
  font-size: 17px;
  width: 80px;
  text-align: center;
  cursor: pointer;
  outline: none;
  color: #fff;
  background-color: #e2651d;
  border: none;
  border-radius: 14px;
  box-shadow: 0 5px #999;
}
#reject-btn:hover {
  background-color: #8e3e3e;
}
#reject-btn:active {
  background-color: #8e3e3e;
  box-shadow: 0 5px #666;
  transform: translateY(4px);
}

input#clear-form {
  background-color: #f8c323;
  color: #000;
  padding: 8px 12px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  &:hover {
    background-color: #e0a800;
  }
}
input#return-form {
  background-color: #f77225;
  color: #000;
  padding: 8px 12px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  &:hover {
    background-color: #f76613;
  }
}
.form-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.actual-ship-from {
  display: grid;
  grid-template-rows: repeat(4, 35px);
  grid-template-columns: 1fr 1fr 1fr;
  grid-column-start: 1;
  grid-column-end: 4;
  border-radius: 5px;
  position: relative;
  align-content: space-around;
  //border: 2px solid #9f998b;
  margin-top: 10px;
  padding: 10px 0px 5px 0px;
  .form-row-actual {
    display: flex;
    justify-content: space-between;
  }
}
.title-actual-ship {
  position: absolute;
  top: -12px;
  left: 20%;
  transform: translateX(-50%);
  background-color: #e6e6e2;
  font-size: 16px;
  font-style: italic;
  color: #565353;
  font-weight: 555;
  padding: 0 5px;
}
.form-row-actual-all {
  display: grid;
  grid-template-rows: 40px 40px 40px;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr;
  grid-row-start: 2;
  grid-row-end: 5;
  grid-column-start: 1;
  grid-column-end: 4;
}

.form-row-actual-all-label {
  grid-column-start: 1;
  grid-column-end: 2;
  grid-row-start: 1;
  grid-row-end: 4;
  display: flex;
  justify-content: flex-start;
  align-items: center;
}

.form-row-actual-all-input {
  grid-column-start: 2;
  grid-column-end: 6;
  display: flex;
  justify-content: flex-start;
}
.form-row-actual-all-input input {
  width: 80%;
  border: none;
  margin-bottom: 2px;
  border-radius: 5px;
}

.form-row-table {
  display: grid;
  grid-template-rows: auto;
  grid-column-start: 1;
  grid-column-end: 4;
}
.class-hr{
    grid-column: 1/4;
}
.class-hr hr {
    margin: 0 auto;
    padding: 0;
    width: 80%;
    color: #000;
}
.table-form {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 5px;
}

.table-form th,
td {
  border: 1px solid #0f0f0f;
  padding: 5px;
  //text-align: center;
  color: #424040;
}

.table-form th {
  color: #424040;
  font-weight: bold;
  background-color: #bbbb9c;
}

.td-show {
  text-align: center;
  justify-content: center;
  background: #f88838;
  color: #fff;
  height: 30px;
  align-items: center;
  align-content: center;
  cursor: pointer;
  margin: 10px;
  &:hover {
    background: #db6008;
  }
}
.div-searchbox {
  margin-bottom: 5px;
  margin-top: 15px;
  height: 40px;
  display: flex;
  align-content: center;
  justify-content: left;
  .div-searchbox-content {
    display: flex;
    margin-bottom: 10px;
    text-align: center;
    height: 40px;
  }
  .div-searchbox-content input {
    border-radius: 5px;
    width: 300px;
    height: 40px;
  }
  .div-searchbox-content button {
    border-radius: 5px;
    padding: 6px 20px;
    height: 40px;
    background: #ff7a1c;
    color: #fff;
    box-sizing: 0;
    &:hover {
      background: #f88838;
    }
  }
}
.div-config-name {
  margin-left: 20px;
  line-height: 50px;
  span {
    font-weight: 555;
    font-size: 17px;
  }
}
.main-contain {
  height: 500px;
  overflow: auto;
}
.div-back {
  float: left;
  background: #eae1e1;
  cursor: pointer;
  margin: 10px 0;
  display: flex;
  align-content: center;
  align-items: center;
  width: 3%;
  border-radius: 10%;
  &:hover {
    background: #b7b7b7;
  }
  .back-icon {
    height: 20px;
    width: 20px;
  }
}

.mytable {
  margin-top: 0px;
  overflow: auto;
  thead {
    th:first {
      border-radius: 20%;
    }
    th {
      background-color: #024873;
      position: sticky;
      top: 0;
      z-index: 2;
      color: #fff;
      min-width: 60px;
      padding: 3px;
      font-size: 16px;
      padding: 0.5rem 0.5rem;
    }
  }
  tr {
    &:hover {
      background: #89cfed;
    }
    td:nth-child(1) {
      cursor: pointer;
      text-decoration: underline;

    }
    td {
      overflow-x: auto;
      white-space: nowrap;
      z-index: 1;
      padding: 2px;
      min-width: 60px;
      border: 0.5px solid #cdc;
      font-size: 17px;
      color: #000;
    }
  }
}
</style>
