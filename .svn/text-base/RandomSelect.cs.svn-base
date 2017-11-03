using System;
using System.Data;
using System.Data.SqlClient;
using suro.util;

namespace PaperSystem
{
	/// <summary>
	/// RandomSelect 的摘要描述。
	/// </summary>
	public class RandomSelect
	{
		//建立SqlDB物件
		SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
		//隨機物件
		System.Random myRandom = new Random();
		SQLString mySQL = new SQLString();
		DataReceiver myReceiver = new DataReceiver();

		public RandomSelect()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}
/*
		public string[] getRandomQIDArray(string strPaperID , int intQuestionNum)
		{
			///亂數取得某一個問題組別中的問題

			//傳回的陣列
			string[] arrayQID = new String[intQuestionNum];
			int intArrayIndex = 0;//代表arrayQID的指標

			//取得Paper_Grouping的資料
			string strSQL = "SELECT * FROM Paper_GroupingQuestion WHERE cPaperID = '"+strPaperID+"' ";
			DataSet dsGroup = sqldb.getDataSet(strSQL);
			if(dsGroup.Tables[0].Rows.Count > 0)
			{
				for(int i=0 ; i<dsGroup.Tables[0].Rows.Count ; i++)
				{
					//針對每一個組別找出屬於此分組的題目
					
					//QuestionGroupID
					string strGroupID = "";
					try
					{
						strGroupID = dsGroup.Tables[0].Rows[i]["cGroupID"].ToString();
					}
					catch
					{
					}

					//Question number
					int intGroupQuestionNum = 0;
					try
					{
						intGroupQuestionNum = Convert.ToInt32(dsGroup.Tables[0].Rows[i]["sQuestionNum"]);
					}
					catch
					{
					}

					//GroupDivision
					string strDivisionID = "";
					try
					{
						strDivisionID =	dsGroup.Tables[0].Rows[i]["cDivisionID"].ToString();
					}
					catch
					{
					}

					//取得屬於該組別並且尚未被引用的問題集合
					strSQL = mySQL.getGroupQuestionLevel1NotSelect(strGroupID , strPaperID);
					DataSet dsQuestionList = sqldb.getDataSet(strSQL);

					//亂數選取問題後存入Array中
					//檢查intQuestionNum是否大於QuestionList中的題目數量
					if(intGroupQuestionNum <= dsQuestionList.Tables[0].Rows.Count)
					{
						for(int j=0 ; j<intGroupQuestionNum ; j++)
						{
							//亂數取得一個介於0~RowCount之間的數字
							int intRandom = 0;
							string strQID = "";
							do
							{
								intRandom = myRandom.Next(0,dsQuestionList.Tables[0].Rows.Count);
								strQID = dsQuestionList.Tables[0].Rows[intRandom]["cQID"].ToString();
							}
							while(checkArray(strQID , arrayQID));

							//將此QID存入Array中
							arrayQID[intArrayIndex] = strQID;
							intArrayIndex += 1;
						}
					}
					else
					{
						//把所有的題目都挑選進Array來
						for(int j=0 ; j<dsQuestionList.Tables[0].Rows.Count ; j++)
						{
							arrayQID[j] = dsQuestionList.Tables[0].Rows[j]["cQID"].ToString();
						}
					}
					dsQuestionList.Dispose();
				}
			}
			dsGroup.Dispose();
			
			return arrayQID;
		}
*/
		public string[] getRandomQIDArray(string strPaperID)
		{
			///亂數取得某一個問題組別中的問題

			//取得此問卷所有需要亂數取得的問題總數
			int intQuestionSum = 0;
			intQuestionSum = myReceiver.getTotalQuestionNumFromRandomQuestion(strPaperID);

			//傳回的陣列
			string[] arrayQID = new String[intQuestionSum];
			int intArrayIndex = 0;//代表arrayQID的指標

			//取得Paper_RandomQuestionNum的資料
			string strSQL = mySQL.getPaper_RandomQuestionNum(strPaperID);
			DataSet dsGroupNum = sqldb.getDataSet(strSQL);
			if(dsGroupNum.Tables[0].Rows.Count > 0)
			{
				for(int i=0 ; i<dsGroupNum.Tables[0].Rows.Count ; i++)
				{
					//get GroupID
					string strGroupID = "";
					try
					{
						strGroupID = dsGroupNum.Tables[0].Rows[i]["cQuestionGroupID"].ToString();
						
					}
					catch
					{
					}

					//get Question number
					int intQuestionNum = 0;
					try
					{
						intQuestionNum = Convert.ToInt32(dsGroupNum.Tables[0].Rows[i]["sQuestionNum"]);
						
					}
					catch
					{
					}

					//取得此問題組別的所有Level 1問題
					if(strGroupID == "Specific")
					{
						strSQL = mySQL.getSpecificSelectionQuestion(strPaperID);
					}
					else
					{
						strSQL = mySQL.getGroupSelectionQuestion(strGroupID);
					}

					DataSet dsQuestionList = sqldb.getDataSet(strSQL);
					if(dsQuestionList.Tables[0].Rows.Count > 0)
					{
						//檢查intQuestionNum是否大於QuestionList中的題目數量
						if(intQuestionNum <= dsQuestionList.Tables[0].Rows.Count)
						{
							//亂數選取問題後存入Array中
							for(int j=0 ; j<intQuestionNum ; j++)
							{
								//亂數取得一個介於0~RowCount之間的數字
								int intRandom = 0;
								string strQID = "";
								do
								{
									intRandom = myRandom.Next(0,dsQuestionList.Tables[0].Rows.Count);
									strQID = dsQuestionList.Tables[0].Rows[intRandom]["cQID"].ToString();
								}
								while(checkArray(strQID , arrayQID));
								//將此QID存入Array中
								arrayQID[intArrayIndex] = strQID;
								intArrayIndex += 1;
							}
						}
						else
						{
							//把所有的題目都挑選進Array來
							for(int j=0 ; j<dsQuestionList.Tables[0].Rows.Count ; j++)
							{
								arrayQID[j] = dsQuestionList.Tables[0].Rows[j]["cQID"].ToString();
							}
						}
					}
					else
					{
						//找不到此問題組別的資料
					}
					dsQuestionList.Dispose();
				}
			}
			else
			{
				//在Paper_RandomQuestion找不到資料
			}
			dsGroupNum.Dispose();

			return arrayQID;
		}

		public string[] getGroupRandomQIDArrayNotSelectLevel1(string strGroupID , int intQuestionNum , string strPaperID)
		{
			//傳回的陣列
			string[] arrayQID = new String[intQuestionNum];
			int intArrayIndex = 0;//代表arrayQID的指標

			//到QuestionIndex找出符合條件的問題
			//string strSQL = "SELECT * FROM QuestionIndex I , QuestionMode M WHERE M.cQuestionGroupID = '"+strGroupID+"' AND I.cQID = M.cQID ";
			string strSQL = mySQL.getGroupQuestionLevel1NotSelect(strGroupID , strPaperID);
			DataSet dsQuestionList = sqldb.getDataSet(strSQL);
			//亂數選取問題後存入Array中
			//檢查intQuestionNum是否大於QuestionList中的題目數量
			if(intQuestionNum <= dsQuestionList.Tables[0].Rows.Count)
			{
				for(int j=0 ; j<intQuestionNum ; j++)
				{
					//亂數取得一個介於0~RowCount之間的數字
					int intRandom = 0;
					string strQID = "";
					do
					{
						intRandom = myRandom.Next(0,dsQuestionList.Tables[0].Rows.Count);
						strQID = dsQuestionList.Tables[0].Rows[intRandom]["cQID"].ToString();
					}
					while(checkArray(strQID , arrayQID));

					//將此QID存入Array中
					arrayQID[intArrayIndex] = strQID;
					intArrayIndex += 1;
				}
			}
			else
			{
				//把所有的題目都挑選進Array來
				for(int j=0 ; j<dsQuestionList.Tables[0].Rows.Count ; j++)
				{
					arrayQID[arrayQID.Length] = dsQuestionList.Tables[0].Rows[j]["cQID"].ToString();
				}
			}
			dsQuestionList.Dispose();

			return arrayQID;
		}

		public string[] getSpecificRandomQIDArrayNotSelectLevel1(int intQuestionNum , string strPaperID)
		{
			//傳回的陣列
			string[] arrayQID = new String[intQuestionNum];
			int intArrayIndex = 0;//代表arrayQID的指標

			//到QuestionIndex找出符合條件的問題
			//string strSQL = "SELECT * FROM QuestionIndex I , QuestionMode M WHERE M.cQuestionGroupID = '"+strGroupID+"' AND I.cQID = M.cQID ";
			string strSQL = mySQL.getSpecificQuestionLevel1NotSelect(strPaperID);
			DataSet dsQuestionList = sqldb.getDataSet(strSQL);
			//亂數選取問題後存入Array中
			//檢查intQuestionNum是否大於QuestionList中的題目數量
			if(intQuestionNum <= dsQuestionList.Tables[0].Rows.Count)
			{
				for(int j=0 ; j<intQuestionNum ; j++)
				{
					//亂數取得一個介於0~RowCount之間的數字
					int intRandom = 0;
					string strQID = "";
					do
					{
						intRandom = myRandom.Next(0,dsQuestionList.Tables[0].Rows.Count);
						strQID = dsQuestionList.Tables[0].Rows[intRandom]["cQID"].ToString();
					}
					while(checkArray(strQID , arrayQID));

					//將此QID存入Array中
					arrayQID[intArrayIndex] = strQID;
					intArrayIndex += 1;
				}
			}
			else
			{
				//把所有的題目都挑選進Array來
				for(int j=0 ; j<dsQuestionList.Tables[0].Rows.Count ; j++)
				{
					arrayQID[arrayQID.Length] = dsQuestionList.Tables[0].Rows[j]["cQID"].ToString();
				}
			}
			dsQuestionList.Dispose();

			return arrayQID;
		}

		private bool checkArray(string strQID , string[] arrayQID)
		{
			//檢查QID是否已經存在於陣列中
			bool bReturn = false;
			for(int i=0 ; i< arrayQID.Length ; i++)
			{
				if(strQID == arrayQID[i])
				{
					bReturn = true;
				}
				
			}
			return bReturn;
		}

	}
}