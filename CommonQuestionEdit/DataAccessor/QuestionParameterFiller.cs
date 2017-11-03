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
	/// 存取資料表所需的參數由此製造
	/// </summary>
	public class QuestionParameterFiller
	{
		//private string QuestionDataTableName;//取得問題ID集合所在的資料表
		//private string QID_Relation_Field;//要與QuestionIndex中的cQID相關連的欄位名稱
		//private string Question_Edit_Type;//Question的編輯型態;有問診的編輯型態:Interrogation_Enquiry,選擇題的編輯型態:Choice_Question,Script問題的編輯型態:Script_Question
		//private Hashtable searchQuestionCondition = null;
		public Hashtable searchConditionTable = new Hashtable();	
		
		public QuestionParameterFiller(string Question_Edit_Type)
		{
//			this.Question_Edit_Type = Question_Edit_Type;
//			setQuestionAccessParameter();
		}

				
	}
}
