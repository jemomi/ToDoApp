$(document).ready(function () {
    $('input[type=text]').on('focus', function () {
        //$($(this).labels()[0]).children('span').remove();
        $($(this).form()).removeClass('has-error');
    });

    $('#btnAddToDo').on('click', function (e) {
        e.preventDefault();
        addItem($(this), 'ToDoes');
    });

    $('#btnAddCat').on('click', function (e) {
        e.preventDefault();
        addItem($(this), 'Categories');
    });

    function addItem(sender, urlEndPoint) {
        var $self = $(sender);
        var $modal = $self.parents('.modal');
        var $form = $self.form();
        var formFields = $form.find('input');

        var data = $form.serialize();

        //$.each(formFields, function (i, item) {
        //    var $field = $(item);
        //    var $label = $($field.labels()[0])
        //    $label.children('span').remove();
        //});

        $.ajax({
            url: '../api/' + urlEndPoint,
            method: 'POST',
            data: data
        })
            .done(function (data) {
                $modal.modal('hide');

                $form.removeClass('has-error');
                $.each(formFields, function (i, item) {
                    var $field = $(item);
                    $field.val('');
                });

                getCategories();

                if (data.Category != null) {
                    $.notify(data.Name + ' tilføjet  til ' + data.Category.Name, 'success');
                } else {
                    $.notify('Oprettet kategori: ' + data.Name, 'success');
                }

            })
            .fail(function (data) {
                $.each(data.responseJSON.ModelState, function (key, val) {
                    var fieldName = key.slice((key.indexOf('.') + 1));
                    var $field = $form.find('input[name=' + fieldName + ']');
                    var $label = $($field.labels()[0]);

                    $form.addClass('has-error');
                    //$('<span>', { class: 'text-danger', text: ' fejl: ' + val[0].replace(fieldName, '') }).appendTo($label);

                    $field.notify(
                        val[0].replace(fieldName, ''),
                        { position: "bottom" }
                    );
                })
            });
    }

    function getCategories() {
        $('.columns-container').empty();

        $.ajax({
            url: '../api/Categories',
            method: 'GET'
        })
        .done(function (data) {
            $.each(data, function (key, item) {
                $('<section>', { 'data-catid': item.ID })
                .append($('<div>', { class: 'header-column' })
                    .append($('<header>')
                        .append($('<div>', { class: 'category', 'data.time': 'Today' })
                            .append($('<a>', { class: 'delCat', text: 'x', 'data-catid': item.ID }))
                            .append($('<h2>', { 'id': item.Name.replace(' ', '_'), text: item.Name })))
                        .append($('<a>', { class: 'add-inline', 'data-catid': item.ID, 'data-toggle': 'modal', 'data-target': '#createTask' })
                            .append($('<span>', { class: 'glyphicon glyphicon-plus' })))))
                .append($('<div>', { class: 'items-column', 'data-catid': item.ID })
                    .append(getTasks(key, item.ToDo))
                ).appendTo($('.columns-container'));
                //console.log(item);
            })

            $(".items-column").sortable({
                connectWith: ".items-column",
                placeholder: "ui-state-highlight",

                receive: function (event, ui) {
                    var $task = $(ui.item).children('.task');
                    var taskID = $task.data().taskid;
                    var catID = $(event.target).data().catid;
                    var $taskData = $task.children('.task-Data');

                    $taskData.children('input[name=CatID]').val(catID);

                    var data = $taskData.serialize();

                    $.ajax({
                        url: '../api/ToDoes/' + taskID,
                        method: 'PUT',
                        data: data
                    }).done(function () {
                        getCategories();
                    });
                }
            }).disableSelection();

            $('.delCat').on('click', function () {
                var id = $(this).data().catid;

                $.ajax({
                    url: '../api/Categories/' + id,
                    method: 'DELETE'
                }).done(function (data) {
                    getCategories();
                    $.notify('Slettet kategori: ' + data.Name, 'warn');
                });
            });

            $('.add-inline').on('click', function (e) {
                var $self = $(this);
                var catID = $self.data().catid;
                var modal = $self.data().target;
                var $modal = $(modal);
                var $form = $modal.children('.modal-dialog').children('form');
                var $inputHolder = $form.children('.modal-content').children('.modal-body')

                $inputHolder.children('input[name=CatID]').val(catID);
            });

            $('.finishTask').on("click", function () {
                var $self = $(this);
                var $task = $self.closest('.task')
                var id = $task.data().taskid;
                var $taskData = $task.children('.task-Data');

                $task.toggleClass("finished");

                $taskData.children('input[name=Finished]').val($task.hasClass('finished'));

                var data = $taskData.serialize();

                $.ajax({
                    url: '../api/ToDoes/' + id,
                    method: 'PUT',
                    data: data
                }).done(function (data) {
                    getCategories();

                    var checked = (data.Finished) ? "fuldført" : "genoptaget"
                    $.notify(data.Name + ' ' + checked, "info");
                });
            });

            $('.removeTask').on("click", function () {
                var $self = $(this);
                var $task = $self.closest('.task')
                var id = $task.data().taskid;

                $.ajax({
                    url: '../api/ToDoes/' + id,
                    method: 'DELETE'
                }).done(function (data) {
                    getCategories();
                    $.notify('Slettet task: ' + data.Name, 'warn');
                });
            });
        });
    }
    getCategories();

    function getTasks(i, ToDos) {
        var ToDoList = [];
        $.each(ToDos, function (index, todo) {
            var fin = (todo.Finished) ? "finished" : "";
            var ToDo = $('<div>', { class: 'task-container' })
            .append($('<div>', { class: 'task ' + fin, 'data-taskid': todo.ID })
                .append($('<p>', { class: 'title', text: todo.Name }))
                .append($('<div>', { class: 'icon finishTask' })
                    .append($('<span>', { class: 'glyphicon glyphicon-ok' })))
                .append($('<div>', { class: 'icon removeTask' })
                    .append($('<span>', { class: 'glyphicon glyphicon-remove' })))
                .append($('<form>', { class: 'task-Data' })
                    .append(getInputs(index, todo))));

            ToDoList.push(ToDo);
            //console.log(todo);
        })
        return ToDoList;
    }
    function getInputs(i, Data) {
        var DataList = [];
        $.each(Data, function (key, val) {
            var input = $('<input>', { 'type': 'hidden', 'name': key, val: val });
            DataList.push(input);
        });
        return DataList;
    }
});
