using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using suro.util;

/// <summary>
/// clsFillOutBlankQuestion 的摘要描述
/// </summary>

namespace PaperSystem
{
    public class clsFillOutBlankQuestion
    {
        public clsFillOutBlankQuestion()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        /// <summary>
        /// 儲存編輯問答題網頁上的一題問答題資料
        /// </summary>
        /// <param name="WebPage"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupDivisionID"></param>
        /// <param name="strGroupID"></param>
        /// <param name="strQuestionMode"></param>
        public void saveTextQuestion(string strQID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode)
        {
            //儲存一筆資料至QuestionIndex
            saveIntoQuestionIndex(strQID, strQuestion, strAnswer, 1);

            //儲存一筆資料至Paper_TextQuestion
            saveIntoPaper_TextQuestion(strQID, strQuestion, strAnswer, 1);

            //儲存一筆資料至QuestionMode
            SQLString mySQL = new SQLString();
            //ben
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "2", null, null);
        }

        /// <summary>
        /// 儲存編輯問答題(FillOutBlank question)網頁上的一題填空題資料 新問答題兩張資料表 FillOutBlank_Question  FillOutBlank_Answer
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strAnswer"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strQuestionDivisionID"></param>
        /// <param name="strQuestionGroupID"></param>
        /// <param name="strQuestionMode"></param>
        public void saveQuestionAnswer(string strQID, string strAID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode, string templateQuestionQID)
        //ben 
        //public void saveQuestionAnswer(string strQID, string strAID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode, string templateQuestionQID = null)
        {
            //儲存一筆資料至QuestionIndex 
            saveIntoQuestionIndex(strQID, strQuestion, strAnswer, 1);

            //儲存一筆資料至QuestionAnswer_Question
            saveIntoQuestionAnswer_Question(strQID, strQuestion);

            //儲存一筆資料至QuestionAnswer_Answer
            saveIntoQuestionAnswer_Answer(strQID, strAID, strAnswer);

            //儲存一筆資料至QuestionMode
            SQLString mySQL = new SQLString();

            //the cQuestionType will be 10 for fillOutBlank question.
            string strQuestionType = "10";
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, strQuestionType, templateQuestionQID, strUserID);
        }
        /// <summary>
        /// 儲存編輯問答題網頁上的一題問答題資料 新問答題兩張資料表 QuestionAnswer_Question  QuestionAnswer_Answer
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strAnswer"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strQuestionDivisionID"></param>
        /// <param name="strQuestionGroupID"></param>
        /// <param name="strQuestionMode"></param>
        /// <param name="strTextTestDataContent"></param>
        /// <param name="strTextOutputFormatContent"></param>
        public void saveProgramQuestionAnswer(string strQID, string strAID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode, string strTextTestDataContent, string strTextOutputFormatContent)
        {
            //儲存一筆資料至QuestionIndex //###parameter "strAnswer" can set to NULL in 程式題
            saveIntoQuestionIndex(strQID, strQuestion, null, 1);

            //儲存一筆資料至QuestionMode
            SQLString mySQL = new SQLString();
            //ben
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "7", null, null);


            // store question description to DB
            //e.g., saveIntoQuestionAnswer_Question(strQID, strQuestion);

            // store correct answer to DB
            //e.g., saveIntoQuestionAnswer_Answer(strQID, strAID, strAnswer);
        }
        /// <summary>
        /// 儲存一筆資料至Paper_TextQuestion
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="intLevel"></param>
        public static void saveIntoPaper_TextQuestion(string strQID, string strQuestion, string strAnswer, int intLevel)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM Paper_TextQuestion WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Paper_TextQuestion SET cQuestion = @cQuestion,cAnswer = @cAnswer , sLevel = '" + intLevel.ToString() + "' " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO Paper_TextQuestion (cQID , cQuestion, cAnswer , sLevel) " +
                         "VALUES ('" + strQID + "' , @cQuestion,@cAnswer , '" + intLevel.ToString() + "') ";
            }
            dsCheck.Dispose();

            object[] pList = { strQuestion, strAnswer };
            myDB.ExecuteNonQuery(strSQL, pList);
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
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
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
            myDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// 儲存一筆資料至QuestionAnswer_Question
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        public static void saveIntoQuestionAnswer_Question(string strQID, string strQuestion)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM FillOutBlank_Question WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE FillOutBlank_Question SET cQuestion = @cQuestion " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO FillOutBlank_Question (cQID , cQuestion) " +
                         "VALUES ('" + strQID + "' , @cQuestion ) ";
            }
            dsCheck.Dispose();

            object[] pList = { strQuestion };
            myDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// 儲存一筆資料至QuestionAnswer_Answer
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <param name="strAnswer"></param>
        public static void saveIntoQuestionAnswer_Answer(string strQID, string strAID, string strAnswer)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM FillOutBlank_Answer WHERE cQID = '" + strQID + "'";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE FillOutBlank_Answer SET cAnswer = @cAnswer " +
                         "WHERE cQID = '" + strQID + "'";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO FillOutBlank_Answer (cQID , cAID, cAnswer) " +
                         "VALUES ('" + strQID + "' , '" + strAID + "', @cAnswer ) ";
            }
            dsCheck.Dispose();

            object[] pList = { strAnswer };
            myDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// 儲存編輯圖形題資料
        /// </summary>
        /// <param name="WebPage"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupDivisionID"></param>
        /// <param name="strGroupID"></param>
        /// <param name="strQuestionMode"></param>
        public void saveSimulatorQuestion(string strQID, string strQuestion, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode)
        {
            //儲存一筆資料至QuestionIndex
            string strAnswer = "";
            saveIntoQuestionIndex(strQID, strQuestion, strAnswer, 1);

            //儲存一筆資料至Paper_TextQuestion
            //saveIntoPaper_TextQuestion(strQID, strQuestion, strAnswer, 1);
            //儲存一筆資料至Paper_simulatorQuestion
            //saveIntoQuestionSimulator(strQID, strAnswer, strOrder, strSimulatorID);
            //儲存一筆資料至QuestionMode  strPaperID=NULL strQuestionDivisionID="" strQuestionGroupID, strQuestionMode=Session
            SQLString mySQL = new SQLString();
            //ben
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "3", null, null);
        }

        /// <summary>
        /// 儲存一筆資料至QuestionSimulator
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="intLevel"></param>
        public void saveIntoQuestionSimulator(string strQID, string strAnswer, string strOrder, string strSimulatorID, string cContent, string cAnsID)
        {   //儲存一筆資料至Question_Simulator
            string strSQL = "";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            //Insert
            strSQL = "INSERT INTO Question_Simulator (cQID , cAnswer, cOrder , cSimulatorID , cContent, cAnsID) " +
                     "VALUES ('" + strQID + "' , @strAnswer , @strOrder, @strSimulatorID, @cContent, @cAnsID) ";

            object[] pList = { strAnswer, strOrder, strSimulatorID, cContent, cAnsID };
            myDB.ExecuteNonQuery(strSQL, pList);
        }

    }

}

