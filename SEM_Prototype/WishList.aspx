<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="WishList.aspx.cs" Inherits="SEM_Prototype.WishList" %>

<asp:Content ID="WishList" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>

        <div>
            <img src="img/wishlist/wishlist_header.png" alt="" class="menu-gallery-header-bg" />

            <div id="wishlist-header-text" class="wishlist-header-text">
                WishList
            </div>
            <p class="wishlist-header-quoto">100% blended fruit juice concentrate specifically designed for childcare centers</p>
            <!-- Continue Add Menu -->
            <asp:Button ID="btnContinueWL" runat="server" Text="View Menu" CssClass="btn-wlMenu" OnClick="btnContinueWL_Click" />
        </div>


        <div class="wl-container">  
            <div class="wishlist-content">
                

                <!-- WishList Table -->
                <div class="wl-table-box">

                    <!-- Add to Cart Button -->
                    <asp:Button ID="addToCartBtn" runat="server" Text="Add To Cart" CssClass="float-right menu-to-cart-btn add-btn-medium" CommandArgument='<%# Eval("MenuId")%>' CommandName="addtocart" OnClick="addToCartBtn_Click" AutoPostback = false/>
                    
                    <br />
                    <br />
                    <br />

                    <!-- table -->
                    <asp:GridView ID="gvWishList" runat="server" AutoGenerateColumns="False" DataKeyNames="WishlistId" ShowHeaderWhenEmpty="True" CssClass="gv-wishList" AllowPaging="true" PageSize="5" OnRowDeleting="gvWishList_RowDeleting">
                        <PagerSettings Mode="NextPreviousFirstLast" Visible="false" />
                        <Columns>
                            <%-- WishList ID Col--%>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWishlistID" Text='<%# Eval("WishListId") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Checkbox --%>
                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="checkBoxHead" AutoPostBack="true" OnCheckedChanged="CheckBoxHead_CheckedChanged" runat="server" />
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItems" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView MenuImage --%>
                            <asp:TemplateField HeaderText="Menu" ItemStyle-Width="20%" HeaderStyle-Width="20%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                        <asp:ImageButton ID="wl_menuImg" runat="server"  ImageUrl='<%# Eval("MenuImage")%>' Height="160" Width="160" OnClick="wl_menuImg_Click" CommandArgument='<%# Eval("MenuId")%>' CommandName="viewdetails" CssClass="menu-menu"/>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView MenuName & Desc --%>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-Width="40%" ItemStyle-Width="40%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="wl_menuName" runat="server"> 
                                <h3><%#Eval("MenuName")%></h3>
                                    </asp:Label>
                                    <br />
                                    <asp:Label ID="wl_menuDes" runat="server"> 
                                <%#Eval("MenuDescription")%>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView Menu Price --%>
                            <asp:TemplateField HeaderText="Price" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    RM <asp:Label ID="wl_price" Text='<%# Eval("Price", "{0:0.00}") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- GridView Data Added --%>
                            <asp:TemplateField HeaderText="Date Added" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="wl_dateAdded" Text='<%# Eval("DateAdded", "{0:dd/MM/yyyy}") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <%-- Action --%>
                            <asp:TemplateField HeaderText=" " ItemStyle-Width="20%" HeaderStyle-Width="20%" HeaderStyle-Height="50px" HeaderStyle-Font-Size="Large" HeaderStyle-BackColor="#484848" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton class="wl_page_deleteBtn" ImageUrl="img/wishlist/icons8-delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="40px" Height="40px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
            </div>
        </div>
        <!-- Paging -->
        <table class="paging" id="paging">
            <tr>
                <td>
                    <asp:Button ID="btnFirstWL" runat="server" Text="FIRST" OnClick="btnFirstWL_Click" CssClass="page-btn" />
                </td>

                <td>
                    <asp:Button ID="btnPreviousWL" runat="server" Text="PREV" OnClick="btnPreviousWL_Click" CssClass="page-btn" />
                </td>

                <td>
                    <asp:Button ID="btnNextWL" runat="server" Text="NEXT" OnClick="btnNextWL_Click" CssClass="page-btn" />
                </td>

                <td>
                    <asp:Button ID="btnLastWL" runat="server" Text="LAST" OnClick="btnLastWL_Click" CssClass="page-btn" />
                </td>
            </tr>
        </table>

    </section>
</asp:Content>
