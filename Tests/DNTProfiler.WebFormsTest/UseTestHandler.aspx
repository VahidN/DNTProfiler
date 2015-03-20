<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UseTestHandler.aspx.cs" Inherits="DNTProfiler.WebFormsTest.UseTestHandler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblSum" runat="server"></asp:Label>
    <script src="TestHandler.ashx" type="text/javascript"></script>
</asp:Content>
