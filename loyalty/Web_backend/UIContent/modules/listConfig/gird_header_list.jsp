<%@ include file="../../common/header/check_login.jsp"%>
<%@ page language="java" contentType="text/html; charset=utf-8"	pageEncoding="utf-8"%>
<%@ page import="java.util.*,com.embraiz.bo.core.*,com.embraiz.dao.core.*,java.text.SimpleDateFormat,org.apache.commons.codec.digest.DigestUtils" %>

{success: true,
columns: [ 
{header: 'List ID', dataIndex: 'list_id',width:350,type:'title'},
{header: 'List Name', dataIndex: 'list_name',width:350},
{header: 'Sorting', dataIndex: 'sorting',width:331},
],
fields: ['list_id','list_name','sorting','href','target'],
title: 'Item List Data',
sort:'', 
iconSrc:'icons/cog_go.png',
start:0, 
pageSize:20,
delete_url: 'delete.jsp',
add_url: "",
edit_hidden:false  
}
