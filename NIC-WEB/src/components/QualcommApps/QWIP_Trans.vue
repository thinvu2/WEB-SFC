<template>
  <div class="wip-inventory">
    <div class="row">
      <div class="div-back" @click="isShowForm ? ReturnForm() : BackToParent()">
        <Icon icon="chevron-left" class="back-icon sidenav-icon" />
      </div>
      <div class="div-config-name row">
        <span>Advance Shipment Notice</span>
      </div>
    </div>
    <div class="content">Inventory_Transaction :</div>
    <div class="wip-report_inv" :style="gridStyles">
      <div
        class="grid-item_inv"
        @click="gotoRoute(item.Route)"
        v-for="(item, index) in items_INV"
        :key="index"
        :style="item.Style"
      >
        <span class="btnGrid">{{ item.Name }}</span>
        <!-- <span class="btnGrid">{{ item.Id + ". " + item.Name }}</span> -->
      </div>
    </div>
    <div class="content">Wip_Transaction :</div>
    <div class="wip-report" :style="gridStyles">
      <div
        class="grid-item"
        @click="gotoRoute(item.Route)"
        v-for="(item, index) in items"
        :key="index"
        :style="item.Style"
      >
        <span class="btnGrid">{{ item.Name }}</span>
        <!-- <span class="btnGrid">{{ item.Id + ". " +  item.Name }}</span> -->
      </div>
    </div>
  </div>
</template>


<script>
import dataWIP from "../../data/data_WipTransaction.js";
import dataINV from "../../data/data_WipInventory.js";
export default {
  data() {
    return {
      items: dataWIP,
      items_INV: dataINV,
      columns: 5,
    };
  },
  computed: {
    gridStyles() {
      return {
        gridTemplateColumns: `repeat(${this.columns}, 1fr)`,
      };
    },
  },
  methods: {
    gotoRoute(route) {
      this.$router.push({ path: route });
    },
  },
};
</script>

<style lang="scss" scoped>
.content {
  font-size: 25px;
  font-weight: bolder;
  color: red;
  margin-left: 1%;
  font-family: "Gill Sans", "Gill Sans MT", Calibri, "Trebuchet MS", sans-serif;
}
.wip-inventory {
  background: linear-gradient(
      rgba(255, 255, 255, 0.5),
      rgba(173, 204, 255, 0.7)
    ),
    url(../../assets/img/config_bg.jpg);
  background-size: cover;
  background-position: center;
  color: #054a5c;
}
.wip-report {
  background-size: cover;
  background-position: center;
  height: 45vh;
  color: #054a5c;
  border-radius: 22px;
  display: grid;
  gap: 10px;
  margin-left: 1%;
  overflow: auto;
}
.grid-item {
  height: 17vh;
  padding: 20px;
  padding-top: 20%;
  margin: 10px 5px 10px 5px;
  border-radius: 12px;
  border: 1px solid #054a5c;
  box-shadow: rgb(140, 241, 250) 3px 2px 3px;
  text-align: center;
  overflow: hidden;
  position: relative;
  cursor: pointer;
  text-decoration: none;
  text-transform: uppercase;
  font-weight: bold;
  font-family: "Gill Sans", "Gill Sans MT", Calibri, "Trebuchet MS", sans-serif;
  &:hover {
    color: white;
    animation: drawColor 15s infinite linear;
    background: linear-gradient(45deg, #03404a, #8ab3e9);
    box-shadow: rgb(28, 180, 231) 3px 2px 3px;
    @keyframes drawColor {
      0% {
        background-position: 100% 0;
      }
      100% {
        background-position: 0 0;
      }
    }
  }

  .btnGrid {
    font-size: 17px;
  }
}
.wip-report_inv {
  background-size: cover;
  background-position: center;
  height: 20vh;
  color: #054a5c;
  border-radius: 22px;
  display: grid;
  gap: 10px;
  margin-left: 1%;
}
.grid-item_inv {
  padding: 20px;
  padding-top: 20%;
  margin: 10px 5px 10px 5px;
  border-radius: 12px;
  border: 1px solid #054a5c;
  box-shadow: rgb(140, 241, 250) 3px 2px 3px;
  text-align: center;
  overflow: hidden;
  position: relative;
  cursor: pointer;
  text-decoration: none;
  text-transform: uppercase;
  font-weight: bold;
  font-family: "Gill Sans", "Gill Sans MT", Calibri, "Trebuchet MS", sans-serif;
  &:hover {
    color: white;
    animation: drawColor 15s infinite linear;
    background: linear-gradient(45deg, #03404a, #8ab3e9);
    box-shadow: rgb(28, 180, 231) 3px 2px 3px;
    @keyframes drawColor {
      0% {
        background-position: 100% 0;
      }
      100% {
        background-position: 0 0;
      }
    }
  }

  .btnGrid {
    font-size: 17px;
  }
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
</style>