

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

(function () {
    $(document).ready((j) => {
        (<any>$("nav.tabs li.tab")).on("click", function (this: HTMLElement) {
            const JQ = $(this);
            openTab(JQ, JQ.attr("x-tabName"));
        });
    })
})();

(function () {
    $(document).ready((j) => {
        (<any>$(".navbar-item.has-dropdown>.navbar-link")).on("click", function (this: HTMLElement) {
            const jq = $(this);
            jq.parent(".navbar-item.has-dropdown").toggleClass("is-active");
        });
    })
})();

(function () {
    $(document).ready((j) => {
        ($(".expandable[expands]")).on("click", function (this: HTMLElement) {
            var attrValue = this.getAttribute("expands");
            if (attrValue) {
                const jq = $(attrValue);
                jq.fadeToggle();
            }
        })
    })
})();

(function () {
    $(document).ready((j) => {
        $("[async-post] input[type=submit], [async-post] button[type=submit]").on("click", function (this: HTMLElement, e) {
            e.preventDefault();
            var input = $(this);
            var fstForm = input.parents("form").eq(0);
            var frm = fstForm.get(0);
            if (!frm)
                return;
            var tgtSelector = fstForm.attr("update-tgt");
            var updateTgt = tgtSelector == undefined ? undefined : $(tgtSelector);
            var url = fstForm.attr("action");

            var data = new FormData(frm)
            
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (updateTgt != null) {
                        updateTgt.html(data);
                    }
                }
            });
            return false;
        });
    })
})();

function openTab(obj: JQuery, tabName: string | undefined) {
    var i, x, tablinks;

    if (tabName == undefined)
        return;

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

    obj.addClass("is-active");
}


