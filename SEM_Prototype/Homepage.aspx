<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="SEM_Prototype.Homepage" %>

<asp:Content ID="Homepage" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Background --%>
    <section class="banner" id="banner">
        <div class="content">
            <h2>Leader in Childcare Juice!</h2>
            <p>
                100% blended fruit juice concentrate specifically designed for childcare centers.
            </p>
            <a href="#menu" class='btn'>Our Menu</a>
        </div>

        <video src="img/video/fruit.mp4" muted loop autoplay></video>
    </section>

    <%-- About --%>
    <section class="about" id="about">
        <div class="row">
            <div class="col50">
                <h2 class="titleText"><span>A</span>bout Us</h2>
                <p> &emsp;&emsp;&emsp;
                    We provide a variety of flavors that all kids can enjoy! 
                    All the juice concentrates that we sell are compliant with 
                    the USDA and FDA guidelines. You don't need to refrigerate 
                    the concentrates before or after opening them. 
                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    If you're running a childcare center and want to serve healthy 
                    beverages to kids every day, order quality juice concentrates 
                    from Penny Juice of America. Get delicious and nutritious 100% 
                    fruit juice from Penny Juice of America. We provide 100% blended 
                    fruit juice concentrates that are specially made for kids and meet 
                    all the requirements of the FDA with regard to subsidy and nutrition. 
                    Customers prefer our products for their benefits, features, variety, 
                    service, and competitive pricing.
                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    Our system is fast and easy to use. Penny Juice greatly reduces 
                    storage and handling requirements. 1 gallon pre-measured mix 
                    pitchers available to assure cost control and flavor consistency.
                </p>
            </div>

            <div class="col50">
                <div class="imgBox">
                    <img src="img/homepage/aboutUs/aboutUs.jpg" alt="About Us">
                </div>
            </div>
        </div>
    </section>

    <%-- Artwork --%>
    <section class="menu" id="menu">
        <div class="title">
            <h2 class="titleText"><span>M</span>enu</h2>

            <p> 
                100% blended fruit juice concentrate specifically designed for childcare centers.
            </p>
        </div>

        <div class="content">
            <asp:DataList ID="MenuDataList" runat="server" DataKeyField="MenuId" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="60" HorizontalAlign="Center" CellPadding="3">
                <ItemTemplate>
                    <table id="artwork-table">
                        <tr>
                            <td>
                                <asp:Image ID="MenuImage" runat="server" CssClass="menu-gallery-image" ImageUrl='<%# Eval("MenuImage") %>' />
                            </td>
                        </tr>
                        <tr class="text-a1 padding-b15">
                            <td>
                                <asp:Label ID="MenuNameLabel" runat="server" Text='<%# Eval("MenuName") %>' />
                            </td>
                        </tr>
                        <tr class="text-a2">
                            <td>
                                <asp:Label ID="MenuDescriptionLabel" runat="server" Text='<%# Eval("MenuDescription") %>' />
                            </td>
                        </tr>
                        <tr class="text-a3">
                            <td>RM
                                <asp:Label ID="PriceLabel" runat="server" Text='<%# String.Format("{0:0.00}", Eval("Price"))  %>' />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </div>

        <div class="title">
            <asp:Button ID="btnViewAll" runat="server" CssClass="btnViewAll" Text="View All" OnClick="btnViewAll_Click" causesvalidation="false" />
        </div>
    </section>

    <%-- Contact --%>
    <section class="contact" id="contact">
        <div class="title">
            <h2 class="titleText"><span>C</span>ontact Us</h2>

            <p> 
                Feel free to Contact Us. 
            </p>
        </div>

        <asp:ScriptManager ID="scriptmanagerContact" runat="server"></asp:ScriptManager>  
        <div class="contactForm">
            <asp:UpdatePanel ID="updatepnl" runat="server">  
                <ContentTemplate> 
                    <h3>Send Message</h3>
                    <div class="inputBox">
                        <asp:TextBox ID="txtContactName" CssClass="contactInput" runat="server" placeholder="Your Name"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvContactName" runat="server" 
							ControlToValidate="txtContactName" ErrorMessage="Please Enter Your Name" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>
                    </div>

                    <div class="inputBox">
                        <asp:TextBox ID="txtContactEmail" CssClass="contactInput" runat="server" placeholder="Your Email"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvContactEmail" runat="server" 
							ControlToValidate="txtContactEmail" ErrorMessage="Please Enter Your Email" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>

						<asp:CustomValidator ID="cvContactEmail" runat="server" 
							ControlToValidate="txtContactEmail"
                            ClientValidationFunction="ValidateEmail"
							OnServerValidate="cvContactEmail_ServerValidate"
							ForeColor="Red"
							Display="Dynamic">
						</asp:CustomValidator>
                    </div>

                    <div class="inputBox">
                        <asp:TextBox ID="txtContactComment" TextMode="MultiLine" CssClass="contactInput" runat="server" placeholder="Your Comment"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="rfvContactComment" runat="server" 
							ControlToValidate="txtContactComment" ErrorMessage="Please Enter Your Message" 
							ForeColor="Red" Display="Dynamic">
						</asp:RequiredFieldValidator>
                    </div>

                    <div class="inputBox">
                        <asp:Button ID="btnContactSubmit" CssClass="contactSubmit" runat="server" Text="Submit" OnClientClick="changeSubmitText()"  OnClick="btnContactSubmit_Click"/>
                    </div>

                </ContentTemplate>  
            </asp:UpdatePanel> 
        </div>

    </section>

    <script type="text/javascript">
        function changeSubmitText() {
            document.getElementById('<%=btnContactSubmit.ClientID%>').value = "Sending";
            document.getElementById('<%=btnContactSubmit.ClientID%>').style.cursor = "default";
        }

        //Client-side validation
        function ValidateEmail(source, args) {
            var txtEmail = document.getElementById('<%=txtContactEmail.ClientID%>');
            var emailRegex = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g;

            if (emailRegex.test(txtEmail.value) == false) {
                alert('Invalid Email Format!');
                source.innerHTML = "Invalid Email Format!";
                args.IsValid = false;

            } else {
                args.IsValid = true;
            }
        }

    </script>

</asp:Content>
