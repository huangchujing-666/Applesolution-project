
<%@page import="java.lang.reflect.Method"%><%@ include file="../../common/header/db_header.jsp"%>
<%@ include file="../../common/header/check_login.jsp"%>
<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="utf-8"%>
<%@ page import="java.util.*,com.embraiz.bo.core.*,com.embraiz.dao.core.*,atg.taglib.json.util.JSONObject"%>
<%@ taglib prefix="json" uri="/WEB-INF/json.tld"%>
<%        
	request.setCharacterEncoding("utf-8");
	String method=request.getParameter("method");
	String className=request.getParameter("class");
	DealCenterDao dealCenterDao=(DealCenterDao)ctx.getBean("dealCenterDao",DealCenterDao.class);
	try{
		if(className==null||className.equals("")){
			CenterList centerList=new CenterList();
			Method f=centerList.getClass().getMethod(method);
		    f.invoke(centerList);
		    List<Object> list=dealCenterDao.getlist(centerList.dbtable,centerList.sorting,centerList);        
		   	StringBuffer outputStr = new StringBuffer();
		   	outputStr.append("{\"data\":[");
		    for(int i=0; i<list.size();i++){
		    	outputStr.append("{");
		     	f=list.get(i).getClass().getMethod(method);
		    	String d=(String)f.invoke(list.get(i));
		    	outputStr.append(d);
		    	if(i==list.size()-1){
		    		outputStr.append("}]}");
		    	}else{
		    		outputStr.append("},");
		    	}
		    }
		    if(list.size()==0){
		    	outputStr.append("]}");
		    }
		    out.println(outputStr);
	    }else{
	    	try{
	    	Class<?> module = Class.forName("com.embraiz.bo.core."+className);
			Object t = module.newInstance();
			Method f=t.getClass().getMethod(method);
			f.invoke(t);
			String table="";
			String sorting="";
			table=(String)t.getClass().getField("dbtable").get(t);
			sorting=(String)t.getClass().getField("sorting").get(t);
			List<Object> list=dealCenterDao.getlist(table,sorting,t);
			StringBuffer outputStr = new StringBuffer();
		   	outputStr.append("{\"data\":[");
		    for(int i=0; i<list.size();i++){
		    	outputStr.append("{");
		     	f=list.get(i).getClass().getMethod(method);
		    	String d=(String)f.invoke(list.get(i));
		    	outputStr.append(d);
		    	if(i==list.size()-1){
		    		outputStr.append("}]}");
		    	}else{
		    		outputStr.append("},");
		    	}
		    }
		    if(list.size()==0){
		    	outputStr.append("]}");
		    }
		    out.println(outputStr);
			}catch(Exception e){
				out.println("{\"items\":[]}");
			}
	    }
    }catch(Exception e){
    	out.println("{\"items\":[]}");
    }
%>

<%@ include file="../../common/header/db_footer.jsp"%>