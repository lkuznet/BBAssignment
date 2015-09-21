<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogManager.aspx.cs" Inherits="CatalogManager.WebWeb.CatalogManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h2>Catalog Manager</h2>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btnDisplayTree" runat="server" Text="Display Category Tree" OnClick="btnDisplayTree_Click" />
    
    </div>
        <div>
            
            <asp:Label ID="Label11" runat="server" Text="Categories:" Font-Bold="True"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label12" runat="server" Text="Products:" Font-Bold="True"></asp:Label>
            
        </div>
        <div>
        <asp:ListBox ID="lbTree" runat="server" Height="302px" Width="292px" AutoPostBack="True" OnSelectedIndexChanged="lbTree_SelectedIndexChanged" ></asp:ListBox>
        <asp:ListBox ID="lbChildren" runat="server" Height="302px" Width="280px" AutoPostBack="True" OnSelectedIndexChanged="lbChildren_SelectedIndexChanged"></asp:ListBox>
            </div>
        <div>
            <hr />
        <asp:Panel ID="Panel1" runat="server" Height="431px">
            <asp:Label ID="Label13" runat="server" Font-Bold="True" Text="Edit Category"></asp:Label>
            <br />
            <asp:HiddenField ID="hidId" runat="server" />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Label">Edit Name:</asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtCatName" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUpdateCategory" runat="server" OnClick="btnUpdateCategory_Click" style="margin-left: 0px" Text="Update Category" Width="111px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDeleteCategory" runat="server"  style="margin-left: 0px" Text="Delete Category" Width="111px" OnClick="btnDeleteCategory_Click" />
            <br />
            <hr />
            <asp:Label ID="Label2" runat="server" Text="Or Add Child Category:" Font-Bold="True"></asp:Label>
            <br />
            <asp:Label ID="Label4" runat="server" Text="Label">Category Name:</asp:Label>
            &nbsp;
            <asp:TextBox ID="txtNewCatName" runat="server"></asp:TextBox>
            <asp:Label ID="Label6" runat="server" Text="Label">to Parent:</asp:Label>
            &nbsp;
            <asp:TextBox ID="txtCatParent" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnAddCategory" runat="server" OnClick="btnAddCategory_Click" Text="Add Category" Width="110px" />
            <br /><hr />
            <asp:Label ID="Label3" runat="server" Text="Or Add Child Product:" Font-Bold="True"></asp:Label>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Label">Product Name:</asp:Label>
            &nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtProdName" runat="server" Height="16px"></asp:TextBox>
            <asp:Label ID="Label7" runat="server" Text="Label">to Parent:</asp:Label>
            &nbsp;
            <asp:TextBox ID="txtProdParent" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnAddProduct" runat="server" OnClick="btnAddProduct_Click" Text="Add Product" Width="107px" />
            <br />
            <asp:Label ID="Label8" runat="server" Text="Label">Description:</asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtProdDescr" runat="server" Height="16px"></asp:TextBox>
            <br />
            <asp:Label ID="Label9" runat="server" Text="Label">Price:</asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtProdPrice" runat="server" Height="16px"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUpdateProduct" runat="server"  Text="Update Product" Width="107px" OnClick="btnUpdateProduct_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDeleteProduct" runat="server" style="margin-left: 0px" Text="Delete Product" Width="111px" OnClick="btnDeleteProduct_Click" />
            &nbsp;<asp:HiddenField ID="hidProductId" runat="server" />
            <br/>
            
            <asp:Label ID="Label10" runat="server" Text="Label">Currency:</asp:Label>
            <asp:RadioButtonList ID="rbCurrency" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True">CAD</asp:ListItem>
                <asp:ListItem>USD</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <br />
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            <br/>
        </asp:Panel>
    </div>
            </form>
</body>
</html>
