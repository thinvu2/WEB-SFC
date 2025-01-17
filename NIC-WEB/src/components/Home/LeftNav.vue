<template>
  <div class="col-md-3 left_col">
    <div class="left_col scroll-view">
      <div class="navbar nav_title" style="border: 0">
        <a href="/" class="site_title"
          ><i class="fa fa-paw"></i> <span>Shopfloor System</span></a
        >
      </div>
      <div class="clearfix"></div>
      <div class="profile clearfix">
        <div class="profile_pic">
          <img
            src="assets/img/avatars/1.png"
            alt="..."
            class="img-circle profile_img"
          />
        </div>
        <div class="profile_info">
          <span>{{
            $store.state.language == "En" ? "Welcome," : "Xin chào,"
          }}</span>
          <h2>{{ empname }}</h2>
        </div>
      </div>
      <br />
      <div id="sidebar-menu" class="main_menu_side hidden-print main_menu">
        <div class="menu_section">
          <h3>{{ $store.state.language == "En" ? "General" : "Chung" }}</h3>
          <ul class="nav side-menu">
            <li>
              <router-link to="/Home">
                <i class="fa fa-home"></i>Home
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li>

             <!-- <li>
              <router-link to="/Home/Applications">
                <i class="fa fa-desktop"></i>
                {{
                  $store.state.language == "En" ? "Applications" : "?ng d?ng"
                }}
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li> -->

            <li v-if="dataShowIcon.includes('NIC')">
              <router-link to="/Home/Applications">
                <i class="fa fa-desktop"></i>
                {{
                  $store.state.language == "En" ? "Applications" : "?ng d?ng"
                }}
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li>
            <li v-if="dataShowIcon.includes('QUALCOMM')">
              <router-link to="/QualcommApps">
                <i class="fa fa-space-shuttle"></i>Qualcomm
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li>

            <li v-if="dataShowIcon.includes('TELIT')">
              <router-link to="/Home/Telit_Apps">
                <i class="fa fa-space-shuttle"></i>Telit
                <span class="fa fa-chevron-right"></span
              ></router-link>
            </li>

          </ul>
        </div>
      </div>
      <!-- <div class="sidebar-footer hidden-small">
        <a
          data-toggle="tooltip"
          data-placement="top"
          title="Logout"
          @click="logout"
        >
          <i class="fa fa-sign-out"></i>
        </a>
      </div> -->
    </div>
  </div>
</template>
<script>
import data from "../../data/menu_data";
import Repository from "../../services/Repository";
export default {
  name: "LeftNav",
  // state: {
  //   dataShowIcon: '',
  // },
  // mutations: {
  //   setDataShowIcon(state, privileges) {
  //     state.dataShowIcon = privileges;
  //   },
  // },
  // actions: {
  //   async fetchPrivileges({ commit }, { databaseName, empNo }) {
  //     try {
  //       const { data } = await Repository.getApiServer(
  //         `GetPrivilegeLeftNav?databaseName=${databaseName}&empNo=${empNo}`
  //       );
  //       commit("setDataShowIcon", data.data[0].OWNER || []);
  //     } catch (error) {
  //       console.error("getPrivilege Error:", error);
  //       const message =
  //         error.response?.data?.error ||
  //         error.message ||
  //         "An unexpected error occurred.";
  //       this.$swal("", message, "error");
  //     }
  //   },
  // },

  data() {
    return {
      dataShowIcon: '',
      databaseName: localStorage.databaseName,
      empNo: localStorage.username,
      listNav: [],
      DataTable: [],
      arrData: [],
      index: 0,
      index1: 0,
      index2: 9999,
      empname: "",
    };
  },
  mounted() {
    this.getPrivilege();
    this.listNav = data;
  },
  created() {
    for (let i = 0; i < this.listNav.length; i++) {
      for (let j = 0; j < this.listNav[i].NavList.length; j++) {
        if (
          this.listNav[i].NavList[j].route == this.$route.path &&
          this.$route.path != "/"
        ) {
          this.listNav[i].NavList[j].selected = true;
          this.index = i;
          this.index1 = j;
          this.index2 = 9999;
        } else {
          this.listNav[i].NavList[j].selected = false;
        }
      }
    }
    this.empname = localStorage.empname;
    this.databaseName = localStorage.databaseName;
  },
  methods: {
    async getPrivilege() {
      let databaseName = this.databaseName;
      let empNo = this.empNo;
      try {
        let { data } = await Repository.getApiServer(
          `GetPrivilegeLeftNav?databaseName=${databaseName}&empNo=${empNo}`
        );
        this.DataTable = data.data;
        this.dataShowIcon = this.DataTable[0].OWNER;
      } catch (error) {
        console.error("getPrivilege Error:", error);
        const message =
          error.response?.data?.error ||
          error.message ||
          "An unexpected error occurred.";
        this.$swal("", message, "error");
      }
    },

    MaximizeWindow() {
      var docElm = document.documentElement;
      if (docElm.requestFullscreen) {
        docElm.requestFullscreen();
      } else if (docElm.mozRequestFullScreen) {
        docElm.mozRequestFullScreen();
      } else if (docElm.webkitRequestFullScreen) {
        docElm.webkitRequestFullScreen();
      } else if (docElm.msRequestFullscreen) {
        docElm.msRequestFullscreen();
      }
      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.webkitCancelFullScreen) {
        document.webkitCancelFullScreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      }
    },
    ChangeLanguage(lang) {
      this.$store.commit("changeLanguage", lang);
      localStorage.language = lang;
    },
    logout() {
      localStorage.username = "";
      localStorage.password = "";
      this.$router.push({ name: "Login" });
    },
    changeSelect(index, index1) {
      if (this.index2 != 9999) {
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = false;
        this.listNav[index].NavList[index1].selected = true;
      } else {
        for (let i = 0; i < this.listNav.length; i++) {
          for (let j = 0; j < this.listNav[i].NavList.length; j++) {
            this.listNav[i].NavList[j].selected = false;
          }
        }
        this.listNav[index].NavList[index1].selected = true;
      }
      this.index = index;
      this.index1 = index1;
      this.index2 = 9999;
    },
    removeAllSelect() {
      for (let i = 0; i < this.listNav.length; i++) {
        for (let j = 0; j < this.listNav[i].NavList.length; j++) {
          this.listNav[i].NavList[j].selected = false;
        }
      }
    },
    changeSubSelect(index, index1, index2) {
      if (this.index2 == 9999) {
        this.listNav[this.index].NavList[this.index1].selected = false;
        this.index = index;
        this.index1 = index1;
        this.index2 = index2;
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = true;
      } else {
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = false;
        this.index = index;
        this.index1 = index1;
        this.index2 = index2;
        this.listNav[this.index].NavList[this.index1].NavList[
          this.index2
        ].selected = true;
      }
    },
  },
};
</script>
<style scoped lang="scss">
.left_col {
  background: #0a5381 !important;
  .nav_title {
    background: #0a5381 !important;
  }
  .sidebar-footer {
    background: #0a5381 !important;
  }
  z-index: 0;
}
.side-menu li {
  font-size: 18px;
  &:hover {
    background-color: #073e61;
  }
}
</style>
