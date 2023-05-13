var ProductQuantityController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        $('#txtFromDate, #txtToDate').datepicker({
            autoclose: true,
            format: 'mm/dd/yyyy'
        });

        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en'
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });
        $("#btn-search").on('click', function () {
            loadData();
        });

        $("#ddl-show-page").on('change', function () {
            webbansach.configs.pageSize = $(this).val();
            webbansach.configs.pageIndex = 1;
            loadData(true);
        });

        $('#btn-export').on('click', function () {
            var Publisher = $('#txt-search-keyword').val();
            var StartDate = $('#txtFromDate').val();
            var EndDate = $('#txtToDate').val();
            $.ajax({
                type: "POST",
                url: "/admin/productquantity/exportexcel",
                data: {
                    publisher: Publisher,
                    startDate: webbansach.dateTimeFormatJson(StartDate),
                    endDate: webbansach.dateTimeFormatJson(EndDate) 
                },
                beforeSend: function () {
                    webbansach.startLoading();
                },
                success: function (response) {
                    window.location.href = response;
                    webbansach.stopLoading();
                },
                error: function () {
                    webbansach.notify('Có lỗi xảy ra trong quá trình xuất phiếu', 'error');
                    webbansach.stopLoading();
                }
            });
        });

    };

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/productquantity/GetAllPaging",
            data: {
                startDate: $('#txtFromDate').val(),
                endDate: $('#txtToDate').val(),
                keyword: $('#txt-search-keyword').val(),
                page: webbansach.configs.pageIndex,
                pageSize: webbansach.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                webbansach.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            ProductName: item.Product.Name,
                            Publisher: item.Product.Publisher,
                            Quantity: item.Quantity,
                            OriginalPrice: webbansach.formatNumber(item.Product.OriginalPrice, 0),
                            CreatedDate: webbansach.dateTimeFormatJson(item.DateCreated),
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render != undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $('#tbl-content').html('');
                }
                webbansach.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / webbansach.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                webbansach.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}