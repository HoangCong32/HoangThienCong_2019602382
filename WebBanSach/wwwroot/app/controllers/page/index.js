﻿var PageController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtAliasM: { required: true }
            }
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

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Page/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    webbansach.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);

                    $('#txtAliasM').val(data.Alias);
                    CKEDITOR.instances.txtContentM.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status === 1);

                    $('#modal-add-edit').modal('show');
                    webbansach.stopLoading();

                },
                error: function () {
                    webbansach.notify('Có lỗi xảy ra', 'error');
                    webbansach.stopLoading();
                }
            });
        });

        $('#btnSaveM').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var seoAlias = $('#txtAliasM').val();
                var content = CKEDITOR.instances.txtContentM.getData();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/Page/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Content: content,
                        Status: status,
                        Alias: seoAlias
                    },
                    dataType: "json",
                    beforeSend: function () {
                        webbansach.startLoading();
                    },
                    success: function () {
                        webbansach.notify('Cập nhật trang thành công', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        webbansach.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        webbansach.notify('Có lỗi trong quá trình cập nhật', 'error');
                        webbansach.stopLoading();
                    }
                });
                return false;
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            webbansach.confirm('Bạn có muốn xóa trang không?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Page/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        webbansach.startLoading();
                    },
                    success: function () {
                        webbansach.notify('Xóa trang thành công', 'success');
                        webbansach.stopLoading();
                        loadData();
                    },
                    error: function () {
                        webbansach.notify('Có lỗi trong quá trình xóa', 'error');
                        webbansach.stopLoading();
                    }
                });
            });
        });
    };

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtAliasM').val('');
        CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);

    }

    function registerControls() {
        var editorConfig = {
            filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
        }
        CKEDITOR.replace('txtContentM', editorConfig);
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/page/GetAllPaging",
            data: {
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
                            Name: item.Name,
                            Alias: item.Alias,
                            Id: item.Id,
                            Status: webbansach.getStatus(item.Status)
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