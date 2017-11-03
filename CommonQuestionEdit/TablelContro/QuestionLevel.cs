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

namespace AuthoringTool.QuestionEditLevel
{
    /// <summary>
    /// QuestionLevel 的摘要描述
    /// </summary>
    public class QuestionLevel
    {
        public QuestionLevel()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        #region SELECT
        /// <summary>
        /// 取得問題定義的難易度list
        /// </summary>
        /// <returns></returns>
        public static DataTable QuestionLevelName()
        {
            DataTable dtResult = new DataTable();
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM QuestionLevelName";
            dtResult = HintsDB.getDataSet(strSQL).Tables[0];
            return dtResult;

            dtResult.Dispose();
        }

        /// <summary>
        /// 取得難易度對應的value
        /// </summary>
        /// <param name="strQuestionLevel"></param>
        /// <returns></returns>
        public static int QuestionLevelName_SELECT_QuestionLevel(string strLevelName)
        {
            int iQuestionLevel = 0;
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT cQuestionLevel FROM QuestionLevelName WHERE cLevelName = '" + strLevelName + "'";
            iQuestionLevel = Convert.ToInt16(HintsDB.getDataSet(strSQL).Tables[0].Rows[0]["cQuestionLevel"].ToString());
            return iQuestionLevel;
        }
        public static string QuestionLevelName_SELECT_LevelName(int iQuestionLevel)
        {
            string strLevelName = "";
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT cLevelName FROM QuestionLevelName WHERE cQuestionLevel = '" + iQuestionLevel + "'";
            strLevelName = HintsDB.getDataSet(strSQL).Tables[0].Rows[0]["cLevelName"].ToString();
            return strLevelName;
        }
        /// <summary>
        /// 取得問題對應的難易度value
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static int QuestionLevelValue(string strQID)
        {
            int iQuestionLevel = 0;
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT cQuestionLevel FROM QuestionLevel WHERE cQID = '" + strQID + "'";
            try
            {
                iQuestionLevel = Convert.ToInt16(HintsDB.getDataSet(strSQL).Tables[0].Rows[0]["cQuestionLevel"].ToString());
            }
            catch
            {
                return -1;
            }
            
            return iQuestionLevel;

        }
        /// <summary>
        /// 取得問題的分數
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string QuestionLevel_SELECT_Grade(string strQID)
        {
            string strQuestionGrade = "0";
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM PaperContent WHERE cQID = '" + strQID + "'";
            try
            {
                strQuestionGrade = HintsDB.getDataSet(strSQL).Tables[0].Rows[0]["cQuestionScore"].ToString();
            }
            catch
            {
                return "-1";
            }
            /*
            string strQuestionGrade = "00";
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM QuestionLevel WHERE cQID = '" + strQID + "'";
            try
            {
                strQuestionGrade = HintsDB.getDataSet(strSQL).Tables[0].Rows[0]["cQuestionGrade"].ToString();
                if (strQuestionGrade == "")
                    strQuestionGrade = "00";
            }
            catch
            {
                return "-1";
            }
            */
            return strQuestionGrade;

        }

        /// <summary>
        /// 取得病徵項目
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string QuestionLevel_SELECT_QuestionSymptoms(string strQID)
        {
            string strQuestionSymptoms = "All";
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM QuestionLevel WHERE cQID = '" + strQID + "'";
            try
            {
                strQuestionSymptoms = HintsDB.getDataSet(strSQL).Tables[0].Rows[0]["cQuestionSymptoms"].ToString();
            }
            catch
            {
                return "-1";
            }

            return strQuestionSymptoms;

        }
        #endregion

        #region INSERT
        public static void INSERT_QuestionLevel(string strQID, int iQuestionLevel)
        {
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "";
            strSQL = "SELECT * FROM QuestionLevel WHERE cQID = '" + strQID + "' ";
            DataSet dsCheck = HintsDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE QuestionLevel SET cQuestionLevel = '" + iQuestionLevel + "' " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //INSERT
                strSQL = "INSERT INTO QuestionLevel (cQID , cQuestionLevel) " +
                         "VALUES ('" + strQID + "', '" + iQuestionLevel + "') ";
            }
             HintsDB.ExecuteNonQuery(strSQL);
             dsCheck.Dispose();
        }
        public static void QuestionGrade_INSERT(string strQID, string strQuestionGrade)
        {
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "";
            strSQL = "SELECT * FROM QuestionLevel WHERE cQID = '" + strQID + "' ";
            DataSet dsCheck = HintsDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE QuestionLevel SET cQuestionGrade = '" + strQuestionGrade + "' " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //INSERT
                strSQL = "INSERT INTO QuestionLevel (cQID , cQuestionGrade) " +
                         "VALUES ('" + strQID + "', '" + strQuestionGrade + "') ";
            }
            HintsDB.ExecuteNonQuery(strSQL);
            dsCheck.Dispose();
        }
        public static void QuestionLevel_INSERT_QuestionSymptoms(string strQID, string strQuestionSymptoms)
        {
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "";
            strSQL = "SELECT * FROM QuestionLevel WHERE cQID = '" + strQID + "' ";
            DataSet dsCheck = HintsDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE QuestionLevel SET cQuestionSymptoms = '" + strQuestionSymptoms + "' " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //INSERT
                strSQL = "INSERT INTO QuestionLevel (cQID , cQuestionSymptoms) " +
                         "VALUES ('" + strQID + "', '" + strQuestionSymptoms + "') ";
            }
            HintsDB.ExecuteNonQuery(strSQL);
            dsCheck.Dispose();
        }


        #endregion
    }
}
