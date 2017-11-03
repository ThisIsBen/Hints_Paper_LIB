using System;
using System.Data;
using suro.util;

namespace PaperSystem
{
    /// <summary>
    /// DataReceiver ���K�n�y�z�C
    /// </summary>
    public class DataReceiver
    {
        //�إ�SqlDB����
        SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
        SQLString mySQL = new SQLString();

        public DataReceiver()
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
        }

        /// <summary>
        /// �Ǧ^�s������IP
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
        /// �Ǧ^������DomainName
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
        /// �Ǧ^�Y�Ӱݵ��D���D�ؤ��e
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
        /// �ھڰ��DID���o���D���e
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
        /// �ھڰ��DID�P����ID���o���פ��e
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
        /// ���o���w���DID���Ҧ�����
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
        /// ���o���w���DID���Ҧ�����
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
        /// ���o���w���DID �B ���׶}�Y�P�������ؤ@��
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
        /// �Ǧ^�Y��QuestionGroupName by QuestionGroupID
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
        /// �Ǧ^�Y��QuestionGroup Serial by QuestionGroupID
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
        /// �Ǧ^�Y��Section�bUserLevelPresent�̭���SectionWorkType
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
        /// �Ǧ^�Y��Section���ݪ�DivisionID
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
        /// �Ǧ^�Y�ݨ���bModify
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
        /// �Ǧ^�Y�ݨ���bMarkable
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
        /// �Ǧ^�Y�ݨ���bSwitch
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
        /// �Ǧ^�Y�ݨ���bIsSituationExam
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
        /// �Ǧ^�Y�ݨ���bIsSituationExam
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
        /// �Ǧ^�Y�ݨ���bSectionSummaryShowCorrectAnswer
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
        /// ���o�Y�ӿﶵ��Response���
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
        /// �qQuestion_Retry���o�Y�Ӱ��D�i�HRetry������
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
        /// ��J�O�����s���A�Ǧ^�ӰO���Ϥ���Src���|
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
        /// ���o�Y�ӦbPaper_AssignedQuestion�D�ت��O���Ϥ�URL
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
        /// ���o�Y�ӦbPaper_AssignedQuestion�D�ت��O���Ϥ�URL
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
        /// �ק�@����ƨ�Paper_AssignedQuestion��sMarkSymbol���
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
        /// �ק�@����ƨ�Paper_AssignedQuestion��sMarkSymbol���
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
        /// �Ǧ^�Y�ӦbPaper_AssignedQuestion�D�ت��O������
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
        /// �Ǧ^�Y�ӦbPaper_AssignedQuestion�D�ت��O������
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
        /// �Ǧ^�@�Ӧs�b��Paper_AssignedQuestion���Y�D�����D�ظ��
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
        /// �ˬd�Y�@��PaperID , StartTime , UserID�O�_�s�b��Paper_AssignedQuestion
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
        /// �ˬd�Y�@��QID�O�_�s�b��Paper_AssignedQuestion
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
        /// �Ǧ^�Y�ӨϥΪ̦APaper_AssignedQuestion���Y�@���ݨ����̤j�D��(sSeq)
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
        /// �Ǧ^�Y�ݨ���sAssignMethod
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
        /// �Ǧ^�Y�ݨ����@���ɶ�
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
        /// �Ǧ^�Y�ϥΪ̾ާ@�Y�ݨ��w�g�����F�h���D�ت��ƶq
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
        /// �Ǧ^�Y�ﶵ���ݪ�QID
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
        /// �ˬd�Y���D�ؤU���Y�ﶵ���S���s��Rationale
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
        /// �ھڰ��DID�P�ﶵID���o�ﶵ������
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
        /// �Ǧ^�Y���D���D��
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
        /// �Ǧ^�Y�ϧ��D���D��
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
        /// �Ǧ^�����D����ĳ�ﶵ�ƥ�
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
        /// �Ǧ^�ϥΪ̾ާ@�Y�ݨ��Y�D�ةҿ�ܪ��ﶵID
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
        /// �Ǧ^�Y�ݨ����Y���D�Y�ﶵ�U�@�B��SectionName
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
        /// �Ǧ^�Y�ݨ����Y���D�Y�ﶵ�U�@�B��QuestionSeq
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
        /// �Ǧ^�Y�ݨ����Y���D�U�@�B����k
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
        /// �ˬdPE�Y���즳�S���s��ݨ��C
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
        /// �Ǧ^PE�Y���쪺PaperID
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
        /// �Ǧ^�w�q�bPaper_Content���U�@�ӭn�e�{��Section�W��
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
        /// �ˬd�ϥΪ̦��S������Y���D��(TempLog_PaperSelectionAnswer)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strStartTime"></param>
        /// <param name="strQID"></param>
        /// <returns></returns>
        public static bool checkUserLogIsCorrectFromTempLogByQID(string strPaperID, string strUserID, string strStartTime, string strQID)
        {
            bool bReturn = false;

            //�����D����ĳ�ﶵ�ƥ�
            int intQuestionSuggestedCount = getSuggestedSelectionCountByQID(strQID);

            //�ϥΪ̾ާ@�����D���諸��ĳ�ﶵ�ƥ�
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
                        //�ϥΪ̿���ĳ�ﶵ
                        intUserSuggestedCount += 1;
                    }
                    else
                    {
                        //�ϥΪ̿��D��ĳ�ﶵ
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
        /// �ˬd�Y�ݨ����ϥΪ̾ާ@�����O�_�s�bTempLog True:TempLog 
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
        /// �ˬd�Y�ݨ����ϥΪ̾ާ@�����O�_�s�bSummary True:Summary
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
        /// �P�_�ثe�ɶ��O�_�j��ǤJ���~���6�Ӥ�A�p�G�O:�Ǧ^0�A�_�h�Ǧ^1�C
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
                        //	Yes �^��0
                        strReturn = "0";
                    }
                    else
                    {
                        //	No  �^��1
                        strReturn = "1";
                    }
                }
                else
                {
                    //	No  �^��1
                    strReturn = "1";
                }
            }
            else
            {
                //	No  �^��1
                strReturn = "1";
            }

            return strReturn;
        }

        /// <summary>
        /// �Ǧ^�Y�Ӱ��D�էO�U���X�Ӱ��D
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
        /// �Ǧ^�Y���D�ة��ݪ����D�էO
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
        /// �ˬd�ϥΪ̬O���O���ާ@�L���ݨ�
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
        /// �ˬd�ϥΪ̬O���O���I��L�����D
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
        /// �ˬd�ϥΪ̬O���O���I��L���ﶵ
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
        /// �Ǧ^�Y���D�ت��D�ؤ��e
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
        /// �ǤJ�@���D�ت��D���A�Ǧ^�Y�ݨ����üƿ��D�����D�����ݰ��D�էO��ID
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
                    //�֥[�ܰ��D�`��
                    intQuestionSum += Convert.ToInt32(ds.Tables[0].Rows[i]["sQuestionNum"]);

                    //���X�����D�էO��ID
                    strReturn = ds.Tables[0].Rows[i]["cQuestionGroupID"].ToString();

                    i += 1;
                } while (intQuestionSum < intIndex);
            }
            ds.Dispose();
            return strReturn;
        }

        /// <summary>
        /// �ǤJ�@���D���A�Ǧ^�Y�ݨ�������D���D���bPaper_Content��DataRow[]
        /// </summary>
        /// <returns></returns>
        public DataRow[] getQIDFromPaper_ContentByQuestionIndex(string strPaperID, int intIndex)
        {
            DataRow[] drArray = new DataRow[1];
            if (intIndex > 0)
            {
                int intRowIndex = intIndex - 1;
                //���oPaper_Content�����
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
        /// �ǤJ�@���D���A�Ǧ^�Y�ݨ��bPaper_AssignedQuestion��DataRow[]
        /// </summary>
        /// <returns></returns>
        public DataRow[] getQIDFromPaper_AssignedQuestionByQuestionIndex(string strPaperID, string strStartTime, string strUserID, int intIndex)
        {
            DataRow[] drArray = new DataRow[1];
            if (intIndex > 0)
            {
                //���oPaper_Content�����
                SQLString mySQL = new SQLString();
                DataTable dt = mySQL.getUserQuestionInPaper_AssignedQuestion(strPaperID, strStartTime, strUserID);

                drArray = dt.Select("sSeq = '" + intIndex.ToString() + "' ", "sSeq");

            }
            return drArray;
        }


        /// <summary>
        /// �Ǧ^�Y�ݨ��Ҧ����D���`�ƱqPaper_Content�MPaper_Random
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperAllQuestionCountFromContentAndRandom(string strPaperID)
        {
            int intReturn = 0;

            //���o���ݨ����Ҧ��D��(Paper_Content)
            int intContentQuestionCount = this.getPaperContentQuestionNum(strPaperID);

            //���o���ݨ����Ҧ��D��(Paper_RandomQuestionNum)
            int intRandomQuestionCount = this.getTotalQuestionNumFromRandomQuestion(strPaperID);

            intReturn = intContentQuestionCount + intRandomQuestionCount;

            return intReturn;
        }

        /// <summary>
        /// �Ǧ^�Y�ݨ����W��
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
        /// �Ǧ^�Y�ݨ������D��r
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
        /// �Ǧ^�Y�D�ت����D��r
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
        /// �Ǧ^�Y�ݨ������D�ت��ƶq(�]�tPaper_Content , Paper_Random)
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getPaperQuestionCount(string strPaperID)
        {
            int intReturn = 0;

            //�p��bPaper_Content�̪��D�ؼƶq
            intReturn += this.getPaperContentQuestionNum(strPaperID);

            //�p��bPaper_RandomQuestionNum�̪��D�ؼƶq
            intReturn += this.getPaperRandomQuestionNum(strPaperID);

            return intReturn;
        }

        /// <summary>
        /// �Ǧ^�Y�ݨ��إ�Author���D���D�ؼƶq(From Paper_Content)
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
        /// �Ǧ^�Y�ݨ��إ�Author���D��ĳ�ﶵ���D�ؼƶq(From Paper_Content)
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
        /// �Ǧ^�ϥΪ̾ާ@�Y�ݨ���Log�D�ؤ�����ĳ�ﶵ���D�ؼƶq
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
        /// �Ǧ^�Y�ݨ��̶üƿ���D�ت��ƶq(From Paper_RandomQuestionNum)
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
        /// �qPaper_CaseDivision���oPaperID
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
        /// �qCaseDivisionSection���oPaperID
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
        /// �Ǧ^�Y�ݨ��bPaper_RandomQuestion���Ҧ����D�էO�����D�`��
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

                    //�⦹�էO�����D�ƶq�֥[�iintReturn
                    intReturn += intQuestionNum;
                }
            }
            dsQuestionNum.Dispose();

            return intReturn;
        }

        /// <summary>
        /// �Ǧ^�b�t�ζüƿ��D�ɡA�Ǧ^�Y�էO���|���Q������D�ؼƶq�C
        /// </summary>
        /// <param name="strGroupID"></param>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getRandomSelectionQuestionCountLevel1NotSelect(string strGroupID, string strPaperID)
        {
            int intReturn = 0;
            int intQuestionNum = 0;
            int intSelectNum = 0;

            //���o���էO���X��Level 1���D��
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

            //���o���ݨ��b���էO�U�w�g�üƿ���F�X���D��
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
        /// �Ǧ^�b�t�ζüƿ��D�ɡA�Ǧ^�Y�ݨ����|���Q�����Specific�D�ؼƶq�C
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public int getRandomSpecificSelectionQuestionCountLevel1NotSelect(string strPaperID)
        {
            int intReturn = 0;
            int intQuestionNum = 0;
            int intSelectNum = 0;

            //���o���էO���X��Level 1���D��
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

            //���o���ݨ��b���էO�U�w�g�üƿ���F�X���D��
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
        /// �Ǧ^�Y�Ӱ��D�էO�U�A�|���Q�����Paper_Content���D�ؼƶq�C
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
        /// �Ǧ^�Y�Ӱݨ��U�|���Q�����Paper_Content��Specific���D�ƶq�C
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
        /// �b�e�{�ɨt�ζüƿ��D������U�A���o�Y�ݨ��bPaper_GroupingQuestion�����C�@�Ӱ��D�էO�����D�`�ơC
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

                    //�֥[�iintReturn
                    intReturn += intQuestionNum;
                }
            }
            dsGroup.Dispose();

            return intReturn;
        }

        /// <summary>
        /// ���o�Y�@���ݨ������D���`��(Question_Content)
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
        /// �ˬd�Y�Ӱݨ��ت��Y�Ӱ��D�O�_�s�b��Paper_Content
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
        /// ���o�Y�ݨ��bPaper_Content���̰������D���ǡC
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
            //��function�|�Ǧ^�ŦX��Ʈw�w�q���ɶ��榡���{�b�ɶ�
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
            //�P�_������ƬO�_��EndTime
            if (dsCase.Tables[0].Rows.Count != 0)
            {
                if (dsCase.Tables[0].Rows[0]["cEndTime"] == DBNull.Value)
                {
                    //�S��cEndTime
                    intSum += this.withoutEndTime(dsCase);
                }
                else
                {
                    //��EndTime
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
                //�I�s�ɶ��۴�
                string strSeq = Convert.ToString(i + 1);
                string strTime = subTime(dsCase.Tables[0].Rows[i]["cActionTime"].ToString(), dsCase.Tables[0].Rows[i + 1]["cActionTime"].ToString());
                if (Convert.ToInt32(strTime) > 600)//�]�����Ǿǥͷ|����@�b�n�X�t�ΡA�j�X�Ѥ���A�Ӱ��A�S��EndTime���ܤ��఻�����ر��ΡC
                {
                    strTime = "180";
                }
                intReturn += Convert.ToInt32(strTime);
            }

            //BackupTime - �̫�@��ActionTime
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
                //�I�s�ɶ��۴�
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
            //--���禡���ت��O�N�ǤJ����Ʈw�ɶ��r���ഫ���@��t�ή榡���ɶ��r��(�۷��n)--//
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
        /// ���o�D�w���D��ID
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

        //���o��node���l�`�I
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
        //�m�ͷs�W�^��Profession���� 2014/9/22
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
