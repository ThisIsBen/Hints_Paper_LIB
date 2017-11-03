using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hints.DB;

namespace Hints.DB.QuestionGroup
{
    /// <summary>
    /// clsQuestionGroup 的摘要描述
    /// </summary>
    public class clsQuestionGroup
    {
        public clsQuestionGroup()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        //根據題組ID取得題組問題的ID
        public static string Paper_QuestionSelectionGroupID_SELECT_QuestionGroupQID(object strQuestionGroupID)
        {
            string strQuestionGroupQID = "";
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL_Paper_QuestionSelectionGroupID = "SELECT * FROM Paper_QuestionSelectionGroupID WHERE " +
                            " cGroupID = '" + strQuestionGroupID + "'";
            DataTable dtPaper_QuestionSelectionGroupID = HintsDB.getDataSet(strSQL_Paper_QuestionSelectionGroupID).Tables[0];
            if (dtPaper_QuestionSelectionGroupID.Rows.Count > 0)
            {
                strQuestionGroupQID = dtPaper_QuestionSelectionGroupID.Rows[0]["cQID"].ToString();
            }

            return strQuestionGroupQID;
        }

        //根據問題ID取得是否為有題組ID
        public static DataTable Paper_QuestionSelectionGroupID_SELECT_QID(object strQID)
        {
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL_Paper_QuestionSelectionGroupID = "SELECT * FROM Paper_QuestionSelectionGroupID WHERE " +
                                 " cQID = '" + strQID + "'";
            DataTable dtPaper_QuestionSelectionGroupID = HintsDB.getDataSet(strSQL_Paper_QuestionSelectionGroupID).Tables[0];

            return dtPaper_QuestionSelectionGroupID;
        }

        //根據問題ID與選項ID取得是否為題組
        public static DataTable Paper_QuestionSelectionGroupID_SELECT_QIDAndSelectionID(object strQID, object strSelectionID)
        {
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL_Paper_QuestionSelectionGroupID = "SELECT * FROM Paper_QuestionSelectionGroupID " +
            "WHERE cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "'";
            DataTable dtPaper_QuestionSelectionGroupID = new DataTable();
            dtPaper_QuestionSelectionGroupID = HintsDB.getDataSet(strSQL_Paper_QuestionSelectionGroupID).Tables[0];
            return dtPaper_QuestionSelectionGroupID;
        }

        //根據問卷ID與題組問題的ID取得問題的順序
        public static string Paper_Content_SELECT_QSeq(object strPaperID, object strQuestionGroupQID)
        {
            string strQuestionGroupSeq = "";
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL_Paper_Content = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQuestionGroupQID + "' ";
            DataTable dtPaper_Content = HintsDB.getDataSet(strSQL_Paper_Content).Tables[0];
            if (dtPaper_Content.Rows.Count > 0)
            {
                strQuestionGroupSeq = dtPaper_Content.Rows[0]["sSeq"].ToString();
            }
            return strQuestionGroupSeq;

        }

        //根據問卷ID與題組問題的順序取得問題的資料
        public static DataTable Paper_Content_SELECT(object strPaperID, object sSeq)
        {
            DataTable dtPaper_Content = new DataTable();
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL_Paper_Content = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND sSeq = '" + sSeq + "' ";
            dtPaper_Content = HintsDB.getDataSet(strSQL_Paper_Content).Tables[0];

            return dtPaper_Content;
        }

        //根據題組ID與題組中相關問題順序取得問題資料
        public static DataTable Paper_QuestionSelectionGroupItem_SELECT_QuestionGroupQID(object strQuestionGroupID, object strQuestionGroupSeq)
        {
            clsHintsDB HintsDB = new clsHintsDB();
            DataTable dtPaper_QuestionSelectionGroupItem = new DataTable();
            string strSQL_Paper_QuestionSelectionGroupItem = "SELECT * FROM Paper_QuestionSelectionGroupItem " +
                           "WHERE cGroupID = '" + strQuestionGroupID + "' AND cSequence = '" + strQuestionGroupSeq + "'";
            dtPaper_QuestionSelectionGroupItem = HintsDB.getDataSet(strSQL_Paper_QuestionSelectionGroupItem).Tables[0];
            return dtPaper_QuestionSelectionGroupItem;
                                                           
        }

        //根據題組問題ID取得題組相關問題資料
        public static DataTable Paper_QuestionSelectionGroupIDJoinItem(object strQuestionGroupQID)
        {
            clsHintsDB HintsDB = new clsHintsDB();
            DataTable dtPaper_QuestionSelectionGroupIDJoinItem = new DataTable();
            string strSQL_Paper_QuestionSelectionGroupIDJoinPaper_QuestionSelectionGroupItem = "SELECT dbo.Paper_QuestionSelectionGroupID.cSelectionID, " +
                    "dbo.Paper_QuestionSelectionGroupID.cQID AS cQGroupID, dbo.Paper_QuestionSelectionGroupItem.cGroupID, " +
                    "dbo.Paper_QuestionSelectionGroupItem.cQID, dbo.Paper_QuestionSelectionGroupItem.cSequence " +
                    "FROM dbo.Paper_QuestionSelectionGroupID INNER JOIN " +
                    "dbo.Paper_QuestionSelectionGroupItem ON dbo.Paper_QuestionSelectionGroupID.cGroupID = dbo.Paper_QuestionSelectionGroupItem.cGroupID " +
                    "WHERE (dbo.Paper_QuestionSelectionGroupID.cQID = '" + strQuestionGroupQID + "') " +
                    "ORDER BY dbo.Paper_QuestionSelectionGroupID.cSelectionID";
            dtPaper_QuestionSelectionGroupIDJoinItem = HintsDB.getDataSet(strSQL_Paper_QuestionSelectionGroupIDJoinPaper_QuestionSelectionGroupItem).Tables[0];
            return dtPaper_QuestionSelectionGroupIDJoinItem; 
        }

    }
}
