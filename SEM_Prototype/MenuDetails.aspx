<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="MenuDetails.aspx.cs" Inherits="SEM_Prototype.MenuWorks.MenuDetails" %>

<asp:Content ID="MenuDetails" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <img src="img/menu/art_about.jpg" class="menu-details-bg" />

    <div class="details-container">
        <div class="details-left">
            <asp:Image ID="dMenuDetailsImage" runat="server" CssClass="details-image" />
        </div>

        <div class="details-right">
            <%-- Name --%>
            <asp:Label ID="dMenuName" runat="server" CssClass="details-menuname"></asp:Label>
            <p class="details-price-str">PRICE</p>

            <%-- Price --%>
            <asp:Label ID="dMenuPrice" runat="server" CssClass="details-price"></asp:Label>

            <%-- Stock --%>
            <div class="details-stock-str">
                Current stock :
                <asp:Label ID="dMenuStock" runat="server"></asp:Label>
            </div>


            <hr style="margin: 20px 2px 0px 2px;" />

            <%-- Add To Cart  --%>
            <div style="justify-content: center; width: 100%">
                <table class="details-btn">

                    <tr>
                        <td>
                            <asp:ImageButton ID="dPlusControl" runat="server" ImageUrl="~/img/icon/plus.png" Height="50px" Width="50px" OnClick="dPlusControl_Click" /></td>
                        <td>
                            <asp:TextBox ID="detailsQtyControl" runat="server" CssClass="d-qty-control" Text="1"></asp:TextBox></td>
                        <td>
                            <asp:ImageButton ID="dMinusControl" runat="server" ImageUrl="~/img/icon/minus.png" Height="50px" Width="50px" OnClick="dMinusControl_Click" /></td>

                        <td>
                            <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="menu-to-cart-btn add-btn-large margin-l10" OnClick="addToCartBtn_Click" />
                        </td>

                    </tr>
                </table>
            </div>
            <br />
            <hr style="margin: 0px 2px 0px 2px;" />
        </div>

        <%-- Wishlist button & Close button --%>
        <div>
            <asp:ImageButton ID="detailsLoveBtn" runat="server" AlternateText="Add to WishList" ImageUrl="img/wishlist/heart-icon-inactive.png" CssClass="cancel-btn" CommandArgument='<%# Eval("MenuId")%>' CommandName="addtowishlist" OnClick="loveBtn_Click" />
            <asp:ImageButton ID="detailsCancelBtn" runat="server" ImageUrl="~/img/menu/icons8-cancel.png" OnClick="detailsCancelBtn_Click" CssClass="cancel-btn" />
        </div>
    </div>
</asp:Content>
