<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="DNTProfiler.WebFormsTest.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Product</legend>
        <div class="editor-label">
            Category:
        </div>
        <div class="editor-field">
            <asp:DropDownList ID="ddlCategories" runat="server">
            </asp:DropDownList>
        </div>
        <div class="editor-label">
            Name:
        </div>
        <div class="editor-field">
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        </div>
        <div class="editor-label">
            Price:
        </div>
        <div class="editor-field">
            <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
        </div>
        <p>
            <asp:Button ID="btnAdd" runat="server" Text="Create" onclick="btnAdd_Click" />
        </p>
    </fieldset>
</asp:Content>
