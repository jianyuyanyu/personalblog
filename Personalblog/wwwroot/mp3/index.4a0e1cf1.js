var m = Object.defineProperty; var S = (o, e, i) => e in o ? m(o, e, { enumerable: !0, configurable: !0, writable: !0, value: i }) : o[e] = i; var c = (o, e, i) => (S(o, typeof e != "symbol" ? e + "" : e, i), i); (function () { const e = document.createElement("link").relList; if (e && e.supports && e.supports("modulepreload")) return; for (const n of document.querySelectorAll('link[rel="modulepreload"]')) s(n); new MutationObserver(n => { for (const t of n) if (t.type === "childList") for (const r of t.addedNodes) r.tagName === "LINK" && r.rel === "modulepreload" && s(r) }).observe(document, { childList: !0, subtree: !0 }); function i(n) { const t = {}; return n.integrity && (t.integrity = n.integrity), n.referrerpolicy && (t.referrerPolicy = n.referrerpolicy), n.crossorigin === "use-credentials" ? t.credentials = "include" : n.crossorigin === "anonymous" ? t.credentials = "omit" : t.credentials = "same-origin", t } function s(n) { if (n.ep) return; n.ep = !0; const t = i(n); fetch(n.href, t) } })(); function u(o) { throw new Error("[PlayerCore error]: " + o) } function f(o) { console.warn("[PlayerCore warn]: " + o) } function w(o) { typeof o > "u" && u("Expect other value but got undefined") } class v { constructor({ }) { c(this, "e"); c(this, "SongIdList"); c(this, "CurrentSongId"); c(this, "IsPlaying"); c(this, "SongIdMap"); c(this, "PlayMode"); c(this, "IsMute"); this.e = document.createElement("audio"), this.e.volume = .1, this.SongIdList = [], this.SongIdMap = {}, this.PlayMode = 1, this.CurrentSongId = -1, this.IsPlaying = !1, this.IsMute = !1 } _AppendSongByIndex(e, i) { this.SongIdList.splice(i, 0, e.id), this.SongIdMap[e.id] = e } _ApeendCheck(e) { e ? (this.SongIdList.indexOf(e.id) !== -1 || this.SongIdMap[e.id]) && u("The song which append to queue has repeat id.") : u("AppendSong function had been call but without song info.") } QuerySongInfo(e, i = !1) { let s = -1; if (typeof e == "number") s = e; else for (const n in this.SongIdMap) { const t = this.SongIdMap[n]; (s === -1 || i) && t.name === e && (s = parseInt(n)) } return this.SongIdMap[s] } RemoveSong(e, i = !1) { w(e); const s = this.QuerySongInfo(e, i); if (s) { if (this.CurrentSongId === s.id) if (this.SongIdList.length === 1) this.e.pause(), this.e.src = "", this.CurrentSongId = NaN; else { let n = this.SongIdList.indexOf(s.id); n === this.SongIdList.length - 1 ? n = 0 : n++; const t = this.SongIdMap[this.SongIdList[n]]; t && (this.CurrentSongId = t.id, this.e.src = t.src) } delete this.SongIdMap[s.id], this.SongIdList.splice(this.SongIdList.indexOf(s.id), 1) } } AppendSong(e, i, s = !1) { this._ApeendCheck(e); let n = typeof i == "number" ? i : this.SongIdList.length; if (typeof i == "string") { const t = this.QuerySongInfo(i, s); if (t) { const r = this.SongIdList.indexOf(t.id); r < n && r >= 0 && (n = r + 1) } } this._AppendSongByIndex(e, n) } AppendSongOnHead(e) { this._ApeendCheck(e), this._AppendSongByIndex(e, -1) } AppendSongOnTail(e) { this._ApeendCheck(e), this._AppendSongByIndex(e, this.SongIdList.length) } AppendSongList(e) { e.forEach(i => { this.AppendSongOnTail(i) }) } UpdateSongPosition(e) { e.length != this.SongIdList.length && u("Update Song Position size is not equal with old list size"); const i = new Set(this.SongIdList.map(s => s)); if (e.forEach(s => { i.delete(s) }), i.size) { const s = []; i.forEach(n => { s.push(n) }), u("New list has unknown id: " + s) } this.SongIdList = e } PlaySelectSong(e) { const i = this.SongIdMap[e]; if (this.SongIdList.indexOf(e) === -1) { f(`Song's id: '${e}' is not in the id list or it's not corrent number.`); return } i && (this.e.src = i.src, this.CurrentSongId = e), this.Play() } NextSong() { const e = this.SongIdList.indexOf(this.CurrentSongId); let i = 0; e !== this.SongIdList.length - 1 && (i = e + 1), this.PlaySelectSong(this.SongIdList[i]) } PrevSong() { const e = this.SongIdList.indexOf(this.CurrentSongId); let i = this.SongIdList.length - 1; e !== 0 && (i = e - 1), this.PlaySelectSong(this.SongIdList[i]) } SwitchMute() { this.IsMute = !this.e.muted, this.e.muted = this.IsMute } SwitchPlayMode(e) { let i = 1; typeof e == "object" && (i = parseInt(e.currentTarget.getAttribute("data-mode-id"))), i === 4 ? i = 1 : i++, this.PlayMode = i } async Play() { this.e.src.length === 0 && this.PlaySelectSong(this.SongIdList[0]), await this.e.play(), this.IsPlaying = !0 } Pause() { this.e.pause(), this.IsPlaying = !1 } ChangeVolume(e) { e < 0 && e <= 1 ? this.e.volume = 0 : e > 1 ? this.e.volume = 1 : this.e.volume = e } ChangeCurrentSongTime(e) { if (typeof e != "number") { f("Type of time expect number but got: " + typeof e); return } this.e.currentTime = e } } const y = o => { const e = o.exp ? document.querySelector(o.exp) : o.el; if (!e) return; let i = 0, s = 0; function n(d, a) { e.style.setProperty("left", d + "px"), e.style.setProperty("top", a + "px") } function t(d) { let a = d.clientX - i, l = d.clientY - s; a <= 0 ? a = 0 : a + e.offsetWidth >= window.innerWidth && (a = window.innerWidth - e.offsetWidth), l <= 0 ? l = 0 : l + e.offsetHeight >= window.innerHeight && (l = window.innerHeight - e.offsetHeight), n(a, l) } window.addEventListener("resize", () => { t({ clientX: window.innerWidth, clientY: window.innerHeight }) }); function r() { window.removeEventListener("mousemove", t) } return o.el.addEventListener("mousedown", d => { i = d.clientX - e.offsetLeft, s = d.clientY - e.offsetTop; function a() { r(), window.removeEventListener("mouseup", a) } window.addEventListener("mousemove", t), window.addEventListener("mouseup", a) }), () => { } }; const I = o => { const e = o.el; return window.addEventListener("mouseup", () => { e.offsetLeft > window.innerWidth / 2 ? e.style.setProperty("left", window.innerWidth - e.clientWidth + "px") : e.style.setProperty("left", "0px") }), () => { } }; function g(o) { o || (o = 0); let e = parseInt(o / 60 + ""), i = parseInt(o % 60 + ""); return e < 10 && (e = "0" + e), i < 10 && (i = "0" + i), `${e}:${i}` } const L =`<div v-scope id="CoreWrapper" v-wrapper-adsorb @mouseup="wrapperMouseUp"@mousedown="clearHiddenTimer" @vue:mounted="onMounted" :class="\`player_module \${dragging ? 'dragging': ''} \${onRight ? 'right' : 'left'} \${hidden && !hover ? 'hidden' : ''}\`"><div class="left_arrow" @click="clickShow"><i class="iconfont icon-24gl-arrowLeft3"></i></div><div class="operate_box" @mousedown="wrapperMouseDown" @mouseenter="wrapperMouseEnter"@mouseleave="wrapperMouseLeave"><div class="left_img" :style="\`background-image: url(\${currentSongInfo.img ? currentSongInfo.img  : 'https://p1.music.126.net/u90NnX64TqnsAn-4mb0yfQ==/2946691187826531.jpg?param=200y200'});\`"></div><div class="right_operate"><div class="song_name" v-draggable="#CoreWrapper"><span class="name">{{ currentSongInfo.name }}</span><span class="time">{{ mediaCurrentTime }}/{{ mediaDuration }}</span></div><div class="operation_button"><i class="iconfont icon-24gl-volumeZero" v-if="store.IsMute" @click="store.SwitchMute"></i><i class="iconfont icon-24gl-volumeMiddle" v-else @click="store.SwitchMute"></i><i class="iconfont icon-24gl-previousCircle" @click="store.PrevSong"></i><i class="iconfont icon-24gl-pauseCircle" v-if="store.IsPlaying" @click="CorePause"></i><i class="iconfont icon-24gl-playCircle" v-else @click="CorePlay"></i><i class="iconfont icon-24gl-nextCircle" @click="store.NextSong"></i><i class="iconfont icon-24gl-arrowRight" v-if="store.PlayMode === 1" data-mode-id="1"@click="store.SwitchPlayMode"></i><i class="iconfont icon-24gl-repeat2" v-else-if="store.PlayMode === 2"data-mode-id="2" @click="store.SwitchPlayMode"></i><i class="iconfont icon-24gl-repeatOnce2" v-else-if="store.PlayMode === 3"data-mode-id="3" @click="store.SwitchPlayMode"></i><i class="iconfont icon-24gl-shuffle" v-else @click="store.SwitchPlayMode"data-mode-id="4"></i></div><div class="slider" @click.stop.prevent><div class="track" @mousedown="jumpTime"><div class="active_track" :style="\`width: \${showupPrecentage}%;\`"></div></div><div class="thumb" :style="\`left: \${showupPrecentage}%;\`"></div></div></div></div><div class="right_arrow" @click="clickShow"><i class="iconfont icon-24gl-arrowRight3"></i></div><div class="show_list_button" @click="switchListShow"></div><div :class="\`list_box player_module \${showList ? 'show' : ''}\`" @mouseenter="wrapperMouseEnter"@mouseleave="wrapperMouseLeave"><div class="empty_tips" v-if="store.SongIdList.length === 0">No Data :(</div><div :class="\`song_item \${i.id === store.CurrentSongId ? 'current' : ''}\`"v-for="(i, index) in songInfoList" @dblclick="CorePlaySelectSong(i.id)"><div class="title">{{ i.name }}</div><div class="function_button"><i class="iconfont icon-24gl-playCircle" @click="store.PlaySelectSong(i.id)"></i><i class="iconfont icon-24gl-trash2" @click="store.RemoveSong(i.id)"></i></div></div></div></div>`;(function(){const o="CoreWrapper",e=PetiteVue.reactive(new v({}));window._PlayerCore=e,console.log("Vue-Mini-Player powered by: https://github.com/QingXia-Ela/vue-mini-player");const i=document.createElement("div");i.setAttribute("id","CorePlayer"),i.innerHTML=L,document.body.appendChild(i);let s;const n=PetiteVue.createApp({store:e,dragging:!1,onRight:!1,hidden:!0,hover:!1,currentTime:0,duration:0,showupPrecentage:0,showList:!1,sliding:!1,slidingTime:0,onMounted(){const{e:t}=e;t.addEventListener("timeupdate",()=>{this.sliding||(this.CurrentTime=t.currentTime,this._UpdateShowPrecentage(this.CurrentTime/this.duration))}),t.addEventListener("loadedmetadata",()=>{this.sliding||(this.duration=t.duration)}),this.store.e.addEventListener("ended",()=>{const{SongIdList:r,CurrentSongId:d,PlayMode:a}=this.store,l=r.indexOf(d);switch(a){case 1:l!==r.length&&e.NextSong();break;case 2:e.NextSong();break;case 3:e.Play();break;case 4:let h=parseInt(r.length*Math.random()+"");for(;h===d;)h=parseInt(r.length*Math.random()+"");e.PlaySelectSong(r[h]);break;default:e.NextSong();break}})},get currentPrecentage(){return(this.currentTime*100).toFixed(2)},get currentSongInfo(){const t=e.SongIdMap[e.CurrentSongId];return t||{name:"_(:\u0437\u300D\u2220)_"}},get mediaDuration(){return g(this.duration)},get mediaCurrentTime(){return this.sliding?g(this.slidingTime):g(this.CurrentTime)},get songInfoList(){const t=[];for(const r of e.SongIdList)t.push(e.SongIdMap[r]);return t},clearHiddenTimer(){clearTimeout(s)},startTimer(){s&&this.clearHiddenTimer(),s=setTimeout(()=>{this.hidden=!0,this.hover=!1,this.showList=!1,this.clearHiddenTimer()},8e3)},wrapperMouseDown(){this.dragging=!0,this.hidden=!1},wrapperMouseUp(){this.dragging=!1;const t=document.getElementById(o);t&&(t.offsetLeft>window.innerWidth/2?this.onRight=!0:this.onRight=!1)},wrapperMouseEnter(){this.hover=!0,this.clearHiddenTimer()},wrapperMouseLeave(){this.hover=!1,this.startTimer()},clickShow(t){clearTimeout(s),this.hidden=!this.hidden,this.hidden&&(this.showList=!1),t.preventDefault(),this.startTimer()},_UpdateShowPrecentage(t){t>1?t=1:t<0&&(t=0),this.showupPrecentage=(t*100).toFixed(2),this.slidingTime=this.duration*t},jumpTime(t){if(isNaN(this.store.e.duration))return;const r=t.currentTarget;let d=t.offsetX/r.clientWidth;this._UpdateShowPrecentage(d),this.sliding=!0,d*=r.clientWidth;const a=h=>{const p=document.getElementById(o);d=h.clientX-p.offsetLeft-r.clientWidth,d+=this.onRight?25:85,this._UpdateShowPrecentage(d/r.clientWidth)},l=()=>{this.store.e.currentTime=d/r.clientWidth*this.duration,this.sliding=!1,window.removeEventListener("mousemove",a),window.removeEventListener("mouseup",l)};window.addEventListener("mousemove",a),window.addEventListener("mouseup",l)},switchListShow(){this.showList=!this.showList},async CorePlay(){try{await e.Play()}catch(t){console.log(t)}},CorePause(){e.Pause()},CorePlaySelectSong(t){e.PlaySelectSong(t)}});n.directive("draggable",y),n.directive("wrapper-adsorb",I),n.mount("#CorePlayer")})();
