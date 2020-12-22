let tagify;

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
    let tags = JSON.stringify(getFilterTags());
    updateTableAjax(tags);
});

const getFilterTags = () => {
    let elems = $('#tagsFilter a');
    let tags = [];
    for (let i = 0; i < elems.length; i++) {
        let tag = {
            id: $(elems[i]).data('id'),
            value: $(elems[i]).data('value')
        };
        tags.push(tag);
    }
    return tags;
};

const updateTagify = () => {
    let input = document.querySelector('#tagsInput');
    if (input) {
        tagify = new Tagify(input, {
            whitelist: [],
            delimiters: ",| ",    
            maxTags: 5,
            dropdown: {
                maxItems: 20,
                classname: "tags-look",
                enabled: 0,
                closeOnSelect: false
            }
        });
        getTagsAjax();
    }
};

const updateNewGameRef = () => {
    $('#newGame').click((e) => {
        e.preventDefault();
        $('#modal').modal('show');
    })
};

const updateCreateGameRef = () => {
    $('#btnCreate').click(e => {
        e.preventDefault();
        let data = $('#createForm').serialize();
        createGameAjax(data);
    })
};

const createGameAjax = data => {
    $.ajax({
        type: "post",
        url: "Home/CreateGame",
        data: data,
        success: data => {
            $('#modalContent').empty();
            $('#modalContent').append(data);
            updateCreateGameRef();
            updateTagify();
        }
    });
}

const getTagsAjax = () => {
    $.ajax({
        type: "GET",
        url: "Home/GetTags",
        success: data => {
            tagify.settings.whitelist.splice(0, data.length, ...data);
        }
    });
}

const updateTableAjax = (tags) => {
    $.ajax({
        type: "POST",
        url: "Home/GamesTable",
        data: { "tagsJson": tags },
        success: data => {
            $('#gamesTable').empty();
            $('#gamesTable').append(data);
            $('#newGame').click((e) => {
                e.preventDefault();
                $('#modal').modal('show');
                updateNewGameRef();
            });
        }
    });
};


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/MainHub")
    .build();

hubConnection.on("UpdateGamesTable", () => {
    updateTableAjax();
});

hubConnection.start();

$(updateTableAjax());
$(updateCreateGameRef());
$(updateNewGameRef());
$(updateTagify());