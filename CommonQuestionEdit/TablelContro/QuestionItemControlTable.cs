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
	/// QuestionItemTable ���K�n�y�z�C
	/// </summary>
	public class QuestionItemControlTable:Table
	{
		public string recordDisplayItemID = "";                          //���欰�ΨӰO������Item Row�O�i�}��
        private int intLevel;                                            //���D���h
        private string strCaseID = "";                                   //Case ID
		private string author_UserID = "";                               //�s��̪��ϥΪ�ID
		private string strQID = "";                                      //The ID of Question
		private string strQuestionText = "";                             //The content of Question
        private string strKeyWordsText = "";                             //The content of KeyWords
		private string Question_Edit_Type;                               //Question���s�諬�A;���ݶE���s�諬�A:Interrogation_Enquiry,����D���s�諬�A:Choice_Question,Script���D���s�諬�A:Script_Question		
		private Button delete_Question_Btn = new Button();               //�R���������D�����s
		private Button add_Selection_Btn = new Button();                 //�W�[���D�ﶵ�����s
		private Button add_SubQuestion_Btn = new Button();               //�W�[�l���D�����s
		private HtmlInputButton edit_HTML_Btn = new HtmlInputButton();   //�s��HTML�����s
        private HtmlInputButton edit_Animation = new HtmlInputButton();   //�s��ʵe�����s, added by dolphin @ 2006-07-28
        private HtmlInputButton edit_VM = new HtmlInputButton();          //�s�WVM�����s, added by daniel @2010-5-4
        private HtmlInputHidden hidenRecordDisplayItemID;
		private Label QuestionLabel = new Label();
        private Label KeyWordsLabel = new Label();                        //����r
        private Label lbQuestionLevel = new Label();                       //���D��������
        private DropDownList edit_Level_Ddl = new DropDownList();        //�s����D�����ת��U�Կ��
        private Label lbQuestionGrade = new Label();                       //���D������
        private DropDownList edit_GradeTens_Ddl = new DropDownList();        //�s����D���ƪ��Q��ƤU�Կ��
        private DropDownList edit_GradeUnits_Ddl = new DropDownList();        //�s����D���ƪ��Ӧ�ƤU�Կ��

        public QuestionItemControlTable(string Question_Edit_Type, string author_UserID, string QID, string QuestionText, Page page, int intLevel, HtmlInputHidden recordDisplayItemID, string strCaseID)
		{
			//Question���s�諬�A;���ݶE���s�諬�A:Interrogation_Enquiry,����D���s�諬�A:Choice_Question,Script���D���s�諬�A:Script_Question		
			this.QuestionLabel.EnableViewState = false;
			this.Question_Edit_Type = Question_Edit_Type;
			this.intLevel = intLevel;
			this.author_UserID = author_UserID;
			this.Page = page;
            this.hidenRecordDisplayItemID = recordDisplayItemID;
            this.recordDisplayItemID = recordDisplayItemID.Value;
			this.strQID = QID;
            this.strQuestionText = QuestionText;
            this.strCaseID = strCaseID;		
			setControlID();
			//�غc������HTML Table
			constructQuestionItemTable();
		}

        public QuestionItemControlTable(string Question_Edit_Type, string author_UserID, string QID, string QuestionText, Page page, int intLevel, HtmlInputHidden recordDisplayItemID, string strCaseID, string KeyWordsText)
        {
            //Question���s�諬�A;���ݶE���s�諬�A:Interrogation_Enquiry,����D���s�諬�A:Choice_Question,Script���D���s�諬�A:Script_Question		
            this.QuestionLabel.EnableViewState = false;
            this.Question_Edit_Type = Question_Edit_Type;
            this.intLevel = intLevel;
            this.author_UserID = author_UserID;
            this.Page = page;
            this.hidenRecordDisplayItemID = recordDisplayItemID;
            this.recordDisplayItemID = recordDisplayItemID.Value;
            this.strQID = QID;
            this.strQuestionText = QuestionText;
            this.strKeyWordsText = KeyWordsText;
            this.strCaseID = strCaseID;
            setControlID();
            //�غc������HTML Table
            constructQuestionItemTableWithKeyWords();
        }
		
		/// <summary>
		/// �Q�ΰѼ�"LevelAndRank"�ӳ]�w����Table�P�����ҥ]�t�l����V��ID
		/// </summary>
		/// <param name="LevelAndRank">���D�Ҧb�����h�P���ǥN�X,���D�P�ﶵ�����H"#"�@���j,�ﶵ�P���D�����H"_"�@���j</param>
		private void setControlID()
		{
			this.ID = "QuestionItemTable@" + this.strQID;
			this.delete_Question_Btn.ID = "deleteQuestionBtn@" + this.strQID;
			this.add_Selection_Btn.ID = "addSelectionBtn@" + this.strQID;
			this.add_SubQuestion_Btn.ID = "addSubQuestionBtn@" + this.strQID;
            this.edit_Level_Ddl.ID = "editLevelDdl@" + this.strQID;
            this.edit_GradeTens_Ddl.ID = "editGradeTensDdl@" + this.strQID;
            this.edit_GradeUnits_Ddl.ID = "editGradeUnitsDdl@" + this.strQID;
		}

		/// <summary>
		/// �غc������HTML Table
		/// </summary>
		/// <param name="QuestionItemDataTable"></param>
		private void constructQuestionItemTable()
		{
			if(this.Question_Edit_Type=="Script_Question")
			{
				this.Rows.Add(new TableRow());
				this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
				this.Rows[this.Rows.Count-1].Cells[0].Wrap = false;
				this.Rows[this.Rows.Count-1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
				this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(getScriptCategorySelectionSelection());
			}
			
			this.Rows.Add(new TableRow());
			this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
			this.Rows[this.Rows.Count-1].Cells[0].Wrap = false;
			this.Rows[this.Rows.Count-1].Cells[0].VerticalAlign = VerticalAlign.Middle;

			Table controlContainer = CommonQuestionUtility.get_HTMLTable(1,3);
			controlContainer.BorderWidth = Unit.Pixel(2);
			controlContainer.BorderColor = Color.Purple;
			controlContainer.GridLines = GridLines.Both;
			controlContainer.Width = Unit.Parse("100%");
			this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(controlContainer);

			//�W�[Button	
			addQuestionLabel(0);
			addQuestionTextBox(1);
			addButtonControl(2);
			setTableStyle();
			addSelections_SubQuestion();
            //���g �Ȯɮ���LEVEL�M���ƥ\�� 2013/1/2
            //addQuestionLevel();
            //addQuestionGrade();
		}

        /// <summary>
        /// �غc������HTML Table(�]�tKeyWords)
        /// </summary>
        /// <param name="QuestionItemDataTable"></param>
        private void constructQuestionItemTableWithKeyWords()
        {
            if (this.Question_Edit_Type == "Script_Question")
            {
                this.Rows.Add(new TableRow());
                this.Rows[this.Rows.Count - 1].Cells.Add(new TableCell());
                this.Rows[this.Rows.Count - 1].Cells[0].Wrap = false;
                this.Rows[this.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                this.Rows[this.Rows.Count - 1].Cells[0].Controls.Add(getScriptCategorySelectionSelection());
            }

            this.Rows.Add(new TableRow());
            this.Rows[this.Rows.Count - 1].Cells.Add(new TableCell());
            this.Rows[this.Rows.Count - 1].Cells[0].Wrap = false;
            this.Rows[this.Rows.Count - 1].Cells[0].VerticalAlign = VerticalAlign.Middle;

            Table controlContainer = CommonQuestionUtility.get_HTMLTable(1, 5);
            controlContainer.BorderWidth = Unit.Pixel(2);
            controlContainer.BorderColor = Color.Purple;
            controlContainer.GridLines = GridLines.Both;
            controlContainer.Width = Unit.Parse("100%");
            this.Rows[this.Rows.Count - 1].Cells[0].Controls.Add(controlContainer);

            //�W�[Button	
            addQuestionLabel(0);
            addQuestionTextBox(1);
            addKeyWordsLabel(2);
            addKeyWordsTextBox(3);
            addButtonControl(4);
            setTableStyle();
            addSelections_SubQuestion();
            //���g �Ȯɮ���LEVEL�M���ƥ\�� 2013/1/2
            //addQuestionLevel();
            //addQuestionGrade();
        }

		/// <summary>
		/// �]�wQuestionItemControlTable���~�[�ݩ�
		/// </summary>
		private void setTableStyle()
		{
			if(this.intLevel==1)
			{
				this.Width = Unit.Parse("100%");
			}
			else
			{
				this.Width = Unit.Parse("97%");
				this.HorizontalAlign = HorizontalAlign.Right;
			}
			this.BorderColor = Color.Black;
			this.BorderWidth = Unit.Parse("2px");
		}

		/// <summary>
		/// �W�[Button
		/// </summary>
		/// <param name="containerNum">�n�ˤJContainer����m</param>
		private void addButtonControl(int containerNum)
		{
			delete_Question_Btn.Click += new EventHandler(deleteQuestion_Btn_Click);
			add_Selection_Btn.Click += new EventHandler(addSelection_Btn_Click);
			add_SubQuestion_Btn.Click +=new EventHandler(add_SubQuestion_Btn_Click);
			edit_HTML_Btn.Attributes.Add("onclick","editHTML('"+"QuestionTextBox@" + this.strQID+"');");
            //edit_Level_Ddl.SelectedIndexChanged += new EventHandler(edit_Level_Ddl_SelectedIndexChanged);

            // transfer hints parameter to VM
            string targetQID = "QuestionTextBox@" + this.strQID;
            string caseID = "&caseID=" + this.strCaseID;
            string userID = "&user=" + this.author_UserID;
            string userLevel = "&userLevel=" + this.intLevel;
            string userPrivilege = "&userPrivilege=t";
            string viewMode = "&viewMode=" + this.Question_Edit_Type;
            string edit = "&edit=true";

            string transferHints = targetQID + caseID + userID + userLevel + userPrivilege + viewMode + edit;
            edit_VM.Attributes.Add("onclick", "editVM('" + transferHints + "');");
            
			delete_Question_Btn.Width = Unit.Pixel(130);
			add_Selection_Btn.Width = Unit.Pixel(130);
			add_SubQuestion_Btn.Width = Unit.Pixel(130);
			edit_HTML_Btn.Style.Add("width","130px");
            edit_VM.Style.Add("width", "130px");

			delete_Question_Btn.Text = "Delete the question";
			add_Selection_Btn.Text = "Add new selection";
			add_SubQuestion_Btn.Text = "Add sub question";
			edit_HTML_Btn.Value = "Edit further";//"Edit HTML Content";
            edit_VM.Value = "Edit VM";//"Edit VM Section";
            lbQuestionLevel.Text = "Question Level&nbsp;";
            lbQuestionGrade.Text = "<br/>Question Grade<br/>";
            DataTable dtQuestionLevel = new DataTable();
            dtQuestionLevel = AuthoringTool.QuestionEditLevel.QuestionLevel.QuestionLevelName();
            foreach (DataRow drQuestionLevel in dtQuestionLevel.Rows)
            {
                edit_Level_Ddl.Items.Add(drQuestionLevel["cLevelName"].ToString());
            }
            for (int i = 0; i < 10; i++)
            {
                edit_GradeTens_Ddl.Items.Add(i.ToString());
                edit_GradeUnits_Ddl.Items.Add(i.ToString());
            }

			delete_Question_Btn.BackColor = Color.FromName("#3d77cb");
			add_Selection_Btn.BackColor = Color.FromName("#3d77cb");
			add_SubQuestion_Btn.BackColor = Color.FromName("#3d77cb");
			edit_HTML_Btn.Style.Add("BACKGROUND-COLOR","#3d77cb");
            edit_VM.Style.Add("BACKGROUND-COLOR", "#3d77cb");

			delete_Question_Btn.ForeColor = Color.FromName("#ffffff");
			add_Selection_Btn.ForeColor = Color.FromName("#ffffff");
			add_SubQuestion_Btn.ForeColor = Color.FromName("#ffffff");
			edit_HTML_Btn.Style.Add("color","#ffffff");
            edit_VM.Style.Add("color", "#ffffff");
            lbQuestionLevel.Attributes.Add("style","font-size:12px;");
            lbQuestionGrade.Attributes.Add("style","font-size:12px;");

			Table container = ((Table)this.Rows[this.Rows.Count-1].Cells[0].Controls[0]);
			container.Rows[0].Cells[containerNum].Width = Unit.Pixel(140);
			if(Question_Edit_Type=="Interrogation_Enquiry")
			{
				container.Rows[0].Cells[containerNum].Controls.Add(delete_Question_Btn);
				delete_Question_Btn.Height = Unit.Pixel(40);
				edit_HTML_Btn.Style.Add("height","40px");
                edit_VM.Style.Add("height", "40px");
			}
			else if(Question_Edit_Type=="Script_Question")
			{
				container.Rows[0].Cells[containerNum].Controls.Add(delete_Question_Btn);
				delete_Question_Btn.Height = Unit.Pixel(27);
				edit_HTML_Btn.Style.Add("height","27px");
                edit_VM.Style.Add("height", "27px");
			}
			else
			{
                //���g �Ȯɮ���LEVEL�M���ƥ\�� 2013/1/2
                //container.Rows[0].Cells[containerNum].Controls.Add(lbQuestionLevel);
                //container.Rows[0].Cells[containerNum].Controls.Add(edit_Level_Ddl);
                //container.Rows[0].Cells[containerNum].Controls.Add(lbQuestionGrade);
                //container.Rows[0].Cells[containerNum].Controls.Add(edit_GradeTens_Ddl);
               // container.Rows[0].Cells[containerNum].Controls.Add(edit_GradeUnits_Ddl);
                container.Rows[0].Cells[containerNum].Controls.Add(delete_Question_Btn);
				container.Rows[0].Cells[containerNum].Controls.Add(add_Selection_Btn);
				container.Rows[0].Cells[containerNum].Controls.Add(add_SubQuestion_Btn);
			}
			container.Rows[0].Cells[containerNum].Controls.Add(edit_HTML_Btn);
            container.Rows[0].Cells[containerNum].Controls.Add(edit_VM);
            
            if (Question_Edit_Type == "Choice_Question") { // modified by dolphin @ 2006-08-10, only choice question(Diagnosis) will show this
                // added @ 2006-07-28 by dolphin
                edit_Animation.Attributes.Add("onclick", "toAnimation('"+strQID+"','')");
                edit_Animation.Style.Add("width", "130px");
                edit_Animation.Value = "Edit animation";//"Edit HTML Content";
                edit_Animation.Style.Add("BACKGROUND-COLOR", "#3d77cb");
                edit_Animation.Style.Add("color", "#ffffff");
                container.Rows[0].Cells[containerNum].Controls.Add(edit_Animation);
            }
            
		}

		/// <summary>
		/// �[�J���D
		/// </summary>
		/// <param name="containerNum">�n�ˤJContainer����m</param>
		private void addQuestionLabel(int containerNum)
		{
			DataRow[] drs = getSelectionDataRow();
			if(drs.Length>0)
			{
				string imgSrc = "";
				if(this.recordDisplayItemID.IndexOf(this.ID+";")==-1)
                { imgSrc = "/HINTS/AuthoringTool/CaseEditor/Paper/CommonQuestionEdit/Image/plus.gif"; }
				else
                { imgSrc = "/HINTS/AuthoringTool/CaseEditor/Paper/CommonQuestionEdit/Image/minus.gif"; }
				QuestionLabel.Text = "<nobr>&nbsp;<img style='cursor:hand;' onclick=\"displaySubQuestion('"+this.ID+"');\" src='"+imgSrc+"'>&nbsp;<b>Question:</b></nobr>";
			}
			else
			{
				QuestionLabel.Text = "&nbsp;<b>Question:</b></nobr>";
			}			

			Table container = ((Table)this.Rows[this.Rows.Count-1].Cells[0].Controls[0]);
			container.Rows[0].Cells[containerNum].Width = Unit.Pixel(100);
			container.Rows[0].Cells[containerNum].Controls.Add(QuestionLabel);
		}

        /// <summary>
        /// �[�J���D
        /// </summary>
        /// <param name="containerNum">�n�ˤJContainer����m</param>
        private void addKeyWordsLabel(int containerNum)
        {

            KeyWordsLabel.Text = "&nbsp;<b>KeyWords:<br />(split by \",\" e.g. fever,dizzy)</b></nobr>";

            Table container = ((Table)this.Rows[this.Rows.Count - 1].Cells[0].Controls[0]);
            container.Rows[0].Cells[containerNum].Width = Unit.Pixel(150);
            container.Rows[0].Cells[containerNum].Controls.Add(KeyWordsLabel);
        }

		/// <summary>
		/// �[�J��r��J������ϥΪ̿�J���D
		/// </summary>
		/// <param name="containerNum">�n�ˤJContainer����m</param>
		private void addQuestionTextBox(int containerNum)
		{			
			TextBox tb = new TextBox();
			tb.Attributes.Add("onblur","changeTextBoxContent();");
			//tb.AutoPostBack = true;
			//tb.TextChanged += new EventHandler(tb_TextChanged);
			tb.ID = "QuestionTextBox@" + this.strQID;
			tb.Attributes.Add("QID",this.strQID);
			tb.Text = this.strQuestionText;
			tb.TextMode = TextBoxMode.MultiLine;
			tb.Width = Unit.Parse("100%");
            //tb.Height = Unit.Parse("100%");	// added by dolphin @ 2006-07-28
            tb.Rows = 7;

			Table container = ((Table)this.Rows[this.Rows.Count-1].Cells[0].Controls[0]);
			container.Rows[0].Cells[containerNum].Controls.Add(tb);
		}

        /// <summary>
        /// �[�J��r��J������ϥΪ̿�J����r
        /// </summary>
        /// <param name="containerNum">�n�ˤJContainer����m</param>
        private void addKeyWordsTextBox(int containerNum)
        {
            TextBox tb = new TextBox();
            tb.Attributes.Add("onblur", "changeTextBoxContent();");
            tb.ID = "KeyWordsTextBox@" + this.strQID;
            tb.Attributes.Add("QID", this.strQID);
            tb.Text = this.strKeyWordsText;
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Width = Unit.Parse("300px");
            tb.Rows = 7;

            Table container = ((Table)this.Rows[this.Rows.Count - 1].Cells[0].Controls[0]);
            container.Rows[0].Cells[containerNum].Width = Unit.Parse("310px");
            container.Rows[0].Cells[containerNum].Controls.Add(tb);
        }		

		/// <summary>
		/// ���o�l���D����ƦC�}�C
		/// </summary>
		/// <returns>�l���D����ƦC�}�C</returns>
		private DataRow[] getSub_Question_DataRow()
		{
			DataTable dt = (DataTable)((QuestionAccessor)this.Page.Session["QuestionAccessor"]).QuestionLinkQID;
			DataRow[] ret = dt.Select("cParentQID = '"+this.strQID+"'");
			return ret;
		} 

		/// <summary>
        /// ���o���D�ﶵ�ΰ��D���l���D����ƦC
		/// </summary>
		/// <returns>���D�ﶵ����ƦC�}�C</returns>
		private DataRow[] getSelectionDataRow()
		{//follow-up
			DataTable dt_QuestionSelectionIndex = (DataTable)((QuestionSelectionAccessor)this.Page.Session["QuestionSelectionAccessor"]).QuestionSelectionIndex;
			DataTable dt_QuestionLinkQID = (DataTable)((QuestionAccessor)this.Page.Session["QuestionAccessor"]).QuestionLinkQID;
			DataTable dt_QuestionIndex = (DataTable)((QuestionAccessor)this.Page.Session["QuestionAccessor"]).QuestionIndex;

			DataTable cloneDataTable = dt_QuestionSelectionIndex.Copy();

			DataRow newRow = null;
			foreach(DataRow dr in dt_QuestionLinkQID.Rows)
			{
				newRow = cloneDataTable.NewRow();				
				newRow["cQID"] = dr["cParentQID"].ToString();
				newRow["cSelectionID"] = dr["cSubQID"].ToString();
				newRow["sSeq"] = Convert.ToInt16(dr["sSeq"]);
                if (dt_QuestionIndex.Select("cQID='" + dr["cSubQID"].ToString() + "'").Length > 0)
                {
                    newRow["cSelection"] = dt_QuestionIndex.Select("cQID='" + dr["cSubQID"].ToString() + "'")[0]["cQuestion"].ToString();
                    cloneDataTable.Rows.Add(newRow);
                }
                else 
                {
                    continue;
                }
			}
			DataRow[] ret = cloneDataTable.Select("cQID = '"+this.strQID+"'","sSeq ASC");
			return ret;
		}

		/// <summary>
		/// �[�J���D�ﶵ�ΰ��D���l���D
		/// </summary>
		public void addSelections_SubQuestion()
		{
			string follow_up_ID = "";             //ID
			string follow_up_Text = "";           //���e
			string follow_up_Response_Text = "";  //Response
			bool bIs_Selection_Correct;        //�ﶵ���T�P�_
			
			DataRow[] drs = getSelectionDataRow();
			SelectionItemControlTable selection_Tb = null;
			QuestionItemControlTable question_Tb = null;
			for(int i=0;i<drs.Length;i++)
			{				
				follow_up_ID = drs[i]["cSelectionID"].ToString();				
				follow_up_Text = drs[i]["cSelection"].ToString();
				follow_up_Response_Text = drs[i]["cResponse"].ToString();
				if(drs[i]["bCaseSelect"]!=System.DBNull.Value)
				{//�]��bCaseSelect���ONull�A�]����ܬ����D���ﶵ
					bIs_Selection_Correct = Convert.ToBoolean(drs[i]["bCaseSelect"]);
                    selection_Tb = new SelectionItemControlTable(this.Question_Edit_Type, this.author_UserID, this.strQID, follow_up_ID, follow_up_Text, follow_up_Response_Text, bIs_Selection_Correct, this.Page, this.intLevel + 1, hidenRecordDisplayItemID, i, this.strCaseID);
					if(this.Question_Edit_Type=="Script_Question")
					{
						set_recordDisplayItemID(follow_up_ID,"Selection");
						Table container = ((Table)this.Rows[1].Cells[0].Controls[0]);
						container.Rows[0].Cells[2].Controls.Add(selection_Tb.add_Sub_Question_Btn);
						selection_Tb.add_Sub_Question_Btn.Height = Unit.Pixel(27);
					}
					this.Rows.Add(new TableRow());
					if(this.recordDisplayItemID.IndexOf(this.ID+";")==-1)
					{
						set_recordDisplayItemID(follow_up_ID,"Question");
						//��ܨϥΪ̩|���I��,�]���������ð_��
						//�Y���O�ݶE�s������,�h�]�N���׳������ð_��
						this.Rows[this.Rows.Count-1].Style.Add("DISPLAY","none");
					}
					this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
					this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(selection_Tb);
				}
				else
				{//�]��bCaseSelect��Null�A�]����ܬ����D���l���D
                    question_Tb = new QuestionItemControlTable(this.Question_Edit_Type, this.author_UserID, follow_up_ID, follow_up_Text, this.Page, this.intLevel + 1, this.hidenRecordDisplayItemID, this.strCaseID);
					this.Rows.Add(new TableRow());
					if(this.recordDisplayItemID.IndexOf(this.ID+";")==-1 && this.Question_Edit_Type!="Interrogation_Enquiry")
					{
						//��ܨϥΪ̩|���I��,�]���������ð_��
						this.Rows[this.Rows.Count-1].Style.Add("DISPLAY","none");
					}
					this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
					this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(question_Tb);
				}				
			}
		}

		private void set_recordDisplayItemID(string strSelectionID,string Question_or_Selection)
		{
			if(this.Question_Edit_Type == "Group_Question")
			{
				if(recordDisplayItemID=="")
				{
					recordDisplayItemID = Question_or_Selection + "ItemTable@" + strSelectionID + ";";
				}
				else
				{
					recordDisplayItemID += Question_or_Selection + "ItemTable@" + strSelectionID + ";";
				}
			}
		}				

        /// <summary>
        /// �R�����D���ƥ�
        /// </summary>
		private void deleteQuestion_Btn_Click(object sender, EventArgs e)
		{
			string parent_SelectionID = "";//���D���ݿﶵID
			string sub_QID = ((Button)sender).ID.Split('@')[1];//���o�n�R�������DID
			QuestionAccessor qAccessor = (QuestionAccessor)this.Page.Session["QuestionAccessor"];
			try
			{
				//�p�G�����ﶵ
				parent_SelectionID = ((Table)this.Parent.Parent.Parent).ID.Split('@')[1];
				qAccessor.deleteSelection_Question_RelationInDataTable(parent_SelectionID,sub_QID);
			}
			catch{}
			TextBox tb = (TextBox)this.FindControl("Form1").FindControl("QuestionTextBox@" + sub_QID);			
			qAccessor.deleteQuestionInDataTable(sub_QID);
			if(((Table)this.Parent.Parent.Parent)!=null && ((Table)this.Parent.Parent.Parent).Rows.Count==2)
			{
				try
				{
					((SelectionItemControlTable)this.Parent.Parent.Parent).disablePlusImage();
				}
				catch{}
			}
			((Table)this.Parent.Parent.Parent).Rows.Remove(((TableRow)this.Parent.Parent));			
			if((this.Question_Edit_Type=="Group_Question" || this.Question_Edit_Type=="Choice_Question") && this.intLevel==1)
			{
				this.Page.Session["totalQuestionNum"] = Convert.ToInt32(this.Page.Session["totalQuestionNum"]) - 1;
				this.Page.Session["CurrentEditQuestionNum"] = Convert.ToInt32(this.Page.Session["CurrentEditQuestionNum"]) - 1;
			}
		}

//		/// <summary>
//		/// �Y���D���s��Ҧ��O"GroupQuestion",�h�ݳ]�w��@���B�~��Session�ܼ�,�H�M�w��e�{���覡
//		/// </summary>
//		private void setGroupQuestionSessionVariableForDeleteQuestion()
//		{
//			if(this.Question_Edit_Type=="Group_Question" && this.intLevel==1)
//			{				
//				if(this.Page.Session["totalQuestionNum"]==null)
//				{
//					Session.Add("totalQuestionNum",drs.Length);
//				}
//				else
//				{
//					Session["totalQuestionNum"] = drs.Length;
//				}
//				if(!Convert.ToBoolean(Session["bModify"]))
//				{
//					Session["CurrentEditQuestionNum"] = Convert.ToInt32(Session["totalQuestionNum"])-1;
//				}
//			}
//		}		

		/// <summary>
		/// �Φb�ݶE�����s�W�l���D�s�a�]�|���ͤl���D���^��
		/// </summary>
		public void addQuestionAnswer(Page page)
		{
			this.Page = page;
			if(this.Question_Edit_Type=="Group_Question" || this.Question_Edit_Type=="Choice_Question")
			{
				string[] new_SelectionID = CommonQuestionUtility.GetNewID(this.author_UserID,"Selection",4);//�s�W���ﶵID
				for(int i=0;i<4;i++)
				{					
					((QuestionSelectionAccessor)this.Page.Session["QuestionSelectionAccessor"]).add_new_selection(this.strQID,new_SelectionID[i]);	
					addSelection(new_SelectionID[i]);
				}
			}
			else
			{
				string new_SelectionID = CommonQuestionUtility.GetNewID(this.author_UserID,"Selection");//�s�W���ﶵID
				((QuestionSelectionAccessor)this.Page.Session["QuestionSelectionAccessor"]).add_new_selection(this.strQID,new_SelectionID);	
				addSelection(new_SelectionID);
			}
		}

//		/// <summary>
//		/// �ק��r�B�z������ƥ�
//		/// </summary>
//		private void tb_TextChanged(object sender, EventArgs e)
//		{
//			string tmpQID = ((TextBox)sender).ID.Split('@')[1];//���o���DID
//			TextBox tb = (TextBox)this.FindControl("Form1").FindControl("QuestionTextBox@" + tmpQID);
//			((QuestionAccessor)this.Page.Session["QuestionAccessor"]).modifyQuestionText(tmpQID,tb.Text);
//		}

		private Table getScriptCategorySelectionSelection()
		{
			Label scriptSelectionLabel = new Label();
			scriptSelectionLabel.Text = "<b>Please select a script category.</b>&nbsp;&nbsp;";

			DropDownList ddl = new DropDownList();			
			ddl.ID = "ScriptCategorySelection@" + this.strQID;
			ddl.AutoPostBack = true;
			ddl.SelectedIndexChanged += new EventHandler(ret_SelectedIndexChanged);
			DataTable dt = ((QuestionAccessor)this.Page.Session["QuestionAccessor"]).VocabularyCategoryIndex;
			ddl.Items.Add(new ListItem("Select a script category"));
			foreach(DataRow dr in dt.Rows)
			{
				ddl.Items.Add(new ListItem(dr["cVocabularyCategory"].ToString(),dr["cUsedUnitScriptID"].ToString()));
			}
//			DataRow[] drs = dt.Select("cUsedUnitScriptID = '"+dr["cUsedUnitScriptID"].ToString()+"'");
//			foreach(DataRow dr in drs)
//			{
//				dr["IsSelect"] = "N";
//			}
			Table container = CommonQuestionUtility.get_HTMLTable(1,2);
			container.Rows[0].Cells[0].Controls.Add(scriptSelectionLabel);
			container.Rows[0].Cells[0].Controls.Add(ddl);
			return container;
		}

		private void ret_SelectedIndexChanged(object sender, EventArgs e)
		{
			string oldQID = ((DropDownList)sender).ID.Split('@')[1];//���o���DID
			TextBox tb = (TextBox)this.FindControl("Form1").FindControl("QuestionTextBox@" + oldQID);
			string scriptCategoryID = ((DropDownList)sender).SelectedValue;
			QuestionAccessor qa = ((QuestionAccessor)this.Page.Session["QuestionAccessor"]);
			((DropDownList)sender).SelectedItem.Text = qa.getScriptValue(scriptCategoryID);		
			string questionText = qa.getScriptQuestion(scriptCategoryID);
			tb.Text = questionText;
			qa.addScriptCategoryIDInQuestionIndex(oldQID,scriptCategoryID,questionText);			
		}

		/// <summary>
		/// �W�[���D�ﶵ
		/// </summary>
		/// <param name="selectionID">�ﶵID</param>
		public void addSelection(string new_SelectionID)
		{
            DataRow[] drs = getSelectionDataRow();
            SelectionItemControlTable selection_Tb = new SelectionItemControlTable(this.Question_Edit_Type, this.author_UserID, this.strQID, new_SelectionID, "", "", false, this.Page, this.intLevel, hidenRecordDisplayItemID, drs.Length - 1, this.strCaseID);
			if(recordDisplayItemID.IndexOf(this.ID + ";")==-1)
			{
                hidenRecordDisplayItemID.Value += this.ID + ";";
			}
			if(this.Question_Edit_Type=="Script_Question")
			{
				Table container = ((Table)this.Rows[1].Cells[0].Controls[0]);
				container.Rows[0].Cells[2].Controls.Add(selection_Tb.add_Sub_Question_Btn);
				selection_Tb.add_Sub_Question_Btn.Height = Unit.Pixel(27);
			}
			//selection_Tb.Style.Add("display","");
			this.Rows.Add(new TableRow());
			this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
			this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(selection_Tb);
			for(int i=0;i<this.Rows.Count-1;i++)
			{
				this.Rows[i].Style.Add("display","");
			}
            string imgSrc = "/HINTS/AuthoringTool/CaseEditor/Paper/CommonQuestionEdit/Image/minus.gif";
			QuestionLabel.Text = "<nobr>&nbsp;<img style='cursor:hand;' onclick=\"displaySubQuestion('"+this.ID+"');\" src='"+imgSrc+"'>&nbsp;<b>Question:</b></nobr>";
		}

		/// <summary>
		/// �غc�s���l���D
		/// </summary>
		public void add_sub_question(string new_Sub_QID)
		{
            QuestionItemControlTable sub_Question_Tb = new QuestionItemControlTable(this.Question_Edit_Type, this.author_UserID, new_Sub_QID, "", this.Page, this.intLevel + 1, hidenRecordDisplayItemID, this.strCaseID);
			if(recordDisplayItemID.IndexOf(this.ID + ";")==-1)
			{
                hidenRecordDisplayItemID.Value += this.ID + ";";
			}
			if(this.Question_Edit_Type=="Script_Question" || this.Question_Edit_Type=="Interrogation_Enquiry")
			{
				sub_Question_Tb.addQuestionAnswer(this.Page);
			}
			this.Rows.Add(new TableRow());			
			this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
			this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(sub_Question_Tb);

			for(int i=0;i<this.Rows.Count-1;i++)
			{
				this.Rows[i].Style.Add("display","");
			}
            string imgSrc = "/HINTS/AuthoringTool/CaseEditor/Paper/CommonQuestionEdit/Image/minus.gif";
			QuestionLabel.Text = "<nobr>&nbsp;<img style='cursor:hand;' onclick=\"displaySubQuestion('"+this.ID+"');\" src='"+imgSrc+"'>&nbsp;<b>Question:</b></nobr>";

//			if(this.recordDisplayItemID.IndexOf(this.ID+";")==-1 && this.Question_Edit_Type!="Interrogation_Enquiry")
//			{
//				//��ܨϥΪ̩|���I��,�]���������ð_��
//				for(int j=0;j<this.Rows.Count-1;j++)
//				{
//					this.Rows[j].Style.Add("DISPLAY","");
//				}
//				((TableRow)this.Parent.Parent).Style.Add("DISPLAY","");
//				string imgSrc = "../image/minus.gif";
//				SelectionLabel.Text = "<nobr>&nbsp;<img style='cursor:hand;' onclick=\"displaySubQuestion('"+this.ID+"');\" src='"+imgSrc+"'>&nbsp;<b>Selection:</b></nobr>";
//			}
		}

		/// <summary>
		/// �W�[�l���D��button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void add_SubQuestion_Btn_Click(object sender, EventArgs e)
		{
			string new_Sub_QID = CommonQuestionUtility.GetNewID(this.author_UserID,"Question");//�ﶵ�ҭn�s�W�����DID
            ((QuestionAccessor)this.Page.Session["QuestionAccessor"]).add_New_Sub_Question(this.strQID, new_Sub_QID, this.intLevel + 1);
            // the following is failed by dolphin, currently removed @ 2006-08-20
            //if (Question_Edit_Type == "Choice_Question") {
            //    try {
            //        string strSQL = "INSERT INTO SubQuestionMap VALUES('" + "hsmokCase200607090009459062500" + "','" + this.strQID + "','" + new_Sub_QID + "')";
            //        Hints.DB.clsHintsDB hintsDB = new Hints.DB.clsHintsDB();
            //        hintsDB.ExecuteNonQuery(strSQL);
            //    }
            //    catch { }
            //}
			add_sub_question(new_Sub_QID);
		}

		/// <summary>
		/// �W�[���D�ﶵ���ƥ�
		/// </summary>
		private void addSelection_Btn_Click(object sender, EventArgs e)
		{
			string new_SelectionID = CommonQuestionUtility.GetNewID(this.author_UserID,"Selection");//�s�W���ﶵID
			((QuestionSelectionAccessor)this.Page.Session["QuestionSelectionAccessor"]).add_new_selection(this.strQID,new_SelectionID);	
			addSelection(new_SelectionID);
		}

        /// <summary>
        /// ���o���D��������
        /// </summary>
        private void addQuestionLevel()
        {
            int iQuestionLevel = AuthoringTool.QuestionEditLevel.QuestionLevel.QuestionLevelValue(this.strQID);
            if (iQuestionLevel != -1)
                edit_Level_Ddl.SelectedValue = AuthoringTool.QuestionEditLevel.QuestionLevel.QuestionLevelName_SELECT_LevelName(iQuestionLevel);
        }
        /// <summary>
        /// ���o���D������
        /// </summary>
        private void addQuestionGrade()
        {
            string strQuestionGrade = QuestionLevel.QuestionLevel_SELECT_Grade(this.strQID);
            if (strQuestionGrade != "-1")
            {
                edit_GradeTens_Ddl.SelectedValue = strQuestionGrade.Substring(0,1);
                edit_GradeUnits_Ddl.SelectedValue = strQuestionGrade.Substring(1,1);
            }

        }
	}
}
