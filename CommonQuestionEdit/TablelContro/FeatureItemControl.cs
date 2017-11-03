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
using Hints.DB;
using AuthoringTool.QuestionEditLevel;

/// <summary>
/// Summary description for FeatureItemControl
/// </summary>
public class FeatureItemControl : Table
{
    protected clsHintsDB hintsDB = new clsHintsDB();
    private string strCaseID = "";                                   //Case ID
    private string author_UserID = "";                               //編輯者的使用者ID
    private string strQID = "";                                      //The ID of Question
    private string strQuestionText = "";                             //The content of Question
    private DataTable dtFeatureSet = new DataTable();
    private DataTable dtFeatureItemList = new DataTable();
    private DataTable dtFeatureItem = new DataTable();
    private string strGroupID = "";
    
    public FeatureItemControl( Page page, int intLevel, string strGroupID,string QID)
	{
        this.author_UserID = author_UserID;
        this.Page = page;
        this.strQID = QID;
        this.strGroupID = strGroupID;
        //判斷strGroupID是否為空值，在編輯問卷模式下修改考卷，strGroupID會為空值
        CheckStrGroupID(this.strQID);
        //初始化資料
        Init();
        //建構標題
        constructQuestionTittleTable();
        //建構本身的HTML Table
        constructQuestionItemTable();
	
    }

    private void Init()
    {
        dtFeatureSet = getFeatureSet(strGroupID);
        if (dtFeatureSet.Rows.Count > 0)
            dtFeatureItemList = getFeatureItemList(dtFeatureSet.Rows[0]["iFeatureSetID"].ToString());
    }
    //判斷strGroupID是否為空值，在編輯問卷模式下修改考卷，strGroupID會為空值
    private void CheckStrGroupID(string strQuestionID)
    {
        //若判斷strGroupID為空值，則利用strQID為條件置資料庫搜尋strGroupID
        if (this.strGroupID == "")
        {
            DataTable dbGetStrGroupID = new DataTable();
            string strSQLGetStrGroupID = "SELECT cQuestionGroupID FROM QuestionMode WHERE cQID='" + strQuestionID + "'";
            dbGetStrGroupID = hintsDB.getDataSet(strSQLGetStrGroupID).Tables[0];
            this.strGroupID = dbGetStrGroupID.Rows[0]["cQuestionGroupID"].ToString();
        }
    }
    private void constructQuestionTittleTable()
    {
      //未來能夠修改成隱藏式按鈕功能
        /*
        Label lbTittle = new Label();
        if (drs.Length > 0)
        {
            string imgSrc = "";
            if (this.recordDisplayItemID.IndexOf(this.ID + ";") == -1)
            { imgSrc = "../image/plus.gif"; }
            else
            { imgSrc = "../image/minus.gif"; }
            QuestionLabel.Text = "<nobr>&nbsp;<img style='cursor:hand;' onclick=\"displaySubQuestion('" + this.ID + "');\" src='" + imgSrc + "'>&nbsp;<b>Question:</b></nobr>";
        }
        else
        {
            QuestionLabel.Text = "&nbsp;<b>Question:</b></nobr>";
        }

        Table container = ((Table)this.Rows[this.Rows.Count - 1].Cells[0].Controls[0]);
        container.Rows[0].Cells[containerNum].Width = Unit.Pixel(80);
        container.Rows[0].Cells[containerNum].Controls.Add(QuestionLabel);*/
 
        //設定特徵組的Tittle
        
        Label lbTittle = new Label();
        if (dtFeatureSet.Rows.Count > 0)
            lbTittle.Text = dtFeatureSet.Rows[0]["FeatureSetName"].ToString();
        else
            lbTittle.Text = "未設定特徵組";
        lbTittle.Font.Bold = true;
        lbTittle.Font.Size = 16;
        lbTittle.Width = 500;
        
        this.Rows.Add(new TableRow());
        this.Rows[this.Rows.Count - 1].Cells.Add(new TableCell());
        this.Rows[this.Rows.Count - 1].Cells[0].Wrap = false;
        this.Rows[this.Rows.Count - 1].Cells[0].VerticalAlign = VerticalAlign.Middle;
        this.Rows[this.Rows.Count - 1].Cells[0].Width = Unit.Pixel(80);
        this.Rows[this.Rows.Count - 1].Cells[0].Controls.Add(lbTittle);
        


    }
    private void constructQuestionItemTable()
    {
        Table controlContainer = new Table();
        controlContainer.BorderWidth = Unit.Pixel(2);
        controlContainer.BorderColor = Color.Purple;
        controlContainer.GridLines = GridLines.Both;
        controlContainer.Width = Unit.Parse("100%");
        
        this.Rows.Add(new TableRow());
        this.Rows[this.Rows.Count - 1].Cells.Add(new TableCell());
        this.Rows[this.Rows.Count - 1].Cells[0].Wrap = false;
        this.Rows[this.Rows.Count - 1].Cells[0].VerticalAlign = VerticalAlign.Middle; 
        this.Rows[this.Rows.Count - 1].Cells[0].Controls.Add(controlContainer);


        for (int countFeature = 0; countFeature < dtFeatureItemList.Rows.Count ; countFeature++)
        {

            Label lbFeatureTittle = new Label();
            lbFeatureTittle.Text = dtFeatureItemList.Rows[countFeature]["cNodeName"].ToString();
            
            ListBox listboxFeature = new ListBox();
            //listboxFeature.ID = "lbxSelectFeature$" + dtFeatureItemList.Rows[countFeature]["cNodeID"].ToString() + countFeature.ToString();
            listboxFeature.SelectionMode = ListSelectionMode.Multiple;
            //listboxFeature.Attributes["onclick"] = "lbxbox_SelectedIndexChange(this)";	
            listboxFeature.Font.Size = 16;
            listboxFeature.Height = 140;

            //將特徵值放入ListBox中
            dtFeatureItem = getFeatureItem(dtFeatureItemList.Rows[countFeature]["cNodeID"].ToString());
            for (int countItem = 0; countItem < dtFeatureItem.Rows.Count; countItem++)
            {
                //value為特徵值ID+特徵組ID
                listboxFeature.Items.Add(new ListItem(dtFeatureItem.Rows[countItem]["sFeatureName"].ToString(), dtFeatureItemList.Rows[countFeature]["cNodeID"].ToString() + "$" + dtFeatureItem.Rows[countItem]["iFeatureNum"].ToString() + "$" + dtFeatureSet.Rows[0]["iFeatureSetID"].ToString() + "$" + strQID));
                listboxFeature.Width = Unit.Parse("100%");
                listboxFeature.Items[listboxFeature.Items.Count - 1].Selected = checkFeatureSelect(this.strQID, dtFeatureItemList.Rows[countFeature]["cNodeID"].ToString(), dtFeatureItem.Rows[countItem]["iFeatureNum"].ToString());
            }


            //每個ROW三個CELL，因此%3
            switch (countFeature % 3)
            {
                case 0:
                    {
                        controlContainer.Rows.Add(new TableRow());

                        //在ROW裡增加三個CELL
                        for (int countCell = 0; countCell < 3; countCell++)
                        {
                            controlContainer.Rows[controlContainer.Rows.Count - 1].Cells.Add(new TableCell());
                            controlContainer.Rows[controlContainer.Rows.Count - 1].Cells[countCell].VerticalAlign = VerticalAlign.Middle;
                            controlContainer.Rows[controlContainer.Rows.Count - 1].Cells[countCell].Width = Unit.Parse("33%");
                        }
                        
                        Table tbFeatureItem = this.creatTable();
                        tbFeatureItem.Rows[0].Cells[0].Controls.Add(lbFeatureTittle);
                        tbFeatureItem.Rows[1].Cells[0].Controls.Add(listboxFeature);
                        controlContainer.Rows[controlContainer.Rows.Count - 1].Cells[0].Controls.Add(tbFeatureItem);
                        break;
                    }
                case 1:
                    {
                        
                        Table tbFeatureItem = this.creatTable();
                        tbFeatureItem.Rows[0].Cells[0].Controls.Add(lbFeatureTittle);
                        tbFeatureItem.Rows[1].Cells[0].Controls.Add(listboxFeature);
                        controlContainer.Rows[controlContainer.Rows.Count - 1].Cells[1].Controls.Add(tbFeatureItem);
                        break;
                    }
                case 2 :
                    {
                        Table tbFeatureItem = this.creatTable();
                        tbFeatureItem.Rows[0].Cells[0].Controls.Add(lbFeatureTittle);
                        tbFeatureItem.Rows[1].Cells[0].Controls.Add(listboxFeature);
                        controlContainer.Rows[controlContainer.Rows.Count - 1].Cells[2].Controls.Add(tbFeatureItem);
                        break;
                    }
         
            }

         }       
        //整個TABLE的屬性設定
        setTableStyle();    
    }
    
    private void setTableStyle()
    { 
        this.Width = Unit.Parse("100%");
        this.BorderColor = Color.Black;
        this.BorderWidth = Unit.Parse("2px");
    }
    
    //產生特徵值表格
    private Table creatTable()
    {
        //製造出2*1的表格
        Table tb = new Table();
        tb.Width = Unit.Parse("100%");
        tb.Rows.Add(new TableRow());
        tb.Rows[tb.Rows.Count - 1].Cells.Add(new TableCell());
        tb.Rows[tb.Rows.Count - 1].Cells[0].VerticalAlign = VerticalAlign.Middle;
        tb.Rows.Add(new TableRow());
        tb.Rows[tb.Rows.Count - 1].Cells.Add(new TableCell());
        tb.Rows[tb.Rows.Count - 1].Cells[0].VerticalAlign = VerticalAlign.Middle;

        return tb;
    }
    
    private Boolean checkFeatureSelect(string QID,string cNodeID,string FeatureNum)
    { 
        Boolean isSelect=false;
        DataTable dbFeatureForSelect = new DataTable();
        string strFeatureForSelect = "SELECT strQuestionID, FeatureSetID, iFeatureNum, cNodeID FROM FeatureForSelect WHERE (strQuestionID = '" + QID + "') AND (iFeatureNum = '" + Convert.ToInt32(FeatureNum) + "') AND (cNodeID = '" + cNodeID + "')";
        dbFeatureForSelect = hintsDB.getDataSet(strFeatureForSelect).Tables[0];
        if (dbFeatureForSelect.Rows.Count > 0)
            isSelect = true;
        return isSelect;
    
    }


    //抓取此問卷所使用的特徵組資料
    private DataTable getFeatureSet(string GroupID)
    {
        DataTable dbFeatureSet = new DataTable();
        string strSQL_FeatureSet = "SELECT B.iFeatureSetID, B.FeatureSetName FROM QuestionList AS A INNER JOIN  FeatureSetList AS B ON A.iFeatureSetID = B.iFeatureSetID INNER JOIN  QuestionGroupTree AS C ON A.qId = C.qId WHERE (C.cNodeID = '" + GroupID + "')";
        dbFeatureSet = hintsDB.getDataSet(strSQL_FeatureSet).Tables[0];
        return dbFeatureSet;
    }
    //獲得特徵值的Tittle清單
    private DataTable getFeatureItemList(string iFeatureSet)
    {
        DataTable dbFeatureItemList = new DataTable();
        string strSQL_FeatureItemList = "SELECT A.cNodeID, B.cNodeName FROM FeatureSetItem AS A INNER JOIN FeaturevalueTree AS B ON A.cNodeID = B.cNodeID WHERE (A.iFeatureSetID = '" + iFeatureSet + "')";
        dbFeatureItemList = hintsDB.getDataSet(strSQL_FeatureItemList).Tables[0];
        return dbFeatureItemList;   
    }
    //抓取特徵值資料
    private DataTable getFeatureItem(string iNodeID)
    {
        DataTable dbFeatureItem = new DataTable();
        string strSQL_FeatureItem = "SELECT iFeatureNum, sFeatureName FROM FeaturevalueItem WHERE (cNodeID = '" + iNodeID + "')";
        dbFeatureItem = hintsDB.getDataSet(strSQL_FeatureItem).Tables[0];
        return dbFeatureItem;
    }

}


