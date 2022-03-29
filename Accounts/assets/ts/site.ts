

(function () {
    console.log("Burger clicked");
    var burger = document.querySelector('.burger');
    if (burger && burger instanceof HTMLElement) {
        var menu = document.querySelector('#' + burger?.dataset.target);
        if (menu instanceof HTMLElement) {
            burger.addEventListener('click', function () {
                burger?.classList.toggle('is-active');
                menu?.classList.toggle('is-active');
            });
        }
    }
})();


(function () {
    $(document).ready((j) => {
        (<any>$("select[multiple]")).picker({ search: true });
    })    
})();
