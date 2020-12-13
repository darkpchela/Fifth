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
function addGameCard(data) {
    let userName = data.UserName;
    let gameName = data.GameName;
    let gameCardElem = '<div class="p-2"><div class="card border rounded border-warning ml-1 mr-1 bg-secondary"><div class="card-body border rounded border-warning">' +
        `<h5 class="card-title">${gameName}</h5><span class="card-text">${userName}</span><a>Play</a></div></div></div>`;
    $('#gameCards').append(gameCardElem);
}

