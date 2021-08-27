using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SEM_Prototype
{
    public partial class Cart : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["PennyJuiceDb"].ConnectionString;
        double totalSelectPrice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RetrievePhoneNoAndAddress();
                refreshdata();
            }
        }

        private void RetrievePhoneNoAndAddress()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da;

                    da = new SqlDataAdapter("SELECT PhoneNo, Address FROM [dbo].[User] WHERE Name = '" + Session["username"].ToString() + "' ", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        lblContactName.Text = "<b>Name: </b>" + Session["username"].ToString();
                        lblPhoneNo.Text = "<b>Phone No: </b>" + dt.Rows[0]["PhoneNo"].ToString();
                        lblAddress.Text = "<b>Address: </b>" + dt.Rows[0]["Address"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "ProfilepageDBError", "alert('Error Occur in Database. Please Contact Quad-Core AWS!');", true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "DirectToHomepage", "alert('Redirecting you to Homepage!'); window.location = 'Homepage.aspx';", true);
            }
        }

        //refresh the data in gridview
        public void refreshdata()
        {
            try{
                //detect current user id
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    string query1 = "Select UserId FROM [dbo].[User] WHERE Name = '" + Session["username"].ToString() + "'"; //Role = 'Customer' AND LoginStatus = 'Active' ";
                    using (SqlCommand cmd1 = new SqlCommand(query1, conn))
                    {
                        Session["userID"] = ((Int32?)cmd1.ExecuteScalar()) ?? 0;
                    }
                    conn.Close();
                }

                //pass data into grid
                SqlConnection con = new SqlConnection(cs);
                con.Open();
                String queryGetData = "Select m.MenuId, m.MenuName, m.MenuImage, m.Price, m.MenuDescription, o.orderDetailId, o.qtySelected, o.Subtotal from [Cart] c " +
                    "INNER JOIN [OrderDetails] o on c.CartId = o.CartId " +
                    "INNER JOIN [Menu] m on o.MenuId = m.MenuId  " + 
                    "Where c.UserId = @userid AND c.status = 'cart'";
                SqlCommand cmd = new SqlCommand(queryGetData, con);
                cmd.Parameters.AddWithValue("@userid", Session["userID"]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvCart.DataSource = dt;
                    gvCart.DataBind();

                    //set displayItem
                    cartEmpty.Visible = false;
                    totalPrice.Visible = true;
                    cart_orderBtn.Visible = true;
                }
                else
                {
                    gvCart.DataSource = dt;
                    gvCart.DataBind();

                    //set display item
                    cartEmpty.Visible = true;
                    totalPrice.Visible = false;
                    cart_orderBtn.Visible = false;
                }
                checkArtAvailability();
            }catch(Exception)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "CartDenied",
                        "alert('Access Denied. Please Login!'); window.location = 'Login.aspx';", true);
            }
        }

        //detect menu product availability
        private void checkArtAvailability()
        {
            SqlConnection con = new SqlConnection(cs);
            con.Open();
            Boolean availability = false;

            for (int i = 0; i < gvCart.Rows.Count; i++)
            {
                string queryArtAvailable = "SELECT Availability FROM Menu WHERE MenuId = (SELECT MenuId FROM OrderDetails WHERE OrderDetailId = @od_Id); ";

                using (SqlCommand cmdArtAvailable = new SqlCommand(queryArtAvailable, con))
                {
                    cmdArtAvailable.Parameters.AddWithValue("@od_Id", gvCart.DataKeys[i].Value.ToString());
                    availability = (Boolean)((cmdArtAvailable.ExecuteScalar()) ?? '0');

                    if (!availability)
                    {
                        gvCart.Rows[i].Cells[0].Enabled = false;
                        gvCart.Rows[i].Cells[2].Text = "Item is not available";
                        gvCart.Rows[i].Cells[3].Text = "-";
                        gvCart.Rows[i].Cells[4].Text = "-";
                        gvCart.Rows[i].Cells[5].Text = "RM - ";
                    }
                }
            }

            //detect product qty
            int qtyAvailable = 0;

            for (int i = 0; i < gvCart.Rows.Count; i++)
            {
                string queryArtAvailable = "SELECT Quantity FROM Menu WHERE MenuId = (SELECT MenuId FROM OrderDetails WHERE OrderDetailId = @od_Id); ";

                using (SqlCommand cmdArtAvailable = new SqlCommand(queryArtAvailable, con))
                {
                    cmdArtAvailable.Parameters.AddWithValue("@od_Id", gvCart.DataKeys[i].Value.ToString());
                    qtyAvailable = (int)((cmdArtAvailable.ExecuteScalar()) ?? 0);

                    if (qtyAvailable == 0)
                    {
                        gvCart.Rows[i].Cells[0].Enabled = false;
                        gvCart.Rows[i].Cells[2].Text = "Item is sold out";
                        gvCart.Rows[i].Cells[3].Text = "-";
                        gvCart.Rows[i].Cells[4].Text = "-";
                        gvCart.Rows[i].Cells[5].Text = "RM - ";
                    }
                }
            }
            con.Close();
        }


        //edit menu qty fn (based on row)
        protected void gvCart_RowEditing(object sender, GridViewEditEventArgs e)
        {

            gvCart.EditIndex = e.NewEditIndex;

            refreshdata();
        }

        protected void gvCart_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCart.EditIndex = -1;
            refreshdata();
        }

        protected void gvCart_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                //check availability
                SqlConnection con = new SqlConnection(cs);
                con.Open();
                int qty = 0;

                //retrieve qty left
                string queryArtQty = "SELECT Quantity FROM Menu WHERE MenuId = (SELECT MenuId FROM OrderDetails WHERE OrderDetailId = @od_Id); ";

                using (SqlCommand cmdArtQty = new SqlCommand(queryArtQty, con))
                {
                    cmdArtQty.Parameters.AddWithValue("@od_Id", gvCart.DataKeys[e.RowIndex].Value.ToString());
                    qty = ((Int32?)cmdArtQty.ExecuteScalar()) ?? 0;

                }
                con.Close();

                
                int select_qty = int.Parse((gvCart.Rows[e.RowIndex].FindControl("cart_qtySelect") as TextBox).Text.Trim());
                
                //if invalid input
                if (select_qty <= 0)
                {
                    Response.Write("<script>alert('Quantity must be 1 or greater than 1. Please enter again, if you do not want it in your cart, you can delete it from your cart.')</script>");
                }
                //if input qty more than available qty
                else if (select_qty > qty)
                {
                    Response.Write("<script>alert('There is only " + qty.ToString() + " quantity for this menu is available. Therefore, quantity should not more than " + qty.ToString() + ".')</script>");
                }
                else
                {
                    //if valid input qty
                    using (con)
                    {
                        con.Open();
                        String query = "Update OrderDetails SET qtySelected=@qty, Subtotal=@subtotal WHERE OrderDetailId =@detailid";
                        
                        SqlCommand cmd = new SqlCommand(query, con);

                        int itemQty = int.Parse((gvCart.Rows[e.RowIndex].FindControl("cart_qtySelect") as TextBox).Text.Trim());
                        double price = double.Parse((gvCart.Rows[e.RowIndex].FindControl("cart_artPrice") as TextBox).Text.Trim());

                        double subTotal = price * itemQty;

                        cmd.Parameters.AddWithValue("@qty", (gvCart.Rows[e.RowIndex].FindControl("cart_qtySelect") as TextBox).Text.Trim());
                        cmd.Parameters.AddWithValue("@subtotal", subTotal);
                        cmd.Parameters.AddWithValue("@detailid", Convert.ToInt32(gvCart.DataKeys[e.RowIndex].Value.ToString()));
                        cmd.ExecuteNonQuery();
                        gvCart.EditIndex = -1;

                        refreshdata();

                        // Response.Write("<script>alert('Cart Information is Updated.')</script>");
                        con.Close();
                    }
                }
            }catch (Exception)
            {
                Response.Write("<script>alert('Update is fail. ')</script>");
                refreshdata();
            }

        }

        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                String query = "DELETE FROM OrderDetails WHERE OrderDetailId = @detailId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@detailId", Convert.ToInt32(gvCart.DataKeys[e.RowIndex].Value.ToString()));
                cmd.ExecuteNonQuery();

                refreshdata();

                Response.Write("<script>alert('Selected Juice Remove From Table Successfully')</script>");
            }
        }

        //the header checkbox
        protected void CheckBoxHead_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chckheader = (CheckBox)gvCart.HeaderRow.FindControl("CheckBoxHead");

            foreach (GridViewRow row in gvCart.Rows)
            {
                CheckBox chckrw = (CheckBox)row.FindControl("chkItems");

                if (chckheader.Checked == true && row.Cells[0].Enabled == true)
                    chckrw.Checked = true;
                else
                   chckrw.Checked = false;

            }
            totalSelectPrice = 0;
            //chkItems
            for (int i = 0; i < gvCart.Rows.Count; i++)
            {
                CheckBox chkb = (CheckBox)gvCart.Rows[i].Cells[0].FindControl("chkItems");
                if (gvCart.Rows[i].Cells[0].Enabled)
                {
                    if (chkb.Checked)
                    {
                        totalSelectPrice += double.Parse((gvCart.Rows[i].FindControl("cart_menuSubPrice") as TextBox).Text.ToString());
                        totalPrice.Text = "Total : RM " + totalSelectPrice.ToString("F");
                    }
                }else{
                    gvCart.Rows[i].Cells[2].Text = "Item is not available";
                    gvCart.Rows[i].Cells[3].Text = "-";
                    gvCart.Rows[i].Cells[4].Text = "-";
                    gvCart.Rows[i].Cells[5].Text = "RM - ";
                    totalSelectPrice += 0;
                }
                totalPrice.Text = "Total : RM " + totalSelectPrice.ToString("F");
            }
        }

        protected void chkItems_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chckheader = (CheckBox)gvCart.HeaderRow.FindControl("CheckBoxHead");
            
            //chkItems
            for (int i = 0; i < gvCart.Rows.Count; i++)
            {
                CheckBox chkb = (CheckBox)gvCart.Rows[i].Cells[0].FindControl("chkItems");

                if (chkb.Checked && gvCart.Rows[i].Cells[0].Enabled == true)
                {
                    totalSelectPrice += double.Parse((gvCart.Rows[i].FindControl("cart_menuSubPrice") as TextBox).Text.ToString());
                    
                }
                else if (chckheader.Checked == true && chkb.Checked == false && gvCart.Rows[i].Cells[0].Enabled == true)
                {
                    chckheader.Checked = false;

                    totalSelectPrice += 0;
                    
                }
                else if (gvCart.Rows[i].Cells[0].Enabled == false)
                {
                    gvCart.Rows[i].Cells[2].Text = "Item is not available";
                    gvCart.Rows[i].Cells[3].Text = "-";
                    gvCart.Rows[i].Cells[4].Text = "-";
                    gvCart.Rows[i].Cells[5].Text = "RM - ";

                    totalSelectPrice += 0;
                    
                }
                totalPrice.Text = "Total : RM " + totalSelectPrice.ToString("F");

            }

            if (chckheader.Checked == false)
            {
                //these 2 var is used to check if all box is selected - if yes then the header box will be chk else unchk
                int numChecked = 0;
                int availableChkBox = 0;

                //chkItems
                for (int i = 0; i < gvCart.Rows.Count; i++)
                {
                    CheckBox chkb = (CheckBox)gvCart.Rows[i].Cells[0].FindControl("chkItems");

                    if (gvCart.Rows[i].Cells[0].Enabled)
                        availableChkBox += 1;

                    if (chkb.Checked)
                        numChecked += 1;
                }

                if (numChecked == availableChkBox)
                    chckheader.Checked = true;
            }

        }

        protected void cart_artImg_click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = sender as ImageButton;
            Int32 menuID = Convert.ToInt32(imgButton.CommandArgument.ToString());

            Response.Redirect("MenuDetails.aspx?MenuId=" + menuID);
        }



    }
}