let input = document.querySelector('#tagsInput');
new Tagify(input);
$('#tagsCloud a, #tagsFilter a').click((e) => {
    e.preventDefault();
    let selected = $(e.target).attr('data-selected');
    if (selected == 0) {
        $(e.target).appendTo($('#tagsFilter'))
        e.target.setAttribute('data-selected', 1)
    }
    else {
        $(e.target).appendTo($('#tagsCloud'));
        e.target.setAttribute('data-selected', 0);
    }
});
$('#newGame').click((e) => {
    e.preventDefault();
    $('#modal').modal('show');
})