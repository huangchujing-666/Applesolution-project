//<!--2009-9-25 18:42:27 DownPlus -- AD:zy-->

var adLeftSrc = "";
var adLeftHref = "";
var adLeftWidth = 20;
var adLeftHeight = 20;
var adRightSrc = "<img onclick='javascript:window.scrollBy(0,0);' border=\"0\" src=\"images/top.jpg\" />";
var adRightHref = "#";
var adRightWidth = 200;
var adRightHeight = 50;
var marginTop = 10;
var marginLeft = 50;
var navUserAgent = navigator.userAgent;
function load(){
judge();
move();
}
function move() {
judge();
setTimeout("move();",80)
}
var heights=0;
function judge(){
if (navUserAgent.indexOf("Firefox") >= 0 || navUserAgent.indexOf("Opera") >= 0) {
if (adLeftSrc != "") {document.getElementById("adLeftFloat").style.top = (document.body.scrollTop?document.body.scrollTop:document.documentElement.scrollTop) + ((document.body.clientHeight > document.documentElement.clientHeight)?document.documentElement.clientHeight:document.body.clientHeight) - adLeftHeight - marginTop + 'px';}
if (adRightSrc != "") {
heights=(document.body.scrollTop?document.body.scrollTop:document.documentElement.scrollTop) + ((document.body.clientHeight > document.documentElement.clientHeight)?document.documentElement.clientHeight:document.body.clientHeight) - adRightHeight - marginTop;

var clientwidth=(document.body.clientWidth > document.documentElement.clientWidth)?document.body.clientWidth:document.documentElement.clientWidth;
if(document.body.scrollHeight-heights>100){
document.getElementById("adRightFloat").style.top =heights  + 'px';
}
else
{document.getElementById("adRightFloat").style.top =document.body.scrollHeight-100  + 'px';}
/*
document.getElementById("adRightFloat").style.left = ((document.body.clientWidth > document.documentElement.clientWidth)?document.body.clientWidth:document.documentElement.clientWidth) - adRightWidth - marginLeft + 'px';
*/

document.getElementById("adRightFloat").style.left =clientwidth - adRightWidth - marginLeft + 'px';
} 
}
else{
if (adLeftSrc != "") {document.getElementById("adLeftFloat").style.top = (document.body.scrollTop?document.body.scrollTop:document.documentElement.scrollTop) + ((document.documentElement.clientHeight == 0)?document.body.clientHeight:document.documentElement.clientHeight) - adLeftHeight - marginTop + 'px';}
if (adRightSrc != "") {
heights=(document.body.scrollTop?document.body.scrollTop:document.documentElement.scrollTop) + ((document.documentElement.clientHeight == 0)?document.body.clientHeight:document.documentElement.clientHeight);
if((document.body.scrollHeight?document.body.scrollHeight:document.documentElement.scrollHeight) - heights>100)
{
document.getElementById("adRightFloat").style.top = heights - adRightHeight - marginTop + 'px';
}
else
{
document.getElementById("adRightFloat").style.top = heights - 100 + 'px';
}
/*
document.getElementById("adRightFloat").style.left = ((document.documentElement.clientWidth == 0)?document.body.clientWidth:document.documentElement.clientWidth) - adRightWidth - marginLeft + 'px';
*/
var ieclientwidth=(document.documentElement.clientWidth == 0)?document.body.clientWidth:document.documentElement.clientWidth;
//alert(ieclientwidth);
document.getElementById("adRightFloat").style.left = ieclientwidth - adRightWidth - marginLeft + 'px';
}
}
if (adLeftSrc != "") {document.getElementById("adLeftFloat").style.left = marginLeft + 'px';}
}
//document.write("<div id=\"adLeftFloat\" style=\"position: absolute;width:" + adLeftWidth + ";\"><a target=_blank href=\"" + adLeftHref +"\"><img src=\"" + adLeftSrc + "\"  width=\"" + adLeftWidth + "\" height=\"" + adLeftHeight + "\"  border=\"0\" \></a></div>");
//document.write("<div id=\"adRightFloat\" style=\"position: absolute;width:" + adRightWidth + ";\"><a href=\"http://www.win4000.cn/?tn=piaodown\" target=\"_blank\"><img src=\"http://www.piaodown.com/upload/2008ad/qq.gif\" width=\"50\" height=\"50\" border=\"0\"></a></div>");
document.write("<div id=\"adRightFloat\" style=\"position: absolute;width:" + adRightWidth + ";\"><a href=\"" + adRightHref +"\">" + adRightSrc + "</a></div>");
load();
