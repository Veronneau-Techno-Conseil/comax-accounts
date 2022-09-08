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
function openTab(evt, tabName) {
    var i, x, tablinks;
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
    evt.currentTarget.className += " is-active";
}
;
