<template>
  <div class="div-all">
    <header class="row header">
      <div class="div-back" @click="isShowForm ? returnForm() : backToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="row div-config-name">
        <span>Telit 850 860</span>
      </div>
    </header>
    <div class="searchbox" v-if="!isShowForm">
      <div class="searchbox-content">
        <input
          :disabled="showTimeForm"
          v-on:keyup.enter="loadComponent()"
          v-model="valueSearch"
          type="text"
          class="form-control"
          placeholder="Enter Po... "
        />
        <button @click="loadComponent()" class="btn-button">
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
    </div>
    <!-- export excel -->
    <div class="export-excel" v-if="!isShowForm">
      <img
        @click="exportexcelxlsx()"
        class="img-excel"
        src="assets/img/xlsx-icon.jpg"
        alt=""
      />
    </div>
    <!-- Form input data -->
    <div class="container" v-if="isShowForm === true">
      <div class="title-class">
        <p>Telit 850 860</p>
      </div>
      <div class="form-row">
        <label for="purchase-order">Po:</label>
        <input
          type="text"
          class="text-input"
          id="purchase-order"
          name="purchase-order"
          readonly
          v-model="model.F_PO"
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
          v-model="model.F_PIP_TYPE"
        />
      </div>
      <div class="form-row">
        <label for="item-no">Item INT:</label>
        <input
          type="text"
          class="text-input"
          id="item-no"
          name="item-no"
          readonly
          v-model="model.ITEM_INT"
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
          v-model="model.F_PO_ITEM"
        />
      </div>
      <div class="form-row">
        <label for="delet-ind">Short Text:</label>
        <input
          type="text"
          class="text-input"
          id="delet-ind"
          name="delet-ind"
          readonly
          v-model="model.SHORT_TEXT"
        />
      </div>
      <div class="class-hr">
        <hr />
      </div>
      <div class="form-row">
        <label for="material">Material:</label>
        <input
          type="text"
          class="text-input"
          id="material"
          name="material"
          readonly
          v-model="model.MATERIAL"
        />
      </div>
      <div class="form-row">
        <label for="stor-loc">Stor Loc:</label>
        <input
          type="text"
          class="text-input"
          id="stor-loc"
          name="stor-loc"
          readonly
          v-model="model.STOR_LOC"
        />
      </div>

      <div class="form-row">
        <label for="changed-on">Change Time:</label>
        <input
          type="text"
          class="text-input"
          id="changed-on"
          name="changed-on"
          readonly
          v-model="model.CHANGED_ON"
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
      <div class="form-row">
        <label for="sched-deliv-date">Sched Deliv Date:</label>
        <input
          type="text"
          id="sched-deliv-date"
          class="text-input"
          name="sched-deliv-date"
          readonly
          v-model="model.SCHED_DELIV_DATE"
        />
      </div>
      <div class="form-row">
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
            :min="minTimeDefault"
            id="sched-deliv-date"
            class="text-input"
            name="sched-deliv-date"
            v-model="minTimeDefault"
          />
          <input
            type="checkbox"
            id="checkbox-time"
            name="checkbox-time"
            v-model="checkboxTimeDefault"
          />
          <label for="sched-qty">Schedule Qty:</label>
          <input
            type="number"
            id="sched-qty"
            class="text-input"
            name="sched-qty"
            autocomplete="off"
            max="999999"
            min="0"
            v-model="model.SCHED_QTY"
            @input="limitInputLength($event, 6)"
          />
        </div>
        <div
          class="form-row-input"
          v-for="(row, index) in additionalRows"
          :key="index"
        >
          <label :for="`sched-deliv-date + ${index}`">Delivery Date:</label>
          <input
            type="date"
            :id="`sched-deliv-date + ${index}`"
            class="text-input"
            :name="`sched-deliv-date + ${index}`"
            :min="minTime"
            v-model="row.minTime"
          />
          <input
            type="checkbox"
            :id="`checkbox-time + ${index}`"
            :name="`checkbox-time + ${index}`"
            v-model="row.checkboxTime"
          />
          <label :for="`sched-qty + ${index}`">Schedule Qty:</label>
          <input
            type="number"
            :id="`sched-qty + ${index}`"
            class="text-input"
            :name="`sched-qty + ${index}`"
            autocomplete="off"
            max="999999"
            min="0"
            v-model="row.scheduleQty"
            @input="limitInputLength($event, 6)"
          />
          <i
            class="fa fa-times"
            aria-hidden="true"
            @click="handleDeleteNewForm(index)"
          ></i>
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
        @click="submitForm()"
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
                <template v-for="(value, key) in row" :key="key">
                  <td @click="key === 'PO' && showDetail(index)">
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
      checkboxTimeDefault: false,
      checkboxTime: false,
      minTimeDefault: this.getTodayDate(),
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
        F_ID: "",
        F_SITE: "",
        F_PIP_TYPE: "",
        F_MSGID: "",
        F_TIMESTAMP: "",
        F_VENDOR: "",
        F_PO: "",
        F_PO_ITEM: "",
        F_CONF_CTG: "",
        F_REFERENCE: "",
        F_CREATION_DATE: "",
        F_LASTEDIT_DT: "",
        F_FILENAME: "",
        F_TIMES: "",
        ITEM_INT: "",
        DOC_DATE: "",
        INCOTERMS: "",
        INCO_2: "",
        ITEM_PO: "",
        SHORT_TEXT: "",
        MATERIAL: "",
        STOR_LOC: "",
        CHANGED_ON: "",
        QUANTITY: "",
        NET_PRICE: "",
        SCHED_DELIV_DATE: "",
        SHIPPING_NAME: "",
        SCHED_QTY: "",
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
    checkboxTimeDefault(newValue) {
      this.minTimeDefault = newValue ? "2099-01-01" : this.getTodayDate();
    },
    additionalRows: {
      handler(newRows) {
        newRows.forEach((row) => {
          if (!row.watched) {
            row.watched = true;
            this.$watch(
              () => row.checkboxTime,
              (newValue) => {
                row.minTime = newValue ? "2099-01-01" : this.getTodayDate();
              },
              { immediate: true }
            );
          }
        });
      },
      deep: true,
      immediate: true,
    },
  },

  mounted() {
    this.loadComponent();
  },
  methods: {
    limitInputLength(event, maxLength) {
      if (event.target.value.length >= maxLength) {
        event.target.value = event.target.value.slice(0, maxLength);
      }
    },
    getTodayDate() {
      const today = new Date();
      const year = today.getFullYear();
      const month = String(today.getMonth() + 1).padStart(2, "0");
      const day = String(today.getDate()).padStart(2, "0");
      return `${year}-${month}-${day}`;
    },
    handAddNewForm() {
      this.additionalRows.push({
        minTime: this.getTodayDate(),
        scheduleQty: "",
        checkboxTime: false,
      });
    },
    handleDeleteNewForm(index) {
      this.additionalRows.splice(index, 1);
    },
    async loadComponent() {
      let databaseName = localStorage.databaseName;
      let F_PO = this.valueSearch;
      try {
        let { data } = await Repository.getApiServer(
          `GetDataTelitEDI?database_name=${databaseName}&F_PO=${F_PO}`
        );
        this.DataTable = data.data;
        if (this.DataTable.length > 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
        }
      } catch (error) {
        console.error("LoadForm Error:", error);
        const message =
          error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async showDetail(index) {
      let databaseName = localStorage.databaseName;
      let F_PO = this.DataTable[index].PO;
      let F_ITEM_INT = this.DataTable[index].ITEM_INT;
      let F_PIP_TYPE = this.DataTable[index].PIP_TYPE;
      try {
        let responseData = await Repository.getApiServer(
          `GetShowDetailTelitEDI?database_name=${databaseName}&F_PO=${F_PO}&F_ITEM_INT=${F_ITEM_INT}&F_PIP_TYPE=${F_PIP_TYPE}`
        );
        this.ShowDataDetail = responseData.data.data;
        if (this.ShowDataDetail.length > 0) {
          let firstItem = this.ShowDataDetail[0];
          this.model.F_ID = firstItem.F_ID;
          this.model.F_SITE = firstItem.F_SITE;
          this.model.F_PIP_TYPE = firstItem.F_PIP_TYPE;
          this.model.F_MSGID = firstItem.F_MSGID;
          this.model.F_TIMESTAMP = firstItem.F_TIMESTAMP;
          this.model.F_VENDOR = firstItem.F_VENDOR;
          this.model.F_PO = firstItem.F_PO;
          this.model.F_PO_ITEM = firstItem.F_PO_ITEM;
          this.model.F_CONF_CTG = firstItem.F_CONF_CTG;
          this.model.F_REFERENCE = firstItem.F_REFERENCE;
          this.model.F_CREATION_DATE = firstItem.F_CREATION_DATE;
          this.model.F_LASTEDIT_DT = firstItem.F_LASTEDIT_DT;
          this.model.F_FILENAME = firstItem.F_FILENAME;
          this.model.F_TIMES = firstItem.F_TIMES;
          this.model.ITEM_INT = firstItem.ITEM_INT;
          this.model.DOC_DATE = firstItem.DOC_DATE;
          this.model.INCOTERMS = firstItem.INCOTERMS;
          this.model.INCO_2 = firstItem.INCO_2;
          this.model.ITEM_PO = firstItem.ITEM_PO;
          this.model.SHORT_TEXT = firstItem.SHORT_TEXT;
          this.model.MATERIAL = firstItem.MATERIAL;
          this.model.STOR_LOC = firstItem.STOR_LOC;
          this.model.CHANGED_ON = firstItem.CHANGED_ON;
          this.model.QUANTITY = firstItem.QUANTITY;
          this.model.NET_PRICE = firstItem.NET_PRICE;
          this.model.SCHED_DELIV_DATE = firstItem.SCHED_DELIV_DATE;
          this.model.SHIPPING_NAME = firstItem.SHIPPING_NAME;
          this.isShowForm = true;
        } else {
          this.isShowForm = false;
        }
      } catch (error) {
        console.error("ShowForm Error:", error);
        const message =
          error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async submitForm() {
      if (!this.model.SCHED_QTY) {
        this.$swal("", "Please input a valid Schedule Qty!", "warning");
        return;
      }
      if (parseInt(this.model.SCHED_QTY) < 0) {
        this.$swal("", "Input qty can't < 0", "warning");
        return;
      }
      let filleredRows = [];
      this.removeValues = this.additionalRows.map((value) => ({
        minTime: value.minTime,
        scheduleQty: value.scheduleQty,
      }));
      this.removeValues.forEach((row) => {
        if (row.minTime || row.scheduleQty) {
          filleredRows.push({ ...row });
        }
      });
      if (this.minTimeDefault || this.model.SCHED_QTY) {
        filleredRows.push({
          minTime: this.minTimeDefault,
          scheduleQty: this.model.SCHED_QTY,
        });
      } else {
        this.$swal("", "Invalid data, input is wrong", "warning");
        return;
      }
      if (filleredRows.length === 0) {
        this.$swal("", "No data to submit", "warning");
        return;
      }
      let arr2 = filleredRows.map((item) => item.minTime);
      const set1 = new Set(arr2);
      if (arr2.length !== set1.size) {
        this.$swal("", "Delivery Date can't choose the same time", "warning");
        return;
      }
      //sum qty
      let sumScheduleQty = 0;
      let Quantities = 0;
      sumScheduleQty = filleredRows.reduce((sum, qty) => {
        if (parseInt(qty.scheduleQty) < 0) {
          this.$swal("", "Input qty can't < 0", "warning");
          return;
        }
        return sum + parseInt(qty.scheduleQty);
      }, 0);
      Quantities = parseInt(this.ShowDataDetail[0].QUANTITY, 10);
      if (Number.isNaN(sumScheduleQty) || Number.isNaN(Quantities)) {
        this.$swal(
          "",
          `Invalid data, input is wrong: ${sumScheduleQty}, Quantity: ${Quantities}`,
          "warning"
        );
        return;
      }
      if (Quantities !== sumScheduleQty) {
        this.$swal(
          "",
          `Total scheduleQty: ${sumScheduleQty} not equal Quantity: ${Quantities}`,
          "warning"
        );
        return;
      }
      let titleValue = "Are you sure edit?";
      let textValue = "Once OK, data will be updated!";
      try {
        const willDelete = await this.$swal({
          title: titleValue,
          text: textValue,
          icon: "warning",
          buttons: true,
          dangerMode: true,
        });
        if (!willDelete) return;
      } catch (err) {
        console.error("SweetAlert Error:", err);
      }

      let payload = {
        EMP_NO: localStorage.username,
        database_name: localStorage.databaseName,
        additionalRows: filleredRows,
        data: [
          {
            F_ID: this.model.F_ID,
            F_SITE: this.model.F_SITE,
            F_PIP_TYPE: this.model.F_PIP_TYPE,
            F_MSGID: this.model.F_MSGID,
            F_TIMESTAMP: this.model.F_TIMESTAMP,
            F_VENDOR: this.model.F_VENDOR,
            F_PO: this.model.F_PO,
            F_PO_ITEM: this.model.F_PO_ITEM,
            F_CONF_CTG: this.model.F_CONF_CTG,
            F_REFERENCE: this.model.F_REFERENCE,
            F_CREATION_DATE: this.model.F_CREATION_DATE,
            F_LASTEDIT_DT: this.model.F_LASTEDIT_DT,
            F_FILENAME: this.model.F_FILENAME,
            F_TIMES: this.model.F_TIMES,
          },
        ],
      };
      try {
        let { data } = await Repository.getRepo("InsertTelitEDI", payload);
        if (data.result == "ok") {
          await this.clearForm();
          this.$swal("", "Successfully applied", "success");
        } else {
          this.$swal("", data.result, "error");
        }
      } catch (error) {
        console.error("submitForm Error:", error);
        const message =
          error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    pad(number) {
      return number < 10 ? `0${number}` : `${number}`;
    },
    //export excel
    exportexcelxlsx() {
      let ws = xlsx.utils.json_to_sheet(this.DataTable);
      let wb = xlsx.utils.book_new();
      xlsx.utils.book_append_sheet(wb, ws, "data");
      xlsx.writeFile(wb, "download.xlsx");
    },
    async clearForm() {
      this.DataTable = [];
      this.DataTableHeader = [];
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.additionalRows = [];
      this.model.SCHED_QTY = "";
      await this.loadComponent();
    },
    async returnForm() {
      this.DataTable = [];
      this.DataTableHeader = [];
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.additionalRows = [];
      this.model.SCHED_QTY = "";
      await this.loadComponent();
    },
    backToParent() {
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
  //width: 100%;
  //height:auto;
  //min-height: 100vh;
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
  grid-template-rows: 50px repeat(3, 35px) 5px repeat(3, 35px) auto;
  grid-template-columns: repeat(3, 1fr);
  align-content: space-around;
  box-sizing: border-box;
  background-color: #e6e6e2;
  padding: 0 20px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  position: relative;
  font-size: 16px;
  border-radius: 5px;
  width: 80%;
  overflow: auto;
  //height: auto;
  //min-height: 100vh;
  row-gap: 15px;
  .text-input {
    width: 50%;
    padding: 5px;
    //background-color: #e6e6e2;
    border: 1px solid #e6e6e2;
    border-radius: 5px;
    box-sizing: border-box;
    margin-bottom: 10px;
    margin-left: 2px;
    resize: vertical;
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
  border: 1px solid #a19c9c;
  position: relative;
  border-radius: 5px;
  margin-bottom: 10px;
}
.form-row-input {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  width: 100%;
  margin-top: 5px;
}
.form-row-input .text-input {
  width: 25%;
}
.form-row-input i {
  display: flex;
  background-color: #c82d2d;
  height: 20px;
  width: 20px;
  margin-left: 10px;
  border: 1px solid #c82d2d;
  border-radius: 6px;
  justify-content: center;
}
.add-new-form {
  display: flex;
  justify-content: flex-end;
  position: absolute;
  right: 0;
  top: -12px;
  border: 1px solid #ffffff;
  border-radius: 5px;
  height: 25px;
  width: 25px;
  cursor: pointer;
  background-color: #00ff00;
}
.add-new-form i {
  display: flex;
  margin-right: 5px;
  align-items: center;
}
input[type="checkbox"] {
  margin-left: 5px;
}

.form-row-address {
  grid-column: 1 / 4;
}
.form-row-address .text-input {
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
.form-row,
.form-input label {
  font-size: 16px;
  font-weight: 555;
  color: #141414;
  margin-left: 30px;
}
.form-row:nth-child(18) {
  grid-column: 1 / 4;
  display: flex;
  justify-content: flex-start;
}
.form-row:nth-child(18) label {
  margin-right: 10px;
}

.class-hr {
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
