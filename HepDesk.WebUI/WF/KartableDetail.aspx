﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KartableDetail.aspx.cs" Inherits="Helpdesk.WebUI.WF.KartableDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="ucl" TagName="Detail" Src="~/UserControls/_KartableDetails.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div><ucl:Detail  ID="detail" runat="server" /></div>
    </form>
</body>
</html>
