using System;
using System.Data;
using suro.util;

namespace PaperSystem
{
    /// <summary>
    /// DataReceiver 的摘要描述。
    /// </summary>
    public class DataReceiver
    {
        //建立SqlDB物件
        SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
        SQLString mySQL = new SQLString();

        public DataReceiver()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        /// <summary>
        /// 傳回瀏覽器的IP
        /// </summary>
        /// <returns></returns>
        public static string getClientIPAddress()
        {
            string c = System.Net.Dns.GetHostName();
            System.Net.IPAddress[] d = System.Net.Dns.Resolve(c).AddressList;
            string ee = d[0].ToString();
            return ee;
            /*
            string a = Page.Request.UserHostName;
            string b = Page.Request.UserHostAddress;
            string c = System.Net.Dns.GetHostName();
            System.Net.IPAddress[] d = System.Net.Dns.Resolve(c).AddressList;
            string ee = d[0].ToString();
            string f = System.Environment.MachineName;
            string g = System.Environment.UserDomainName;
            string h = System.Environment.UserName;
            string i = Request.Url.ToString();
            string[] j = i.Split('/');
            string k = j[2].ToString();
            */
        }

        /// <summary>
        /// 傳回網頁的DomainName
        /// </summary>
        /// <param name="WebPage"></param>
        /// <returns></returns>
        public static string getDomainNameBySplitingURL(System.Web.UI.Page WebPage)
        {
            string i = WebPage.Request.Url.ToString();
            string[] j = i.Split('/');
            string k = j[2].ToString();
            return k;
        }

        /// <summary>
        /// 傳回某個問答題的題目內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getTextQuestionContentByQID(string strQID)
        {
            string strReturn = "";

            string strSQL = "SELECT A.cQuestion,B.cAnswer FROM QuestionAnswer_Question A,QuestionAnswer_Answer B WHERE A.cQID=B.cQID AND A.cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cQuestion"].ToString() + "$" + ds.Tables[0].Rows[0]["cAnswer"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 根據問題ID取得問題內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string QuestionAnswer_Question_SELECT_Question(string strQID)
        {
            string strReturn = "";

            string strSQL = "SELECT cQuestion FROM QuestionAnswer_Question WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cQuestion"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 根據問題ID與答案ID取得答案內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <returns></returns>
        public static string QuestionAnswer_Answer_SELECT_Answer(string strQID, string strAID)
        {
            string strReturn = "";

            string strSQL = "SELECT cAnswer FROM QuestionAnswer_Answer WHERE cQID = '" + strQID + "' AND cAID = '" + strAID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cAnswer"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 取得指定問題ID的所有答案
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static DataTable QuestionAnswer_Answer_SELECT_AllAnswer(string strQID)
        {
            DataTable dt = new DataTable();
            string strSQL = "SELECT * FROM QuestionAnswer_Answer WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            ds.Dispose();

            return dt;
        }

        /// <summary>
        /// 取得指定問題ID的所有答案
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public DataTable QuestionAnswer_Answer_SELECT_AllAnswers(string strQID)
        {
            DataTable dt = new DataTable();
            string strSQL = "SELECT * FROM QuestionAnswer_Answer WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            ds.Dispose();

            return dt;
        }

        /// <summary>
        /// 取得指定問題ID 且 答案開頭與分類項目一樣
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strSimpleAnswer"></param>
        /// <returns></returns>
        public static DataTable QuestionAnswer_Answer_SELECT_SimpleAnswer(string strQID, string strSimpleAnswer)
        {
            DataTable dt = new DataTable();
            string strSQL = "SELECT * FROM QuestionAnswer_Answer WHERE cQID = '" + strQID + "' AND cAnswer LIKE '" + strSimpleAnswer + "%' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            ds.Dispose();

            return dt;
        }


        /// <summary>
        /// 傳回某個QuestionGroupName by QuestionGroupID
        /// </summary>
        /// <returns></returns>
        public static string getQuestionGroupNameByQuestionGroupID(string strQuestionGroupID)
        {
            string strReturn = "";

            string strSQL = "SELECT cNodeName FROM QuestionGroupTree WHERE cNodeID = '" + strQuestionGroupID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cNodeName"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回某個QuestionGroup Serial by QuestionGroupID
        /// </summary>
        /// <param name="strQuestionGroupID"></param>
        /// <returns></returns>
        public static int getQuestionGroupSerialNumByQuestionGroupID(string strQuestionGroupID)
        {
            int iSerialNum = 0;

            string strSQL = "SELECT iSerialNum FROM QuestionGroupTree WHERE cNodeID = '" + strQuestionGroupID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                iSerialNum = Convert.ToInt32(ds.Tables[0].Rows[0]["iSerialNum"].ToString());
            }
            ds.Dispose();

            return iSerialNum;
        }


        /// <summary>
        /// 傳回某個Section在UserLevelPresent裡面的SectionWorkType
        /// </summary>
        /// <returns></returns>
        public static string getSectionWorkTypeFromUserLevelPresentBySectionName(string strCaseID, int intClinicNum, string strSectionName)
        {
            string strReturn = "";

            string strSQL = "SELECT sWorkType FROM UserLevelPresent WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + intClinicNum.ToString() + "' AND cSectionName = '" + strSectionName + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["sWorkType"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回某個Section所屬的DivisionID
        /// </summary>
        /// <returns></returns>
        public static string getDivisionIDFromUserLevelPresentBySectionName(string strCaseID, int intClinicNum, string strSectionName)
        {
            string strReturn = "";

            string strSQL = "SELECT cDivisionID FROM UserLevelPresent WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + intClinicNum.ToString() + "' AND cSectionName = '" + strSectionName + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cDivisionID"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回某問卷的bModify
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static bool getModifyableFromPaper_Header(string strPaperID)
        {
            bool bReturn = true;

            string strSQL = "SELECT bModify FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = Convert.ToBoolean(ds.Tables[0].Rows[0]["bModify"]);
            }
            ds.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 傳回某問卷的bMarkable
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static bool getMarkableFromPaper_Header(string strPaperID)
        {
            bool bReturn = true;

            string strSQL = "SELECT bMarkable FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = Convert.ToBoolean(ds.Tables[0].Rows[0]["bMarkable"]);
            }
            ds.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 傳回某問卷的bSwitch
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static bool getSwitchableFromPaper_Header(string strPaperID)
        {
            bool bReturn = true;

            string strSQL = "SELECT bSwitch FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = Convert.ToBoolean(ds.Tables[0].Rows[0]["bSwitch"]);
            }
            ds.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 傳回某問卷的bIsSituationExam
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static bool getIsSituationModeFromPaper_Header(string strPaperID)
        {
            bool bReturn = true;

            string strSQL = "SELECT bIsSituationExam FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = Convert.ToBoolean(ds.Tables[0].Rows[0]["bIsSituationExam"]);
            }
            ds.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 傳回某問卷的bIsSituationExam
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static bool getIsStudentNameVisibleFromPaper_Header(string strPaperID)
        {
            bool bReturn = true;

            string strSQL = "SELECT bIsStudentNameVisible FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = Convert.ToBoolean(ds.Tables[0].Rows[0]["bIsStudentNameVisible"]);
            }
            ds.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 傳回某問卷的bSectionSummaryShowCorrectAnswer
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static bool getCorrectAnswerableFromPaper_Header(string strPaperID)
        {
            bool bReturn = true;

            string strSQL = "SELECT bSectionSummaryShowCorrectAnswer FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["bSectionSummaryShowCorrectAnswer"].ToString() == "")
                {
                    bReturn = false;
                }
                else
                {
                    bReturn = Convert.ToBoolean(ds.Tables[0].Rows[0]["bSectionSummaryShowCorrectAnswer"]);
                }
            }
            ds.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 取得某個選項的Response資料
        /// </summary>
        /// <param name="strSelectionID"></param>
        /// <returns></returns>
        public static string getResponseBySelectionID(string strSelectionID)
        {
            string strReturn = "";

            string strSQL = "SELECT cResponse FROM QuestionSelectionIndex WHERE cSelectionID = '" + strSelectionID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cResponse"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 從Question_Retry取得某個問題可以Retry的次數
        /// </summary>
        /// <param name="strUserLevel"></param>
        /// <param name="strCaseID"></param>
        /// <param name="strDivisionID"></param>
        /// <param name="intClinicNum"></param>
        /// <param name="strSectionName"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static int getRetryNumFromQuestion_Retry(string strUserLevel, string strCaseID, string strDivisionID, int intClinicNum, string strSectionName, string strQID)
        {
            int intReturn = 1;

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = "SELECT cValue2 FROM Question_Retry WHERE cUserLevel = '" + strUserLevel + "' AND cCaseID = '" + strCaseID + "' AND cDivisionID = '" + strDivisionID + "' AND CurrentTerm = '" + intClinicNum.ToString() + "' AND cSectionName = '" + strSectionName + "' AND cQID = '" + strQID + "' ";
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["cValue2"] != DBNull.Value)
                {
                    intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["cValue2"]);
                }
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 輸入記號的編號，傳回該記號圖片的Src路徑
        /// </summary>
        /// <param name="intSymbol"></param>
        /// <returns></returns>
        public static string getMarkSymbolSrcByMethod(int intSymbol)
        {
            string strReturn = "";

            switch (intSymbol)
            {
                case 0:
                    strReturn = "";
                    break;
                case 1:
                    strReturn = "../../BasicForm/Image/circle.gif";
                    break;
                case 2:
                    strReturn = "../../BasicForm/Image/square.gif";
                    break;
                case 3:
                    strReturn = "../../BasicForm/Image/triangle.gif";
                    break;
                default:
                    strReturn = "";
                    break;
            }

            return strReturn;
        }

        /// <summary>
        /// 取得某個在Paper_AssignedQuestion題目的記號圖片URL
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getMarkSymbolUrlBySeq(string strPaperID, string strStartTime, string strUserID, int intSeq)
        {
            string strReturn = "";

            int intSymbol = getMarkSymbolFromPaper_AssignedQuestionBySeq(strPaperID, strStartTime, strUserID, intSeq);

            getMarkSymbolSrcByMethod(intSymbol);

            return strReturn;
        }

        /// <summary>
        /// 取得某個在Paper_AssignedQuestion題目的記號圖片URL
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getMarkSymbolUrlByQID(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            string strReturn = "";

            int intSymbol = getMarkSymbolFromPaper_AssignedQuestionByQID(strPaperID, strStartTime, strUserID, strQID);
            switch (intSymbol)
            {
                case 0:
                    strReturn = "";
                    break;
                case 1:
                    strReturn = "../../BasicForm/Image/circle.gif";
                    break;
                case 2:
                    strReturn = "../../BasicForm/Image/square.gif";
                    break;
                case 3:
                    strReturn = "../../BasicForm/Image/triangle.gif";
                    break;
                default:
                    strReturn = "";
                    break;
            }

            return strReturn;
        }

        /// <summary>
        /// 修改一筆資料到Paper_AssignedQuestion的sMarkSymbol欄位
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strQuestionType"></param>
        /// <param name="strQuestionMode"></param>
        public static void UpdateMarkSymbolToPaper_AssignedQuestionBySeq(string strPaperID, string strStartTime, string strUserID, int intSeq, int intMarkSymbol)
        {
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

            string strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND sSeq = '" + intSeq.ToString() + "' ";
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Paper_AssignedQuestion SET sMarkSymbol = '" + intMarkSymbol.ToString() + "' " +
                    "WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND sSeq = '" + intSeq.ToString() + "' ";

                myDB.ExecuteNonQuery(strSQL);
            }
            dsCheck.Dispose();
        }

        /// <summary>
        /// 修改一筆資料到Paper_AssignedQuestion的sMarkSymbol欄位
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strQuestionType"></param>
        /// <param name="strQuestionMode"></param>
        public static void UpdateMarkSymbolToPaper_AssignedQuestionByQID(string strPaperID, string strStartTime, string strUserID, string strQID, int intMarkSymbol)
        {
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

            string strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE Paper_AssignedQuestion SET sMarkSymbol = '" + intMarkSymbol.ToString() + "' " +
                    "WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";

                myDB.ExecuteNonQuery(strSQL);
            }
            dsCheck.Dispose();
        }

        /// <summary>
        /// 傳回某個在Paper_AssignedQuestion題目的記號種類
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static int getMarkSymbolFromPaper_AssignedQuestionBySeq(string strPaperID, string strStartTime, string strUserID, int intSeq)
        {
            int intReturn = 0;

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = "SELECT sMarkSymbol FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND sSeq = '" + intSeq.ToString() + "' ";
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["sMarkSymbol"] != DBNull.Value)
                {
                    intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sMarkSymbol"]);
                }
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某個在Paper_AssignedQuestion題目的記號種類
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static int getMarkSymbolFromPaper_AssignedQuestionByQID(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            int intReturn = 0;

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = "SELECT sMarkSymbol FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["sMarkSymbol"] != DBNull.Value)
                {
                    intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sMarkSymbol"]);
                }
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回一個存在於Paper_AssignedQuestion的某題號的題目資料
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSeq"></param>
        /// <returns></returns>
        public static DataRow[] checkPaper_AssignedQuestionByQID(string strPaperID, string strStartTime, string strUserID, int intSeq)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM Paper_AssignedQuestion A , QuestionMode M WHERE A.cQID = M.cQID AND A.cPaperID = '" + strPaperID + "' AND A.cStartTime = '" + strStartTime + "' AND A.cUserID = '" + strUserID + "' AND A.sSeq = '" + intSeq.ToString() + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            DataRow[] drReturn = dsCheck.Tables[0].Select();
            dsCheck.Dispose();

            return drReturn;
        }

        /// <summary>
        /// 檢查某一個PaperID , StartTime , UserID是否存在於Paper_AssignedQuestion
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSeq"></param>
        /// <returns></returns>
        public static bool checkUserDataFromPaper_AssignedQuestion(string strPaperID, string strStartTime, string strUserID)
        {
            bool bReturn = false;

            string strSQL = "";
            strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
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
        /// 檢查某一個QID是否存在於Paper_AssignedQuestion
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSeq"></param>
        /// <returns></returns>
        public static bool checkPaper_AssignedQuestionByQID(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            bool bReturn = false;

            string strSQL = "";
            strSQL = "SELECT * FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
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
        /// 傳回某個使用者再Paper_AssignedQuestion中某一份問卷的最大題號(sSeq)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public static int getUserQuestionInPaper_AssignedQuestion(string strPaperID, string strStartTime, string strUserID)
        {
            int intReturn = 0;
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = "SELECT sSeq FROM Paper_AssignedQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ORDER BY sSeq DESC";
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sSeq"]);
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷的sAssignMethod
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public static int getAssignMethodFromPaper_Header(string strPaperID)
        {
            int intReturn = 1;

            string strSQL = "SELECT sAssignMethod FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sAssignMethod"]);
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷的作答時間
        /// </summary>
        /// <returns></returns>
        public static int getTestDurationFromPaper_Header(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = "SELECT sTestDuration FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sTestDuration"]);
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某使用者操作某問卷已經做答了多少題目的數量
        /// </summary>
        /// <returns></returns>
        public static int getUserFinishQuestionCountFromTempLog_PaperSelectionAnswer(string strPaperID, string strStartTime, string strUserID)
        {
            int intReturn = 0;

            string strSQL = "SELECT DISTINCT cQID FROM TempLog_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intReturn = ds.Tables[0].Rows.Count;
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某選項所屬的QID
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getQIDBySelectionID(string strSelectionID)
        {
            string strReturn = "";

            string strSQL = "SELECT cQID FROM QuestionSelectionIndex WHERE cSelectionID = '" + strSelectionID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cQID"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 檢查某個題目下的某選項有沒有編輯Rationale
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static bool checkSelectHasRationaleBySelectionID(string strQID, string strSelectionID)
        {
            bool bReturn = false;

            string strSQL = "SELECT cResponse FROM QuestionSelectionIndex WHERE cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["cResponse"].ToString().Trim().Length > 0)
                {
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }
            }
            ds.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 根據問題ID與選項ID取得選項的順序
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        /// <returns></returns>
        public static int QuestionSelectionIndex_SELECT_Seq(string strQID, string strSelectionID)
        {
            int iSeq = 0;
            string strSQL = "SELECT * FROM QuestionSelectionIndex WHERE cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataTable dt = myDB.getDataSet(strSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                iSeq = Convert.ToInt32(dt.Rows[0]["sSeq"].ToString());
            }
            return iSeq;
            dt.Dispose();
        }

        /// <summary>
        /// 傳回某問題的題目
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getQuestionContentByQID(string strQID)
        {
            string strReturn = "";

            string strSQL = "SELECT cQuestion FROM QuestionIndex WHERE cQID = '" + strQID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cQuestion"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回某圖形題的題目
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getQuestionContentByQID_sim(string strQID)
        {
            string strReturn = "";

            string strSQL = "SELECT cContent FROM Question_Simulator WHERE cQID = '" + strQID + "' ";

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cContent"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回此問題的建議選項數目
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static int getSuggestedSelectionCountByQID(string strQID)
        {
            int intReturn = 0;

            string strSQL = "SELECT bCaseSelect FROM QuestionSelectionIndex WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    bool bSuggested = Convert.ToBoolean(ds.Tables[0].Rows[i]["bCaseSelect"]);

                    if (bSuggested == true)
                    {
                        intReturn += 1;
                    }
                }
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回使用者操作某問卷某題目所選擇的選項ID
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getUserSelectionLogFromTempLogForPaperSelectionAnswer(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            string strReturn = "";

            string strSQL = "SELECT cSelectionID FROM TempLog_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ORDER BY sSeq";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cSelectionID"].ToString();
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回某問卷的某問題某選項下一步的SectionName
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getNextSectionNameFromNextStepBySelectionID(string strPaperID, string strQID, string strSelectionID)
        {
            string strReturn = "";

            string strSQL = "SELECT cNextSection FROM Paper_NextStepBySelectionID WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = Convert.ToString(ds.Tables[0].Rows[0]["cNextSection"]);
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 傳回某問卷的某問題某選項下一步的QuestionSeq
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static int getNextQuestionSeqFromNextStepBySelectionID(string strPaperID, string strQID, string strSelectionID)
        {
            int intReturn = 999;

            string strSQL = "SELECT sNextquestionSeq FROM Paper_NextStepBySelectionID WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sNextquestionSeq"]);
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷的某問題下一步的方法
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static int getNextMethodFromPaperContent(string strPaperID, string strQID, string strSelectionID)
        {
            int intReturn = 99;

            string strSQL = "SELECT sNextMethod FROM Paper_NextStepBySelectionID WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sNextMethod"]);
            }
            ds.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 檢查PE某部位有沒有編輯問卷。
        /// </summary>
        /// <param name="strCaseID"></param>
        /// <param name="intClinicNum"></param>
        /// <param name="strSourceSec"></param>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public static bool checkExistPEQuestion(string strCaseID, int intClinicNum, string strSourceSec, string strItem)
        {
            bool bReturn = false;

            string strSQL = "SELECT cPaperID FROM Paper_PEQuestion WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + intClinicNum.ToString() + "' AND cSectionName = '" + strSourceSec + "' AND cItem = '" + strItem + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsPE = myDB.getDataSet(strSQL);
            if (dsPE.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            dsPE.Dispose();
            return bReturn;
        }

        /// <summary>
        /// 傳回PE某部位的PaperID
        /// </summary>
        /// <param name="strCaseID"></param>
        /// <param name="intClinicNum"></param>
        /// <param name="strSourceSec"></param>
        /// <param name="strItem"></param>
        /// <returns></returns>
        public static string getPEQuestionPaperID(string strCaseID, int intClinicNum, string strSourceSec, string strItem)
        {
            string strReturn = "";

            string strSQL = "SELECT cPaperID FROM Paper_PEQuestion WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + intClinicNum.ToString() + "' AND cSectionName = '" + strSourceSec + "' AND cItem = '" + strItem + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsPE = myDB.getDataSet(strSQL);
            if (dsPE.Tables[0].Rows.Count > 0)
            {
                strReturn = dsPE.Tables[0].Rows[0]["cPaperID"].ToString();
            }
            dsPE.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 傳回定義在Paper_Content的下一個要呈現的Section名稱
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static string getNextSectionFromPaperContent(string strPaperID, string strQID)
        {
            string strReturn = "";

            string strSQL = "SELECT cNextSection FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["cNextSection"] != DBNull.Value)
                {
                    strReturn = ds.Tables[0].Rows[0]["cNextSection"].ToString();
                }
            }
            ds.Dispose();

            return strReturn;
        }

        /// <summary>
        /// 檢查使用者有沒有答對某個題目(TempLog_PaperSelectionAnswer)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static bool checkUserLogIsCorrectFromTempLogByQID(string strPaperID, string strUserID, string strStartTime, string strQID)
        {
            bool bReturn = false;

            //此問題的建議選項數目
            int intQuestionSuggestedCount = getSuggestedSelectionCountByQID(strQID);

            //使用者操作此問題答對的建議選項數目
            int intUserSuggestedCount = 0;

            string strSQL = "SELECT * FROM TempLog_PaperSelectionAnswer WHERE cPaperID = '" + strPaperID + "' AND cUserID = '" + strUserID + "' AND cStartTime = '" + strStartTime + "' AND cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    bool bIsCorrect = Convert.ToBoolean(ds.Tables[0].Rows[i]["bCaseSelect"]);

                    if (bIsCorrect == true)
                    {
                        //使用者選到建議選項
                        intUserSuggestedCount += 1;
                    }
                    else
                    {
                        //使用者選到非建議選項
                        if (intUserSuggestedCount > 0)
                        {
                            intUserSuggestedCount = intUserSuggestedCount - 1;
                        }
                    }
                }
            }
            ds.Dispose();

            if (intQuestionSuggestedCount == intUserSuggestedCount)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }

        /// <summary>
        /// 檢查某問卷的使用者操作紀錄是否存在TempLog True:TempLog 
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strStartTime"></param>
        /// <returns></returns>
        public static bool checkStartTimeInTempLog(string strPaperID, string strUserID, string strStartTime)
        {
            bool bReturn = false;
            string strSQL = "SELECT cFinishTime FROM TempLog_PaperHeader WHERE cPaperID = '" + strPaperID + "' AND cUserID = '" + strUserID + "' AND cStartTime = '" + strStartTime + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }
            ds.Dispose();
            return bReturn;
        }

        /// <summary>
        /// 檢查某問卷的使用者操作紀錄是否存在Summary True:Summary
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strStartTime"></param>
        /// <returns></returns>
        public static bool checkStartTimeInSummary(string strPaperID, string strUserID, string strStartTime)
        {
            bool bReturn = false;
            string strSQL = "SELECT cFinishTime FROM Summary_PaperHeader WHERE cPaperID = '" + strPaperID + "' AND cUserID = '" + strUserID + "' AND cStartTime = '" + strStartTime + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }
            ds.Dispose();
            return bReturn;
        }

        /// <summary>
        /// 判斷目前時間是否大於傳入的年月日6個月，如果是:傳回0，否則傳回1。
        /// </summary>
        /// <param name="intYear"></param>
        /// <param name="intMonth"></param>
        /// <param name="intDate"></param>
        /// <returns></returns>
        private string checkNowAndRecordTime(int intYear, int intMonth, int intDate)
        {
            string strReturn = "1";

            if (DateTime.Now.Year >= intYear)
            {
                if (DateTime.Now.Month >= intMonth + 6)
                {
                    if (DateTime.Now.Day >= intDate)
                    {
                        //	Yes 回傳0
                        strReturn = "0";
                    }
                    else
                    {
                        //	No  回傳1
                        strReturn = "1";
                    }
                }
                else
                {
                    //	No  回傳1
                    strReturn = "1";
                }
            }
            else
            {
                //	No  回傳1
                strReturn = "1";
            }

            return strReturn;
        }

        /// <summary>
        /// 傳回某個問題組別下有幾個問題
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupID"></param>
        /// <returns></returns>
        public int getQuestionCountInQuestionGroup(string strPaperID, string strGroupID)
        {
            int intReturn = 0;
            string strSQL = mySQL.getPaper_RandomQuestionNumByGroup(strPaperID, strGroupID);
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intReturn = Convert.ToInt32(ds.Tables[0].Rows[0]["sQuestionNum"]);
            }
            ds.Dispose();
            return intReturn;
        }

        /// <summary>
        /// 傳回某個題目所屬的問題組別
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getQuestionGroupByQID(string strQID)
        {
            string strReturn = "";
            string strSQL = mySQL.getSingleQuestionInformation(strQID);
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cQuestionGroupID"].ToString();
            }
            ds.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 檢查使用者是不是有操作過此問卷
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public bool checkTempLog_PaperHeader(string strPaperID, string strStartTime, string strUserID)
        {
            bool bReturn = false;

            string strSQL = "";
            strSQL = "SELECT * FROM TempLog_PaperHeader WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' ";
            DataSet dsTempLogHeader = sqldb.getDataSet(strSQL);

            if (dsTempLogHeader.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }
            dsTempLogHeader.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 檢查使用者是不是有點選過此問題
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public bool checkTempLog_PaperSelectionQuestion(string strPaperID, string strStartTime, string strUserID, string strQID)
        {
            bool bReturn = false;

            string strSQL = "";
            strSQL = "SELECT * FROM TempLog_PaperSelectionQuestion WHERE cPaperID = '" + strPaperID + "' AND cStartTime = '" + strStartTime + "' AND cUserID = '" + strUserID + "' AND cQID = '" + strQID + "' ";
            DataSet dsQuestion = sqldb.getDataSet(strSQL);

            if (dsQuestion.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }
            dsQuestion.Dispose();

            return bReturn;
        }

        /// <summary>
        /// 檢查使用者是不是有點選過此選項
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strUserID"></param>
        /// <param name="strQID"></param>
        /// <param name="strSelectionID"></param>
        /// <returns></returns>
        public bool checkTempLog_PaperSelectionAnswer(string strPaperID, string strStartTime, string strUserID, string strQID, string strSelectionID)
        {
            bool bReturn = false;

            SQLString mySQL = new SQLString();

            string strSQL = "";
            strSQL = mySQL.getSingleTempLog_PaperSelectionAnswer(strPaperID, strStartTime, strUserID, strQID, strSelectionID);

            DataSet dsSelectionAnswer = sqldb.getDataSet(strSQL);
            if (dsSelectionAnswer.Tables[0].Rows.Count > 0)
            {
                bReturn = true;
            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }

        /// <summary>
        /// 傳回某個題目的題目內容
        /// </summary>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public string getSelectionQuestionContent(string strQID)
        {
            string strReturn = "";
            SQLString mySQL = new SQLString();
            string strSQL = mySQL.getSingleQuestionInformation(strQID);
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cQuestion"].ToString();
            }
            ds.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 傳入一個題目的題號，傳回某問卷的亂數選題中該題號所屬問題組別的ID
        /// </summary>
        /// <returns></returns>
        public string getQuestionGroupIDFromRandomByPaperIDAndQuestionIndex(string strPaperID, int intIndex)
        {
            string strReturn = "";
            int intQuestionSum = 0;

            SQLString mySQL = new SQLString();
            string strSQL = mySQL.getPaper_RandomQuestionNum(strPaperID);

            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int i = 0;
                do
                {
                    //累加至問題總數
                    intQuestionSum += Convert.ToInt32(ds.Tables[0].Rows[i]["sQuestionNum"]);

                    //取出此問題組別的ID
                    strReturn = ds.Tables[0].Rows[i]["cQuestionGroupID"].ToString();

                    i += 1;
                } while (intQuestionSum < intIndex);
            }
            ds.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 傳入一個題號，傳回某問卷的選擇題該題號在Paper_Content的DataRow[]
        /// </summary>
        /// <returns></returns>
        public DataRow[] getQIDFromPaper_ContentByQuestionIndex(string strPaperID, int intIndex)
        {
            DataRow[] drArray = new DataRow[1];
            if (intIndex > 0)
            {
                int intRowIndex = intIndex - 1;
                //取得Paper_Content的資料
                SQLString mySQL = new SQLString();
                DataTable dt = mySQL.getPaperContent(strPaperID);
                if (dt.Rows.Count > intRowIndex)
                {
                    drArray[0] = dt.Rows[intRowIndex];
                }
            }
            return drArray;
        }


        /// <summary>
        /// 傳入一個題號，傳回某問卷在Paper_AssignedQuestion的DataRow[]
        /// </summary>
        /// <returns></returns>
        public DataRow[] getQIDFromPaper_AssignedQuestionByQuestionIndex(string strPaperID, string strStartTime, string strUserID, int intIndex)
        {
            DataRow[] drArray = new DataRow[1];
            if (intIndex > 0)
            {
                //取得Paper_Content的資料
                SQLString mySQL = new SQLString();
                DataTable dt = mySQL.getUserQuestionInPaper_AssignedQuestion(strPaperID, strStartTime, strUserID);

                drArray = dt.Select("sSeq = '" + intIndex.ToString() + "' ", "sSeq");

            }
            return drArray;
        }


        /// <summary>
        /// 傳回某問卷所有的題目總數從Paper_Content和Paper_Random
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperAllQuestionCountFromContentAndRandom(string strPaperID)
        {
            int intReturn = 0;

            //取得此問卷的所有題目(Paper_Content)
            int intContentQuestionCount = this.getPaperContentQuestionNum(strPaperID);

            //取得此問卷的所有題目(Paper_RandomQuestionNum)
            int intRandomQuestionCount = this.getTotalQuestionNumFromRandomQuestion(strPaperID);

            intReturn = intContentQuestionCount + intRandomQuestionCount;

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷的名稱
        /// </summary>
        /// <returns></returns>
        public string getPaperName(string strPaperID)
        {
            string strReturn = "";
            string strSQL = "SELECT * FROM Paper_Header WHERE cPaperID = '" + strPaperID + "'";
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cPaperName"].ToString();
            }
            ds.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 傳回某問卷的標題文字
        /// </summary>
        /// <returns></returns>
        public string getPaperTitle(string strPaperID)
        {
            string strReturn = "";
            string strSQL = "SELECT cTitle FROM Paper_Header WHERE cPaperID = '" + strPaperID + "' ";
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cTitle"].ToString();
            }
            ds.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 傳回某題目的標題文字
        /// </summary>
        /// <returns></returns>
        public string getQuestionTitle(string strPaperID, string strItem)
        {
            string strReturn = "";
            string strSQL = "SELECT cTitle FROM Paper_ItemTitle WHERE cPaperID = '" + strPaperID + "' AND cItem = '" + strItem + "' ";
            DataSet ds = sqldb.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                strReturn = ds.Tables[0].Rows[0]["cTitle"].ToString();
            }
            ds.Dispose();
            return strReturn;
        }

        /// <summary>
        /// 傳回某問卷全部題目的數量(包含Paper_Content , Paper_Random)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperQuestionCount(string strPaperID)
        {
            int intReturn = 0;

            //計算在Paper_Content裡的題目數量
            intReturn += this.getPaperContentQuestionNum(strPaperID);

            //計算在Paper_RandomQuestionNum裡的題目數量
            intReturn += this.getPaperRandomQuestionNum(strPaperID);

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷裏由Author選題的題目數量(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperContentQuestionNum(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = "SELECT COUNT (cQID) AS 'ContentCount' FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' ";
            DataSet dsContent = sqldb.getDataSet(strSQL);
            if (dsContent.Tables[0].Rows.Count > 0)
            {
                intReturn += Convert.ToInt32(dsContent.Tables[0].Rows[0]["ContentCount"]);
            }
            dsContent.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷裏由Author選題建議選項的題目數量(From Paper_Content)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperContentSuggestQuestionNum(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = "SELECT COUNT(cSelectionID) AS 'ContentCount' FROM Paper_Content C , QuestionSelectionIndex Q WHERE cPaperID = '" + strPaperID + "' AND C.cQID = Q.cQID AND bCaseSelect = '1' ";
            DataSet dsContent = sqldb.getDataSet(strSQL);
            if (dsContent.Tables[0].Rows.Count > 0)
            {
                intReturn += Convert.ToInt32(dsContent.Tables[0].Rows[0]["ContentCount"]);
            }
            dsContent.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回使用者操作某問卷的Log題目中的建議選項的題目數量
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getsSuggestQuestionNumFromLog(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = "SELECT COUNT(cSelectionID) AS 'ContentCount' FROM Paper_Content C , QuestionSelectionIndex Q WHERE cPaperID = '" + strPaperID + "' AND C.cQID = Q.cQID AND bCaseSelect = '1' ";
            DataSet dsContent = sqldb.getDataSet(strSQL);
            if (dsContent.Tables[0].Rows.Count > 0)
            {
                intReturn += Convert.ToInt32(dsContent.Tables[0].Rows[0]["ContentCount"]);
            }
            dsContent.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回某問卷裡亂數選取題目的數量(From Paper_RandomQuestionNum)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperRandomQuestionNum(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = mySQL.getPaper_RandomQuestionNum(strPaperID);
            DataSet dsRandom = sqldb.getDataSet(strSQL);
            if (dsRandom.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsRandom.Tables[0].Rows.Count; i++)
                {
                    intReturn += Convert.ToInt32(dsRandom.Tables[0].Rows[i]["sQuestionNum"]);
                }
            }
            dsRandom.Dispose();

            return intReturn;
        }

        public string getPaperIDByPresentType(string strPresentType, string strCaseID, string strDivisionID, string strClinicNum, string strSectionName)
        {
            string strReturn = "";

            if (strPresentType == "Case")
            {
                strReturn = getPaperIDFromCaseDivision(strCaseID, strDivisionID);
            }
            else
            {
                strReturn = getPaperIDFromCaseDivisionSection(strCaseID, strClinicNum, strSectionName);
            }

            return strReturn;
        }

        /// <summary>
        /// 從Paper_CaseDivision取得PaperID
        /// </summary>
        /// <param name="strCaseID"></param>
        /// <param name="strDivisionID"></param>
        /// <returns></returns>
        public string getPaperIDFromCaseDivision(string strCaseID, string strDivisionID)
        {
            string strReturn = "";

            string strSQL = "SELECT * FROM Paper_CaseDivision WHERE cCaseID = '" + strCaseID + "' AND cDivisionID = '" + strDivisionID + "' ";
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

        /// <summary>
        /// 從CaseDivisionSection取得PaperID
        /// </summary>
        /// <param name="strCaseID"></param>
        /// <param name="strClinicNum"></param>
        /// <param name="strSectionName"></param>
        /// <returns></returns>
        public string getPaperIDFromCaseDivisionSection(string strCaseID, string strClinicNum, string strSectionName)
        {
            string strReturn = "";

            string strSQL = "SELECT * FROM Paper_CaseDivisionSection WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + strClinicNum + "' AND cSectionName = '" + strSectionName + "' ";
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

        /// <summary>
        /// 傳回某問卷在Paper_RandomQuestion中所有問題組別的問題總數
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getTotalQuestionNumFromRandomQuestion(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = mySQL.getPaper_RandomQuestionNum(strPaperID);
            DataSet dsQuestionNum = sqldb.getDataSet(strSQL);
            if (dsQuestionNum.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsQuestionNum.Tables[0].Rows.Count; i++)
                {
                    //get Question number
                    int intQuestionNum = 0;
                    try
                    {
                        intQuestionNum = Convert.ToInt32(dsQuestionNum.Tables[0].Rows[i]["sQuestionNum"]);

                    }
                    catch
                    {
                    }

                    //把此組別的問題數量累加進intReturn
                    intReturn += intQuestionNum;
                }
            }
            dsQuestionNum.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 傳回在系統亂數選題時，傳回某組別中尚未被選取的題目數量。
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getRandomSelectionQuestionCountLevel1NotSelect(string strGroupID, string strPaperID)
        {
            int intReturn = 0;
            int intQuestionNum = 0;
            int intSelectNum = 0;

            //取得此組別有幾個Level 1的題目
            string strSQL = "SELECT COUNT(*) AS 'sNum' FROM QuestionIndex I , QuestionMode M " +
                "WHERE  M.cQuestionGroupID = '" + strGroupID + "' " +
                "AND I.cQID = M.cQID AND I.sLevel = '1' ";
            DataSet dsQuestionNum = sqldb.getDataSet(strSQL);
            if (dsQuestionNum.Tables[0].Rows.Count > 0)
            {
                if (dsQuestionNum.Tables[0].Rows[0]["sNum"] != DBNull.Value)
                {
                    intQuestionNum = Convert.ToInt32(dsQuestionNum.Tables[0].Rows[0]["sNum"]);
                }
            }
            dsQuestionNum.Dispose();

            //取得此問卷在此組別下已經亂數選取了幾個題目
            strSQL = mySQL.getPaper_RandomQuestionNumByGroup(strPaperID, strGroupID);
            DataSet dsSelectNum = sqldb.getDataSet(strSQL);
            if (dsSelectNum.Tables[0].Rows.Count > 0)
            {
                try
                {
                    intSelectNum = Convert.ToInt32(dsSelectNum.Tables[0].Rows[0]["sQuestionNum"]);
                }
                catch
                {
                }
            }
            dsSelectNum.Dispose();

            if (intQuestionNum >= intSelectNum)
            {
                intReturn = intQuestionNum - intSelectNum;
            }

            return intReturn;
        }

        /// <summary>
        /// 傳回在系統亂數選題時，傳回某問卷中尚未被選取的Specific題目數量。
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getRandomSpecificSelectionQuestionCountLevel1NotSelect(string strPaperID)
        {
            int intReturn = 0;
            int intQuestionNum = 0;
            int intSelectNum = 0;

            //取得此組別有幾個Level 1的題目
            string strSQL = " SELECT COUNT(*) AS 'sNum' FROM QuestionIndex I , QuestionMode M " +
                " WHERE  M.cPaperID = '" + strPaperID + "' AND M.cQuestionMode = 'Specific' " +
                " AND I.cQID = M.cQID AND I.sLevel = '1' ";

            DataSet dsQuestionNum = sqldb.getDataSet(strSQL);
            if (dsQuestionNum.Tables[0].Rows.Count > 0)
            {
                if (dsQuestionNum.Tables[0].Rows[0]["sNum"] != DBNull.Value)
                {
                    intQuestionNum = Convert.ToInt32(dsQuestionNum.Tables[0].Rows[0]["sNum"]);
                }
            }
            dsQuestionNum.Dispose();

            //取得此問卷在此組別下已經亂數選取了幾個題目
            strSQL = mySQL.getPaper_RandomQuestionNumBySpecific(strPaperID);
            DataSet dsSelectNum = sqldb.getDataSet(strSQL);
            if (dsSelectNum.Tables[0].Rows.Count > 0)
            {
                try
                {
                    intSelectNum = Convert.ToInt32(dsSelectNum.Tables[0].Rows[0]["sQuestionNum"]);
                }
                catch
                {
                }
            }
            dsSelectNum.Dispose();

            if (intQuestionNum >= intSelectNum)
            {
                intReturn = intQuestionNum - intSelectNum;
            }

            return intReturn;
        }

        /// <summary>
        /// 傳回某個問題組別下，尚未被選取至Paper_Content的題目數量。
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getGroupSelectionQuestionCountLevel1NotSelect(string strGroupID, string strPaperID)
        {
            int intReturn = 0;
            string strSQL = "SELECT COUNT(*) AS 'sNum' FROM QuestionIndex I , QuestionMode M " +
                "WHERE  M.cQuestionGroupID = '" + strGroupID + "' " +
                "AND I.cQID = M.cQID AND I.sLevel = '1'  AND NOT EXISTS " +
                "(SELECT * FROM Paper_Content C WHERE cPaperID = '" + strPaperID + "' AND I.cQID = C.cQID AND M.cQID = C.cQID ) ";

            DataSet QuestionCount = sqldb.getDataSet(strSQL);
            if (QuestionCount.Tables[0].Rows.Count > 0)
            {
                try
                {
                    intReturn = Convert.ToInt32(QuestionCount.Tables[0].Rows[0]["sNum"]);
                }
                catch
                {
                }
            }
            QuestionCount.Dispose();
            return intReturn;
        }

        /// <summary>
        /// 傳回某個問卷下尚未被選取至Paper_Content的Specific問題數量。
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getSpecificSelectionQuestionCountLevel1NotSelect(string strPaperID)
        {
            int intReturn = 0;
            string strSQL = " SELECT COUNT(*) AS 'sNum' " +
                " FROM QuestionIndex I , QuestionMode M " +
                " WHERE  M.cPaperID = '" + strPaperID + "' AND M.cQuestionMode = 'Specific' " +
                " AND I.cQID = M.cQID AND I.sLevel = '1'  AND NOT EXISTS " +
                " (SELECT * FROM Paper_Content C WHERE cPaperID = '" + strPaperID + "' AND I.cQID = C.cQID AND M.cQID = C.cQID ) ";

            DataSet QuestionCount = sqldb.getDataSet(strSQL);
            if (QuestionCount.Tables[0].Rows.Count > 0)
            {
                try
                {
                    intReturn = Convert.ToInt32(QuestionCount.Tables[0].Rows[0]["sNum"]);
                }
                catch
                {
                }
            }
            QuestionCount.Dispose();
            return intReturn;
        }

        /// <summary>
        /// 在呈現時系統亂數選題的機制下，取得某問卷在Paper_GroupingQuestion中的每一個問題組別的問題總數。
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getQuestionCountFromGroupingQuestion(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = "SELECT * FROM Paper_GroupingQuestion WHERE cPaperID = '" + strPaperID + "' ";
            DataSet dsGroup = sqldb.getDataSet(strSQL);
            if (dsGroup.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsGroup.Tables[0].Rows.Count; i++)
                {
                    //get QuestionNum
                    int intQuestionNum = 0;
                    try
                    {
                        intQuestionNum = Convert.ToInt32(dsGroup.Tables[0].Rows[i]["sQuestionNum"]);
                    }
                    catch
                    {
                    }

                    //累加進intReturn
                    intReturn += intQuestionNum;
                }
            }
            dsGroup.Dispose();

            return intReturn;
        }

        /// <summary>
        /// 取得某一份問卷中的題目總數(Question_Content)
        /// </summary>
        /// <param name="strEditMethod"></param>
        /// <param name="strGenerationMethod"></param>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperQuestionCountFromContent(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = "SELECT COUNT(*) AS 'Count' FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' ";
            DataSet dsCount = sqldb.getDataSet(strSQL);
            if (dsCount.Tables[0].Rows.Count > 0)
            {
                try
                {
                    intReturn = Convert.ToInt32(dsCount.Tables[0].Rows[0]["Count"]);
                }
                catch
                {
                }
            }
            dsCount.Dispose();

            return intReturn;
        }

        public string getGroupDivisionID(string GroupID)
        {
            string DivisionID = "";
            string strSQL = "SELECT * FROM QuestionGroupTree WHERE cNodeID='" + GroupID + "'";
            DataTable dt = sqldb.getDataSet(strSQL).Tables[0];

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                string NodeType = dr["cNodeType"].ToString();
                string NodeID = dr["cNodeID"].ToString();
                string ParentID = dr["cParentID"].ToString();

                if (!NodeType.Equals("Division"))
                {
                    DivisionID = getGroupDivisionID(ParentID);
                }
                else
                {
                    DivisionID = NodeID.Split('_')[1];
                }
            }
            else
            {
                DivisionID = "";
            }

            return DivisionID;
        }

        /// <summary>
        /// 檢查某個問卷裏的某個問題是否存在於Paper_Content
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public bool checkExistPaperContent(string strPaperID, string strQID)
        {
            bool bReturn = false;
            string strSQL = "SELECT * FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' AND cQID = '" + strQID + "' ";
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
        /// 取得某問卷在Paper_Content中最高的問題順序。
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperContentMaxSeq(string strPaperID)
        {
            int intReturn = 0;

            string strSQL = "SELECT MAX(sSeq) AS 'sSeq' FROM Paper_Content WHERE cPaperID = '" + strPaperID + "' ";
            DataSet dsMaxSeq = sqldb.getDataSet(strSQL);
            if (dsMaxSeq.Tables[0].Rows.Count > 0)
            {
                try
                {
                    intReturn = Convert.ToInt32(dsMaxSeq.Tables[0].Rows[0]["sSeq"]);
                }
                catch
                {
                }
            }
            dsMaxSeq.Dispose();
            return intReturn;
        }

        public bool IsNum(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str, i))
                {
                    return false;
                }
            }
            return true;
        }


        public string getNowTime()
        {
            //此function會傳回符合資料庫定義的時間格式的現在時間
            string strYear, strMonth, strDay, strHour, strMin, strSec;

            //get year
            strYear = DateTime.Now.Year.ToString();
            //get month
            if (DateTime.Now.Month > 9)
            {
                strMonth = DateTime.Now.Month.ToString();
            }
            else
            {
                strMonth = "0" + DateTime.Now.Month.ToString();
            }
            //get day
            if (DateTime.Now.Day > 9)
            {
                strDay = DateTime.Now.Day.ToString();
            }
            else
            {
                strDay = "0" + DateTime.Now.Day.ToString();
            }
            //get Hour
            if (DateTime.Now.Hour > 9)
            {
                strHour = DateTime.Now.Hour.ToString();
            }
            else
            {
                strHour = "0" + DateTime.Now.Hour.ToString();
            }
            //get min
            if (DateTime.Now.Minute > 9)
            {
                strMin = DateTime.Now.Minute.ToString();
            }
            else
            {
                strMin = "0" + DateTime.Now.Minute.ToString();
            }
            //get sec
            if (DateTime.Now.Second > 9)
            {
                strSec = DateTime.Now.Second.ToString();
            }
            else
            {
                strSec = "0" + DateTime.Now.Second.ToString();
            }

            string strReturn = strYear + strMonth + strDay + strHour + strMin + strSec;
            return strReturn;
        }

        public string calculateSectionDuration(string strCaseID, string strDivisionID, string strUserLevel, string strUserID, string strStartTime)
        {
            string strReturn = "";
            int intSum = 0;
            string strSQL = "";
            strSQL = " SELECT * FROM Summary_Header Record , Summary_Seq Seq , UserLevelPresent P , HintsUser U " +
                " WHERE Seq.cCaseID = '" + strCaseID + "' AND Seq.cDivisionID = '" + strDivisionID + "' AND Seq.cUserID = '" + strUserID + "'  AND Seq.cUserLevel = '" + strUserLevel + "' AND Seq.cStartTime = '" + strStartTime + "' " +
                " AND Record.cCaseID = Seq.cCaseID AND Record.cDivisionID = Record.cDivisionID AND Record.cUserID = Seq.cUserID AND Record.cStartTime = Seq.cStartTime AND Record.cUserLevel = Seq.cUserLevel " +
                " AND Seq.cUserLevel = P.cUserLevel AND Seq.cDivisionID = P.cDivisionID AND Seq.cCaseID = P.cCaseID AND Seq.sClinicNum = P.sClinicNum AND Seq.cSectionName = P.cSectionName " +
                " AND Seq.cUserID = U.cUserID " +
                " ORDER BY Seq.cActionTime";

            DataSet dsCase = sqldb.getDataSet(strSQL);
            //判斷此筆資料是否有EndTime
            if (dsCase.Tables[0].Rows.Count != 0)
            {
                if (dsCase.Tables[0].Rows[0]["cEndTime"] == DBNull.Value)
                {
                    //沒有cEndTime
                    intSum += this.withoutEndTime(dsCase);
                }
                else
                {
                    //有EndTime
                    intSum += this.withEndTime(dsCase);
                }
            }
            dsCase.Dispose();

            strReturn = this.setMinSec(intSum.ToString());
            return strReturn;
        }

        private int withoutEndTime(DataSet dsCase)
        {
            int intReturn = 0;
            for (int i = 0; i < (dsCase.Tables[0].Rows.Count - 1); i++)
            {
                //呼叫時間相減
                string strSeq = Convert.ToString(i + 1);
                string strTime = subTime(dsCase.Tables[0].Rows[i]["cActionTime"].ToString(), dsCase.Tables[0].Rows[i + 1]["cActionTime"].ToString());
                if (Convert.ToInt32(strTime) > 600)//因為有些學生會做到一半登出系統，隔幾天之後再來做，沒有EndTime的話不能偵測此種情形。
                {
                    strTime = "180";
                }
                intReturn += Convert.ToInt32(strTime);
            }

            //BackupTime - 最後一個ActionTime
            string strLastSection = "";
            string strBackupTime = "";
            string strLastAction = "";
            try
            {
                strBackupTime = dsCase.Tables[0].Rows[dsCase.Tables[0].Rows.Count - 1]["cBackupTime"].ToString();
                strLastAction = dsCase.Tables[0].Rows[dsCase.Tables[0].Rows.Count - 1]["cActionTime"].ToString();
                strLastSection = subTime(strLastAction, strBackupTime);
            }
            catch
            {
            }
            int intLastSection = 0;
            intLastSection = Convert.ToInt32(strLastSection);
            if (intLastSection > 600)
            {
                intLastSection = 180;
            }

            intReturn += intLastSection;

            return intReturn;
        }

        private int withEndTime(DataSet dsCase)
        {
            int intReturn = 0;
            for (int i = 0; i < dsCase.Tables[0].Rows.Count; i++)
            {
                //呼叫時間相減
                string strSeq = Convert.ToString(i + 1);
                if (dsCase.Tables[0].Rows[i]["cEndTime"].ToString().Trim() != "")
                {
                    if (dsCase.Tables[0].Rows[i]["cEndTime"].ToString().Trim().Length == 14)
                    {
                        string strTime = subTime(dsCase.Tables[0].Rows[i]["cActionTime"].ToString(), dsCase.Tables[0].Rows[i]["cEndTime"].ToString());
                        intReturn += Convert.ToInt32(strTime);
                    }
                }
            }
            return intReturn;
        }

        public string subTime(string strStartTime, string strEndTime)
        {
            DateTime NowTime = setDateTime(strStartTime);
            DateTime PostTime = setDateTime(strEndTime);

            TimeSpan Time = PostTime.Subtract(NowTime);

            return Convert.ToString(Convert.ToInt32(Time.TotalSeconds));
        }

        public DateTime setDateTime(string PreTime)
        {
            //--此函式的目的是將傳入的資料庫時間字串轉換成一般系統格式的時間字串(相當重要)--//
            char[] temp;
            temp = PreTime.ToCharArray();
            string strDateTime, strYear, strMonth, strDate, strHour, strMin, strSec = "";
            strYear = temp[0].ToString() + temp[1].ToString() + temp[2].ToString() + temp[3].ToString();
            strMonth = temp[4].ToString() + temp[5].ToString();
            strDate = temp[6].ToString() + temp[7].ToString();
            strHour = temp[8].ToString() + temp[9].ToString();
            if (strHour.Trim() == "")
            {
                strHour = "00";
            }
            strMin = temp[10].ToString() + temp[11].ToString();
            if (strMin.Trim() == "")
            {
                strMin = "00";
            }
            strSec = temp[12].ToString() + temp[13].ToString();
            if (strSec.Trim() == "")
            {
                strSec = "00";
            }
            strDateTime = strYear + "/" + strMonth + "/" + strDate + " " + strHour + ":" + strMin + ":" + strSec;
            DateTime ReturnTime = Convert.ToDateTime(strDateTime);
            return ReturnTime;
        }

        public string setMinSec(string strTime)
        {
            string strReturn = "";
            string strHour = "";
            string strMin = "";
            string strSec = "";
            int Hour = 0;
            int Min = 0;
            int Sec = 0;
            if (strTime == "0")
            {
                strReturn = "00'00";
            }
            else
            {
                Min = int.Parse(strTime) / 60;
                Sec = int.Parse(strTime) % 60;
                if (Min > 60)
                {
                    Hour = Min / 60;
                    Min = Min % 60;
                }
                if (Hour < 10)
                    strHour = "0" + Hour.ToString();
                else
                    strHour = Hour.ToString();
                if (Min < 10)
                    strMin = "0" + Min.ToString();
                else
                    strMin = Min.ToString();
                if (Sec < 10)
                    strSec = "0" + Sec.ToString();
                else
                    strSec = Sec.ToString();
                if (Hour == 0)
                {
                    strReturn = strMin + "'" + strSec;
                }
                else
                {
                    strReturn = strHour + "'" + strMin + "'" + strSec;
                }
            }
            return strReturn;
        }

        public int getUserTotalTime(string strUserLevel, string strCaseID, string strDivisionID, string strUserID)
        {
            string strSQL = " SELECT  SUM(T.iTime) AS 'iTime' " +
                " FROM UserLevelPresent U " +
                " LEFT JOIN " +
                " ( " +
                " SELECT T.cUserLevel , T.cCaseID , T.cDivision , T.cUserID , T.sClinicNum , T.cSectionName , T.iTime " +
                " FROM SectionLogTab T , Summary_UserRecord R " +
                " WHERE T.cUserLevel = '" + strUserLevel + "' AND T. cUserID = '" + strUserID + "'  AND T.cCaseID = '" + strCaseID + "'   AND T.cDivision = '" + strDivisionID + "' " +
                " AND T.cUserLevel = R.cUserLevel AND T.cUserID = R.cUserID   AND T.cCaseID = R.cCaseID AND T.cDivision = R.cDivisionID AND T.cStartTime = R.cStartTime " +
                " ) T " +
                " ON   U.cSectionName = T.cSectionName AND T.cCaseID = U.cCaseID AND  T.cDivision = U.cDivisionID   AND T.cUserLevel = U.cUserLevel AND T.sClinicNum = U.sClinicNum " +
                " WHERE U.cCaseID = '" + strCaseID + "' AND U.cDivisionID = '" + strDivisionID + "' AND U.cUserLevel = '" + strUserLevel + "'";
            DataSet dsUserTotalTime = sqldb.getDataSet(strSQL);
            int intTotal = 0;
            if (dsUserTotalTime.Tables[0].Rows.Count > 0)
            {
                intTotal = Convert.ToInt32(dsUserTotalTime.Tables[0].Rows[0]["iTime"]);
            }
            return intTotal;
        }

        public string getUserName(string strUserID)
        {
            string strUserName = "";
            string strSQL = "SELECT DISTINCT cFullName FROM HintsUser WHERE cUserID = '" + strUserID + "'";
            DataSet dsUserName = sqldb.getDataSet(strSQL);
            if (dsUserName.Tables[0].Rows.Count > 0)
            {
                strUserName = dsUserName.Tables[0].Rows[0]["cFullName"].ToString();
            }
            dsUserName.Dispose();
            return strUserName;
        }

        public string getCaseName(string strCaseID)
        {
            string strReturn = "";
            string strCaseNameSQL = "SELECT * FROM TeachingCase WHERE cCaseID = '" + strCaseID + "'";
            DataSet dsCaseName = sqldb.getDataSet(strCaseNameSQL);
            if (dsCaseName.Tables[0].Rows.Count != 0)
            {
                strReturn = dsCaseName.Tables[0].Rows[0]["cCaseName"].ToString();
            }
            return strReturn;
        }

        /// <summary>
        /// 取得題庫的題目ID
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public DataTable GetQuestuionID(string strGroupID, string strPaperID)
        {
            DataTable dtQuestionID = new DataTable();
            string strSQL = "SELECT cQID FROM QuestionIndex I , QuestionMode M " +
                "WHERE  M.cQuestionGroupID = '" + strGroupID + "' " +
                "AND I.cQID = M.cQID AND I.sLevel = '1'  AND NOT EXISTS " +
                "(SELECT * FROM Paper_Content C WHERE cPaperID = '" + strPaperID + "' AND I.cQID = C.cQID AND M.cQID = C.cQID ) ";
            dtQuestionID = sqldb.getDataSet(strSQL).Tables[0];
            return dtQuestionID;
        }
        public static DataTable GetQuestionLevelNum(string strGroupID)
        {
            int iQuesyionLevelNum = 0;
            string strSQL = "SELECT COUNT(*) AS QuestionLevelNum, cQuestionLevel FROM ViewQuestionLevelNum WHERE cQuestionGroupID='" + strGroupID + "' GROUP BY cQuestionLevel";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataTable dtQuestionLevelNum = myDB.getDataSet(strSQL).Tables[0];
            return dtQuestionLevelNum;
        }
        public static DataTable GetQuestionLevelAndGrade(string strGroupID, string strQID)
        {
            int iQuesyionLevelNum = 0;
            string strSQL = "SELECT cQuestionLevel, cQuestionGrade FROM ViewQuestionLevelNum WHERE cQuestionGroupID='" + strGroupID + "'AND cQID='" + strQID + "'";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataTable dtQuestionLevelAndGrade = myDB.getDataSet(strSQL).Tables[0];
            return dtQuestionLevelAndGrade;
        }

        //取得此node的子節點
        public static DataTable getChildNode(string cParentID)
        {
            DataTable dt = new DataTable();
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataTable dtQuestionGroupTreeNodeID = new DataTable();
            string strSQL_QuestionGroupTreeNodeID = "SELECT * FROM QuestionGroupTree WHERE cNodeName ='" + cParentID + "'";
            dtQuestionGroupTreeNodeID = myDB.getDataSet(strSQL_QuestionGroupTreeNodeID).Tables[0];
            if (dtQuestionGroupTreeNodeID.Rows.Count > 0)
            {
                string strSQL = "SELECT * FROM QuestionGroupTree WHERE cParentID ='" + dtQuestionGroupTreeNodeID.Rows[0]["cNodeID"].ToString() + "'";
                dt = myDB.getDataSet(strSQL).Tables[0];
            }

            return dt;
        }
        //煒凱新增擷取Profession分類 2014/9/22
        public static DataTable getProfessionNode(string Profession)
        {
            DataTable dt = new DataTable();
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL_QuestionGroupTreeNodeID = "SELECT * FROM QuestionGroupTree WHERE cParentID ='" + Profession + "'";
            dt = myDB.getDataSet(strSQL_QuestionGroupTreeNodeID).Tables[0];

            return dt;
        }
    }
}
