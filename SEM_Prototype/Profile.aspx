<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="SEM_Prototype.Profile" %>

<asp:Content ID="Profile" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

        <asp:ScriptManager ID="scriptmanagerProfile" runat="server"></asp:ScriptManager>

        <div class="profileBar" id="profileBar">
            <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Conditional">  
			    <ContentTemplate> 
                    <div id="profileContent" class="profileContent" >
                        <div class="profileBarImgBox" id="profileBarImgBox">
                            <img id="userPic" src="img/userPic/user_default.png" alt="Profile Pic" />
                        </div>

                        <div class="editIcon">
                            <div id="editIconBox" class="editIconBox" onclick="showEditIconBox()"></div>
                        </div>

                        <div class="profileBarContent">
                            <div id="profileName">
                                <asp:Label ID="lblProfileName" runat="server" Text="testing"></asp:Label>
                            </div>

                            <div id="phoneNo">
                                <asp:Label ID="lblPhoneNo" runat="server" Text="testing"></asp:Label>
                            </div>

                            <div id="address">
                                <asp:Label ID="lblAddress" runat="server" Text="testing"></asp:Label>
                            </div>

                            <div id="userBio" class="userBio">
                                <textarea id="txtAreaUserBio" readonly="readonly" runat="server"></textarea>
                            </div>

                            <div id="editBio" class="editBio">
                                Edit Your BIO here:<br />
                                <textarea maxlength="50" id="txtAreaEditBio" runat="server"></textarea><br />
                                <label id="lblBioMaxLen" style="font-size: 12px;">Only 50 Character are Allowed</label>
                            </div>

                            <div id="profileBarBtnStyle">
                                <div style="display: inline; width: 350px;">
                                    <asp:Button ID="btnEditBio" CssClass="editBioBtnStyle" runat="server" Text="Edit Bio" OnClick="btnEditBio_Click"></asp:Button>
                                    <asp:Button ID="btnCancelEditBio" CssClass="editBioBtnStyle" runat="server" Text="Cancel" OnClick="btnCancelEditBio_Click"/>
                                </div>
                                
                                <asp:Button ID="btnEditPhone" CssClass="profileBtn profileBarBtnStyle" runat="server" Text="Edit Phone No" OnClientClick="showPhoneNoEditBox()" OnClick="btnEdit_Click"></asp:Button>
                                <asp:Button ID="btnEditAddress" CssClass="profileBtn profileBarBtnStyle" runat="server" Text="Edit Address" OnClientClick="showAddressEditBox()" OnClick="btnEdit_Click"></asp:Button>
                                <asp:Button ID="btnEdit" CssClass="profileBtn profileBarBtnStyle" runat="server" Text="Edit Password" OnClientClick="showPasswordEditBox()" OnClick="btnEdit_Click"></asp:Button>
                                <asp:Button ID="btnManageMenu" CssClass="profileBtn profileBarBtnStyle" runat="server" Text="Manage Menu" OnClick="btnManageMenu_Click"></asp:Button>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>  
		    </asp:UpdatePanel> 
        </div>

        <div class="fileUploadBox">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">  
			    <ContentTemplate> 
                    <p style="text-align: center; font-weight: 700; font-size: 24px; color: #ff0157;">Upload Images:</p>
                    <div class="cancelToggle" id="cancelFileBox" onclick="toggleCancel()"></div>

                    <div class="fileUploadBoxContent">

                        <div class="previewImgBox" id="previewImgBox">
                            <img id="previewPic" src="img/userPic/user_default.png" alt="Profile Pic" />
                        </div>

                        <div class="fileUploadBtnStyle">
                            <asp:Button ID="btnBrowsePic" runat="server" CssClass="btnUploadPic" Text="Browse Picture" OnClientClick="browsePic()"/><br />
                        </div>
                
                        <div class="btnUploadPicStyle">
                            <asp:Button ID="btnUploadPic" runat="server" CssClass="btnUploadPic" Text="Upload Picture" OnClick="btnUploadPic_Click"/><br />
                        </div>
                    </div>
                </ContentTemplate>  

                <Triggers>
                    <asp:PostBackTrigger ControlID="btnUploadPic" />
                </Triggers>
		    </asp:UpdatePanel> 

            <asp:FileUpload ID="fileUpProfilePic" runat="server" onChange="profilePicPreview(this)" style="display:none;"/>
        </div>

        <div class="editPhoneNoBox">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">  
			    <ContentTemplate> 
                    <p style="text-align: center; font-weight: 700; font-size: 24px; color: #ff0157;">Edit Phone No:</p>
                    <div class="cancelToggle" id="cancelPhoneNoEditBox" onclick="toggleCancel()"></div>

                    <div class="editPhoneNoBoxContent">
                        <div class="userImgBox" id="userImgBox">
                            <img id="userPhoneNoPicPreview" src="img/userPic/user_default.png" alt="Profile Pic" />
                        </div>

                        <div class="newPhoneNo">
                            <asp:Label ID="lblNewPhoneNo" runat="server" Text="New Phone No&nbsp;:&nbsp" Width="150px"></asp:Label>
                            <asp:TextBox ID="txtBoxNewPhoneNo" runat="server" Width="200px"></asp:TextBox>
                        </div>
                        
                        <div class="editUpdateBtnStyle">
                            <asp:Button ID="btnUpdatePhoneNo" runat="server" CssClass="btnUploadPic" Text="Update" OnClick="btnUpdatePhoneNo_Click" OnClientClick="changeUpdateText()"/><br />
                        </div>
                    </div>
                </ContentTemplate>  
		    </asp:UpdatePanel> 

            <asp:FileUpload ID="FileUpload1" runat="server" onChange="profilePicPreview(this)" style="display:none;"/>
        </div>

        <div class="editAddressBox">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">  
			    <ContentTemplate> 
                    <p style="text-align: center; font-weight: 700; font-size: 24px; color: #ff0157;">Edit Address:</p>
                    <div class="cancelToggle" id="cancelAddressEditBox" onclick="toggleCancel()"></div>

                    <div class="editAddressBoxContent">
                        <div class="userImgBox">
                            <img id="userAddressPicPreview" src="img/userPic/user_default.png" alt="Profile Pic" />
                        </div>

                        <div class="newAddress">
                            <asp:Label ID="lblNewAddress" runat="server" Text="New Address&nbsp;:&nbsp" Width="150px"></asp:Label>
                            <asp:TextBox ID="txtBoxNewAddress" runat="server" Width="200px"></asp:TextBox>
                        </div>
                        
                        <div class="editUpdateBtnStyle">
                            <asp:Button ID="btnUpdateAddress" runat="server" CssClass="btnUploadPic" Text="Update" OnClick="btnUpdateAddress_Click" OnClientClick="changeUpdateText()"/><br />
                        </div>
                    </div>
                </ContentTemplate>  
		    </asp:UpdatePanel> 

            <asp:FileUpload ID="FileUpload2" runat="server" onChange="profilePicPreview(this)" style="display:none;"/>
        </div>

        <div class="editPasswordBox">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">  
			    <ContentTemplate> 
                    <p style="text-align: center; font-weight: 700; font-size: 24px; color: #ff0157;">Edit Password:</p>
                    <div class="cancelToggle" id="cancelEditBox" onclick="toggleCancel()"></div>

                    <div class="editPasswordBoxContent">
                        <div class="userImgBox">
                            <img id="userPasswordPicPreview" src="img/userPic/user_default.png" alt="Profile Pic" />
                        </div>

                        <div class="currentPassword">
                            <asp:Label ID="lblCurrentPassword" runat="server" Text="Current Password:&nbsp" Width="150px"></asp:Label>
                            <asp:TextBox ID="txtBoxCurrentPassword" runat="server" Width="200px"></asp:TextBox>
                        </div>

                        <div class="newPassword">
                            <asp:Label ID="lblNewPassword" runat="server" Text="New Password&nbsp;:&nbsp" Width="150px"></asp:Label>
                            <asp:TextBox ID="txtBoxNewPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
                            <i class="fas fa-eye" onclick="passwordFunction()"></i>	
                        </div>
                        
                        <div class="editUpdateBtnStyle">
                            <asp:Button ID="btnUpdatePassword" runat="server" CssClass="btnUploadPic" Text="Update" OnClick="btnUpdatePassword_Click" OnClientClick="changeUpdateText()"/><br />
                        </div>
                    </div>
                </ContentTemplate>  
		    </asp:UpdatePanel> 

            <asp:FileUpload ID="FileUpload3" runat="server" onChange="profilePicPreview(this)" style="display:none;"/>
        </div>

        <img src="img/Profile/white_texture.jpg" style="left: 0; top: 0" />
    </section>

	<script type="text/javascript">
        //Initialize the UI
        window.addEventListener("load", function () {
            //Change the color of logo and toggle to black, to display it
            document.getElementById('logo').style.color = '#111'; 
            document.getElementById('toggle').style.filter = 'invert()';
            document.getElementById('header').style.backgroundColor = '#fff';
            document.getElementById('editBio').style.display = "none";

            //Cancel Edit Bio will not display before user click edit bio btn
            undisplayCancelEditButton();

            //Hide the file upload box
            //$(".fileUploadBox").hide();
        });

        function undisplayCancelEditButton() {
            //Initialize the btnEditBio
            document.getElementById('<%=btnEditBio.ClientID %>').style.width = "350px";

            document.getElementById('<%=btnCancelEditBio.ClientID %>').style.display = "none";
        }

        function changeColorForCancelBtn() {
            document.getElementById('<%=btnCancelEditBio.ClientID %>').style.background = '#8a2be2';
        }

        //Preview the profile pic
        function profilePicPreview(input) {
            if (input.files && input.files[0]) {
                var filedr = new FileReader();

                filedr.onload = function (e) {
                    $('#previewPic').attr('src', e.target.result);
                }

                filedr.readAsDataURL(input.files[0]);
            }
        }

        function browsePic() {
            document.getElementById('<%=fileUpProfilePic.ClientID %>').click();
        }

        //Show Password
        function passwordFunction() {
            var x = document.getElementById('<%= txtBoxNewPassword.ClientID %>');

            if (x.type == "password") {
                x.setAttribute("type", "text");

            } else {
                x.setAttribute("type", "password");
            }
        }

        //change the text of update in edit password box
        function changeUpdateText() {
            document.getElementById('<%= btnUpdatePassword.ClientID %>').value = "Updating...";
            document.getElementById('<%= btnUpdatePassword.ClientID %>').style.cursor = "default";
        }

        //display manage menu btn when role == management
        function displayManageArt() {
            document.getElementById('<%= btnManageMenu.ClientID %>').style.display = "inline-block";
        }

        //undisplay manage menu btn when role != management
        function undisplayManageArt() {
            document.getElementById('<%= btnManageMenu.ClientID %>').style.display = "none";
        }

        function toggleCancel() {
            $(".fileUploadBox").hide();
            $(".editPhoneNoBox").hide();
            $(".editAddressBox").hide();
            $(".editPasswordBox").hide();
        }

        function showPhoneNoEditBox() {
            $(".editPhoneNoBox").animate({
                height: "toggle"
            }, 750, function () {
                $(".editPhoneNoBox").show();
            });
        }

        function showAddressEditBox() {
            $(".editAddressBox").animate({
                height: "toggle"
            }, 750, function () {
                $(".editAddressBox").show();
            });
        }

        function showPasswordEditBox() {
            $(".editPasswordBox").animate({
                height: "toggle"
            }, 750, function () {
                $(".editPasswordBox").show();
            });
        }

        function showEditIconBox() {
            $(".fileUploadBox").animate({
                height: "toggle"
            }, 750, function () {
                $(".fileUploadBox").show();
            });
        }

        //Hide the box if user click outside the box
        $(document).mouseup(function (e) {
            if ($(e.target).closest(".fileUploadBox").length === 0 ) {

                $(".fileUploadBox").hide();
                
            }

            if ($(e.target).closest(".editPhoneNoBox").length === 0) {
                $(".editPhoneNoBox").hide();
            }

            if ($(e.target).closest(".editAddressBox").length === 0) {
                $(".editAddressBox").hide();
            }

            if ($(e.target).closest(".editPasswordBox").length === 0) {
                $(".editPasswordBox").hide();
            }

        }); 

    </script>

</asp:Content>
