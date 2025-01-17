<template>
  <div class="div-all">
    <div class="row">
      <div class="div-back" @click="backToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>ASN OUT</span>
      </div>
    </div>
    <!-- Search Box -->
    <div class="searchbox">
      <div class="searchbox-content">
        <input
          :disabled="showTimeForm"
          v-on:keyup.enter="loadComponent()"
          v-model="valueSearch"
          type="text"
          class="form-control"
          placeholder="Enter To... "
        />
        <button
          @click="showTimeForm ? querySearch() : loadComponent()"
          class="btn-button"
        >
          <Icon icon="search" class="sidenav-icon" />
        </button>
      </div>
      <!-- datepicker -->
      <div class="searchbox-timerange">
        <div class="showTimeForm">
          <input id="showTimeForm" type="checkbox" v-model="showTimeForm" />
        </div>
        <div class="datepicker-wrapper">
          <datepicker
            :disabled="!showTimeForm"
            class="form-control form-control-sm"
            v-model="dateFrom"
            :upperLimit="dateTo"
          />
        </div>
        <div class="datepicker-wrapper">
          <datepicker
            :disabled="!showTimeForm"
            class="form-control form-control-sm"
            v-model="dateTo"
            :lowerLimit="dateFrom"
          />
        </div>
      </div>
    </div>
    <!-- start -->
    <div class="searchbox-asn-out">
      <div class="searchbox-content">
        <input
          v-on:keyup.enter="querySearch()"
          v-model="searchDN"
          type="text"
          class="form-control"
          placeholder="Enter DN..."
        />
        <button @click="submitBtn()" class="submit-form">Submit</button>
      </div>
    </div>
    <!--end asn out -->
    <div class="export-excel">
      <img
        @click="exportexcelxlsx()"
        class="img-excel"
        src="assets/img/xlsx-icon.jpg"
        alt=""
      />
      <img
        @click="exportExcel()"
        class="img-excel"
        src="assets/img/excel_64.png"
        alt=""
      />
    </div>
    <!-- table inside -->
    <div class="main-contain">
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
              <template v-for="(item, index) in DataTable" :key="index">
                <tr>
                  <template v-for="(item1, index1) in item" :key="index1">
                    <td>
                      {{ item1 }}
                    </td>
                  </template>
                </tr>
              </template>
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
      showTimeForm: false,
      isShowSubmitForm: false,
      showError: false,
      dateFrom: new Date(),
      dateTo: new Date(),
      isShowForm: false,
      isVisible: false,
      DataTableHeader: [],
      DataTable: [],
      valueSearch: "",
      searchDN: "",
      databaseName: localStorage.databaseName,
      empNo: localStorage.username,
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
    async submitBtn() {
      let titleValue = "";
      let textValue = "";
      titleValue = "Are you sure edit?";
      textValue = "Once OK, data will be updated!";
      this.$swal({
        title: titleValue,
        text: textValue,
        icon: "warning",
        buttons: true,
        dangerMode: true,
      }).then(async (willDelete) => {
        if (willDelete.isConfirmed == false) return;

        let databaseName = this.databaseName;
        let empNo = this.empNo;
        let inFunc = "INSERTDATA";
        const dateFrom = "";
        const dateTo = "";
        let packslipNo = this.searchDN;
        try {
          let { data } = await Repository.getApiServer(
            `QAsnOutDataTable?databaseName=${databaseName}&inFunc=${inFunc}&inData=${packslipNo}&empNo=${empNo}&dateFrom=${dateFrom}&dateTo=${dateTo}`
          );
          if (data.result == "ok") {
            this.clearForm();
            this.$swal("", "Successfully applied", "success");
          } else if (data.result == "notPrivilege") {
            if (localStorage.language == "En") {
              this.$swal("", "Not privilege", "error");
            } else {
              this.$swal("", "Bạn không có quyền thêm sửa", "error");
            }
          } else {
            this.$swal("", data.data, "error");
          }
        } catch (error) {
          console.error("Error:", error);
          const message =
            error.response?.data?.message ||
            error.response?.data?.error ||
            error.message ||
            "An unexpected error occurred.";
          this.$swal("", message, "error");
        }
      });
    },

    async loadComponent() {
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      let inFunc = "SHOWLISTBYPACKNO";
      const dateFrom = "";
      const dateTo = "";
      let packslipNo = this.valueSearch;
      try {
        let { data } = await Repository.getApiServer(
          `QAsnOutDataTable?databaseName=${databaseName}&inFunc=${inFunc}&inData=${packslipNo}&empNo=${empNo}&dateFrom=${dateFrom}&dateTo=${dateTo}`
        );
        this.DataTable = [];
        this.DataTableHeader = [];
        this.DataTable = data.data;
        if (this.DataTable.length > 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
        }
      } catch (error) {
        console.error("loadComponent Error:", error);
        const message =
          error.response?.data?.message ||
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },

    async querySearch() {
      const formatDate = (date) => {
        if (!date) return "";
        return `${this.pad(date.getFullYear())}${this.pad(
          date.getMonth() + 1
        )}${this.pad(date.getDate())}`;
      };
      const dateFrom = formatDate(this.dateFrom);
      const dateTo = formatDate(this.dateTo);
      let empNo = this.empNo;
      let inFunc = "SHOWLISTBYTIME";
      let databaseName = this.databaseName;
      let packslipNo = this.valueSearch;
      try {
        let { data } = await Repository.getApiServer(
          `QAsnOutDataTable?databaseName=${databaseName}&inFunc=${inFunc}&inData=${packslipNo}&empNo=${empNo}&dateFrom=${dateFrom}&dateTo=${dateTo}`
        );
        this.DataTable = [];
        this.DataTableHeader = [];
        this.DataTable = data.data;
        if (this.DataTable.length > 0) {
          this.DataTableHeader = Object.keys(this.DataTable[0]);
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
    pad(number) {
      return number < 10 ? `0${number}` : `${number}`;
    },
    exportexcelxlsx() {
      const filteredData = this.DataTable.map((element) => {
        return Object.keys(element).reduce((acc, key) => {
          if (key !== "FLAG" && key !== "ARRANGE") {
            acc[key] = element[key];
          }
          return acc;
        }, {});
      });
      let ws = xlsx.utils.json_to_sheet(filteredData);
      let wb = xlsx.utils.book_new();
      xlsx.utils.book_append_sheet(wb, ws, "data");
      xlsx.writeFile(wb, "download.xlsx");
    },
    exportExcel() {
      const filteredData = this.DataTable.map((element) => {
        return Object.keys(element).reduce((acc, key) => {
          if (key !== "FLAG" && key !== "ARRANGE") {
            acc[key] = element[key];
          }
          return acc;
        }, {});
      });
      const items = filteredData;
      const replacer = (key, value) => (value === null ? "" : value);
      const header = Object.keys(items[0]);
      const csv = [
        header.join(","),
        ...items.map((row) =>
          header
            .map((fieldName) => JSON.stringify(row[fieldName], replacer))
            .join(",")
        ),
      ].join("\r\n");
      let downloadLink = document.createElement("a");
      let blob = new Blob(["\ufeff", csv]);
      let url = URL.createObjectURL(blob);
      downloadLink.href = url;
      downloadLink.download = "data.csv";
      document.body.appendChild(downloadLink);
      downloadLink.click();
      document.body.removeChild(downloadLink);
    },
    clearForm() {
      this.isShowSubmitForm = false;
      this.searchDN = "";
      this.loadComponent();
    },
    backToParent() {
      this.$router.push({ path: "/Home/QualcommApps" });
    },
  },
};
</script>

<style scoped>
.div-all {
  font-family: Arial, sans-serif;
  margin: 0;
  padding: 10px;
  background-color: #f9f9f9;
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

.searchbox-asn-out {
  display: inline-block;
  /* margin-bottom: 5px; */
  width: 30%;
  border: 1px solid #ccc;
  border-radius: 8px;
  /* padding: 10px; */
  padding-top: 10px;
  padding-left: 10px;
  margin-left: 10px;
  background-color: #f9f9f9;
}

.btn-button {
  border-radius: 5px;
  padding: 6px 20px;
  height: 40px;
  border: none;
  background: #f18436;
  color: #fff;
  box-sizing: 0;
}

.btn-button:hover {
  background: #ec690c;
  transition: 0.5s;
}
.submit-form {
  font-weight: 555;
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
.submit-form:hover {
  background-color: #3e8e41;
}
.submit-form:active {
  background-color: #3e8e41;
  box-shadow: 0 5px #666;
  transform: translateY(4px);
}

.searchbox-content {
  display: flex;
  gap: 5px;
  margin-bottom: 1px;
}
.searchbox-timerange {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px;
}
.datepicker-wrapper {
  flex: 1;
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
  grid-template-rows: 50px repeat(4, 35px) auto 5px repeat(4, 35px) auto repeat(
      7,
      35px
    );
  grid-template-columns: 1fr 1fr 1fr;
  align-content: space-around;
  box-sizing: border-box;
  background: #e6e6e2;
  padding: 0 20px 0px 20px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  position: relative;
  font-size: 16px;
  border-radius: 5px;
  overflow-x: auto;
  width: 85%;
  row-gap: 5px;
  /* height: 600px; */
}
.text-input-over {
  width: 60%;
  padding: 5px;
  border: none;
  box-sizing: border-box;
  border: none;
  resize: vertical;
  background-color: #e6e6e2;
}
.text-input {
  width: 50%;
  padding: 5px;
  border: none;
  box-sizing: border-box;
  border: none;
  resize: vertical;
  background-color: #e6e6e2;
}
label {
  font-size: 16px;
  font-weight: 555;
  color: #141414;
  margin: 0;
  margin-left: 15px;
}
p {
  color: #141414;
  margin: 0;
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
  margin: 0;
}
/* submit-form */
.button-areas {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 45px;
}
/* .submit-btn .reject-btn {
     text-align: center;
   } */
.submit-btn input {
  margin-right: 5px;
}
#submit-btn {
  font-weight: 555;
  margin-top: 5px;
  padding: 8px 11px;
  font-size: 17px;
  width: 80px;
  text-align: center;
  cursor: pointer;
  outline: none;
  color: #fff;
  background-color: #04aa6d;
  border: none;
  border-radius: 14px;
  box-shadow: 0 5px #999;
}
#submit-btn:hover {
  background-color: #3e8e41;
}
#submit-btn:active {
  background-color: #3e8e41;
  box-shadow: 0 5px #666;
  transform: translateY(4px);
}
/* reject -button */
#reject-btn {
  font-weight: 555;
  margin-top: 5px;
  padding: 8px 11px;
  font-size: 17px;
  width: 80px;
  text-align: center;
  cursor: pointer;
  outline: none;
  color: #fff;
  background-color: #e2651d;
  border: none;
  border-radius: 14px;
  box-shadow: 0 5px #999;
}
#reject-btn:hover {
  background-color: #8e3e3e;
}
#reject-btn:active {
  background-color: #8e3e3e;
  box-shadow: 0 5px #666;
  transform: translateY(4px);
}

input#clear-form {
  background-color: #f8c323;
  color: #000;
  padding: 8px 12px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
input#clear-form:hover {
  background-color: #e0a800;
}
input#return-form {
  background-color: #f77225;
  color: #000;
  padding: 8px 12px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
input#return-form:hover {
  background-color: #f76613;
}
.form-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.actual-ship-from {
  display: grid;
  grid-template-rows: repeat(5, 35px);
  grid-template-columns: 1fr 1fr 1fr;
  grid-column-start: 1;
  grid-column-end: 4;
  border-radius: 5px;
  position: relative;
  align-content: space-around;
  margin-top: 10px;
}
.form-row-actual {
  display: flex;
  justify-content: space-between;
}
.form-row-actual label {
  display: flex;
  align-items: center;
  margin: 0;
  margin-left: 15px;
}
.title-actual-ship {
  grid-row-start: 1;
  grid-row-end: 2;
  grid-column-start: 1;
  grid-column-end: 4;
  background-color: #e6e6e2;
  font-size: 16px;
  /* font-style: italic; */
  color: #565353;
  font-weight: 555;
  padding: 0 5px;
}
.form-row-actual-all {
  display: grid;
  grid-template-rows: 40px 40px 40px;
  grid-template-columns: 1fr 1fr 1fr 1fr 1fr 1fr;
  grid-row-start: 3;
  grid-row-end: 6;
  grid-column-start: 1;
  grid-column-end: 4;
}

.form-row-actual-all-label {
  grid-column-start: 1;
  grid-column-end: 2;
  grid-row-start: 1;
  grid-row-end: 4;
  display: flex;
  justify-content: flex-start;
  align-items: center;
}

.form-row-actual-all-input {
  grid-column-start: 2;
  grid-column-end: 6;
  display: flex;
  justify-content: flex-start;
  align-items: center;
}
.form-row-actual-all-input input {
  width: 80%;
  border: none;
  margin-bottom: 2px;
  border-radius: 5px;
}

.form-row-table {
  display: grid;
  grid-template-rows: auto;
  grid-column-start: 1;
  grid-column-end: 4;
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
.table-form {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 5px;
}

.table-form th,
td {
  border: 1px solid #0f0f0f;
  padding: 5px;
  /* text-align: center; */
  color: #424040;
}

.table-form th {
  color: #424040;
  font-weight: bold;
  background-color: #bbbb9c;
}

.td-show {
  text-align: center;
  justify-content: center;
  background: #f88838;
  color: #fff;
  height: 30px;
  align-items: center;
  align-content: center;
  cursor: pointer;
  margin: 10px;
}
.td-show:hover {
  background: #db6008;
}
.div-searchbox {
  margin-bottom: 5px;
  margin-top: 15px;
  height: 40px;
  display: flex;
  align-content: center;
  justify-content: left;
}
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
}
.div-searchbox-content button:hover {
  background: #f88838;
}
.div-config-name {
  margin-left: 20px;
  line-height: 50px;
}
.div-config-name span {
  font-weight: 555;
  font-size: 17px;
}
.main-contain {
  max-height: 100vh;
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
}
.div-back:hover {
  background: #b7b7b7;
}
.back-icon {
  height: 20px;
  width: 20px;
}

.mytable {
  margin-top: 0px;
  overflow: auto;
}
.mytable th:first {
  border-radius: 20%;
}
.mytable th {
  background-color: #024873;
  position: sticky;
  top: 0;
  z-index: 2;
  color: #fff;
  min-width: 60px;
  padding: 3px;
  font-size: 16px;
  padding: 0.5rem 0.5rem;
}

.mytable tr:hover {
  background: #89cfed;
}
.mytable td {
  overflow-x: auto;
  white-space: nowrap;
  z-index: 1;
  padding: 2px;
  min-width: 60px;
  border: 0.5px solid #cdc;
  font-size: 17px;
  color: #000;
}
</style>
