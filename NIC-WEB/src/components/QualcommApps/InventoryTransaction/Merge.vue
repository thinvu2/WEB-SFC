<template>
  <div class="WOMapping-content">
    <div class="breadcrumb-content">
      <nav>
        <ul class="breadcrumb">
          <li @click="isShowForm ? ReturnForm() : BackToParent()">
            <button class="return-btn">&#8592;</button>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/Home/QualcommApps">Qualcomm</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/Home/ConfigApp/QWIP_Trans"
              >Inventory_Transactions</router-link
            >
          </li>
          <li class="breadcrumb-item active" id="breadcrumb-active">MERGE</li>
        </ul>
      </nav>
    </div>
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
    <div class="output-content" v-if="isShowForm === true">
      <div id="app">
        <div class="listbox-container">
          <div class="row col-12">
            <div class="col-3">
              <!-- ListBox 1 -->
              <div class="listbox">
                <h3>List LOT_NO</h3>
                <select v-model="selectedItem1" multiple>
                  <option v-for="item in listBox1" :key="item" :value="item">
                    {{ item }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col-1 controls">
              <!-- Control Buttons -->
              <button
                @click="moveToRight"
                :disabled="!selectedItem1.length"
                class="btn"
              >
                <Icon icon="angle-double-right" />
              </button>
              <div class="btnchange">
                <Icon icon="exchange-alt" />
              </div>
              <button
                @click="moveToLeft"
                :disabled="!selectedItem2.length"
                class="btn"
              >
                <Icon icon="angle-double-left" />
              </button>
            </div>
            <div class="col-3">
              <!-- ListBox 2 -->
              <div class="listbox">
                <h3>List CHOOSE</h3>
                <select v-model="selectedItem2" multiple>
                  <option v-for="item in listBox2" :key="item" :value="item">
                    {{ item }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col-1 controls1">
              <!-- Control Buttons -->

              <div
                class="btn"
                @click="moveToMerge"
                :disabled="selectedItem2.length > 1"
              >
                &#8658;
              </div>
            </div>
            <div class="col-3">
              <!-- ListBox 2 -->
              <div class="listbox" :hidden="disMerge">
                <h3>List MERGE</h3>
                <div v-for="item in listBox3" :key="item" class="radio-item">
                  <label>
                    <input
                      type="radio"
                      :value="item"
                      v-model="selectedProduct"
                    />
                    {{ item }}
                  </label>
                </div>
                <!-- <select v-model="selectedItem3" multiple>
                  <option v-for="item in listBox3" :key="item" :value="item">
                    {{ item }}
                  </option>
                </select> -->
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="submit-form">
        <input
          type="button"
          id="submit-form"
          value="Submit"
          @click="SubmitForm()"
        />
      </div>
    </div>
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
                  <td @click="key === 'PO_NO' && ShowDetail(index)">
                    {{ value }}
                  </td>
                </template>
              </tr>
            </tbody>
          </table>
        </template>
      </div>
    </div>

    <div class="main-contain1" v-if="isShowForm === true">
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
                  <td @click="key === 'PO_NO' && ShowDetail(index)">
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
import Repository from "../../../services/Repository";
export default {
  name: "InventoryMerge",
  created() {
    this.loadComponents();
  },
  data() {
    return {
      listResult: [],
      listHeader: [],
      listCreateLot: [],
      listCreateLotOwner: [],
      labelResult: "",
      isShowCreatedLot: false,
      createValue: "",
      reasonValue: "",
      searchKey: "",
      Qty: 0,
      action: "Merge",
      transaction: "InventoryMergeTransaction",
      filteredList: [],
      showDropdown: false,
      listBox1: ["Item 1", "Item 2", "Item 3", "Item 4", "Item 5"],
      listBox2: [],
      listBox3: [],
      selectedItem3: [],
      selectedItem1: [],
      selectedItem2: [],
      disMerge: true,
      selectedProduct: "",
      DataTableHeader: [],
      DataTable: [],
      DataTableHeader1: [],
      DataTable1: [],
      isShowForm: false,
    };
  },
  mounted() {
    document.title = "WIP Transaction Report, copyright: Hip";
    this.RefreshState();
  },
  methods: {
    moveToRight() {
      // Chuyển các mục được chọn từ listBox1 sang listBox2
      this.listBox2.push(...this.selectedItem1);
      this.listBox1 = this.listBox1.filter(
        (item) => !this.selectedItem1.includes(item)
      );
      this.selectedItem1 = "";

      this.selectedProduct = [];
      this.disMerge = true;
    },
    moveToLeft() {
      // Chuyển các mục được chọn từ listBox2 sang listBox1
      this.listBox1.push(...this.selectedItem2);
      this.listBox2 = this.listBox2.filter(
        (item) => !this.selectedItem2.includes(item)
      );
      this.selectedItem2 = [];

      this.selectedProduct = "";
      this.disMerge = true;
    },
    moveToMerge() {
      if (this.listBox2.length > 1) {
        this.listBox3 = this.listBox2;
        this.disMerge = false;
      } else {
        this.$swal("", "Choose two or more options", "warning");
      }
    },
    //inputselect

    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "QWIP_MERGE",
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        if (localStorage.language == "En") {
          this.$swal("", "Not privilege", "error");
        } else {
          this.$swal("", "Bạn không có quyền thêm sửa", "error");
        }
        this.$router.push({ path: "/Home/ConfigApp/QWIP_Trans" });
      }
    },

    async loadComponents() {
      this.CheckPrivilege();
      //this.RefreshState();

      let databaseName = localStorage.databaseName;
      let lot_no = this.valueSearch;
      let payload = {
        database_name: databaseName,
        IN_FUNC: this.transaction,
        IN_SUB_FUNC: "ShowListItem",
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
    setHeader() {
      const headers = document.querySelectorAll("th");
      headers.forEach((header) => {
        const width = header.getBoundingClientRect().width;
        header.style.width = `${width}px`;
      });
    },

    async SubmitForm() {
      if (this.selectedProduct.length == 0) {
        this.$swal("", "Your choose once option ListMerge", "error");
      } else {
        const jsonObject = {};
        this.listBox2.forEach((item, index) => {
          jsonObject[`LOT${index + 1}`] = item;
        });
        let IN_data = JSON.stringify(jsonObject);

        let payload = {
          database_name: localStorage.databaseName,
          IN_FUNC: this.transaction,
          IN_SUB_FUNC: "InsertData",
          IN_EMP: localStorage.username,
          IN_LOTNO: this.selectedProduct,
          IN_ACTION_QTY: this.listBox2.length,
          IN_DATA: IN_data,
        };

        var { data } = await Repository.getRepo("GetDataMerge", payload);

        this.DataTable1 = [];
        if (data.result == "ok") {
          this.DataTable1 = data.data;
          this.DataTableHeader1 = Object.keys(this.DataTable1[0]);

          this.$swal({
            title: "Success!",
            text: "Operation completed successfully.",
            icon: "success", // Biểu tượng thành công
            confirmButtonText: "OK", // Nút xác nhận
          });
        } else {
          this.$swal("", data.result, "error");
        }
      }
    },

    async btnSearchClick(event) {
      var database_name = localStorage.databaseName;

      var { data } = await Repository.getApiServer(
        `GetLoadFormQWip?database_name=${database_name}&trans_name=${this.transaction}&lot_no=${event}`
      );
      if (data.result == "ok") {
        this.listResult = data.data;
        this.listHeader = Object.keys(this.listResult[0]);
        this.setHeader();
      } else {
        this.$swal("", "LOT_NO not exists!", "error");
      }
    },
    async ShowDetail(index) {
      let databaseName = localStorage.databaseName;
      let lot_no = this.DataTable[index].ITEM_NO;
      let payload = {
        database_name: databaseName,
        IN_FUNC: this.transaction,
        IN_SUB_FUNC: "ShowDataByItem",
        IN_EMP: localStorage.username,
        IN_LOTNO: "",
        IN_ACTION_QTY: "",
        IN_DATA: lot_no,
      };
      try {
        this.DataTable1 = [];
        let { data } = await Repository.getRepo("GetDataMerge", payload);
        this.ShowDataDetail = data.data;
        if (this.ShowDataDetail.length > 0) {
          this.listBox1 = [];
          this.ShowDataDetail.forEach((element) => {
            this.listBox1.push(element.LOT_NO);
          });
          this.DataTable1 = data.data;
          this.DataTableHeader1 = Object.keys(this.DataTable1[0]);
          this.isShowForm = true;
        } else {
          this.$swal("", data.result, "error");
          this.isShowForm = false;
        }
      } catch (error) {
        console.error("ShowForm Error:", error);
      }
    },
    RefreshState() {
      this.isShowCreatedLot = false;
      this.createValue = "";
      this.WOValue = "";
      this.labelResult = "";
    },
    ReturnForm() {
      this.DataTable = [];
      this.DataTableHeader = [];
      this.isShowForm = false;
      this.isShowSubmitForm = true;
      this.listBox1 = [];
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
    margin-top: 5px;
    color: #adc3c9;
  }

  #breadcrumb-active {
    color: #054a5c;
    font-weight: 400;
    cursor: pointer; /* Đổi con trỏ thành bàn tay */
  }
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
.WOMapping-content {
  width: 100%;
  position: relative;
  background-color: #f2f5fc;
  border-radius: 20px;
  border: solid 1px #e9ecef;
  height: 90vh;
  margin-top: 55px;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  color: #054a5c;
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
    }
  }

  .pagetitle {
    padding-left: 15px;
    margin-top: -20px;
    p {
      font-size: 16px;
      color: #adc3c9;
      span {
        font-size: 22px;
        font-weight: 600;
        color: #054a5c;
      }
    }
  }

  .output-content {
    z-index: 1;
    background-color: white;
    position: absolute;
    top: 45%;
    left: 50%;
    transform: translate(-50%, -50%);
    height: 63%;
    width: 98%;
    border-radius: 15px;
    box-shadow: 10px 10px 20px rgba(19, 84, 95, 0.233);

    overflow-x: auto;
    overflow-y: auto;
    // overflow: auto;
    .div-table {
      overflow-x: auto;
      overflow-y: auto;
      //  border-radius: 15px;
      // top: 62%;
      // left: 50%;
      top: 0;
      height: 90%;
      width: 100%;
    }
    .mytable {
      //overflow: auto;
      border-collapse: collapse;
      th {
        background-color: #024873;
        color: white;
        border: 0.5px solid #adc3c9;
        white-space: nowrap;
        padding-left: 15px;
        padding-right: 15px;
        font-size: 20px;
        font-weight: 500;
        height: 40px;

        position: sticky;
        top: 0;
        z-index: 2;
      }

      tr {
        cursor: pointer;

        &:hover {
          background-color: #17d092;
          color: black;
        }

        td {
          border: 0.5px solid #adc3c9;
          padding-left: 5px;
          padding-right: 5px;
          font-size: 17px;
          color: #333;
          font-weight: 500;
        }
      }
    }
    .card-title {
      font-size: 22px;
      margin-top: 10px;
      span {
        color: #4f5252;
        font-size: 16px;
      }
    }

    .div-CreateLOT {
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: #c6efce;
      width: 100%;
      height: 10%;
      left: 10px;
      margin-bottom: 20px;
      p {
        margin: 0;
        font-size: 18px;
        color: #00b050;
        span {
          font-weight: bold;
          font-size: 22px;
        }
      }
    }
    body {
      font-family: Arial, sans-serif;
      background-color: #f4f4f9;
      padding: 20px;
    }

    .listbox-container {
      display: flex;
      align-items: center;
      gap: 20px;
    }

    .listbox {
      display: flex;
      flex-direction: column;
      align-items: center;
    }

    .listbox h3 {
      margin-bottom: 10px;
      color: #333;
    }

    select {
      width: 300px;
      height: 300px;
      border: 1px solid #ccc;
      border-radius: 5px;
      padding: 5px;
      background-color: #fff;
      color: #333;
      font-size: 18px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .controls {
      display: flex;
      flex-direction: column;
      gap: 10px;
      margin-top: 8%;
    }
    .controls1 {
      display: flex;
      flex-direction: column;
      gap: 10px;
      margin-top: 12%;
    }
    .btnchange {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 50px;
      height: 50px;
      border: none;
      background-color: gray;
      color: #fff;
      font-size: 18px;
      border-radius: 50%;
    }
    .btn {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 50px;
      height: 50px;
      border: none;
      background-color: #4594fc;
      color: #fff;
      font-size: 18px;
      border-radius: 50%;
      cursor: pointer;
      transition: all 0.3s ease;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }

    .btn:disabled {
      background-color: #ccc;
      cursor: not-allowed;
    }

    .btn:hover:not(:disabled) {
      background-color: rgb(255, 125, 86);

      transform: scale(1.1);
    }

    .btn i {
      pointer-events: none; /* Prevent pointer events on icon */
    }
    .submit-form {
      margin-top: 3%;
      text-align: center;
      height: 45px;
    }
    .submit-form input {
      margin-right: 5%;
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
  .main-contain {
    max-height: 100vh;
    overflow: auto;
  }

  .main-contain1 {
    margin-top: 35%;
    max-height: 50vh;
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
}
</style>