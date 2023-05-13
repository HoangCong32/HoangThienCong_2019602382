var CartController = function () {
    var cachedObj = {
    }
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax({
                url: '/Cart/RemoveFromCart',
                type: 'post',
                data: {
                    productId: id
                },
                success: function () {
                    webbansach.notify('Đã xóa sản phẩm khỏi giỏ hàng.', 'success');
                    loadHeaderCart();
                    loadData();
                }
            });
        });
        $('body').on('keyup', '.txtQuantity', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var q = $(this).val();
            if (q > 0) {
                $.ajax({
                    url: '/Cart/UpdateCart',
                    type: 'post',
                    data: {
                        productId: id,
                        quantity: q
                    },
                    success: function () {
                        webbansach.notify('Đã cập nhật số lượng', 'success');
                        loadHeaderCart();
                        loadData();
                    }
                });
            } else {
                webbansach.notify('Số nhập vào không đúng', 'error');
            }

        });

        //$('#btnClearAll').on('click', function (e) {
        //    e.preventDefault();
        //    $.ajax({
        //        url: '/Cart/ClearCart',
        //        type: 'post',
        //        success: function () {
        //            webbansach.notify('Clear cart is successful', 'success');
        //            loadHeaderCart();
        //            loadData();
        //        }
        //    });
        //});
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }

    function loadData() {
        $.ajax({
            url: '/Cart/GetCart',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                var template = $('#template-cart').html();
                var render = "";
                var totalAmount = 0;
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            ProductId: item.Product.Id,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: webbansach.formatNumber(item.Price, 0),
                            Quantity: item.Quantity,
                            Amount: webbansach.formatNumber(item.Price * item.Quantity, 0),
                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                        });
                    totalAmount += item.Price * item.Quantity;
                });
                $('#lblTotalAmount').text(webbansach.formatNumber(totalAmount, 0));
                if (render !== "")
                    $('#table-cart-content').html(render);
                else
                    $('#table-cart-content').html('Không có sản phẩm trong giỏ hàng');
            }
        });
        return false;
    }
}