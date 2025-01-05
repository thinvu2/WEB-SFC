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
      <!-- Left Section: Above -->
      <div class="section-left">
        <div class="block">
          <label for="msg-sender-name">Message sender name:</label>
          <input type="text" id="msg-sender-name" placeholder="Msg sender name" autocomplete="off" />

          <label for="msg-sender-duns">Message sender Duns:</label>
          <input type="text" id="msg-sender-duns" placeholder="Msg sender duns" autocomplete="off" />

          <label for="msg-receiver-name">Message receiver name:</label>
          <input type="text" id="msg-receiver-name" placeholder="Msg receiver name" autocomplete="off" />

          <label for="msg-receiver-duns">Message receiver Duns:</label>
          <input type="text" id="msg-receiver-duns" placeholder="Msg receiver duns" autocomplete="off" />

          <label for="receiver-name">Receiver name:</label>
          <input type="text" id="receiver-name" placeholder="Receiver name" autocomplete="off" />

          <label for="location-name">Actual receiving location name:</label>
          <input type="text" id="location-name" placeholder="Location name" autocomplete="off" />
        </div>
      </div>
      <!-- Right Section -->
      <div class="section-right">
        <label for="address-line1">Actual receiving address line 1:</label>
        <input type="text" id="address-line1" placeholder="Address line 1" autocomplete="off" />

        <label for="address-line2">Actual receiving address line 2:</label>
        <input type="text" id="address-line2" placeholder="Address line 2" autocomplete="off" />

        <label for="address-line3">Actual receiving address line 3:</label>
        <input type="text" id="address-line3" placeholder="Address line 3" autocomplete="off" />

        <label for="city-name">Actual receiving city name:</label>
        <input type="text" id="city-name" placeholder="City name" autocomplete="off" />

        <label for="country-code">Actual receiving country code:</label>
        <input type="text" id="country-code" placeholder="Country code" autocomplete="off" />

        <label for="postal-code">Actual receiving postal code:</label>
        <input type="text" id="postal-code" placeholder="Postal code" autocomplete="off" />
      </div>
    </div>

    <div class="footer">
      <input type="submit" value="Submit" id="input-submit" />
    </div>
  </div>
</template>
<script>
import Repository from "../../services/Repository";
export default {
  data() {
    return {
      model: {
        database_name: localStorage.databaseName,
        empNo: localStorage.username,
        supplierName: '',
        supplierDuns: '',
        supplierDuns4: '',
        receiverName: '',
        customerName: '',
        customerDuns: '',
        customerDuns4: '',
        ssmVersion: '',
        ssmItem: ''
      }
    }
  },
  created() {
    window.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.isVisible = false;
      }
    });
  },

//   mounted() {
//     this.LoadComponent();
//   },
  computed: {},
  methods: {
    async SubmitForm() {
      if (!this.model.SCHED_QTY) {
        this.$swal("", "Please Schedule Qty!", "warning");
        return;
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

        let payload = {
            databaseName: this.database_name,
            empNo: this.empNo,
            supplierName: this.supplierName,
            supplierDuns: this.supplierDuns,
            supplierDuns4: this.supplierDuns4,
            receiverName: this.receiverName,
            customerName: this.customerName,
            customerDuns: this.customerDuns,
            customerDuns4: this.customerDuns4,
            ssmVersion: this.ssmVersion,
            ssmItem: this.ssmItem
        };
        try {
          let { data } = await Repository.getRepo("InsertTelitEDI", payload);
          if (data.result == "ok") {
            this.ClearForm();
            this.$swal("", "Successfully applied", "success");
          } else {
            this.$swal("", data.result, "error");
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
    clearForm() {
        this.supplierName = '',
        this.supplierDuns = '',
        this.supplierDuns4 = '',
        this.receiverName = '',
        this.customerName = '',
        this.customerDuns = '',
        this.customerDuns4 = '',
        this.ssmVersion = '',
        this.ssmItem = ''
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
  line-height: 45px;
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
  display: flex;
  justify-content: space-between;
  gap: 20px;
}

.section-left {
  flex: 1;
  background-color: #f9f9f9;
  padding: 0 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
}
.section-right {
  flex: 1;
  background-color: #f9f9f9;
  padding: 0 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  height: auto;
}

.block {
  margin-bottom: 10px;
}

h2 {
  color: #0056b3;
  margin-bottom: 10px;
  font-size: 1rem;
  font-weight: bold;
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
  width: 100px;
  height: 50px;
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
