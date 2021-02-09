<%@ include file="../../common/header/db_header.jsp"%>
<%@ include file="../../common/header/check_login.jsp"%>
<%@ page language="java" contentType="text/html; charset=utf-8"
	pageEncoding="utf-8"%>
<%@ page import="java.util.*,com.embraiz.bo.core.*,com.embraiz.dao.core.*,atg.taglib.json.util.JSONObject"%>
<%@ taglib prefix="json" uri="/WEB-INF/json.tld"%>
<%        
        
        ListConfigDao listConfigDao=(ListConfigDao)ctx.getBean("listConfigDao",ListConfigDao.class);
        List<ListConfig> listConfig=listConfigDao.listWeekday();

        request.setAttribute("listConfig",listConfig);
%>
<json:object escapeXml="false">
<json:property name="success" value="true"></json:property>
<json:array name="data" var="item1" items="${listConfig}">
	<json:object>
		<json:property name="id" value="${item1.listId}" />
		<json:property name="value" value="${item1.listName}" />
	</json:object>
</json:array>
</json:object>
<%@ include file="../../common/header/db_footer.jsp"%>