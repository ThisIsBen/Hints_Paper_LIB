using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using suro.util;

namespace AuthoringTool.CommonQuestionEdit
{
	/// <summary>
	/// CommonQuestionUtility 的摘要描述。
	/// </summary>
	public class CommonQuestionUtility
	{
		public CommonQuestionUtility()
		{
			
		}

		/// <summary>
		/// 為了利用"LevelandRank"來作字串的排序
		/// 因此需將"LevelandRank"補零例如;假設一個數字1
		/// 則根據參數"returnStringLength"的數字決定要補齊的位數
		/// 假設要補齊5位數的話,則1就變成"00001"
		/// 例如數字32要補齊6位數的話,就變成"000032"
		/// </summary>
		/// <param name="LevelandRank">要的字串</param>
		/// <param name="returnStringLength">要補齊的位數</param>
		/// <returns></returns>
		public static string FillZero(int number,int returnStringLength)
		{
			string strNumber = Convert.ToString(number);
			int originalStringLength = strNumber.Length;
			string ZeroString="";
			for(int i=0;i<returnStringLength-originalStringLength;i++)
			{
				ZeroString+="0";
			}
			return ZeroString + strNumber;
		}

		/// <summary>
		/// 取得一個有rows列數與columns行數的HTML Table
		/// </summary>
		/// <param name="rows">列數</param>
		/// <param name="columns">行數</param>
		/// <returns>取得一個有rows列數與columns行數的HTML Table</returns>
		public static Table get_HTMLTable(int rows,int columns)
		{
			Table tb = new Table();
			for(int i=0;i<rows;i++)
			{
				tb.Rows.Add(new TableRow());
				for(int j=0;j<columns;j++)
				{
					tb.Rows[i].Cells.Add(new TableCell());
					tb.Rows[i].Cells[tb.Rows[i].Cells.Count-1].Text = "&nbsp;";
				}
			}
			return tb;
		}

		/// <summary>
		/// 新增一個問題或選項的ID
		/// </summary>
		/// <param name="p_strUser">編輯者的使用者ID</param>
		/// <param name="ID_Type">要新增的ID是問題ID:Question或選項ID:Selection</param>
		/// <returns></returns>
		public static string GetNewID(string p_strUser,string ID_Type)
		{			
			string strNewID;
			DateTime dtNow = DateTime.Now;
			while(dtNow.AddSeconds(0.1) < DateTime.Now){}
			strNewID = p_strUser +ID_Type+DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
			return(strNewID);
		}

		/// <summary>
		/// 新增一個問題或選項的ID
		/// </summary>
		/// <param name="p_strUser">編輯者的使用者ID</param>
		/// <param name="ID_Type">要新增的ID是問題ID:Question或選項ID:Selection</param>
		/// <returns></returns>
		public static string[] GetNewID(string p_strUser,string ID_Type,int ID_Num)
		{			
			string[] strNewID = new string[ID_Num];
			DateTime dtNow = DateTime.Now;
			while(dtNow.AddSeconds(0.1) < DateTime.Now){}
			for(int i=0;i<ID_Num;i++)
			{
			    strNewID[i] = p_strUser + "_" + ID_Type + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfffffff")+"_"+i.ToString();
			}
			return(strNewID);
		}
		/// <summary>
		/// 取得問題和後續問題或問題選項的最大序號
		/// </summary>
		/// <param name="strQID"></param>
		/// <param name="QuestionSelectionIndex"></param>
		/// <param name="QuestionLinkQID"></param>
		/// <returns></returns>
		public static int GetMaxSequence(string strQID,DataTable QuestionSelectionIndex,DataTable QuestionLinkQID)
		{
			int ret = 0;
			DataRow[] drs = QuestionSelectionIndex.Select("cQID='"+strQID+"'","sSeq DESC");
			if(drs.Length>0)
			{
				ret = Convert.ToInt32(drs[0]["sSeq"]);
			}
			drs = QuestionLinkQID.Select("cParentQID='"+strQID+"'","sSeq DESC");
			if(drs.Length>0)
			{
				if(Convert.ToInt32(drs[0]["sSeq"])>ret)
				{
					ret = Convert.ToInt32(drs[0]["sSeq"]);
				}
			}
			ret++;
			return ret;
		}
	}
}
