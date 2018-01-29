$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        var Next = [];
        $("#myTable tr .searchTheme").filter(function () {
            Next.push($(this).text().toLowerCase().indexOf(value) > -1);
            $(this.parentElement).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
        if (Next.indexOf(true) == -1) {
            for (var i = 0; i < Next.length; i++) {
                $("#myTable tr .searchName").filter(function () {
                    $(this.parentElement).toggle(Next = $(this).text().toLowerCase().indexOf(value) > -1)
                    Next = true;
                });
            }
        }
    });

    var counter = 0;
});

function on(poz) {
    var Next = [];

    $('#myTable tr').sort(function (a, b) { 
        var arg1 = a.children[poz].textContent;
        var arg2 = b.children[poz].textContent;
        Next.push(arg1.toLowerCase() > arg2.toLowerCase());
        return arg1.toLowerCase() > arg2.toLowerCase();
    })
        .appendTo('#myTable');

    if (Next.indexOf(true) == -1) {
        $('#myTable tr').sort(function (a, b) { 
            var arg1 = a.children[poz].textContent;
            var arg2 = b.children[poz].textContent;
            Next.push(arg1.toLowerCase() > arg2.toLowerCase());
            return arg1.toLowerCase() < arg2.toLowerCase();
        })
            .appendTo('#myTable');

    }
}