<template>
  <li class="node-tree">
    <input type="checkbox" :id="node.label" />
    <label class="tree_label">{{ node.label }}</label>

    <ul v-if="node.children && node.children.length">
      <node v-for="child in node.children" :node="child" :key="child"></node>
    </ul>
  </li>
</template>

<script>
export default {
  name: "node",
  props: {
    node: Object
  }
};
</script>
<style>
.node-tree input {
  position: absolute;
  clip: rect(0, 0, 0, 0);
  }
.tree-list input ~ ul { display: none; }

.tree-list input:checked ~ ul { display: block; }  
.tree-list li {
  line-height: 1.2;
  position: relative;
  padding: 0 0 1em 1em;
  
  }

.tree-list ul li { padding: 1em 0 0 1em; }

.tree-list > li:last-child { padding-bottom: 0; }
.tree_label {
  position: relative;
  display: inline-block;
  background: #fff;
  }
label.tree_label { cursor: pointer; }

label.tree_label:hover { color: #666; }  
label.tree_label:before {
  background: #000;
  color: #fff;
  position: relative;
  z-index: 1;
  float: left;
  margin: 0 1em 0 -2em;
  width: 1em;
  height: 1em;
  border-radius: 1em;
  content: '+';
  text-align: center;
  line-height: .9em;
  }

:checked ~ label.tree_label:before { content: '–'; }

/* ————————————————————–
  Tree branches
*/
.tree li:before {
  position: absolute;
  top: 0;
  bottom: 0;
  left: -.5em;
  display: block;
  width: 0;
  border-left: 1px solid #777;
  content: "";
  }

.tree_label:after {
  position: absolute;
  top: 0;
  left: -1.5em;
  display: block;
  height: 0.5em;
  width: 1em;
  border-bottom: 1px solid #777;
  border-left: 1px solid #777;
  border-radius: 0 0 0 .3em;
  content: '';
  }

label.tree_label:after { border-bottom: 0; }

:checked ~ label.tree_label:after {
  border-radius: 0 .3em 0 0;
  border-top: 1px solid #777;
  border-right: 1px solid #777;
  border-bottom: 0;
  border-left: 0;
  bottom: 0;
  top: 0.5em;
  height: auto;
  }

.tree-list li:last-child:before {
  height: 1em;
  bottom: auto;
  }

.tree-list > li:last-child:before { display: none; }
</style>