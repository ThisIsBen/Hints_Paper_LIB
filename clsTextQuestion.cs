using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using suro.util;


namespace PaperSystem
{
	/// <summary>
	/// clsTextQuestion ���K�n�y�z�C
	/// </summary>
	public class clsTextQuestion
	{
		public clsTextQuestion()
		{
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
		}

		/// <summary>
		/// �x�s�s��ݵ��D�����W���@�D�ݵ��D���
		/// </summary>
		/// <param name="WebPage"></param>
		/// <param name="strPaperID"></param>
		/// <param name="strGroupDivisionID"></param>
		/// <param name="strGroupID"></param>
		/// <param name="strQuestionMode"></param>
		public void saveTextQuestion(string strQID , string strQuestion , string strAnswer ,  string strUserID , string strPaperID , string strQuestionDivisionID , string strQuestionGroupID , string strQuestionMode)
		{
            //�x�s�@����Ʀ�QuestionIndex
            saveIntoQuestionIndex(strQID, strQuestion,strAnswer, 1);

			//�x�s�@����Ʀ�Paper_TextQuestion
			saveIntoPaper_TextQuestion(strQID , strQuestion, strAnswer , 1);

			//�x�s�@����Ʀ�QuestionMode
			SQLString mySQL = new SQLString();
			mySQL.saveIntoQuestionMode(strQID , strPaperID , strQuestionDivisionID , strQuestionGroupID , strQuestionMode , "2");
		}

        /// <summary>
        /// �x�s�s��ݵ��D�����W���@�D�ݵ��D��� �s�ݵ��D��i��ƪ� QuestionAnswer_Question  QuestionAnswer_Answer
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="strAnswer"></param>
        /// <param name="strUserID"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strQuestionDivisionID"></param>
        /// <param name="strQuestionGroupID"></param>
        /// <param name="strQuestionMode"></param>
        public void saveQuestionAnswer(string strQID, string strAID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode)
        {
            //�x�s�@����Ʀ�QuestionIndex 
            saveIntoQuestionIndex(strQID, strQuestion, strAnswer, 1);

            //�x�s�@����Ʀ�QuestionAnswer_Question
            saveIntoQuestionAnswer_Question(strQID, strQuestion);

            //�x�s�@����Ʀ�QuestionAnswer_Answer
            saveIntoQuestionAnswer_Answer(strQID, strAID, strAnswer);

            //�x�s�@����Ʀ�QuestionMode
            SQLString mySQL = new SQLString();
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "2");
        }
        /// <summary>
        /// �x�s�s��ݵ��D�����W���@�D�ݵ��D��� �s�ݵ��D��i��ƪ� QuestionAnswer_Question  QuestionAnswer_Answer
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
        public void saveProgramQuestionAnswer(string strQID, string strAID, string strQuestion, string strAnswer, string strUserID, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode,string strTextTestDataContent, string strTextOutputFormatContent)
        {
            //�x�s�@����Ʀ�QuestionIndex //###parameter "strAnswer" can set to NULL in �{���D
            saveIntoQuestionIndex(strQID, strQuestion, null, 1);
            
            //�x�s�@����Ʀ�QuestionMode
            SQLString mySQL = new SQLString();
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "7");


            // store question description to DB
            //e.g., saveIntoQuestionAnswer_Question(strQID, strQuestion);

            // store correct answer to DB
            //e.g., saveIntoQuestionAnswer_Answer(strQID, strAID, strAnswer);
        }
        /// <summary>
        /// �x�s�@����Ʀ�Paper_TextQuestion
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="intLevel"></param>
        public static void saveIntoPaper_TextQuestion(string strQID , string strQuestion,string strAnswer , int intLevel)
		{
			string strSQL = "";
			strSQL = "SELECT * FROM Paper_TextQuestion WHERE cQID = '"+strQID+"' ";
			SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
			DataSet dsCheck = myDB.getDataSet(strSQL);
			if(dsCheck.Tables[0].Rows.Count > 0)
			{
				//Update
				strSQL = "UPDATE Paper_TextQuestion SET cQuestion = @cQuestion,cAnswer = @cAnswer , sLevel = '"+intLevel.ToString()+"' "+
						 "WHERE cQID = '"+strQID+"' ";
			}
			else
			{
				//Insert
				strSQL = "INSERT INTO Paper_TextQuestion (cQID , cQuestion, cAnswer , sLevel) "+
						 "VALUES ('"+strQID+"' , @cQuestion,@cAnswer , '"+intLevel.ToString()+"') ";
			}
			dsCheck.Dispose();

			object[] pList = {strQuestion,strAnswer};
			myDB.ExecuteNonQuery(strSQL,pList);
		}

        /// <summary>
        /// �x�s�@����Ʀ�QuestionIndex
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="intLevel"></param>
        public static void saveIntoQuestionIndex(string strQID, string strQuestion,string strAnswer, int intLevel)
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

            object[] pList = { strQuestion,strAnswer };
            myDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// �x�s�@����Ʀ�QuestionAnswer_Question
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        public static void saveIntoQuestionAnswer_Question(string strQID, string strQuestion)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM QuestionAnswer_Question WHERE cQID = '" + strQID + "' ";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE QuestionAnswer_Question SET cQuestion = @cQuestion " +
                         "WHERE cQID = '" + strQID + "' ";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO QuestionAnswer_Question (cQID , cQuestion) " +
                         "VALUES ('" + strQID + "' , @cQuestion ) ";
            }
            dsCheck.Dispose();

            object[] pList = { strQuestion };
            myDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// �x�s�@����Ʀ�QuestionAnswer_Answer
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strAID"></param>
        /// <param name="strAnswer"></param>
        public static void saveIntoQuestionAnswer_Answer(string strQID, string strAID, string strAnswer)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM QuestionAnswer_Answer WHERE cQID = '" + strQID + "'";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsCheck = myDB.getDataSet(strSQL);
            if (dsCheck.Tables[0].Rows.Count > 0)
            {
                //Update
                strSQL = "UPDATE QuestionAnswer_Answer SET cAnswer = @cAnswer " +
                         "WHERE cQID = '" + strQID + "'";
            }
            else
            {
                //Insert
                strSQL = "INSERT INTO QuestionAnswer_Answer (cQID , cAID, cAnswer) " +
                         "VALUES ('" + strQID + "' , '" + strAID + "', @cAnswer ) ";
            }
            dsCheck.Dispose();

            object[] pList = { strAnswer };
            myDB.ExecuteNonQuery(strSQL, pList);
        }

        /// <summary>
        /// �x�s�s��ϧ��D���
        /// </summary>
        /// <param name="WebPage"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strGroupDivisionID"></param>
        /// <param name="strGroupID"></param>
        /// <param name="strQuestionMode"></param>
        public void saveSimulatorQuestion(string strQID, string strQuestion, string strPaperID, string strQuestionDivisionID, string strQuestionGroupID, string strQuestionMode)
        {
            //�x�s�@����Ʀ�QuestionIndex
            string strAnswer = "";
            saveIntoQuestionIndex(strQID, strQuestion, strAnswer, 1);

            //�x�s�@����Ʀ�Paper_TextQuestion
            //saveIntoPaper_TextQuestion(strQID, strQuestion, strAnswer, 1);
            //�x�s�@����Ʀ�Paper_simulatorQuestion
            //saveIntoQuestionSimulator(strQID, strAnswer, strOrder, strSimulatorID);
            //�x�s�@����Ʀ�QuestionMode  strPaperID=NULL strQuestionDivisionID="" strQuestionGroupID, strQuestionMode=Session
            SQLString mySQL = new SQLString();
            mySQL.saveIntoQuestionMode(strQID, strPaperID, strQuestionDivisionID, strQuestionGroupID, strQuestionMode, "3");
        }

        /// <summary>
        /// �x�s�@����Ʀ�QuestionSimulator
        /// </summary>
        /// <param name="strQID"></param>
        /// <param name="strQuestion"></param>
        /// <param name="intLevel"></param>
        public void saveIntoQuestionSimulator(string strQID, string strAnswer, string strOrder, string strSimulatorID, string cContent, string cAnsID)
        {   //�x�s�@����Ʀ�Question_Simulator
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
