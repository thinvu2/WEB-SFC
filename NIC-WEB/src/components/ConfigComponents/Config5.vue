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
            placeholder="Enter route code..."
          />
          <button @click="getRouteCode()" class="btn">
            <Icon icon="search" class="sidenav-icon" />
          </button>
        </div>
      </div>
    </header>
    <section>
      <div class="container">
        <div class="mytable">
            <!-- line-main-row -->
            <tr class="line-main-row">
          <template v-for="(mainItem, index) in dataLineMain" :key="index">
            <td class="line-main-cell">
              <span>{{ mainItem[0] }}</span>
            </td>
            <div v-if="index < dataLineMain.length - 1" class="space-line"></div>
          </template>
          
        </tr>
            <tr class="line-relation">
              <template v-for="(mainItem, index) in dataLineMain" :key="index">
                  <td class="relation-return">
                  <!-- <template v-if="groupedData[mainItem[0]]"> -->
                    <!-- <canvas id="myCanvas"></canvas> -->
                    <canvas :id="'myCanvas-' + index" width="50" height="100"></canvas>
                  <!-- </template> -->
                </td>
            <div class="space"></div>
          </template>
          </tr>
            
            <tr class="line-r-row">
          <template v-for="(mainItem, index) in dataLineMain" :key="index">
            <td class="line-r-cell">
              <template v-if="groupedData[mainItem[0]]">
                <span v-for="(relation, relIndex) in groupedData[mainItem[0]]" :key="relIndex">
                  {{ relation.GROUP_NEXT }}
                </span>
                
              </template>
            </td>
            <div class="space"></div>
          </template>
        </tr>
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
      dataLineMain: [],
      dataLineR: [],
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
  dataLineMain: {
    handler() {
      this.$nextTick(() => this.drawOnCanvas());
    },
    immediate: true,
    deep: true
  }
},
  computed: {
    groupedData() {
      const map = {};
      this.dataLineR.forEach((item) => {
        if (!map[item.GROUP_NAME]) {
          map[item.GROUP_NAME] = [];
        }
        map[item.GROUP_NAME].push(item);
      });
      return map;
    },
  },
  methods: {
    drawOnCanvas() {
    this.dataLineMain.forEach((_, index) => {
      const canvas = document.getElementById(`myCanvas-${index}`);
      if (canvas) {
        const ctx = canvas.getContext('2d');
        ctx.beginPath();
        ctx.moveTo(25, 0);
        ctx.lineTo(25, 100);
        ctx.strokeStyle = '#1fc814';
        ctx.lineWidth = 3;
        ctx.stroke();
      } else {
        console.log("Canvas not found for index:", index);
      }
    });
  },
    filteredDataLineR(groupName) {
      return this.groupedDataLineR[groupName] || [];
    },
    async getRouteCode() {
      let routeCode = this.valueSearch;
      let database_name = localStorage.databaseName;
      try {
        let { data } = await Repository.getApiServer(
          `getRouteCode?database_name=${database_name}&routeCode=${routeCode}`
        );
        this.dataLineMain = (data.dataLineMain || []).map((item) =>
          Object.values(item)
        );
        this.dataLineR = data.dataLineR;
        //console.log("dataLineMain: ", this.dataLineMain);
        //console.log("dataLineR: ", this.dataLineR);
      } catch (error) {
        if (error.response && error.response.data) {
          this.$swal("", error.response.data.error, "error");
        } else {
          this.$swal("", error.Message, "error");
        }
      }
    },
    BackToParent() {
      this.$router.push({ path: "/Home/ConfigApp" });
    },
  },
//   mounted() {
//  console.log("DataLineMain:", this.dataLineMain);

//   this.$nextTick(() => {
//     this.dataLineMain.forEach((_, index) => {
//       const canvasId = `myCanvas-${index}`;
//       const canvas = document.getElementById(canvasId);
//       console.log(`Checking canvas: ${canvasId}`, canvas);

//       if (canvas) {
//         const ctx = canvas.getContext('2d');
//         ctx.beginPath();
//         ctx.moveTo(25, 0);
//         ctx.lineTo(25, 100);
//         ctx.strokeStyle = 'blue';
//         ctx.lineWidth = 3;
//         ctx.stroke();
//       } else {
//         console.log("Canvas not found for index:", index);
//       }
//     });
//   });

  // this.$nextTick(() => {
  //   const mainCells = document.querySelectorAll('.line-main-cell');
  //   const rCells = document.querySelectorAll('.line-r-cell');
  //   const canvases = document.querySelectorAll('#myCanvas');

  //   mainCells.forEach((mainCell, index) => {
  //     const rCell = rCells[index];
  //     const canvas = canvases[index];
  //     if (canvas && mainCell && rCell) {
  //       const mainRect = mainCell.getBoundingClientRect();
  //       const rRect = rCell.getBoundingClientRect();
  //       console.log("mainRect, rRect: ", mainRect, rRect);
  //       canvas.width = 50;
  //       canvas.height = rRect.top - mainRect.bottom;

  //       const ctx = canvas.getContext('2d');
  //       if (ctx) {
  //         ctx.clearRect(0, 0, canvas.width, canvas.height);
  //         ctx.beginPath();
  //         ctx.moveTo(canvas.width / 2, 0);
  //         ctx.lineTo(canvas.width / 2, canvas.height);
  //         ctx.lineWidth = 5;
  //         ctx.strokeStyle = "red";
  //         ctx.stroke();
        
  //       }
  //     }
  //   });
  // });
//},
};
</script>
<style lang="scss" scoped>
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
      width: 300px;
      height: 40px;
      font-size: 16px;
    }
    button {
      border-radius: 0 10px 10px 0;
      padding: 7px 20px;
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

.container {
  height: 400px;
  overflow: auto;
}

.mytable {
  width: auto;
  height: 400px;
}
.line-main-row {
  height: 50px;
}
.line-main-cell {
  width: auto;
  height: 40px;
  border: 1px solid #00acc1;
  background-color: #e0f7fa;
  border-radius: 10px;
  font-weight: bold;
  text-align: center;
  padding: 10px;
}
.line-r-cell {
  width: auto;
  height: 40px;
  border: 1px solid #00acc1;
  background-color: #e0f7fa;
  border-radius: 10px;
  font-weight: bold;
  padding: 10px;
}
.line-relation {
  height: 100px;
}
.relation-return {
  //background-color: rgb(114, 202, 114);
  box-sizing: border-box;
}
#myCanvas {
  display: block;
  margin: auto;
  border: 1px solid black;
}

// canvas {

//   border: 1px solid black;
// }
.space-line {
  //width: 20px;
  height: 3px;
  background-color: rgb(8, 8, 247);
}
</style>
