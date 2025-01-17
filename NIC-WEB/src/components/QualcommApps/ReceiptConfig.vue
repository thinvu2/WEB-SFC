<template>
  <div class="receipt-config">
    <header class="row header">
      <div class="div-back" @click="backToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="row div-config-name">
        <span>Receipt Config</span>
      </div>
    </header>
    <h1>Receipt Config</h1>

    <div class="container">
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
</template>
<script>
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      isDisabled: true,
      dataTable: [],
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
    this.loadComponent();
  },
  computed: {},
  methods: {
    async loadComponent() {
      const databaseName = this.databaseName;
      const IN_FUNC = "QRECEIPT";
      const IN_SUBFUNC = "SHOWDATA";
      const IN_DATA = null;
      const empNo = this.empNo;
      try {
        const { data } = await Repository.getApiServer(
          `QDataConfig?databaseName=${databaseName}&IN_FUNC=${IN_FUNC}&IN_SUBFUNC=${IN_SUBFUNC}&IN_DATA=${IN_DATA}&IN_EMPNO=${empNo}`
        );
        this.dataTable = data.data;
        if (this.dataTable.length > 0) {
          this.objData = this.dataTable[0];
          this.dataTableHeader = Object.keys(this.dataTable[0]);
        }

      } catch (error) {
        console.error("LoadForm Error:", error);
        const message =
        error.response?.data?.message ||
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
        const IN_FUNC = "QRECEIPT";
        const IN_SUBFUNC = "INSERTDATA";
        const IN_DATA = JSON.stringify(this.objData);
        const empNo = this.empNo;
        try {
          const { data } = await Repository.getApiServer(
          `QDataConfig?databaseName=${databaseName}&IN_FUNC=${IN_FUNC}&IN_SUBFUNC=${IN_SUBFUNC}&IN_DATA=${IN_DATA}&IN_EMPNO=${empNo}`
        );
          if (data.result == "ok") {
            this.isDisabled = !this.isDisabled;
           await this.loadComponent();
            this.$swal("", "Successfully applied", "success");
          } else {
            this.$swal("", data.data.substr(0, 100), "error");
          }
        } catch (error) {
          const message =
          error.response?.data?.message ||
            error.response?.data?.error ||
            error.message ||
            "An unexpected error occurred.";
          this.$swal("", message, "error");
        }
      });
    },
   toggleEdit() {
    this.isDisabled = !this.isDisabled;
  },
    backToParent() {
      this.$router.push({ path: "/Home/QualcommApps" });
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

.footer {
  text-align: center;
  margin-top: 10px;
}
</style>
