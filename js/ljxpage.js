 
//设置弹出窗口 divname为div的名字，titlestr 为标题名称， closeflag为初始显示还是关闭标志，false为关闭
function setwin(divname,titlestr,closeflag) {
    $('#' + divname).dialog({
        closed: closeflag,
        title: titlestr
    });
}

//关闭窗口  divname为div的名字
function closewin(divname) {
    $('#' + divname).dialog('close');   

}

//关闭窗口  divname为div的名字
function openwin(divname) {
    $('#' + divname).dialog('open');
}
//设置单列下拉框绑定  comboxname的名字， myurl为读取数据的处理程序地址，valuename为选择的字段名称，textname为显示的字段名称
function setcombobox(comboxname,myurl,valuename,textname) {
    $('#' + comboxname).combobox({
        url: myurl,
        valueField: valuename,
        textField: textname
    });
}
//设置显示多列下拉框的绑定，多选， comboxname的名字， myurl为读取数据的处理程序地址，valuename为选择的字段名称，text1为显示的字段名称，从1到10个只能少不能多。
function setcombogridm(comboxname, myurl, valuename, text1, text2, text3, text4, text5, text6, text7, text8, text9, text10) {
    
    $('#' + comboxname).combogrid({
        panelWidth: 280,
        multiple: true,
        idField: valuename,
        textField: valuename,
        url: myurl,
        columns: [[
            { field: 'id', checkbox: true },
            { field: valuename, title: valuename, width: 100 },
            { field: text1, title: text1, width: 100 },
            { field: text2, title: text2, width: 100 },
            { field: text3, title: text3, width: 100 },
            { field: text4, title: text4, width: 100 },
            { field: text5, title: text5, width: 100 },
            { field: text6, title: text6, width: 100 },
            { field: text7, title: text7, width: 100 },
            { field: text8, title: text8, width: 100 },
            { field: text9, title: text9, width: 100 },
            { field: text10, title: text10, width: 100 }
        ]]
    });
}
//设置多列下拉框，单选， comboxname的名字， myurl为读取数据的处理程序地址，valuename为选择的字段名称，text1为显示的字段名称，从1到10个只能少不能多。
function setcombogrids(comboxname, myurl, valuename, text1, text2, text3, text4, text5, text6, text7, text8, text9, text10) {

    $('#' + comboxname).combogrid({
        panelWidth: 280,
        multiple: false,
        idField: valuename,
        textField: valuename,
        url: myurl,
        columns: [[
            { field: valuename, title: valuename, width: 100 },
            { field: text1, title: text1, width: 100 },
            { field: text2, title: text2, width: 100 },
            { field: text3, title: text3, width: 100 },
            { field: text4, title: text4, width: 100 },
            { field: text5, title: text5, width: 100 },
            { field: text6, title: text6, width: 100 },
            { field: text7, title: text7, width: 100 },
            { field: text8, title: text8, width: 100 },
            { field: text9, title: text9, width: 100 },
            { field: text10, title: text10, width: 100 }
        ]]
    });
}
 
//初始化搜索框 searchdivname 为div的名字，datagridname为显示的datagrid名字。
function InitSearch(searchdivname,datagridname) {
    $("#" + searchdivname).searchbox({
        width: 200,
        iconCls: 'icon-save',
        searcher: function (val, name) {
            $('#' + datagridname).datagrid('options').queryParams.search_type = name;
            $('#' + datagridname).datagrid('options').queryParams.search_value = val;
            $('#' + datagridname).datagrid('reload');
        },
        prompt: '请输入要查询的账号'
    });
}
 
//初始化表格 datagridname为显示的datagrid名字，titlename为标题， urlstr读取的处理程序地址。text1为显示的字段名称，从1到15个只能少不能多。
function InitGird1(datagridname, titlename, urlstr, text1, text2, text3, text4, text5, text6, text7, text8, text9, text10, text11, text12, text13, text14, text15) {
    var mywidth = 0.5;
    if (text2 == undefined)
        mywidth = 1;
    else if (text3 == undefined)
        mywidth = 0.4;
    else if (text4 == undefined)
        mywidth = 0.32;
    else if (text5 == undefined)
        mywidth = 0.24;
    else if (text6 == undefined)
        mywidth = 0.19;
    else if (text6 == undefined)
        mywidth = 0.14;
    else if (text7 == undefined)
        mywidth = 0.13;
    else if (text8 == undefined)
        mywidth = 0.12;
    else if (text9 == undefined)
        mywidth = 0.1;
    else if (text10 == undefined)
        mywidth = 0.09;
    else if (text11 == undefined)
        mywidth = 0.08;
    else if (text12 == undefined)
        mywidth = 0.07;
    else if (text13 == undefined)
        mywidth = 0.07;
    else if (text14 == undefined)
        mywidth = 0.066;
    else if (text15 == undefined)
        mywidth = 0.06;

    $('#' + datagridname).datagrid({
        title: titlename, //表格标题
        url: urlstr, //请求数据的页面   sortName: '产品名称', //排序字段
       
        idField: 'Id', //标识字段,主键
        iconCls: '', //标题左边的图标
        width: '100%', //宽度
        singleSelect: true,
        nowrap: false, //是否换行，True 就会把数据显示在一行里
        striped: true, //True 奇偶行使用不同背景色
        collapsible: false, //可折叠   sortOrder: 'desc', //排序类型
        
        remoteSort: false, //定义是否从服务器给数据排序
        columns: [[
            { field: text1, title: text1, width: $(this).width()*mywidth },
            { field: text2, title: text2, width: $(this).width() * mywidth },
            { field: text3, title: text3, width: $(this).width() * mywidth },
            { field: text4, title: text4, width: $(this).width() * mywidth },
            { field: text5, title: text5, width: $(this).width() * mywidth },
            { field: text6, title: text6, width: $(this).width() * mywidth },
            { field: text7, title: text7, width: $(this).width() * mywidth },
            { field: text8, title: text8, width: $(this).width() * mywidth },
            { field: text9, title: text9, width: $(this).width() * mywidth },
            { field: text10, title: text10, width: $(this).width() * mywidth },
            { field: text10, title: text11, width: $(this).width() * mywidth },
            { field: text10, title: text12, width: $(this).width() * mywidth },
            { field: text10, title: text13, width: $(this).width() * mywidth },
            { field: text10, title: text14, width: $(this).width() * mywidth },
            { field: text10, title: text15, width: $(this).width() * mywidth }
        ]],
        queryParams: { "action": "query" },
        pagination: true, //是否开启分页
        pageNumber: 1, //默认索引页
        pageSize: 20, //默认一页数据条数
        rownumbers: true //行号
    });

}


//编辑窗口 datagridname为显示的datagrid名字，  urlstr读取的处理程序地址。columns为编辑的控件列表字符串， divname为div的名字
function editdata(datagridname,urlstr,columns,divname) {
    var node = $('#' + datagridname).datagrid('getSelected');
    if (node) {  
        $.post(urlstr + '?type=edit&Id=' + node.Id, function (msg) {
            var strret = msg.split(',');
            var arr = columns.split(',');
            var i=0;
            for (i = 0; i < strret.length; i++) {
                if (arr[i].indexOf("combobox") >= 0)
                    $('#' + arr[i]).combobox('setValue', strret[i]);
                else if (arr[i].indexOf("combogrid") >= 0)
                    $('#' + arr[i]).combogrid('setValue', strret[i]);
                else if (arr[i].indexOf("time") >= 0)
                    $('#' + arr[i]).datebox('setValue', strret[i]);
                else
                    $('#' + arr[i]).val(strret[i]);
            }
        });
        $('#Id').val(node.id);
        openwin(divname);
    } else {
        msgShow('系统提示', '请选择要编辑的记录', 'error');
    }
}

//根据id删除数据 datagridname为显示的datagrid名字，  urlstr读取的处理程序地址。
function deldata(datagridname,urlstr) {
    var node = $('#' + datagridname).datagrid('getSelected');
    if (node) {         
        $.messager.confirm('系统提示', '删除后不可恢复，确定要删除？', function (i) {
            if (i) {
                $.post(urlstr+'?type=del&Id=' + node.Id, function (msg) {
                    msgShow('系统提示', '恭喜，删除成功！', 'info');
                    $('#' + datagridname).datagrid('reload');
                });
            }
        })
    } else {
        msgShow('系统提示', '请选择要编辑的权限', 'error');
    }
}
//保存数据 datagridname为显示的datagrid名字，  urlstr读取的处理程序地址。 columns为编辑的控件列表字符串， divname为div的名字
function savedata(datagridname, urlstr, columns, divname) {
    var strsend = '';
    var arr = columns.split(',');
    var i = 0;
    strsend = "Id=" + encodeURI($('#Id' ).val());
    for (i = 0; i < arr.length; i++) {
        if (strsend != '')
            strsend = strsend + '&';
        if (arr[i] == "Id")
            continue;
        else if (arr[i].indexOf("combobox") >= 0)
            strsend = strsend + "" + arr[i] + "=" + encodeURI($('#' + arr[i]).combobox('getValue'));
        else if (arr[i].indexOf("combogrid") >= 0) 
            strsend = strsend + "" + arr[i] + "=" + encodeURI($('#' + arr[i]).combogrid('getValues'));
        else if (arr[i].indexOf("time") >= 0)
            strsend = strsend + "" + arr[i] + "=\"" + encodeURI($('#' + arr[i]).datebox('getValue'))+"\"";
        else
            strsend = strsend + "" + arr[i] + "=" + encodeURI($('#' + arr[i]).val());
    }
    $.post(urlstr + '?type=save&' + strsend, function (msg) {
        msgShow('系统提示', '保存成功', 'info');
            $('#' + divname).dialog('close');
            $('#' + datagridname).datagrid('reload');
        });

        //var strsend = '{';
        //var arr = columns.split(',');
        //var i = 0;
        //for (i = 0; i < arr.length; i++) {
        //    if (strsend != '{')
        //        strsend = strsend + ',';
        //    if (arr[i].indexOf("combobox") >= 0)
        //        strsend = strsend + "'" + arr[i] + "':" + encodeURI($('#'+arr[i]).combobox('getValue'));
        //    else if (arr[i].indexOf("combogrid") >= 0)
        //        strsend = strsend + "'" + arr[i] + "':" + encodeURI($('#' + arr[i]).combogrid('getValues'));
        //    else if (arr[i].indexOf("date") >= 0)
        //        strsend = strsend + "'" + arr[i] + "':" + encodeURI($('#' + arr[i]).datebox('getValue'));
        //    else
        //        strsend = strsend + "'" + arr[i] + "':" + encodeURI($('#' + arr[i]).val());
        //}
        //strsend = strsend + '}';
        //alert(strsend);
    //alert('txt_name:'+encodeURI('sdf'));
        //var a1 = encodeURI('sdf中国人12');
        //var a2 = encodeURI('sdfas11好啊12');
        //var a3 = encodeURI('1');
        //var a4 = encodeURI('sdfsa12');

        //$.ajax({
        //    url: urlstr + '?type=save',
        //    type: 'post',
        //    data:{'txt_name':a1,'combogrid_gg':a2,'txt_sl':a3,'txt_beizhu':a4} ,//{'txt_name':encodeURI('sdf'),'combogrid_gg':encodeURI('sdfas'),'txt_sl':encodeURI('21'),'txt_beizhu':encodeURI('sdfsa')}    { 'txt_name': 'sdf', 'combogrid_gg': 'sdfas', 'txt_sl': '21', 'txt_beizhu':'sdfsa' }
        //    success: function (msg) {
        //        if (msg != 'error') {
        //            msgShow('系统提示', '保存成功', 'info');
        //            $('#' + divname).dialog('close');
        //            $('#' + datagridname).datagrid('reload');
        //        }
        //        else
        //            msgShow('系统提示', 'error', 'info');
        //    }
        //});

}






