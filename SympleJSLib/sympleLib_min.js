/*
 * JSMin
 * Javascript Compressor
 * http://www.crockford.com/
 * http://www.smallsharptools.com/Projects/Packer/
*/

// Source\sympleTech_Ajax.js

$.fn.sympleTech_ajaxlink=function(options){var settings=$.extend({'httpMethod':'GET','showHideLoaderMethod':function(show){},'onComplete':function(data){}},options);return this.each(function(){var href=$(this).attr('href');$(this).click(function(e){e.preventDefault();settings.showHideLoaderMethod(true);$.ajax({url:href,type:settings.httpMethod,success:function(data){settings.onComplete(data);settings.showHideLoaderMethod(false);}});});});};$.fn.sympleTech_ajaxform=function(options){var settings=$.extend({'showHideLoaderMethod':function(show){},'onComplete':function(data){}},options);return this.each(function(){$(this).submit(function(e){e.preventDefault();settings.showHideLoaderMethod(true);var href=$(this).attr('action');var postData=$(this).serialize();$.post(href,postData,function(data){settings.onComplete(data);settings.showHideLoaderMethod(false);});});});};
// Source\sympleTech_KendoGrid.js

$.fn.sympleTech_KendoGrid=function(options){var settings=$.extend({'dataSourceURL':'','model':{id:'id',fields:{}},'columns':[],'pagesize':10,'rowSelectable':true,'onRowSelected':function(id){},'searchForm':''},options);return this.each(function(){var gridDataSource=new kendo.data.DataSource({transport:{read:{url:settings.dataSourceURL,type:"POST"}},schema:{data:function(data){return data.Items;},model:settings.model,total:function(data){return data.TotalCount;}},serverSorting:true,serverFiltering:true,serverPaging:true,pageSize:settings.pagesize});var grid=$(this).kendoGrid({dataSource:gridDataSource,pageable:{refresh:true,pageSizes:true},sortable:true,selectable:(settings.rowSelectable==true)?"row":"",change:function(arg){var selected=$.map(this.select(),function(item){return $(item).find('td').first().text();});settings.onRowSelected(selected);},columns:settings.columns,dataBound:function(e){$(".k-grid .k-grid-header colgroup col").first().remove();$(".k-grid .k-grid-content colgroup col").first().remove();$(".k-grid thead th").first().hide();$(".k-grid-content tbody tr").each(function(){$(this).find('td').first().hide();if(settings.rowSelectable==true){$(this).addClass('hoverable');}});}});if(settings.searchForm!=''){var sForm=$("#"+settings.searchForm);sForm.submit(function(e){e.preventDefault();var formData=sForm.serialize().split('&');var params={page:1,pageSize:settings.pagesize};$(formData).each(function(){var nvp=this.split('=');params[nvp[0]]=nvp[1];});grid.data("kendoGrid").dataSource.query(params);});}
return grid;});};
