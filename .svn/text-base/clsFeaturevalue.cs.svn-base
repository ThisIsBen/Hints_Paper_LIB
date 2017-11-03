using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using suro.util;
using AuthoringTool.QuestionEditLevel;

/// <summary>
/// Summary description for clsFeaturevalue
/// </summary>
public class clsFeaturevalue
{
    public Page page = null;  
    private SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
    public clsFeaturevalue()
	{

	}
    //朱君 2012/11/27 更新特徵值
    public void update_FeatureItemIntoDataBase(DataTable dtFeatureItem)
    {
        if (dtFeatureItem.Rows.Count > 0)
        {
            //將原資料刪除
            string delete_SQL = "DELETE FeatureForSelect WHERE strQuestionID= '" + dtFeatureItem.Rows[0]["strQID"].ToString() + "'";
            sqldb.ExecuteNonQuery(delete_SQL);


            string strSQL = "SELECT * FROM FeatureForSelect WHERE 1=0";
            DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
            DataRow new_Row = null;

            foreach (DataRow dr in dtFeatureItem.Rows)
            {
                new_Row = dt.NewRow();
                new_Row["strQuestionID"] = dr["strQID"].ToString();
                new_Row["FeatureSetID"] = Convert.ToInt32(dr["FeatureSetID"].ToString());
                new_Row["iFeatureNum"] = Convert.ToInt32(dr["FeatureItemID"].ToString());
                new_Row["cNodeID"] = dr["cNodeID"].ToString();
                dt.Rows.Add(new_Row);

            }
            sqldb.Update(dt, "SELECT * FROM FeatureForSelect");
        }
        /*
        foreach (DataRow dr in dtFeatureItem.Rows)
        {
            try
            {
                string strSQL = "INSERT INTO FeatureForSelect VALUES('" + QID + "','" + dr["FeatureSetID"].ToString() + "','" + dr["FeatureItemID"].ToString() + "','" + dr["cNodeID"].ToString() + "')";
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch { }
        }*/
    }
    //朱君 2012/11/27 讀取page上的特徵值，並讀取出來存入DataTable
    public DataTable get_dtFeatureItem_Data(string strGroup, Page page)
    {
        //宣告一個暫存DataTable，且新增四個欄位
        DataTable dbFeatureItem = new DataTable();
        dbFeatureItem.Columns.Add(new DataColumn("strQID", typeof(string)));
        dbFeatureItem.Columns.Add(new DataColumn("cNodeID", typeof(string)));
        dbFeatureItem.Columns.Add(new DataColumn("FeatureItemID", typeof(string)));
        dbFeatureItem.Columns.Add(new DataColumn("FeatureSetID", typeof(string)));

        Table tbFeatureItem = (Table)page.FindControl("Form1").FindControl("FeatureItemControlTable");

        //獲得特徵ListBox
        foreach (Control trContent in tbFeatureItem.Controls)
            if (trContent is TableRow)
                foreach (Control tcContent in trContent.Controls)
                    if (tcContent is TableCell)
                        foreach (Control tbControlContainer in tcContent.Controls)
                            if (tbControlContainer is Table)
                                foreach (Control trControlContainer in tbControlContainer.Controls)
                                    if (trControlContainer is TableRow)
                                        foreach (Control tcControlContainer in trControlContainer.Controls)
                                            if (tcControlContainer is TableCell)
                                                foreach (Control tbListBox in tcControlContainer.Controls)
                                                    if (tbListBox is Table)
                                                        foreach (Control trListBox in tbListBox.Controls)
                                                            if (trListBox is TableRow)
                                                                foreach (Control tcListBox in trListBox.Controls)
                                                                    if (tcListBox is TableCell)
                                                                        foreach (Control ctlListBox in tcListBox.Controls)
                                                                            if (ctlListBox is ListBox)
                                                                            {
                                                                                //判斷ListBox選項是否有選取，若選取則暫存至dbFeatureItem
                                                                                ListBox lbx = (ListBox)ctlListBox;

                                                                                for (int i = 0; i < lbx.Items.Count; i++)
                                                                                {
                                                                                    if (lbx.Items[i].Selected == true)
                                                                                    {
                                                                                        DataRow drFeatureRow = dbFeatureItem.NewRow();
                                                                                        drFeatureRow["cNodeID"] = lbx.Items[i].Value.ToString().Split('$')[0];
                                                                                        drFeatureRow["FeatureItemID"] = lbx.Items[i].Value.ToString().Split('$')[1];
                                                                                        drFeatureRow["FeatureSetID"] = lbx.Items[i].Value.ToString().Split('$')[2];
                                                                                        drFeatureRow["strQID"] = lbx.Items[i].Value.ToString().Split('$')[3];
                                                                                        dbFeatureItem.Rows.Add(drFeatureRow);
                                                                                    }
                                                                                }

                                                                            }
        int count = dbFeatureItem.Rows.Count;

        return dbFeatureItem;
    }

}
