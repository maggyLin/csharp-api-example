<template>
  <div>
     <el-carousel indicator-position="outside">
        <el-carousel-item v-for="item in looplist" :key="item.bannerId">
          <img :src="item.imgUrl"/>
        </el-carousel-item>
      </el-carousel>
    <ul>
      <nuxt-link to="/film/nowplaying" tag="li" activeClass="active">正在热映</nuxt-link>
      <nuxt-link to="/film/comingsoon" tag="li" activeClass="active">即将上演</nuxt-link>
    </ul>

    <!-- pages/film 內 -->
    <nuxt-child></nuxt-child>
    
  </div>
</template>
<script>
import axios from 'axios'
export default {
  asyncData() {
    return axios({
      url:"https://m.maizuo.com/gateway?type=2&cityId=110100&k=4562288",
      headers:{
        'X-Client-Info': '{"a":"3000","ch":"1002","v":"5.0.4","e":"15610855429195524981146"}',
        'X-Host': 'mall.cfg.common-banner'
      }
    }).then(res=>{
      return {
        looplist:res.data.data
      }
    })
  },
}
</script>
<style scoped>
  img{
    width: 100%;
  }
</style>
