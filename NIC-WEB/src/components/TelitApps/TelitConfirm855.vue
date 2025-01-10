<template>
  <div class="div-all">
    <header class="row header">
      <div class="div-back" @click="isShowForm ? returnForm() : backToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="row div-config-name">
        <span>Re-Confirm 855</span>
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
    <!-- Form input data -->
    <div class="container" v-if="isShowForm === true">
      <div class="title-class">
        <p>Re-Confirm 855</p>
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
        <label for="item-no">Item No:</label>
        <input
          type="text"
          class="text-input"
          id="item-no"
          name="item-no"
          readonly
          v-model="model.F_PO_ITEM"
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
          v-model="model.F_TIMESTAMP"
        />
      </div>
      <div class="form-row">
        <label for="vendor">Vendor:</label>
        <input
          type="text"
          class="text-input"
          id="vendor"
          name="vendor"
          readonly
          v-model="model.F_VENDOR"
        />
      </div>

      <div class="form-row">
        <label for="f_conf_ctg">Conf Ctg</label>
        <input
          type="text"
          class="text-input"
          id="f_conf_ctg"
          name="f_conf_ctg"
          readonly
          v-model="model.F_CONF_CTG"
        />
      </div>
      <div class="form-row">
        <label for="f_lastedit_dt">Last Edit Time:</label>
        <input
          type="text"
          class="text-input"
          id="f_lastedit_dt"
          name="f_lastedit_dt"
          readonly
          v-model="model.F_LASTEDIT_DT"
        />
      </div>
      <!-- input -->
      <div class="form-input">
        <span class="add-new-form" @click="handAddNewForm()">
          <i class="fa fa-plus" aria-hidden="true"></i>
        </span>
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
            :min="minTime"
            :name="`sched-deliv-date + ${index}`"
            v-model="row.minTime"
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
                  <td @click="key === 'F_PO' && showDetail(index)">
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
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      minTime: this.getTodayDate(),
      minTimeDefault: this.getTodayDate(),
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
      databaseName: localStorage.databaseName,
      empNo: localStorage.username,
      QuantitiesAfter: 0,
      model: {
        F_SITE: "",
        F_PIP_TYPE: "",
        F_MSGID: "",
        F_TIMESTAMP: "",
        F_VENDOR: "",
        F_PO: "",
        F_PO_ITEM: "",
        F_CONF_CTG: "",
        F_REFERENCE: "",
        F_LASTEDIT_DT: "",
        F_FILENAME: "",
        F_TIMES: "",
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
        minTime: this.minTime,
        scheduleQty: "",
      });
    },
    handleDeleteNewForm(index) {
      this.additionalRows.splice(index, 1);
    },
    async loadComponent() {
      let databaseName = this.databaseName;
      let F_PO = this.valueSearch;
      try {
        let { data } = await Repository.getApiServer(
          `GetDataConfirm855?databaseName=${databaseName}&F_PO=${F_PO}`
        );
        this.DataTable = data.data;
        if (this.DataTable.length > 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
        }
      } catch (error) {
        console.error("LoadForm Error:", error);
        const message =
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async showDetail(index) {
      let databaseName = this.databaseName;
      let F_PO = this.DataTable[index].F_PO;
      let F_PO_ITEM = this.DataTable[index].F_PO_ITEM;
      let F_PIP_TYPE = this.DataTable[index].F_PIP_TYPE;
      try {
        let responseData = await Repository.getApiServer(
          `GetShowDetailConfirm855?databaseName=${databaseName}&F_PO=${F_PO}&F_PO_ITEM=${F_PO_ITEM}&F_PIP_TYPE=${F_PIP_TYPE}`
        );
        this.ShowDataDetail = responseData.data.data;
        let additionalRows = responseData.data.dataInput;

        this.QuantitiesAfter = additionalRows.reduce((sum, qty) => {
          return (sum = sum + parseInt(qty.F_QTY));
        }, 0);

        this.additionalRows = additionalRows.map((item) => {
          return {
            minTime: item.F_DEL_DATE.split(" ")[0],
            scheduleQty: item.F_QTY,
          };
        });
        if (this.ShowDataDetail.length > 0) {
          let firstItem = this.ShowDataDetail[0];
          this.model.F_PIP_TYPE = firstItem.F_PIP_TYPE;
          this.model.F_SITE = firstItem.F_SITE;
          this.model.F_MSGID = firstItem.F_MSGID;
          this.model.F_REFERENCE = firstItem.F_REFERENCE;
          this.model.F_FILENAME = firstItem.F_FILENAME;
          this.model.F_TIMES = firstItem.F_TIMES;
          this.model.F_TIMESTAMP = firstItem.F_TIMESTAMP;
          this.model.F_VENDOR = firstItem.F_VENDOR;
          this.model.F_PO = firstItem.F_PO;
          this.model.F_PO_ITEM = firstItem.F_PO_ITEM;
          this.model.F_CONF_CTG = firstItem.F_CONF_CTG;
          this.model.F_LASTEDIT_DT = firstItem.F_LASTEDIT_DT;
          this.isShowForm = true;
        } else {
          this.isShowForm = false;
        }
      } catch (error) {
        console.error("ShowForm Error:", error);
        const message =
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async submitForm() {
      let filleredRows = [];
      this.additionalRows.forEach((row) => {
        if (row.minTime || row.scheduleQty) {
          filleredRows.push({ ...row });
        }
      });

      let QuantitiesBefore = 0;
      QuantitiesBefore = filleredRows.reduce((sum, qty) => {
        if (parseInt(qty.scheduleQty) < 0) {
          this.$swal("", "Input qty can't < 0", "warning");
          return;
        }
        return (sum = sum + parseInt(qty.scheduleQty));
      }, 0);

      if (this.QuantitiesAfter !== QuantitiesBefore) {
        this.$swal(
          "",
          `Input quantities: ${QuantitiesBefore} is not equal to Total quantities: ${this.QuantitiesAfter}`,
          "warning"
        );
        return;
      }

      let compareArrDate = [];
      let compareSetDate = [];
      compareArrDate = filleredRows.map((item) => item.minTime);
      compareSetDate = new Set(compareArrDate);

      if (compareArrDate.length !== compareSetDate.size) {
        this.$swal("", "Delivery Date can't choose the same time", "warning");
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
        empNo: this.empNo,
        databaseName: this.databaseName,
        additionalRows: filleredRows,
        data: [
          {
            F_SITE: this.model.F_SITE,
            F_PIP_TYPE: this.model.F_PIP_TYPE,
            F_MSGID: this.model.F_MSGID,
            F_VENDOR: this.model.F_VENDOR,
            F_PO: this.model.F_PO,
            F_PO_ITEM: this.model.F_PO_ITEM,
            F_CONF_CTG: this.model.F_CONF_CTG,
            F_REFERENCE: this.model.F_REFERENCE,
            F_FILENAME: this.model.F_FILENAME,
            F_TIMES: this.model.F_TIMES,
          },
        ],
      };
      try {
        let { data } = await Repository.getRepo("InsertConfirm855", payload);
        if (data.result == "ok") {
          await this.clearForm();
          this.$swal("", "Successfully applied", "success");
        } else {
          this.$swal("", data.result, "error");
        }
      } catch (error) {
        console.error("submitForm Error:", error);
        const message =
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    pad(number) {
      return number < 10 ? `0${number}` : `${number}`;
    },
    async clearForm() {
      this.DataTable = [];
      this.QuantitiesAfter = 0;
      this.DataTableHeader = [];
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.additionalRows = [];
      await this.loadComponent();
    },
    async returnForm() {
      this.DataTable = [];
      this.QuantitiesAfter = 0;
      this.DataTableHeader = [];
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.additionalRows = [];
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
.container {
  display: grid;
  grid-template-rows: 50px repeat(2, 35px) auto;
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
.form-row:nth-child(17) {
  grid-column: 1 / 4;
  display: flex;
  justify-content: flex-start;
}
.form-row:nth-child(17) label {
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
