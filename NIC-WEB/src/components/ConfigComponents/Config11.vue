<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config Key Parts Setup (11):</span>
      </div>
    </div>
    <div class="div-searchbox row">
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
              ? 'Enter keypart no...'
              : 'Nhập keypart no...'
          "
        />
         <button @click="QuerySearch()" class="btn">
          <Icon icon="search" class="sidenav-icon" />
        </button> 
    </div>

    </div>
    <div class="main-contain">
      <div class="row col-sm-12 div-content">
        <template v-if="DataTableHeader">
          <table id="tableMain" class="table mytable">
            <thead>
              <tr>
                <th style="width: 1px">
                  {{ $store.state.language == "En" ? "Delete" : "Xóa" }}
                </th>
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
<template v-for="(item, index) in DataTable" :key="index">
              <tr>
                <td class="td-delete" @click="DeleteRecord(item)">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 24 24"
                    width="24"
                    height="24"
                  >
                    <path fill="none" d="M0 0h24v24H0z" />
                    <path
                      fill="#FFF"
                      d="M17 6h5v2h-2v13a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V8H2V6h5V3a1 1 0 0 1 1-1h8a1 1 0 0 1 1 1v3zm1 2H6v12h12V8zm-9 3h2v6H9v-6zm4 0h2v6h-2v-6zM9 4v2h6V4H9z"
                    />
                  </svg>
                </td>
                <td class="td-edit" @click="ShowDetail(item)">
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
          </table>
        </template>
       </div>
    </div>
    <div class="div-button">
      <button
        class="btn btn-success"
        type="submit"
        @click="SaveData()"
        title="Save"
      >
        Save
      </button>
      <button
        class="btn btn-warning"
        type="button"
        @click="ClearForm()"
        title="Refresh form"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          width="24"
          height="24"
        >
          <path fill="none" d="M0 0h24v24H0z" />
          <path
            fill="red"
            d="M5.463 4.433A9.961 9.961 0 0 1 12 2c5.523 0 10 4.477 10 10 0 2.136-.67 4.116-1.81 5.74L17 12h3A8 8 0 0 0 6.46 6.228l-.997-1.795zm13.074 15.134A9.961 9.961 0 0 1 12 22C6.477 22 2 17.523 2 12c0-2.136.67-4.116 1.81-5.74L7 12H4a8 8 0 0 0 13.54 5.772l.997 1.795z"
          />
        </svg>
      </button>
    </div>
    <div class="div-bellow">
      <div class="form-row">
        <div class="col-md-4 mb-3">
          <label for="validationDefault02">KEYPART NO</label>
          <input 
          placeholder="Enter keypart no"
            autocomplete="off"
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            v-model.trim="model.KEY_PART_NO"
            required/>
        </div>

          <!-- <div class="col-md-4 mb-3">
            <label for="validationDefault02">KEYPART NAME (TYPE)</label>
            <DropdownSearch
              class="form-control form-control-sm text-element col-md-3"
              :listAll="filter_KP"
              @update-selected-item="UpdateKPNAMEReceive"
              :textContent="model.KP_NAME"
              type="model"
              textPlaceHolder="Enter model name"/>
          </div> -->

      <div class="col-md-4 mb-3">
            <label for="validationDefault02">KEYPART NAME (TYPE)</label>
          <div class="input-datalist">
            <input type="text" 
            v-model.trim="model.KP_NAME" 
            placeholder="Enter keypart name"
            autocomplete="off"
            list="datalistOptions" 
            id="KeypartName"
            @focus="showDropdownList" 
            @blur="hideDropdownList" 
            />
            <ul v-if="isDropdownVisible">
            <li v-for="KeypartName in filtered_KP" :key="KeypartName" @click="select_KP (KeypartName)">{{ KeypartName }}</li>
            </ul>
          </div>

            <datalist id="datalistOptions">
            <option v-for="(option, index) in filteredOptions" :key="index" :value="option">
              {{ option }}
            </option>
            </datalist>
          </div>

        <div class="col-md-4 mb-3">
          <label for="validationDefault02">KEYPART DESCRIPTION</label>
          <input
          placeholder="Enter keypart desc"
          autocomplete="off"
            type="text"
            class="form-control form-control-sm text-element"
            id="validationDefault01"
            v-model="model.KP_DESC"
          />
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import Repository from "../../services/Repository";
export default {
  components: {
  },
  data() {
    return {
      searchQuery: "",

      selectedOption: '',
        isDropdownVisible: false,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      columnName: [],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        KEY_PART_NO: "",
        KP_NAME: "",
        KP_DESC: "",
        MODEL_NAME:"",
        KP_USE_QTY:0,
      },
      ListKPName: [],
      ListModel:[],
      searchText: "",

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
    'model.KEY_PART_NO': function(newVal) {
      this.model.KEY_PART_NO = newVal.replace(/\s/g, '');
    },
    'model.KP_NAME': function(newVal){
      this.model.KP_NAME = newVal.replace(/^\s+|\s+$/gm,'');
    }
  },
  
  computed: {
    filtered_KP() {
          return this.ListKPName.filter(KeypartName => KeypartName.toLowerCase().includes(this.model.KP_NAME.toLowerCase()));
        },

        
    filterModel: function () {
      const query = this.searchText.toLowerCase();
      if (query.length < 3) return this.ListModel;
      if (this.searchText === "") {
        return this.ListModel;
      }
      return this.ListModel.filter((user) => {
        return String(user).toLowerCase().includes(query);
      });
    },

  },
  mounted() {
    this.CheckPrivilege();
    this.GetKPName();
    this.GetModel();
  },

  methods: {

    showDropdownList() {
          this.isDropdownVisible = true;
        },
        hideDropdownList() {
          setTimeout(() => {
            this.isDropdownVisible = false;
          }, 100);
        },
        select_KP(KeypartName) {
          this.model.KP_NAME = KeypartName;
          this.hideDropdownList();
        },

    UpdateKPNAMEReceive(value) {
      this.model.KP_NAME = value;
      this.selectedOption = value;
    },
    UpdateModelNameReceive(value) {
      this.model.MODEL_NAME = value;
    },
    async GetKPName() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config11GetKeyPartName", payload);
      data.data.forEach((element) => {
        this.ListKPName.push(element.MODEL_NAME);
      });      
    },

    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config6GetAllModel", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
    },




    async SaveData() {
      if (this.model.KEY_PART_NO === "" || this.model.KP_NAME === "") {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields Không được bỏ trống", "error");
        } else {
          this.$swal("", "Không được bỏ trống", "error");
        }
      } else {
        var titleValue = "";
        var textValue = "";
        if (localStorage.language == "En") {
          titleValue = "Are you sure edit?";
          textValue = "Once OK, data will be updated!";
        } else {
          titleValue = "Chắc chắn sửa/thêm?";
          textValue = "Dữ liệu sẽ được cập nhật";
        }
        
        this.$swal({
          title: titleValue,
          text: textValue,
          icon: "warning",
          buttons: true,
          dangerMode: true,
        }).then(async (willDelete) => {
          if (willDelete.isConfirmed == false) return;


          var { data } = await Repository.getRepo("InsertUpdateConfig11",this.model);
          if (data.result == "privilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          } else if (data.result == "ok") {
            await this.QuerySearch();
            if (localStorage.language == "En") {
              this.$swal("", "Apply successfully", "success");
            } else {
              this.$swal("", "Cập nhật thành công", "success");
            }
          } else {
            this.$swal("", data.result, "error");
          }
        });
      }
    },


    DeleteRecord(item) {
      var titleValue = "";
      var textValue = "";
      if (localStorage.language == "En") {
        titleValue = "Are you sure?";
        textValue =
          "Once deleted, you will not be able to recover this record!";
      } else {
        titleValue = "Chắc chắn xóa?";
        textValue = "Sau khi xóa sẽ không thể khôi phục!";
      }
      this.$swal({
        title: titleValue,
        text: textValue,
        icon: "warning",
        buttons: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;
        var payload = {
          database_name: localStorage.databaseName,
          EMP: localStorage.username,
          ID: item.ID,
          KEY_PART_NO: item.KEY_PART_NO,
          KP_DESC: item.KP_DESC,
          KP_NAME: item.KP_NAME,

        };
        var { data } = await Repository.getRepo("DeleteConfig11", payload);
        if (data.result == "ok") {
          await this.QuerySearch();
          if (localStorage.language == "En") {
            this.$swal("", "Apply successfully", "success");
          } else {
            this.$swal("", "Cập nhật thành công", "success");
          }
        } else if (data.result == "privilege") {
          if (localStorage.language == "En") {
            this.$swal("", "Not privilege", "error");
          } else {
            this.$swal("", "Bạn không có quyền xóa", "error");
          }
        } else {
          this.$swal("", data.result, "error");
        }
      });
    },
    ClearForm() {
      this.model.ID = "";
      this.model.KEY_PART_NO = "";
      this.model.KP_NAME = "";
      this.selectedOption="";
      this.model.KP_DESC = "";
      this.model.MODEL_NAME="";
      this.model.KP_USE_QTY=0;
    },
    ShowDetail(detail) {
      this.model.ID = detail.ID;
      this.model.KEY_PART_NO = detail.KEY_PART_NO;
      this.model.KP_NAME = detail.KP_NAME;
      this.selectedOption = detail.KP_NAME;
      this.model.KP_DESC = detail.KP_DESC;
      this.model.MODEL_NAME=detail.MODEL_NAME;
      this.model.KP_USE_QTY=detail.KP_USE_QTY;
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "KEYPART NO_",
      };
      var { data } = await Repository.getRepo("CheckConfigPrivilege", payload);
      if (data.result != "ok") {
        this.$router.push({ path: "/Home/ConfigApp" });
      } else {
        this.LoadComponent();
      }
    },
    async LoadComponent() {
      this.valueSearch = "";
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("GetConfig11Content", payload);
      this.DataTable = [];
      this.DataTable = data.data;
      if (typeof this.DataTable != "undefined") {
        if (this.DataTable.length != 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
          this.DataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
      }
    },
    async QuerySearch() {
      var payload = {
        database_name: localStorage.databaseName,
        KEY_PART_NO: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig11Content", payload);
      this.DataTable = [];
      this.DataTable = data.data;
      if (typeof this.DataTable != "undefined") {
        if (this.DataTable.length != 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
          this.DataTableHeader.forEach((element) => {
            this.columnName.push({
              label: element,
              field: element,
            });
          });
        }
      }
    },

    
  },

};
</script>

<style lang="scss" scoped>
.dropdown-wrapper {
  max-width: 100%;
  position: relative;
  margin: 0 auto;
}

.text-element {
  color: #000;
  font-weight: 555;
}
.td-delete {
  display: flex;
  justify-content: center;
  background: #e73a23;
  cursor: pointer;
  height: 30px;
  align-items: center;
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
.btn-warning {
  margin-left: 20px;
}
.btn-danger {
  margin-left: 20px;
  height: 40px;
}
.div-button {
  margin-top: 10px;
  position: relative;
  right: 50px;
  text-align: right;
}
.div-bellow {
  margin-top: 5px;
  background: #1c87b5;
  color: #fff;
  //padding: 15px;
  padding: 20px 20px 35px 20px;
  margin-right: 20px;
  div {
    div {
      label {
        font-size: 14px;
        font-weight: bold;
        color: #9ff9c8;
      }
    }
  }
}
.div-all {
  margin-left: 35px;
}
.div-searchbox {
  margin-top: 15px;
  height: 60px;
  display: flex;
  align-content: center;
  justify-content: left;
  .div-searchbox-content {
    // position: absolute;
    display: flex;
    // bottom: 0;
    margin-bottom: 10px;
    // left: 50%;
    text-align: center;
    // input {
    //   border-radius: 10px 0 0 10px;
    //   // padding: 0px 5px;
    //   width: 400px;

    // }
    button {
      border-radius: 0 10px 10px 0;
      padding: 0 20px;
      background: #ff7a1c;
      height: 35px;
      color: #fff;
      box-sizing: 0;
      cursor: pointer;
      transition: 0.5s;
      &:hover {
        background:#db6008;
      }
    }
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
.main-contain {
  height: 350px;
  overflow: auto;
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
      font-size: 13px;
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
      font-size: 13px;
      font-weight: 555;
    }
  }
}
.lb_checked {
  color: #efef8d !important;
}

input{
  transition: 0.2s;
  width:300px;
  height: 35px;
  margin:0;
  box-sizing:border-box;
  padding:8px;
  //border:1px solid #fff;
  outline:none;
  border-radius:4px;
  //background:#fff;
  color:#000;
  position: relative;
  z-index: 2;
}

input:focus{
  border: 1px solid #db9c2e; 
  box-shadow: 0px 0px 3px 0px #f2f2f2;
  border-radius:4px 4px 0 0;}


.input-datalist {
      position: relative;
      display: inline-block;
    }

    .input-datalist input[type="text"] {
      width: 300px;
      padding: 10px;
      border: 1px solid #ccc;
      border-radius: 4px;
      margin: 0;
      box-sizing: border-box;
 
    }

    .input-datalist ul {
      position: absolute;
      top: 100%;
      left: 0;
      z-index: 1;
      width: 100%;
      padding: 0;
      margin: 5px 0;
      list-style: none;
      background-color: #b7b7b7;
      border: 1px solid #ccc;
      border-radius: 4px;
      box-shadow: 0 2px 4px rgb(255, 253, 253);
      color: #000;
      max-height: 10rem;
      overflow: auto;
    }

    .input-datalist li {
      padding: 8px 10px;
      cursor: pointer;
    }

    .input-datalist li:hover {
      background-color: #db9c2e;
    }

    .input-datalist li:last-child {
      border-bottom-left-radius: 4px;
      border-bottom-right-radius: 4px;
    }

</style>