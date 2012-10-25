/***********************************************************
        KendoUI Grid Helper 
        -- Requires KendoUI and Expects Data in the from of SympleLib.MVC.HelpersKendoUiHelper.KendoGridResult
************************************************************/
$.fn.sympleTech_KendoGrid = function (options) {

    var settings = $.extend({
        'title': '',
        'dataSourceURL': '',
        'exportName': 'GridData',
        'model': {
            id: 'id',
            fields: {}
        },
        'columns': [],
        'pagesize': 10,
        'rowSelectable': false,
        'onRowSelected': function (id) { },
        'multiSelectable' : false,
        'searchForm': '',
        'showExport': false
    }, options);

    return this.each(function () {

        //Bind the datasource to the linked form
        var formParams = {};
        if (settings.searchForm != "") {
            var $form = $("#" + settings.searchForm);
            var formData = $form.serialize().split('&');

            $(formData).each(function () {
                var nvp = this.split('=');
                formParams[nvp[0]] = function () {
                    var $inputEl = $form.find('*[name=' + nvp[0] + ']').first();

                    if ($inputEl.attr("type") == "checkbox") {
                        return $inputEl.is(':checked');
                    }

                    return $inputEl.val();
                };
            });
        }

        //-- Data Source 
        var gridDataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: settings.dataSourceURL,
                    type: "POST",
                    data: formParams
                }
            },
            schema: {
                data: function (data) {
                    return data.Items;
                },
                model: settings.model,
                total: function (data) {
                    return data.TotalCount;
                }
            },
            serverSorting: true,
            serverPaging: true,
            pageSize: settings.pagesize
        });

        //-- Page Size Choices
        var pageSizeChoices = [5, 10, 15];
        if ($.inArray(settings.pagesize, pageSizeChoices) == -1) {
            pageSizeChoices.push(settings.pagesize);
            pageSizeChoices.sort(function (a, b) { return a - b});
        }
        
        //-- Title Bar
        var titleBar = '<div class="grid-title" style="min-height:20px;">';

        //-- Build Export Link
        if (settings.showExport == true) {
            var exportUrl = settings.dataSourceURL;
            if (exportUrl.indexOf('?') > -1) {
                exportUrl += "&";
            } else {
                exportUrl += "?";
            }

            exportUrl += "export=" + settings.exportName;
            var exportAnchor = "<a href='" + exportUrl + "' target='_blank'>Export</a>";

            titleBar += '<div style="float:right">' + exportAnchor + "</div>";
        }

        titleBar += '<b>' + settings.title + '</b>';
        titleBar += '</div>';
        
        //-- If MultiSelect add a checkbox to the first col
        if (settings.multiSelectable === true) {
            settings.columns.splice(1, 0, { field: "check_row", title: " ", width: 30, template: "<input class='check_row' type='checkbox' />" });
        }

        //-- Kendo Grid
        var grid = $(this).kendoGrid({
            dataSource: gridDataSource,
            pageable: {
                refresh: true,
                pageSize: settings.pagesize,
                pageSizes: pageSizeChoices
            },
            sortable: true,
            resizable: true,
            selectable: (settings.rowSelectable === true) ? "row" : "",
            change: function (arg) {
                var selected = $.map(this.select(), function (item) {
                    return $(item).find('td').first().text();
                });
                grid.attr('data-sympleTech-KendoGrid-selected', selectedVals);
                settings.onRowSelected(selected[0]);

            },
            toolbar: titleBar,
            columns: settings.columns,
            dataBound: function (e) {
                //Hide The First Column (the primary Key )
                //Have to do this so you can then read it on the row select
                grid.find(".k-grid-header colgroup col").first().hide();
                grid.find(".k-grid-content colgroup col").first().hide();
                grid.find("thead th").first().hide();
                grid.find(".k-grid-content tbody tr").each(function () {
                    $(this).find('td').first().hide();
                    if (settings.rowSelectable == true) {
                        $(this).addClass('hoverable');
                    }
                });
                
                //Remove any null entries
                grid.find(".k-grid-content tbody tr td").each(function () {
                    if($(this).html() == "null") {
                        $(this).html("");
                    }
                });
            }
        });

        //Bind reload Grid to the form post
        var gData = grid.data("kendoGrid");
        if (settings.searchForm != "") {
            $("#" + settings.searchForm).submit(function (e) {
                e.preventDefault();
                gData.dataSource.read();
            });
        }

        //-- If MultiSelect add on click to the checkbox to select checked rows
        if (settings.multiSelectable === true) {
            $('.check_row').live('click', function (e) {
                gData.tbody.find('tr').removeClass('k-state-selected');
                var checkedBoxes = gData.tbody.find(".check_row:checked");
                var selectedVals = [];
                $(checkedBoxes).each(function () {
                    var selectedRow = $(this).parents("tr:first");
                    var rowID = $(selectedRow).find('td:first').text();
                    selectedVals.push(rowID);
                    selectedRow.addClass('k-state-selected');
                });
                grid.attr('data-sympleTech-KendoGrid-selected', selectedVals);

                settings.onRowSelected(selectedVals);
            });
        }

        return grid;
    });
};