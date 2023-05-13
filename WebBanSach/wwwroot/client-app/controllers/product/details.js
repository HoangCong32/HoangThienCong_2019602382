var ProductDetailController = function () {
    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('#btnAddToCart').on('click', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            $.ajax({
                url: '/Cart/AddToCart',
                type: 'post',
                dataType: 'json',
                data: {
                    productId: id,
                    quantity: parseInt($('#txtQuantity').val())
                },
                success: function () {
                    webbansach.notify('Sản phẩm đã được thêm vào giỏ hàng', 'success');
                    loadHeaderCart();
                },
                error: function () {
                    webbansach.notify('Có lỗi khi thêm vào giỏ hàng', 'error');
                }
            });
        });
    }

    function loadHeaderCart() {
        $("#headerCart").load("/AjaxContent/HeaderCart");
    }
}