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
	/// SelectionItemControlTable 的摘要描述。
	/// </summary>
	public class SelectionItemControlTable:Table
	{		
		private int intLevel;                                             //選項所屬問題的階層
		private string author_UserID = "";                                //編輯者的使用者ID
		private bool bIsCorrectAnswer;                                    //此選項是否為建議選項
		private string QID = "";                                          //選項所在的問題ID
		private string strSelectionID = "";                               //The ID of Selection
		private string strSelectionText = "";                             //The content of Selection		
		private string strSelectionResponseText = "";                     //The content of Response
        private string strSelectionKeyWordsText = "";                     //The content of KeyWords	
        private string Question_Edit_Type;                                //Question的編輯型態;有問診的編輯型態:Interrogation_Enquiry,選擇題的編輯型態:Choice_Question,Script問題的編輯型態:Script_Question		
        private string strCaseID = "";                                    //Case ID	
		public Button delete_Selection_Btn = new Button();                //刪除本身選項的按鈕
		public Button add_Sub_Question_Btn = new Button();                //增加子問題的按鈕
		private HtmlInputButton edit_HTML_Btn = new HtmlInputButton();    //編輯HTML的按鈕
        private HtmlInputButton btn_Rationale = new HtmlInputButton();    //編輯rationale的按鈕
		private Label SelectionLabel = new Label();
		private Label SelectionResponseLabel = new Label();
		private Label CorrectAnswerLabel = new Label();
        private Label KeyWordsLabel = new Label();
        private HtmlInputHidden hidenRecordDisplayItemID;
		private string recordDisplayItemID = "";   //此欄為用來記錄哪些Item Row是展開的
        private int intNumber = 0;

        public SelectionItemControlTable(string Question_Edit_Type, string author_UserID, string QID, string selectionID, string selectionText, string strSelectionResponseText, bool bIsCorrectAnswer, Page page, int intLevel, HtmlInputHidden recordDisplayItemID, int intNumber, string strCaseID)
		{
            this.intNumber = intNumber;
			this.SelectionLabel.EnableViewState = false;
			this.Question_Edit_Type = Question_Edit_Type;
			this.intLevel = intLevel;
			this.author_UserID = author_UserID;
			this.bIsCorrectAnswer = bIsCorrectAnswer;
			this.Page = page;
            this.hidenRecordDisplayItemID = recordDisplayItemID;
            this.recordDisplayItemID = recordDisplayItemID.Value;
			this.QID = QID;
            this.strSelectionID = selectionID;
            this.strCaseID = strCaseID;
			if(selectionID == "wyt_Selection_200606171701380468750_0")
			{
				string hh = "ddd";
			}
			this.strSelectionText = selectionText;
			this.strSelectionResponseText = strSelectionResponseText;
			//利用參數"LevelAndRank"來設定本身Table與本身所包含子控制向的ID
			setControlID();
			//建構本身的HTML Table
			constructSelectionItemTable();	
			if(Question_Edit_Type.Equals("Script_Question"))
			{
				((Table)this.Rows[0].Cells[0].Controls[0]).Style.Add("display","none");
			}
		}
		
		/// <summary>
		/// 利用參數"LevelAndRank"來設定本身Table與本身所包含子控制向的ID
		/// </summary>
		/// <param name="LevelAndRank">問題所在的階層與順序代碼,問題與選項之間以"#"作分隔,選項與問題之間以"_"作分隔</param>
		private void setControlID()
		{
			this.ID = "SelectionItemTable@" + this.strSelectionID;
			this.delete_Selection_Btn.ID = "deleteSelectionBtn@" + this.strSelectionID;
			this.add_Sub_Question_Btn.ID = "addSubQuestionBtn@" + this.strSelectionID;			
		}

		/// <summary>
		/// 建構本身的HTML Table
		/// </summary>
		/// <param name="QuestionItemDataTable"></param>
		private void constructSelectionItemTable()
		{
			this.Rows.Add(new TableRow());
			this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
			this.Rows[this.Rows.Count-1].Cells[0].Wrap = false;
			this.Rows[this.Rows.Count-1].Cells[0].VerticalAlign = VerticalAlign.Middle;
			Table controlContainer = CommonQuestionUtility.get_HTMLTable(2,4);
            if (intNumber > 0)
            {
                controlContainer.Rows[0].Visible = false;
            }
			controlContainer.BorderWidth = Unit.Pixel(2);
			controlContainer.BorderColor = Color.Purple;
			controlContainer.GridLines = GridLines.Both;
			controlContainer.Width = Unit.Parse("100%");
			this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(controlContainer);
			
			addSelectionLabel(0,0);
			addSelectionResponseLabel(0,1);
			addSelectionCorrectAnswerLabel(0,2);
            if (intNumber > 0)
            {
                addButtonControl(1,3);
            }
            else
            {
                addButtonControl(0,3);
            }
			addSelectionTextBox(1,0);
			addSelectionResponseTextBox(1,1);
			addCheckBox(1,2);

			addPlusImage(true);
			
			
			add_sub_question();
			setTableStyle();
		}

		/// <summary>
		/// 設定QuestionItemControlTable的外觀屬性
		/// </summary>
		private void setTableStyle()
		{			
			if(this.Question_Edit_Type=="Choice_Question" || this.Question_Edit_Type=="Group_Question")
			{
				this.Width = Unit.Parse("100%");
				this.HorizontalAlign = HorizontalAlign.Right;
				//this.BorderColor = Color.Red;
			}
			else if(this.Question_Edit_Type=="Interrogation_Enquiry" || this.Question_Edit_Type=="Script_Question")
			{
				this.Width = Unit.Parse("100%");
			}			
			//this.BorderWidth = Unit.Parse("2px");
		}

		/// <summary>
		/// 增加Button
		/// </summary>
		private void addButtonControl(int rowNum,int colNum)
		{
			delete_Selection_Btn.Click += new EventHandler(delete_Selection_Btn_Click);
			add_Sub_Question_Btn.Click += new EventHandler(add_Sub_Question_Btn_Click);
			edit_HTML_Btn.Attributes.Add("onclick","editHTML('"+"SelectionTextBox@" + strSelectionID+"');");
            btn_Rationale.Attributes.Add("onclick","EditRationale('"+strSelectionID+"');");

			delete_Selection_Btn.Width = Unit.Pixel(136);
			add_Sub_Question_Btn.Width = Unit.Pixel(136);
			edit_HTML_Btn.Style.Add("width","136px");
            btn_Rationale.Style.Add("width", "136px");

			delete_Selection_Btn.Text = "Delete the selection";
			add_Sub_Question_Btn.Text = "Add the sub question";
			edit_HTML_Btn.Value = "Edit further";//"Edit HTML Content";
            btn_Rationale.Value = "Rationale";

			delete_Selection_Btn.BackColor = Color.FromName("#3d77cb");
			add_Sub_Question_Btn.BackColor = Color.FromName("#3d77cb");
			edit_HTML_Btn.Style.Add("BACKGROUND-COLOR","#3d77cb");
            btn_Rationale.Style.Add("BACKGROUND-COLOR", "#3d77cb");

			delete_Selection_Btn.ForeColor = Color.FromName("#ffffff");
			add_Sub_Question_Btn.ForeColor = Color.FromName("#ffffff");
			edit_HTML_Btn.Style.Add("color","#ffffff");
            btn_Rationale.Style.Add("color", "#ffffff");

			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);
			container.Rows[rowNum].Cells[colNum].Width = Unit.Parse("1px");
            if (rowNum == 0)
            {
                container.Rows[rowNum].Cells[colNum].RowSpan = 2;
                container.Rows[rowNum + 1].Cells.RemoveAt(colNum - 1);
            }
			if(Question_Edit_Type=="Interrogation_Enquiry")
			{
				add_Sub_Question_Btn.Height = Unit.Pixel(40);
				edit_HTML_Btn.Style.Add("height","40px");
				container.Rows[rowNum].Cells[colNum].Controls.Add(add_Sub_Question_Btn);				
			}
			else
			{
				container.Rows[rowNum].Cells[colNum].Controls.Add(delete_Selection_Btn);
				container.Rows[rowNum].Cells[colNum].Controls.Add(add_Sub_Question_Btn);				
			}
			if(Question_Edit_Type=="Choice_Question")
			{
				add_Sub_Question_Btn.Visible = false;

                // added @ 2006-08-04 by dolphin, allowing to upload animation in Diagnosis.
                HtmlInputButton edit_Animation = new HtmlInputButton();   //編輯動畫的按鈕, added by dolphin @ 2006-08-04
                string[] temp = strSelectionID.Split('_');
                string seq = temp[temp.Length - 1];
                edit_Animation.Attributes.Add("onclick", "toAnimation('" + QID + "','" + seq + "')");
                edit_Animation.Style.Add("width", "136px");
                edit_Animation.Value = "Edit animation";//"Edit HTML Content";
                edit_Animation.Style.Add("BACKGROUND-COLOR", "#3d77cb");
                edit_Animation.Style.Add("color", "#ffffff");
                container.Rows[rowNum].Cells[colNum].Controls.Add(edit_Animation);
                //*****************************************************************
                container.Rows[rowNum].Cells[colNum].Controls.Add(btn_Rationale);
			}
			container.Rows[rowNum].Cells[colNum].Controls.Add(edit_HTML_Btn);
		}

		/// <summary>
		/// 增加和取方塊讓使用者選擇此選項是否為建議選項
		/// </summary>
		/// <param name="containerNum">要裝入Container的位置</param>
		private void addCheckBox(int rowNum,int colNum)
		{
			HtmlInputCheckBox chkbox =new HtmlInputCheckBox();
			chkbox.Style["width"] = "13px";
			chkbox.Style["height"] = "13px";
			chkbox.ID = "IsCorrectChkBox@" + this.QID + "#" + this.strSelectionID;			
			chkbox.Attributes["onclick"] = "chkbox_CheckedChanged(this);";			
			chkbox.Checked = this.bIsCorrectAnswer;
			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);
			container.Rows[rowNum].Cells[colNum].Wrap = false;			
			container.Rows[rowNum].Cells[colNum].Controls.Add(chkbox);
			container.Rows[rowNum].Cells[colNum].HorizontalAlign = HorizontalAlign.Center;
			container.Rows[rowNum].Cells[colNum].Width = Unit.Parse("150px");
		}


		/// <summary>
		/// 加入標題
		/// </summary>
		/// <param name="containerNum">要裝入Container的位置</param>
		private void addSelectionResponseLabel(int rowNum,int colNum)
		{
			if(this.Question_Edit_Type=="Group_Question" || this.Question_Edit_Type=="Choice_Question")
			{
				SelectionResponseLabel.Text = "&nbsp;<b>Hint</b></nobr>";
			}
			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);			
			container.Rows[rowNum].Cells[colNum].Controls.Add(SelectionResponseLabel);
			container.Rows[rowNum].Cells[colNum].HorizontalAlign = HorizontalAlign.Center;
		}

		/// <summary>
		/// 加入標題
		/// </summary>
		/// <param name="containerNum">要裝入Container的位置</param>
		private void addSelectionCorrectAnswerLabel(int rowNum,int colNum)
		{
			if(this.Question_Edit_Type=="Group_Question" || this.Question_Edit_Type=="Choice_Question")
			{
				CorrectAnswerLabel.Text = "<nobr>&nbsp;<b>Correct Answer</b></nobr>";
			}
			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);			
			container.Rows[rowNum].Cells[colNum].Controls.Add(CorrectAnswerLabel);
			container.Rows[rowNum].Cells[colNum].HorizontalAlign = HorizontalAlign.Center;
		}

        /// <summary>
        /// 加入關鍵字標題
        /// </summary>
        /// <param name="containerNum">要裝入Container的位置</param>
        private void addSelectionKeyWordsLabel(int rowNum, int colNum)
        {
            if (this.Question_Edit_Type == "Group_Question" || this.Question_Edit_Type == "Choice_Question")
            {
                KeyWordsLabel.Text = "<nobr>&nbsp;<b>KeyWords(split by\",\" e.g. fever,dizzy )</b></nobr>";
            }
            Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);
            container.Rows[rowNum].Cells[colNum].Controls.Add(KeyWordsLabel);
            container.Rows[rowNum].Cells[colNum].HorizontalAlign = HorizontalAlign.Center;
        }


		/// <summary>
		/// 加入標題
		/// </summary>
		/// <param name="containerNum">要裝入Container的位置</param>
		private void addSelectionLabel(int rowNum,int colNum)
		{
			if(this.Question_Edit_Type=="Interrogation_Enquiry" || this.Question_Edit_Type=="Script_Question")
			{
				SelectionLabel.Text = "<b>Answer</b>&nbsp;&nbsp;";
			}
			else
			{
				SelectionLabel.Text = "&nbsp;<b>Selection</b></nobr>";
			}
			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);			
			container.Rows[rowNum].Cells[colNum].Controls.Add(SelectionLabel);
			container.Rows[rowNum].Cells[colNum].HorizontalAlign = HorizontalAlign.Center;
		}

		/// <summary>
		/// 將"+"符號給隱藏起來
		/// </summary>		
		public void disablePlusImage()
		{			
			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);		
			container.Rows[0].Cells[0].Text = "";
			container.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
		}

		/// <summary>
		/// 加入標題
		/// </summary>
		/// <param name="containerNum">要裝入Container的位置</param>
		private void addPlusImage(bool bAddCell)
		{
			string strPlusImage = "";
			if(this.Question_Edit_Type=="Interrogation_Enquiry" || this.Question_Edit_Type=="Script_Question")
			{
				
			}
			else
			{
				DataRow[] drs = getSub_Question_DataRow();
				if(drs.Length>0)
				{
					string imgSrc = "";
					if(this.recordDisplayItemID.IndexOf(this.ID+";")==-1)
					{	
						imgSrc = "../image/plus.gif";
					}
					else
					{
						imgSrc = "../image/minus.gif";
					}
					strPlusImage = "&nbsp;<img style='cursor:hand;' onclick=\"displaySubQuestion('"+this.ID+"');\" src='"+imgSrc+"'>";
				}
				else
				{
					//SelectionLabel.Text = "&nbsp;<b>Selection</b></nobr>";
				}
			}
			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);
			if(bAddCell)
			{
				container.Rows[0].Cells.AddAt(0,new TableCell());
				container.Rows[0].Cells[0].RowSpan = 2;
			}
			container.Rows[0].Cells[0].Text = strPlusImage;
			container.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
		}

		/// <summary>
		/// 增加問題選項文字輸入方塊
		/// </summary>
		/// <param name="containerNum">要裝入Container的位置</param>
		private void addSelectionResponseTextBox(int rowNum,int colNum)
		{
			TextBox tb_response = new TextBox();
			tb_response.Attributes.Add("onblur","changeTextBoxContent();");			
			tb_response.ID = "SelectionResponseTextBox@" + strSelectionID;
			tb_response.Text = this.strSelectionResponseText;
			tb_response.TextMode = TextBoxMode.MultiLine;
			tb_response.Width = Unit.Parse("350px");
            tb_response.Height = Unit.Parse("100px");
			Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);
			container.Rows[rowNum].Cells[colNum].Controls.Add(tb_response);
            container.Rows[rowNum].Cells[colNum].HorizontalAlign = HorizontalAlign.Center;
            //fadis 2006.12.27
            Button btn_HTMLEditor = new Button();
            btn_HTMLEditor.Text = "...";
            btn_HTMLEditor.OnClientClick = "OpenHTMLEditor('" + tb_response.ID + "')";
            //btn_HTMLEditor.ID = "txt_" + tb_response.ID;
            container.Rows[rowNum].Cells[colNum].Controls.Add(btn_HTMLEditor);
            /////////////////////////////////
			//container.Rows[rowNum].Cells[colNum].Width = Unit.Parse("49%");
		}

		/// <summary>
		/// 增加問題選項文字輸入方塊
		/// </summary>
		/// <param name="containerNum">要裝入Container的位置</param>
		private void addSelectionTextBox(int rowNum,int colNum)
		{
			TextBox tb = new TextBox();
			tb.Attributes.Add("onblur","changeTextBoxContent();");			
			tb.ID = "SelectionTextBox@" + strSelectionID;
			tb.Text = this.strSelectionText;
			tb.TextMode = TextBoxMode.MultiLine;
			tb.Width = Unit.Parse("450px");
            tb.Height = Unit.Parse("100px");
            Table container = ((Table)this.Rows[0].Cells[0].Controls[0]);			
			container.Rows[rowNum].Cells[colNum].Controls.Add(tb);
            //container.Rows[rowNum].Cells[colNum].Width = Unit.Parse("49%");
			container.Rows[rowNum].Cells[colNum].HorizontalAlign = HorizontalAlign.Center;
		}

		/// <summary>
		/// 建構子問題集合
		/// </summary>
		public void add_sub_question()
		{
			string sub_QID = "";
			string sub_QuestionText = "";
			
			DataRow[] drs = getSub_Question_DataRow();
			QuestionItemControlTable sub_Question_Tb = null;//子問題的HTML Table
			for(int i=0;i<drs.Length;i++)
			{				
				sub_QID = drs[i]["cLinkedQID"].ToString();
				set_recordDisplayItemID(sub_QID);
				sub_QuestionText = ((QuestionAccessor)this.Page.Session["QuestionAccessor"]).getQuestionText(sub_QID);
                sub_Question_Tb = new QuestionItemControlTable(this.Question_Edit_Type, this.author_UserID, sub_QID, sub_QuestionText, this.Page, this.intLevel + 1, hidenRecordDisplayItemID, strCaseID);
				this.Rows.Add(new TableRow());
				if(this.recordDisplayItemID.IndexOf(this.ID+";")!=-1 && this.Question_Edit_Type!="Interrogation_Enquiry")
				{
					//表示使用者尚未點選,因此應該隱藏起來
					this.Rows[this.Rows.Count-1].Style.Add("DISPLAY","");
				}
				else
				{
					this.Rows[this.Rows.Count-1].Style.Add("DISPLAY","none");
				}
				this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
				this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(sub_Question_Tb);
			}			
		}

		private void set_recordDisplayItemID(string strQID)
		{
			if(this.Question_Edit_Type == "Group_Question")
			{
				if(recordDisplayItemID=="")
				{
					recordDisplayItemID = "QuestionItemTable@" + strQID + ";";
				}
				else
				{
					recordDisplayItemID += "QuestionItemTable@" + strQID + ";";
				}
			}
		}

		/// <summary>
		/// 建構新的子問題
		/// </summary>
		public void add_sub_question(string new_Sub_QID)
		{
            QuestionItemControlTable sub_Question_Tb = new QuestionItemControlTable(this.Question_Edit_Type, this.author_UserID, new_Sub_QID, "", this.Page, this.intLevel + 1, hidenRecordDisplayItemID, strCaseID);
			if(recordDisplayItemID.IndexOf(this.ID + ";")==-1)
			{
				hidenRecordDisplayItemID.Value += this.ID + ";";
				this.recordDisplayItemID += this.ID + ";";
			}
			if(this.Question_Edit_Type=="Script_Question" || this.Question_Edit_Type=="Interrogation_Enquiry")
			{
				sub_Question_Tb.addQuestionAnswer(this.Page);
			}
			this.Rows.Add(new TableRow());			
			this.Rows[this.Rows.Count-1].Cells.Add(new TableCell());
			this.Rows[this.Rows.Count-1].Cells[0].Controls.Add(sub_Question_Tb);
			if(this.recordDisplayItemID.IndexOf(this.ID+";")!=-1 && this.Question_Edit_Type!="Interrogation_Enquiry")
			{
				//表示使用者尚未點選,因此應該隱藏起來
				for(int j=0;j<this.Rows.Count-1;j++)
				{
					this.Rows[j].Style.Add("DISPLAY","");
				}
				((TableRow)this.Parent.Parent).Style.Add("DISPLAY","");				
				SelectionLabel.Text = "&nbsp;<b>Selection</b></nobr>";
			}
		}

		/// <summary>
		/// 取得子問題的資料列陣列
		/// </summary>
		/// <returns>子問題的資料列陣列</returns>
		private DataRow[] getSub_Question_DataRow()
		{
			DataTable dt = (DataTable)((QuestionAccessor)this.Page.Session["QuestionAccessor"]).SelectionLinkQID;
			DataRow[] ret = dt.Select("cQID = '"+this.QID+"' AND cSelectionID='"+this.strSelectionID+"'");
			return ret;
		}

		private void delete_Selection_Btn_Click(object sender, EventArgs e)
		{
			string tmpSelectionID = ((Button)sender).ID.Split('@')[1];                        //取得要刪除的選項ID
			string tmpQID = ((Table)this.Parent.Parent.Parent).ID.Split('@')[1];              //取得要刪除的選項所屬的問題的ID
			QuestionSelectionAccessor qsAccessor = (QuestionSelectionAccessor)this.Page.Session["QuestionSelectionAccessor"];
			qsAccessor.deleteSelectionDataRow(tmpQID,tmpSelectionID);                         //在Datatable中刪除選項
			((Table)this.Parent.Parent.Parent).Rows.Remove(((TableRow)this.Parent.Parent));   
		}

		private void add_Sub_Question_Btn_Click(object sender, EventArgs e)
		{
			string tmpQID = ((Table)this.Parent.Parent.Parent).ID.Split('@')[1];               //取得要選項所屬的問題的ID
			string new_Sub_QID = CommonQuestionUtility.GetNewID(this.author_UserID,"Question");//選項所要新增的問題ID
			((QuestionAccessor)this.Page.Session["QuestionAccessor"]).add_New_Sub_Question(tmpQID,strSelectionID,new_Sub_QID,this.intLevel+1);			
			add_sub_question(new_Sub_QID);
			addPlusImage(false);
		}
	}
}
