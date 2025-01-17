<template>
  <div class="WOMapping-content">
    <div class="breadcrumb-content">
      <nav>
        <ul class="breadcrumb">
          <li class="breadcrumb-item">
            <router-link to="/Home/QualcommApps">Qualcomm</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="/Home/ConfigApp/QWIP_Trans"
              >WIP_Transactions</router-link
            >
          </li>
          <li class="breadcrumb-item active" id="breadcrumb-active">
            {{ action.toUpperCase().replace("LOT", "_LOT") }}
          </li>
        </ul>
      </nav>
    </div>

    <div class="input-center">
      <div class="title row col-12">
        <div class="titleReceviceLot col-3">
          <p>Choose LOT_NO:</p>
        </div>
        <div class="titleReceviceLot col-2" style="margin-left: -15px">
          <p>Qty:</p>
        </div>
        <div class="titleReceviceLot col-2" style="margin-left: -5px">
          <p>Lot_no Qty:</p>
        </div>
        <div class="titleReceviceLot col-2">
          <p>Remain Qty:</p>
        </div>
      </div>
      <div class="optionInput row col-12">
        <div style="position: relative" class="valueReceviceLot col-3">
          <input
            class="form-control"
            id="CreateLot"
            name="CreateLot"
            v-model="createValue"
            @focus="showDropdown = true"
            @input="filterOptions"
            @blur="hideDropdown"
            v-on:change="UpdateQty"
          />

          <!-- Danh sách tùy chỉnh -->
          <ul
            v-show="showDropdown && filteredList.length"
            style="
              position: absolute;
              top: 105%;
              left: 15px;
              width: 90%;
              border: 1px solid #ccc;
              background: white;
              max-height: 200px;
              overflow-y: auto;
              z-index: 1000;
              list-style: none;
              margin: 0;
              padding: 0;
            "
          >
            <li
              v-for="(item, index) in filteredList"
              :key="index"
              @mousedown="selectOption(item.RECEIVELOT)"
              style="padding: 8px; cursor: pointer"
              class="form-control"
              v-on:change="UpdateQty"
            >
              {{ item.RECEIVELOT }}
            </li>
          </ul>
        </div>

        <span class="dropdown-icon" @click="toggleDropdown">▼</span>
        <div class="valueReceviceLot col-2">
          <input
            class="form-control"
            type="number"
            v-model="Qty"
            v-on:change="CheckQty"
          />
        </div>
        <div class="valueReceviceLot col-2">
          <input
            class="form-control"
            type="number"
            v-model="Qty_lotno"
            disabled
          />
        </div>
        <div class="valueReceviceLot col-2">
          <input
            class="form-control"
            type="number"
            v-model="Qty_remain"
            disabled
          />
        </div>
        <div class="valueActionArea col-2">
          <button class="btn-MappingLot" @click="btnClick($event)">
            {{ action.replace("lot", "").toUpperCase() }}
          </button>
        </div>
      </div>
    </div>
    <div class="output-content">
      <div class="row col-12">
        <p class="card-title">
          Output Center
          <span> | Show result query data. </span>
        </p>
      </div>
      <div class="div-CreateLOT" v-if="isShowCreatedLot">
        <p>
          <span>{{ labelResult }} SUCCESSED ➪[</span> LOT No:
          <b>{{ createValue }}</b
          ><span>]</span>
        </p>
      </div>
      <div class="div-table">
        <table class="mytable" id="my-table">
          <thead>
            <tr>
              <template v-for="(item, index) in listHeader" :key="index">
                <th>{{ item }}</th>
              </template>
            </tr>
          </thead>
          <tr v-for="(item, index) in listResult" :key="index">
            <template v-for="(itemContent, index1) in item" :key="index1">
              <td>{{ itemContent }}</td>
            </template>
          </tr>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
import Repository from "../../../services/Repository";
export default {
  name: "WipConsume",
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
      Qty: 0,
      Qty_lotno: 0,
      Qty_remain: 0,
      searchKey: "",
      action: "Consume",
      transaction: "WIPConsumeTransaction",
      filteredList: [],
      showDropdown: false,
    };
  },
  mounted() {
    document.title = "WIP Transaction Report, copyright: Hip";
    this.RefreshState();
  },
  methods: {
    //inputselect
    filterOptions() {
      this.filteredList = this.listCreateLot.filter((item) =>
        item.RECEIVELOT.toLowerCase().includes(this.createValue.toLowerCase())
      );
    },
    selectOption(value) {
      this.createValue = value;
      this.showDropdown = false;
      this.LoadQty(value);
    },
    hideDropdown() {
      setTimeout(() => {
        this.showDropdown = false;
      }, 100);
    },
    toggleDropdown() {
      this.showDropdown = !this.showDropdown;
      if (this.showDropdown) this.filterOptions();
    },
    UpdateQty: function (event) {
      this.createValue = event.target.value;
      this.LoadQty(event.target.value);
      // this.btnSearchClick(event.target.value);
    },
    CheckQty: function (event) {
      this.Qty = event.target.value;
    },
    async CheckQty_Qwip(qty) {
      var payload = {
        database_name: localStorage.databaseName,
        lot_no: this.createValue,
        reason: "",
        qty: qty,
        transaction: this.transaction,
      };

      var { data } = await Repository.getRepo("CheckQty_Qwip", payload);

      if (data.result != "ok") {
        this.Qty = 0;
        this.$swal("", data.result, "error");
      }
    },
    async LoadQty(value) {
      var payload = {
        database_name: localStorage.databaseName,
        qty: "20",
        lot_no: value,
        in_move_type: "CONSUME_GET_QTY",
        transaction: "WIPConsumeTransaction",
      };
      this.listStage = [];
      var { data } = await Repository.getRepo("InsertQWipTrans", payload);
      console.log(data.data);
      if (data.result == "ok") {
        let result = data.data.split("|");
        this.Qty_lotno = result[0];
        this.Qty_remain = result[1];
      }
    },

    async loadComponents() {
      this.CheckPrivilege();
      this.RefreshState();
      this.loadCreateLot();
      try {
        var event = "";
        let { data } = await Repository.getApiServer(
          `GetLoadFormQWip?database_name=${localStorage.databaseName}&trans_name=${this.transaction}&lot_no=${event}`
        );
        if (data.result == "ok") {
          this.listResult = data.data;
          this.listHeader = Object.keys(this.listResult[0]);
          this.setHeader();
        }
      } catch (error) {
        if (error.response && error.response.data) {
          this.$swal("", error.response.data.error, "error");
        } else {
          this.$swal("", error.Message, "error");
        }
      }
    },
    setHeader() {
      const headers = document.querySelectorAll("th");
      headers.forEach((header) => {
        const width = header.getBoundingClientRect().width;
        header.style.width = `${width}px`;
      });
    },
    async loadCreateLot() {
      let databaseName = localStorage.databaseName;
      var { data } = await Repository.getApiServer(
        `GetLoadLotNoQWip?database_name=${databaseName}&type=CONSUME_GET_LOTNO`
      );
      if (data.result == "ok") {
        this.listCreateLot = data.data;
      }
    },
    async btnClick(event) {
      if (this.createValue.length == 0 || this.Qty <= 0) {
        this.$swal("", "Choose LOT_NO & input QTY", "error");
      } else {
        if (parseInt(this.Qty) > parseInt(this.Qty_remain)) {
          this.$swal("", "QTY can not bigger Remain_QTY", "warning");
          return;
        }
        var payload = {
          database_name: localStorage.databaseName,
          lot_no: this.createValue,
          reason: "",
          qty: this.Qty,
          transaction: this.transaction,
        };

        var { data } = await Repository.getRepo("InsertQWipTrans", payload);

        if (data.result == "ok") {
          this.$swal({
            title: "",
            text: this.createValue.toLocaleUpperCase() + " SUCCESSFULLY",
            icon: "success",
            timer: 1000,
            showConfirmButton: false,
          }).then(() => {});
          this.listResult = data.data;
          this.listHeader = Object.keys(this.listResult[0]);
          this.setHeader();
          this.labelResult = event.target.innerText.trim();
          this.isShowCreatedLot = true;

          //this.createValue = "";
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
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "QWIP_CONSUME",
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
    RefreshState() {
      this.isShowCreatedLot = false;
      this.createValue = "";
      this.WOValue = "";
      this.labelResult = "";
    },
  },
};
</script>

<style lang="scss" scoped>
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

  .input-center {
    z-index: 2;
    background-color: white;
    position: absolute;
    top: 15%;
    left: 50%;
    transform: translate(-50%, -50%);
    height: 13%;
    width: 98%;
    border-radius: 15px;
    box-shadow: 10px 10px 20px rgba(19, 84, 95, 0.233);

    .title {
      height: 48%;
      width: 100%;
      .titleSearchArea {
        font-style: italic;
        font-weight: 600;
        font-size: 18px;
        top: 16%;
        margin-top: 10px;
      }
      .titleReceviceLot {
        font-weight: 600;
        font-size: 15px;
        top: 16%;
        margin-top: 10px;
      }
      .btn-CreateLot {
        text-align: center;
        vertical-align: middle;
        background-color: transparent;
        color: #054a5c;
        border: solid 1px #054a5c;
        border-radius: 5px;
        font-size: 17px;
        height: 80%;
        width: 80%;
        box-shadow: 3px 3px #748db8;
        outline: none;
        margin-top: 5px;
        &:hover {
          color: #054a5c;
        }
        &:active {
          transform: translate(4px, 4px);
          box-shadow: none;
          outline: none;
        }
      }
    }

    .optionInput {
      height: 40%;
      width: 100%;
      .valueSearchArea {
        top: -10%;
        display: flex;
        align-items: center;
        position: relative;
        img {
          height: 20px;
          width: 20px;
          margin-top: 2px;
          position: absolute;
          left: 25px;
        }
        .search_input {
          width: 100%;
          height: 80%;
          border-radius: 20px;
          background-color: transparent;
          font-size: 15px;
          line-height: 10px;
          text-align: left;
          padding-left: 35px;
          border: solid 1px #adc3c9;
          color: #021317;

          &::placeholder {
            font-size: 14px;
            color: #3c3e3e;
          }
        }
      }
      .valueReceviceLot {
        top: -10%;
        display: flex;
        align-items: center;
        position: relative;
        select {
          width: 100%;
          height: 85%;
          border: 1px solid #adc3c9;
          background-color: white;
          color: #054a5c;
        }
      }
      .valueWO {
        @extend .valueReceviceLot;
      }

      .btn-MappingLot {
        text-align: center;
        vertical-align: middle;
        background-color: transparent;
        color: #054a5c;
        border: solid 1px #054a5c;
        border-radius: 5px;
        font-size: 17px;
        height: 160%;
        width: 80%;
        box-shadow: 3px 3px #748db8;
        outline: none;
        margin-top: -15%;
        &:hover {
          color: #054a5c;
        }
        &:active {
          transform: translate(4px, 4px);
          box-shadow: none;
          outline: none;
        }
      }
    }
  }

  .output-content {
    z-index: 1;
    background-color: white;
    position: absolute;
    top: 60%;
    left: 50%;
    transform: translate(-50%, -50%);
    height: 73%;
    width: 98%;
    border-radius: 15px;
    box-shadow: 10px 10px 20px rgba(19, 84, 95, 0.233);
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
  }
}
.dropdown-icon {
  z-index: 3;
  cursor: pointer;
  padding: 8px;
  margin-left: -3.5%;
}
</style>