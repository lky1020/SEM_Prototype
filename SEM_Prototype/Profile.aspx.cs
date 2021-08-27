using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace SEM_Prototype
{
    public partial class Profile : System.Web.UI.Page
    {
        //DB
        string cs = ConfigurationManager.ConnectionStrings["PennyJuiceDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(Session["userRole"] != null)
            {
                //Set up the user detail (Pic + Name)
                InitializeProfile();
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfileDenied",
                        "alert('Access Denied. Please Login!'); window.location = 'Login.aspx';", true);
            }

        }

        private void InitializeProfile()
        {
            lblProfileName.Text = Session["username"].ToString();

            //Update Profile Pic
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateUserPic", "document.getElementById('userPic').src ='" + Session["userPicPath"].ToString() + "';", true);

            //Update the Profile Pic for change pic
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBrowsePic", "document.getElementById('previewPic').src ='" + Session["userPicPath"].ToString() + "';", true);

            RetrievePhoneNoAndAddress();

            //Display manage menu btn when role == Management
            if (Session["userRole"].ToString().Equals("Management")){

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DisplayManageMenuBtn", "displayManageMenu();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "UndisplayManageMenuBtn", "undisplayManageMenu();", true);
            }

            //Update the Profile Pic for phone no, address password
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateUserEditPhoneNoPic", "document.getElementById('userPhoneNoPicPreview').src ='" + Session["userPicPath"].ToString() + "';", true);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateUserEditAddressPic", "document.getElementById('userAddressPicPreview').src ='" + Session["userPicPath"].ToString() + "';", true);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateUserEditPasswordPic", "document.getElementById('userPasswordPicPreview').src ='" + Session["userPicPath"].ToString() + "';", true);

            //Set user bio
            if (RetrieveUserBio() == false)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Bio Error", "alert('Fail to Retrieve Your Bio, System will Refresh The Page');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Refresh Profile", "window.location = 'Profile.aspx';", true);
            }
        }

        protected void btnEditBio_Click(object sender, EventArgs e)
        {
            //Change the text of the btnBio
            if (btnEditBio.Text.Equals("Edit Bio"))
            {
                btnEditBio.Text = "Confirm Bio";

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Cancel Btn Background",
                    "changeColorForCancelBtn();", true);
            }
            else
            {
                string bio = txtAreaEditBio.Value.ToString();

                //Update the Bio into db
                UpdateUserBio(bio);

                btnEditBio.Text = "Edit Bio";
                txtAreaUserBio.Value = bio;

                //Undisplay the editBio and cancel btn fr editBio
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBioUI", 
                    "document.getElementById('editBio').style.display = 'none';", true);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "UnDisplay Cancel Btn",
                   "undisplayCancelEditButton();", true);
            }
        }

        protected void btnCancelEditBio_Click(object sender, EventArgs e)
        {
            //Undisplay the txtAreaEditBio
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBioUI",
                    "document.getElementById('editBio').style.display = 'none';", true);
            btnEditBio.Text = "Edit Bio";

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UnDisplay Cancel Btn",
                    "undisplayCancelEditButton();", true);
        }

        protected void btnUploadPic_Click(object sender, EventArgs e)
        {
            if(fileUpProfilePic.PostedFile != null)
            {
                string imgFile = Path.GetFileName(fileUpProfilePic.PostedFile.FileName);
                string imgFileExtension = Path.GetExtension(imgFile);

                if (imgFileExtension.Equals(".jpg"))
                {
                    fileUpProfilePic.SaveAs(Server.MapPath("~") + "/img/userPic/" + lblProfileName.Text + imgFileExtension);

                    try
                    {
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            SqlCommand cmd = new SqlCommand("sp_UpdateProfilePic", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("name", lblProfileName.Text);
                            cmd.Parameters.AddWithValue("profileImg", "../img/userPic/" + lblProfileName.Text + imgFileExtension);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            Session["userPicPath"] = "../img/userPic/" + lblProfileName.Text + imgFileExtension + "?" + DateTime.Now.Ticks.ToString();

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Upload Success", "alert('Profile Pic Upload Success');", true);
                        }
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Upload Fail", "alert('Please Upload .jpg Profile Image Only!');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Upload Fail", "alert('Profile Pic Fail to Upload');", true);
            }

            //Refresh page
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "window.location ='Profile.aspx';", true);
        }

        private void UpdateUserBio(string bio)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateBio", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", lblProfileName.Text);
                    cmd.Parameters.AddWithValue("bio", bio);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private Boolean RetrievePhoneNoAndAddress()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da;

                    da = new SqlDataAdapter("SELECT PhoneNo, Address FROM [dbo].[User] WHERE Name = '" + lblProfileName.Text + "' ", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        lblPhoneNo.Text = "<b>Phone No: </b>" + dt.Rows[0]["PhoneNo"].ToString();
                        lblAddress.Text = "<b>Address: </b>" + dt.Rows[0]["Address"].ToString();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean RetrieveUserBio()
        {
            if (txtAreaEditBio.Value.Equals(""))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlDataAdapter da;

                        da = new SqlDataAdapter("SELECT Bio FROM [dbo].[User] WHERE Name = '" + lblProfileName.Text + "' ", con);

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count >= 1)
                        {
                            txtAreaUserBio.Value = dt.Rows[0]["Bio"].ToString();
                            txtAreaEditBio.Value = dt.Rows[0]["Bio"].ToString();
                            return true;
                        }

                        return false;
                    }
                }
                catch (Exception)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
                }
            }

            //No bio retrieve
            return true;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //Undisplay the txtAreaEditBio
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBioUI",
                    "document.getElementById('editBio').style.display = 'none';", true);
            btnEditBio.Text = "Edit Bio";

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UnDisplay Cancel Btn",
                    "undisplayCancelEditButton();", true);
        }

        protected void btnUpdatePhoneNo_Click(object sender, EventArgs e)
        {
            //Check whether new phone no entered
            if (!txtBoxNewPhoneNo.Text.Equals(""))
            {
                //Check Whether the current and new phone no is the same
                if (CheckUserCurrentPhoneNo() == true)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Duplicate Phone No",
                        "alert('Duplicate Phone No!');", true);
                }
                else
                {
                    UpdateUserPhoneNo();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Update Phone No",
                        "alert('Phone No. Update Successfully!'); window.location = 'Profile.aspx';", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "No New Phone No",
                    "alert('Please Enter New Phone No!');", true);
            }
        }

        protected void btnUpdateAddress_Click(object sender, EventArgs e)
        {
            //Check whether new phone no entered
            if (!txtBoxNewAddress.Text.Equals(""))
            {
                //Check Whether the current and new phone no is the same
                if (CheckUserCurrentAddress() == true)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Duplicate Address",
                        "alert('Duplicate Address!');", true);
                }
                else
                {
                    UpdateUserAddress();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Update Phone No",
                        "alert('Address Update Successfully!'); window.location = 'Profile.aspx';", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "No New Address",
                    "alert('Please Enter New Address!');", true);
            }
        }

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            //Check whether current and new password entered
            if(!txtBoxCurrentPassword.Text.Equals("") && !txtBoxNewPassword.Text.Equals(""))
            {
                //Check Whether the current and new password is the same
                if (!txtBoxCurrentPassword.Text.Equals(txtBoxNewPassword.Text))
                {
                    //Check current password valid or not
                    if (CheckUserCurrentPassword() == true)
                    {
                        //Update the user password
                        UpdateUserPassword();

                        //Deactive the user account (required login again)
                        DeactivateProfileNavigation();

                        //Active Customer Navigation (By Default)
                        ActiveCustomerNavigation();

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Update Password",
                            "alert('Password Update Successfully. Please Login Again!');", true);

                        //Go to homepage
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Proceed to Homepage",
                            "window.location = 'Homepage.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Incorrect Current Password",
                            "alert('Incorrect Current Password!');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Same Password",
                       "alert('Current Password and New Password are Same. Please Enter Again!');", true);
                }        
            }
            else
            {
                if (txtBoxCurrentPassword.Text.Equals(""))
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "No Current Password",
                        "alert('Please Enter Current Password!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "No New Password",
                        "alert('Please Enter New Password!');", true);
                }
            }
 
        }

        protected void btnManageMenu_Click(object sender, EventArgs e)
        {
            //Undisplay the txtAreaEditBio
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UpdateBioUI",
                    "document.getElementById('editBio').style.display = 'none';", true);
            btnEditBio.Text = "Edit Bio";

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "UnDisplay Cancel Btn",
                    "undisplayCancelEditButton();", true);

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Direct to manage menu",
                    "window.location = 'Menu.aspx';", true);
        }

        private Boolean CheckUserCurrentPhoneNo()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT PhoneNo FROM [dbo].[User] WHERE Name = '" + lblProfileName.Text + "' AND " +
                    " PhoneNo =  '" + txtBoxNewPhoneNo.Text + "' ", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean CheckUserCurrentAddress()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Address FROM [dbo].[User] WHERE Name = '" + lblProfileName.Text + "' AND " +
                    " Address =  '" + txtBoxNewAddress.Text + "' ", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private Boolean CheckUserCurrentPassword()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Password FROM [dbo].[User] WHERE Name = '" + lblProfileName.Text + "' AND " +
                    " Password =  '" + txtBoxCurrentPassword.Text + "' ", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

            return false;
        }

        private void UpdateUserPhoneNo()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdatePhoneNo", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", lblProfileName.Text);
                    cmd.Parameters.AddWithValue("phoneNo", txtBoxNewPhoneNo.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private void UpdateUserAddress()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateAddress", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", lblProfileName.Text);
                    cmd.Parameters.AddWithValue("address", txtBoxNewAddress.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private void UpdateUserPassword()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdatePassword", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", lblProfileName.Text);
                    cmd.Parameters.AddWithValue("password", txtBoxNewPassword.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private void DeactivateProfileNavigation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ProfileDeactive", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("name", lblProfileName.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        private void ActiveCustomerNavigation()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("sp_ActiveCustomer", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }

        }
    }
}