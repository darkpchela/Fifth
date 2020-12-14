let tagify;
let input = document.querySelector('#tagsInput');
if (input) {
    tagify = new Tagify(input, {
        whitelist: [],
        maxTags: 10
    })
    tagify.settings.whitelist.splice(0, 2, 'a', 'b');
}

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

function addCard(data) {
    let cardElem = $('#cardPrototype').clone();
    cardElem.removeAttr('id');
    cardElem.attr("data-id", data.id)
    $(cardElem).find('[name="game"]').text(data.name);
    $(cardElem).find('[name="user"]').text(data.userName);
    $(cardElem).find('[name="ref"]').prop('href', `/Home/Game/${data.id}`)
    cardElem.prop('hidden', false);
    $('#gameCards').append(cardElem);
};


