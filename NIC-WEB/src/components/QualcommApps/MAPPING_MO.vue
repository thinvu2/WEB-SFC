<template>
  <div class="div-all">
    <header>
      <h1>MAPPING MO</h1>
    </header>
    <main>
      <div class="container">
        <form @submit.prevent="saveData">
          <div class="div-searchbox row">
            <div class="div-searchbox-content">
              <input
                v-on:keyup.enter="QuerySearch()"
                v-model="valueSearch"
                type="text"
                class="form-control"
                @click="selectAll"
                :placeholder="
                  $store.state.language == 'En'
                    ? 'Enter Lot no...'
                    : 'Nhập Lot no...'
                "
              />
              <button @click="QuerySearch()" class="btn">
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
              @click="queryLotNo()"
              :listAll="filterLotNo"
              @update-selected-item="updateLotNo"
              :textContent="model.LOT_NO"
              textPlaceHolder="Enter Lot no"
            />
          </div>

          <div class="col-md-4 mb-4">
            <label for="mo-number">Mo</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-9"
              id="mo-number"
              type="model"
              @click="queryMoNumber()"
              :listAll="filterMoNumber"
              @update-selected-item="updateMoNumber"
              :textContent="model.MO_NUMBER"
              textPlaceHolder="Enter Mo"
            />
          </div>

          <button @click="saveData" class="submit" type="button">Submit</button>

          <div class="data-table">
            <table class="tableMain">
              <thead>
                <tr>
                  <th v-for="(item, index) in dataTableHeader" :key="index">
                    {{ item }}
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item2, index2) in dataTable" :key="index2">
                  <td v-for="(item3, index3) in item2" :key="index3">
                    {{ item3 }}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </form>
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
    },
    updateMoNumber(value) {
      this.model.MO_NUMBER = value;
    },

    async saveData() {
      let database_name = localStorage.databaseName;
      let EMP_NO = this.model.EMP_NO;
      let IN_ACTION_TYPE = "LINK";
      let LOT_NO = this.model.LOT_NO;
      let MO_NUMBER = this.model.MO_NUMBER;
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}`
        );
        if (data.result == "ok") {
          this.$swal("", "Successfully applied", "success");
          await this.clearForm();
        } else {
          this.$swal("", data.data, "error");
        }
        await this.clearForm();
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
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}`
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
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}`
        );
        this.dataTableLotNo = [];
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
      try {
        const { data } = await Repository.getApiServer(
          `GetMappingMo?database_name=${database_name}&EMP_NO=${EMP_NO}&IN_ACTION_TYPE=${IN_ACTION_TYPE}&LOT_NO=${LOT_NO}&MO_NUMBER=${MO_NUMBER}`
        );
        console.log("data.data: ", data.data);
        this.dataTableMoNumber = [];
        data.data.forEach((element) => {
          this.dataTableMoNumber.push(element.MO_NUMBER);
        });
      } catch (error) {
        this.$swal("error ex", error, "error");
      }
    },
    clearForm() {
      this.model.MO_NUMBER = "";
      this.model.LOT_NO = "";
      this.loadDataTable();
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
  padding: 50px 0 80px 0;
}

h1 {
  text-align: center;
  color: #333330;
  font-weight: bold;
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
/* main{
    display: grid;
    justify-content: center;
    max-height: 400px;
    grid-template-columns: 20% 40%;
    gap: 20px;
  
  } */
//   form input{
//     width: 100%;
//     padding: 12px;
//     font-size: 1rem;
//     border: 1px solid #ccc;
//     border-radius: 4px;
//     box-sizing: border-box;
//     margin-top: 6px;
//     margin-bottom: 16px;
//     resize: vertical;
//   }

label {
  color: rgb(0, 0, 0);
  font-size: 1rem;
  font-weight: bold;
}

.container {
  border-radius: 5px;
  background-color: rgb(235 235 235);;
  padding: 20px;
  width: 70%;
  height: auto;
}

.submit {
  display: inline-block;
  background-color: #a19d9d;
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
  position: sticky;
  z-index: 1;
  margin-top: 20px;
  width: 100%;
}

table {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}
table td,
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
