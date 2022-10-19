(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";
(function () {
    console.log("Burger clicked");
    var burger = document.querySelector('.burger');
    if (burger && burger instanceof HTMLElement) {
        var menu = document.querySelector('#' + (burger === null || burger === void 0 ? void 0 : burger.dataset.target));
        if (menu instanceof HTMLElement) {
            burger.addEventListener('click', function () {
                burger === null || burger === void 0 ? void 0 : burger.classList.toggle('is-active');
                menu === null || menu === void 0 ? void 0 : menu.classList.toggle('is-active');
            });
        }
    }
})();
(function () {
    $(document).ready((j) => {
        $("select[multiple]").picker({ search: true });
    });
})();
(function () {
    $(document).ready((j) => {
        $("nav.tabs li.tab").on("click", function () {
            const JQ = $(this);
            openTab(JQ, JQ.attr("x-tabName"));
        });
    });
})();
(function () {
    $(document).ready((j) => {
        $(".navbar-item.has-dropdown>.navbar-link").on("click", function () {
            const jq = $(this);
            jq.parent(".navbar-item.has-dropdown").toggleClass("is-active");
        });
    });
})();
function openTab(obj, tabName) {
    var i, x, tablinks;
    if (tabName == undefined)
        return;
    x = Array.from(document.getElementsByClassName('content-tab'));
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tab");
    for (i = 0; i < x.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" is-active", "");
    }
    var element = document.getElementById(tabName);
    if (element != undefined) {
        element.style.display = "block";
    }
    obj.addClass("is-active");
}

},{}],2:[function(require,module,exports){
"use strict";!function(t,i,e,n){var r=function(i,e){this.elem=i,this.$elem=t(i),this.options=e,this.currentData=[]};r.prototype={defaults:{containerClass:"",containerWidth:!1,width:!1,search:!1,searchAutofocus:!1,autofocusScrollOffset:0,coloring:{},limit:n,texts:{trigger:"Select value",noResult:"No results",search:"Search"}},config:{},init:function(){return this.config=t.extend({},this.defaults,this.options),this.$elem.is("select")?(this.config.multiple=this.$elem.is("select[multiple='multiple']")||this.$elem.is("select[multiple]"),this.config.width===!1||Math.floor(this.config.width)==this.config.width&&t.isNumeric(this.config.width)?this.config.containerWidth===!1||Math.floor(this.config.containerWidth)==this.config.containerWidth&&t.isNumeric(this.config.containerWidth)?0==this.$elem.find("option:not([hidden])").length?void console.log("Picker - Select has no options. Can not proceed!"):(!this.config.multiple&&this.config.limit>0&&console.log("Picker - You are applying limit parameter on single-seleciton mode Picker!"),this.config.limit<0?void console.log("Picker - Limit has to be greater then 0!"):(this._build(),this.$elem.hide(),this._fillList(),this.$container.find(".pc-trigger").click(function(){var i=this.$container.find(".pc-list");i.toggle(),this.$elem.trigger(i.is(":visible")?"sp-open":"sp-close"),this.config.search&&this.config.searchAutofocus&&i.is(":visible")&&(i.find("input").focus(),t("html, body").animate({scrollTop:i.find("input").offset().top-this.config.autofocusScrollOffset},800))}.bind(this)),t(e).mouseup(function(t){var i=this.$container.find(".pc-list");i.is(t.target)||0!==i.has(t.target).length||this.$container.find(".pc-trigger").is(t.target)||(i.hide(),this.$elem.trigger("sp-close"),this.config.search&&(this.$container.find(".pc-list input").val(""),this._updateList(this.currentData)))}.bind(this)),this)):void console.log("Picker - Container width is not a integer."):void console.log("Picker - Width is not a integer.")):void console.log("Picker - Element is not Selectbox")},pc_selected:function(i){var e=t(i.target),n=e.data("id");this._selectElement(n,e),this.$container.find(".pc-list").hide(),this.config.search&&(this.$container.find(".pc-list input").val(""),this._updateList(this.currentData)),this.$elem.trigger("sp-change")},pc_remove:function(i){var e=t(i.target),n=e.parent().data("id"),r=e.parent().data("order"),s=t("<li>").html(e.parent().text()).attr("data-id",n).attr("data-order",r);s.click(this.pc_selected.bind(this)),this.config.search&&this._insertIntoCurrentData(i),this.$container.find(".pc-trigger").show();var a=this.$container.find(".pc-list li");0==this.$container.find(".pc-list li").length?this.$container.find(".pc-list ul").html("").append(s):1==a.length?r>a.data("order")?s.insertAfter(a):s.insertBefore(a):a.each(function(i,e){if(e=t(e),e.is(":first-child")){if(r<e.data("order"))return s.insertBefore(e),!1;if(r>e.data("order")&&r<e.next().data("order"))return s.insertAfter(e),!1}else if(e.is(":last-child")){if(r>e.data("order"))return s.insertAfter(e),!1}else if(r>e.data("order")&&r<e.next().data("order"))return s.insertAfter(e),!1}),this.$elem.find(" option[value='"+n+"']").removeAttr("selected"),e.parent().remove(),this.$elem.trigger("sp-change")},pc_search:function(i){var e=t(i.target).val().toLowerCase(),n=this._filterData(e);this._updateList(n,e)},_selectElement:function(t,i){if(i==n&&(i=this.$container.find('.pc-list li[data-id="'+t+'"]'),0==i.length))return void console.log("Picker - ID to select not found!");if(this.config.multiple){this.$container.prepend(this._createElement(i)),i.remove();var e=this.config.limit&&this.$container.find(".pc-element:not(.pc-trigger)").length>=this.config.limit;this.config.search?(this.currentData=this.currentData.filter(function(i){return i.id!=t}),(0==this.currentData.length||e)&&this.$container.find(".pc-trigger").hide()):(0==this.$container.find(".pc-list li").length||e)&&this.$container.find(".pc-trigger").hide()}else this.$elem.find("option").removeAttr("selected"),this.config.coloring[t]?this.$container.find(".pc-trigger").removeClass().addClass(this.config.coloring[selectedId]+" pc-trigger pc-element").contents().first().replaceWith(i.text()):this.$container.find(".pc-trigger").contents().first().replaceWith(i.text());this.$elem.find("option[value='"+t+"']").attr("selected","selected")},_insertIntoCurrentData:function(i){var e=t(i.target),n=e.parent().data("id"),r=e.parent().data("order");if(0==this.currentData.length)return void(this.currentData=[{id:n,text:e.parent().text(),order:r}]);var s;for(s=0;s<this.currentData.length;s++)if(0==s){if(r<this.currentData[s].order||1==this.currentData.length){this.currentData.splice(0,0,{id:n,text:e.parent().text(),order:r});break}}else if(s==this.currentData.length-1){if(r>this.currentData[s].order){this.currentData.splice(s,0,{id:n,text:e.parent().text(),order:r});break}}else if(this.currentData[s-1].order<r&&r<this.currentData[s].order){this.currentData.splice(s,0,{id:n,text:e.parent().text(),order:r});break}},_createElement:function(i){var e=this.config.coloring[i.data("id")],n=t("<span>").addClass("pc-element "+(e?e:"")).text(i.text()).attr("data-id",i.data("id")).attr("data-order",i.data("order"));return n.append(t('<span class="pc-close"></span>').click(this.pc_remove.bind(this))),n},_build:function(){var i=this.config.texts.trigger;this.$container=t("<div class='picker"+(this.config.containerClass?" "+this.config.containerClass:"")+"'><span class='pc-select'><span class='pc-element pc-trigger'>"+i+"</span><span class='pc-list' "+(this.config.width?"style='width:"+this.config.width+"px; display:none;'":"style='display:none;'")+"><ul></ul></span></span></div>"),this.config.containerWidth!==!1&&this.$container.width(this.config.containerWidth),this.$container.insertAfter(this.$elem),this.config.search&&this._buildSearch()},_buildSearch:function(){var i=t("<input type='search' placeholder='"+this.config.texts.search+"'>");i.on("input",this.pc_search.bind(this)),i.on("keypress",function(i){if(13==i.which){var e=t(i.target).val().toLowerCase(),n=this._filterData(e);if(1==n.length)return this.$container.find(".pc-list li").first().click(),!1}return!0}.bind(this)),this.$container.find(".pc-list").prepend(i)},_fillList:function(){var i=this.$container.find(".pc-list ul"),e=0;this.$elem.find("option:not([hidden])").each(function(n,r){var s=t("<li>").html(t(r).text()).attr("data-id",t(r).attr("value")).attr("data-order",e);s.click(this.pc_selected.bind(this)),i.append(s),this.config.search&&this.currentData.push({id:t(r).attr("value"),text:t(r).text(),order:e}),"selected"==t(r).attr("selected")&&s.click(),e++}.bind(this)),this.$container.find(".pc-trigger").show()},_filterData:function(t){return this.currentData.filter(function(i){return-1!=i.text.toLowerCase().indexOf(t)})},_updateList:function(i,e){var r=this.$container.find(".pc-list ul");if(0==i.length)return void r.html('<li class="not-found">'+this.config.texts.noResult+"</li>");r.html("");var s,a;for(s=0;s<i.length;s++){if(e!==n){var c=new RegExp("("+e+")","gi");a=i[s].text.replace(c,'<span class="searched">$1</span>')}else a=i[s].text;var o=t("<li>").html(a).attr("data-id",i[s].id).attr("data-order",i[s].order);o.click(this.pc_selected.bind(this)),r.append(o)}},api:function(t){return r.prototype["api_"+t[0]]?this["api_"+t[0]](t.slice(1)):void console.log("Picker - unknown command!")},api_destroy:function(){return this.$container.remove(),this.$elem.show(),this.$elem.removeData("plugin_picker"),this.$elem},api_get:function(){return this.$elem.val()},api_set:function(t){return 1!=t.length?void console.log("Picker - unknown number of arguments."):(this._selectElement(t[0]),this.$elem.trigger("sp-change"),this.$elem)},api_remove:function(t){if(1!=t.length)return void console.log("Picker - unknown number of arguments.");if(!this.config.multiple)return void console.log("Picker - remove method is allowed only with multiple-selection mode!");var i={};return i.target=this.$container.find('.pc-element[data-id="'+t[0]+'"] .pc-close')[0],this.pc_remove(i),this.$elem}},t.fn.picker=function(i){var e=arguments;if(1==t(this).length){var n=t(this).data("plugin_picker");return n?n.api(Array.prototype.slice.call(e)):(t(this).data("plugin_picker",new r(this,i).init()),this)}return this.each(function(){var n=t(this).data("plugin_picker");n?n.api(Array.prototype.slice.call(e)):t(this).data("plugin_picker",new r(this,i).init())})}}(jQuery,window,document);
},{}]},{},[2,1]);
