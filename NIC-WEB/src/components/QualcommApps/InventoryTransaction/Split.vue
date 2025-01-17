<template>
  <div class="div-all">
    <header class="row header">
      <div class="breadcrumb-content">
        <nav>
          <ul class="breadcrumb">
            <li @click="isShowForm ? ReturnForm() : BackToParent()">
              <button class="return-btn">&#8592;</button>
            </li>
            <li class="breadcrumb-item">
              <router-link to="/Home/QualcommApps"
                >Qualcomm</router-link
              >
            </li>
            <li class="breadcrumb-item">
              <router-link to="/Home/ConfigApp/QWIP_Trans"
                >Inventory_Transactions</router-link
              >
            </li>
            <li class="breadcrumb-item active" id="breadcrumb-active">SPLIT</li>
          </ul>
        </nav>
      </div>
    </header>
    <div class="searchbox" v-if="!isShowForm">
      <div class="searchbox-content">
        <input
          :disabled="showTimeForm"
          v-on:keyup.enter="LoadComponent()"
          v-model="valueSearch"
          type="text"
          class="form-control"
          placeholder="Enter Po... "
        />
        <button @click="LoadComponent()" class="btn-button">
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
    </div>
    <!-- Form input data -->
    <div class="container" v-if="isShowForm === true">
      <div class="title-class">
        <p>SPLIT</p>
      </div>
      <div class="form-row">
        <label for="shipping-name">LOT NO:</label>
        <input
          type="text"
          class="text-input"
          id="shipping-name"
          name="shipping-name"
          readonly
          v-model="model.LOT_NO"
        />
      </div>
      <div class="form-row">
        <label for="shipping-name">LOT_NO QTY:</label>
        <input type="number" class="text-input" v-model="model.QTY" readonly />
      </div>
      <!-- input -->
      <div class="form-input">
        <span class="add-new-form" @click="handAddNewForm()">
          <i class="fa fa-plus" aria-hidden="true"></i>
        </span>
        <div class="form-row-input">
          <label for="sched-deliv-date">LOT_NO 1 :</label>

          <label for="sched-qty">Qty:</label>
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
          <label :for="`sched-deliv-date + ${index}`"
            >LOT_NO {{ index + 2 }} :</label
          >
          <label :for="`sched-qty + ${index}`">Qty:</label>
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
                <template v-for="(value, key) in row" :key="key">
                  <template v-if="key === 'PO_NO'">
                    <td @click="ShowDetail(index)">
                      {{ value }}
                    </td></template
                  >
                  <template v-else>
                    <td>
                      {{ value }}
                    </td></template
                  >
                </template>
              </tr>
            </tbody>
          </table>
        </template>
      </div>
    </div>

    <div style="height: 20px"><hr /></div>
    <!-- table -->
    <div class="main-contain" v-if="isShowForm === true">
      <div class="row col-sm-12 div-content">
        <template v-if="DataTableHeader1">
          <table id="tableMain" class="table mytable">
            <thead v-if="DataTable1.length > 0">
              <tr>
                <template
                  v-for="(item, index) in DataTableHeader1"
                  :key="index"
                >
                  <th>
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(row, index) in DataTable1" :key="index">
                <template v-for="(value, key) in row" :key="key">
                  <template v-if="key === 'PO_NO'">
                    <td @click="ShowDetail(index)">
                      {{ value }}
                    </td></template
                  >
                  <template v-else>
                    <td>
                      {{ value }}
                    </td></template
                  >
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
import Repository from "../../../services/Repository";
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
      DataTableHeader1: [],
      DataTable1: [],
      ShowDataDetail: [],
      ListModel: ["AAAA", "BBBBB", "CCCCC"],
      valueSearch: "",
      transaction: "InventorySplitTransaction",
      model: {
        database_name: localStorage.databaseName,
        EMP_NO: localStorage.username,
        LOT_NO: "",
        QTY: "",
        transaction: "InventorySplitTransaction",
      },
      IN_FUNC: this.transaction,
      IN_SUB_FUNC: "",
      IN_EMP: localStorage.username,
      IN_LOTNO: "",
      IN_ACTION_QTY: "",
      IN_DATA: "",
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
    //this.getTodayDate();
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
        scheduleQty: "0",
      });
    },
    handleDeleteNewForm(index) {
      this.additionalRows.splice(index, 1);
    },
    async SubmitForm() {
      if (!this.model.SCHED_QTY) {
        this.$swal("", "Please input a valid Qty!", "warning");
        return;
      }
      // let titleValue = "";
      // let textValue = "";
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

      let filleredRows = [];
      this.additionalRows.forEach((row) => {
        if (row.scheduleQty) {
          filleredRows.push({ ...row });
        }
      });
      //sum qty
      let sumScheduleQty = filleredRows.reduce(
        (sum, qty) => sum + parseInt(qty.scheduleQty),
        0
      );
      // let sumScheduleQty = 0;
      let QUANTITY = parseInt(this.model.QTY, 10);
      console.log(QUANTITY + "-" + sumScheduleQty);
      if (QUANTITY < sumScheduleQty) {
        this.$swal(
          "",
          `Total Qty input: ${sumScheduleQty} not equal Quantity: ${QUANTITY}`,
          "warning"
        );
        return;
      }

      let listJson = [];
      listJson = this.additionalRows;
      listJson.push({
        scheduleQty: this.model.SCHED_QTY,
      });
      const formattedJson = listJson.reduce((result, item, index) => {
        result[`LOT${index + 1}`] = item.scheduleQty; // Tạo key "LOT1", "LOT2", ...
        return result;
      }, {});
      let IN_data = JSON.stringify(formattedJson);

      console.log(JSON.stringify(formattedJson));

      let payload = {
        database_name: localStorage.databaseName,
        IN_FUNC: this.transaction,
        IN_SUB_FUNC: "InsertData",
        IN_EMP: localStorage.username,
        IN_LOTNO: this.model.LOT_NO,
        IN_ACTION_QTY: this.additionalRows.length,
        IN_DATA: IN_data,
      };

      var { data } = await Repository.getRepo("GetDataMerge", payload);
      this.DataTable1 = [];
      if (data.result == "ok") {
        this.DataTable1 = data.data;
        this.DataTableHeader1 = Object.keys(this.DataTable1[0]);
        this.additionalRows = [];

        this.model.QTY = this.DataTable1[0].AVAILABLEQTY;
        this.model.SCHED_QTY = "";

        this.$swal({
          title: "Success!",
          text: "Operation completed successfully.",
          icon: "success", // Biểu tượng thành công
          confirmButtonText: "OK", // Nút xác nhận
        });
      } else {
        this.$swal("", data.result, "error");
      }
    },
    async LoadComponent() {
      let databaseName = localStorage.databaseName;
      let lot_no = this.valueSearch;
      let payload = {
        database_name: databaseName,
        IN_FUNC: this.transaction,
        IN_SUB_FUNC: "ShowListData",
        IN_EMP: localStorage.username,
        IN_LOTNO: lot_no,
        IN_ACTION_QTY: "",
        IN_DATA: "",
      };
      try {
        let { data } = await Repository.getRepo("GetDataMerge", payload);

        if (data.result == "ok") {
          this.DataTable = data.data;
          if (this.DataTable.length > 0) {
            this.DataTableHeader = Object.keys(this.DataTable[0]);
          }
        } else {
          this.$swal("", data.result, "error");
        }
      } catch (error) {
        console.error("LoadForm Error:", error);
      }
    },
    async ShowDetail(index) {
      let databaseName = localStorage.databaseName;
      let F_PO = this.DataTable[index].LOT_NO;
      try {
        let payload = {
          database_name: databaseName,
          IN_FUNC: this.transaction,
          IN_SUB_FUNC: "ShowData",
          IN_EMP: localStorage.username,
          IN_LOTNO: F_PO,
          IN_ACTION_QTY: "",
          IN_DATA: "",
        };
        let { data } = await Repository.getRepo("GetDataMerge", payload);
        this.ShowDataDetail = data.data;
        console.log(this.ShowDataDetail);
        if (data.result == "ok") {
          let firstItem = this.ShowDataDetail[0];
          this.model.LOT_NO = firstItem.LOT_NO;
          this.model.QTY = firstItem.AVAILABLEQTY;

          this.DataTable1 = data.data;
          if (this.DataTable1.length > 0) {
            this.DataTableHeader1 = Object.keys(this.DataTable1[0]);
          }

          this.isShowForm = true;
        } else {
          this.$swal("", data.result, "error");
          this.isShowForm = false;
        }
      } catch (error) {
        console.error("ShowForm Error:", error);
      }
    },
    pad(number) {
      return number < 10 ? `0${number}` : `${number}`;
    },
    ClearForm() {
      this.DataTable = [];
      this.DataTableHeader = [];
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.additionalRows = [];
      this.model.SCHED_QTY = "";
      this.LoadComponent();
    },
    ReturnForm() {
      this.DataTable = [];
      this.DataTableHeader = [];
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.additionalRows = [];
      this.model.SCHED_QTY = "";
      this.LoadComponent();
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp/QWIP_Trans" });
    },
  },
};
</script>

<style lang="scss" scoped>
.breadcrumb {
  margin-top: 10px;
  font-size: 18px;
  background-color: transparent;
  font-weight: 500;
  color: #333;

  .breadcrumb-item {
    color: #adc3c9;
  }

  #breadcrumb-active {
    color: #054a5c;
    font-weight: 400;
    cursor: pointer; /* Đổi con trỏ thành bàn tay */
  }
}
.div-all {
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  color: #054a5c;
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
  margin-bottom: -17px;
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
  background-color: #800000;
  height: 20px;
  width: 20px;
  right: 0;
  margin-left: 10px;
  border: 1px solid black;
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
.breadcrumb {
  display: flex;
  align-items: center;
  font-family: Arial, sans-serif;
  font-size: 20px;
}

.return-btn {
  margin-right: 10px;
  padding: 5px 10px;
  font-size: 20px;
  color: white;
  background-color: #adc3c9; /* Màu cam */
  border: none;
  border-radius: 4px;
  cursor: pointer;
  display: flex;
  align-items: center;
}

.return-btn:hover {
  background-color: #e66920; /* Màu cam đậm hơn khi hover */
}

.return-btn:focus {
  outline: none;
}

.breadcrumb span {
  color: #333; /* Màu chữ */
}
</style>
