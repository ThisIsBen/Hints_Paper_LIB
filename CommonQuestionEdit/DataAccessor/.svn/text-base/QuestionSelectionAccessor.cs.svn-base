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
	/// QuestionSelectionAccessor 的摘要描述。
	/// </summary>
	public class QuestionSelectionAccessor
	{
		SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
		public string Question_Edit_Type;//Question的編輯型態;有問診的編輯型態:Interrogation_Enquiry,選擇題的編輯型態:Choice_Question,Script問題的編輯型態:Script_Question
		public DataTable QuestionSelectionIndex = null;
		private QuestionAccessor qAccessor = null;        

        public QuestionSelectionAccessor(QuestionAccessor qAccessor, string Question_Edit_Type)
		{
			this.qAccessor = qAccessor;
			this.Question_Edit_Type = Question_Edit_Type;
			setQuestionDatatable();
			qAccessor.qsAccessor = this;			
		}

		public DataTable getQuestionSelections(string QID)
		{
			string strSQL = "SELECT cSelectionID,cSelection,bCaseSelect,cResponse FROM QuestionSelectionIndex WHERE cQID='"+QID+"' ORDER BY sSeq";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt;
		}

		/// <summary>
		/// 取得問題選項的資料表
		/// </summary>
		/// <param name="qAccessor">QuestionAccessor物件：擷取Question的問題</param>
		/// <returns>取得問題選項的資料表</returns>
		public DataTable get_QuestionSelectionIndex_Datatable()
		{
			string strSQL = "(SELECT * FROM QuestionSelectionIndex WHERE cQID IN ("+qAccessor.QID_SQL_String+"))";
			if(this.Question_Edit_Type=="Choice_Question")
			{   // modified by dolphin @ 2006-08-21, fix the casting statement for text, add the varchar length
                strSQL = "(SELECT DISTINCT cQID,cQID + '_'+ CAST(sSeq AS VARCHAR(8000)) AS cSelectionID,sSeq,CAST(cAnswer AS VARCHAR(8000)) AS cSelection,bRightOrFalse AS bCaseSelect,CAST(cResponse AS VARCHAR(8000)) AS cResponse FROM ItemForAnswer WHERE cQID IN (" + qAccessor.QID_SQL_String + ") AND cCaseID='" + this.qAccessor.page.Session["CaseID"].ToString() + "')";
			}
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt;
		}

		/// <summary>
		/// 設定問題以及選項的DataTable
		/// </summary>
		private void setQuestionDatatable()
		{
			//取得問題以及選項的DataTable
			QuestionSelectionIndex = get_QuestionSelectionIndex_Datatable();
		}
        
		/// <summary>
		/// 刪除屬於Question ID為"QID"的問題選項資料列
		/// </summary>
		/// <param name="QID">選項所屬的問題</param>
		public void deleteSelectionDataRow(string QID)
		{
			DataRow[] drs = this.QuestionSelectionIndex.Select("cQID='"+QID+"'");
			foreach(DataRow dr in drs)
			{
				this.qAccessor.deleteQuestionInDataTable(QID,dr["cSelectionID"].ToString());
				this.QuestionSelectionIndex.Rows.Remove(dr);				
			}
		}
        
		/// <summary>
		/// 刪除選項ID為del_SelectionID的選項及此選項的後續問題
		/// </summary>
		/// <param name="del_SelectionID">欲刪除的選項的ID</param>
		public void deleteSelectionDataRow(string QID,string del_SelectionID)
		{
			DataRow[] drs = this.QuestionSelectionIndex.Select("cQID='"+QID+"' AND cSelectionID='"+del_SelectionID+"'");
			foreach(DataRow dr in drs)
			{
				//刪除選項的後續問題
				this.qAccessor.deleteQuestionInDataTable(QID,dr["cSelectionID"].ToString());
				this.QuestionSelectionIndex.Rows.Remove(dr);	
			}
		}

		/// <summary>
		/// 若使用者有新增選項,會將新增的選項暫存在"new_Selection_DataRow"資料列中,等到下次網頁PostBack時在處理
		/// </summary>
		/// <param name="QID">問題ID</param>
		/// <param name="new_selectionID">新增的選項ID</param>
		public void add_new_selection(string QID,string new_SelectionID)
		{
			DataRow new_Selection_DataRow = this.QuestionSelectionIndex.NewRow(); 
			new_Selection_DataRow["cQID"] = QID;
			new_Selection_DataRow["cSelectionID"] = new_SelectionID;
			new_Selection_DataRow["sSeq"] = CommonQuestionUtility.GetMaxSequence(QID,QuestionSelectionIndex,qAccessor.QuestionLinkQID);
			new_Selection_DataRow["cSelection"] = "";
			new_Selection_DataRow["bCaseSelect"] = false;
			QuestionSelectionIndex.Rows.Add(new_Selection_DataRow);
		}

		/// <summary>
		/// 修改某個選項的內容
		/// </summary>
		/// <param name="QID">選項所屬問題的ID</param>
		/// <param name="SelectionID">選項ID</param>
		/// <param name="SelectionText">選項內容</param>
		public void modifySelectionResponseText(string QID,string SelectionID,string SelectionResponseText)
		{
			DataRow[] drs = this.QuestionSelectionIndex.Select("cQID='"+QID+"' AND cSelectionID='"+SelectionID+"'");
			drs[0]["cResponse"] = SelectionResponseText;
		}

		/// <summary>
		/// 修改某個選項的內容
		/// </summary>
		/// <param name="QID">選項所屬問題的ID</param>
		/// <param name="SelectionID">選項ID</param>
		/// <param name="SelectionText">選項內容</param>
		public void modifySelectionText(string QID,string SelectionID,string SelectionText)
		{
			DataRow[] drs = this.QuestionSelectionIndex.Select("cQID='"+QID+"' AND cSelectionID='"+SelectionID+"'");
			drs[0]["cSelection"] = SelectionText;
		}

		/// <summary>
		/// 修改選項是否為建議或非建議選項
		/// </summary>
		/// <param name="QID">問題ID</param>
		/// <param name="SelectionID">選項ID</param>
		/// <param name="bIsCorrect">是否為建議選項</param>
		public void modifySelectionCorrect(string QID,string SelectionID,bool bIsCorrect)
		{
			DataRow[] drs = this.QuestionSelectionIndex.Select("cQID='"+QID+"' AND cSelectionID='"+SelectionID+"'");
			drs[0]["bCaseSelect"] = bIsCorrect;
		}

		/// <summary>
		/// 將QuestionSelectionIndex更新回資料庫
		/// </summary>
		private void update_QuestionSelectionIndex_IntoDatabase()
		{
			string delete_SQL = "DELETE QuestionSelectionIndex WHERE cQID IN ("+qAccessor.QID_SQL_String+")";
			sqldb.ExecuteNonQuery(delete_SQL);

			string strSQL = "SELECT * FROM QuestionSelectionIndex WHERE 1=0";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			DataRow new_Row = null;
			foreach(DataRow dr in QuestionSelectionIndex.Rows)
			{
				new_Row = dt.NewRow();
				for(int i=0;i<new_Row.ItemArray.Length;i++)
				{
					new_Row[i] = dr.ItemArray[i];
				}
				dt.Rows.Add(new_Row);
			}

			sqldb.Update(dt,"SELECT * FROM QuestionSelectionIndex");
		}

		/// <summary>
		/// 將QuestionSelectionIndex更新回ItemForAnswer
		/// </summary>
		private void update_ItemForAnswer_IntoDatabase()
		{
			string delete_SQL = "DELETE ItemForAnswer WHERE cQID IN ("+qAccessor.QID_SQL_String+")";
			sqldb.ExecuteNonQuery(delete_SQL);

			string strSQL = "SELECT * FROM ItemForAnswer WHERE 1=0";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			DataRow new_Row = null;
			foreach(DataRow dr in QuestionSelectionIndex.Rows)
			{
				new_Row = dt.NewRow();
				
				new_Row["cCaseID"] = this.qAccessor.searchQuestionCondition["cCaseID"].ToString();
				new_Row["cQID"] = dr["cQID"].ToString();
				new_Row["sSeq"] = dr["sSeq"].ToString();
				new_Row["cAnswer"] = dr["cSelection"].ToString();
				new_Row["bRightOrFalse"] = dr["bCaseSelect"].ToString();
				new_Row["cResponse"] = dr["cResponse"].ToString();
				new_Row["sSelectScore"] = 0;
				new_Row["sUnSelectScore"] = 0;

				dt.Rows.Add(new_Row);
			}

			sqldb.Update(dt,"SELECT * FROM ItemForAnswer");
		}

		public string GetQIDBySelectionID(string strSelectionID)
		{			
			DataRow[] drs = this.QuestionSelectionIndex.Select("cSelectionID='"+strSelectionID+"'");
			return drs[0]["cQID"].ToString();
		}

		/// <summary>
		/// 將QuestionSelectionIndex DataTable更新回資料庫
		/// </summary>
		public void update_DataTableIntoDatabase()
		{
			if(this.Question_Edit_Type=="Choice_Question")
			{
				update_ItemForAnswer_IntoDatabase();
			}
			else
			{
				update_QuestionSelectionIndex_IntoDatabase();
			}
		}

		public void addScriptCategoryIDInQuestionIndex(string oldID,string ScriptCategoryID)
		{
			DataRow[] drs = this.QuestionSelectionIndex.Select("cQID='"+oldID+"'");
			foreach(DataRow dr in drs)
			{
				dr["cQID"] = ScriptCategoryID;
			}
		}		
	}
}
