using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using suro.util;
using System.IO;
using System.Web.UI.DataVisualization.Charting;

namespace PaperSystem
{
    /// <summary>
    /// clsTextQuestion 的摘要描述。
    /// </summary>
    public class clsProgramQuestion
    {
        public clsProgramQuestion()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }
        string QuestionAnswerfilePath = HttpContext.Current.Server.MapPath("~/ProgramQuestion/");
        public static string segmentsCut = "aeiouSeparateAEIOU";
        /// <summary>
        /// 儲存編輯問答題網頁上的一題問答題資料
        /// </summary>
        /// <param name="WebPage"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupDivisionID"></param>
        /// <param name="strGroupID"></param>
        /// <param name="strQuestionMode"></param>
      /*  public void saveTextQuestion(string strQID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode)
        {
            //儲存一筆資料至QuestionIndex
            //saveIntoQuestionIndex(strQID, strQuestion, strAnswer, 1);

            //儲存一筆資料至Paper_TextQuestion
            saveIntoPaper_TextQuestion(strQID, strQuestion, strAnswer, 1);

            //儲存一筆資料至QuestionMode
            SQLString mySQL = new SQLString();
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "2", null, null);
        }
        */
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
       /* public void saveQuestionAnswer(string strQID, string strAID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode)
        {
            //儲存一筆資料至QuestionIndex 
            saveIntoQuestionIndex(strQID, strQuestion, strAnswer, 1);

            //儲存一筆資料至QuestionAnswer_Question
            saveIntoProgram_Question(strQID, strQuestion, strQID);

            //儲存一筆資料至QuestionAnswer_Answer
            saveIntoProgram_Answer(strQID, strAnswer,strQID);

            //儲存一筆資料至QuestionMode
            SQLString mySQL = new SQLString();
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "2", null, null);
        }*/
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
            //All the questions need to store a record in QuestionIndex and QuestionMode table.
            //儲存一筆資料至QuestionIndex //###parameter "strAnswer" can be set to NULL in 程式題 
            saveIntoQuestionIndex(strQID, strQuestion, null, 1);//set sLevel to 1 for the time being because although sLevel ranges from 0 to 12 ,only sLevel == 1 is used in the entire Hints system 

            //儲存一筆資料至QuestionMode  //###with parameter "strQuestionType" set to 7 in 程式題,which is defined in Paper_QuestionTypeNew.aspx to represent the  QuestionType of  程式題
            SQLString mySQL = new SQLString();
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "7", null, null);

            //#######write your code below

            // save OutputFormatContent to a file and store question description 'strQuestion' and outputFormatFilePath to DB 
            saveIntoProgram_Question(strQID, strQuestion, strTextOutputFormatContent);

            // store correct answer 'strAnswer' and Testing Data 'strTextTestDataContent' to file and record some description to DB
            saveIntoProgram_Answer(strQID, strAnswer, strTextTestDataContent);

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
                //Update the existing questions in the QuestionIndex table, if there is a question with the same cQID in the QuestionIndex table.
                strSQL = "UPDATE QuestionIndex SET cQuestion = @cQuestion , cAnswer = @cAnswer , sLevel = '" + intLevel.ToString() + "' " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert the new questions in the QuestionIndex table, if there is not a question with the same cQID in the QuestionIndex table.
                strSQL = "INSERT INTO QuestionIndex (cQID , cQuestion, cAnswer , sLevel) " +
                         "VALUES ('" + strQID + "' , @cQuestion , @cANswer, '" + intLevel.ToString() + "') ";
            }
            dsCheck.Dispose();

            object[] pList = { strQuestion, (object)strAnswer ?? DBNull.Value };
            myDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// 儲存一筆資料至Program_Question
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strTextOutputFormatContent"></param>
        public void saveIntoProgram_Question(string strQID, string strQuestion, string strTextOutputFormatContent)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM Program_Question WHERE cQID = '" + strQID + "' ";
            string OutputFormatFilePath = saveOutputFormatContentToFile(strQID, strTextOutputFormatContent);
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Program_Question SET cQuestion = @cQuestion " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO Program_Question (cQID , cQuestion, OutputExample) " +
                         "VALUES ('" + strQID + "' , @cQuestion ,@OutputExample) ";
            }
            dsCheck.Dispose();

            object[] pList = { strQuestion, OutputFormatFilePath };
            myDB.ExecuteNonQuery(strSQL, pList);
        }


        public string getAllProgramTypeQuestion(string strGroupID)
        {


     
            string strSQL = "SELECT * FROM Program_Question I, QuestionMode M WHERE  M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID";
            
           
            return strSQL;
        }
        public string getFeatureProgramQuestion(DataTable dtFeatureTextQuestionQID)
        {
            string strSQL = "";
            if (dtFeatureTextQuestionQID.Rows.Count > 0)
            {
                strSQL += " SELECT * FROM Program_Question AS A INNER JOIN QuestionMode AS B ON A.cQID=B.cQID";
                strSQL += " WHERE";
                for (int i = 0; i < dtFeatureTextQuestionQID.Rows.Count; i++)
                {
                    if (i != 0)
                        strSQL += " OR";
                    strSQL += " A.cQID ='" + dtFeatureTextQuestionQID.Rows[i]["cQID"].ToString() + "'";
                }
            }
            return strSQL;
        }

        /// <summary>
        /// 儲存一筆資料至Program_Answer
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <param name="strAnswer"></param>
        public void saveIntoProgram_Answer(string strQID, string strAnswer, string strTextTestDataContent)
        {
            string strSQL = "";
            string cTestingDataAmount = saveAnswerToFile(strQID, strAnswer);
            string cAnswerFile = QuestionAnswerfilePath + @"correctAnswer";

            saveTestDataToFile(strQID, strTextTestDataContent);
            strSQL = "SELECT * FROM Program_Answer WHERE cQID = '" + strQID + "'";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Program_Answer SET cAnswer = @cAnswer_Input " +
                         "WHERE cQID = '" + strQID + "'";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO Program_Answer (cQID , cTestingDataAmount, cAnswer_Input) " +
                         "VALUES ('" + strQID + "' , '" + cTestingDataAmount + "', @cAnswer_Input ) ";
            }
            dsCheck.Dispose();

            object[] pList = { cAnswerFile };
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

        /// <summary>
        /// 將OutputfotmatContent存成檔案
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strTextOutputFormatContent"></param>
        public string saveOutputFormatContentToFile(string strQID, string strTextOutputFormatContent)
        {
            string filepath = QuestionAnswerfilePath + @"outputFormat\" + strQID + @".txt";
            StreamWriter example = new StreamWriter(filepath, false);
            try
            {
                example.Write(strTextOutputFormatContent);
            }
            catch (Exception e)
            {
                //例外處理e.g.記錄錯誤
            }
            finally
            {
                example.Close();
            }
            return filepath;
        }

        /// <summary>
        /// 將strAnswer存成檔案 回傳答案數量
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAnswer"></param>
        public string saveAnswerToFile(string strQID, string strAnswer)
        {
            string[] Answers = strAnswer.Split(new string[] { segmentsCut+ "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int count = 1;
            foreach (string Answer in Answers)
            {
                string filepath = QuestionAnswerfilePath + @"correctAnswer\" + strQID + @"-" + count + @".txt";
                StreamWriter testinput = new StreamWriter(filepath, false);
                testinput.Write(Answer);
                testinput.Close();
                count += 1;
            }
            return (count - 1).ToString();
        }
        /// <summary>
        /// 將strTextTestDataContent存成檔案
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strTextTestDataContent"></param>
        public void saveTestDataToFile(string strQID, string strTextTestDataContent)
        {
            string[] Answers = strTextTestDataContent.Split(new string[] { segmentsCut+"\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int count = 1;
            foreach (string Answer in Answers)
            {
                string filepath = QuestionAnswerfilePath + @"testinput\" + strQID + @"-" + count + @".txt";
                StreamWriter testinput = new StreamWriter(filepath, false);
                testinput.Write(Answer);
                testinput.Close();
                count += 1;
            }
        }
    }

}
