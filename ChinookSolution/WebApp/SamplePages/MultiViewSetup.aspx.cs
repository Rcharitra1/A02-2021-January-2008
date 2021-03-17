using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp.SamplePages
{
    public partial class MultiViewSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            //to switch the view to display
            int index = Int32.Parse(e.Item.Value);
            ///assign the index to ActiveViewIndexx
            MultiView1.ActiveViewIndex = index;
            
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            IODataV3.Text = IODataV1.Text;

            MultiView1.ActiveViewIndex = 1;
            Menu1.Items[1].Selected = true;
        }

        protected void Button4_Click(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}