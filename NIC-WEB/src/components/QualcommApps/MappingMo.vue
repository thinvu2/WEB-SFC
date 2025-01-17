<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Mapping Mo</span>
      </div>
    </div>
    <header>
      <h1>Mapping Mo</h1>
    </header>
    <main>
      <div class="container">
        <div class="div-searchbox row">
          <div class="div-searchbox-content">
            <input
              v-on:keyup.enter="querySearch()"
              v-model="valueSearch"
              type="text"
              class="form-control"
              :placeholder="
                $store.state.language == 'En'
                  ? 'Enter Lot no, Po no...'
                  : 'Nhập Lot no...'
              "
            />
            <button @click="querySearch()" class="btn">
              <Icon icon="search" class="sidenav-icon" />
            </button>
          </div>
        </div>
        <div class="col-md-4 mb-4">
          <label for="lot-no">Lot no:</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-9"
            id="lot-no"
            type="model"
            @dropdown-click="queryLotNo"
            :listAll="filterLotNo"
            @update-selected-item="updateLotNo"
            :textContent="model.lotNo"
            textPlaceHolder="Enter Lot no"
          />
          <p
            v-if="model.lotNo"
            style="display: inline-block;color: #3f3737;font-size: 13px;margin: 0;"
          >
            Po No: {{ objLotNo.PO_NO }}, QTY: {{ objLotNo.QTY }}
          </p>
        </div>

        <div class="col-md-4 mb-4">
          <label for="mo-number">Mo:</label>
          <DropdownSearch
            class="form-control form-control-sm text-element col-md-9"
            id="mo-number"
            type="model"
            @dropdown-click="queryMoNumber"
            :listAll="filterMoNumber"
            @update-selected-item="updateMoNumber"
            :textContent="model.moNumber"
            textPlaceHolder="Enter Mo"
          />
          <p
            v-if="model.moNumber"
            style="display: inline-block;color: #3f3737;font-size: 13px;margin: 0;"
          >
            Model Name: {{ objMo.MODEL_NAME }}, QTY: {{ objMo.QTY }}
          </p>
        </div>

        <button @click="saveData" class="submit" type="button">Submit</button>

        <div class="data-table">
          <table class="table-main">
            <thead>
              <tr>
                <th v-for="(item, index) in dataTableHeader" :key="index">
                  {{ item }}
                </th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in dataTable" :key="index">
                <template v-for="(value, key) in item" :key="key">
                  <td>
                    {{ value }}
                  </td>
                </template>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </main>
  </div>
</template>
<script>
import DropdownSearch from "../Template/DropdownSearch.vue";
import Repository from "../../services/Repository";
export default {
  components: {
    DropdownSearch,
  },
  data() {
    return {
      searchText: "",
      valueSearch: "",
      objLotNo: {},
      objMo: {},
      dataTableLotNoBellow: [],
      dataTableMoBellow: [],
      dataTableLotNo: [],
      dataTableMoNumber: [],
      dataTable: [],
      dataTableHeader: [],
      databaseName: localStorage.databaseName,
      empNo: localStorage.username,
      model: {
        lotNo: "",
        moNumber: "",
      },
    };
  },
  mounted() {
    this.loadDataTable();
  },
  computed: {
    filterLotNo: function() {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.dataTableLotNo;
      if (this.searchText === "") {
        return this.dataTableLotNo;
      }
      return this.dataTableLotNo.filter((user) => {
        return String(user)
          .toLowerCase()
          .includes(query);
      });
    },
    filterMoNumber: function() {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.dataTableMoNumber;
      if (this.searchText === "") {
        return this.dataTableMoNumber;
      }
      return this.dataTableMoNumber.filter((user) => {
        return String(user)
          .toLowerCase()
          .includes(query);
      });
    },
  },
  methods: {
    updateLotNo(value) {
      this.model.lotNo = value;
      this.queryLotNoBellow(this.model.lotNo);
    },
    updateMoNumber(value) {
      this.model.moNumber = value;
      this.queryMoBellow(this.model.moNumber);
    },
    async queryLotNoBellow() {
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inActionType = "GET_LOTNO_BELLOW";
      console.log("aaabb", this.model.lotNo);
      let lotNo = encodeURIComponent(this.model.lotNo);
      let moNumber = this.model.moNumber;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?databaseName=${databaseName}&empNo=${empNo}&inActionType=${inActionType}&lotNo=${lotNo}&moNumber=${moNumber}&valueSearch=${valueSearch}`
        );
        this.dataTableLotNoBellow = data.data;
        this.objLotNo = this.dataTableLotNoBellow[0];
      } catch (error) {
        console.error("queryLotNoBellow Error:", error);
        const message =
        error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async queryMoBellow() {
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inActionType = "GET_MO_BELLOW";
      console.log("aa",this.model.lotNo)
      let lotNo = encodeURIComponent(this.model.lotNo);
      let moNumber = this.model.moNumber;
      let valueSearch = this.valueSearch;

      console.log("lotNo", lotNo)
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?databaseName=${databaseName}&empNo=${empNo}&inActionType=${inActionType}&lotNo=${lotNo}&moNumber=${moNumber}&valueSearch=${valueSearch}`
        );
        this.dataTableMoBellow = data.data;
        this.objMo = this.dataTableMoBellow[0];
      } catch (error) {
        console.error("queryMoBellow Error:", error);
        const message =
        error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async querySearch() {
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inActionType = "GET_GRID";
      let lotNo = encodeURIComponent(this.model.lotNo);
      let moNumber = this.model.moNumber;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?databaseName=${databaseName}&empNo=${empNo}&inActionType=${inActionType}&lotNo=${lotNo}&moNumber=${moNumber}&valueSearch=${valueSearch}`
        );
        this.dataTable = data.data;
        if (typeof this.dataTable != "undefined") {
          if (this.dataTable.length != 0) {
            this.dataTableHeader = Object.keys(this.dataTable[0]);
          }
        }
      } catch (error) {
        console.error("querySearch Error:", error);
        const message =
        error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async saveData() {
      if (!this.model.lotNo || !this.model.moNumber) {
        this.$swal("", "Please input Lot no and Mo!", "warning");
        return;
      }
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inActionType = "LINK";
      let lotNo = encodeURIComponent(this.model.lotNo);
      let moNumber = this.model.moNumber;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?databaseName=${databaseName}&empNo=${empNo}&inActionType=${inActionType}&lotNo=${lotNo}&moNumber=${moNumber}&valueSearch=${valueSearch}`
        );
        if (data.result == "ok") {
          this.$swal("", "Successfully applied", "success");
          this.clearForm();
        } else {
          this.$swal("", data.data.substr(0, 100), "error");
        }
      } catch (error) {
        console.error("saveData Error:", error);
        const message =
        error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async loadDataTable() {
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inActionType = "GET_GRID";
      let lotNo = encodeURIComponent(this.model.lotNo);
      let moNumber = this.model.moNumber;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?databaseName=${databaseName}&empNo=${empNo}&inActionType=${inActionType}&lotNo=${lotNo}&moNumber=${moNumber}&valueSearch=${valueSearch}`
        );
        this.dataTable = data.data;
        if (typeof this.dataTable != "undefined") {
          if (this.dataTable.length != 0) {
            this.dataTableHeader = Object.keys(this.dataTable[0]);
          }
        }
      } catch (error) {
        console.error("loadDataTable Error:", error);
        const message =
        error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async queryLotNo() {
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inActionType = "GET_LOTNO";
      console.log(this.model.lotNo)
      let lotNo = encodeURIComponent(this.model.lotNo);
      let moNumber = this.model.moNumber;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?databaseName=${databaseName}&empNo=${empNo}&inActionType=${inActionType}&lotNo=${lotNo}&moNumber=${moNumber}&valueSearch=${valueSearch}`
        );
        data.data.forEach((element) => {
          this.dataTableLotNo.push(element.LOT_NO);
        });
      } catch (error) {
        console.error("queryLotNo Error:", error);
        const message =
        error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    async queryMoNumber() {
      if (!this.model.lotNo) {
        this.$swal("", "Please input Lot no first!", "warning");
        return;
      }
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inActionType = "GET_MO";
      let lotNo = encodeURIComponent(this.model.lotNo);
      let moNumber = this.model.moNumber;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?databaseName=${databaseName}&empNo=${empNo}&inActionType=${inActionType}&lotNo=${lotNo}&moNumber=${moNumber}&valueSearch=${valueSearch}`
        );
        data.data.forEach((element) => {
          this.dataTableMoNumber.push(element.MO_NUMBER);
        });
      } catch (error) {
        console.error("queryMoNumber Error:", error);
        const message =
        error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },
    clearForm() {
      this.model.moNumber = "";
      this.model.lotNo = "";
      this.dataTableLotNo = [];
      this.dataTableMoNumber = [];
      this.objLotNo = {};
      this.objMo = {};
      this.dataTableLotNoBellow = [];
      this.dataTableMoBellow = [];
      this.dataTable = [];
      this.dataTableHeader = [];
      this.loadDataTable();
    },
    BackToParent() {
      this.$router.push({ path: "/Home/QualcommApps" });
    },
  },
};
</script>
<style scoped lang="scss">
* {
  box-sizing: border-box;
  -moz-box-sizing: border-box;
  -webkit-box-sizing: border-box;
  font-family: Arial, Helvetica, sans-serif;
}
.div-all {
  padding: 0 20px;
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

h1 {
  text-align: center;
  color: #333330;
  //font-weight: bold;
  font-size: 35px;
  font-family: serif;
  font-style: italic;
  margin-top: 0;
}
.div-searchbox {
  margin-top: 15px;
  height: 45px;
  display: flex;
  align-content: center;
  justify-content: left;
  .div-searchbox-content {
    display: flex;
    margin-bottom: 10px;
    text-align: center;
    input {
      border-radius: 10px 0 0 10px;
      width: 300px;
      height: 40px;
    }
    button {
      border-radius: 0 10px 10px 0;
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
}
label {
  color: rgb(0, 0, 0);
  font-size: 1rem;
  font-weight: bold;
  float: left;
  margin-right: 5px;
}

.dropdown-wrapper {
  z-index: 3;
}

.container {
  border-radius: 5px;
  background-color: rgb(235 235 235);
  padding: 20px;
  width: 90%;
  height: auto;
}

.submit {
  display: inline-block;
  background-color: #504f4f;
  color: rgb(255 253 253);
  padding: 8px 16px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
  font-size: 18px;
}

.submit:hover {
  background-color: #050505;
  color: rgb(255, 255, 255);
  transition: 1s;
}
.query-table {
  width: 100%;
  max-height: 400px;
}

/* start */
.data-table {
  //position: sticky;
  //z-index: 1;
  //margin-top: 20px;
  width: 100%;
  max-height: 400px;
  overflow-x: hidden;
}

.table-main {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}
.table-main td,
th {
  text-align: left;
  padding: 8px;
  font-size: 1rem;
}
thead th {
  background-color: #333330;
  color: #fff;
  position: sticky;
  top: 0;
  z-index: 2;
}
tbody td {
  color: #000;
  white-space: nowrap;
  z-index: 1;
}

tbody tr:hover {
  background-color: #a19e9e;
}
/* end */
.searchbox {
  height: 40px;
  margin-bottom: 3px;
}
.searchbox input {
  width: 50%;
  height: 40px;
  border: 1px solid #ccc;
  border-radius: 5px;
}
.btn-query {
  border: 1px solid rgb(26, 25, 25);
  border-radius: 5px;
  height: 40px;
  width: 40px;
}
</style>
