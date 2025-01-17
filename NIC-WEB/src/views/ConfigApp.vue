<template>
  <div class="">
    <div class="row config-content">
      <template v-for="(item, index) in dataList" :key="index">
        <div class="icon-app-disable col-sm-2" v-if="item.ISDISABLE == 2">
          <div class="icon-content">
            <span>{{ item.ID }} </span>
          </div>
          <span class="text-config-name">{{ item.NAME }}</span>
        </div>
        <div v-else class="icon-app col-sm-2" @click="GotoRoute(item.ROUTE)">
          <template v-if="item.ROUTE != null">
            <div class="icon-content">
              <span> {{ item.ID }}. {{ item.NAME }}</span>
            </div>
          </template>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import Repository from "../services/Repository";
export default {
  data() {
    return {
      dataList: [],
      dataList1: [],
    };
  },
  created() {
    document.title = "Config Application";
  },
  mounted() {
    this.CheckAllPrivilege();
  },
  methods: {
    GotoRoute(route) {
      this.$router.push({ path: route });
    },
    async CheckAllPrivilege() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
      };
      var { data } = await Repository.getRepo(
        "GetConfigPrivilge_Group",
        payload
      );
      this.dataList = [];
      this.dataList = data.data;
    },

    async GetDatalist() {
      var payload = {
        database_name: localStorage.databaseName,
        emp_no: localStorage.username,
      };
      var { data } = await Repository.getRepo(
        "GetConfigPrivilge_Group",
        payload
      );
      this.dataList = [];
      this.dataList = data.data;
    },
  },
};
</script>

<style lang="scss" scoped>
.div-main {
  padding-left: 20px;
  padding: 5px;
}
.config-content {
  display: grid;
  height: 90vh;
  background: #9fc6f2;
  grid-template-columns: 25% 25% 25% 25%;
  grid-template-rows: 10% 10% 10% 10%;
  grid-column-gap: 20px;
  grid-row-gap: 10px;
}
.icon-app-disable {
  width: 100px;
  .icon-content {
    background: #dddddd !important;
    color: #737677 !important;
    &:hover {
      border: 5px solid #737677;
      cursor: default;
    }
    span {
      color: #737677;
      margin: auto;
      text-align: left;
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
  margin-block: 10px;
  width: 100px;
}
.icon-content {
  user-select: none;
  text-align: center;
  background: transparent;
  box-shadow: 10px 5px 5px rgb(63, 239, 245);
  justify-content: left;
  margin-bottom: 20px;
  width: 185px;
  height: 40px;
  border-radius: 10%;
  border: 2px solid #5c5ff7;

  span {
    color: #020b0c;
    margin-top: 10px;
    margin: auto;
    justify-content: center;
    text-align: left;
    font-size: 15px;
    font-weight: 555;
    justify-content: left;
  }
  &:hover {
    cursor: pointer;
    background: rgb(63, 239, 245);
  }
}
.text-config-name {
  font-family: "Roboto", sans-serif;
  text-align: left;
  font-size: 12px;
  font-weight: 666;
  display: flex;
  user-select: none;
  justify-content: center;
  color: #1b2b38;
}
.col-sm-2 {
  width: 150px;
  height: 50px;
  margin-bottom: 20px;
}
</style>
