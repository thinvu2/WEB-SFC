<template>
    <div class="div-all">
      <header class="row header">
        <div class="div-back" @click="isShowForm ? ReturnForm() : BackToParent()">
          <Icon icon="chevron-left" class="back-icon sidenav-icon" />
        </div>
        <div class="row div-config-name">
          <span>TelitEDI</span>
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
            placeholder="Enter Po... "
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
          <p>Telit EDI</p>
        </div>
        <div class="form-row">
          <label for="purchase-order">Po:</label>
          <input
            type="text"
            class="text-input"
            id="purchase-order"
            name="purchase-order"
            readonly
            v-model="model.PURCHASE_ORDER"
          />
        </div>
        <div class="form-row">
          <label for="pip-type">Po Type:</label>
          <input
            type="text"
            class="text-input"
            id="pip-type"
            name="pip-type"
            readonly
            v-model="model.PIP_TYPE"
          />
        </div>
        <div class="form-row">
          <label for="item-no">Item No:</label>
          <input
            type="text"
            class="text-input"
            id="item-no"
            name="item-no"
            readonly
            v-model="model.ITEM_NO"
          />
        </div>
        <div class="form-row">
          <label for="doc-date">Time:</label>
          <input
            type="text"
            class="text-input"
            id="doc-date"
            name="doc-date"
            readonly
            v-model="model.DOC_DATE"
          />
        </div>
        <div class="form-row">
          <label for="incoterms">Incoterms:</label>
          <input
            type="text"
            class="text-input"
            id="incoterms"
            name="incoterms"
            readonly
            v-model="model.INCOTERMS"
          />
        </div>

          <div class="form-row">
            <label for="inco-2">Inco 2:</label>
            <input
              type="text"
              class="text-input"
              id="inco-2"
              name="inco-2"
              readonly
              v-model="model.INCO_2"
            />
          </div>
          <div class="form-row">
            <label for="item-po">Item Po:</label>
            <input
              type="text"
              class="text-input"
              id="item-po"
              name="item-po"
              readonly
              v-model="model.ITEM_PO"
            />
          </div>
          <div class="form-row">
            <label for="item">Item:</label>
            <input
              type="text"
              class="text-input"
              id="item"
              name="item"
              readonly
              v-model="model.ITEM"
            />
          </div>
          <div class="form-row">
          <label for="delet-ind">Delete Ind:</label>
          <input
            type="text"
            class="text-input"
            id="delet-ind"
            name="delet-ind"
            readonly
            v-model="model.DELET_IND"
          />
        </div>
        <div class="class-hr">
          <hr/>
        </div>

        <div class="form-row">
          <label for="changed-on">Change Time:</label>
          <input
            type="text"
            class="text-input"
            id="changed-on"
            name="changed-on"
            readonly
            v-model="model.CHANGE_ON"
          />
        </div>
        <div class="form-row">
          <label for="quantity">Quantity:</label>
          <input
            type="text"
            class="text-input"
            id="quantity"
            name="quantity"
            readonly
            v-model="model.QUANTITY"
          />
        </div>
        <div class="form-row">
          <label for="net-price">Net Price:</label>
          <input
            type="text"
            id="net-price"
            class="text-input"
            name="net-price"
            readonly
            v-model="model.NET_PRICE"
          />
        </div>
        <div class="form-row-address">
          <label for="shipping-name">Shipping Address:</label>
          <input
            type="text"
            class="text-input"
            id="shipping-name"
            name="shipping-name"
            readonly
            v-model="model.SHIPPING_NAME"
          />
        </div>
        <!-- input -->
         <div class="form-input">
            <span class="add-new-form" @click="handAddNewForm()">
                <i class="fa fa-plus" aria-hidden="true"></i>
            </span>
            <div class="form-row-input">
                <label for="sched-deliv-date">Delivery Date:</label>
                <input
                    type="date"
                    :min="minTime"
                    id="sched-deliv-date"
                    class="text-input"
                    name="sched-deliv-date"
                    v-model="minTime"
                />
            <!-- </div>
            <div class="form-row"> -->
                <label for="sched-qty">Schedule Qty:</label>
                <input
                    type="text"
                    id="sched-qty"
                    class="text-input"
                    name="sched-qty"
                    v-model="model.SCHED_QTY"
                />
            </div>
            <div class="form-row-input" v-for="(row, index) in additionalRows" :key="index">
                <label :for="`sched-deliv-date + ${index}`">Delivery Date:</label>
                <input 
                    type="date"
                    :id="`sched-deliv-date + ${index}`"
                    class="text-input"
                    :min="minTime"
                    :name="`sched-deliv-date + ${index}`"
                    v-model="row.minTime"
                />
                <label :for="`sched-qty + ${index}`">Schedule Qty:</label>
                <input 
                    type="text"
                    :id="`sched-qty + ${index}`"
                    class="text-input"
                    :name="`sched-qty + ${index}`"
                    v-model="row.scheduleQty"
                />
                <i class="fa fa-times" aria-hidden="true" @click="handleDeleteNewForm(index)"></i>
            </div>
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
                    <th>
                      {{ item }}
                    </th>
                  </template>
                </tr>
              </thead>
              <tbody>
                  <tr v-for="(row, index) in DataTable" :key="index">
                   <template v-for="(value, key) in row" :key="key" >
                      <td
                       @click="key === 'PURCHASE_ORDER' && ShowDetail(index)">
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
        minTime: this.getTodayDate(),
        additionalRows: [],
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
        valueSearch: "",
        model: {
            database_name: localStorage.databaseName,
            EMP_NO: localStorage.username,
            PURCHASE_ORDER: '',
            PIP_TYPE: '',
            ITEM_NO: '',
            DOC_DATE: '',
            INCOTERMS: '',
            INCO_2: '',
            ITEM_PO: '',
            ITEM: '',
            DELET_IND: '',
            CHANGE_ON: '',
            QUANTITY: '',
            NET_PRICE: '',
            SHIPPING_NAME: '',
            SCHED_DELIV_DATE: '',
            SCHED_QTY: ''
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
      this.getTodayDate();
    },
    methods: {
        getTodayDate() {
            const today = new Date();
            const year = today.getFullYear();
            const month = String(today.getMonth() + 1).padStart(2, '0');
            const day = String(today.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`
        },
        handAddNewForm() {
            this.additionalRows.push({
                minTime: this.minTime,
                scheduleQty: ''
            })
        },
        handleDeleteNewForm(index) {
            this.additionalRows.splice(index, 1)
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
    
                const filleredRows = this.additionalRows.map((row) => ({
                ...row
                })).filter(row => row.minTime || row.scheduleQty);
                if(filleredRows.length === 0) {
                this.$swal("", "No data to submit", "warning");
                return;
                }
                // if(this.ShowDataTableOuterLpn.length !== filleredRows.length) {
                // this.$swal("", "Please enter enough data.", "warning");
                //     return;
                // }
                //logic: receiveQty + rejectQty == SHIPPEDQTY
                // const invalidRows = filleredRows.filter(row => {
    
                // const receiveQty = parseInt(row.receiveQty) || 0;
                // const rejectQty = parseInt(row.rejectQty) || 0;
                // const SHIPPEDQTY = parseInt(row.SHIPPEDQTY) || 0;

                // return receiveQty + rejectQty !== SHIPPEDQTY;
                // });
            //     console.log(invalidRows)
            // if (invalidRows.length > 0) {
            //     this.$swal("", `ReceiveQty + RejectQty must equal ShippedQty, Lot no: ${invalidRows[0].LOT_NO}`, "warning");
            //     return;
            //     }
    
                let payload = {
                EMP_NO: localStorage.username,
                database_name: localStorage.databaseName,
                additionalRows: filleredRows,
                data: [
                    {
                    PURCHASE_ORDER: this.model.PURCHASE_ORDER,
                    PIP_TYPE: this.model.PO_TYPE,
                    ITEM_NO: this.model.ITEM_NO,
                    DOC_DATE: this.model.TIME,
                    INCOTERMS: this.model.INCOTERMS,
                    INCO_2: this.model.INCO_2,
                    ITEM_PO: this.model.ITEM_PO,
                    ITEM: this.model.ITEM,
                    DELET_IND: this.model.DELET_IND,
                    CHANGE_ON: this.model.CHANGE_TIME,
                    QUANTITY: this.model.QUANTITY,
                    NET_PRICE: this.model.NET_PRICE,
                    SHIPPING_NAME: this.model.SHIPPING_ADDRESS,
                    SCHED_QTY: this.model.SCHED_QTY,
                    SCHED_DELIV_DATE: this.model.SCHED_DELIV_DATE
                    }
                ]
                };
                console.log("payload: ", payload);
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
      async LoadComponent() {
        let databaseName = localStorage.databaseName;
        let PURCHASE_ORDER = this.valueSearch;
        let showTimeForm = this.showTimeForm;
        const dateFrom = new Date;
        const dateTo = new Date;
        try {
          let { data } = await Repository.getApiServer(
           `GetDataTelitEDI?database_name=${databaseName}&PURCHASE_ORDER=${PURCHASE_ORDER}&dateFrom=${dateFrom}&dateTo=${dateTo}&showTimeForm=${showTimeForm}`
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
        let PURCHASE_ORDER = this.valueSearch;
        let showTimeForm = this.showTimeForm;
        try {
          let { data } = await Repository.getApiServer(
            `GetDataTelitEDI?database_name=${databaseName}&PURCHASE_ORDER=${PURCHASE_ORDER}&dateFrom=${dateFrom}&dateTo=${dateTo}&showTimeForm=${showTimeForm}`
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
        let PURCHASE_ORDER = this.DataTable[index].PURCHASE_ORDER;
        try {
          let responseData = await Repository.getApiServer(
            `GetShowDetailTelitEDI?database_name=${databaseName}&PURCHASE_ORDER=${PURCHASE_ORDER}`
          );
          this.ShowDataDetail = [];
          this.ShowDataDetail = responseData.data.data;
          if (this.ShowDataDetail.length > 0) {
            let firstItem = this.ShowDataDetail[0];
            this.model.PURCHASE_ORDER = firstItem.PURCHASE_ORDER;
            this.model.PIP_TYPE = firstItem.PO_TYPE;
            this.model.ITEM_NO = firstItem.ITEM_NO;
            this.model.DOC_DATE = firstItem.TIME;
            this.model.INCOTERMS = firstItem.INCOTERMS;
            this.model.INCO_2 = firstItem.INCO_2;
            this.model.ITEM_PO = firstItem.ITEM_PO;
            this.model.ITEM = firstItem.ITEM;
            this.model.DELET_IND = firstItem.DELET_IND;
            this.model.CHANGE_ON = firstItem.CHANGE_TIME;
            this.model.QUANTITY = firstItem.QUANTITY;
            this.model.NET_PRICE = firstItem.NET_PRICE;
            this.model.SHIPPING_NAME = firstItem.SHIPPING_ADDRESS;
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
        this.model.PURCHASE_ORDER = "";
        this.isShowSubmitForm = true;
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
        this.$router.push({ path: "/Home/Telit_Apps" });
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
    grid-template-rows: 50px repeat(5, 35px) auto;
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
    height: 600px;
    row-gap: 5px;
    .text-input {
      width: 50%;
      padding: 5px;
      border: 1px solid #e6e6e2;
      //@includebackground-color: #e6e6e2;
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

  .form-input {
    grid-column: 1 / 4;
    display: grid;
    justify-items: center;
    grid-template-rows: auto;
    grid-template-columns: 1fr;
    width: 100%;
    border: 1px solid rgb(63, 62, 61);
    position: relative;
    border-radius: 5px;
  }
  .form-row-input {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    width: 100%;
  }
  .form-row-input .text-input {
    width: 25%;
  }
  .form-row-input i {
    margin-left: 10px;
    border: 1px solid black;
    border-radius: 5px;
  }
  .add-new-form {
    display: flex;
    justify-content: flex-end;
    position: absolute;
    right: 0;
    top: -12px;
    border: 1px solid green;
    border-radius: 5px;
    height: 25px;
    width: 25px;
    cursor: pointer;
    background-color: #800000;
  }
  .add-new-form i {
    display: flex;
    margin-right: 5px;
    align-items: center;
  }

  .form-row-address {
    grid-column: 1 / 4;
  }
  .form-row-address .text-input{
    width: 45%;
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
  
  .form-row {
    display: flex;
    justify-content: space-between;
    align-items: center;
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
    height: 450px;
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
  