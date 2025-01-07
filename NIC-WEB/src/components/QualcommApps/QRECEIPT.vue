<template>
  <div class="div-all">
    <header class="row header">
      <div class="div-back" @click="isShowForm ? ReturnForm() : BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="row div-config-name">
        <span>QReceipt</span>
      </div>
    </header>
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
          <input id="showTimeForm" type="checkbox" v-model="showTimeForm" />
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
    <!-- Form input data -->
    <div class="container" v-if="isShowForm === true">
      <div class="title-class">
        <p>Shipment Receipt Notice</p>
      </div>
      <div class="form-row">
        <label for="ship_mcmn_creattime">To:</label>
        <input
          type="text"
          class="text-input"
          id="ship_mcmn_creattime"
          name="ship_mcmn_creattime"
          readonly
          v-model="model.MSG_SENDER_NAME"
        />
      </div>
      <div class="form-row">
        <label for="msg_sender_name">From:</label>
        <input
          type="text"
          class="text-input"
          id="msg_sender_name"
          name="msg_sender_name"
          readonly
          v-model="model.MSG_RECEIVER_NAME"
        />
      </div>
      <div class="form-row">
        <label for="creat-time">Time:</label>
        <input
          type="text"
          class="text-input"
          id="creat-time"
          name="creat-time"
          readonly
          v-model="model.CREAT_TIME"
        />
      </div>
      <div class="form-row">
        <label for="po_no">Pack Slip No:</label>
        <input
          type="text"
          class="text-input"
          id="po_no"
          name="po_no"
          readonly
          v-model="model.PACKSLIP_NO"
        />
      </div>
      <div class="form-row">
        <label for="poline_no">Airway Bill:</label>
        <input
          type="text"
          class="text-input"
          id="poline_no"
          name="poline_no"
          readonly
          v-model="model.SHIP_MCMN_AIRWAYBILL"
        />
      </div>
      <div class="actual-ship-from">
        <div class="form-row-actual">
          <label for="item_shippedqty">Country Code:</label>
          <input
            type="text"
            class="text-input"
            id="item_shippedqty"
            name="item_shippedqty"
            readonly
            v-model="model.COUNTRY_CODE"
          />
        </div>
        <div class="form-row-actual">
          <label for="item_unitofmeasure">City:</label>
          <input
            type="text"
            class="text-input"
            id="item_unitofmeasure"
            name="item_unitofmeasure"
            readonly
            v-model="model.SHIP_MCMN_CITY"
          />
        </div>
        <div class="form-row-actual">
          <label for="item_unitofmeasure">Postal Code:</label>
          <input
            type="text"
            class="text-input"
            id="item_unitofmeasure"
            name="item_unitofmeasure"
            readonly
            v-model="model.POSTAL_CODE"
          />
        </div>

        <div class="form-row-actual-all">
          <div class="form-row-actual-all-label">
            <label for="packslip_no">Address:</label>
          </div>
          <div class="form-row-actual-all-input">
            <p>{{ model.SHIP_MCMN_ADDR1 }}</p>
          </div>
          <div class="form-row-actual-all-input">
            <p>{{ model.SHIP_MCMN_ADDR2 }}</p>
          </div>
          <div class="form-row-actual-all-input">
            <p>{{ model.SHIP_MCMN_ADDR3 }}</p>
          </div>
        </div>
      </div>
      <div class="class-hr">
        <hr/>
      </div>
      <div class="form-row">
        <label for="item_unitofmeasure">Pallet No:</label>
        <input
          type="text"
          class="text-input"
          id="item_unitofmeasure"
          name="item_unitofmeasure"
          readonly
          v-model="model.SHIP_MCMN_LPN"
        />
      </div>
      <div class="form-row">
        <label for="item_mpn">GrossWeight:</label>
        <input
          type="text"
          class="text-input"
          id="item_mpn"
          name="item_mpn"
          readonly
          v-model="model.PAL_GROSSWEIGHT"
        />
      </div>
      <div class="form-row">
        <label for="item_description">NetWeight:</label>
        <input
          type="text"
          class="text-input"
          id="item_description"
          name="item_description"
          readonly
          v-model="model.PAL_NETWEIGHT"
        />
      </div>
      <div class="form-row">
        <label for="lot_no">Width:</label>
        <input
          type="text"
          id="lot_no"
          class="text-input"
          name="lot_no"
          readonly
          v-model="model.PAL_WIDTH"
        />
      </div>
      <div class="form-row">
        <label for="receiver_locationname">Length:</label>
        <input
          type="text"
          class="text-input"
          id="receiver_locationname"
          name="receiver_locationname"
          readonly
          v-model="model.PAL_LENGTH"
        />
      </div>
      <div class="form-row">
        <label for="receiver_name">Height:</label>
        <input
          type="text"
          class="text-input"
          id="receiver_name"
          name="receiver_name"
          readonly
          v-model="model.PAL_HEIGHT"
        />
      </div>
      <!-- itemno -->
      <div class="form-row">
        <label for="lot_no">Items No:</label>
        <input
          type="text"
          id="lot_no"
          class="text-input-width"
          name="lot_no"
          readonly
          v-model="model.ITEM_NO"
        />
      </div>
      <div class="form-row">
        <label for="receiver_locationname">QTY:</label>
        <input
          type="text"
          class="text-input-width"
          id="receiver_locationname"
          name="receiver_locationname"
          readonly
          v-model="model.ITEM_SHIPPEDQTY"
        />
      </div>
      <div class="form-row">
        <label for="receiver_name">MPN:</label>
        <input
          type="text"
          class="text-input-width"
          id="receiver_name"
          name="receiver_name"
          readonly
          v-model="model.ITEM_MPN"
        />
      </div>
      <!-- end -->
      <div class="form-row-table">
        <table class="table-form">
          
            <thead>
              <tr>
                <template v-for="(item, index) in ShowDataTableOuterLpnHeader" :key="index"> 
                  <th v-if="item == 'LOT_NO' || 
                    item == 'OUTERBOX' ||
                    item == 'INNERBOX' ||
                    item == 'SHIPPEDQTY' ||
                    item == 'RECEIVEQTY' ||
                    item == 'REJECTQTY' ||
                    item == 'REASON'
                  ">
                  {{ item }}
                  </th>
                </template>
                  <!-- <th>Receive Quantity</th>
                  <th>Reject Quantity</th>
                  <th>Reason</th> -->
              </tr>
            </thead>
            <tbody v-if="isShowSubmitForm === false">
              <tr
                v-for="(item, index) in ShowDataTableOuterLpn" :key="index">
                <template v-for="(value, key) in item" :key="key">
                  <td
                    v-if="
                      key == 'LOT_NO' ||
                      key == 'OUTERBOX' ||
                      key == 'INNERBOX' ||
                      key == 'SHIPPEDQTY' ||
                      key == 'RECEIVEQTY' || 
                      key == 'REJECTQTY' ||
                      key == 'REASON'
                    "
                  > 
                    {{ value }}
                  </td>
                </template>
              </tr>
            </tbody>
            <tbody v-else>
              <tr
                v-for="(item, index) in ShowDataTableOuterLpn" :key="index">
                <template v-for="(value, key) in item" :key="key">
                  <td
                    v-if="
                      key == 'LOT_NO' ||
                      key == 'OUTERBOX' ||
                      key == 'INNERBOX' ||
                      key == 'SHIPPEDQTY'
                    "
                  > 
                    {{ value }}
                  </td>
                </template>
                <td>
                  <input 
                    type="number"
                    autocomplete="off"
                    class="class-re-qty"
                    max="99999"
                    min="0"
                    v-model="rowsInnerBox[index].receiveQty"
                    @input="limitInputLength($event, 5)"
                  />
                </td>
                <td>
                  <input 
                    type="number"
                    autocomplete="off"
                    class="class-re-qty"
                    max="99999"
                    min="0"
                    v-model="rowsInnerBox[index].rejectQty"
                    @input="limitInputLength($event, 5)"
                  />
                </td>
                <td>
                  <input 
                    type="text"
                    autocomplete="off"
                    v-model="rowsInnerBox[index].reason"
                  />
                </td>
              </tr>
            </tbody>
        </table>
      </div>
      <!-- <div class="form-row-actual-address">
        <label for="box_received">Receive address:</label>
        <select>
          <option v-for="(item, index) in ShowDataReceiveAddress" :key="index">
            {{ item.SHIP_ADDRESS }}
          </option>
        </select>
      </div> -->
      <div class="form-row-center">
        <p>Country: Viet Nam, City: Bac Giang, Postal Code: 230000</p>
      </div>
    </div>
    <div
      class="submit-form"
      v-if="isShowForm === true && isShowSubmitForm === true"
    >
      <input
        type="button"
        id="submit-form"
        value="Submit"
        @click="SubmitForm()"
      />
    </div>
    <div
      class="submit-form"
      v-if="isShowForm === true && isShowSubmitForm === false"
    ></div>
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
                        item == 'STATUS' ||
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
                <tr v-for="(row, index) in DataTable" :key="index">
                 <template v-for="(value, key) in row" :key="key" >
                    <td
                     @click="key === 'PACKSLIP_NO' && ShowDetail(index)"
                      :style="{ 
                        backgroundColor: key === 'STATUS' && value ==='COMPLETED' ? 'rgb(43 226 66)' 
                        :key === 'STATUS' && value ==='WAIT TO RECEIVE' ? 'rgb(224 236 12)' : ''
                        }"
                      v-if="
                        key === 'PACKSLIP_NO' ||
                        key === 'STATUS' ||
                        key === 'CREATE_TIME' ||
                        key === 'PO_NO' ||
                        key === 'PO_LINE' ||
                        key === 'ITEM_NO' ||
                        key === 'ITEM_SHIPPEDQTY' ||
                        key === 'ITEM_MPN' ||
                        key === 'ITEM_DESCRIPTION' ||
                        key === 'LOT_NO' ||
                        key === 'RECEIVER_LOCATION' ||
                        key === 'RECEIVER_NAME' ||
                        key === 'PALLET_LPN' ||
                        key === 'PAL_GROSSWEIGHT' ||
                        key === 'PAL_NETWEIGHT'
                      "
                    >
                      {{ value }}
                    </td>
                </template>
                </tr>
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
      rowsInnerBox: [],
      showTimeForm: false,
      showError: false,
      dateFrom: new Date(),
      dateTo: new Date(),
      isShowSubmitForm: true,
      isShowForm: false,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      ShowDataDetail: [],
      ShowDataTableOuterLpn: [],
      ShowDataTableOuterLpnHeader: [],
      ShowDataReceiveAddress: [],
      columnName: [],
      valueSearch: "",
      model: {
        database_name: localStorage.databaseName,
        EMP_NO: localStorage.username,
        SITE: "",
        PIP_TYPE: "",
        PO_TYPE: "",
        CREAT_TIME: "",
        MSG_ID: "",
        MSG_SENDER_NAME: "",
        MSG_SENDER_DUNS: "",
        MSG_RECEIVER_NAME: "",
        MSG_RECEIVER_DUNS: "",
        PACKSLIP_NO: "",
        SHIP_MCMN_AIRWAYBILL: "",
        SHIP_MCMN_FREIGHT_CARRIER_CODE: "",
        IMPORT_PERMIT_NO: "",
        LOCATION_CODE: "",
        F_NAME: "",
        RECEIVER_DUNS: "",
        RECEIVER_DUNS4: "",
        DATE_TIME: "",
        SHIP_MCMN_LOCATIONNAME: "",
        SHIP_MCMN_CITY: "",
        SHIP_MCMN_COUNTRYCODE: "",
        SHIP_MCMN_POSTALCODE: "",
        SHIP_MCMN_ADDR1: "",
        SHIP_MCMN_ADDR2: "",
        SHIP_MCMN_ADDR3: "",
        PO_NO: "",
        PO_LINE: "",
        ITEMSHIP_MCMN_UNITOFMEASURE: "",
        ITEMSHIP_MCMN_NO: "",
        ITEMSHIP_MCMN_RECEIVEDQTY: "",
        ITEMSHIP_MCMN_ACCEPTEDQTY: "",
        INNERBOX_SHIP_MCMN_LPN: "",
        INNERBOX_SHIP_MCMN_RECEIVEDQTY: "",
        LAST_EDIT_TIME: "",
        GROSSWEIGHT: "",
        NETWEIGHT: '',
        COUNTRY_CODE: "",
        POSTAL_CODE: "",
        SHIP_MCMN_LPN: "",
        PAL_GROSSWEIGHT: "",
        PAL_NETWEIGHT: "",
        PAL_WIDTH: "",
        PAL_LENGTH: "",
        PAL_HEIGHT: "",
        ITEM_SHIPPEDQTY: '',
        ITEM_MPN: "",
        ITEM_NO: ''
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
  watch: {
    ShowDataTableOuterLpn: {
      immediate: true,
      handler(newValue) {
        this.rowsInnerBox = newValue.map(() => ({
          receiveQty: '',
          rejectQty: '',
          reason: ''
        }));
      }
    }
  },
  computed: {},
  mounted() {
    this.LoadComponent();
    this.initializeRowsInnerBox();
  },
  methods: {
    async LoadComponent() {
      let databaseName = localStorage.databaseName;
      try {
        let { data } = await Repository.getApiServer(
          `GetLoadFormQReceipt?database_name=${databaseName}`
        );
        this.DataTable = [];
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
          `GetDataQReciept?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}&dateFrom=${dateFrom}&dateTo=${dateTo}&showTimeForm=${showTimeForm}`
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
      if (FLAG != "0") {
        this.isShowSubmitForm = false;
      }
      try {
        let responseData = await Repository.getApiServer(
          `GetShowDetailQReceipt?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}`
        );
        this.ShowDataDetail = [];
        this.ShowDataDetail = responseData.data.data;
        this.ShowDataTableOuterLpn = responseData.data.dataTableOuterLPN;
        if(this.ShowDataTableOuterLpn.length > 0) {
          this.ShowDataTableOuterLpnHeader = Object.keys(this.ShowDataTableOuterLpn[0]);
        }
        if (this.ShowDataDetail.length > 0) {
          //start insert
          let firstItem = this.ShowDataDetail[0];
          this.model.SITE = firstItem.SITE;
          this.model.PIP_TYPE = firstItem.PIP_TYPE;
          this.model.PO_TYPE = firstItem.PO_TYPE;
          this.model.CREAT_TIME = firstItem.CREAT_TIME;
          this.model.MSG_ID = firstItem.MSG_ID;
          this.model.MSG_SENDER_NAME = firstItem.MSG_SENDER_NAME;
          this.model.MSG_SENDER_DUNS = firstItem.MSG_SENDER_DUNS;
          this.model.MSG_RECEIVER_NAME = firstItem.MSG_RECEIVER_NAME;
          this.model.MSG_RECEIVER_DUNS = firstItem.MSG_RECEIVER_DUNS;
          this.model.PACKSLIP_NO = firstItem.PACKSLIP_NO;
          this.model.SHIP_MCMN_AIRWAYBILL = firstItem.SHIP_MCMN_AIRWAYBILL;
          this.model.SHIP_MCMN_FREIGHT_CARRIER_CODE = firstItem.SHIP_MCMN_FREIGHT_CARRIER_CODE;
          this.model.IMPORT_PERMIT_NO = firstItem.IMPORT_PERMIT_NO;
          this.model.LOCATION_CODE = firstItem.LOCATION_CODE;
          this.model.F_NAME = firstItem.F_NAME;
          this.model.RECEIVER_DUNS = firstItem.RECEIVER_DUNS;
          this.model.RECEIVER_DUNS4 = firstItem.RECEIVER_DUNS4;
          this.model.DATE_TIME = firstItem.DATE_TIME;
          this.model.SHIP_MCMN_LOCATIONNAME = firstItem.SHIP_MCMN_LOCATIONNAME;
          this.model.SHIP_MCMN_CITY = firstItem.SHIP_MCMN_CITY;
          this.model.SHIP_MCMN_COUNTRYCODE = firstItem.SHIP_MCMN_COUNTRYCODE;
          this.model.SHIP_MCMN_POSTALCODE = firstItem.SHIP_MCMN_POSTALCODE;
          this.model.SHIP_MCMN_ADDR1 = firstItem.SHIP_MCMN_ADDR1;
          this.model.SHIP_MCMN_ADDR2 = firstItem.SHIP_MCMN_ADDR2;
          this.model.SHIP_MCMN_ADDR3 = firstItem.SHIP_MCMN_ADDR3;
          this.model.PO_NO = firstItem.PO_NO;
          this.model.PO_LINE = firstItem.PO_LINE;
          this.model.ITEMSHIP_MCMN_UNITOFMEASURE = firstItem.ITEMSHIP_MCMN_UNITOFMEASURE;
          this.model.ITEMSHIP_MCMN_NO = firstItem.ITEMSHIP_MCMN_NO;
          this.model.ITEMSHIP_MCMN_RECEIVEDQTY = firstItem.ITEMSHIP_MCMN_RECEIVEDQTY;
          this.model.ITEMSHIP_MCMN_ACCEPTEDQTY = firstItem.ITEMSHIP_MCMN_ACCEPTEDQTY;
          this.model.INNERBOX_SHIP_MCMN_LPN = firstItem.INNERBOX_SHIP_MCMN_LPN;
          this.model.INNERBOX_SHIP_MCMN_RECEIVEDQTY = firstItem.INNERBOX_SHIP_MCMN_RECEIVEDQTY;
          this.model.LAST_EDIT_TIME = firstItem.LAST_EDIT_TIME;
          //end insert
          this.model.GROSSWEIGHT = firstItem.GROSSWEIGHT;
          this.model.NETWEIGHT = firstItem.NETWEIGHT;
          this.model.COUNTRY_CODE = firstItem.COUNTRY_CODE;
          this.model.POSTAL_CODE = firstItem.POSTAL_CODE;
          this.model.SHIP_MCMN_LPN = firstItem.SHIP_MCMN_LPN;
          this.model.PAL_GROSSWEIGHT = firstItem.PAL_GROSSWEIGHT;
          this.model.PAL_NETWEIGHT = firstItem.PAL_NETWEIGHT;
          this.model.PAL_WIDTH = firstItem.PAL_WIDTH;
          this.model.PAL_LENGTH = firstItem.PAL_LENGTH;
          this.model.PAL_HEIGHT = firstItem.PAL_HEIGHT;
          this.model.ITEM_SHIPPEDQTY = firstItem.ITEM_SHIPPEDQTY;
          this.model.ITEM_MPN = firstItem.ITEM_MPN;
          this.model.ITEM_NO = firstItem.ITEM_NO;
          this.isShowForm = true;
        } else {
          this.isShowForm = false;
        }
      } catch (error) {
        if (error.response && error.response.data) {
          this.$swal("", error.response.data.error, "error");
        } else {
          this.$swal("", error.Message, "error");
        }
      }
    },
    async SubmitForm() {
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

          const filleredRows = this.rowsInnerBox.map((row, index) => ({
            ...row,
            LOT_NO: this.ShowDataTableOuterLpn[index].LOT_NO,
            SHIPPEDQTY: this.ShowDataTableOuterLpn[index].SHIPPEDQTY
          })).filter(row => row.receiveQty || row.rejectQty || row.reason);

          if(filleredRows.length === 0) {
            this.$swal("", "No data to submit", "warning");
            return;
          }
          if(this.ShowDataTableOuterLpn.length !== filleredRows.length) {
            this.$swal("", "Please enter enough data.", "warning");
              return;
          }
          this.isInputReason = false;
          const invalidRows = [];
         filleredRows.forEach(row => {
          const receiveQty = parseInt(row.receiveQty) || 0;
          const rejectQty = parseInt(row.rejectQty) || 0;
          const SHIPPEDQTY = parseInt(row.SHIPPEDQTY) || 0;
          const reason = row.reason;
          const LOT_NO = row.LOT_NO;

          row = { receiveQty, rejectQty, SHIPPEDQTY, reason, LOT_NO };

            if(rejectQty > 0 && reason ==='') {
              this.isInputReason = true;
              invalidRows.push({...row, errorType: 'MISSING_REASON'});
              //return;
             }
             else if(receiveQty + rejectQty !== SHIPPEDQTY) {

              invalidRows.push({...row, errorType: 'MISMATCH_QTY'});
             }
            //return receiveQty + rejectQty !== SHIPPEDQTY;
          });
          if(invalidRows.length > 0) {
            const missingReason = invalidRows.filter(row => row.errorType === 'MISSING_REASON').map(row => row.LOT_NO).join(", ");
            if(missingReason) {
              this.$swal("", `Reason is not null, Lot no: ${missingReason}`, "warning");
                return;
            }
            const misMatchQty = invalidRows.filter(row => row.errorType === 'MISMATCH_QTY').map(row => row.LOT_NO).join(", ");
            if(misMatchQty) {
              this.$swal("", `ReceiveQty + RejectQty must equal ShippedQty, Lot no: ${misMatchQty}`, "warning");
              return;
            }
          }
          let payload = {
            EMP_NO: localStorage.username,
            database_name: localStorage.databaseName,
            rowsInnerBox: filleredRows,
            data: [
              {
                SITE: this.model.SITE,
                PIP_TYPE: this.model.PIP_TYPE,
                PO_TYPE: this.model.PO_TYPE,
                CREAT_TIME: this.model.CREAT_TIME,
                MSG_ID: this.model.MSG_ID,
                MSG_SENDER_NAME: this.model.MSG_SENDER_NAME,
                MSG_SENDER_DUNS: this.model.MSG_SENDER_DUNS,
                MSG_RECEIVER_NAME: this.model.MSG_RECEIVER_NAME,
                MSG_RECEIVER_DUNS: this.model.MSG_RECEIVER_DUNS,
                PACKSLIP_NO: this.model.PACKSLIP_NO,
                SHIP_MCMN_AIRWAYBILL: this.model.SHIP_MCMN_AIRWAYBILL,
                SHIP_MCMN_FREIGHT_CARRIER_CODE: this.model.SHIP_MCMN_FREIGHT_CARRIER_CODE,
                IMPORT_PERMIT_NO: this.model.IMPORT_PERMIT_NO,
                LOCATION_CODE: this.model.LOCATION_CODE,
                F_NAME: this.model.F_NAME,
                RECEIVER_DUNS: this.model.RECEIVER_DUNS,
                RECEIVER_DUNS4: this.model.RECEIVER_DUNS4,
                DATE_TIME: this.model.DATE_TIME,
                SHIP_MCMN_LOCATIONNAME: this.model.SHIP_MCMN_LOCATIONNAME,
                SHIP_MCMN_CITY: this.model.SHIP_MCMN_CITY,
                SHIP_MCMN_COUNTRYCODE: this.model.SHIP_MCMN_COUNTRYCODE,
                SHIP_MCMN_POSTALCODE: this.model.SHIP_MCMN_POSTALCODE,
                SHIP_MCMN_ADDR1: this.model.SHIP_MCMN_ADDR1,
                SHIP_MCMN_ADDR2: this.model.SHIP_MCMN_ADDR2,
                SHIP_MCMN_ADDR3: this.model.SHIP_MCMN_ADDR3,
                PO_NO: this.model.PO_NO,
                PO_LINE: this.model.PO_LINE,
                ITEMSHIP_MCMN_UNITOFMEASURE: this.model.ITEMSHIP_MCMN_UNITOFMEASURE,
                ITEMSHIP_MCMN_NO: this.model.ITEMSHIP_MCMN_NO,
                ITEMSHIP_MCMN_RECEIVEDQTY: this.model.ITEMSHIP_MCMN_RECEIVEDQTY,
                ITEMSHIP_MCMN_ACCEPTEDQTY: this.model.ITEMSHIP_MCMN_ACCEPTEDQTY,
                INNERBOX_SHIP_MCMN_LPN: this.model.INNERBOX_SHIP_MCMN_LPN,
                INNERBOX_SHIP_MCMN_RECEIVEDQTY: this.model.INNERBOX_SHIP_MCMN_RECEIVEDQTY,
                LAST_EDIT_TIME: this.model.LAST_EDIT_TIME
              }
            ]
          };
          try {
            let { data } = await Repository.getRepo("InsertQReceipt", payload);
            if (data.result == "ok") {
              this.ClearForm();
              this.$swal("", "Successfully applied", "success");
            } else {
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
    limitInputLength(event, maxLength){
    if(event.target.value.length >= maxLength){
      event.target.value = event.target.value.slice(0, maxLength);
    }
  },
    initializeRowsInnerBox() {
    this.rowsInnerBox = this.ShowDataTableOuterLpn.map(item => ({
      LOT_NO: item.LOT_NO,
      SHIPPEDQTY: item.SHIPPEDQTY,
      //F_ID: item.F_ID,
      receiveQty: '',
      rejectQty: '',
      reason: ''
    }));
  },
    exportexcelxlsx() {
      const filteredData = this.DataTable.map((element) => {
        return Object.keys(element).reduce((acc, key) => {
          if (key !== "FLAG" && key !== "F_ID" && key !== "STATUS") {
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
          if (key !== "FLAG" && key !== "F_ID" && key !== "STATUS") {
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
    pad(number) {
      return number < 10 ? `0${number}` : `${number}`;
    },
    ClearForm() {
      this.isShowForm = false;
      this.model.PACKSLIP_NO = "";
      this.isShowSubmitForm = true;
      this.model.FLAG = "";
      this.model.F_ID = "";
      this.selectOptions = [];
      this.selectReceiveQty = [];
      this.selectRejectQty = [];
      this.selectReason = [];
      this.selectedItems = [];
      this.LoadComponent();
    },
    ReturnForm() {
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.selectOptions = [];
      this.selectReceiveQty = [];
      this.selectRejectQty = [];
      this.selectReason = [];
      this.selectedItems = [];
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
  padding: 0 10px;
  background-color: #f9f9f9;
}
/* Back Button Styles */
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
.div-config-name {
  margin-left: 20px;
  line-height: 50px;
  span {
    font-weight: 555;
    font-size: 17px;
  }
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
  padding: 5px 15px;
  font-size: 20px;
  text-align: center;
  cursor: pointer;
  outline: none;
  color: #fff;
  background-color: #f5944f;
  border: none;
  border-radius: 5px;
  box-shadow: 0 3px #999;
}
.btn-button:hover {
  background-color: #f37318;
}
.btn-button:active {
  background-color: #f37318;
  box-shadow: 0 5px #666;
  transform: translateY(4px);
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
  grid-template-rows: 50px repeat(2, 35px) auto 1px repeat(3, 35px) auto 35px;
  grid-template-columns: repeat(3, 1fr);
  align-content: space-around;
  box-sizing: border-box;
  background-color: #e6e6e2;
  padding: 0 20px 0px 20px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  position: relative;
  font-size: 16px;
  border-radius: 5px;
  width: 80%;
  overflow: auto;
  //height: 600px;
  row-gap: 5px;
  .text-input {
    width: 55%;
    padding: 5px;
    border: 1px solid #e6e6e2;
    background-color: #e6e6e2;
    border-radius: 5px;
    box-sizing: border-box;
    margin-bottom: 10px;
    margin-left: 2px;
    resize: vertical;
  }
  .text-input-width {
    width: 60%;
    padding: 5px;
    border: 1px solid #e6e6e2;
    background-color: #e6e6e2;
    border-radius: 5px;
    box-sizing: border-box;
    margin-bottom: 10px;
    margin-left: 2px;
    resize: vertical;
  }
  label {
    font-size: 16px;
    font-weight: 555;
    color: #141414;
    margin-left: 30px;
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
  margin: 0px;
}
.submit-form {
  text-align: center;
  height: 45px;
}
.submit-form input {
  margin-right: 5px;
}
#submit-form {
  font-weight: 555;
  margin-top: 5px;
  padding: 8px 11px;
  font-size: 17px;
  text-align: center;
  cursor: pointer;
  outline: none;
  color: #fff;
  background-color: #04aa6d;
  border: none;
  border-radius: 14px;
  box-shadow: 0 5px #999;
}
#submit-form:hover {
  background-color: #3e8e41;
}
#submit-form:active {
  background-color: #3e8e41;
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
  grid-template-rows: 35px auto;
  grid-template-columns: 1fr 1fr 1fr;
  grid-column-start: 1;
  grid-column-end: 4;
  //border-radius: 5px;
  position: relative;
  align-content: space-around;
  //border: 2px solid #9f998b;
  //margin-bottom: 5px;
  //padding: 10px 0px 5px 0px;
  .form-row-actual {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
.title-actual-ship {
  position: absolute;
  top: -12px;
  left: 10%;
  transform: translateX(-50%);
  background-color: #e6e6e2;
  font-size: 16px;
  font-style: italic;
  color: #4b4949;
  font-weight: 600;
}
.form-row-actual-all {
  display: grid;
  grid-template-rows: 35px auto auto;
  grid-template-columns: repeat(6, 1fr);
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
  align-items: flex-start;;
}

.form-row-actual-all-input {
  grid-column-start: 2;
  grid-column-end: 7;
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
    //overflow: auto;
    //height: 250px;
    grid-column-start: 1;
    grid-column-end: 4;
    margin-bottom: 20px;
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
  //width: 100%;
  border-collapse: collapse;
  //margin-bottom: 20px;
  //overflow: auto;
  height: auto;
}

.table-form th, td {
  border: 1px solid #0f0f0f;
  padding: 5px;
  //text-align: center;
  color: #0f0f0f;
}

.table-form th {
  color: #0f0f0f;
  font-weight: bold;
  background-color: #bbbb9c;
  top: 0;
  position: sticky;
  z-index: 2;
  //padding: 0.5rem 1.5rem;
}
.class-re-qty {
  width: 100px;
}

.form-row-center {
  grid-column-start: 1;
  grid-column-end: 4;
  text-align: center;
  color: #423d3d;
  font-size: 16px;
}

.form-row-actual-address {
  grid-column-start: 1;
  grid-column-end: 4;
}
.form-row-actual-address select {
  width: 80%;
}

.td-edit {
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
.main-contain {
  //height: 500px;
  max-height: 100vh;
  overflow: auto;
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
      padding: 0.5rem 1.5rem;
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
      //font-weight: 555;
    }
  }
}
</style>
