<%@ include file="../../common/header/db_header.jsp"%>
<%@ include file="../../common/header/check_login.jsp"%>
<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="utf-8"%>
<%@ page import="java.util.Date,java.util.*,com.embraiz.bo.core.*,com.embraiz.dao.core.*,java.text.SimpleDateFormat,org.apache.commons.codec.digest.DigestUtils" %>
<%
    request.setCharacterEncoding("utf-8");
    String listid = request.getParameter("id");
    String name = request.getParameter("name");
    String value=request.getParameter("value");
    String itemid=request.getParameter("item");
     ListConfig listConfig=null;
     ListConfigDao listConfigDao=(ListConfigDao)ctx.getBean("listConfigDao",ListConfigDao.class);
     listConfig=listConfigDao.getreflist(listid==null?"":listid).size()>0?listConfigDao.getreflist(listid==null?"":listid).get(0):new ListConfig();
    if(itemid!=null&&!itemid.equals("")){
    listConfig.setItemId(Integer.valueOf(itemid));
    }
    if(name.equals("name")){
    listConfig.setListName(value);
    }else if(name.equals("namecn")){
    listConfig.setListNameCn(value);
    }else if(name.equals("sorting")){
    if(value!=null){
    listConfig.setSorting(value);
    }
    }
    if(listid!=null&&!listid.equals("")&&!listid.equals("null")){
    	if(name.equals("name")){
    	  listConfigDao.updatelist(listConfig);
	      out.println("{success:true,");
	      out.println("url:'',flag:0}");
    	}else if(name.equals("namecn")){
    	  listConfigDao.updatelistcn(listConfig);
	      out.println("{success:true,");
	      out.println("url:'',flag:0}");
    	}else if(name.equals("sorting")){
    	listConfigDao.updatesorting(listConfig);
	      out.println("{success:true,");
	      out.println("url:'',flag:0}");
    	}else{
	      out.println("{success:true,");
	      out.println("url:'',flag:0}");
    	}
    }else{
    long keyId = listConfigDao.insertlist(listConfig);  
    if(keyId>0){
      listConfig.setListId(Integer.parseInt(String.valueOf(keyId)));
      out.println("{success:true,");
      out.println("url:'',");
      out.println("flag:1,listid:'"+keyId+"'}");
    }
    }

    
%>
<%@ include file="../../common/header/db_footer.jsp"%>