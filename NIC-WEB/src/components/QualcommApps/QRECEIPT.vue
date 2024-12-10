<template>
    <div class="div-all">
      <header class="row header">
        <div class="div-back" @click="isShowForm ? ReturnForm() : BackToParent()">
          <Icon icon="chevron-left" class="back-icon sidenav-icon" />
        </div>
        <div class="row div-config-name">
          <span>QRECEIPT</span>
        </div>
      </header>
      <div class="searchbox" v-if="!isShowForm">
      <div class="searchbox-content">
        <input
          v-on:keyup.enter="QuerySearch()"
          v-model="valueSearch"
          type="text"
          ref="input"
          class="form-control"
          placeholder="Enter Pack Slip No... "/>
        <button @click="QuerySearch()" class="btn-button">
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
      <!-- datepicker -->
      <div class="searchbox-timerange">
        <div class="datepicker-wrapper">
          <datepicker
            class="form-control form-control-sm"
            v-model="dateFrom"
            :upperLimit="dateTo"
          />
        </div>
        <div class="datepicker-wrapper">
          <datepicker
            class="form-control form-control-sm"
            v-model="dateTo"
            :lowerLimit="dateFrom"
          />
        </div>
      </div>
    </div>
      <!-- <div class="div-searchbox row" v-if="isShowForm === false">
        <div class="div-searchbox-content">
          <input
            v-on:keyup.enter="QuerySearch()"
            v-model="valueSearch"
            type="text"
            ref="input"
            class="form-control"
            @click="selectAll"
            :placeholder="
              $store.state.language == 'En'
                ? 'Enter PACKSLIP_NO...'
                : 'Nhập PACKSLIP_NO...'"
          />
          <button @click="QuerySearch()" class="btn">
            <Icon icon="search" class="sidenav-icon" />
          </button>
        </div>
      </div> -->
      <!-- Form input data -->
      <div class="container" v-if="isShowForm === true">
            <div class="title-class">
                <p>Shipment Receipt Notice</p>
            </div>
            <div class="form-row">
                <label for="msg_sender_name">To:</label>
                <input type="text"
                    class="text-input"
                    id="msg_sender_name"
                    name="msg_sender_name"
                    readonly
                    v-model="model.MSG_RECEIVER_NAME"
                />
            </div>
            <div class="form-row">
                <label for="ship_mcmn_creattime">From:</label>
                <input type="text"
                    class="text-input"
                    id="ship_mcmn_creattime"
                    name="ship_mcmn_creattime"
                    readonly
                    v-model="model.MSG_SENDER_NAME"
                />
            </div>
             <div class="form-row">
                <label for="creat-time">Time:</label>
                <input type="text"
                    class="text-input"
                    id="creat-time"
                    name="creat-time"
                    readonly
                    v-model="model.CREAT_TIME"
                />
            </div>
            <div class="form-row">
                <label for="po_no">Pack Slip No:</label>
                <input type="text"
                    class="text-input"
                    id="po_no"
                    name="po_no"
                    readonly
                    v-model="model.PACKSLIP_NO"
                />
            </div>
            <div class="form-row">
                <label for="poline_no">Airway Bill:</label>
                <input type="text"
                    class="text-input"
                    id="poline_no"
                    name="poline_no"
                    readonly
                    v-model="model.AIRWAYBILL"
                />
            </div>
            <!-- <div class="form-row">
                <label for="item_no">Invoice:</label>
                <input type="text"
                    class="text-input"
                    id="item_no"
                    name="item_no"
                    readonly
                    v-model="model.PO_NO"
                />
            </div> -->
            <div class="actual-ship-from">
                <span class="title-actual-ship">Actual ship from</span>
                <div class="form-row-actual">
                <label for="item_shippedqty">Country Code:</label>
                <input type="text"
                    class="text-input"
                    id="item_shippedqty"
                    name="item_shippedqty"
                    readonly
                    v-model="model.COUNTRY_CODE"
                />
                </div>
                <div class="form-row-actual">
                    <label for="item_unitofmeasure">City:</label>
                    <input type="text"
                        class="text-input"
                        id="item_unitofmeasure"
                        name="item_unitofmeasure"
                        readonly
                        v-model="model.SHIP_MCMN_CITY"
                    />
                </div>
                <div class="form-row-actual">
                <label for="item_unitofmeasure">Postal Code:</label>
                <input type="text"
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
                        <!-- <input type="text"
                        id="packslip_no"
                        name="packslip_no"
                        readonly
                        v-model="model.SHIP_MCMN_STREET1"
                    /> -->
                    <p>{{ model.SHIP_MCMN_STREET1 }}</p>
                    </div>
                    <div class="form-row-actual-all-input">
                        <!-- <input type="text"
                        id="packslip_no"
                        name="packslip_no"
                        readonly
                        v-model="model.SHIP_MCMN_STREET2"
                    /> -->
                    <p>{{ model.SHIP_MCMN_STREET2 }}</p>
                    </div>
                    <div class="form-row-actual-all-input">
                        <!-- <input type="text"
                        id="packslip_no"
                        name="packslip_no"
                        readonly
                        v-model="model.SHIP_MCMN_STREET3"
                        /> -->
                        <p>{{ model.SHIP_MCMN_STREET3 }}</p>
                    </div>
                </div>
            </div>

            <div class="form-row">
                <label for="item_unitofmeasure">Pallet No:</label>
                <input type="text"
                    class="text-input"
                    id="item_unitofmeasure"
                    name="item_unitofmeasure"
                    readonly
                    v-model="model.SHIP_MCMN_LPN"
                />
            </div>
            <div class="form-row">
                <label for="item_mpn">GrossWeight:</label>
                <input type="text"
                    class="text-input"
                    id="item_mpn"
                    name="item_mpn"
                    readonly
                    v-model="model.PAL_GROSSWEIGHT"
                />
            </div>
            <div class="form-row">
                <label for="item_description">NetWeight:</label>
                <input type="text"
                    class="text-input"
                    id="item_description"
                    name="item_description"
                    readonly
                    v-model="model.PAL_NETWEIGHT"
                />
            </div>
            <div class="form-row">
                <label for="lot_no">Width:</label>
                <input type="text"
                    id="lot_no"
                    class="text-input"
                    name="lot_no"
                    readonly
                    v-model="model.PAL_WIDTH"
                />
            </div>
            <div class="form-row">
                <label for="receiver_locationname">Length:</label>
                <input type="text"
                    class="text-input"
                    id="receiver_locationname"
                    name="receiver_locationname"
                    readonly
                    v-model="model.PAL_LENGTH"
                />
            </div>
            <div class="form-row">
                <label for="receiver_name">Height:</label>
                <input type="text"
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
                <input type="text"
                    id="lot_no"
                    class="text-input"
                    name="lot_no"
                    readonly
                    v-model="model.ITEM_NO"
                />
            </div>
            <div class="form-row">
                <label for="receiver_locationname">QTY:</label>
                <input type="text"
                    class="text-input"
                    id="receiver_locationname"
                    name="receiver_locationname"
                    readonly
                    v-model="model.ITEM_SHIPPEDQTY"
                />
            </div>
            <div class="form-row">
                <label for="receiver_name">MPN:</label>
                <input type="text"
                    class="text-input"
                    id="receiver_name"
                    name="receiver_name"
                    readonly
                    v-model="model.ITEM_MPN"
                />
            </div>
            <!-- end -->
            <div class="form-row-table">
                <table class="table-form">
                    <template v-for="(item, index) in ShowDataTableOuterLPN" :key="index">
                        <thead>
                            <tr>
                                <th></th>
                                <th>LPN</th>
                                <th>Gross Weight({{ item.OUTERBOX_WEIGHTUNITOFMEASURE }})</th>
                                <th>Net Weight({{ item.INNERBOX_WEIGHTUNITOFMEASURE }})</th>
                                <th>Quantity</th>
                                <th>Receive</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>{{ item.OUTERBOX_LPN }}</td>
                                <td>LPN</td>
                                <td>{{ item.OUTERBOX_GROSSWEIGHT }}</td>
                                <td>{{ item.OUTERBOX_NETWEIGHT }}</td>
                                <td>{{ item.OUTERBOX_QTY }}</td>
                                <td></td>
                                <td></td>
                            </tr>
                            <!-- Inner box -->
                            <tr v-for="(itemInnerLPN, indexInnerLPN) in ShowDataTableInnerLPN" :key="indexInnerLPN">
                                <template v-for="(itemInnerLPN1, indexInnerLPN1) in itemInnerLPN" :key="indexInnerLPN1">
                                    <td v-if="indexInnerLPN1 == 'INNERBOX_LPN' || indexInnerLPN1 == 'LPN' || indexInnerLPN1 == 'INNERBOX_GROSSWEIGHT' || indexInnerLPN1 == 'INNERBOX_NETWEIGHT' || indexInnerLPN1 == 'INNERBOX_QTY'">
                                        {{ itemInnerLPN1 }}
                                    </td>
                                </template>
                                <td>
                                    <input
                                    type="checkbox"
                                    :value="itemInnerLPN" 
                                    v-model="selectedItems"
                                    />
                                </td>
                                <td>
                                    <select>
                                        <option>Accept</option>
                                        <option>Reject</option>
                                    </select>
                                </td>
                            </tr>
                        </tbody>
                    </template>
                </table>
            </div>
            <div class="form-row-actual-address">
                <label for="box_received">Receive address:</label>
                <select>
                    <option v-for="(item, index) in ShowDataReceiveAddress" :key="index">
                        {{ item.SHIP_ADDRESS }}
                    </option>
                </select>
            </div>
            <div class="form-row-center">
                <p>Country: Viet Nam, City: Bac Giang, Postal Code: 230000</p>
            </div>
        </div>
        <div class="submit-form" v-if="isShowForm === true && isShowSubmitForm === true" >
            <input type="button" 
                id="submit-form" 
                value="Submit"
                @click="SubmitForm()"
            />
        </div>
        <div class="submit-form" v-if="isShowForm === true && isShowSubmitForm === false">
        </div>

      <div class="main-contain" v-if="isShowForm === false">
        <div class="row col-sm-12 div-content">
          <template v-if="DataTableHeader">
            <table id="tableMain" class="table mytable">
              <thead v-if="DataTable.length > 0">
                <tr>
                  <th style="width: 1px">
                    {{ $store.state.language == "En" ? "Edit" : "Sửa" }}
                  </th>
                  <template v-for="(item, index) in DataTableHeader" :key="index">
                    <th v-if="item != 'ID'">
                      {{ item }}
                    </th>
                  </template>
                </tr>
              </thead>
              <!-- <tbody>
                <template v-for="(item, index) in DataTable" :key="index">
                <tr>
                    <td 
                    class="td-edit" 
                    :style="{ backgroundColor: FLAG != 1 ? 'blue' : 'inherit' }" 
                    @click="ShowDetail(index)">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                        width="24"
                        height="24"
                    >
                        <path fill="none" d="M0 0h24v24H0z" />
                        <path
                        fill="#FFF"
                        d="M16.757 3l-2 2H5v14h14V9.243l2-2V20a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1h12.757zm3.728-.9L21.9 3.516l-9.192 9.192-1.412.003-.002-1.417L20.485 2.1z"
                        />
                    </svg>
                    </td>
                    <template v-for="(item1, index1) in item" :key="index1">
                    <td v-if="index1 != 'ID'">{{ item1 }}</td>
                    </template>
                </tr>
                </template>
            </tbody> -->
                <tbody>
                    <template v-for="(item, index) in DataTable" :key="index">
                        <tr>
                            <td class="td-edit" @click="ShowDetail(index)">
                                <svg
                                xmlns="http://www.w3.org/2000/svg"
                                viewBox="0 0 24 24"
                                width="24"
                                height="24"
                                >
                                <path fill="none" d="M0 0h24v24H0z" />
                                <path
                                    fill="#FFF"
                                    d="M16.757 3l-2 2H5v14h14V9.243l2-2V20a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1h12.757zm3.728-.9L21.9 3.516l-9.192 9.192-1.412.003-.002-1.417L20.485 2.1z"
                                />
                                </svg>
                            </td>
                            <template v-for="(item1, index1) in item" :key="index1">
                                <td v-if="index1 != 'ID'">{{ item1 }}</td>
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
  import Repository from "../../services/Repository";
  export default {
    data() {
      return {
        showError: false,
        dateFrom: new Date(),
        dateTo: new Date(),
        isShowSubmitForm: true,
        selectedItems: [],
        isShowForm: false,
        isVisible: false,
        DataTableHeader: [],
        DataTable: [],
        ShowDataDetail:[],
        ShowDataTableOuterLPN: [],
        ShowDataTableInnerLPN: [],
        ShowDataReceiveAddress: [],
        columnName: [],
        valueSearch: "",
        model: {
            database_name: localStorage.databaseName,
            EMP_NO: localStorage.username,
            PACKSLIP_NO: "",
            FLAG_RECEIPT: "",
            F_ID_RECEIPT: "",
            SITE: "",
            PIP_TYPE: "", 
            PO_TYPE: "", 
            CREAT_TIME: "", 
            MSG_ID: "", 
            MSG_SENDER_DUNS: "", 
            MSG_RECEIVER_DUNS: "", 
            RECEIVER_DUNS: "", 
            RECEIVER_DUNS4: "", 
            SHIP_MCMN_LOCATIONNAME: "", 
            LAST_EDIT_TIME: "",
            Inner_LPN: "",
            Box_Received_Qty: "",
            Box_Accepted_Qty: "",
            MSG_SENDER_NAME: "Quancomm", 
            MSG_RECEIVER_NAME: "Foxconn", 
            PO_NO: "", 
            POLINE_NO: "", 
            ITEM_NO: "", 
            ITEM_SHIPPEDQTY: "",
            ITEM_UNITOFMEASURE: "", 
            ITEM_MPN: "", 
            ITEM_DESCRIPTION: "",
            LOT_NO: "",
            RECEIVER_LOCATIONNAME: "",
            RECEIVER_NAME: "", 
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
            SHIP_MCMN_FREIGHT_CARRIER_CODE: "",
            LOCATIONNAME: "",
            STATUS: ""
        }, 
      }
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
      async SubmitForm() {
        if( this.selectedItems.length=== 0){
            this.$swal("", "Please select at least one checkbox before submitting.", "warning")
            return;
        }else{
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
                SITE: this.model.SITE,
                PIP_TYPE: this.model.PIP_TYPE, 
                PO_TYPE: this.model.PO_TYPE, 
                CREAT_TIME: this.model.CREAT_TIME, 
                MSG_ID: this.model.MSG_ID, 
                MSG_SENDER_NAME: this.model.MSG_SENDER_NAME, 
                MSG_SENDER_DUNS: this.model.MSG_SENDER_DUNS, 
                MSG_RECEIVER_NAME: this.model.MSG_RECEIVER_NAME, 
                RECEIVER_DUNS: this.model.RECEIVER_DUNS,
                MSG_RECEIVER_DUNS: this.model.MSG_RECEIVER_DUNS, 
                RECEIVER_DUNS4: this.model.RECEIVER_DUNS4, 
                SHIP_MCMN_LOCATIONNAME: this.model.SHIP_MCMN_LOCATIONNAME, 
                SHIP_MCMN_CITY: this.model.SHIP_MCMN_CITY, 
                PO_NO: this.model.PO_NO, 
                LOT_NO: this.model.LOT_NO,  
                LAST_EDIT_TIME: this.model.LAST_EDIT_TIME,
                AIRWAYBILL: this.model.AIRWAYBILL,
                SHIP_MCMN_FREIGHT_CARRIER_CODE: this.model.SHIP_MCMN_FREIGHT_CARRIER_CODE,
                LOCATIONNAME: this.model.LOCATIONNAME,
                SHIP_MCMN_COUNTRYCODE: this.model.COUNTRY_CODE,
                SHIP_MCMN_POSTALCODE: this.model.POSTAL_CODE,
                SHIP_MCMN_ADDR1: this.model.SHIP_MCMN_STREET1,
                SHIP_MCMN_ADDR2: this.model.SHIP_MCMN_STREET2,
                SHIP_MCMN_ADDR3: this.model.SHIP_MCMN_STREET3
            };
            console.log("payload: ", payload);
            try {
                let { data } = await Repository.getRepo("InsertQReceipt", payload);
                if (data.result == "ok") {
                    this.ClearForm();
                    this.$swal("", "Successfully applied", "success")
                } else {
                    this.$swal("", data.result, "error")
                }
            }catch(error) {
                if(error.response && error.response.data) {
                this.$swal("", error.response.data.error, "error");
                }else {
                this.$swal ("", error.Message, "error")
                }
            }
        })
        }
      },
    async LoadComponent() {
        let databaseName = localStorage.databaseName;
        let PACKSLIP_NO = this.model.PACKSLIP_NO;
        let dateFrom = "";
        let dateTo = "";
        try {
            let { data } = await Repository.getApiServer(`GetDataQReciept?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}&dateFrom=${dateFrom}&dateTo=${dateTo}`);
            this.DataTable = [];
            this.DataTable = data.data;
            if (this.DataTable.length > 0) {
                this.DataTableHeader = Object.keys(this.DataTable[0]);
            }
        }catch(error) {
            if(error.response && error.response.data) {
            this.$swal("", error.response.data.error, "error");
            }else {
            this.$swal ("", error.Message, "error")
            }
        }
    },
    pad(number) {
        return number < 10 ? `0${number}` : `${number}`;
    },
    async QuerySearch() {
        const formatDate = (date) => {
            if (!date) return "";
            return `${this.pad(date.getFullYear())}${this.pad(date.getMonth() +1)}${this.pad(date.getDate())}`;
        };
        const dateFrom = formatDate(this.dateFrom);
        const dateTo = formatDate(this.dateTo);
        let databaseName =  localStorage.databaseName;
        let PACKSLIP_NO = this.valueSearch;
        try {
            let { data } = await Repository.getApiServer(`GetDataQReciept?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}&dateFrom=${dateFrom}&dateTo=${dateTo}`);
            this.DataTable = [];
            this.DataTableHeader = [];
            this.DataTable = data.data;
            if (this.DataTable.length > 0) {
                this.DataTableHeader = Object.keys(this.DataTable[0]);
            }
        }catch(error) {
            if(error.response && error.response.data) {
            this.$swal("", error.response.data.error, "error");
            }else {
            this.$swal ("", error.Message, "error")
            }
        }
    },
    async ShowDetail(index) {
        let databaseName = localStorage.databaseName;
        let PACKSLIP_NO = this.DataTable[index].PACKSLIP_NO;
        let FLAG = this.DataTable[index].STATUS;
        let F_ID = this.DataTable[index].F_ID;
        if(FLAG == "WAIT") {
            FLAG = 0;
        }else {
            FLAG = 1;
        }
        console.log("Initial FLAG: ", FLAG);
        console.log("Initial isShowSubmitForm: ", this.isShowSubmitForm);
        if (FLAG != "0") {
            console.log("Condition met: FLAG != '0'");
            this.isShowSubmitForm = false;
        }
        console.log("Final isShowSubmitForm: ", this.isShowSubmitForm);

        try {
            let responseData = await Repository.getApiServer(`GetShowDetailQReceipt?database_name=${databaseName}&PACKSLIP_NO=${PACKSLIP_NO}&FLAG=${FLAG}&F_ID=${F_ID}`);
             this.ShowDataDetail = [];
             this.ShowDataDetail = responseData.data.data;
             this.ShowDataTableOuterLPN = responseData.data.dataTableOuterLPN;
             this.ShowDataTableInnerLPN = responseData.data.dataTableInnerLPN;
             this.ShowDataReceiveAddress = responseData.data.dataReceiveAddress;
            if (this.ShowDataDetail.length > 0) {
            let firstItem = this.ShowDataDetail[0];
            this.model.PACKSLIP_NO = firstItem.PACKSLIP_NO;
            this.model.FLAG = firstItem.FLAG;
            this.model.F_ID = firstItem.F_ID;
            this.model.SITE = firstItem.SITE;
            this.model.PIP_TYPE = firstItem.PIP_TYPE;
            this.model.PO_TYPE = firstItem.PO_TYPE;
            this.model.MSG_ID = firstItem.MSG_ID;
            this.model.LAST_EDIT_TIME = firstItem.LAST_EDIT_TIME;
            this.model.MSG_SENDER_DUNS = firstItem.MSG_SENDER_DUNS;
            this.model.MSG_RECEIVER_DUNS = firstItem.MSG_RECEIVER_DUNS;
            this.model.RECEIVER_DUNS = firstItem.RECEIVER_DUNS;
            this.model.RECEIVER_DUNS4 = firstItem.RECEIVER_DUNS4;
            this.model.SHIP_MCMN_LOCATIONNAME = firstItem.SHIP_MCMN_LOCATIONNAME;
            this.model.CREAT_TIME = firstItem.CREAT_TIME;
            this.model.PO_NO = firstItem.PO_NO;
            this.model.POLINE_NO = firstItem.POLINE_NO;
            this.model.ITEM_NO = firstItem.ITEM_NO;
            this.model.ITEM_SHIPPEDQTY = firstItem.ITEM_SHIPPEDQTY;
            this.model.ITEM_UNITOFMEASURE = firstItem.ITEM_UNITOFMEASURE;
            this.model.ITEM_MPN = firstItem.ITEM_MPN;
            this.model.ITEM_DESCRIPTION = firstItem.ITEM_DESCRIPTION;
            this.model.LOT_NO = firstItem.LOT_NO;
            this.model.RECEIVER_NAME = firstItem.RECEIVER_NAME;
            //new
            this.model.AIRWAYBILL = firstItem.AIRWAYBILL;
            this.model.SHIP_MCMN_FREIGHT_CARRIER_CODE = firstItem.FREIGHT_CARRIER_CODE;
            this.model.LOCATIONNAME = firstItem.RECEIVER_LOCATIONNAME;
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
                console.log("this.FLAG: ", this.model.FLAG, "this.F_ID: ", this.model.F_ID);
            this.isShowForm = true;
        } else {
            console.log("No data available in ShowDataDetail.");
            this.isShowForm = false;
        }
        }catch(error) {
            if(error.response && error.response.data) {
            this.$swal("", error.response.data.error, "error");
            }else {
            this.$swal ("", error.Message, "error")
            }
        }
      },
      ClearForm() {
        this.isShowForm = false;
        this.model.PACKSLIP_NO = "";
        this.model.FLAG = "";
        this.model.F_ID = "";
        this.LoadComponent()
      },
      ReturnForm() {
        this.isShowForm = false;
        this.isShowSubmitForm = true;
        this.selectedItems = []
      },
      BackToParent() {
        this.$router.push({ path: "/Home/Qualcomm_Aplication" });
      }
    }
}
  </script>
  
<style lang="scss" scoped>
    .div-all {
        font-family: Arial, sans-serif;
        margin: 0;
        padding: 0 10px;
        background-color: #f9f9f9;
    }
    // .header {
    //     display: flex;
    //     align-items: center;
    //     justify-content: space-between;
    //     padding: 16px;
    //     background: linear-gradient(90deg, #007bff, #6610f2, #e83e8c, #ffc107);
    //     color: white;
    //     box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    //     border-radius: 8px;
    // }

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
    // .back-button {
    //     cursor: pointer;
    //     display: flex;
    //     align-items: center;
    //     background-color: rgba(255, 255, 255, 0.2);
    //     padding: 8px;
    //     border-radius: 50%;
    //     transition: background-color 0.3s ease;
    // }

    // .back-button:hover {
    //     background-color: rgba(255, 255, 255, 0.4);
    // }

    // .back-icon {
    //     font-size: 1.5rem;
    //     color: white;
    // }

    // .config-name span {
    //     font-size: 1.8rem;
    //     font-weight: bold;
    //     text-transform: uppercase;
    //     text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
    // }

    .searchbox-time{
        height: 40px;
    }
    .searchbox-time1 {
        margin-right: 5px;
    }
    .form-control:disabled, .form-control[readonly] {
        background-color: #e9ecef;
        opacity: 1;
        height: 35px;
        border-radius: 3px;
    }
    .searchbox {
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
    // .btn-button {
    //     border-radius: 5px;
    //     padding: 6px 20px;
    //     height: 40px;
    //     border: none;
    //     background: #f18436;
    //     color: #fff;
    //     box-sizing: 0;
    //     &:hover {
    //         background: #ec690c;
    //         transition: 0.5s;
    //     }
    // }
    .searchbox-content {
        display: flex;
        gap: 5px;
        margin-bottom: 1px;
    }
    .searchbox-timerange {
        display: flex;
        justify-content: space-between;
        gap: 16px;
    }
    .datepicker-wrapper {
        flex: 1;
    }
    .container {
        display: grid;
        grid-template-rows: 50px 40px 40px auto 40px 40px 40px auto 40px 40px;
        grid-template-columns: 1fr 1fr 1fr;
        align-content: space-around;
        box-sizing: border-box;
        background: #e6e6e2;
        padding: 0 20px 0px 20px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        position: relative;
        font-size: 16px;
        border-radius: 5px;
        width: 80%;
        overflow: auto;
        height: 500px;
        .text-input {
            width: 50%;
            padding: 5px;
            border: 1px solid #e6e6e2;
            border-radius: 5px;
            box-sizing: border-box;
            margin-bottom: 10px;
            margin-left: 2px;
            resize: vertical;
        }
        label {
            // color: #141414;
            // font-size: 16px;
            // margin-left: 10px;
            font-size: 14px;
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
    .submit-form{
       // width: 100%;
        text-align: center;
        height: 45px;
    }
    .submit-form input {
            margin-right: 5px;
        }
    // #submit-form {
    //     background-color: #0dce87;
    //     color: #fffdfd;
    //     font-weight: 555;
    //     padding: 8px 12px;
    //     border: none;
    //     border-radius: 5px;
    //     cursor: pointer;
    //     &:hover {
    //         background-color: #028d5a;
    //         transition: 0.5s;
    //     }
    // }
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
        //font-weight: 555;
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
    }
    .actual-ship-from {
        display: grid;
        grid-template-rows: repeat(4 40px);
        grid-template-columns: 1fr 1fr 1fr;
        grid-column-start: 1;
        grid-column-end: 4;
        border-radius: 5px;
        position: relative;
        align-content: space-around;
        border: 2px solid #9f998b;
        margin-bottom: 5px;
        padding: 10px 0px 5px 0px;
        .form-row-actual {
            display: flex;
            justify-content: space-between;
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
        grid-template-rows: 40px 40px 40px;
        grid-template-columns: repeat(6 1fr);
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
    .table-form {
        width: 100%;
        border-collapse: collapse;
        margin-bottom: 20px;
    }

    .table-form th, td {
            border: 1px solid #0f0f0f;
            padding: 5px;
            text-align: center;
            color: #0f0f0f;
        }

        .table-form th {
            color: #0f0f0f;
            font-weight: bold;
            background-color: #bbbb9c;
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
    height: 500px;
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