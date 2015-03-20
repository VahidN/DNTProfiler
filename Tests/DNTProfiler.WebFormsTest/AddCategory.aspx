<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCategory.aspx.cs" Inherits="DNTProfiler.WebFormsTest.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Category</legend>
        <div class="editor-label">
            Name:
        </div>
        <div class="editor-field">
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        </div>
        <div class="editor-label">
            Title:
        </div>
        <div class="editor-field">
            <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
        </div>
        <p>
            <asp:Button ID="btnAdd" runat="server" Text="Create" onclick="btnAdd_Click" />
        </p>
    </fieldset>
</asp:Content>
