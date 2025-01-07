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
            <label for="lot-no">Lot no</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-9"
              id="lot-no"
              type="model"
              @dropdown-click="queryLotNo"
              :listAll="filterLotNo"
              @update-selected-item="updateLotNo"
              :textContent="model.LOT_NO"
              textPlaceHolder="Enter Lot no"
            />
            <p v-if="model.LOT_NO" style="display: inline-block;color: #3f3737;font-size: 13px;margin: 0;">Po No: {{ objLotNo.PO_NO }}, QTY: {{ objLotNo.QTY }}</p>
          </div>

          <div class="col-md-4 mb-4">
            <label for="mo-number">Mo</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-9"
              id="mo-number"
              type="model"
              @dropdown-click="queryMoNumber"
              :listAll="filterMoNumber"
              @update-selected-item="updateMoNumber"
              :textContent="model.MO_NUMBER"
              textPlaceHolder="Enter Mo"
            />
            <p v-if="model.MO_NUMBER" style="display: inline-block;color: #3f3737;font-size: 13px;margin: 0;">Model Name: {{ objMo.MODEL_NAME }}, QTY: {{ objMo.QTY }}</p>
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
                <tr v-for="(item, index) in dataTable" :key="index" >
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
      model: {
        database_name: localStorage.databaseName,
        EMP_NO: localStorage.username,
        LOT_NO: "",
        MO_NUMBER: "",
      },
    };
  },
    mounted() {
      console.log("this.model.LOT_NO", this.model.LOT_NO)
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
      this.model.LOT_NO = value;
      this.queryLotNoBellow(this.model.LOT_NO);
    },
    updateMoNumber(value) {
      this.model.MO_NUMBER = value;
      this.queryMoBellow(this.model.MO_NUMBER);
    },
    async queryLotNoBellow() {
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "GET_LOTNO_BELLOW";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      let valueSearch = this.valueSearch;
      console.log("IN_ACTION_TYPE:", IN_ACTION_TYPE)
      console.log("LOT_NO: ",LOT_NO)
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}&valueSearch=${valueSearch}`
        );
        this.dataTableLotNoBellow = data.data;
        this.objLotNo = this.dataTableLotNoBellow[0];

        console.log('this.objLotNo: ', this.objLotNo);

      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
    async queryMoBellow() {
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "GET_MO_BELLOW";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}&valueSearch=${valueSearch}`
        );
        this.dataTableMoBellow = data.data;
        this.objMo = this.dataTableMoBellow[0];

        console.log('this.objMo: ', this.objMo);

      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
    async querySearch() {
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "GET_GRID";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}&valueSearch=${valueSearch}`
        );
        this.dataTable = data.data;
        if (typeof this.dataTable != "undefined") {
          if (this.dataTable.length != 0) {
            this.dataTableHeader = Object.keys(this.dataTable[0]);
          }
        }
      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
    async saveData() {
      if(!LOT_NO || !MO_NUMBER) {
        this.$swal("", "Please input Lot no and Mo!", "warning");
        return;
      }
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "LINK";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      let valueSearch = this.valueSearch;

      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}&valueSearch=${valueSearch}`
        );
        if (data.result == "ok") {
          this.$swal("", "Successfully applied", "success");
          await this.clearForm();
        } else {
          this.$swal("", data.data, "error");
        }
      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
    async loadDataTable() {
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "GET_GRID";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}&valueSearch=${valueSearch}`
        );
        this.dataTable = data.data;
        if (typeof this.dataTable != "undefined") {
          if (this.dataTable.length != 0) {
            this.dataTableHeader = Object.keys(this.dataTable[0]);
          }
        }
      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
    async queryLotNo() {
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "GET_LOTNO";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      let valueSearch = this.valueSearch;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}&valueSearch=${valueSearch}`
        );
        data.data.forEach((element) => {
          this.dataTableLotNo.push(element.LOT_NO);
        });
      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
    async queryMoNumber() {
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "GET_MO";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      let valueSearch = this.valueSearch;
      if(!LOT_NO) {
        this.$swal("", "Please input Lot no first!", "warning");
        return;
      }
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}&valueSearch=${valueSearch}`
        );
        this.dataTableMoNumber = [];
        data.data.forEach((element) => {
          this.dataTableMoNumber.push(element.MO_NUMBER);
        });
      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
   async clearForm() {
      this.model.MO_NUMBER = '';
      this.model.LOT_NO = '';
      this.dataTableLotNo = [];
      this.dataTableMoNumber = [];
      this.objLotNo = {};
      this.objMo = {};
      this.dataTableLotNoBellow = [];
      this.dataTableMoBellow = [];
      this.dataTable = [];
      this.dataTableHeader = [];
     await this.loadDataTable();
    },
    BackToParent() {
      this.$router.push({ path: "/Home/Qualcomm_Application" })
    },
  },
};
</script>
<style scoped lang="scss">
body {
  font-family: Arial, Helvetica, sans-serif;
}
* {
  box-sizing: border-box;
}

.div-all {
  //background-color: rgb(235 235 235);
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
  background-color: rgb(235 235 235);;
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
