using System;
using suro.util;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PaperSystem
{
    /// <summary>
    /// SQLString 的摘要描述。
    /// </summary>
    public class SQLString
    {
        //建立SqlDB物件
        SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

        public SQLString()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        public string getCaseFolder(string strCaseID)
        {
            return "SELECT T.cURL , T.cDivisionID , U.cURL AS 'Server' FROM TeachingCase T , DivIDtoURL U WHERE T.cCaseID = '" + strCaseID + "' AND T.cDivisionID = U.cDivisionID";
        }

        /// <summary>
        /// 刪除一個問答題
        /// </summary>
        /// <param name="strQID"></param>
        public static void deleteTextQuestionByQID(string strQID)
        {
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

            string strSQL = "DELETE Paper_TextQuestion WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            strSQL = "DELETE QuestionMode WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            strSQL = "DELETE QuestionIndex WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            //刪除QuestionLevel
            strSQL = "DELETE QuestionLevel WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

        }

        /// <summary>
        /// 刪除一個問答題
        /// </summary>
        /// <param name="strQID"></param>
        public static void QuestionAnswer_DELETE_ByQID(string strQID)
        {
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

            string strSQL = "DELETE QuestionAnswer_Question WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            strSQL = "DELETE QuestionAnswer_Answer WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            strSQL = "DELETE QuestionMode WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            strSQL = "DELETE QuestionIndex WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            //刪除QuestionLevel
            strSQL = "DELETE QuestionLevel WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

        }

        public static void Conversation_DELETE_ByQID(string strQID)
        {
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

            string strSQL = "DELETE Conversation_Question WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            //strSQL = "DELETE QuestionAnswer_Answer WHERE cQID = '" + strQID + "' ";
            //myDB.ExecuteNonQuery(strSQL);

            strSQL = "DELETE QuestionMode WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            strSQL = "DELETE QuestionIndex WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

            //刪除QuestionLevel
            strSQL = "DELETE QuestionLevel WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

        }

        /// <summary>
        /// 儲存一筆資料至QuestionMode
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strQuestionDivisionID"></param>
        /// <param name="strQuestionGroupID"></param>
        /// <param name="strQuestionMode"></param>
        /// <param name="strQuestionType"></param>
        /// <param name="templateQuestionQID"></param>
        /// <param name="strUserID"></param>
        public void saveIntoQuestionMode(string strQID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode, string strQuestionType, string templateQuestionQID, string strUserID)
        //ben
        //public void saveIntoQuestionMode(string strQID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode, string strQuestionType, string templateQuestionQID = null, string strUserID=null)
        {
            string strSQL = "";
            //取得QuestionGroupName
            string strQuestionGroupName = DataReceiver.getQuestionGroupNameByQuestionGroupID(strQuestionGroupID);


            //if teacher save the edited question as a new question
            //add similarID to the new question to show all the related similar question when the teacher picks one of the parent or offspring of this new question.
            if (templateQuestionQID != null && strUserID != null)
            {
                //to contain similarID to assign to the new question
                string similarID = "";
                string TemplateQuesQuestionGroupID = "";
                string TemplateQuesQuestionGroupName = "";
                //Ben check whether the the value of “similarID”of the question that is used as a template is null or not.
                strSQL = " SELECT similarID ,cQuestionGroupID,cQuestionGroupName FROM QuestionMode" +
                                " WHERE cQID = '" + templateQuestionQID + "' and cQuestionType= '" + strQuestionType + "' and similarID IS NOT NULL";

                SqlDB QuestionModeDB = new SqlDB(System.Configuration.ConfigurationManager.AppSettings["connstr"]);
                DataSet similarIDCheck = QuestionModeDB.getDataSet(strSQL);



                //If the question that is used as a template already has similarID in QuestionMode table.
                if (similarIDCheck.Tables[0].Rows.Count > 0)
                {
                    //set similarID the same as the that of the question that is used as a template
                    similarID = similarIDCheck.Tables[0].Rows[0]["similarID"].ToString();

                    //get the cQuestionGroupName and cQuestionGroupID of the template question
                    TemplateQuesQuestionGroupID = similarIDCheck.Tables[0].Rows[0]["cQuestionGroupID"].ToString();
                    TemplateQuesQuestionGroupName = similarIDCheck.Tables[0].Rows[0]["cQuestionGroupName"].ToString();


                }

                //If the question that is used as a template doesn't have similarID in QuestionMode table.
                else
                {

                    //set new similarID to the template question and the new question.
                    DataReceiver myReceiver = new DataReceiver();
                    similarID = strUserID + "_simiID_" + myReceiver.getNowTime();

                    //assign the new similarID to the question that is used as a template
                    //Update similarID of the template question
                    strSQL = " UPDATE QuestionMode SET similarID = '" + similarID + "'  WHERE cQID = '" + templateQuestionQID + "' and cQuestionType='" + strQuestionType + "'";

                    
                    QuestionModeDB.ExecuteNonQuery(strSQL);

                   


                    //strQuestionGroupID and strQuestionGroupName are all empty, just get these fields from the template question.
                    if (strQuestionGroupID == "" && strQuestionGroupName == "")
                    {
                        //get the cQuestionGroupName and cQuestionGroupID of the template question
                        strSQL = " SELECT cQuestionGroupID,cQuestionGroupName FROM QuestionMode" +
                                  " WHERE cQID = '" + templateQuestionQID + "' and cQuestionType= '" + strQuestionType + "' and similarID IS NOT NULL";


                        DataSet QuestionGroupCheck = QuestionModeDB.getDataSet(strSQL);

                        //If the question that is used as a template already has similarID in QuestionMode table.
                        if (QuestionGroupCheck.Tables[0].Rows.Count > 0)
                        {
                            //set similarID the same as the that of the question that is used as a template
                            similarID = QuestionGroupCheck.Tables[0].Rows[0]["similarID"].ToString();

                            //get the cQuestionGroupName and cQuestionGroupID of the template question
                            TemplateQuesQuestionGroupID = QuestionGroupCheck.Tables[0].Rows[0]["cQuestionGroupID"].ToString();
                            TemplateQuesQuestionGroupName = QuestionGroupCheck.Tables[0].Rows[0]["cQuestionGroupName"].ToString();


                        }
                        QuestionGroupCheck.Dispose();
                    }

                    else//use the GroupID passed by parameter in to this function and the corresponding GroupName for the new question
                    {
                        TemplateQuesQuestionGroupID=strQuestionGroupID;
                        TemplateQuesQuestionGroupName = strQuestionGroupName;
                    }
                }
                similarIDCheck.Dispose();
               
                

                

                //store similarID along with all other related data to table QuestionMode
                strSQL = " SELECT * FROM QuestionMode" +
                                " WHERE cQID = '" + strQID + "' ";

                SqlDB myDB = new SqlDB(System.Configuration.ConfigurationManager.AppSettings["connstr"]);
                DataSet dsCheck = myDB.getDataSet(strSQL);


                //Do nothing if there is an existing question with the same cQID in QuestionMode table.
                if (dsCheck.Tables[0].Rows.Count > 0)
                {
                    // 先註解，因為將QuestionMode資料表的QID欄位的Primary Key拿掉，所以若執行此Update會導致有重複筆資料。(功能上是為了可以將一題對話題放入不同的問題主題中。)  老詹 2015/09/06
                    /*{
                        //Update
                        strSQL = " UPDATE QuestionMode SET cPaperID = '" + strPaperID + "' , cDivisionID = '" + strQuestionDivisionID + "' , cQuestionGroupID = '" + strQuestionGroupID + "' , cQuestionGroupName = '" + strQuestionGroupName + "' , cQuestionMode = '" + strQuestionMode + "' , cQuestionType = '" + strQuestionType + "' " +
                                " WHERE cQID = '" + strQID + "' ";
                    }*/
                }

                //Insert the new question to QuestionMode table, if there is no existing questions with the same cQID.
                else
                {
                    //Insert
                    strSQL = " INSERT INTO QuestionMode (cQID , cPaperID , cDivisionID , cQuestionGroupID , cQuestionGroupName , cQuestionMode , cQuestionType,similarID) " +
                            " VALUES ('" + strQID + "' , '" + strPaperID + "' , '" + strQuestionDivisionID + "' , '" + TemplateQuesQuestionGroupID + "' , '" + strQuestionGroupName + "' , '" + strQuestionMode + "' , '" + strQuestionType + "' , '" + similarID + "') ";
                    /*write SQL to file to 
                    //to inspect the SQL cmd when something went wrong with SQL cmd 
                    // Create a file to write to.              
                    File.WriteAllText("D:/Hints_on_60/Hints/App_Code/AuthoringTool/CaseEditor/Paper/updateSimilarIDSQL.txt", strSQL);
                    */
                
                }
                dsCheck.Dispose();
                myDB.ExecuteNonQuery(strSQL);


            }


            //store a record to table QuestionMode(If it's not 另存新題)
            else
            {
                
                strSQL = " SELECT * FROM QuestionMode" +
                                " WHERE cQID = '" + strQID + "' ";

                SqlDB myDB = new SqlDB(System.Configuration.ConfigurationManager.AppSettings["connstr"]);
                DataSet dsCheck = myDB.getDataSet(strSQL);


                //Do nothing if there is an existing question with the same cQID in QuestionMode table.
                if (dsCheck.Tables[0].Rows.Count > 0)
                {
                    // 先註解，因為將QuestionMode資料表的QID欄位的Primary Key拿掉，所以若執行此Update會導致有重複筆資料。(功能上是為了可以將一題對話題放入不同的問題主題中。)  老詹 2015/09/06
                    /*{
                        //Update
                        strSQL = " UPDATE QuestionMode SET cPaperID = '" + strPaperID + "' , cDivisionID = '" + strQuestionDivisionID + "' , cQuestionGroupID = '" + strQuestionGroupID + "' , cQuestionGroupName = '" + strQuestionGroupName + "' , cQuestionMode = '" + strQuestionMode + "' , cQuestionType = '" + strQuestionType + "' " +
                                " WHERE cQID = '" + strQID + "' ";
                    }*/
                }

                //Insert the new question to QuestionMode table, if there is no existing questions with the same cQID.
                else
                {
                    //Insert
                    strSQL = " INSERT INTO QuestionMode (cQID , cPaperID , cDivisionID , cQuestionGroupID , cQuestionGroupName , cQuestionMode , cQuestionType) " +
                            " VALUES ('" + strQID + "' , '" + strPaperID + "' , '" + strQuestionDivisionID + "' , '" + strQuestionGroupID + "' , '" + strQuestionGroupName + "' , '" + strQuestionMode + "' , '" + strQuestionType + "') ";
                }
                dsCheck.Dispose();
                //insert record to table QuestionMode 
                myDB.ExecuteNonQuery(strSQL);
            }
           
        }

        /// <summary>
        /// 儲存一筆資料至Paper_NextStepBySelectionID
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <param name="intQuestionIndex"></param>
        /// <param name="strSelectionID"></param>
        /// <param name="intSelectionIndex"></param>
        /// <param name="intNextMethod"></param>
        /// <param name="strNextSection"></param>
        /// <param name="intNextIndex"></param>
        public static void SaveToPaper_NextStepBySelectionID(string strPaperID, string strQID, int intQuestionIndex, string strSelectionID, int intSelectionIndex, int intNextMethod, string strNextSection, int intNextIndex)
        {
            string strSQL = " SELECT * FROM Paper_NextStepBySelectionID" +
                            " WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_NextStepBySelectionID SET sQuestionSeq = '" + intQuestionIndex.ToString() + "' , sSelectionSeq = '" + intSelectionIndex.ToString() + "' , sNextMethod = '" + intNextMethod.ToString() + "' , cNextSection = '" + strNextSection + "' , sNextquestionSeq = '" + intNextIndex.ToString() + "' " +
                        " WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_NextStepBySelectionID (cPaperID , cQID , sQuestionSeq , cSelectionID , sSelectionSeq , sNextMethod , cNextSection , sNextquestionSeq) " +
                        " VALUES ('" + strPaperID + "' , '" + strQID + "' , '" + intQuestionIndex.ToString() + "' , '" + strSelectionID + "' , '" + intSelectionIndex.ToString() + "' , '" + intNextMethod.ToString() + "' , '" + strNextSection + "' , '" + intNextIndex.ToString() + "') ";
            }
            dsCheck.Dispose();
            myDB.ExecuteNonQuery(strSQL);
        }

        /// <summary>
        /// 修改某問卷的Title資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strTitle"></param>
        public void UpdatePaperTitleOfPaper_Header(string strPaperID, string strTitle)
        {
            string strSQL = " UPDATE Paper_Header SET cTitle = '" + strTitle + "'" +
                            " WHERE cPaperID = '" + strPaperID + "' ";
            sqldb.ExecuteNonQuery(strSQL);
        }

        /// <summary>
        /// 修改填寫理由最大字數
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strIsChoiceQuestionsFillReasons"></param>
        public void UpdatePaperMaximumNumberOfWordsReasonsOfPaper_Header(string strPaperID, string strMaximumNumberOfWordsReasons)
        {
            string strSQL = " UPDATE Paper_Header SET cMaximumNumberOfWordsReasons = '" + strMaximumNumberOfWordsReasons + "'" +
                            " WHERE cPaperID = '" + strPaperID + "' ";
            sqldb.ExecuteNonQuery(strSQL);
        }

        /// <summary>
        /// 修改一個題目的分數
        /// </summary>
        /// <param name="strQID"></param>
        public static void UpdateQuestionScore(string strQID, int intScore, string strPaperID)
        {
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = " UPDATE Paper_Content SET cQuestionScore = '" + intScore + "'" +
                             " WHERE cQID = '" + strQID + "' AND cPaperID='"+strPaperID+"' ";
            myDB.ExecuteNonQuery(strSQL);

        }
        /// <summary>
        /// 得到一個題目的分數
        /// </summary>
        /// <param name="strQID"></param>
        public static int GetQuestionScore(string strQID, string strPaperID)
        {
            int Score = 0;
            string strSQL = " SELECT cQuestionScore FROM Paper_Content" +
                            " WHERE cQID = '" + strQID + "' AND cPaperID='" + strPaperID + "'";
           
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsScore = myDB.getDataSet(strSQL);
            Score = int.Parse(dsScore.Tables[0].Rows[0][0].ToString());

            return Score;
        }

        /// <summary>
        /// 得到一張考卷的題目
        /// </summary>
        /// <param name="strQID"></param>
        public static DataSet GetPaperQuestion(string strPaperID)
        {
            DataSet dsQuestionNumber = new DataSet();
            string strSQL = " SELECT * FROM Paper_Content" +
                            " WHERE cPaperID='" + strPaperID + "'";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            dsQuestionNumber = myDB.getDataSet(strSQL);
            return dsQuestionNumber;
        }

        /// <summary>
        /// 儲存一筆資料至Paper_ItemTitle
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strItem"></param>
        /// <param name="strType"></param>
        /// <param name="strTitle"></param>
        public void SaveToPaper_ItemTitle(string strPaperID, string strItem, string strType, string strTitle)
        {
            string strSQL = " SELECT * FROM Paper_ItemTitle" +
                            " WHERE cPaperID = '" + strPaperID + "' AND cItem = '" + strItem + "'";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_ItemTitle SET sQIDorQuestionGroupID = '" + strType + "' , cTitle = '" + strTitle + "' " +
                        " WHERE cPaperID = '" + strPaperID + "' AND cItem = '" + strItem + "'";
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_ItemTitle (cPaperID , cItem , sQIDorQuestionGroupID , cTitle) " +
                        " VALUES ('" + strPaperID + "' , '" + strItem + "' , '" + strType + "' , '" + strTitle + "') ";
            }
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            dsCheck.Dispose();
        }

        public void saveSpecificToPaper_Content(string strPaperID)
        {
            //找出此問卷所有尚未被存至Paper_Content的Specific問題
            string strSQL = this.getSpecificQuestionLevel1NotSelect(strPaperID);
            DataSet dsQuestion = sqldb.getDataSet(strSQL);
            if (dsQuestion.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsQuestion.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        //QID
                        string strQID = dsQuestion.Tables[0].Rows[i]["cQID"].ToString();

                        //QuestonType
                        string strQuestionType = dsQuestion.Tables[0].Rows[i]["cQuestionType"].ToString();

                        //QuestionMode
                        string strQuestionMode = dsQuestion.Tables[0].Rows[i]["cQuestionMode"].ToString();

                        //取得題號
                        int intQuestionIndex = this.getMaxQuestionIndexFromContent(strPaperID);
                        intQuestionIndex += 1;

                        this.SaveToQuestionContent(strPaperID, strQID, "0", strQuestionType, strQuestionMode, intQuestionIndex.ToString());
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                //沒有尚未被存至Paper_Content的Specific問題
            }
            dsQuestion.Dispose();
        }

        public int getMaxQuestionIndexFromContent(string strPaperID)
        {
            int intReturn = 0;
            string strSQL = "SELECT MAX(sSeq) AS 'Index' FROM Paper_Content WHERE (cPaperID = '" + strPaperID + "') ";
            DataSet dsIndex = sqldb.getDataSet(strSQL);
            if (dsIndex.Tables[0].Rows.Count > 0)
            {
                try
                {
                    intReturn = Convert.ToInt32(dsIndex.Tables[0].Rows[0]["Index"]);
                }
                catch
                {
                }
            }
            dsIndex.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷在Paper_RandomQuestionNum的資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static DataTable getPaperInformationFromPaper_RandomQuestionNum(string strPaperID)
        {
            string strSQL = "SELECT * FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "'";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }

        /// <summary>
        /// 傳回某問卷在Paper_RandomQuestionNum的資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static DataTable getPaperInformationFromPaper_RandomQuestionNumWithGroupName(string strPaperID)
        {
            string strSQL = "SELECT * FROM Paper_RandomQuestionNum R , QuestionGroupTree T WHERE R.cPaperID = '" + strPaperID + "' AND T.cNodeID = R.cQuestionGroupID ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }

        /// <summary>
        /// 取得Paper_Content INNER JOIN QuestionMode下某問卷的所有問題資料
        /// </summary>
        /// <returns></returns>
        public static DataTable getPaperContentJoinQuestionMode(string strPaperID)
        {
            DataTable dt = new DataTable();

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = "SELECT * FROM Paper_Content C , QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = M.cQID ORDER BY sSeq ";
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            ds.Dispose();

            return dt;
        }

        /// <summary>
        /// 傳回某題目與某問題Join的詳細資料
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        /// <returns></returns>
        public static DataTable getQuestionAndSelectionContentByQIDSelectionID(string strQID, string strSelectionID)
        {
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = "SELECT cQuestion , sLevel , sSeq , cSelection , cResponse , bCaseSelect FROM QuestionIndex Q , QuestionSelectionIndex S WHERE Q.cQID = S.cQID AND Q.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "' ";
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dt = ds.Tables[0];
            ds.Dispose();
            return dt;
        }

        /// <summary>
        /// 傳回某個題目下的所有選項清單
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public static DataTable getSelectionListFromQuestionSelectionIndexByQID(string strQID)
        {
            string strSQL = "SELECT cSelectionID , sSeq , cSelection , bCaseSelect , cResponse FROM QuestionSelectionIndex WHERE cQID = '" + strQID + "' ORDER BY sSeq ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dtReturn = ds.Tables[0];
            ds.Dispose();
            return dtReturn;
        }

        /// <summary>
        /// 傳回某個使用者操作某問卷的QID明細
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public static DataTable getSelectionListFromSummary_PaperSelectionAnswerByQID(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            string strSQL = "SELECT cSelectionID , sSeq , cSelection , bCaseSelect FROM Summary_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ORDER BY sSeq ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dtReturn = ds.Tables[0];
            ds.Dispose();
            return dtReturn;
        }

        /// <summary>
        /// 傳回某個使用者操作某問卷的Selection明細
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public static DataTable getSelectionListFromTempLog_PaperSelectionAnswerByQID(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            string strSQL = "SELECT cSelectionID , sSeq , cSelection , bCaseSelect FROM TempLog_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ORDER BY sSeq ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dtReturn = ds.Tables[0];
            ds.Dispose();
            return dtReturn;
        }

        /// <summary>
        /// 傳回某個使用者操作某問卷的QID明細
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public static DataTable getQuestionIDListFromSummary_PaperSelectionAnswer(string strPaperID, string strStartTime, string strUserID)
        {
            string strSQL = "SELECT DISTINCT cQID FROM Summary_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dtReturn = ds.Tables[0];
            ds.Dispose();
            return dtReturn;
        }

        /// <summary>
        /// 傳回某個使用者操作某問卷的QID明細
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public static DataTable getQuestionIDListFromTempLog_PaperSelectionAnswer(string strPaperID, string strStartTime, string strUserID)
        {
            string strSQL = "SELECT DISTINCT cQID FROM TempLog_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            DataTable dtReturn = ds.Tables[0];
            ds.Dispose();
            return dtReturn;
        }

        //Paper_ItemTitle----------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 檢查某問卷的某問題有沒有編輯問題Title
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public bool checkExistQuestionTitle(string strPaperID, string strItem)
        {
            bool bReturn = false;
            string strSQL = "SELECT cItem FROM Paper_ItemTitle WHERE cPaperID = '" + strPaperID + "' AND cItem = '" + strItem + "' ";
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            ds.Dispose();
            return bReturn;
        }

        //PaperPhyAct----------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 傳回TempLog_PaperPhyAct的一筆紀錄
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        /// <param name="strTimes"></param>
        /// <param name="strMin"></param>
        /// <returns></returns>
        public DataRow[] getARecordFromTempLog_PaperPhyAct(string strPaperID, string strStartTime, string strUserID, string strQID, string strSelectionID)
        {
            string strSQL = " SELECT * FROM TempLog_PaperPhyAct" +
                " WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            DataSet ds = sqldb.getDataSet(strSQL);
            DataRow[] drReturn = ds.Tables[0].Select("cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ");
            ds.Dispose();
            return drReturn;
        }

        /// <summary>
        /// 寫入一筆資料至TempLog_PaperPhyAct
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        /// <param name="strWeek"></param>
        /// <param name="strMin"></param>
        public void SaveToTempLog_PaperPhyAct(string strPaperID, string strStartTime, string strUserID, string strQID, string strSelectionID, string strTimes, string strMin)
        {
            string strSQL = " SELECT * FROM TempLog_PaperPhyAct" +
                " WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE TempLog_PaperPhyAct SET sTimes = '" + strTimes + "' , sMin = '" + strMin + "' " +
                    " WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO TempLog_PaperPhyAct (cPaperID , cStartTime , cUserID , cQID , cSelectionID , sTimes , sMin) " +
                    " VALUES ('" + strPaperID + "' , '" + strStartTime + "' , '" + strUserID + "' , '" + strQID + "' , '" + strSelectionID + "' , '" + strTimes + "' , '" + strMin + "') ";
            }
            dsCheck.Dispose();
            sqldb.ExecuteNonQuery(strSQL);
        }

        /// <summary>
        /// 傳回Paper_AssignedQuestionBySeq某使用者某次操作紀錄的某一題號的QID
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strSeq"></param>
        /// <returns></returns>
        public string getQIDFromPaper_AssignedQuestionBySeq(string strPaperID, string strStartTime, string strUserID, string strSeq)
        {
            string strReturn = "";

            string strSQL = "";
            strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND sSeq = '" + strSeq + "' ";

            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                strReturn = dsCheck.Tables[0].Rows[0]["cQID"].ToString();
            }
            dsCheck.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回Paper_AssignedQuestionBySeq某使用者某次操作紀錄的某一題號的DataRow
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strSeq"></param>
        /// <returns></returns>
        public DataRow[] getDaraRowFromPaper_AssignedQuestionBySeq(string strPaperID, string strStartTime, string strUserID, string strSeq)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND sSeq = '" + strSeq + "' ORDER BY sSeq ";

            DataSet dsCheck = sqldb.getDataSet(strSQL);
            DataRow[] drReturn = dsCheck.Tables[0].Select("sSeq = '" + strSeq + "'", "ORDER BY sSeq");
            dsCheck.Dispose();

            return drReturn;
        }

        /// <summary>
        /// 檢查某一個題目是否存在於Paper_AssignedQuestion
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSeq"></param>
        /// <returns></returns>
        public bool checkPaper_AssignedQuestion(string strPaperID, string strStartTime, string strUserID, string strSeq)
        {
            bool bReturn = false;

            string strSQL = "";
            strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND sSeq = '" + strSeq + "' ";

            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }
            dsCheck.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 傳回某個使用者再Paper_AssignedQuestion中某一份問卷的所有資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public DataTable getUserQuestionInPaper_AssignedQuestion(string strPaperID, string strStartTime, string strUserID)
        {
            DataTable dtReturn = new DataTable();

            string strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ORDER BY sSeq";
            DataSet ds = sqldb.getDataSet(strSQL);
            dtReturn = ds.Tables[0];
            ds.Dispose();

            return dtReturn;
        }

        /// <summary>
        /// 把資料存入Paper_CaseDivision
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <param name="strDivisionID"></param>
        public void saveToPaper_CaseDivision(string strPaperID, string strCaseID, string strDivisionID)
        {
            string strSQL = " SELECT * FROM Paper_CaseDivision" +
                " WHERE cCaseID = '" + strCaseID + "' AND cDivisionID = '" + strDivisionID + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_CaseDivision SET cPaperID = '" + strPaperID + "' " +
                    " WHERE cCaseID = '" + strCaseID + "' AND cDivisionID = '" + strDivisionID + "' ";
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_CaseDivision (cCaseID , cDivisionID , cPaperID) " +
                    " VALUES ('" + strCaseID + "' , '" + strDivisionID + "' , '" + strPaperID + "') ";
            }
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            dsCheck.Dispose();
        }

        /// <summary>
        /// 把資料存入Paper_CaseDivisionSection
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <param name="strClinicNum"></param>
        /// <param name="strSectionName"></param>
        public void saveToPaper_CaseDivisionSection(string strPaperID, string strCaseID, string strClinicNum, string strSectionName)
        {
            string strSQL = " SELECT * FROM Paper_CaseDivisionSection " +
                " WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + strClinicNum + "' AND cSectionName = '" + strSectionName + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_CaseDivisionSection SET cPaperID = '" + strPaperID + "' " +
                    " WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + strClinicNum + "' AND cSectionName = '" + strSectionName + "' ";
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_CaseDivisionSection (cCaseID , sClinicNum , cSectionName , cPaperID) " +
                    " VALUES ('" + strCaseID + "' , '" + strClinicNum + "'  ,'" + strSectionName + "' , '" + strPaperID + "') ";
            }
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            dsCheck.Dispose();
        }

        /// <summary>
        /// 傳回資料庫中所有的Division資料
        /// </summary>
        /// <returns></returns>
        public string getEveryDivision()
        {
            return "SELECT * FROM Division ORDER BY cDivisionID";
        }

        /// <summary>
        /// 檢查使用者再某次操作紀錄中是不是有操作過某問卷(check TempLog , Summary)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public bool checkUserDone(string strPaperID, string strStartTime, string strUserID)
        {
            bool bReturn = false;

            bReturn = (this.checkTemplogDone(strPaperID, strStartTime, strUserID) || this.checkSummaryDone(strPaperID, strStartTime, strUserID));

            return bReturn;
        }

        /// <summary>
        /// 檢查TempLog_PaperHeader，使用者是不是已經做過此問卷。
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public bool checkTemplogDone(string strPaperID, string strStartTime, string strUserID)
        {
            bool bReturn = false;
            string strSQL = "SELECT * FROM TempLog_PaperHeader WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            dsCheck.Dispose();
            return bReturn;
        }

        /// <summary>
        /// 檢查Summary_PaperHeader，使用者是不是已經做過此問卷。
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public bool checkSummaryDone(string strPaperID, string strStartTime, string strUserID)
        {
            bool bReturn = false;
            string strSQL = "SELECT * FROM Summary_PaperHeader WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            dsCheck.Dispose();
            return bReturn;
        }

        public void saveToPaper_Header(string strPaperID, string strPaperName, string strTitle, string strObjective, string strEditMethod, string strGenerationMethod, string strPresentMethod, string strTestDuration, string strPresentType)
        {
            //儲存資料至Paper_Header
            string strSQL = "";
            //判斷此紀錄是否存在資料庫
            strSQL = "SELECT * FROM Paper_Header WHERE cPaperID = '" + strPaperID + "'";
            int intRowCount = 0;
            DataSet dsPaper = sqldb.getDataSet(strSQL);
            intRowCount = dsPaper.Tables[0].Rows.Count;
            dsPaper.Dispose();

            if (intRowCount > 0)
            {
                //修改到資料庫
                strSQL = "UPDATE Paper_Header SET cPaperName = '" + strPaperName + "' , cTitle = '" + strTitle + "' , cObjective = '" + strObjective + "' , cEditMethod = '" + strEditMethod + "' ,  cGenerationMethod = '" + strGenerationMethod + "' , cPresentMethod = '" + strPresentMethod + "' , sTestDuration = '" + strTestDuration + "' , cPresentType = '" + strPresentType + "' " +
                    "WHERE cPaperID = '" + strPaperID + "' ";
            }
            else
            {
                //新增到資料庫
                strSQL = " INSERT Paper_Header (cPaperID , cPaperName , cTitle , cObjective , cEditMethod ,  cGenerationMethod , cPresentMethod , sTestDuration , cPresentType) " +
                    " VALUES('" + strPaperID + "' , '" + strPaperName + "' , '" + strTitle + "' , '" + strObjective + "' , '" + strEditMethod + "' , '" + strGenerationMethod + "' , '" + strPresentMethod + "' , '" + strTestDuration + "' , '" + strPresentType + "') ";
            }
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 刪除某問卷中某個亂數選取的問題組別的資料(DELETE FROM Paper_RandomQuestionNum)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupID"></param>
        public void deletePaper_RandomQuestionNum(string strPaperID, string strGroupID)
        {
            string strSQL = "DELETE FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "' AND cQuestionGroupID = '" + strGroupID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 傳回某問卷再Paper_RandomQuestionNum的所有資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaper_RandomQuestionNum(string strPaperID)
        {
            return "SELECT * FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "'";
        }

        /// <summary>
        /// 傳回某問卷的某個問題組別再Paper_RandomQuestionNum的所有資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getPaper_RandomQuestionNumByGroup(string strPaperID, string strGroupID)
        {
            return "SELECT * FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "' AND cQuestionGroupID = '" + strGroupID + "' ";
        }

        public string getPaper_RandomQuestionNumBySpecific(string strPaperID)
        {
            return "SELECT * FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "' AND cQuestionGroupID = 'Specific' ";
        }

        /// <summary>
        /// 把資料存入Paper_RandomQuestionNum
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupID"></param>
        /// <param name="intQuestionNum"></param>
        public void saveRandomQuestionNum(string strPaperID, string strGroupID, int intQuestionNum, int iQuestionLevel)
        {
            string strSQL = "SELECT * FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "' AND cQuestionGroupID = '" + strGroupID + "' AND sQuestionLevel = '" + iQuestionLevel + "' ";
            DataSet dsRandom = sqldb.getDataSet(strSQL);
            if (dsRandom.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_RandomQuestionNum SET sQuestionNum = '" + intQuestionNum.ToString() + "' " +
                    " WHERE cPaperID = '" + strPaperID + "' AND cQuestionGroupID = '" + strGroupID + "' AND sQuestionLevel = '" + iQuestionLevel + "' ";
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_RandomQuestionNum (cPaperID , cQuestionGroupID , sQuestionNum, sQuestionLevel ) " +
                    " VALUES ('" + strPaperID + "' , '" + strGroupID + "' , '" + intQuestionNum.ToString() + "', '" + iQuestionLevel + "')";
            }
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            dsRandom.Dispose();
        }

        /// <summary>
        /// 把資料存入Paper_RandomQuestionNum
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="intQuestionNum"></param>
        public void saveRandomSpecificQuestionNum(string strPaperID, int intQuestionNum)
        {
            string strSQL = "SELECT * FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "' AND cQuestionGroupID = 'Specific' ";
            DataSet dsRandom = sqldb.getDataSet(strSQL);
            if (dsRandom.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_RandomQuestionNum SET sQuestionNum = '" + intQuestionNum.ToString() + "' " +
                    " WHERE cPaperID = '" + strPaperID + "' AND cQuestionGroupID = 'Specific' ";
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_RandomQuestionNum (cPaperID , cQuestionGroupID , sQuestionNum) " +
                    " VALUES ('" + strPaperID + "' , 'Specific' , '" + intQuestionNum.ToString() + "')";
            }
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            dsRandom.Dispose();
        }

        //		--取得每除了實驗室的成員之外的每個人第一次填此問卷的紀錄
        //		SELECT P.cPaperID , Min(cStartTime) AS cStartTime , P.cUserID FROM Summary_PaperHeader P , HintsUser H 
        //		WHERE P.cPaperID = 'swakevin20050907120001' 
        //		AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'
        //		GROUP BY P.cPaperid , P.cUserID 

        /// <summary>
        /// 傳回某個組別第一次操作教案的Summary紀錄
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <param name="strGroupName"></param>
        /// <returns></returns>
        public string getCaseGroupDuration(string strPaperID, string strCaseID, string strGroupName)
        {
            return " SELECT A.cFullName , A.cUserID , A.cStartTime , B.cBackupTime FROM " +
                " ( " +
                " SELECT H.cFullName , Min(P.cStartTime) AS cStartTime , P.cUserID , P.cPaperID , S.cCaseID " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G , TeachingCase T  " +
                " WHERE P.cPaperID = '" + strPaperID + "'  AND G.cGroup = '" + strGroupName + "' AND T.cCaseID = '" + strCaseID + "'  " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'   " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " AND G.cUserID = S.cUserID " +
                " AND S.cCaseID = T.cCaseID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName  " +
                " ) A  " +
                " LEFT JOIN Summary_Header B  " +
                " ON A.cCaseID = B.cCaseID AND A.cUserID = B.cUserID AND A.cStartTime = B.cStartTime  " +
                " ORDER BY A.cUserID  ";
        }

        /// <summary>
        /// 傳回某個班級第一次操作教案的Summary紀錄
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <param name="strClassName"></param>
        /// <returns></returns>
        public string getCaseClassDuration(string strPaperID, string strCaseID, string strClassName)
        {
            return " SELECT A.cFullName , A.cUserID , A.cStartTime , B.cBackupTime FROM " +
                " ( " +
                " SELECT H.cFullName , Min(P.cStartTime) AS cStartTime , P.cUserID , P.cPaperID , S.cCaseID " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , TeachingCase T " +
                " WHERE P.cPaperID = '" + strPaperID + "'  AND H.cClass = '" + strClassName + "' AND T.cCaseID = '" + strCaseID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " AND S.cCaseID = T.cCaseID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) A " +
                " LEFT JOIN Summary_Header B " +
                " ON A.cCaseID = B.cCaseID AND A.cUserID = B.cUserID AND A.cStartTime = B.cStartTime " +
                " ORDER BY A.cUserID ";
        }

        /// <summary>
        /// 傳回某個教案所有學生第一次操作的Summary_Header紀錄
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <returns></returns>
        public string getCaseDuration(string strPaperID, string strCaseID)
        {
            return " SELECT A.cFullName , A.cUserID , A.cStartTime , B.cBackupTime FROM " +
                " ( " +
                " SELECT H.cFullName , Min(P.cStartTime) AS cStartTime , P.cUserID , P.cPaperID , S.cCaseID " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , TeachingCase T " +
                " WHERE P.cPaperID = '" + strPaperID + "'  AND T.cCaseID = '" + strCaseID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " AND S.cCaseID = T.cCaseID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) A " +
                " LEFT JOIN Summary_Header B " +
                " ON A.cCaseID = B.cCaseID AND A.cUserID = B.cUserID AND A.cStartTime = B.cStartTime " +
                " ORDER BY A.cUserID ";
        }

        /// <summary>
        /// By Class 傳回學生操作此問卷的選擇題人數
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        /// <param name="strCaseID"></param>
        /// <returns></returns>
        public string getSelectionSummaryByCase(string strPaperID, string strQID, string strSelectionID, string strCaseID)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S  " +
                " WHERE P.cPaperID = '" + strPaperID + "'  " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName  " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = '" + strCaseID + "' " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID  AND T.cCaseID = '" + strCaseID + "' " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'   " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime  ";
            // SELECT * FROM 
            // ( 
            // SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            // ( 
            // SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            // FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            // WHERE P.cPaperID = 'swakevin20050907120001' 
            // AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            // AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            // GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            // ) P 
            // INNER JOIN 
            // ( 
            // SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S 
            // ) S 
            // ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = 'wytCase200509091518424531250'
            // INNER JOIN 
            // TeachingCase T 
            // ON S.cCaseID = T.cCaseID  AND T.cCaseID = 'wytCase200509091518424531250'
            // ) H 
            // INNER JOIN Summary_PaperSelectionAnswer S 
            // ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            // AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getSelectionSummaryByCaseClass(string strPaperID, string strQID, string strSelectionID, string strCaseID, string strClass)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S  " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName  " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = '" + strCaseID + "' " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID  AND T.cCaseID = '" + strCaseID + "' " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'   " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime  ";
            //SELECT * FROM 
            // ( 
            //	 SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            //	 ( 
            //	 SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //	 FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //	 WHERE P.cPaperID = 'swakevin20050907120001' AND H.cClass = '97'
            //	 AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //	 AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	 ) P 
            //	INNER JOIN 
            //	 ( 
            //	 SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S 
            //	 ) S 
            //	 ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = 'wytCase200509091518424531250'
            //	 INNER JOIN 
            //	 TeachingCase T 
            //	 ON S.cCaseID = T.cCaseID  AND T.cCaseID = 'wytCase200509091518424531250'
            // ) H 
            // INNER JOIN Summary_PaperSelectionAnswer S 
            // ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            // AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getSelectionSummaryByCaseGroup(string strPaperID, string strQID, string strSelectionID, string strCaseID, string strGroup)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G  " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND G.cGroup = '" + strGroup + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = '" + strCaseID + "' " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID  AND T.cCaseID = '" + strCaseID + "' " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'   " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime  ";
            // SELECT * FROM 
            // ( 
            //	 SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            //	 ( 
            //	 SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //	 FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G 
            //	 WHERE P.cPaperID = 'swakevin20050907120001' AND G.cGroup = 'T3'
            //	 AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //	 AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //	 AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID 
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	 ) P 
            //	INNER JOIN 
            //	 ( 
            //	 SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S 
            //	 ) S 
            //	 ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = 'wytCase200509091518424531250'
            //	 INNER JOIN 
            //	 TeachingCase T 
            //	 ON S.cCaseID = T.cCaseID  AND T.cCaseID = 'wytCase200509091518424531250'
            // ) H 
            // INNER JOIN Summary_PaperSelectionAnswer S 
            // ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            // AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getSelectionSummaryByAuthor(string strPaperID, string strQID, string strSelectionID, string strAuthorID)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S  " +
                " WHERE P.cPaperID = '" + strPaperID + "'  " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName  " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID AND T.cFullName = '" + strAuthorID + "' " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'  " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime ";
            //SELECT * FROM 
            //( 
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            //( 
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' 
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P 
            //INNER JOIN 
            //( 
            //SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S 
            //) S 
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID AND T.cFullName = '林毓志醫師'
            //) H 
            //INNER JOIN Summary_PaperSelectionAnswer S 
            //ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            //AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getSelectionSummaryByAuthorClass(string strPaperID, string strQID, string strSelectionID, string strAuthorID, string strClass)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S  " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName  " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID AND T.cFullName = '" + strAuthorID + "' " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'  " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime ";
            //SELECT * FROM 
            //( 
            //	SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            //	( 
            //	SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //	FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //	WHERE P.cPaperID = 'swakevin20050907120001' AND H.cClass = '97'
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //	AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	) P 
            //	INNER JOIN 
            //	( 
            //	SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S 
            //	) S 
            //	ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //	INNER JOIN 
            //	TeachingCase T 
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '林毓志醫師'
            //) H 
            //INNER JOIN Summary_PaperSelectionAnswer S 
            //ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            //AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getSelectionSummaryByAuthorGroup(string strPaperID, string strQID, string strSelectionID, string strAuthorID, string strGroup)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND G.cGroup = '" + strGroup + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID AND T.cFullName = '" + strAuthorID + "' " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'  " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime ";
            //SELECT * FROM 
            //( 
            //	SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            //	( 
            //	SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //	FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G
            //	WHERE P.cPaperID = 'swakevin20050907120001' AND G.cGroup = 'B3'
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //	AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            // 	AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID 
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	) P 
            //	INNER JOIN 
            //	( 
            //	SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S 
            //	) S 
            //	ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //	INNER JOIN 
            //	TeachingCase T 
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '林毓志醫師'
            //) H 
            //INNER JOIN Summary_PaperSelectionAnswer S 
            //ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            //AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getSelectionSummaryByClass(string strPaperID, string strQID, string strSelectionID, string strClass)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S  " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND cClass = '" + strClass + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName  " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID  " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'   " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime ";
            //SELECT * FROM 
            //( 
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            //( 
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' AND cClass = '95'
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P 
            //INNER JOIN 
            //( 
            //SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S 
            //) S 
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID 
            //) H 
            //INNER JOIN Summary_PaperSelectionAnswer S 
            //ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            //AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getSelectionSummaryByGroup(string strPaperID, string strQID, string strSelectionID, string strGroup)
        {
            return " SELECT * FROM " +
                " (  " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM  " +
                " (  " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName  " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S  " +
                " WHERE P.cPaperID = '" + strPaperID + "'  " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE'  " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime   " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName  " +
                " ) P  " +
                " INNER JOIN  " +
                " (  " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S , UserGroup G  " +
                " WHERE G.cGroup = '" + strGroup + "' AND S.cUserID = G.cUserID  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID  " +
                " ) H  " +
                " INNER JOIN Summary_PaperSelectionAnswer S  " +
                " ON S.cQID = '" + strQID + "' AND S.cSelectionID = '" + strSelectionID + "'   " +
                " AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime  ";

            //SELECT * FROM 
            //( 
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM 
            //( 
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' 
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P 
            //INNER JOIN 
            //( 
            //SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S , UserGroup G 
            //WHERE G.cGroup = 'T3' AND S.cUserID = G.cUserID 
            //) S 
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID 
            //) H 
            //INNER JOIN Summary_PaperSelectionAnswer S 
            //ON S.cQID = 'swakevin_Question_200509071204458440192' AND S.cSelectionID = 'swakevin_Selection_200509071205444883456'  
            //AND H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime 
        }

        public string getTextSummaryByGroup(string strPaperID, string strQID, string strGroup)
        {
            //By Group 傳回學生操作此問卷的問答題答案
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN  " +
                " ( " +
                " SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S , UserGroup G  " +
                " WHERE G.cGroup = '" + strGroup + "' AND S.cUserID = G.cUserID  " +
                " ) S  " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID  " +
                " ) H  " +
                " INNER JOIN Summary_PaperTextQuestion S  " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "'";
            //SELECT * FROM
            //(
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM
            //(
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' 
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P
            //INNER JOIN 
            //(
            //SELECT S.cStartTime , S.cUserID , S.cCaseID FROM Summary_Header S , UserGroup G 
            //WHERE G.cGroup = 'T3' AND S.cUserID = G.cUserID 
            //) S 
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByClass(string strPaperID, string strQID, string strClass)
        {
            //By Class 傳回學生操作此問卷的問答題答案
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN  " +
                " Summary_Header S " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  " +
                " INNER JOIN  " +
                " TeachingCase T  " +
                " ON S.cCaseID = T.cCaseID  " +
                " ) H  " +
                " INNER JOIN Summary_PaperTextQuestion S  " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "'";
            //SELECT * FROM
            //(
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM
            //(
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' AND H.cClass = '95'
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P
            //INNER JOIN 
            //Summary_Header S
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByAuthor(string strPaperID, string strQID, string strAuthorID)
        {
            //By Author 傳回學生操作此問卷的問答題答案
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN " +
                " Summary_Header S " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " INNER JOIN " +
                " TeachingCase T " +
                " ON S.cCaseID = T.cCaseID AND T.cFullName = '" + strAuthorID + "' " +
                " ) H " +
                " INNER JOIN Summary_PaperTextQuestion S " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "' ";
            //SELECT * FROM
            //(
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM
            //(
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' 
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P
            //INNER JOIN 
            //Summary_Header S
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID AND T.cFullName = 'afeng' 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByAuthorClass(string strPaperID, string strQID, string strAuthorID, string strClass)
        {
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN " +
                " Summary_Header S " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " INNER JOIN " +
                " TeachingCase T " +
                " ON S.cCaseID = T.cCaseID AND T.cFullName = '" + strAuthorID + "' " +
                " ) H " +
                " INNER JOIN Summary_PaperTextQuestion S " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "' ";
            //SELECT * FROM
            //(
            //	SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM
            //	(
            //	SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //	FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //	WHERE P.cPaperID = 'swakevin20050907120001' AND H.cClass = '97'
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //	AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	) P
            //	INNER JOIN 
            //	Summary_Header S
            //	ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //	INNER JOIN 
            //	TeachingCase T 
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '林其和醫師' 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByAuthorGroup(string strPaperID, string strQID, string strAuthorID, string strGroup)
        {
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND G.cGroup = '" + strGroup + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN " +
                " Summary_Header S " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " INNER JOIN " +
                " TeachingCase T " +
                " ON S.cCaseID = T.cCaseID AND T.cFullName = '" + strAuthorID + "' " +
                " ) H " +
                " INNER JOIN Summary_PaperTextQuestion S " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "' ";
            //SELECT * FROM
            //(
            //	SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM
            //	(
            //	SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //	FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G
            //	WHERE P.cPaperID = 'swakevin20050907120001' AND G.cGroup = 'B3'
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //	AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //	AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID 
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	) P
            //	INNER JOIN 
            //	Summary_Header S
            //	ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //	INNER JOIN 
            //	TeachingCase T 
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '林其和醫師' 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByCase(string strPaperID, string strQID, string strCaseID)
        {
            //By Case 傳回學生操作此問卷的問答題答案
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , P.cFullName FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN " +
                " Summary_Header S " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = '" + strCaseID + "'" +
                " INNER JOIN " +
                " TeachingCase T " +
                " ON S.cCaseID = T.cCaseID AND T.cCaseID = '" + strCaseID + "' " +
                " ) H " +
                " INNER JOIN Summary_PaperTextQuestion S " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "' ";
            //SELECT * FROM
            //(
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , P.cFullName  FROM
            //(
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' 
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P
            //INNER JOIN 
            //Summary_Header S
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID AND T.cCaseID = 'wytCase200509151018281562500' 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByCaseClass(string strPaperID, string strQID, string strCaseID, string strClass)
        {
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , P.cFullName FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN " +
                " Summary_Header S " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = '" + strCaseID + "'" +
                " INNER JOIN " +
                " TeachingCase T " +
                " ON S.cCaseID = T.cCaseID AND T.cCaseID = '" + strCaseID + "' " +
                " ) H " +
                " INNER JOIN Summary_PaperTextQuestion S " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "' ";
            //SELECT * FROM
            //(
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , P.cFullName  FROM
            //(
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S 
            //WHERE P.cPaperID = 'swakevin20050907120001' AND H.cClass = '95' 
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P
            //INNER JOIN 
            //Summary_Header S
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID AND T.cCaseID = 'wytCase200509151018281562500' 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByCaseGroup(string strPaperID, string strQID, string strCaseID, string strGroup)
        {
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , P.cFullName FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND G.cGroup = '" + strGroup + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) P " +
                " INNER JOIN " +
                " Summary_Header S " +
                " ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = '" + strCaseID + "'" +
                " INNER JOIN " +
                " TeachingCase T " +
                " ON S.cCaseID = T.cCaseID AND T.cCaseID = '" + strCaseID + "' " +
                " ) H " +
                " INNER JOIN Summary_PaperTextQuestion S " +
                " ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '" + strQID + "' ";
            //SELECT * FROM
            //(
            //SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , P.cFullName  FROM
            //(
            //SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName 
            //FROM Summary_PaperHeader P , HintsUser H , Summary_Header S , UserGroup G 
            //WHERE P.cPaperID = 'swakevin20050907120001' AND G.cGroup = 'B3' 
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' 
            //AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID 
            //GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //) P
            //INNER JOIN 
            //Summary_Header S
            //ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //INNER JOIN 
            //TeachingCase T 
            //ON S.cCaseID = T.cCaseID AND T.cCaseID = 'wytCase200509151018281562500' 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些班級操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strClassName"></param>
        /// <returns></returns>
        public string getClassGroupPaperList(string strPaperID, string strClassName)
        {
            return "SELECT DISTINCT G.cGroup " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , UserGroup G , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClassName + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                "AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些班級操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getGroupPaperList(string strPaperID)
        {
            return "SELECT DISTINCT G.cGroup " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , UserGroup G , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                "AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些班級操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strClass"></param>
        /// <param name="strAuthorID"></param>
        /// <returns></returns>
        public string getAuthorGroupPaperList(string strPaperID, string strClass, string strAuthorID)
        {
            return "SELECT DISTINCT G.cGroup " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , UserGroup G , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' AND T.cFullName = '" + strAuthorID + "' " +
                "AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些班級操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strClass"></param>
        /// <param name="strCaseID"></param>
        /// <returns></returns>
        public string getCaseGroupPaperList(string strPaperID, string strClass, string strCaseID)
        {
            return "SELECT DISTINCT G.cGroup " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , UserGroup G , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' AND T.cCaseID = '" + strCaseID + "' " +
                "AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些班級操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getClassPaperList(string strPaperID)
        {
            return "SELECT DISTINCT H.cClass " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "ORDER BY H.cClass ";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些班級操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strAuthorID"></param>
        /// <returns></returns>
        public string getAuthorClassPaperList(string strPaperID, string strAuthorID)
        {
            return "SELECT DISTINCT H.cClass " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND T.cFullName = '" + strAuthorID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "ORDER BY H.cClass ";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些班級操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <returns></returns>
        public string getCaseClassPaperList(string strPaperID, string strCaseID)
        {
            return "SELECT DISTINCT H.cClass " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND T.cCaseID = '" + strCaseID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "ORDER BY H.cClass ";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些Author所編輯的Case有被操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getAuthorPaperList(string strPaperID)
        {
            return "SELECT DISTINCT T.cFullName " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "ORDER BY T.cFullName ";

            //			return  "SELECT DISTINCT T.cProvider , T.cFullName "+
            //					"FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H "+
            //					"WHERE P.cPaperID = '"+strPaperID+"' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID "+
            //					"AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' "+
            //					"ORDER BY T.cFullName ";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些Case有被操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getCasePaperList(string strPaperID)
        {
            return "SELECT DISTINCT T.cCaseID , T.cCaseName " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "ORDER BY cCaseName ";
        }

        /// <summary>
        /// 傳回使用者操作的紀錄與Summary_Header的Inner Join，幫助建立哪些Case有被操做過問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strAuthorID"></param>
        /// <returns></returns>
        public string getAuthorCasePaperList(string strPaperID, string strAuthorID)
        {
            return "SELECT DISTINCT T.cCaseID , T.cCaseName " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND T.cFullName = '" + strAuthorID + "' " +
                "AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '電通所' AND H.cClass <> 'NckuEE' " +
                "ORDER BY cCaseName ";
        }

        /// <summary>
        /// 將資料存入Paper_GroupingQuestion
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupID"></param>
        /// <param name="strGroupDivisionID"></param>
        /// <param name="strQuestionNum"></param>
        public void SaveToPaperGroupingQuestion(string strPaperID, string strGroupID, string strGroupDivisionID, string strQuestionNum)
        {
            string strSQL = "SELECT * FROM Paper_GroupingQuestion WHERE cPaperID = '" + strPaperID + "' AND cGroupID = '" + strGroupID + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Paper_GroupingQuestion SET cDivisionID = '" + strGroupDivisionID + "' , sQuestionNum = '" + strQuestionNum + "' , cGroupMethod = 'Group' " +
                    "WHERE cPaperID = '" + strPaperID + "' AND cGroupID = '" + strGroupID + "'  ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO Paper_GroupingQuestion (cPaperID , cGroupID , cDivisionID , sQuestionNum , cGroupMethod) " +
                    "VALUES ('" + strPaperID + "' , '" + strGroupID + "' , '" + strGroupDivisionID + "' , '" + strQuestionNum + "' , 'Group') ";
            }
            dsCheck.Dispose();
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 傳回某個問卷中的某個問題組別，尚未被選入Paper_Content的題目
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getGroupQuestionLevel1NotSelect(string strGroupID, string strPaperID)
        {
            return " SELECT * FROM QuestionIndex I , QuestionMode M " +
                " WHERE  M.cQuestionGroupID = '" + strGroupID + "' " +
                " AND I.cQID = M.cQID AND I.sLevel = '1'  AND NOT EXISTS " +
                " (SELECT * FROM Paper_Content C WHERE cPaperID = '" + strPaperID + "' AND I.cQID = C.cQID AND M.cQID = C.cQID ) ";
        }

        public string getSpecificQuestionLevel1NotSelect(string strPaperID)
        {
            return " SELECT * FROM QuestionIndex I , QuestionMode M " +
                " WHERE  M.cPaperID = '" + strPaperID + "' AND cQuestionMode = 'Specific' " +
                " AND I.cQID = M.cQID AND I.sLevel = '1'  AND NOT EXISTS " +
                " (SELECT * FROM Paper_Content C WHERE cPaperID = '" + strPaperID + "' AND I.cQID = C.cQID AND M.cQID = C.cQID ) ";
        }

        /// <summary>
        /// 儲存資料至Paper_CaseDivisionSection
        /// </summary>
        /// <param name="strCaseID"></param>
        /// <param name="strDivisionID"></param>
        /// <param name="strClinicNum"></param>
        /// <param name="strSectionName"></param>
        /// <param name="strPaperID"></param>
        public void SaveToPaperCaseDivision(string strCaseID, string strDivisionID, string strClinicNum, string strSectionName, string strPaperID)
        {
            string strSQL = "SELECT * FROM Paper_CaseDivisionSection WHERE cCaseID = '" + strCaseID + "' AND cDivisionID = '" + strDivisionID + "' AND sClinicNum = '" + strClinicNum + "' AND cSectionName = '" + strSectionName + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Paper_CaseDivisionSection SET cPaperID = '" + strPaperID + "' WHERE cCaseID = '" + strCaseID + "' AND cDivisionID = '" + strDivisionID + "' AND sClinicNum = '" + strClinicNum + "' AND cSectionName = '" + strSectionName + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO Paper_CaseDivisionSection (cCaseID , cDivisionID , sClinicNum , cSectionName , cPaperID) VALUES ('" + strCaseID + "' , '" + strDivisionID + "' , '" + strClinicNum + "' , '" + strSectionName + "' , '" + strPaperID + "' )";
            }
            dsCheck.Dispose();
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 刪除一個問題，刪除的欄位有(QuestionIndex , QuestionSelectionIndex , QuestionMode, QuestionLevel)
        /// </summary>
        /// <param name="strQID"></param>問題的編號
        public void DeleteGeneralQuestion(string strQID)
        {
            //刪除QuestionMode
            string strSQL = "DELETE QuestionMode WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //刪除QuestionSelectionIndex
            strSQL = "DELETE QuestionSelectionIndex WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //刪除QuestionIndex
            strSQL = "DELETE QuestionIndex WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //刪除QuestionLevel
            strSQL = "DELETE QuestionLevel WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            //刪除QuestionLevel
            strSQL = "DELETE FeatureForSelect WHERE strQuestionID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }


        /// <summary>
        /// 刪除一個情境問題，刪除的資料表有(QuestionIndex , QuestionMode , Question_Situational)
        /// </summary>
        /// <param name="strQID"></param>問題的編號
        public void DeleteSituationQuestion(string strQID)
        {
            //刪除QuestionMode
            string strSQL = "DELETE QuestionMode WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //刪除QuestionIndex
            strSQL = "DELETE QuestionIndex WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //刪除Question_Situational
            strSQL = "DELETE Question_Situational WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            //刪除QuestionLevel
            strSQL = "DELETE FeatureForSelect WHERE strQuestionID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 把一筆資料從Question_Simulator刪除
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        public void DeleteQuestion_SimulatorContent(string strQID)
        {
            //Delete this question from Paper_Content
            string strSQL = "DELETE Question_Simulator WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 把一筆資料從Paper_Content刪除
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        public void DeleteFromQuestionContent(string strPaperID, string strQID)
        {
            string strSQL = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";
            DataSet dsQuestion = sqldb.getDataSet(strSQL);
            if (dsQuestion.Tables[0].Rows.Count > 0)
            {
                //Delete this question from Paper_Content
                strSQL = "DELETE Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";
                try
                {
                    sqldb.ExecuteNonQuery(strSQL);
                }
                catch
                {
                }
            }
            dsQuestion.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <param name="strStandardScore"></param>
        /// <param name="strQuestionType">問題類型:1.選擇題2.問答題</param>
        /// <param name="strQuestionMode"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strSeq"></param>
        public void SaveToQuestionContent(string strPaperID, string strQID, string strStandardScore, string strQuestionMode, string strModifyType)
        {
            if (strModifyType == "Paper")
            {
                //找出此問題在問卷裏的題號
                string strSeq = "0";
                string strSQL = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";
                DataSet dsContent = sqldb.getDataSet(strSQL);
                if (dsContent.Tables[0].Rows.Count > 0)
                {
                    strSeq = dsContent.Tables[0].Rows[0]["sSeq"].ToString();
                }
                dsContent.Dispose();

                SaveToQuestionContent(strPaperID, strQID, strStandardScore, "1", strQuestionMode, strSeq);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <param name="strStandardScore"></param>
        /// <param name="strQuestionType">問題類型:1.選擇題2.問答題</param>
        /// <param name="strQuestionMode"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strSeq"></param>
        public void SaveToQuestionContent(string strPaperID , string strQID , string strStandardScore , string strQuestionType , string strQuestionMode , string strQuestion , string strSeq)
        {
            //將值存入Paper_QuestionContent
            string strSQL = "SELECT * FROM Paper_Content WHERE cPaperID = '"+strPaperID+"' AND cQID = '"+strQID+"' ";
            DataSet dsQuestion = sqldb.getDataSet(strSQL);
            if(dsQuestion.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_Content SET  sStandardScore = '"+strStandardScore+"' , cQuestionType = '"+strQuestionType+"' , cQuestionMode = '"+strQuestionMode+"' , cQuestion = @cQuestion , sSeq = '"+strSeq+"' "+
                         " WHERE cPaperID = '"+strPaperID+"' AND cQID = '"+strQID+"' ";

               
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_Content (cPaperID , cQID , sStandardScore , cQuestionType , cQuestionMode , cQuestion , sSeq) "+
                         " VALUES ('"+strPaperID+"' , '"+strQID+"'  ,'"+strStandardScore+"'  ,'"+strQuestionType+"'  ,'"+strQuestionMode+"'  ,@cQuestion  ,'"+strSeq+"')";
            }
            dsQuestion.Dispose();
            try
            {
                object[] pList = {strQuestion};
                sqldb.ExecuteNonQuery(strSQL,pList);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 將值存入Paper_QuestionContent
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <param name="strStandardScore"></param>
        /// <param name="strQuestionType"></param>
        /// <param name="strQuestionMode"></param>
        /// <param name="strSeq"></param>
        public void SaveToQuestionContent(string strPaperID, string strQID, string strStandardScore, string strQuestionType, string strQuestionMode, string strSeq)
        {
            string strSQL = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";
            DataSet dsQuestion = sqldb.getDataSet(strSQL);
            if (dsQuestion.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = " UPDATE Paper_Content SET  sStandardScore = '" + strStandardScore + "' , cQuestionType = '" + strQuestionType + "' , cQuestionMode = '" + strQuestionMode + "' , sSeq = '" + strSeq + "' " +
                    " WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";


                
            }
            else
            {
                //Insert
                strSQL = " INSERT INTO Paper_Content (cPaperID , cQID , sStandardScore , cQuestionType , cQuestionMode , sSeq) " +
                    " VALUES ('" + strPaperID + "' , '" + strQID + "'  ,'" + strStandardScore + "'  ,'" + strQuestionType + "'  ,'" + strQuestionMode + "'  ,'" + strSeq + "')";
            }

            /*write SQL to file to 
            //to inspect the SQL cmd when something went wrong with SQL cmd 
            // Create a file to write to.              
            //File.WriteAllText("D:/Hints_on_60/Hints/App_Code/AuthoringTool/CaseEditor/Paper/updateSimilarIDSQL.txt", strSQL);
            */

            dsQuestion.Dispose();
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }


        /// <summary>
        /// 取得某個組別下的所有Level 1的填空題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupFillOutBlankQuestion(string strGroupID)
        {
            return "SELECT * FROM FillOutBlank_Question I, QuestionMode M WHERE  M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID";
        }

        /// <summary>
        /// 取得某個組別下的所有Level 1的問答題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupTextQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionAnswer_Question I, QuestionMode M WHERE  M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID" ;
        }

        /// <summary>
        /// 取得某些特徵條件下的所有的填空題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getFeatureFillOutBlankQuestion(DataTable dtFeatureTextQuestionQID)
        {
            string strSQL="";
            if (dtFeatureTextQuestionQID.Rows.Count > 0)
            {
                strSQL += " SELECT * FROM FillOutBlank_Question AS A INNER JOIN QuestionMode AS B ON A.cQID=B.cQID";
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
        /// 取得某些特徵條件下的所有的問答題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getFeatureTextQuestion(DataTable dtFeatureTextQuestionQID)
        {
            string strSQL = "";
            if (dtFeatureTextQuestionQID.Rows.Count > 0)
            {
                strSQL += " SELECT * FROM QuestionAnswer_Question AS A INNER JOIN QuestionMode AS B ON A.cQID=B.cQID";
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
        /// 取得simulation下的所有Level 1的問答題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSimulationQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE  M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID  AND (M.cQuestionType = '3')";
        }

        /// <summary>
        /// 取得群組中的問題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupQuestionAnswer(string strGroupID)
        {
            return "SELECT * FROM dbo.QuestionAnswer_Question INNER JOIN " +
            " dbo.QuestionMode ON dbo.QuestionAnswer_Question.cQID = dbo.QuestionMode.cQID " +
            " WHERE (dbo.QuestionMode.cQuestionGroupID = '" + strGroupID + "') AND  dbo.QuestionMode.cQuestionType = '2'";
        }

        /// <summary>
        /// 取得群組中的問題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupConversation(string strGroupID)
        {
            return "SELECT * FROM dbo.Conversation_Question INNER JOIN " +
            " dbo.QuestionMode ON dbo.Conversation_Question.cQID = dbo.QuestionMode.cQID " +
            " WHERE (dbo.QuestionMode.cQuestionGroupID = '" + strGroupID + "') AND  dbo.QuestionMode.cQuestionType = '4'";
        }

        /// <summary>
        /// 取得群組中特定Problem Type的對話題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupConversationByProblemType(string strGroupID, string strCurrentProType)
        {
            return "SELECT * FROM Conversation_Question AS C, QuestionLevel AS L, QuestionMode AS M" +
            " WHERE C.cQID = L.cQID AND L.cQID = M.cQID AND M.cQuestionGroupID='" + strGroupID + "' AND L.cQuestionSymptoms = '" + strCurrentProType + "'";
        }

        /// <summary>
        /// 取得群組中的圖形題 Question_simulator
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupQuestionSimulator(string strGroupID)
        {
            //return "SELECT * FROM dbo.Question_simulator INNER JOIN " +
            //" dbo.QuestionMode ON dbo.Question_simulator.cQID = dbo.QuestionMode.cQID " +
            //" WHERE (dbo.QuestionMode.cQuestionGroupID = '" + strGroupID + "') AND  dbo.QuestionMode.cQuestionType = '3'";
            return "SELECT * FROM " +
            " QuestionMode " +
            " WHERE (cQuestionGroupID = '" + strGroupID + "') AND cQuestionType = '3'";


        }
        /// <summary>
        /// 取得某問題下的所有選項資料
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getAllSsimulatorAns(string strQID)
        {
            return "SELECT * FROM QuestionSelectionindex WHERE cQID = '" + strQID + "' ORDER BY sSeq";
        }

        /// <summary>
        /// 取得某個組別下的所有Level 1的選擇題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSelectionQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE I.cQID NOT IN (Select cQID From Paper_TextQuestion ) and M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID AND I.sLevel = '1' AND (M.cQuestionType = '1')";
        }

        /// <summary>
        /// 取得某個組別下的所有Level 1的選擇題包含關鍵字
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSelectionWithKeyWordsQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE I.cQID NOT IN (Select cQID From Paper_TextQuestion ) and M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID AND I.sLevel = '1' AND (M.cQuestionType = '6')";
        }

        /// <summary>
        /// 依照特徵值條件取得某個組別下的所有Level 1的選擇題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getFeatureSelectionQuestion(DataTable dtFeatureSelectResult)
        {
            string strSQL = "";
            //若條件結果有搜尋到才搜尋，否則不下達SQL指令
            if (dtFeatureSelectResult.Rows.Count > 0)
            {
                strSQL = " SELECT * FROM QuestionIndex AS I INNER JOIN QuestionMode AS M ON I.cQID = M.cQID";
                strSQL += " WHERE";
                strSQL += " ( I.cQID NOT IN (SELECT cQID FROM Paper_TextQuestion)) AND (I.sLevel = '1') AND (M.cQuestionType = '1') AND ";
                strSQL += " (";
                for (int i = 0; i < dtFeatureSelectResult.Rows.Count; i++)
                {
                    if (i != 0)
                        strSQL += " OR";
                    strSQL += " M.cQID ='" + dtFeatureSelectResult.Rows[i]["cQID"].ToString() + "'";
                }
                strSQL += " )";
            }
            return strSQL;
        }

        /// <summary>
        /// 依照特徵值條件取得某個組別下的所有Level 1的選擇題包含關鍵字
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getFeatureSelectionWithKeyWordsQuestion(DataTable dtFeatureSelectResult)
        {
            string strSQL = "";
            //若條件結果有搜尋到才搜尋，否則不下達SQL指令
            if (dtFeatureSelectResult.Rows.Count > 0)
            {
                strSQL = " SELECT * FROM QuestionIndex AS I INNER JOIN QuestionMode AS M ON I.cQID = M.cQID";
                strSQL += " WHERE";
                strSQL += " ( I.cQID NOT IN (SELECT cQID FROM Paper_TextQuestion)) AND (I.sLevel = '1') AND (M.cQuestionType = '6') AND ";
                strSQL += " (";
                for (int i = 0; i < dtFeatureSelectResult.Rows.Count; i++)
                {
                    if (i != 0)
                        strSQL += " OR";
                    strSQL += " M.cQID ='" + dtFeatureSelectResult.Rows[i]["cQID"].ToString() + "'";
                }
                strSQL += " )";
            }
            return strSQL;
        }

        /// <summary>
        /// 取得某個組別下的所有的情境題
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSituationQuestion(string strGroupID)
        {
            return " SELECT * FROM QuestionIndex AS A INNER JOIN QuestionMode AS B ON A.cQID = B.cQID WHERE (B.cQuestionGroupID = '"+strGroupID+"') AND B.cQuestionType='"+5+"'";
        }

        /// <summary>
        /// 取得某個情境題的問題與詳細資料
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getSituationQuestionData(string strQID)
        {
            return " SELECT * FROM Question_Situational B WHERE cQID='" + strQID + "'";
        }


        /// <summary>
        /// 取得某問卷下的所有Specific問題
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getSpecificSelectionQuestion(string strPaperID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE I.cQID = M.cQID AND M.cQuestionMode = 'Specific' AND I.sLevel = '1' AND cPaperID = '" + strPaperID + "' ";
        }
        /*
                public string getDivisionName(string strDivisionID)
                {
                    //取得某Division的名稱
                    string strReturn = "";
                    string strSQL = "SELECT * FROM Division WHERE cDivisionID = '"+strDivisionID+"' "; 
                    DataSet dsGroupName = sqldb.getDataSet(strSQL);
                    if(dsGroupName.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            strReturn = dsGroupName.Tables[0].Rows[0]["cDivisionName"].ToString();
                        }
                        catch
                        {
                        }
                    }
                    dsGroupName.Dispose();
                    return strReturn;
                }
        */
        /// <summary>
        /// 依照某個問題組別的GroupID取得該Group的名稱
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getQuestionGroupName(string strGroupID)
        {
            string strReturn = "";
            //string strSQL = "SELECT * FROM QuestionMode WHERE cQuestionGroupID = '"+strGroupID+"' "; 
            string strSQL = "SELECT * FROM QuestionGroupTree WHERE cNodeID = '" + strGroupID + "' ";
            DataSet dsGroupName = sqldb.getDataSet(strSQL);
            if (dsGroupName.Tables[0].Rows.Count > 0)
            {
                try
                {
                    //strReturn = dsGroupName.Tables[0].Rows[0]["cQuestionGroupName"].ToString();
                    strReturn = dsGroupName.Tables[0].Rows[0]["cNodeName"].ToString();
                }
                catch
                {
                }
            }
            dsGroupName.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 取得Paper_Content下某問卷的所有問題資料
        /// </summary>
        /// <returns></returns>
        public DataTable getPaperContent(string strPaperID)
        {
            DataTable dt = new DataTable();

            string strSQL = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' ORDER BY sSeq ";
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            ds.Dispose();

            return dt;
        }

        /// <summary>
        /// 取得某個問卷下的選擇題資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSelectionContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQuestionType='1' ORDER BY sSeq ";
        }

        /// <summary>
        /// 取得某個問卷下的選擇題包含關鍵字資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSelectionWithKeyWordsContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQuestionType='6' ORDER BY sSeq ";
        }


        /// <summary>
        /// 取得某個問卷下的程式題資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperProgramQuestionQuestionContent(string strPaperID)
        {
            //return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C , QuestionAnswer_Question T , QuestionAnswer_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
            return "SELECT C.sSeq , T.cQID , T.cQuestion, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C ,Program_Question T , QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND   C.cQID = M.cQID ORDER BY sSeq ";
        }


        /// <summary>
        /// 取得某個問卷下的AITypeQuestion資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperAITypeQuestionContent(string strPaperID)
        {
            //return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C , QuestionAnswer_Question T , QuestionAnswer_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
            return "SELECT C.sSeq , T.cQID , T.cQuestion, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C ,QuestionIndex T , QuestionMode M ,AITypeQuestionCorrectAnswer AITypeQuestion WHERE C.cPaperID ='" + strPaperID + "' AND  AITypeQuestion.cQID = C.cQID AND   AITypeQuestion.cQID = M.cQID  AND AITypeQuestion.cQID = T.cQID ORDER BY sSeq ";
            
            
        }


        /// <summary>
        /// 取得某個問卷下的填空題資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperFillOutBlankContent(string strPaperID)
        {
            return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C ,FillOutBlank_Question T , FillOutBlank_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
        }


        /// <summary>
        /// 取得某個問卷下的問答題資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperTextContent(string strPaperID)
        {
            return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C , QuestionAnswer_Question T , QuestionAnswer_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
        }

        /// <summary>
        /// 取得某個問卷下的問答題資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSituationContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content A,QuestionMode B,Question_Situational C,QuestionIndex D WHERE A.cQID=B.cQID AND B.cQID=C.cQID AND C.cQID=D.cQID AND A.cPaperID='"+strPaperID+"' ORDER BY sSeq";
        }

        /// <summary>
        /// 取得某個問卷下的圖形題資料(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSimulationContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQuestionType='3' ORDER BY sSeq ";
        }

        /// <summary>
        /// 從Paper_CaseDivisionSection取得某個問卷的ID
        /// </summary>
        /// <param name="strCaseID"></param>
        /// <param name="strClinicNum"></param>
        /// <param name="strSectionName"></param>
        /// <returns></returns>
        public string getPaperIDFromCase(string strCaseID, string strClinicNum, string strSectionName)
        {
            string strReturn = "";

            string strSQL = "SELECT * FROM Paper_CaseDivisionSection WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + strClinicNum + "'";
            DataSet dsPaperID = sqldb.getDataSet(strSQL);
            if (dsPaperID.Tables[0].Rows.Count > 0)
            {
                try
                {
                    strReturn = dsPaperID.Tables[0].Rows[0]["cPaperID"].ToString();
                }
                catch
                {
                }
            }
            dsPaperID.Dispose();

            return strReturn;
        }
        /*
                public string getPaper_AuthoringSeq(string strEditMode , string strFunction , int intSeq)
                {
                    return "SELECT * FROM Paper_AuthoringSeq WHERE cEditMode = '"+strEditMode+"' AND cFunction = '"+strFunction+"' AND sSeq = '"+intSeq.ToString()+"' ";
                }
        */
        /// <summary>
        /// 取得某問題下的所有選項資料
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getAllSelections(string strQID)
        {
            return "SELECT * FROM QuestionSelectionindex WHERE cQID = '" + strQID + "' ORDER BY sSeq";
        }

        /// <summary>
        /// 取得某問題的資料(From QuestionIndex)
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getQuestion(string strQID)
        {
            return "SELECT * FROM QuestionIndex WHERE cQID = '" + strQID + "' ";
        }

        /// <summary>
        /// 取得圖形題的資料(From Question_Simulator)
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getQuestion_sim(string strQID)
        {
            return "SELECT * FROM Question_Simulator WHERE cQID = '" + strQID + "' ";
        }


        /// <summary>
        /// 取得某問題符合Symptom的資料(From QuestionIndex)
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestionSymptom"></param>
        /// <returns></returns>
        public string getQuestionBySymptoms(string strQID, string strQuestionSymptom)
        {
            return "SELECT * FROM dbo.QuestionIndex INNER JOIN dbo.QuestionLevel ON dbo.QuestionIndex.cQID = dbo.QuestionLevel.cQID " +
                "WHERE dbo.QuestionIndex.cQID = '" + strQID + "' AND cQuestionSymptoms = '" + strQuestionSymptom + "'";
        }

        /// <summary>
        /// 取得某問題的問題與選項的資料
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getQuestionAndSelection(string strQID)
        {
            return "SELECT * FROM QuestionIndex Q , QuestionSelectionIndex S , QuestionMode M " +
                "WHERE Q.cQID = '" + strQID + "' AND Q.cQID = M.cQID AND S.cQID =M.cQID " +
                "AND Q.cQID = S.cQID ORDER BY sSeq ";
        }

        public string getSingleTempLog_PaperSelectionAnswer(string strPaperID, string strStartTime, string strUserID, string strQID, string strSelectionID)
        {
            return "SELECT * FROM TempLog_PaperSelectionAnswer A WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "'";
        }

        /// <summary>
        /// 取得某個問題的詳細資料(From QuestionIndex , QuestionMode)
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getSingleQuestionInformation(string strQID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE  I.cQID = '" + strQID + "' AND I.cQID = M.cQID ";
        }

        /// <summary>
        /// 把資料存入TempLog_PaperTextQuestion
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strAnswer"></param>
        public void SaveToTempLog_PaperTextQuestion(string strPaperID, string strStartTime, string strUserID, string strQID, string strQuestion, string strAnswer)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM TempLog_PaperTextQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE TempLog_PaperTextQuestion SET cQuestion = @cQuestion , cAnswer = '" + strAnswer + "' " +
                    "WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO TempLog_PaperTextQuestion (cPaperID , cStartTime , cUserID , cQID , cQuestion , cAnswer) " +
                    "VALUES ('" + strPaperID + "' , '" + strStartTime + "' , '" + strUserID + "' , '" + strQID + "' , @cQuestion , '" + strAnswer + "') ";
            }
            dsCheck.Dispose();

            try
            {
                object[] pList = { strQuestion };
                sqldb.ExecuteNonQuery(strSQL, pList);
            }
            catch
            {
            }
        }

        //將資料存入Paper_AssignedQuestion
        /*
        public void SaveToPaper_AssignedQuestion(string strPaperID , string strUserID , string strQID , string strQuestion , string strQuestionType , string strQuestionMode)
        {
            string strSQL = "";
            //檢查此計付是否已存在資料庫
            bool bExist = false;
            strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '"+strPaperID+"' AND cUserID = '"+strUserID+"' AND cQID = '"+strQID+"' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if(dsCheck.Tables[0].Rows.Count > 0)
            {
                bExist = true;
            }
            dsCheck.Dispose();

            if(bExist == true)
            {
                //Update
                strSQL ="UPDATE Paper_AssignedQuestion SET cQuestion = @cQuestion , cQuestionType = '"+strQuestionType+"' , cQuestionMode = '"+strQuestionType+"' "+
                        "WHERE cPaperID = '"+strPaperID+"' AND cUserID = '"+strUserID+"' AND cQID = '"+strQID+"' ";
            }
            else
            {
                //Insert
                strSQL =" INSERT Paper_AssignedQuestion (cPaperID , cUserID , cQID , cQuestion , cQuestionType , cQuestionMode) "+
                        " VALUES ('"+strPaperID+"' , '"+strUserID+"' ,'"+strQID+"' ,@cQuestion ,'"+strQuestionType+"' ,'"+strQuestionMode+"') ";
            }
            try
            {
                object[] pList = {strQuestion};
                sqldb.ExecuteNonQuery(strSQL,pList);
            }
            catch
            {
            }
        }
        */
        /// <summary>
        /// 儲存一筆資料到Paper_AssignedQuestion
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strQuestionType"></param>
        /// <param name="strQuestionMode"></param>
        public void SaveToPaper_AssignedQuestion(string strPaperID, string strStartTime, string strUserID, string strQID, string strSeq, string strQuestionType, string strQuestionMode)
        {
            string strSQL = "";
            bool bExist = false;
            strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND sSeq = '" + strSeq + "' ";
            DataSet dsCheck = sqldb.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                bExist = true;
            }
            dsCheck.Dispose();

            if (bExist == true)
            {
                //Update
                strSQL = "UPDATE Paper_AssignedQuestion SET cQuestionType = '" + strQuestionType + "' , cQuestionMode = '" + strQuestionType + "' " +
                    "WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND sSeq = '" + strSeq + "' ";
            }
            else
            {
                //Insert
                strSQL = " INSERT Paper_AssignedQuestion (cPaperID , cStartTime , cUserID , cQID , sSeq , cQuestionType , cQuestionMode) " +
                    " VALUES ('" + strPaperID + "' , '" + strStartTime + "' , '" + strUserID + "' ,'" + strQID + "' , '" + strSeq + "' , '" + strQuestionType + "' ,'" + strQuestionMode + "') ";
            }
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 將資料存入TempLog_PaperHeader
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strOperationTime"></param>
        /// <param name="strFinishTime"></param>
        public void SaveToTempLog_PaperHeader(string strPaperID, string strStartTime, string strUserID, string strOperationTime, string strFinishTime)
        {
            string strSQL = "";
            //檢查此筆資料是否已存在TempLogForHeader
            bool bExist = false;
            strSQL = "SELECT * FROM TempLog_PaperHeader WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            DataSet dsTempLogHeader = sqldb.getDataSet(strSQL);
            if (dsTempLogHeader.Tables[0].Rows.Count > 0)
            {
                bExist = true;
            }
            dsTempLogHeader.Dispose();

            if (bExist == true)
            {
                //Update
                strSQL = "UPDATE TempLog_PaperHeader SET cFinishTime = '" + strFinishTime + "' , cOperationTime = '" + strOperationTime + "' " +
                    "WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            }
            else
            {
                //insert
                strSQL = "INSERT INTO TempLog_PaperHeader (cPaperID , cStartTime , cUserID , cOperationTime , cFinishTime) " +
                    "VALUES ('" + strPaperID + "' , '" + strStartTime + "' , '" + strUserID + "' , '" + strOperationTime + "' , '" + strFinishTime + "')";
            }

            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 把資料存入TempLog_PaperSelectionQuestion
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strQuestionLevel"></param>
        /// <param name="strQuestion"></param>
        public void SaveToTempLog_PaperSelectionQuestion(string strPaperID, string strStartTime, string strUserID, string strQID, string strQuestionLevel, string strQuestion)
        {
            string strSQL = "";
            bool bQuestionExist = false;
            strSQL = "SELECT * FROM TempLog_PaperSelectionQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            DataSet dsQuestion = sqldb.getDataSet(strSQL);
            if (dsQuestion.Tables[0].Rows.Count > 0)
            {
                bQuestionExist = true;
            }
            dsQuestion.Dispose();
            if (bQuestionExist == true)
            {
                //Update
                strSQL = "UPDATE TempLog_PaperSelectionQuestion SET sLevel = '" + strQuestionLevel + "' , cQuestion = @cQuestion " +
                    "WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO TempLog_PaperSelectionQuestion (cPaperID , cStartTime , cUserID , cQID , sLevel , cQuestion) " +
                    "VALUES('" + strPaperID + "' , '" + strStartTime + "' , '" + strUserID + "' , '" + strQID + "' , '" + strQuestionLevel + "' , @cQuestion) ";
            }
            try
            {
                object[] pList = { strQuestion };
                sqldb.ExecuteNonQuery(strSQL, pList);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 把資料存入TempLog_PaperSelectionAnswer
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        /// <param name="strSeq"></param>
        /// <param name="strSelection"></param>
        /// <param name="strCaseSelect"></param>
        public void SaveToTempLog_PaperSelectionAnswer(string strPaperID, string strStartTime, string strUserID, string strQID, string strSelectionID, string strSeq, string strSelection, string strCaseSelect)
        {
            string strSQL = "";
            strSQL = this.getSingleTempLog_PaperSelectionAnswer(strPaperID, strStartTime, strUserID, strQID, strSelectionID);
            DataSet dsSelectionAnswer = sqldb.getDataSet(strSQL);
            if (dsSelectionAnswer.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE TempLog_PaperSelectionAnswer SET  sSeq = '" + strSeq + "' , cSelection = '" + strSelection + "' , bCaseSelect = '" + strCaseSelect + "'" +
                    "WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "'";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO TempLog_PaperSelectionAnswer (cPaperID , cStartTime , cUserID , cQID , cSelectionID, sSeq , cSelection , bCaseSelect) " +
                    "VALUES('" + strPaperID + "' , '" + strStartTime + "' , '" + strUserID + "' , '" + strQID + "' , '" + strSelectionID + "' , '" + strSeq + "' , '" + strSelection + "' , '" + strCaseSelect + "' )";
            }
            dsSelectionAnswer.Dispose();
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 從TempLog_PaperSelectionAnswer刪除某個問題下的所有選項紀錄
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        public void DeleteTempLog_PaperSelectionAnswerByQID(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            string strSQL = "DELETE FROM TempLog_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 從TempLog_PaperSelectionAnswer刪除某個問題下某個選項的紀錄
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        public void DeleteTempLog_PaperSelectionAnswerBySelectionID(string strPaperID, string strStartTime, string strUserID, string strQID, string strSelectionID)
        {
            string strSQL = "DELETE FROM TempLog_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 從Paper_AssignedQuestion刪除某個使用者操作某問卷的資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        public void DeletePaper_AssignQuestion(string strPaperID, string strStartTime, string strUserID)
        {
            string strSQL = "DELETE FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 取得Paper_Header的資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperHeader(string strPaperID)
        {
            return "SELECT * FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";
        }

        public string getStatisticFunctionList()
        {
            //取得統計Function的列表
            return "SELECT * FROM Paper_StatisticFunctions";
        }

        public string getDivisionName(string strDivisionID)
        {
            //取得某Division的名稱
            string strReturn = "";
            string strSQL = "SELECT * FROM Division WHERE cDivisionID = '" + strDivisionID + "' ";
            DataSet dsGroupName = sqldb.getDataSet(strSQL);
            if (dsGroupName.Tables[0].Rows.Count > 0)
            {
                try
                {
                    strReturn = dsGroupName.Tables[0].Rows[0]["cDivisionName"].ToString();
                }
                catch
                {
                }
            }
            dsGroupName.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 判斷情境題是否已存在資料庫
        /// </summary>
        /// <returns></returns>
        public DataSet checkSituationQuestionExist(string strQID)
        {
            DataSet dsSituationQuestion;               
            string strCheckSituation = " SELECT * FROM Question_Situational A,QuestionIndex B WHERE (A.cQID = B.cQID) AND (B.cQID = '"+strQID+"')";
            dsSituationQuestion = sqldb.getDataSet(strCheckSituation);
            return dsSituationQuestion;

        }

        /// <summary>
        /// 判斷情境題的小精靈資訊是否已存在資料庫
        /// </summary>
        /// <returns></returns>
        public DataSet checkTADataExist(string strQID)
        {
            DataSet dsSituationQuestion;
            string strCheckSituation = " SELECT * FROM VRTeachingAssistant TA WHERE (TA.strQuestionID = '" + strQID + "')";
            dsSituationQuestion = sqldb.getDataSet(strCheckSituation);
            return dsSituationQuestion;

        }

        /// <summary>
        /// 判斷情境題是否已存在
        /// </summary>
        /// <returns></returns>
        public Boolean boolSituationQuestionExist(string strQID)
        {
            Boolean isExist = false;
            DataSet dsSituationQuestion;
            string strCheckSituation = " SELECT * FROM Question_Situational  WHERE cQID = '" + strQID + "'";
            dsSituationQuestion = sqldb.getDataSet(strCheckSituation);
            if (dsSituationQuestion.Tables[0].Rows.Count > 0)
                isExist = true;
            return isExist;

        }

        /// <summary>
        /// 更新StiuationQuestion的資料
        /// </summary>
        /// <returns></returns>
        public void saveStiuationQuestion(string Question, string discription, string qid, string cQuestionGroup, string cQuestionType, string strSimulationQuestionType, string strSenceID) 
        {
            string strSaveSQL_Question_Situational;
            string strSaveSQL_QuestionMode;
            string strSaveSQL_QuestionIndex;
            
            if (checkSituationQuestionExist(qid).Tables[0].Rows.Count > 0)
            {
                strSaveSQL_Question_Situational = "UPDATE Question_Situational SET strInformation='" + discription + "', cSceneID='" + strSenceID + "' , QuestionType='" + strSimulationQuestionType + "'  WHERE cQID='" + qid + "'";
                strSaveSQL_QuestionMode = "UPDATE QuestionMode SET cQuestionGroupID='" + cQuestionGroup + "' WHERE cQID='" + qid + "'";
                strSaveSQL_QuestionIndex = "UPDATE QuestionIndex SET cQuestion='" + Question + "' WHERE cQID='" + qid + "'";
            }
            else 
            {
                strSaveSQL_Question_Situational = "INSERT Question_Situational (cQID , strInformation , cSceneID , QuestionType) VALUES ('" + qid + "','" + discription + "','" + strSenceID + "','" + strSimulationQuestionType + "')";
                strSaveSQL_QuestionMode = "INSERT QuestionMode (cQID , cQuestionGroupID , cQuestionMode , cQuestionType) VALUES ('" + qid + "','" + cQuestionGroup + "' ,'" + "General" + "' , '" + cQuestionType + "')";
                strSaveSQL_QuestionIndex = "INSERT QuestionIndex (cQID , cQuestion , sLevel ) VALUES ('" + qid + "','" + Question + "' ,'" + 1 + "')";
            }
            
            try
            {
                sqldb.ExecuteNonQuery(strSaveSQL_Question_Situational);
                sqldb.ExecuteNonQuery(strSaveSQL_QuestionIndex);
                sqldb.ExecuteNonQuery(strSaveSQL_QuestionMode);
                
                 
            }
            catch
            {

            }           
 
        }

        //傳入cQuestionID刪除全部有關的註記
        public void SearchAnnotationIDBycQuestionID(string cQuestionID)
        { 
            DataSet dsAnnotationIDs;
            string strSQLForAnnotationID = " SELECT strAnnotationID FROM ItemForVMAnnotations  WHERE cQuestionID = '" + cQuestionID + "'";
            dsAnnotationIDs = sqldb.getDataSet(strSQLForAnnotationID);
            if (dsAnnotationIDs.Tables[0].Rows.Count > 0)
            {
                int iItemCount=dsAnnotationIDs.Tables[0].Rows.Count;
                for (int i = 0; i < iItemCount; i++)
                {
                    string strAnnotationID=dsAnnotationIDs.Tables[0].Rows[i][0].ToString();
                    DeleteVMAnnotations(strAnnotationID);
                    //朱君2013/7/2 遞迴執行刪除註記
                    SearchAnnotationIDBycQuestionID(strAnnotationID);
                }
            } 
        }

        //朱君 傳入AnnotationID刪除ItemForAnnotations資料表中的指定註記
        public void DeleteVMAnnotations(string strAnnotationID)
        {
            string strSQLAnnotationData = "DELETE FROM ItemForVMAnnotations WHERE strAnnotationID = '" + strAnnotationID + "' ";
            string strSQLAnnotationLink = "DELETE FROM ItemForLinksVRAnnotations WHERE cAnnotationID = '" + strAnnotationID + "' ";           
            try
            {
                sqldb.ExecuteNonQuery(strSQLAnnotationData);
                sqldb.ExecuteNonQuery(strSQLAnnotationLink);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 根據QID取得QuestionGroupID
        /// </summary>
        /// <param name="strQID"></param>
        public string GetQuestionGroupIDByQID(string strQID)
        {
            string strSQL = " SELECT cQuestionGroupID FROM QuestionMode WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            return dsCheck.Tables[0].Rows[0]["cQuestionGroupID"].ToString();
        }
    }
}
