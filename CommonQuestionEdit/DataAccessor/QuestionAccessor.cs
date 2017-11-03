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
using AuthoringTool.QuestionEditLevel;

namespace AuthoringTool.CommonQuestionEdit
{
	/// <summary>
	/// QuestionAccess ���K�n�y�z�C
	/// </summary>
	public class QuestionAccessor
	{
		public Page page = null;             //�ϥΦ����󪺺���		
		private SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
		private string QuestionDataTableName;//���o���DID���X�Ҧb����ƪ�
		private string QuestionRelationDataTableName;//���o���l���D���Y����ƪ�
		private string Sub_Qid_WHERE_Field;//�l���D��쪺�j�M����
		private string Sub_Qid_Field;//�l���D���		
		private string QID_Relation_Field;//�n�PQuestionIndex����cQID�����s�����W��
		private string Question_Field;    //�n�PQuestionIndex����cQID�����s�����W�٪����D���e
		public string Question_Edit_Type;//Question���s�諬�A;���ݶE���s�諬�A:Interrogation_Enquiry,����D���s�諬�A:Choice_Question,Script���D���s�諬�A:Script_Question
		public Hashtable searchQuestionCondition = null;//SQL�y�k�������D�^������
		private string strQID_SQL_String = "";//���o���DID���X��ƪ�SQL�y�k,�]�N�O�Q�Φ��y�k�i�H�^����Section���Ҧ����DID���X
		public QuestionSelectionAccessor qsAccessor = null;//���D�ﶵ�s������
		public DataTable QuestionIndex = null;
		public DataTable QuestionLinkQID = null;
		public DataTable SelectionLinkQID = null;
		public DataTable VocabularyCategoryIndex = null;

        public QuestionAccessor(string Question_Edit_Type, Hashtable searchQuestionCondition, Page page)
		{
			this.page = page;
			//Question���s�諬�A;���ݶE���s�諬�A:Interrogation_Enquiry,����D���s�諬�A:Choice_Question,Script���D���s�諬�A:Script_Question
			this.Question_Edit_Type = Question_Edit_Type;
			this.searchQuestionCondition = searchQuestionCondition;
			//�ܸ�ƪ���o���DID���X�һݪ���L�Ѽ�,��QuestionDataTableName,QID_Relation_Field,searchQuestionCondition
			setQuestionAccessParameter();
			//�]�w���D�H�οﶵ��DataTable
			setQuestionDatatable();
		}	
	
		/// <summary>
		/// ���oQID�U�����h���Ҧ��l���DID
		/// </summary>
		/// <param name="QID"></param>
		/// <returns></returns>
		public static ArrayList getSubQIDArray(string QID)
		{
			ArrayList ret = new ArrayList();
			ArrayList subQIDArrayList = null;
			string strSQL = "SELECT cLinkedQID FROM SelectionLinkQID WHERE cQID='"+QID+"'";
			SqlDB tmp_sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
			DataTable dt  = tmp_sqldb.getDataSet(strSQL).Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				subQIDArrayList = getSubQIDArray(dr["cLinkedQID"].ToString());
				for(int i=0;i<subQIDArrayList.Count;i++)
				{
					ret.Add(subQIDArrayList[i].ToString());
				}
			}
			return ret;
		}

		/// <summary>
		/// ���o��Ʈw��������Section���Ҧ����D
		/// </summary>
		/// <returns>�N���o�쪺��ƥHDataTable������Ǧ^</returns>
		public DataTable get_QuestionIndex_Datatable()
		{
//			string strSQL_Left = "("+QID_SQL_String+")A";
//			string strSQL_Right = "(SELECT * FROM QuestionIndex)B";
//			string strSQL = "SELECT B.* FROM " + strSQL_Left +" LEFT JOIN " + strSQL_Right + " ON A."+this.QID_Relation_Field+"=B.cQID ORDER BY B.sLevel";
//			DataTable ret_dt = sqldb.getDataSet(strSQL).Tables[0];
			string strSQL = "";
			DataTable ret_dt = null;
			if(this.Question_Edit_Type!="Choice_Question")
			{

				strSQL = "SELECT * FROM QuestionIndex WHERE cQID IN ("+QID_SQL_String+")";
				ret_dt = sqldb.getDataSet(strSQL).Tables[0];
			}
			else
			{
				strSQL = "(SELECT "+this.QID_Relation_Field+" AS cQID,"+Question_Field+" AS cQuestion,'1' AS sLevel FROM " + QuestionDataTableName + getQIDWhereString() + " AND "+QID_Relation_Field+" NOT IN (SELECT DISTINCT "+Sub_Qid_Field+" FROM "+this.QuestionRelationDataTableName+"))";
				ret_dt = sqldb.getDataSet(strSQL).Tables[0];
				strSQL = "(SELECT "+this.QID_Relation_Field+" AS cQID,"+Question_Field+" AS cQuestion,'2' AS sLevel FROM " + QuestionDataTableName + getQIDWhereString() + " AND "+QID_Relation_Field+" IN (SELECT DISTINCT "+Sub_Qid_Field+" FROM "+this.QuestionRelationDataTableName+"))";				
				DataTable tmpDT = sqldb.getDataSet(strSQL).Tables[0];
				foreach(DataRow dr in tmpDT.Rows)
				{
					ret_dt.ImportRow(dr);
				}
			}
			
			return ret_dt;
		}		

		/// <summary>
		/// ���o��Ʈw��������Section���Ĥ@���h���D
		/// </summary>
		/// <returns>�N���o�쪺��ƥHDataTable������Ǧ^</returns>
		public DataTable get_StartQuestionIdex_Datatable()
		{
//			string strSQL_Left = "("+QID_SQL_String+")A";
//			string strSQL_Right = "(SELECT * FROM QuestionIndex)B";
//			string strSQL = "SELECT B.* FROM " + strSQL_Left +" LEFT JOIN " + strSQL_Right + " ON A."+this.QID_Relation_Field+"=B.cQID WHERE B.sLevel=1";
			string strSQL = "SELECT * FROM QuestionIndex WHERE cQID IN ("+QID_SQL_String+") AND sLevel=1";
			if(this.Question_Edit_Type!="Choice_Question")
			{
				strSQL = "(SELECT "+this.QID_Relation_Field+" AS cQID,CAST("+Question_Field+" AS VARCHAR) AS cQuestion,'1' AS sLevel FROM " + QuestionDataTableName + getQIDWhereString() + " AND "+QID_Relation_Field+" NOT IN (SELECT DISTINCT "+Sub_Qid_Field+" FROM "+this.QuestionRelationDataTableName+"))";
			}
			DataTable ret_dt = sqldb.getDataSet(strSQL).Tables[0];
			return ret_dt;
		}

		/// <summary>
		/// ���o�����D�P������D���������Y��ƪ�
		/// </summary>
		/// <returns>���o�����D�P������D���������Y��ƪ�</returns>
		public DataTable get_QuestionLinkQID_Datatable()
		{
			string strSQL = "SELECT * FROM QuestionLinkQID WHERE cParentQID IN ("+QID_SQL_String+")";
			if(this.Question_Edit_Type=="Choice_Question")
			{   // modified by dolphin @ 2006-08-02, change the sSeq to 10
				strSQL = "SELECT cParentQID AS cParentQID,cChildQID AS cSubQID,'10' AS sSeq FROM SubQuestionMap WHERE cParentQID IN ("+QID_SQL_String+")";
			}
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt;
		}

		/// <summary>
		/// ���o�ﶵ�P������D���������Y��ƪ�
		/// </summary>
		/// <returns>���o�ﶵ�P������D���������Y��ƪ�</returns>
		public DataTable get_SelectionLinkQID_Datatable()
		{
//			string strQID = "("+QID_SQL_String+")";
//			string strSQL_LEFT = "(SELECT * FROM SelectionLinkQID WHERE cQID IN ("+strQID+"))A";
//			string strSQL_Right = "(SELECT * FROM QuestionIndex WHERE cQID IN ("+strQID+"))B";
//			string strSQL = "SELECT A.* FROM "+strSQL_LEFT+" LEFT JOIN "+strSQL_Right+" ON A.cLinkedQID=B.cQID";
			string strSQL = "SELECT * FROM SelectionLinkQID WHERE cQID IN ("+QID_SQL_String+")";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt;
		}

		/// <summary>
		/// ���o���D�һݪ�SQL�y�k��WHERE����r��
		/// </summary>
		/// <returns>�Ǧ^���D�һݪ�SQL�y�k��WHERE����r��</returns>
		private string getQIDWhereString()
		{
			string ret = " WHERE";
			string FieldName = "";//�^���ŦX������D�����W��
			string FieldConditionValue = "";//�^���ŦX������D������			
			ArrayList FieldIndex = (ArrayList)searchQuestionCondition["FieldIndex"];
			for(int i=0;i<FieldIndex.Count;i++)
			{
				FieldName = FieldIndex[i].ToString();
				FieldConditionValue = searchQuestionCondition[FieldName].ToString();
				ret += " "+FieldName+"='"+FieldConditionValue+"' AND ";
			}
			ret = ret.Remove(ret.LastIndexOf("AND"),4);//�N�̫�@��"AND"�r�겾��
		    return ret;
		}

 
		/// <summary>
		/// ���o���DID���X��ƪ�SQL�y�k,�]�N�O�Q�Φ��y�k�i�H�^����Section���Ҧ����DID���X
		/// </summary>
		public string QID_SQL_String
		{
			get
			{
				if(!strQID_SQL_String.Equals(""))
				{
					return strQID_SQL_String;
				}
				else
				{
					strQID_SQL_String = "(SELECT "+this.QID_Relation_Field+" AS cQID FROM " + QuestionDataTableName + getQIDWhereString() + ")";
					if(Question_Edit_Type=="Choice_Question")
					{
						strQID_SQL_String = "SELECT "+this.QID_Relation_Field+" AS cQID FROM " + QuestionDataTableName + getQIDWhereString()+"";
						//strQID_SQL_String = "SELECT cQID FROM ("+strSQL+")F ";
					}
					return strQID_SQL_String;
				}				
			}
		}

		/// <summary>
		/// �ܸ�ƪ���o���DID���X�һݪ���L�Ѽ�,��QuestionDataTableName,QID_Relation_Field,searchQuestionCondition
		/// </summary>
		private void setQuestionAccessParameter()
		{
			switch(Question_Edit_Type)
			{				
				case "Interrogation_Enquiry":
					QuestionDataTableName = "SectionQuestionIndex";
					QID_Relation_Field = "cQID";
					break;	
				case "Choice_Question":
					QuestionDataTableName = "ItemForQuestion";
					QuestionRelationDataTableName = "SubQuestionMap";
					Sub_Qid_WHERE_Field = "cQID";
					Sub_Qid_Field = "cChildQID";
					QID_Relation_Field = "cItem";
					Question_Field = "cQuestion";
					break;
				case "Group_Question":
					QuestionDataTableName = "QuestionMode";
					QID_Relation_Field = "cQID";					
					break;
				case "Script_Question":
					
					QuestionDataTableName = "VocabularyCategoryIndex";
					QID_Relation_Field = "cUsedUnitScriptID";
					break;
			}
		}

		/// <summary>
		/// �]�w���D�H�οﶵ��DataTable
		/// </summary>
		private void setQuestionDatatable()
		{
			//���o���D�H�οﶵ��DataTable
			QuestionIndex = get_QuestionIndex_Datatable();
			QuestionLinkQID = get_QuestionLinkQID_Datatable();
			SelectionLinkQID = get_SelectionLinkQID_Datatable();
            if(this.Question_Edit_Type == "Script_Question")
			{
				VocabularyCategoryIndex = this.get_VocabularyCategoryIndex_Datatable();
			}			
		}

		/// <summary>
		/// get ScriptCategoryIndex Datatable
		/// </summary>
		/// <returns></returns>
		private DataTable get_VocabularyCategoryIndex_Datatable()
		{
			string strSQL = "SELECT *,'N' AS IsSelect FROM VocabularyCategoryIndex";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt;
		}

		/// <summary>																																				
		/// �R��QuestionIndex,SelectionLinkQID,QuestionSelectionIndex��Datatable�������D���,�ós�a�N�����D�������ﶵ�H�Ϋ�����D��ƱqDatatable���R��							
		/// </summary>																																					
		/// <param name="QID">���DID</param>																																	
		public void deleteQuestionInDataTable(string QID)																														
		{																																										
			DataRow[] drs = this.QuestionIndex.Select("cQID='"+QID+"'");																											
			foreach(DataRow dr in drs)																																
			{																																												
				QuestionIndex.Rows.Remove(dr);																																		
				//�R�������D���ﶵ																																			
				qsAccessor.deleteSelectionDataRow(QID);																														
			}																																						
		}                                                                                                                                                      

		/// <summary>
		/// �R���Y�ӿﶵ������l���D
		/// </summary>
		/// <param name="QID">���DID</param>
		/// <param name="SelectionID">�ﶵID</param>
		public void deleteQuestionInDataTable(string QID,string SelectionID)
		{
			DataRow[] drs = this.SelectionLinkQID.Select("cQID='"+QID+"' AND cSelectionID='"+SelectionID+"'");
			foreach(DataRow dr in drs)
			{			
				deleteQuestionInDataTable(dr["cLinkedQID"].ToString());
				this.SelectionLinkQID.Rows.Remove(dr);				
			}
		}

		/// <summary>
		/// �R���Y�ӿﶵ�P�l���D���������Y
		/// </summary>
		/// <param name="SelectionID">���D���ݿﶵID</param>
		/// <param name="sub_QID">�l���DID</param>
		public void deleteSelection_Question_RelationInDataTable(string parent_SelectionID,string sub_QID)
		{
			DataRow[] drs = this.SelectionLinkQID.Select("cSelectionID='"+parent_SelectionID+"' AND cLinkedQID='"+sub_QID+"'");
			foreach(DataRow dr in drs)
			{
				this.SelectionLinkQID.Rows.Remove(dr);				
			}
		}

		/// <summary>
		/// ���o���D�����e
		/// </summary>
		/// <param name="QID">���DID</param>
		/// <returns>���D�����e</returns>
		public string getQuestionText(string QID)
		{
			DataRow[] drs = this.QuestionIndex.Select("cQID='"+QID+"'");
			string ret = drs[0]["cQuestion"].ToString();
			return ret;
		}

		/// <summary>
		///  �ק�Y�Ӱ��D�����e
		/// </summary>
		/// <param name="QID">���DID</param>
		/// <param name="QuestionText">���D���e</param>
		public void modifyQuestionText(string QID,string QuestionText)
		{
			DataRow[] drs = this.QuestionIndex.Select("cQID='"+QID+"'");
			drs[0]["cQuestion"] = QuestionText;
		}

        /// <summary>
        ///  �ק�Y�Ӱ��D������r���e
        /// </summary>
        /// <param name="QID">���DID</param>
        /// <param name="KeyWordsText">����r���e</param>
        public void modifyKeyWordsText(string QID, string KeyWordsText)
        {
            DataRow[] drs = this.QuestionIndex.Select("cQID='" + QID + "'");
            drs[0]["cKeyWords"] = KeyWordsText;
        }

		/// <summary>
		/// �s�W�ﶵID��SelectionID���ﶵ��������D
		/// </summary>
		/// <param name="QID">���DID</param>
		/// <param name="SelectionID">�ҭn�s�W������D���ﶵ��ID</param>
		/// <param name="new_Sub_QID">�n�s�W������D��ID</param>
		public void add_New_Sub_Question(string QID,string SelectionID,string new_Sub_QID,int intLevel)
		{
			//�s�W��ƨ�QuestionIndex
			DataRow new_Sub_QID_DataRow = this.QuestionIndex.NewRow(); 
			new_Sub_QID_DataRow["cQID"] = new_Sub_QID;
			new_Sub_QID_DataRow["cQuestion"] = "";
			new_Sub_QID_DataRow["sLevel"] = intLevel;
			QuestionIndex.Rows.Add(new_Sub_QID_DataRow);
			//�s�W��ƨ�SelectionLinkQID
			new_Sub_QID_DataRow = this.SelectionLinkQID.NewRow(); 
			new_Sub_QID_DataRow["cQID"] = QID;
			new_Sub_QID_DataRow["cSelectionID"] = SelectionID;
			new_Sub_QID_DataRow["cLinkedQID"] = new_Sub_QID;//cQuestion
			SelectionLinkQID.Rows.Add(new_Sub_QID_DataRow);
		}

		/// <summary>
		/// �s�W�����D�᭱��������D
		/// </summary>
		/// <param name="QID">���DID</param>
		/// <param name="new_Sub_QID">�n�s�W������D��ID</param>
		public void add_New_Sub_Question(string QID,string new_Sub_QID,int intLevel)
		{
			//�s�W��ƨ�QuestionIndex
			DataRow new_Sub_QID_DataRow = this.QuestionIndex.NewRow(); 
			new_Sub_QID_DataRow["cQID"] = new_Sub_QID;
			new_Sub_QID_DataRow["cQuestion"] = "";
			new_Sub_QID_DataRow["sLevel"] = intLevel;
			QuestionIndex.Rows.Add(new_Sub_QID_DataRow);
            //�s�W��ƨ�QuestionLinkQID
			new_Sub_QID_DataRow = this.QuestionLinkQID.NewRow(); 
			new_Sub_QID_DataRow["cParentQID"] = QID;
			new_Sub_QID_DataRow["cSubQID"] = new_Sub_QID;
			new_Sub_QID_DataRow["sSeq"] = CommonQuestionUtility.GetMaxSequence(QID,qsAccessor.QuestionSelectionIndex,QuestionLinkQID);
			QuestionLinkQID.Rows.Add(new_Sub_QID_DataRow);
		}

        /// <summary>
        /// �s�W�����D�᭱��������D
        /// </summary>
        /// <param name="QID">���DID</param>
        /// <param name="new_Sub_QID">�n�s�W������D��ID</param>
        public void add_New_Sub_Question(string QID, string new_Sub_QID, int intLevel, string question_type, string case_id)
        {
            //�s�W��ƨ�QuestionIndex
            DataRow new_Sub_QID_DataRow = this.QuestionIndex.NewRow();
            new_Sub_QID_DataRow["cQID"] = new_Sub_QID;
            new_Sub_QID_DataRow["cQuestion"] = "";
            new_Sub_QID_DataRow["sLevel"] = intLevel;
            QuestionIndex.Rows.Add(new_Sub_QID_DataRow);
            //�s�W��ƨ�QuestionLinkQID
            new_Sub_QID_DataRow = this.QuestionLinkQID.NewRow();
            new_Sub_QID_DataRow["cParentQID"] = QID;
            new_Sub_QID_DataRow["cSubQID"] = new_Sub_QID;
            new_Sub_QID_DataRow["sSeq"] = CommonQuestionUtility.GetMaxSequence(QID, qsAccessor.QuestionSelectionIndex, QuestionLinkQID);
            QuestionLinkQID.Rows.Add(new_Sub_QID_DataRow);
            //�s�W��ƨ�SubQuestionMap
            try { 
                string strSQL = "INSERT INTO SubQuestionMap VALUES('"+case_id+"','"+QID+"','"+new_Sub_QID+"')";
                sqldb.ExecuteNonQuery(strSQL);
            }
            catch { }
        }

		/// <summary>
		/// �s�W�_�l���D
		/// </summary>
		/// <param name="QID">���DID</param>
		public void add_New_Question(string QID)
		{
			//�s�W��ƨ�QuestionIndex
			DataRow new_Sub_QID_DataRow = this.QuestionIndex.NewRow(); 
			new_Sub_QID_DataRow["cQID"] = QID;
			new_Sub_QID_DataRow["cQuestion"] = "";
			new_Sub_QID_DataRow["sLevel"] = 1;
			QuestionIndex.Rows.Add(new_Sub_QID_DataRow);
		}

		/// <summary>
		/// �N���D�ӷ�����ƪ��s
		/// </summary>
		public void update_QuestionSource_In_Database()
		{
			string delete_SQL = "DELETE " + QuestionDataTableName + getQIDWhereString();
			sqldb.ExecuteNonQuery(delete_SQL);
			string strSQL = "SELECT * FROM " + this.QuestionDataTableName + " WHERE 1=0";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			ArrayList FieldIndex = (ArrayList)searchQuestionCondition["FieldIndex"];
			string field = "";
			DataRow new_Dr = null;
			foreach(DataRow dr in QuestionIndex.Rows)
			{
				new_Dr = dt.NewRow();
				for(int i=0;i<FieldIndex.Count;i++)
				{
					field = FieldIndex[i].ToString();//���W��
					new_Dr[field] = searchQuestionCondition[field].ToString();
				}
				new_Dr[QID_Relation_Field] = dr["cQID"].ToString();
				dt.Rows.Add(new_Dr);
			}
			sqldb.Update(dt,"SELECT * FROM " + this.QuestionDataTableName);
		}
	
		public string getScriptQuestion(string scriptCategoryID)
		{
			string strSQL = "SELECT * FROM VocabularyCategoryIndex WHERE cUsedUnitScriptID='"+scriptCategoryID+"'";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt.Rows[0]["cInterrogativeSentence"].ToString();
		}

		public string getScriptValue(string scriptCategoryID)
		{
			string strSQL = "SELECT * FROM VocabularyCategoryIndex WHERE cUsedUnitScriptID='"+scriptCategoryID+"'";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			return dt.Rows[0]["cVocabularyCategory"].ToString();
		}

		/// <summary>
		/// �NQuestionIndex��Datatable��s�^��Ʈw
		/// </summary>
		private void update_QuestionIndex_In_Database()
		{
			string delete_SQL = "DELETE QuestionIndex WHERE cQID IN ("+this.QID_SQL_String+")";
			sqldb.ExecuteNonQuery(delete_SQL);
			string strSQL = "SELECT * FROM QuestionIndex WHERE 1=0";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			DataRow new_Row = null;
			foreach(DataRow dr in QuestionIndex.Rows)
			{
				new_Row = dt.NewRow();
				for(int i=0;i<new_Row.ItemArray.Length;i++)
				{
					new_Row[i] = dr.ItemArray[i];
				}
				dt.Rows.Add(new_Row);
			}
			sqldb.Update(dt,"SELECT * FROM QuestionIndex");
		}

		private void update_ItemForQuestion_In_Database()
		{
			string delete_SQL = "DELETE ItemForQuestion WHERE cItem IN ("+this.QID_SQL_String+")";
			sqldb.ExecuteNonQuery(delete_SQL);
			string strSQL = "SELECT * FROM ItemForQuestion WHERE 1=0";

			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			DataRow new_Row = null;
			foreach(DataRow dr in QuestionIndex.Rows)
			{
				new_Row = dt.NewRow();
				new_Row["cCaseID"] = searchQuestionCondition["cCaseID"].ToString();
				new_Row["cItem"] = dr["cQID"].ToString();
				new_Row["sClinicNum"] = searchQuestionCondition["sClinicNum"].ToString();
				new_Row["cSectionName"] = searchQuestionCondition["cSectionName"].ToString();
				new_Row["cQuestion"] = dr["cQuestion"].ToString();
				dt.Rows.Add(new_Row);
			}
			sqldb.Update(dt,"SELECT * FROM ItemForQuestion");			
		}

		/// <summary>
		/// �NQuestionLinkQID��Datatable��s�^��Ʈw
		/// </summary>
		private void update_QuestionLinkQID_In_Database()
		{
			string delete_SQL = "DELETE QuestionLinkQID WHERE cParentQID IN ("+this.QID_SQL_String+")";
			sqldb.ExecuteNonQuery(delete_SQL);
			string strSQL = "SELECT * FROM QuestionLinkQID WHERE 1=0";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			DataRow new_Row = null;
			foreach(DataRow dr in QuestionLinkQID.Rows)
			{
				new_Row = dt.NewRow();
				for(int i=0;i<new_Row.ItemArray.Length;i++)
				{
					new_Row[i] = dr.ItemArray[i];
				}
				dt.Rows.Add(new_Row);
			}
			sqldb.Update(dt,"SELECT * FROM QuestionLinkQID");
		}

		/// <summary>
		/// �NQuestionLinkQID��Datatable��s�^SubQuestionMap��ƪ�
		/// </summary>
        private void update_SubQuestionMap_In_Database()
        {
            string delete_SQL = "DELETE SubQuestionMap WHERE cParentQID IN (" + this.QID_SQL_String + ")";
            sqldb.ExecuteNonQuery(delete_SQL);
            string strSQL = "SELECT * FROM SubQuestionMap WHERE 1=0";
            DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
            DataRow new_Row = null;
            foreach (DataRow dr in QuestionLinkQID.Rows)
            {
                new_Row = dt.NewRow();
                new_Row["cCaseID"] = searchQuestionCondition["cCaseID"].ToString();
                new_Row["cParentQID"] = dr["cParentQID"].ToString();
                new_Row["cChildQID"] = dr["cSubQID"].ToString();
                dt.Rows.Add(new_Row);
            }
            sqldb.Update(dt, "SELECT * FROM SubQuestionMap");
        }

		/// <summary>
		/// �NSelectionLinkQID��Datatable��s�^��Ʈw
		/// </summary>
		private void update_SelectionLinkQID_In_Database()
		{
			string delete_SQL = "DELETE SelectionLinkQID WHERE cQID IN ("+this.QID_SQL_String+")";
			sqldb.ExecuteNonQuery(delete_SQL);
			string strSQL = "SELECT * FROM SelectionLinkQID WHERE 1=0";
			DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
			DataRow new_Row = null;
			foreach(DataRow dr in SelectionLinkQID.Rows)
			{
				new_Row = dt.NewRow();
				for(int i=0;i<new_Row.ItemArray.Length;i++)
				{
					new_Row[i] = dr.ItemArray[i];
				}
				dt.Rows.Add(new_Row);
			}
			sqldb.Update(dt,"SELECT * FROM SelectionLinkQID");
		}

		/// <summary>
		/// �NQuestionIndex,SelectionLinkQID,QuestionDataTableName�ҫ�����DataTable��s�^��Ʈw
		/// </summary>
		public void update_DataTableIntoDatabase()
		{					
			//�NSelectionLinkQID��Datatable��s�^��Ʈw
			if(this.Question_Edit_Type!="Choice_Question")
			{
				update_SelectionLinkQID_In_Database();
			}
			//�NQuestionIndex��Datatable��s�^��Ʈw
			if(this.Question_Edit_Type=="Choice_Question")
			{
				update_ItemForQuestion_In_Database();
			}
			else
			{
				update_QuestionIndex_In_Database();
			}
			//�NQuestionLinkQID��Datatable��s�^��Ʈw
			if(this.Question_Edit_Type=="Choice_Question")
			{
				update_SubQuestionMap_In_Database();
			}
			else
			{
				update_QuestionLinkQID_In_Database();
			}
		}
        //���g 2012/11/27 ��s�S�x��
        public void update_FeatureItemIntoDataBase(DataTable dtFeatureItem)
        {
            //�N���ƧR��
            string delete_SQL = "DELETE FeatureForSelect WHERE strQuestionID= '" + dtFeatureItem.Rows[0]["strQID"].ToString() + "'";
            sqldb.ExecuteNonQuery(delete_SQL);

            string strSQL = "SELECT * FROM FeatureForSelect WHERE 1=0";
            DataTable dt = sqldb.getDataSet(strSQL).Tables[0];
            DataRow new_Row = null;

            foreach (DataRow dr in dtFeatureItem.Rows)
            {
                new_Row = dt.NewRow();
                new_Row["strQuestionID"] = dr["strQID"].ToString();
                new_Row["FeatureSetID"] = Convert.ToInt32(dr["FeatureSetID"].ToString());
                new_Row["iFeatureNum"] = Convert.ToInt32(dr["FeatureItemID"].ToString());
                new_Row["cNodeID"] = dr["cNodeID"].ToString();
                dt.Rows.Add(new_Row);

            }
            sqldb.Update(dt, "SELECT * FROM FeatureForSelect");
            /*
            foreach (DataRow dr in dtFeatureItem.Rows)
            {
                try
                {
                    string strSQL = "INSERT INTO FeatureForSelect VALUES('" + QID + "','" + dr["FeatureSetID"].ToString() + "','" + dr["FeatureItemID"].ToString() + "','" + dr["cNodeID"].ToString() + "')";
                    sqldb.ExecuteNonQuery(strSQL);
                }
                catch { }
            }*/
        }
        //���g 2012/11/27 Ū��page�W���S�x�ȡA��Ū���X�Ӧs�JDataTable
        public DataTable get_dtFeatureItem_Data(string strGroup, Page page)
        {
            //�ŧi�@�ӼȦsDataTable�A�B�s�W������
            DataTable dbFeatureItem = new DataTable();
            dbFeatureItem.Columns.Add(new DataColumn("strQID", typeof(string)));
            dbFeatureItem.Columns.Add(new DataColumn("cNodeID", typeof(string)));
            dbFeatureItem.Columns.Add(new DataColumn("FeatureItemID", typeof(string)));
            dbFeatureItem.Columns.Add(new DataColumn("FeatureSetID", typeof(string)));

            Table tbFeatureItem = (Table)page.FindControl("Form1").FindControl("FeatureItemControlTable");

            //��o�S�xListBox
            foreach (Control trContent in tbFeatureItem.Controls)
                if (trContent is TableRow)
                    foreach (Control tcContent in trContent.Controls)
                        if (tcContent is TableCell)
                            foreach (Control tbControlContainer in tcContent.Controls)
                                if (tbControlContainer is Table)
                                    foreach (Control trControlContainer in tbControlContainer.Controls)
                                        if (trControlContainer is TableRow)
                                            foreach (Control tcControlContainer in trControlContainer.Controls)
                                                if (tcControlContainer is TableCell)
                                                    foreach (Control tbListBox in tcControlContainer.Controls)
                                                        if (tbListBox is Table)
                                                            foreach (Control trListBox in tbListBox.Controls)
                                                                if (trListBox is TableRow)
                                                                    foreach (Control tcListBox in trListBox.Controls)
                                                                        if (tcListBox is TableCell)
                                                                            foreach (Control ctlListBox in tcListBox.Controls)
                                                                                if (ctlListBox is ListBox)
                                                                                {
                                                                                    //�P�_ListBox�ﶵ�O�_������A�Y����h�Ȧs��dbFeatureItem
                                                                                    ListBox lbx = (ListBox)ctlListBox;

                                                                                    for (int i = 0; i < lbx.Items.Count; i++)
                                                                                    {
                                                                                        if (lbx.Items[i].Selected == true)
                                                                                        {
                                                                                            DataRow drFeatureRow = dbFeatureItem.NewRow();
                                                                                            drFeatureRow["cNodeID"] = lbx.Items[i].Value.ToString().Split('$')[0];
                                                                                            drFeatureRow["FeatureItemID"] = lbx.Items[i].Value.ToString().Split('$')[1];
                                                                                            drFeatureRow["FeatureSetID"] = lbx.Items[i].Value.ToString().Split('$')[2];
                                                                                            drFeatureRow["strQID"] = lbx.Items[i].Value.ToString().Split('$')[3];
                                                                                            dbFeatureItem.Rows.Add(drFeatureRow);
                                                                                        }
                                                                                    }

                                                                                }

            return dbFeatureItem;
        }

		public void selectScriptCategoryItem(string ScriptCategoryID)
		{
			DataRow[] drs = this.VocabularyCategoryIndex.Select("cVocabularyCategoryIndex='"+ScriptCategoryID+"'");
			drs[0]["IsSelect"] = "Y";
		}	

		public void addScriptCategoryIDInQuestionIndex(string oldID,string ScriptCategoryID,string QuestionText)
		{
			DataRow[] drs_1 = this.QuestionIndex.Select("cQID='"+oldID+"'");
			drs_1[0]["cQID"] = ScriptCategoryID;
			drs_1[0]["cQuestion"] = QuestionText;			
			
			DataRow[] drs_3 = this.SelectionLinkQID.Select("cQID='"+oldID+"'");
			foreach(DataRow dr in drs_3)
			{
				dr["cQID"] = ScriptCategoryID;
			}
			DataRow[] drs_4 = this.SelectionLinkQID.Select("cLinkedQID='"+oldID+"'");
			foreach(DataRow dr in drs_4)
			{
				dr["cLinkedQID"] = ScriptCategoryID;
			}
			qsAccessor.addScriptCategoryIDInQuestionIndex(oldID,ScriptCategoryID);
		}

        public void QuestionIndex_INSERT(string strQID, string strQuestion)
        {
            string strSQL = "INSERT INTO QuestionIndex (cQID, cQuestion, sLevel, cAnswer) " + 
                "VALUES ('" + strQID + "', '" + strQuestion + "', '1', '')";
            sqldb.ExecuteNonQuery(strSQL);
        }

        public void QuestionSelectionIndex_INSERT(string strQID, string strSelectionID, int iSeq, string strAnswer, string strResponse, int iCaseSelect)
        {
            string strSQL_SELECT = "SELECT bCaseSelect FROM QuestionSelectionIndex WHERE cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "'";
            DataTable dtQuestionSelectionIndex = sqldb.getDataSet(strSQL_SELECT).Tables[0];
            if (dtQuestionSelectionIndex.Rows.Count > 0)
            {
                if (dtQuestionSelectionIndex.Rows[0]["bCaseSelect"].ToString() == "True" || dtQuestionSelectionIndex.Rows[0]["bCaseSelect"].ToString() == "1")
                {

                }
                else
                {
                    string strSQL_DELETE = "DELETE QuestionSelectionIndex WHERE cQID = '" + strQID + "' AND cSelectionID = '" + strSelectionID + "'";
                    sqldb.ExecuteNonQuery(strSQL_DELETE);

                    string strSQL = "INSERT INTO QuestionSelectionIndex (cQID, cSelectionID, sSeq, cSelection, cResponse, bCaseSelect) " +
               "VALUES ('" + strQID + "', '" + strSelectionID + "', '" + iSeq + "', '" + strAnswer + "', '" + strResponse + "', '" + iCaseSelect + "')";
                    sqldb.ExecuteNonQuery(strSQL);
                }
            }
            else
            {
                string strSQL = "INSERT INTO QuestionSelectionIndex (cQID, cSelectionID, sSeq, cSelection, cResponse, bCaseSelect) " +
                              "VALUES ('" + strQID + "', '" + strSelectionID + "', '" + iSeq + "', '" + strAnswer + "', '" + strResponse + "', '" + iCaseSelect + "')";
                sqldb.ExecuteNonQuery(strSQL);
            }
        }	

	}
}
