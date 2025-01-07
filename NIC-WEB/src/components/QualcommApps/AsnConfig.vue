<template>
  <div class="receipt-config">
    <header class="row header">
      <div class="div-back" @click="isShowForm ? returnForm() : backToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="row div-config-name">
        <span>ASN Config</span>
      </div>
    </header>
    <h1>ASN Config</h1>

    <div class="container" v-if="isShowForm === true">
      <div
        class="form-row-input"
        v-for="(item, index) in dataTableHeader"
        :key="index"
      >
        <label :for="`input-${item}`">{{ item }}:</label>

        <input
          type="text"
          :id="`input-${item}`"
          autocomplete="off"
          :disabled="isDisabled"
          v-model="objData[item]"
        />
      </div>
    </div>
    <!-- Start table -->
    <div class="main-contain" v-if="isShowForm === false">
          <table class="mytable">
            <thead>
              <tr>
                <template v-for="(item, index) in dataTableHeaderList" :key="index">
                  <th>
                    {{ item }}
                  </th>
                </template>
              </tr>
            </thead>
            <tbody>
              <template v-for="(item, index) in dataTableList" :key="index">
                <tr>
                  <template v-for="(item1, index1) in item" :key="index1">
                    <td
                      @click="index1 == 'PO_NO' && loadComponent(item1)">
                      {{ item1 }}
                    </td>
                  </template>
                </tr>
              </template>
            </tbody>
          </table>
    </div>
    <!-- end table -->
     <div v-if="isShowForm === true">
      <div class="footer" v-if="isDisabled">
    <input
      type="submit"
      @click="toggleEdit"
      value="Edit"
      id="edit-button"
    />
  </div>
  <div class="footer" v-else>
    <input
      type="submit"
      @click="submitForm"
      value="Save"
      id="edit-button"
    />
  </div>
     </div>

  </div>
</template>
<script>
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      isShowForm: false,
      isDisabled: true,
      dataTable: [],
      dataTableList: [],
      dataTableHeaderList: [],
      objData: {},
      dataTableHeader: [],
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

  mounted() {
    this.loadDataTable();
  },
  computed: {},
  methods: {
    async loadDataTable() {
      const databaseName = this.databaseName;
      const IN_FUNC = "QASN_OUT";
      const IN_SUBFUNC = "SHOWLIST";
      const IN_DATA = null;
      const empNo = this.empNo;
      try {
        const { data } = await Repository.getApiServer(
          `QDataConfig?databaseName=${databaseName}&IN_FUNC=${IN_FUNC}&IN_SUBFUNC=${IN_SUBFUNC}&IN_DATA=${IN_DATA}&IN_EMPNO=${empNo}`
        );
        console.log(data.data);
        this.dataTableList = data.data;
        if (this.dataTableList.length > 0) {
          this.objData = this.dataTableList[0];
          this.dataTableHeaderList = Object.keys(this.dataTableList[0]);
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
    
    async loadComponent(index) {
      this.isShowForm = true
      const databaseName = this.databaseName;
      const IN_FUNC = "QASN_OUT";
      const IN_SUBFUNC = "SHOWDATA";
      const IN_DATA = index;
      const empNo = this.empNo;
      console.log("IN_DATA: ", IN_DATA)
      try {
        const { data } = await Repository.getApiServer(
          `QDataConfig?databaseName=${databaseName}&IN_FUNC=${IN_FUNC}&IN_SUBFUNC=${IN_SUBFUNC}&IN_DATA=${IN_DATA}&IN_EMPNO=${empNo}`
        );
        console.log(data.data);
        this.dataTable = data.data;
        if (this.dataTable.length > 0) {
          this.objData = this.dataTable[0];
          this.dataTableHeader = Object.keys(this.dataTable[0]);
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
    async submitForm() {
      for(let item of this.dataTableHeader) {
        if(!this.objData[item]) {
          this.$swal("", `Please input data: ${item}`, "warning");
        return;
        }
      }
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

        const databaseName = this.databaseName;
        const IN_FUNC = "QASN_OUT";
        const IN_SUBFUNC = "INSERTDATA";
        const IN_DATA = JSON.stringify(this.objData);
        const empNo = this.empNo;

        console.log(IN_DATA)

        try {
          const { data } = await Repository.getApiServer(
          `QDataConfig?databaseName=${databaseName}&IN_FUNC=${IN_FUNC}&IN_SUBFUNC=${IN_SUBFUNC}&IN_DATA=${IN_DATA}&IN_EMPNO=${empNo}`
        );

        console.log(data.data);
          if (data.result == "ok") {
            this.isDisabled = !this.isDisabled;
            this.$swal("", "Successfully applied", "success");
          } else {
            this.$swal("", data.data, "error");
          }
        } catch (error) {
          const message =
            error.response?.data?.error ||
            error.message ||
            "An unexpected error occurred.";
          this.$swal("", message, "error");
        }
      });
    },
   async toggleEdit() {
    this.isDisabled = !this.isDisabled;
  },
  returnForm() {
      this.isShowForm = false;
      this.loadDataTable();
    },
    backToParent() {
      this.$router.push({ path: "/Home/Qualcomm_Application" });
    },
  },
};
</script>
<style scoped lang="scss">
.receipt-config {
  font-family: Arial, sans-serif;
  margin: 0 20px;
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
  color: #333;
  margin: 0;
}

.container {
  display: grid;
  width: 80%;
  gap: 10px;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
}


label {
  display: block;
  margin: 10px 0 5px;
  color: #555;
  font-size: 1rem;
  font-weight: 555;
}

input[type="text"] {
  width: 100%;
  padding: 7px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

input[type="submit"] {
  font-size: 16px;
    font-weight: bold;
    width: 90px;
    height: 45px;
  padding: 7px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
  background-color: #0056b3;
  color: white;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

input[type="submit"]:hover {
  background-color: #003d82;
}

.main-contain {
  max-height: 100vh;
  overflow: auto;
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
    //min-width: 60px;
    white-space: nowrap;
    padding: 3px;
    font-size: 16px;
    padding: 0.5rem 0.5rem;
  }

  .mytable tr:hover {
    background: #89cfed;
  }
  .mytable td:nth-child(1) {
    cursor: pointer;
    text-decoration: underline;
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

.footer {
  text-align: center;
  margin-top: 10px;
}
</style>
