<template>
    <div class="div-content">
      <div class="div-top">{{
                      $store.state.language == "En"
                        ? "All applications"
                        : "Kho ứng dụng"
                    }}</div>
      <div class="div-app col-md-12 row">
        <template v-if="CheckPrivilege == 'INSERT'">
          <template v-for="(item, index) in dataInsert" :key="index">
            <div class="icon-app col-sm-2" :title="item.Name">
              <div class="icon-content" :style="item.Style" @click="GotoRoute(item.Route)">
                <img :src="item.Image" class="image-app" alt="" />
              </div>
              <span class="text-config-name">{{ item.Name }}</span>
            </div>
          </template>
        </template>
        <template v-if="CheckPrivilege == 'QUERY'">
        <template v-for="(item, index) in dataQuery" :key="index">
          <div class="icon-app col-sm-2" :title="item.Name">
            <div class="icon-content" :style="item.Style" @click="GotoRoute(item.Route)">
              <img :src="item.Image" class="image-app" alt="" />
            </div>
            <span class="text-config-name">{{ item.Name }}</span>
          </div>
        </template>
        </template>
      </div>
    </div>
  </template>
  
  <script>
  import QualcommApplications from "../data/qualcomm_applications";
  import Repository from "../services/Repository";
  export default {
    data() {
      return {
        dataInsert: [],
        dataQuery: [],
        CheckPrivilege: "QUERY",
      };
    },
    mounted() { 
      this.CheckEmpClass();
    },
    methods: {
      GotoRoute(route) {
        this.$router.push({ path: route });
      },
      async CheckEmpClass(){
        let databaseName = localStorage.databaseName;
        let emp_no = localStorage.username;
        try{
          let {data } = await Repository.getApiServer(`GetPrivilegeShowIcon?database_name=${databaseName}&emp_no=${emp_no}`);
          if (data.result == "ok" && data.data.length > 0) {
            this.CheckPrivilege = "INSERT";
            this.dataInsert = QualcommApplications[1].Applications;
          }else {
            this.dataQuery = QualcommApplications[0].Applications;
          }
        }catch(error) {
          if(error.response && error.response.data) {
            this.$swal("", error.response.data.error, "error");
            }else {
            this.$swal ("", error.Message, "error")
          }
        }
      }
    }
  }
  </script>
  
  <style scoped lang="scss">
  @media only screen and (hover: none) and (pointer: coarse) {
    .text-config-name {
      font-size: 12px !important;
    }
  }
  .text-config-name { 
    text-align: center;  
    align-content: center;
    font-size: 14px;
    font-weight: 666;
    display: flex;
    user-select: none;
    justify-content: center;
    color: #1b2b38;
    margin-top: 4px;
    margin-bottom: 10px;
  }
  
  .div-content {
    min-height: 95vh;
    background: url("../../public/assets/img/background.jpg");
    background-size: 100% auto;
  }
  .div-app {
    padding-top: 20px;
  }
  .image-app {
    height: 100%;
    width: 100%;
    border-radius: 15px;
  }
  .div-top {
    font-size: 1.1rem;
    color: rgb(43, 42, 42);
  }
  .div-main {
    padding-left: 20px;
  }
  .config-content {
    margin-top: 30px;
    height: 90vh;
    overflow: auto;
  }
  .icon-app-disable {
    height: 50px;
    width: 50px;
    .icon-content {
      background: #dddddd !important;
      border: 5px solid #737677;
      color: #737677 !important;
      &:hover {
        border: 5px solid #737677;
        cursor: default;
      }
      span {
        color: #737677;
        margin: auto;
        text-align: center;
        font-size: 20px;
        font-weight: 555;
        justify-content: center;
      }
    }
    .text-config-name-disable {
      color: #737677 !important;
    }
  }
  .icon-app {
    height: 70%;
    width: 70%;
  }
  .icon-content {
    user-select: none;
    background: #1b2b38;
    display: flex;
    justify-content: center;
    margin: auto;
    width: 60%;
    height: 60%;
    border-radius: 20%;
    border: 3px solid #0ac9ff;
    span {
      color: #0ac9ff;
      margin: auto;
      text-align: center;
      font-size: 20px;
      font-weight: 555;
      justify-content: center;
    }
    &:hover {
      border: 5px solid #0ac9ff;
      cursor: pointer;
      span {
        font-size: 25px;
      }
    }
  }
  </style>