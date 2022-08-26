

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

function openTab(evt: { currentTarget: { className: string; }; }, tabName: string) {
    var i, x, tablinks;

    x = Array.from(document.getElementsByClassName('content-tab') as HTMLCollectionOf<HTMLElement>)
    
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }

    tablinks = document.getElementsByClassName("tab");
    for (i = 0; i < x.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" is-active", "");
    }

    var element = document.getElementById(tabName)
    
    if (element != undefined) {
        element.style.display = "block";
    }

    evt.currentTarget.className += " is-active";
};
