<%@ Page Title="MultiViewSetup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MultiViewSetup.aspx.cs" Inherits="WebApp.SamplePages.MultiViewSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Multiview Control Setup</h1>
    <div class="row">
        <div class="offset-1">
            <asp:Label ID="Label1" runat="server" Text="Common Data or controls on the web page independent of the multiview control"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="offset-1">
            <asp:Menu ID="Menu1" runat="server" Font-Size="Large" Orientation="Horizontal" CssClass="tabs"
                StaticSelectedStyle-CssClass="selectedTab"
                StaticSelectedStyle-BackColor="LavenderBlush"
                StaticMenuStyle-HorizontalPadding="50px" OnMenuItemClick="Menu1_MenuItemClick">

                <Items>
                    <asp:MenuItem Selected="True" Text="Page 1" Value="0"></asp:MenuItem>
                    <asp:MenuItem Text="Page 2" Value="1"></asp:MenuItem>
                     <asp:MenuItem Text="Page 3" Value="2"></asp:MenuItem>
                </Items>
            </asp:Menu>
        </div>
    </div>
    <div class="row">
        <div class="offset-1">
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                    <asp:TextBox ID="IODataV1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button3" runat="server" Text="Button" OnClick="Button3_Click" />
                    <asp:Button ID="Button4" runat="server" Text="Button" OnClick="Button4_Click" />
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:TextBox ID="IODataV2" runat="server"></asp:TextBox>
                </asp:View>
                <asp:View ID="View3" runat="server">
                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                    <asp:TextBox ID="IODataV3" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                    <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>
