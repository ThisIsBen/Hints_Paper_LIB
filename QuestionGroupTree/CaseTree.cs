using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using suro.util;


namespace CaseTreeControl
{
	/// <summary>
	/// CaseTree 的摘要描述。
	/// </summary>
	public class CaseTree:Control
	{
		SqlDB sqldb=new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
		private string Directory_DataTable = "QuestionGroupTree";
		private string UserID = "";
		private bool bIslink = false;
		public string strScript = "";//要呈現到前端的以呈現TreeView的javascript
		public CaseTree(string UserID,bool bIslink)
		{
			this.bIslink = bIslink;
			this.UserID = UserID;
		}

		public void BuildTree()
		{
			outputJavascriptForRoot();
		}
	
		protected void outputJavascriptForRoot()
		{			
			DataTable dt = this.getNodeDataTable("root");
			//strScript += "alert('ff');"+System.Environment.NewLine;
			strScript += "foldersTree = gFld(\"<a href='javascript:noop();' NodeID='root' onblur='BlurNode();' onclick='ClickNodeText();' style='font-size:12px;text-decoration:none;color:Black;font-family:Times New Roman;'>Question Group</a>\");"+System.Environment.NewLine;
			strScript += "foldersTree.NodeID='root';"+System.Environment.NewLine;
			foreach(DataRow dr in dt.Rows)
			{
				outputJavascriptForSubFolder("foldersTree",dr["cNodeID"].ToString(), dr["cNodeName"].ToString(),dr["cNodeType"].ToString());
			}
		}

		/// <summary>
		/// 取得節點在資料庫中的資訊資料表
		/// </summary>
		/// <param name="ParentID">節點的父節點</param>
		/// <returns>傳回資料表</returns>
		private DataTable getNodeDataTable(string ParentID)
		{
			string strSQL = "SELECT * FROM "+Directory_DataTable+" WHERE cParentID='"+ParentID+"' ORDER BY cNodeType desc,cNodeName";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt;
		}

		/// <summary>
		/// 取得節點所要連結的CaseID和DivisionID
		/// </summary>
		/// <param name="UserID">使用者ID</param>
		/// <param name="NodeID">節點ID</param>
		/// <returns>傳回一字串陣列,陣列的第一個元素是DivisionID,第二個元素是CaseID</returns>
		private string[] getCase_Division(string NodeID)
		{
			string[] ret = new string[2];			
			string strSQL = "SELECT cCaseID,cDivisionID FROM "+Directory_DataTable+" WHERE cUserID='"+UserID+"' AND cNodeID='"+NodeID+"'";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			DataRow dr = dt.Rows[0];
			ret[0] = dr["cCaseID"].ToString();
			ret[1] = dr["cDivisionID"].ToString();
			return ret;
		}

		protected void outputJavascriptForSubFolder(string ParentID,string NodeID, string NodeName,string NodeType)
		{
			if(NodeType.Equals("Group"))
			{
				strScript += "tmpObj = gFld(\"<a id='NodeText_"+NodeID+"' onblur='BlurNode()' NodeID='"+NodeID+"' onclick='ClickNodeText();' href='javascript:noop();' style='font-size:12px;text-decoration:none;color:Black;font-family:Times New Roman;'>"+"&nbsp;"+NodeName+"</a>\", 'javascript:parent.op()');"+System.Environment.NewLine;
				strScript += NodeID + " = insFld('"+NodeID+"',"+ParentID+",tmpObj);"+System.Environment.NewLine;
			}
			else if(NodeType.Equals("Division"))
			{
				strScript += "tmpObj = gFld(\"<a id='NodeText_"+NodeID+"' onblur='BlurNode()' NodeID='"+NodeID+"' onclick='ClickNodeText()' href='javascript:noop();' style='font-size:12px;text-decoration:none;color:Black;font-family:Times New Roman;'>"+"&nbsp;"+NodeName+"</a>\", 'javascript:parent.op()');"+System.Environment.NewLine;
				strScript += "tmpObj.iconSrc = ICONPATH + \"DivisionOpen.gif\";"+System.Environment.NewLine;
				strScript += "tmpObj.iconSrcClosed = ICONPATH + \"Division.gif\";"+System.Environment.NewLine;
				strScript += NodeID + " = insFld('"+NodeID+"',"+ParentID+",tmpObj);"+System.Environment.NewLine;
			}
//			else if(NodeType.Equals("Group"))
//			{
//				//string url = "caseSelect.asp?ID="+caseID+"&Name="+NodeName+"&URL=localhost&Did="+DivisionID;
//				//string frame = "CONTROL";
//				//string linkFunction = "hrefFunction(\'"+url+"\',\'"+frame+"\');";
//				string linkFunction = "hrefFunction();";
//				if(!this.bIslink)
//				{
//					linkFunction = "noop();";
//				}
//				//string[] CaseID_DivisionID = getCase_Division(NodeID);
//				string GroupID = NodeID;
//				strScript += " tmpLnk = gLnk(\"Rh\",\"<a onclick='"+linkFunction+"' href='javascript:noop();' ID='"+GroupID+"' Name='"+NodeName+"' style='font-size:12px;text-decoration:none;color:Black;font-family:Times New Roman;'>"+NodeName+"</a>\",\"javascript:LinkCase()\");"+System.Environment.NewLine;
//				strScript += NodeID + " = insDoc('"+NodeID+"',"+ParentID+",tmpLnk);"+System.Environment.NewLine;
//				//strScript += NodeID + " = insDoc("+ParentID+", gLnk('Rh','"+NodeName+"',\'javascript:LinkCase()\'));"+System.Environment.NewLine;
//				strScript += NodeID + ".iconSrc = ICONPATH + \"page.gif\";"+System.Environment.NewLine;				
//			}
            DataTable dt = this.getNodeDataTable(NodeID);			
			foreach (DataRow dr in dt.Rows)
			{
				outputJavascriptForSubFolder(NodeID,dr["cNodeID"].ToString(), dr["cNodeName"].ToString(),dr["cNodeType"].ToString());
			}
		}

		private string folder_NodeTextStyle
		{
			get
			{
				string NodeTextStyle = "style=\'font-size:12px;text-decoration:none;color:Black;font-family:Times New Roman;\'";
				return NodeTextStyle;
			}
		}
	}
}
