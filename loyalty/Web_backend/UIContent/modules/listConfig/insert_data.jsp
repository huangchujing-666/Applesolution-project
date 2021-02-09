<%@ include file="../../common/header/db_header.jsp"%>
<%@ include file="../../common/header/check_login.jsp"%>
<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="utf-8"%>
<%@ page import="java.util.Date,java.util.*,com.embraiz.bo.core.*,com.embraiz.dao.core.*,java.text.SimpleDateFormat,org.apache.commons.codec.digest.DigestUtils" %>
<%
    request.setCharacterEncoding("utf-8");
    String itemid = request.getParameter("itemid");
    String itemname = request.getParameter("itemname");
     ListConfig listConfig=null;
     ListConfigDao listConfigDao=(ListConfigDao)ctx.getBean("listConfigDao",ListConfigDao.class);
     listConfig=listConfigDao.getlist(itemid==null?"":itemid).size()>0?listConfigDao.getlist(itemid==null?"":itemid).get(0):new ListConfig();
   	listConfig.setItemName(itemname);
    if(itemid!=null&&!itemid.equals("")&&!itemid.equals("null")){
        listConfigDao.update(listConfig);
        out.println("{success:true,url:'',itemid:'"+itemid+"',itemname:'"+itemname+"'}");
    }else{
    long keyId = listConfigDao.insert(listConfig);  
    if(keyId>0){
      listConfig.setItemId(Integer.parseInt(String.valueOf(keyId)));
      out.println("{success:true,");
      out.println("url:'',");
      out.println("itemid:'"+keyId+"',itemname:'"+itemname+"'}");
    }
    }

    
%>
<%@ include file="../../common/header/db_footer.jsp"%>