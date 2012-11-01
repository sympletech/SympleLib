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
        'showExport': false,
        'height': null,
        'primaryKey' : 'id',
        'dataBound' : function (e) { }
    }, options);

    return this.each(function () {

        //Bind the datasource to the linked form
        var formParams = {};
        if (settings.searchForm != "") {
            var $form = $("#" + settings.searchForm);

            //Serialize the form into an array
            var formData = $form.serialize().split('&');
            _.each(formData, function (formItem) {
                var nvp = formItem.split('=');

                //Set the form param to a function to look up the current value of the element
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
            settings.columns.splice(0, 0, { field: "check_row", title: " ", width: 30, template: "<input class='check_row' type='checkbox' />" });
        }

        //-- Kendo Grid
        var $kGrid = $(this);
        var grid = $kGrid.kendoGrid({
            dataSource: gridDataSource,
            pageable: {
                refresh: true,
                pageSize: settings.pagesize,
                pageSizes: pageSizeChoices
            },
            sortable: true,
            resizable: true,
            height: settings.height,
            selectable: (settings.rowSelectable === true) ? "row" : "",
            change: function (arg) {
                var selected = $.map(this.select(), function (item) {
                    var $item = $(item);
                    if ($item.hasClass('sympletech-noclick')) {
                        $item.removeClass('k-state-selected');
                    } else {
                        return $(item).attr('data-sympletech-kendogrid-rowid');
                    } 
                });
                if (selected[0] != null) {
                    $kGrid.attr('data-sympleTech-KendoGrid-selected', selected[0]);
                    settings.onRowSelected(selected[0]);
                }
            },
            toolbar: titleBar,
            columns: settings.columns,
            dataBound: function (e) {
                //Go Through Each visible row and add a data property with the unique ID from the dataset
                $kGrid.find(".k-grid-content tbody tr").each(function () {
                    var $tr = $(this);

                    //Get the kendo uid and then match it to the entry in the dataset
                    var uid = $tr.attr("data-uid");
                    var data_entry = _.find(gridDataSource._data, function (data_source) {
                        return data_source.uid === uid;
                    });
                    var id = eval('data_entry.' + settings.primaryKey);
                    $tr.addClass('kendo-data-row').attr("data-sympleTech-KendoGrid-rowid", id);

                    //Remove any null entries (otherwise it displays NULL in the field)
                    $tr.find('td').each(function () {
                        if ($(this).html() == "null") {
                            $(this).html("");
                        }
                    });
                });

                if (settings.multiSelectable === true) {
                    //Mark any selected rows as selected (persists selections from page to page)
                    var selectedRowIds = $kGrid.attr('data-sympleTech-KendoGrid-selected');
                    if (selectedRowIds != null) {
                        var selectedRowIdArray = selectedRowIds.split(',');
                        var visibleRows = $kGrid.find('.kendo-data-row');
                        $(visibleRows).each(function () {
                            $row = $(this);
                            var rowID = $row.attr('data-sympleTech-KendoGrid-rowid');
                            if (_.contains(selectedRowIdArray, rowID)) {
                                $row.addClass('k-state-selected');
                                $row.find('.check_row').attr('checked', 'checked');
                            }
                        });
                    }
                }

                if (settings.rowSelectable == true) {
                    $kGrid.find(".k-grid-content tbody tr")
                        .css('cursor', 'pointer')
                        .hover(
                            function () {
                                $(this).addClass('k-grid-hover');
                            },
                            function () {
                                $(this).removeClass('k-grid-hover');
                            }
                        );
                }

                //Get the current column Count
                var colCount = $("#data-grid").find('.k-grid-header colgroup > col').length;

                //If There are no results place an indicator row
                if (gridDataSource._view.length == 0) {
                    $kGrid.find('.k-grid-content tbody')
                        .append('<tr class="kendo-data-row sympletech-noclick"><td colspan="' + colCount + '" style="text-align:center"><b>No Results Found!</b></td></tr>');
                }

                //Get visible row count
                var rowCount = $kGrid.find('.k-grid-content tbody tr').length;

                //If the row count is less that the page size add in the number of missing rows
                if (rowCount < gridDataSource._take) {
                    var addRows = gridDataSource._take - rowCount;
                    for (var i = 0; i < addRows; i++) {
                        $kGrid.find('.k-grid-content tbody')
                            .append('<tr class="kendo-data-row sympletech-noclick"><td>&nbsp;</td></tr>');
                    }
                }

                //Call the user function presented for on databound
                settings.dataBound(e, gridDataSource);
            }
        });

        //Bind reload Grid to the form post
        var gData = grid.data("kendoGrid");
        if (settings.searchForm != "") {
            $("#" + settings.searchForm).submit(function (e) {
                e.preventDefault();
                gData.dataSource.page(1);
                gData.dataSource.read();
            });
        }

        //-- If MultiSelect add on click to the checkbox to select checked rows
        if (settings.multiSelectable === true) {
            $('.check_row').live('click', function (e) {
                //Get Current Selected Values
                var selectedVals = [];
                var selectedRowIds = $kGrid.attr('data-sympleTech-KendoGrid-selected');
                if (selectedRowIds != null) {
                    selectedVals = selectedRowIds.split(',');
                }

                var $row = $(this).parents('.kendo-data-row').first();
                var rowId = $row.attr('data-sympleTech-KendoGrid-rowid');
                if ($(this).is(':checked')) {
                    $row.addClass('k-state-selected');
                    selectedVals.push(rowId);
                } else {
                    $row.removeClass('k-state-selected');
                    selectedVals = _.without(selectedVals, rowId);
                }

                //Set selected values to a custom data attribute on the grid
                selectedVals = _.without(selectedVals, '', null);
                grid.attr('data-sympleTech-KendoGrid-selected', selectedVals);

                //Call the on selected function set by caller
                settings.onRowSelected(selectedVals);
            });
        }

        return grid;
    });
};