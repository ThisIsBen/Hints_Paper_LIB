using System;
using suro.util;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PaperSystem
{
    /// <summary>
    /// SQLString ���K�n�y�z�C
    /// </summary>
    public class SQLString
    {
        //�إ�SqlDB����
        SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

        public SQLString()
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
        }

        public string getCaseFolder(string strCaseID)
        {
            return "SELECT T.cURL , T.cDivisionID , U.cURL AS 'Server' FROM TeachingCase T , DivIDtoURL U WHERE T.cCaseID = '" + strCaseID + "' AND T.cDivisionID = U.cDivisionID";
        }

        /// <summary>
        /// �R���@�Ӱݵ��D
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

            //�R��QuestionLevel
            strSQL = "DELETE QuestionLevel WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

        }

        /// <summary>
        /// �R���@�Ӱݵ��D
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

            //�R��QuestionLevel
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

            //�R��QuestionLevel
            strSQL = "DELETE QuestionLevel WHERE cQID = '" + strQID + "' ";
            myDB.ExecuteNonQuery(strSQL);

        }

        /// <summary>
        /// �x�s�@����Ʀ�QuestionMode
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
            //���oQuestionGroupName
            string strQuestionGroupName = DataReceiver.getQuestionGroupNameByQuestionGroupID(strQuestionGroupID);


            //if teacher save the edited question as a new question
            //add similarID to the new question to show all the related similar question when the teacher picks one of the parent or offspring of this new question.
            if (templateQuestionQID != null && strUserID != null)
            {
                //to contain similarID to assign to the new question
                string similarID = "";
                string TemplateQuesQuestionGroupID = "";
                string TemplateQuesQuestionGroupName = "";
                //Ben check whether the the value of ��similarID��of the question that is used as a template is null or not.
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
                    // �����ѡA�]���NQuestionMode��ƪ�QID��쪺Primary Key�����A�ҥH�Y���榹Update�|�ɭP�����Ƶ���ơC(�\��W�O���F�i�H�N�@�D����D��J���P�����D�D�D���C)  �Ѹ� 2015/09/06
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


            //store a record to table QuestionMode(If it's not �t�s�s�D)
            else
            {
                
                strSQL = " SELECT * FROM QuestionMode" +
                                " WHERE cQID = '" + strQID + "' ";

                SqlDB myDB = new SqlDB(System.Configuration.ConfigurationManager.AppSettings["connstr"]);
                DataSet dsCheck = myDB.getDataSet(strSQL);


                //Do nothing if there is an existing question with the same cQID in QuestionMode table.
                if (dsCheck.Tables[0].Rows.Count > 0)
                {
                    // �����ѡA�]���NQuestionMode��ƪ�QID��쪺Primary Key�����A�ҥH�Y���榹Update�|�ɭP�����Ƶ���ơC(�\��W�O���F�i�H�N�@�D����D��J���P�����D�D�D���C)  �Ѹ� 2015/09/06
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
        /// �x�s�@����Ʀ�Paper_NextStepBySelectionID
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
        /// �ק�Y�ݨ���Title���
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
        /// �ק��g�z�ѳ̤j�r��
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
        /// �ק�@���D�ت�����
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
        /// �o��@���D�ت�����
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
        /// �o��@�i�Ҩ����D��
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
        /// �x�s�@����Ʀ�Paper_ItemTitle
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
            //��X���ݨ��Ҧ��|���Q�s��Paper_Content��Specific���D
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

                        //���o�D��
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
                //�S���|���Q�s��Paper_Content��Specific���D
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
        /// �Ǧ^�Y�ݨ��bPaper_RandomQuestionNum�����
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
        /// �Ǧ^�Y�ݨ��bPaper_RandomQuestionNum�����
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
        /// ���oPaper_Content INNER JOIN QuestionMode�U�Y�ݨ����Ҧ����D���
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
        /// �Ǧ^�Y�D�ػP�Y���DJoin���ԲӸ��
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
        /// �Ǧ^�Y���D�ؤU���Ҧ��ﶵ�M��
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
        /// �Ǧ^�Y�ӨϥΪ̾ާ@�Y�ݨ���QID����
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
        /// �Ǧ^�Y�ӨϥΪ̾ާ@�Y�ݨ���Selection����
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
        /// �Ǧ^�Y�ӨϥΪ̾ާ@�Y�ݨ���QID����
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
        /// �Ǧ^�Y�ӨϥΪ̾ާ@�Y�ݨ���QID����
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
        /// �ˬd�Y�ݨ����Y���D���S���s����DTitle
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
        /// �Ǧ^TempLog_PaperPhyAct���@������
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
        /// �g�J�@����Ʀ�TempLog_PaperPhyAct
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
        /// �Ǧ^Paper_AssignedQuestionBySeq�Y�ϥΪ̬Y���ާ@�������Y�@�D����QID
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
        /// �Ǧ^Paper_AssignedQuestionBySeq�Y�ϥΪ̬Y���ާ@�������Y�@�D����DataRow
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
        /// �ˬd�Y�@���D�جO�_�s�b��Paper_AssignedQuestion
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
        /// �Ǧ^�Y�ӨϥΪ̦APaper_AssignedQuestion���Y�@���ݨ����Ҧ����
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
        /// ���Ʀs�JPaper_CaseDivision
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
        /// ���Ʀs�JPaper_CaseDivisionSection
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
        /// �Ǧ^��Ʈw���Ҧ���Division���
        /// </summary>
        /// <returns></returns>
        public string getEveryDivision()
        {
            return "SELECT * FROM Division ORDER BY cDivisionID";
        }

        /// <summary>
        /// �ˬd�ϥΪ̦A�Y���ާ@�������O���O���ާ@�L�Y�ݨ�(check TempLog , Summary)
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
        /// �ˬdTempLog_PaperHeader�A�ϥΪ̬O���O�w�g���L���ݨ��C
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
        /// �ˬdSummary_PaperHeader�A�ϥΪ̬O���O�w�g���L���ݨ��C
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
            //�x�s��Ʀ�Paper_Header
            string strSQL = "";
            //�P�_�������O�_�s�b��Ʈw
            strSQL = "SELECT * FROM Paper_Header WHERE cPaperID = '" + strPaperID + "'";
            int intRowCount = 0;
            DataSet dsPaper = sqldb.getDataSet(strSQL);
            intRowCount = dsPaper.Tables[0].Rows.Count;
            dsPaper.Dispose();

            if (intRowCount > 0)
            {
                //�ק���Ʈw
                strSQL = "UPDATE Paper_Header SET cPaperName = '" + strPaperName + "' , cTitle = '" + strTitle + "' , cObjective = '" + strObjective + "' , cEditMethod = '" + strEditMethod + "' ,  cGenerationMethod = '" + strGenerationMethod + "' , cPresentMethod = '" + strPresentMethod + "' , sTestDuration = '" + strTestDuration + "' , cPresentType = '" + strPresentType + "' " +
                    "WHERE cPaperID = '" + strPaperID + "' ";
            }
            else
            {
                //�s�W���Ʈw
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
        /// �R���Y�ݨ����Y�Ӷüƿ�������D�էO�����(DELETE FROM Paper_RandomQuestionNum)
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
        /// �Ǧ^�Y�ݨ��APaper_RandomQuestionNum���Ҧ����
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaper_RandomQuestionNum(string strPaperID)
        {
            return "SELECT * FROM Paper_RandomQuestionNum WHERE cPaperID = '" + strPaperID + "'";
        }

        /// <summary>
        /// �Ǧ^�Y�ݨ����Y�Ӱ��D�էO�APaper_RandomQuestionNum���Ҧ����
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
        /// ���Ʀs�JPaper_RandomQuestionNum
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
        /// ���Ʀs�JPaper_RandomQuestionNum
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

        //		--���o�C���F����Ǫ��������~���C�ӤH�Ĥ@���񦹰ݨ�������
        //		SELECT P.cPaperID , Min(cStartTime) AS cStartTime , P.cUserID FROM Summary_PaperHeader P , HintsUser H 
        //		WHERE P.cPaperID = 'swakevin20050907120001' 
        //		AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'
        //		GROUP BY P.cPaperid , P.cUserID 

        /// <summary>
        /// �Ǧ^�Y�ӲէO�Ĥ@���ާ@�Юת�Summary����
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'   " +
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
        /// �Ǧ^�Y�ӯZ�ŲĤ@���ާ@�Юת�Summary����
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " AND S.cCaseID = T.cCaseID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) A " +
                " LEFT JOIN Summary_Header B " +
                " ON A.cCaseID = B.cCaseID AND A.cUserID = B.cUserID AND A.cStartTime = B.cStartTime " +
                " ORDER BY A.cUserID ";
        }

        /// <summary>
        /// �Ǧ^�Y�ӱЮשҦ��ǥͲĤ@���ާ@��Summary_Header����
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                " AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                " AND S.cCaseID = T.cCaseID " +
                " GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName " +
                " ) A " +
                " LEFT JOIN Summary_Header B " +
                " ON A.cCaseID = B.cCaseID AND A.cUserID = B.cUserID AND A.cStartTime = B.cStartTime " +
                " ORDER BY A.cUserID ";
        }

        /// <summary>
        /// By Class �Ǧ^�ǥ;ާ@���ݨ�������D�H��
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            // AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            //	 AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            //	 AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
            //ON S.cCaseID = T.cCaseID AND T.cFullName = '�L������v'
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '�L������v'
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '�L������v'
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE'  " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
            //By Group �Ǧ^�ǥ;ާ@���ݨ����ݵ��D����
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
            //By Class �Ǧ^�ǥ;ާ@���ݨ����ݵ��D����
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' AND H.cClass = '" + strClass + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
            //By Author �Ǧ^�ǥ;ާ@���ݨ����ݵ��D����
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , T.cProvider , P.cFullName  FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
            //	AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	) P
            //	INNER JOIN 
            //	Summary_Header S
            //	ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //	INNER JOIN 
            //	TeachingCase T 
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '�L��M��v' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //	AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
            //	AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime  
            //	AND P.cUserID = G.cUserID AND H.cUserID = G.cUserID AND S.cUserID = G.cUserID 
            //	GROUP BY P.cPaperid , S.cCaseID , P.cUserID , H.cFullName 
            //	) P
            //	INNER JOIN 
            //	Summary_Header S
            //	ON P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime 
            //	INNER JOIN 
            //	TeachingCase T 
            //	ON S.cCaseID = T.cCaseID AND T.cFullName = '�L��M��v' 
            //) H 
            //INNER JOIN Summary_PaperTextQuestion S 
            //ON H.cPaperID = S.cPaperID AND H.cUserID = S.cUserID AND H.cStartTime = S.cStartTime AND S.cQID = '07'
        }

        public string getTextSummaryByCase(string strPaperID, string strQID, string strCaseID)
        {
            //By Case �Ǧ^�ǥ;ާ@���ݨ����ݵ��D����
            return " SELECT * FROM " +
                " ( " +
                " SELECT P.cPaperID , P.cStartTime , P.cuserID , T.cCaseID , T.cCaseName  , P.cFullName FROM " +
                " ( " +
                " SELECT P.cPaperID , Min(P.cStartTime) AS cStartTime , P.cUserID  , H.cFullName " +
                " FROM Summary_PaperHeader P , HintsUser H , Summary_Header S " +
                " WHERE P.cPaperID = '" + strPaperID + "' " +
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
                " AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
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
            //AND P.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' 
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
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭��ǯZ�žް��L�ݨ�
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
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭��ǯZ�žް��L�ݨ�
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getGroupPaperList(string strPaperID)
        {
            return "SELECT DISTINCT G.cGroup " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , UserGroup G , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime " +
                "AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭��ǯZ�žް��L�ݨ�
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
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭��ǯZ�žް��L�ݨ�
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
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "AND P.cUserID = G.cUserID AND S.cUserID = G.cUserID " +
                "ORDER BY G.cGroup";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭��ǯZ�žް��L�ݨ�
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getClassPaperList(string strPaperID)
        {
            return "SELECT DISTINCT H.cClass " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "ORDER BY H.cClass ";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭��ǯZ�žް��L�ݨ�
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strAuthorID"></param>
        /// <returns></returns>
        public string getAuthorClassPaperList(string strPaperID, string strAuthorID)
        {
            return "SELECT DISTINCT H.cClass " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND T.cFullName = '" + strAuthorID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "ORDER BY H.cClass ";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭��ǯZ�žް��L�ݨ�
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <returns></returns>
        public string getCaseClassPaperList(string strPaperID, string strCaseID)
        {
            return "SELECT DISTINCT H.cClass " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND T.cCaseID = '" + strCaseID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "ORDER BY H.cClass ";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭���Author�ҽs�誺Case���Q�ް��L�ݨ�
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getAuthorPaperList(string strPaperID)
        {
            return "SELECT DISTINCT T.cFullName " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "ORDER BY T.cFullName ";

            //			return  "SELECT DISTINCT T.cProvider , T.cFullName "+
            //					"FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H "+
            //					"WHERE P.cPaperID = '"+strPaperID+"' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID "+
            //					"AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' "+
            //					"ORDER BY T.cFullName ";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭���Case���Q�ް��L�ݨ�
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getCasePaperList(string strPaperID)
        {
            return "SELECT DISTINCT T.cCaseID , T.cCaseName " +
                "FROM Summary_PaperHeader P , Summary_Header S , TeachingCase T , HintsUser H " +
                "WHERE P.cPaperID = '" + strPaperID + "' AND P.cUserID = S.cUserID AND P.cStartTime = S.cStartTime AND S.cCaseID = T.cCaseID " +
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "ORDER BY cCaseName ";
        }

        /// <summary>
        /// �Ǧ^�ϥΪ̾ާ@�������PSummary_Header��Inner Join�A���U�إ߭���Case���Q�ް��L�ݨ�
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
                "AND P.cUserID = H.cUserID AND S.cUserID = H.cUserID AND H.cClass <> 'MIRAC' AND H.cClass <> '�q�q��' AND H.cClass <> 'NckuEE' " +
                "ORDER BY cCaseName ";
        }

        /// <summary>
        /// �N��Ʀs�JPaper_GroupingQuestion
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
        /// �Ǧ^�Y�Ӱݨ������Y�Ӱ��D�էO�A�|���Q��JPaper_Content���D��
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
        /// �x�s��Ʀ�Paper_CaseDivisionSection
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
        /// �R���@�Ӱ��D�A�R������즳(QuestionIndex , QuestionSelectionIndex , QuestionMode, QuestionLevel)
        /// </summary>
        /// <param name="strQID"></param>���D���s��
        public void DeleteGeneralQuestion(string strQID)
        {
            //�R��QuestionMode
            string strSQL = "DELETE QuestionMode WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //�R��QuestionSelectionIndex
            strSQL = "DELETE QuestionSelectionIndex WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //�R��QuestionIndex
            strSQL = "DELETE QuestionIndex WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //�R��QuestionLevel
            strSQL = "DELETE QuestionLevel WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            //�R��QuestionLevel
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
        /// �R���@�ӱ��Ұ��D�A�R������ƪ�(QuestionIndex , QuestionMode , Question_Situational)
        /// </summary>
        /// <param name="strQID"></param>���D���s��
        public void DeleteSituationQuestion(string strQID)
        {
            //�R��QuestionMode
            string strSQL = "DELETE QuestionMode WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //�R��QuestionIndex
            strSQL = "DELETE QuestionIndex WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }

            //�R��Question_Situational
            strSQL = "DELETE Question_Situational WHERE cQID = '" + strQID + "' ";
            try
            {
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch
            {
            }
            //�R��QuestionLevel
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
        /// ��@����ƱqQuestion_Simulator�R��
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
        /// ��@����ƱqPaper_Content�R��
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
        /// <param name="strQuestionType">���D����:1.����D2.�ݵ��D</param>
        /// <param name="strQuestionMode"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strSeq"></param>
        public void SaveToQuestionContent(string strPaperID, string strQID, string strStandardScore, string strQuestionMode, string strModifyType)
        {
            if (strModifyType == "Paper")
            {
                //��X�����D�b�ݨ��ت��D��
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
        /// <param name="strQuestionType">���D����:1.����D2.�ݵ��D</param>
        /// <param name="strQuestionMode"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strSeq"></param>
        public void SaveToQuestionContent(string strPaperID , string strQID , string strStandardScore , string strQuestionType , string strQuestionMode , string strQuestion , string strSeq)
        {
            //�N�Ȧs�JPaper_QuestionContent
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
        /// �N�Ȧs�JPaper_QuestionContent
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
        /// ���o�Y�ӲէO�U���Ҧ�Level 1������D
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupFillOutBlankQuestion(string strGroupID)
        {
            return "SELECT * FROM FillOutBlank_Question I, QuestionMode M WHERE  M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID";
        }

        /// <summary>
        /// ���o�Y�ӲէO�U���Ҧ�Level 1���ݵ��D
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupTextQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionAnswer_Question I, QuestionMode M WHERE  M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID" ;
        }

        /// <summary>
        /// ���o�Y�ǯS�x����U���Ҧ�������D
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
        /// ���o�Y�ǯS�x����U���Ҧ����ݵ��D
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
        /// ���osimulation�U���Ҧ�Level 1���ݵ��D
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSimulationQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE  M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID  AND (M.cQuestionType = '3')";
        }

        /// <summary>
        /// ���o�s�դ������D
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
        /// ���o�s�դ������D
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
        /// ���o�s�դ��S�wProblem Type������D
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupConversationByProblemType(string strGroupID, string strCurrentProType)
        {
            return "SELECT * FROM Conversation_Question AS C, QuestionLevel AS L, QuestionMode AS M" +
            " WHERE C.cQID = L.cQID AND L.cQID = M.cQID AND M.cQuestionGroupID='" + strGroupID + "' AND L.cQuestionSymptoms = '" + strCurrentProType + "'";
        }

        /// <summary>
        /// ���o�s�դ����ϧ��D Question_simulator
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
        /// ���o�Y���D�U���Ҧ��ﶵ���
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getAllSsimulatorAns(string strQID)
        {
            return "SELECT * FROM QuestionSelectionindex WHERE cQID = '" + strQID + "' ORDER BY sSeq";
        }

        /// <summary>
        /// ���o�Y�ӲէO�U���Ҧ�Level 1������D
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSelectionQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE I.cQID NOT IN (Select cQID From Paper_TextQuestion ) and M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID AND I.sLevel = '1' AND (M.cQuestionType = '1')";
        }

        /// <summary>
        /// ���o�Y�ӲէO�U���Ҧ�Level 1������D�]�t����r
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSelectionWithKeyWordsQuestion(string strGroupID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE I.cQID NOT IN (Select cQID From Paper_TextQuestion ) and M.cQuestionGroupID = '" + strGroupID + "' AND I.cQID = M.cQID AND I.sLevel = '1' AND (M.cQuestionType = '6')";
        }

        /// <summary>
        /// �̷ӯS�x�ȱ�����o�Y�ӲէO�U���Ҧ�Level 1������D
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getFeatureSelectionQuestion(DataTable dtFeatureSelectResult)
        {
            string strSQL = "";
            //�Y���󵲪G���j�M��~�j�M�A�_�h���U�FSQL���O
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
        /// �̷ӯS�x�ȱ�����o�Y�ӲէO�U���Ҧ�Level 1������D�]�t����r
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getFeatureSelectionWithKeyWordsQuestion(DataTable dtFeatureSelectResult)
        {
            string strSQL = "";
            //�Y���󵲪G���j�M��~�j�M�A�_�h���U�FSQL���O
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
        /// ���o�Y�ӲէO�U���Ҧ��������D
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getGroupSituationQuestion(string strGroupID)
        {
            return " SELECT * FROM QuestionIndex AS A INNER JOIN QuestionMode AS B ON A.cQID = B.cQID WHERE (B.cQuestionGroupID = '"+strGroupID+"') AND B.cQuestionType='"+5+"'";
        }

        /// <summary>
        /// ���o�Y�ӱ����D�����D�P�ԲӸ��
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public string getSituationQuestionData(string strQID)
        {
            return " SELECT * FROM Question_Situational B WHERE cQID='" + strQID + "'";
        }


        /// <summary>
        /// ���o�Y�ݨ��U���Ҧ�Specific���D
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
                    //���o�YDivision���W��
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
        /// �̷ӬY�Ӱ��D�էO��GroupID���o��Group���W��
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
        /// ���oPaper_Content�U�Y�ݨ����Ҧ����D���
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
        /// ���o�Y�Ӱݨ��U������D���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSelectionContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQuestionType='1' ORDER BY sSeq ";
        }

        /// <summary>
        /// ���o�Y�Ӱݨ��U������D�]�t����r���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSelectionWithKeyWordsContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQuestionType='6' ORDER BY sSeq ";
        }


        /// <summary>
        /// ���o�Y�Ӱݨ��U���{���D���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperProgramQuestionQuestionContent(string strPaperID)
        {
            //return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C , QuestionAnswer_Question T , QuestionAnswer_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
            return "SELECT C.sSeq , T.cQID , T.cQuestion, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C ,Program_Question T , QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND   C.cQID = M.cQID ORDER BY sSeq ";
        }


        /// <summary>
        /// ���o�Y�Ӱݨ��U��AITypeQuestion���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperAITypeQuestionContent(string strPaperID)
        {
            //return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C , QuestionAnswer_Question T , QuestionAnswer_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
            return "SELECT C.sSeq , T.cQID , T.cQuestion, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C ,QuestionIndex T , QuestionMode M ,AITypeQuestionCorrectAnswer AITypeQuestion WHERE C.cPaperID ='" + strPaperID + "' AND  AITypeQuestion.cQID = C.cQID AND   AITypeQuestion.cQID = M.cQID  AND AITypeQuestion.cQID = T.cQID ORDER BY sSeq ";
            
            
        }


        /// <summary>
        /// ���o�Y�Ӱݨ��U������D���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperFillOutBlankContent(string strPaperID)
        {
            return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C ,FillOutBlank_Question T , FillOutBlank_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
        }


        /// <summary>
        /// ���o�Y�Ӱݨ��U���ݵ��D���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperTextContent(string strPaperID)
        {
            return "SELECT C.sSeq , T.cQID , T.cQuestion,A.cAnswer, M.cDivisionID , M.cQuestionGroupID , M.cQuestionGroupName , M.cQuestionMode , M.cQuestionType FROM Paper_Content C , QuestionAnswer_Question T , QuestionAnswer_Answer  A,QuestionMode M WHERE C.cPaperID = '" + strPaperID + "' AND C.cQID = T.cQID AND  C.cQID = A.cQID AND C.cQID = M.cQID ORDER BY sSeq ";
        }

        /// <summary>
        /// ���o�Y�Ӱݨ��U���ݵ��D���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSituationContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content A,QuestionMode B,Question_Situational C,QuestionIndex D WHERE A.cQID=B.cQID AND B.cQID=C.cQID AND C.cQID=D.cQID AND A.cPaperID='"+strPaperID+"' ORDER BY sSeq";
        }

        /// <summary>
        /// ���o�Y�Ӱݨ��U���ϧ��D���(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperSimulationContent(string strPaperID)
        {
            return "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQuestionType='3' ORDER BY sSeq ";
        }

        /// <summary>
        /// �qPaper_CaseDivisionSection���o�Y�Ӱݨ���ID
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
        /// ���o�Y���D�U���Ҧ��ﶵ���
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getAllSelections(string strQID)
        {
            return "SELECT * FROM QuestionSelectionindex WHERE cQID = '" + strQID + "' ORDER BY sSeq";
        }

        /// <summary>
        /// ���o�Y���D�����(From QuestionIndex)
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getQuestion(string strQID)
        {
            return "SELECT * FROM QuestionIndex WHERE cQID = '" + strQID + "' ";
        }

        /// <summary>
        /// ���o�ϧ��D�����(From Question_Simulator)
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getQuestion_sim(string strQID)
        {
            return "SELECT * FROM Question_Simulator WHERE cQID = '" + strQID + "' ";
        }


        /// <summary>
        /// ���o�Y���D�ŦXSymptom�����(From QuestionIndex)
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
        /// ���o�Y���D�����D�P�ﶵ�����
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
        /// ���o�Y�Ӱ��D���ԲӸ��(From QuestionIndex , QuestionMode)
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getSingleQuestionInformation(string strQID)
        {
            return "SELECT * FROM QuestionIndex I , QuestionMode M WHERE  I.cQID = '" + strQID + "' AND I.cQID = M.cQID ";
        }

        /// <summary>
        /// ���Ʀs�JTempLog_PaperTextQuestion
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

        //�N��Ʀs�JPaper_AssignedQuestion
        /*
        public void SaveToPaper_AssignedQuestion(string strPaperID , string strUserID , string strQID , string strQuestion , string strQuestionType , string strQuestionMode)
        {
            string strSQL = "";
            //�ˬd���p�I�O�_�w�s�b��Ʈw
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
        /// �x�s�@����ƨ�Paper_AssignedQuestion
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
        /// �N��Ʀs�JTempLog_PaperHeader
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strOperationTime"></param>
        /// <param name="strFinishTime"></param>
        public void SaveToTempLog_PaperHeader(string strPaperID, string strStartTime, string strUserID, string strOperationTime, string strFinishTime)
        {
            string strSQL = "";
            //�ˬd������ƬO�_�w�s�bTempLogForHeader
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
        /// ���Ʀs�JTempLog_PaperSelectionQuestion
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
        /// ���Ʀs�JTempLog_PaperSelectionAnswer
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
        /// �qTempLog_PaperSelectionAnswer�R���Y�Ӱ��D�U���Ҧ��ﶵ����
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
        /// �qTempLog_PaperSelectionAnswer�R���Y�Ӱ��D�U�Y�ӿﶵ������
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
        /// �qPaper_AssignedQuestion�R���Y�ӨϥΪ̾ާ@�Y�ݨ������
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
        /// ���oPaper_Header�����
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public string getPaperHeader(string strPaperID)
        {
            return "SELECT * FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";
        }

        public string getStatisticFunctionList()
        {
            //���o�έpFunction���C��
            return "SELECT * FROM Paper_StatisticFunctions";
        }

        public string getDivisionName(string strDivisionID)
        {
            //���o�YDivision���W��
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
        /// �P�_�����D�O�_�w�s�b��Ʈw
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
        /// �P�_�����D���p���F��T�O�_�w�s�b��Ʈw
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
        /// �P�_�����D�O�_�w�s�b
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
        /// ��sStiuationQuestion�����
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

        //�ǤJcQuestionID�R���������������O
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
                    //���g2013/7/2 ���j����R�����O
                    SearchAnnotationIDBycQuestionID(strAnnotationID);
                }
            } 
        }

        //���g �ǤJAnnotationID�R��ItemForAnnotations��ƪ������w���O
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
        /// �ھ�QID���oQuestionGroupID
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
