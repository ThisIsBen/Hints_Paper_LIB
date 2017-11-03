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
using PaperSystem;

namespace Hints.DB.Conversation
{
    /// <summary>
    /// clsConversation 的摘要描述
    /// </summary>
    public class clsConversation
    {
        public clsConversation()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        #region 資料表 VP_AnswerSet, StudentAnsType

        // 根據VPAID取得所有ResponseType   老詹 2014/12/06
        public static DataTable VPResponseType_SELECT_AllResponseType(string strVPAID)
        {
            DataTable dtResult = new DataTable();
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM VP_AnswerSet WHERE cVPAID = '" + strVPAID + "' ORDER BY cVPResponseType DESC";
            DataTable dt = HintsDB.getDataSet(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dtResult = dt;
            }
            return dtResult;
        }

        // 根據VPAID取得設定好的AnswerType   老詹 2014/12/10
        public static DataTable StudentAnsType_SELECT_AnswerType(string strVPAID)
        {
            DataTable dtResult = new DataTable();
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM StudentAnsType WHERE cVPAID = '" + strVPAID + "'";
            DataTable dt = HintsDB.getDataSet(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dtResult = dt;
            }
            return dtResult;
        }

        public static void saveVPAnswer_BasicQuestionList(string strVPAID, string strCurrentProType, string strVPResponseType, string strVPAnsTitle, string strResponseContent, string strGroupID)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM VP_AnswerSet WHERE cVPAID = '" + strVPAID + "' AND cVPResponseType='" + strVPResponseType + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet dsCheck = HintsDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                strSQL = "UPDATE VP_AnswerSet SET cVPResponseContent='" + strResponseContent + "' WHERE cVPAID = '" + strVPAID + "' AND cVPResponseType='" + strVPResponseType + "'";   
            }
            else
            {
                strSQL = "INSERT INTO VP_AnswerSet (cVPAID , cProblemType, cVPResponseType , cVPResponseContent, cGroupID) " +
                         "VALUES ('" + strVPAID + "' , '" + strCurrentProType + "' , '" + strVPResponseType + "', '" + strResponseContent + "', '"+ strGroupID +"') ";
            }
            HintsDB.ExecuteNonQuery(strSQL);

            string strUpdateSQL = "UPDATE VP_AnswerSet SET cVPAnsTitle='" + strVPAnsTitle + "' WHERE cVPAID = '" + strVPAID + "'";
            HintsDB.ExecuteNonQuery(strUpdateSQL);

        }

        #endregion

        #region 資料表 Conversation_Question,Conversation_Answer,Conversation_AnswerType

        #region SELECT
        /// <summary>
        /// 根據問題分類的流水號取得所有答案的型態
        /// </summary>
        /// <param name="strQuestionClassifyID"></param>
        /// <returns></returns>
        public static DataTable Conversation_AnswerType_SELECT_AllAnswerType(int iQuestionClassifyID)
        {
            DataTable dtResult = new DataTable();
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM Conversation_AnswerType WHERE cQuestionClassifyID = '" + iQuestionClassifyID + "'";
            DataTable dt = HintsDB.getDataSet(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dtResult = dt;
            }
            return dtResult;
        }

        /// <summary>
        /// 根據問題分類的流水號與答案型態順序取得答案的型態資料
        /// </summary>
        /// <param name="iQuestionClassifyID"></param>
        /// <param name="iAnswerTypeNum"></param>
        /// <returns></returns>
        public static DataTable Conversation_AnswerType_SELECT_AssignedAnswerType(int iQuestionClassifyID, int iAnswerTypeNum)
        {
            DataTable dtResult = new DataTable();
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM Conversation_AnswerType WHERE " +
            "cQuestionClassifyID = '" + iQuestionClassifyID + "' AND cAnswerTypeNum = '" + iAnswerTypeNum + "'";
            DataTable dt = HintsDB.getDataSet(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dtResult = dt;
            }
            return dtResult;
        }

        public static string Conversation_AnswerType_SELECT_AssignedAnswerTypeName(int iQuestionClassifyID, int iAnswerTypeNum)
        {
            string strAnswerTypeName = "";
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL = "SELECT * FROM Conversation_AnswerType WHERE " +
            "cQuestionClassifyID = '" + iQuestionClassifyID + "' AND cAnswerTypeNum = '" + iAnswerTypeNum + "'";
            DataTable dt = HintsDB.getDataSet(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                strAnswerTypeName = dt.Rows[0]["cAnswerTypeName"].ToString();
            }
            return strAnswerTypeName;
        }

        /// <summary>
        /// 根據問題ID取得問題內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string Conversation_Question_SELECT_Question(string strQID)
        {
            string strReturn = "";

            string strSQL = "SELECT cQuestion FROM Conversation_Question WHERE cQID = '" + strQID + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet ds = HintsDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cQuestion"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        //取得Conversation_Answer目前的流水號
        public static int Conversation_Answer_Select_MaxiSerialNum()
        {
            int iMaxSerialNum = 0;
            clsHintsDB HintsDB = new clsHintsDB();
            string strSQL_Conversation_Question = "SELECT MAX(iSerialNum) AS iSerialNum FROM Conversation_Answer";
            DataTable dtConversation_Question = HintsDB.getDataSet(strSQL_Conversation_Question).Tables[0];
            if (dtConversation_Question.Rows.Count > 0)
            {
                if (dtConversation_Question.Rows[0]["iSerialNum"].ToString() != "")
                {
                    iMaxSerialNum = (Convert.ToInt32(dtConversation_Question.Rows[0]["iSerialNum"].ToString()) + 1);
                }
                else
                {
                    iMaxSerialNum = 1;
                }

            }
            else
            {
                iMaxSerialNum = 1;
            }
            return iMaxSerialNum;
        }

        /// <summary>
        /// 根據問題ID與答案型態取得答案內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <returns></returns>
        public static DataTable Conversation_Answer_SELECT_Answer(string strQID, string strAnswerTypeName)
        {
            DataTable dtResult = new DataTable();
            string strSQL = "SELECT * FROM Conversation_Answer WHERE cQID = '" + strQID + "' AND cAnswerType = '" + strAnswerTypeName + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet ds = HintsDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtResult = ds.Tables[0];
            }
            ds.Dispose();

            return dtResult;
        }

        /// <summary>
        /// 根據問題ID與答案ID取得答案內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <returns></returns>
        public static DataTable Conversation_Answer_SELECT_Answer_QIDandAID(string strQID, string strAID)
        {
            DataTable dtResult = new DataTable();
            string strSQL = "SELECT * FROM Conversation_Answer WHERE cQID = '" + strQID + "' AND cAID = '" + strAID + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet ds = HintsDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtResult = ds.Tables[0];
            }
            ds.Dispose();

            return dtResult;
        }

        /// <summary>
        /// 根據問題ID取得答案內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static DataTable Conversation_Answer_SELECT_Answer_QID(string strQID)
        {
            DataTable dtResult = new DataTable();
            string strSQL = "SELECT * FROM Conversation_Answer WHERE cQID = '" + strQID + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet ds = HintsDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dtResult = ds.Tables[0];
            }
            ds.Dispose();

            return dtResult;
        }

        #endregion

        #region INSERT
        //儲存對話題的問題資料
        public static void saveConversation_Question(string strQID, string strQuestion, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode)
        {
            //儲存一筆資料至QuestionIndex
            clsTextQuestion.saveIntoQuestionIndex(strQID, strQuestion, "", 1);

            //儲存一筆資料至QuestionMode
            SQLString mySQL = new SQLString();
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "4");

            //儲存一筆資料至QuestionAnswer_Question
            saveIntoConversation_Question(strQID, strQuestion);
        }

        /// <summary>
        /// 儲存一筆資料至QuestionIndex
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="intLevel"></param>
        public static void saveIntoQuestionIndex(string strQID, string strQuestion, string strAnswer, int intLevel)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM QuestionIndex WHERE cQID = '" + strQID + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet dsCheck = HintsDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE QuestionIndex SET cQuestion = @cQuestion , cAnswer = @cAnswer , sLevel = '" + intLevel.ToString() + "' " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO QuestionIndex (cQID , cQuestion, cAnswer , sLevel) " +
                         "VALUES ('" + strQID + "' , @cQuestion , @cANswer, '" + intLevel.ToString() + "') ";
            }
            dsCheck.Dispose();

            object[] pList = { strQuestion, strAnswer };
            HintsDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// 儲存一筆資料至Conversation_Question
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        public static void saveIntoConversation_Question(string strQID, string strQuestion)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM Conversation_Question WHERE cQID = '" + strQID + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet dsCheck = HintsDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Conversation_Question SET cQuestion = @cQuestion " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO Conversation_Question (cQID , cQuestion) " +
                         "VALUES ('" + strQID + "' , @cQuestion ) ";
            }
            dsCheck.Dispose();

            object[] pList = { strQuestion };
            HintsDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// 儲存一筆資料至Conversation_Answer
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <param name="strAnswer"></param>
        /// <param name="strAnswerType"></param>
        /// <param name="AnswerContentType"></param>
        public static void Conversation_Answer_INSERT(int iSerialNum, string strQID, string strAID, string strAnswer, string strAnswerType, string AnswerContentType)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM Conversation_Answer WHERE cQID = '" + strQID + "' AND cAID = '" + strAID + "' ";
            clsHintsDB HintsDB = new clsHintsDB();
            DataSet dsCheck = HintsDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Conversation_Answer SET cAnswer = @cAnswer, cAnswerContentType = @cAnswerContentType " +
                         "WHERE cQID = '" + strQID + "' AND cAID = '" + strAID + "' ";
                object[] pList = { strAnswer, AnswerContentType };
                HintsDB.ExecuteNonQuery(strSQL, pList);
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO Conversation_Answer (iSerialNum, cQID , cAID , cAnswer , cAnswerType , cAnswerContentType) " +
                         "VALUES (@iSerialNum , @cQID , @cAID , @cAnswer , @cAnswerType , @cAnswerContentType ) ";
                object[] pList = {iSerialNum, strQID, strAID, strAnswer, strAnswerType, AnswerContentType };
                HintsDB.ExecuteNonQuery(strSQL, pList);
            }
            dsCheck.Dispose();
        }
        #endregion

        #region DELETE
        /// <summary>
        /// 刪除特定的答案
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        public static void Conversation_Answer_DELETE_AssignedAnswer(string strQID, string strAID)
        {
            string strSQLDelete = "";
            clsHintsDB HintsDB = new clsHintsDB();
            strSQLDelete = "Delete From Conversation_Answer Where cQID = '" + strQID + "' AND cAID = '" + strAID + "'";
            HintsDB.ExecuteNonQuery(strSQLDelete);
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// 更新答案資料
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <param name="strAnswer"></param>
        /// <param name="strAnswerContentType"></param>
        public static void Conversation_Answer_UPDATE_Answer(string strQID, string strAID, string strAnswer, string strAnswerContentType)
        {
            string strSQLUpdateAnswer = "";

            clsHintsDB HintsDB = new clsHintsDB();

            strSQLUpdateAnswer = "Update Conversation_Answer SET cAnswer = '" + strAnswer + "' , cAnswerContentType = '" + strAnswerContentType + "' " +
                "WHERE cQID = '" + strQID + "' AND cAID = '" + strAID + "' ";

            try
            {

                HintsDB.ExecuteNonQuery(strSQLUpdateAnswer);

            }
            catch
            {

            }
        }

        #endregion

        #endregion

        #region 資料表 ItemForConversation

        #region SELECT
        /// <summary>
        /// 取得指定Case與Section的問題
        /// </summary>
        /// <param name="CaseID"></param>
        /// <param name="ClinicNumber"></param>
        /// <param name="SectionName"></param>
        /// <returns></returns>
        public static DataTable GetCaseSectionQuestionList(string CaseID, int ClinicNumber, string SectionName)
        {
            try
            {
                clsHintsDB database = new clsHintsDB();

                string sql = "SELECT * FROM ItemForConversation Inner Join (Select * From  Conversation_Question )AS Q On  ItemForConversation.cQID = Q.cQID" +
                               " WHERE cCaseID='" + CaseID + "' AND sClinicNum=" + ClinicNumber + " AND cSectionName='" + SectionName + "' ";

                return database.getDataSet(sql).Tables[0];

            }
            catch
            {
            }

            return null;
        }

        //取得case中此section的題目
        public static DataTable GetCaseSectionAllQuestionList(string CaseID, int ClinicNumber, string SectionName)
        {
            try
            {
                clsHintsDB database = new clsHintsDB();

                string sql = "SELECT * FROM ItemForConversation " +
                            " WHERE cCaseID='" + CaseID + "' AND sClinicNum=" + ClinicNumber + " AND cSectionName='" + SectionName + "' ";

                return database.getDataSet(sql).Tables[0];

            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// 取得指定問題的資料
        /// </summary>
        /// <param name="CaseID"></param>
        /// <param name="ClinicNumber"></param>
        /// <param name="SectionName"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static DataTable ItemForConversation_SELECT_AssignedQID(string CaseID, int ClinicNumber, string SectionName, string strQID)
        {
            try
            {
                clsHintsDB database = new clsHintsDB();

                string sql = "SELECT * FROM ItemForConversation " +
                            " WHERE cCaseID='" + CaseID + "' AND sClinicNum=" + ClinicNumber + " AND cSectionName='" + SectionName + "' " +
                            " AND cQID = '" + strQID + "'";

                return database.getDataSet(sql).Tables[0];

            }
            catch
            {
            }

            return null;
        }
        #endregion

        #region DELETE
        public static void ItemForConversation_DeleteAllCaseItem(string CaseID, int ClinicNumber, string SectionName)
        {
            try
            {
                clsHintsDB database = new clsHintsDB();

                string sql = "DELETE FROM ItemForConversation " +
                            " WHERE cCaseID='" + CaseID + "' AND sClinicNum=" + ClinicNumber + " AND cSectionName='" + SectionName + "' ";

                database.ExecuteNonQuery(sql);

            }
            catch
            {
            }
        }
        #endregion

        #region INSERT
        public static void ItemForConversation_INSERT_CaseQuestion(string CaseID, int ClinicNumber, string SectionName, string cQID, string cAID, string strQuestionGroupID)
        {
            clsHintsDB database = new clsHintsDB();

            string sql = "INSERT INTO ItemForConversation (cCaseID,sClinicNum,cSectionName,cQID,cAID,cQuestionGroupID) VALUES('" + CaseID + "',"
                        + ClinicNumber + ",'" + SectionName + "','" + cQID + "','" + cAID + "', '" + strQuestionGroupID + "') ";

            database.ExecuteNonQuery(sql);
        }
        #endregion

        #region UPDATE

        /// <summary>
        /// 更新答案項目
        /// </summary>
        /// <param name="CaseID"></param>
        /// <param name="ClinicNumber"></param>
        /// <param name="SectionName"></param>
        /// <param name="cQID"></param>
        /// <param name="cAID"></param>
        public static void ItemForConversation_UPDATE_Answer(string CaseID, int ClinicNumber, string SectionName, string cQID, string cAID)
        {
            try
            {
                clsHintsDB database = new clsHintsDB();
                string strSetSQLAnswer = "";

                    strSetSQLAnswer = "UPDATE ItemForConversation SET cAID= '" + cAID + "' " +
                               " WHERE cCaseID='" + CaseID + "' AND sClinicNum=" + Convert.ToInt16(ClinicNumber) + " AND cSectionName='" + SectionName + "' AND cQID='" + cQID + "' ";
                    database.ExecuteNonQuery(strSetSQLAnswer);
 
            }
            catch
            {
            }
        }

        #endregion
        #endregion

    }
}
