var QuantityManagement = function () {
    var self = this;
    var cachedObj = {
    }

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-quantity', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            loadQuantities();
            $('#modal-quantity-management').modal('show');
        });
        $('body').on('click', '.btn-delete-quantity', function (e) {
            e.preventDefault();
            $(this).closest('tr').remove();
        });

        $('#btn-add-quantity').on('click', function () {
            var template = $('#template-table-quantity').html();
            var render = Mustache.render(template, {
                Id: 0,
                Quantity: 0
            });
            $('#table-quantity-content').append(render);
        });
        $("#btnSaveQuantity").on('click', function () {
            var quantityList = [];
            $.each($('#table-quantity-content').find('tr'), function (i, item) {
                quantityList.push({
                    Id: $(item).data('id'),
                    ProductId: $('#hidId').val(),
                    Quantity: $(item).find('input.txtQuantity').first().val(),
                });
            });
            $.ajax({
                url: '/admin/Product/SaveQuantities',
                data: {
                    productId: $('#hidId').val(),
                    quantities: quantityList
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-quantity-management').modal('hide');
                    $('#table-quantity-content').html('');
                }
            });
        });
    }
    function loadQuantities() {
        $.ajax({
            url: '/admin/Product/GetQuantities',
            data: {
                productId: $('#hidId').val()
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                var template = $('#template-table-quantity').html();
                $.each(response, function (i, item) {
                    render += Mustache.render(template, {
                        Id: item.Id,
                        Quantity: item.Quantity
                    });
                });
                $('#table-quantity-content').html(render);
                resetFile();
            }
        });
    }
}