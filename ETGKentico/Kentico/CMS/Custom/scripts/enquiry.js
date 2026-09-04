cmsdefine(['jQuery', 'Underscore'], function ($, _) {

    var doc = $(document),
        selectStatus = function(e) {

            $('.select-status').change(function(e) {
                var domain = $('.data-presentation-url').text();
                $.ajax({
                        url: domain +
                            '/api/enquiry/updatestatus?itemid=' +
                            $(this).attr('data-id') +
                            '&status=' +
                            $(this).val(),
                        type: 'GET',
                        dataType: 'json',
                        contentType: 'application/json'
                    })
                    .done(function(response) {
                        alert('status updated');

                    })
                    .always(function() {
                        // HIDE BLOCKER
                    });
            });

        },
        selectAssignee = function(e) {

            $('.select-assignee').change(function(e) {
                var domain = $('.data-presentation-url').text();
                $.ajax({
                        url: domain +
                            '/api/enquiry/assign?itemid=' +
                            $(this).attr('data-id') +
                            '&userguid=' +
                            $(this).val(),
                        type: 'GET',
                        dataType: 'json',
                        contentType: 'application/json'
                    })
                    .done(function(response) {
                        alert('consultant updated');

                    })
                    .always(function() {
                        // HIDE BLOCKER
                    });
            });

        };


    doc.ready(selectStatus);
    doc.ready(selectAssignee);
    return function () { }
});