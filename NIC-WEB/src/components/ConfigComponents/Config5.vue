<template>
    <div class="div-all">
        <header>
            <div class="row">
                <div class="div-back" @click="BackToParent()">
                <Icon icon="chevron-left" class="back-icon sidenav-icon" />
                </div>
                <div class="div-config-name row">
                <span>Config (5):</span>
                </div>
            </div>
            <div class="div-searchbox row">
                <div class="div-searchbox-content">
                <input
                    v-on:keyup.enter="getRouteCode()"
                    v-model="valueSearch"
                    type="text"
                    class="form-control"
                    @click="selectAll"
                    placeholder = 'Enter route code...'/>
                <button @click="getRouteCode()" class="btn">
                    <Icon icon="search" class="sidenav-icon" />
                </button>
                </div>
            </div>
        </header>
        <section>
            <div class="main-contain">
                    <div class="mytable">
                    <template v-for="(item, index) in DataTable" :key="index">
                        <div class="table-row">
                        <template v-for="(item1, index1) in item" :key="index1">
                            <div class="table-cell">
                                <span>{{ item1 }}</span>
                            </div>      
                            <div v-if="index1 < item.length - 1" class="space-line"></div>
                        </template>
                        
                        </div>
                        
                    </template>
                    </div>
            </div>
        </section>
    </div>
  </template>
  <script>
import Repository from "../../services/Repository";
  export default {
    data() {
      return {
        isVisible: false,
        DataTable: [],
        valueSearch: "10385",
        model: {
          database_name: localStorage.databaseName,
          EMP_NO: localStorage.username,
        },
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
        DataTable: {
            immediate: true,
            handler(newValue) {
            newValue.forEach((item, index) => {
                console.log(`Item at index ${index}:`, item, Array.isArray(item), " item.length : ", item.length );
            });
            }
        }
    },
    methods: {
      BackToParent() {
        this.$router.push({ path: "/Home/ConfigApp" });
      },
      async getRouteCode() {
        let routeCode = this.valueSearch;
        let  database_name = localStorage.databaseName;
        try {
            let { data } = await Repository.getApiServer(`getRouteCode?database_name=${database_name}&routeCode=${routeCode}`);
            this.DataTable = data.data || [];
            this.DataTable = this.DataTable.map((obj) => Object.values(obj));   
        }catch (error) {
        if (error.response && error.response.data) {
          this.$swal("", error.response.data.error, "error");
        } else {
          this.$swal("", error.Message, "error");
        }
      }
      }
    }
  }
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
    padding: 15px;
    margin-right: 20px;
    div {
      div {
        label {
          font-size: 13px;
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
      display: flex;
      margin-bottom: 10px;
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
  display: flex;
  flex-direction: row;
  gap: 1rem;
}

.table-row {
  display: flex;
  gap: 0.5rem;
}

.table-cell {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 10px;
  background-color: #e0f7fa;
  border-radius: 10px;
  font-weight: bold;
  border: 1px solid #00acc1;
}

.space-line {
  width: 40px;
  height: 5px;
  background-color: blue;
  margin: 0;
  align-self: center;
}

  </style>