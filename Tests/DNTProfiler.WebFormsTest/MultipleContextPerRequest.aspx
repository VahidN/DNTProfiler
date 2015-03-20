<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MultipleContextPerRequest.aspx.cs" Inherits="DNTProfiler.WebFormsTest.MultipleContextPerRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    Products:<asp:Label ID="lblProducts" runat="server" />
    <br />
    Categories:<asp:Label ID="lblCategories" runat="server" />
</asp:Content>
