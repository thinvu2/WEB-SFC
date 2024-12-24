<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Config CUST SNRULE (19):</span>
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
              ? 'Enter model name...'
              : 'Nhập tên model...'
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

      <div class="form-row-all">
        <div class="form-row">
          <label for="customer-input">CUSTOMER</label>
          <input
            id="customer-input"
            v-model="model.CUSTOMER"
            type="text"
            class="input-class"
            readonly
          />
        </div>
        <div class="form-row">
          <label for="cust-sn-legnth-input">CUST SN LENGTH</label>
          <input
            type="number"
            class="input-class"
            id="cust-sn-legnth-input"
            v-model="model.CUST_SN_LENG"
          />
        </div>
        <div class="form-row">
          <label for="model-name-input">MODEL NAME</label>
          <DropdownSearch
            :listAll="ListModel"
            @update-selected-item="UpdateModelReceive"
            :textContent="model.MODEL_NAME"
            type="model"
            id="model-name-input"
            class="input-class"
            textPlaceHolder="Enter model name"
          />
        </div>
        <div class="form-row">
          <label for="cust-sn-str-input">CUST SN STR</label>
          <input
            type="text"
            class="input-class"
            id="cust-sn-str-input"
            v-model="model.CUST_SN_STR"
          />
        </div>

        <div class="form-row">
          <label for="version-code-input">VERSION CODE</label>
          <input
            id="version-code-input"
            type="text"
            class="input-class"
            v-model="model.VERSION_CODE"
          />
        </div>
        <div class="form-row">
          <label for="cust-box-prefix-input">CUST BOX PREFIX</label>
          <input
            type="text"
            class="input-class"
            id="cust-box-prefix-input"
            v-model="model.CUST_BOX_PREFIX"
          />
        </div>

        <div class="form-row">
          <label for="cust-model-name-input">CUST MODEL NAME</label>
          <input
            type="text"
            class="input-class"
            id="cust-model-name-input"
            v-model="model.CUST_MODEL_NAME"
          />
        </div>
        <div class="form-row">
          <label for="cust-box-length-input">CUST BOX LENGTH</label>
          <input
            type="number"
            class="input-class"
            id="cust-box-length-input"
            v-model="model.CUST_BOX_LENG"
          />
        </div>
        <div class="form-row">
          <label for="cust-version-code-input">CUST VERSION CODE</label>
          <input
            type="text"
            class="input-class"
            id="cust-version-code-input"
            v-model="model.VERSION_CODE"
          />
        </div>
        <div class="form-row">
          <label for="cust-box-str-input">CUST BOX STR</label>
          <input
            type="text"
            class="input-class"
            id="cust-box-str-input"
            v-model="model.CUST_BOX_STR"
          />
        </div>
        <div class="form-row">
          <label for="cust-model-desc-input">CUST MODEL DESC</label>
          <input
            type="text"
            class="input-class"
            id="cust-model-desc-input"
            v-model="model.CUST_MODEL_DESC"
          />
        </div>
        <div class="form-row">
          <label for="cust-carton-prefix-input">CUST CARTON PREFIX</label>
          <input
            type="text"
            class="input-class"
            id="cust-carton-prefix-input"
            v-model="model.CUST_CARTON_PREFIX"
          />
        </div>

        <div class="form-row">
          <label for="carton-label-name-input">CARTON LABEL NAME</label>
          <input
            type="text"
            class="input-class"
            id="carton-label-name-input"
            v-model="model.CARTON_LAB_NAME"
          />
        </div>
        <div class="form-row">
          <label for="cust-carton-postfix-input">CUST CARTON POSTFIX</label>
          <input
            type="text"
            class="input-class"
            id="cust-carton-postfix-input"
            v-model="model.CUST_CARTON_POSTFIX"
          />
        </div>
        <div class="form-row">
          <label for="upceandata-input">UPCEANDATA</label>
          <input
            type="text"
            class="input-class"
            id="upceandata-input"
            v-model="model.UPCEANDATA"
          />
        </div>
        <div class="form-row">
          <label for="cust-carton-length-input">CUST CARTON LENGTH</label>
          <input
            type="number"
            class="input-class"
            id="cust-carton-length-input"
            v-model="model.CUST_CARTON_LENG"
          />
        </div>
        <div class="form-row">
          <label for="cust-sn-prefix-input">CUST SN PREFIX</label>
          <input
            type="text"
            class="input-class"
            id="cust-sn-prefix-input"
            v-model="model.CUST_SN_PREFIX"
          />
        </div>
        <div class="form-row">
          <label for="cust-carton-str-input">CUST CARTON STR</label>
          <input
            type="text"
            class="input-class"
            id="cust-carton-str-input"
            v-model="model.CUST_CARTON_STR"
          />
        </div>
        <div class="form-row">
          <label for="cust-pallet-str-input">CUST PALLET STR</label>
          <input
            type="text"
            class="input-class"
            id="cust-pallet-str-input"
            v-model="model.CUST_PALLET_STR"
          />
        </div>
        <div class="form-row">
          <label for="cust-pallet-prefix-input">CUST PALLET PREFIX</label>
          <input
            type="text"
            class="input-class"
            id="cust-pallet-prefix-input"
            v-model="model.CUST_PALLET_PREFIX"
          />
        </div>

        <div class="form-row">
          <label for="snqty-label-input">SN QTY / LABEL</label>
          <input
            type="number"
            class="input-class"
            id="snqty-label-input"
            v-model="model.SN_QTY_LABEL"
          />
        </div>

        <div class="form-row">
          <label for="cust-pallet-postfix-input">CUST PALLET POSTFIX</label>
          <input
            type="text"
            class="input-class"
            id="cust-pallet-postfix-input"
            v-model="model.CUST_PALLET_POSTFIX"
          />
        </div>
        <div class="form-row">
          <label for="cust-pallet-legnth-input">CUST PALLET LENGTH</label>
          <input
            type="number"
            class="input-class"
            id="cust-pallet-legnth-input"
            v-model="model.CUST_PALLET_LENGTH"
          />
        </div>
        <div class="form-row">
          <label for="pallet-label-name-input">PALLET LABEL NAME</label>
          <input
            type="text"
            class="input-class"
            id="pallet-label-name-input"
            v-model="model.PALLET_LAB_NAME"
          />
        </div>
      </div>
  </div>
</template>
<script>
import Repository from "../../services/Repository";
import DropdownSearch from "../Template/DropdownSearch.vue";
export default {
  components: {
    DropdownSearch,
  },
  data() {
    return {
      textContent: "",
      searchText: "",
      selectedItem: null,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      columnName: [],
      valueSearch: "",
      line_name: "",
      line_type: "",
      line_code: "",
      line_desc: "",
      ListModel: [],
      model: {
        ID: "",
        database_name: localStorage.databaseName,
        EMP: localStorage.username,
        fun: "CUST SNRULE_",
        CUSTOMER: "FOXCONN",
        CUST_NAME: "",
        CUST_CODE: "",
        MODEL_NAME: "",
        VERSION_CODE: "",
        CUST_MODEL_NAME: "",
        CUST_MODEL_DESC: "",
        CARTON_LAB_NAME: "C_DEFAULT.LAB",
        UPCEANDATA: "",
        CUST_SN_PREFIX: "",
        CUST_VENDER_CODE: "",
        CUST_SN_POSTFIX: "",
        CUST_SN_LENG: "",
        CUST_SN_STR: "",
        CUST_CARTON_PREFIX: "",
        CUST_CARTON_POSTFIX: "",
        CUST_CARTON_LENG: "6",
        CUST_CARTON_STR: "",
        CUST_PALLET_PREFIX: "",
        CUST_PALLET_POSTFIX: "",
        CUST_PALLET_LENGTH: "6",
        CUST_PALLET_STR: "",
        CUST_LAST_SN: "",
        CUST_LAST_CARTON: "",
        CUST_LAST_PALLET: "",
        EMP_NO: "",
        SN_QTY_LABEL: "",
        IN_STATION_TIME: "",
        PALLET_LAB_NAME: "P_DEFAULT.LAB",
        MACID_PREFIX: "",
        CUST_BOX_PREFIX: "",
        CUST_BOX_LENG: "0",
        CUST_LAST_BOX: "",
        CUST_BOX_STR: "",
        FINISH_GOOD: "",
      },
      listChecked: [],
      //ListCustomer: [],
    };
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.isVisible = false;
      }
    });
  },
  computed: {
    filterUser: function () {
      const query = this.searchText.toLowerCase();
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
    this.GetModel();
    //this.GetCustomer();
  },
  methods: {
    // UpdateCustReceive(value) {
    //   this.model.CUST_NAME = value.CUSTOMER;
    //   this.model.CUST_CODE = value.CUST_CODE;
    // },
    UpdateModelReceive(value) {
      this.model.MODEL_NAME = value;
      this.model.CUST_MODEL_NAME = value;
     // this.model.PALLET_LAB_NAME = `P_${this.model.MODEL_NAME}.LAB`;
      this.model.CARTON_LAB_NAME = `${this.model.MODEL_NAME}.LAB`;
    },
    async GetModel() {
      var payload = {
        database_name: localStorage.databaseName,
      };
      var { data } = await Repository.getRepo("Config19GetAllModel", payload);
      data.data.forEach((element) => {
        this.ListModel.push(element.MODEL_NAME);
      });
    },
    SetTextDropDown(text) {
      this.textContent = text;
      this.isVisible = false;
    },
    // async GetCustomer() {
    //   var payload = {
    //     database_name: localStorage.databaseName,
    //   };
    //   var { data } = await Repository.getRepo("Config19GetCustomer", payload);
    //   this.ListCustomer = data.data;
    // },
    ChangeModelName() {
      //this.model.PALLET_LAB_NAME = `P_${this.model.MODEL_NAME}.LAB`;
      this.model.CARTON_LAB_NAME = `${this.model.MODEL_NAME}.LAB`;
    },
    async SaveData() {
      if (this.model.MODEL_NAME == "" || this.model.VERSION_CODE == "") {
        if (localStorage.language == "En") {
          this.$swal("", "Empty fields", "error");
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
          titleValue = "Chắc chắn sửa?";
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

          var { data } = await Repository.getRepo(
            "InsertUpdateConfig19",
            this.model
          );
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
          ID: item.ID,
          EMP: localStorage.username,
          MODEL_NAME: item.MODEL_NAME,
          VERSION_CODE: item.VERSION_CODE,
        };
        var { data } = await Repository.getRepo("DeleteConfig19", payload);
        if (data.result == "ok") {
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
      this.line_name = "";
      this.line_type = "";
      this.line_code = "";
      this.line_desc = "";
    },
    ClearForm() {
      this.model.ID = "";
      this.model.CUST_CODE = "";
      this.model.MODEL_NAME = "";
      this.model.VERSION_CODE = "";
      this.model.CUST_MODEL_NAME = "";
      this.model.CUST_MODEL_DESC = "";
      this.model.CARTON_LAB_NAME = "C_DEFAULT.LAB";
      this.model.UPCEANDATA = "";
      this.model.CUST_SN_PREFIX = "";
      this.model.CUST_VENDER_CODE = "";
      this.model.CUST_SN_POSTFIX = "";
      this.model.CUST_SN_LENG = "";
      this.model.CUST_SN_STR = "";
      this.model.CUST_CARTON_PREFIX = "AMBIT";
      this.vCUST_CARTON_POSTFIX = "";
      this.model.CUST_CARTON_LENG = "6";
      this.model.CUST_CARTON_STR = "";
      this.model.CUST_PALLET_PREFIX = "AMBIT";
      this.model.CUST_PALLET_POSTFIX = "";
      this.model.CUST_PALLET_LENGTH = "6";
      this.model.CUST_PALLET_STR = "";
      this.model.CUST_LAST_SN = "";
      this.model.CUST_LAST_CARTON = "";
      this.model.CUST_LAST_PALLET = "";
      this.model.EMP_NO = "";
      this.model.SN_QTY_LABEL = "";
      this.model.IN_STATION_TIME = "";
      this.model.PALLET_LAB_NAME = "P_DEFAULT.LAB";
      this.model.MACID_PREFIX = "";
      this.model.CUST_BOX_PREFIX = "";
      this.model.CUST_BOX_LENG = "0";
      this.model.CUST_LAST_BOX = "";
      this.model.CUST_BOX_STR = "";
      this.model.FINISH_GOOD = "";
    },
    ShowDetail(detail) {
      this.model.ID = detail.ID;
      this.model.CUST_NAME = detail.CUSTOMER;
      this.model.CUST_CODE = detail.CUST_CODE;
      this.model.MODEL_NAME = detail.MODEL_NAME;
      this.model.VERSION_CODE = detail.VERSION_CODE;
      this.model.CUST_MODEL_NAME = detail.CUST_MODEL_NAME;
      this.model.CUST_MODEL_DESC = detail.CUST_MODEL_DESC;
      this.model.CARTON_LAB_NAME = detail.CARTON_LAB_NAME;
      this.model.UPCEANDATA = detail.UPCEANDATA;
      this.model.CUST_SN_PREFIX = detail.CUST_SN_PREFIX;
      this.model.CUST_VENDER_CODE = detail.CUST_VENDER_CODE;
      this.model.CUST_SN_POSTFIX = detail.CUST_SN_POSTFIX;
      this.model.CUST_SN_LENG = detail.CUST_SN_LENG;
      this.model.CUST_SN_STR = detail.CUST_SN_STR;
      this.model.CUST_CARTON_PREFIX = detail.CUST_CARTON_PREFIX;
      this.model.CUST_CARTON_POSTFIX = detail.CUST_CARTON_POSTFIX;
      this.model.CUST_CARTON_LENG = detail.CUST_CARTON_LENG;
      this.model.CUST_CARTON_STR = detail.CUST_CARTON_STR;
      this.model.CUST_PALLET_PREFIX = detail.CUST_PALLET_PREFIX;
      this.model.CUST_PALLET_POSTFIX = detail.CUST_PALLET_POSTFIX;
      this.model.CUST_PALLET_LENGTH = detail.CUST_PALLET_LENGTH;
      this.model.CUST_PALLET_STR = detail.CUST_PALLET_STR;
      this.model.CUST_LAST_SN = detail.CUST_LAST_SN;
      this.model.CUST_LAST_CARTON = detail.CUST_LAST_CARTON;
      this.model.CUST_LAST_PALLET = detail.CUST_LAST_PALLET;
      this.model.EMP_NO = detail.EMP_NO;
      this.model.SN_QTY_LABEL = detail.SN_QTY_LABEL;
      this.model.IN_STATION_TIME = detail.IN_STATION_TIME;
      this.model.PALLET_LAB_NAME = detail.PALLET_LAB_NAME;
      this.model.MACID_PREFIX = detail.MACID_PREFIX;
      this.model.CUST_BOX_PREFIX = detail.CUST_BOX_PREFIX;
      this.model.CUST_BOX_LENG = detail.CUST_BOX_LENG;
      this.model.CUST_LAST_BOX = detail.CUST_LAST_BOX;
      this.model.CUST_BOX_STR = detail.CUST_BOX_STR;
      this.model.FINISH_GOOD = detail.FINISH_GOOD;
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
    async CheckPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
        fun: "CUST SNRULE",
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
      var { data } = await Repository.getRepo("GetConfig19Content", payload);
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
        model_name: this.valueSearch,
      };
      var { data } = await Repository.getRepo("GetConfig19Content", payload);
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
.form-row-all {
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: repeat(12, 30px);
  row-gap: 5px;
  width: 80%;
  margin: 0 auto;
}
.form-row {
  display: flex;
  justify-content: normal;
}
.form-row label{
  font-size: 16px;
  color: #000;
  width: 30%;
}
.input-class {
  height: 30px;
  width: 50%;
}

.div-all {
  margin-left: 35px;
}
.div-searchbox {
  height: 60px;
  display: flex;
  align-content: center;
  justify-content: left;
  .div-searchbox-content {
    display: flex;;
    text-align: center;
    input {
      border-radius: 10px 0 0 10px;
      width: 400px;
    }
    button {
      border-radius: 0 10px 10px 0;
      padding: 6px 20px;
      background: #ff7a1c;
      color: #fff;
      box-sizing: 0;
      &:hover {
        background: #f88838;
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
  height: 300px;
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
.selected-item {
    height: 100%;
    width: 100%;
    border: 1px solid;
    color: black;
    align-items: center;
    font-size: 14px;
    font-weight: 555;
    display: flex;
    justify-content: space-between;
}
.dropdown-wrapper {
  max-width: 100%;
  position: relative;
  margin: 0;
  border: 1px solid black;
}
.dropdown-popover.visible {
  margin: 0;
}
</style>