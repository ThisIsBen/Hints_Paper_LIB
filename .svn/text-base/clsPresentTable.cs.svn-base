using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using suro.util;

namespace PaperSystem
{
    /// <summary>
    /// clsPresentTable 的摘要描述。
    /// </summary>
    public class clsPresentTable
    {
        public clsPresentTable()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }
        /// <summary>
        /// 建立Simulator的Table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strPaperID"></param>
        public void getSimulatorTable(Table table, string strPaperID, string strCaseID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcAssignTitle = new TableCell();
            tr.Cells.Add(tcAssignTitle);
            tcAssignTitle.Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + "套用Table Driven關係:";
            tcAssignTitle.ForeColor = System.Drawing.Color.Black;
            tcAssignTitle.Font.Size = 18;

            TableCell tcTablrDriven = new TableCell();
            tr.Cells.Add(tcTablrDriven);
            tcTablrDriven.VerticalAlign = VerticalAlign.Top;
            tcTablrDriven.ColumnSpan = 2;

            DataTable dtTD_QandA = Hints.DB.TableDriven.TD_QandA.TD_QandA_SELECT(strPaperID, strCaseID);
            if (dtTD_QandA.Rows.Count > 0)
            {
                Label lbTDTemplate = new Label();
                lbTDTemplate.Text = Hints.DB.TableDriven.clsTableDriven.TDTemplate_SELECT(dtTD_QandA.Rows[0]["cTDTemplateID"].ToString()).Rows[0]["cTDTemplateName"].ToString() + "&nbsp;&nbsp;";
                lbTDTemplate.ForeColor = System.Drawing.Color.Blue;
                lbTDTemplate.Font.Size = 18;
                tcTablrDriven.Controls.Add(lbTDTemplate);

                HtmlButton btModifyTD = new HtmlButton();
                btModifyTD.InnerText = "修改";
                btModifyTD.Attributes.Add("class", "button_continue");

                tcTablrDriven.Controls.Add(btModifyTD);
                btModifyTD.Attributes.Add("onclick", "OpenTDTemplate()");

                Label lbSpace = new Label();
                lbSpace.Text = "&nbsp;&nbsp;";
                tcTablrDriven.Controls.Add(lbSpace);

                Button btDeleteTD = new Button();
                btDeleteTD.Text = "刪除";
                btDeleteTD.Attributes.Add("class", "button_continue");
                tcTablrDriven.Controls.Add(btDeleteTD);
                btDeleteTD.Attributes.Add("onclick", "DeleteTDTemplate()");

            }
            else
            {
                HtmlButton btAddTD = new HtmlButton();
                btAddTD.InnerText = "套用TableDriven關係樣版";
                btAddTD.Attributes.Add("class", "button_continue");
                tcTablrDriven.Controls.Add(btAddTD);
                btAddTD.Attributes.Add("onclick", "OpenTDTemplate()");
            }
        }
        /// <summary>
        /// 建立是否開啟Switch按鈕的Table
        /// </summary>
        /// <returns></returns>
        public void getSwitchSelectionTable(Table table, string strPaperID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcSwitchTitle = new TableCell();
            tr.Cells.Add(tcSwitchTitle);
            tcSwitchTitle.Text = "是否允許切換操作模式";

            TableCell tcYes = new TableCell();
            tr.Cells.Add(tcYes);
            tcYes.Style["width"] = "150px";

            RadioButton rbYes = new RadioButton();
            tcYes.Controls.Add(rbYes);
            rbYes.ID = "rb-Switch-Yes";
            rbYes.Text = "是";
            rbYes.Attributes["onclick"] = "";

            TableCell tcNo = new TableCell();
            tr.Cells.Add(tcNo);
            tcNo.Style["width"] = "50px";

            RadioButton rbNo = new RadioButton();
            tcNo.Controls.Add(rbNo);
            rbNo.ID = "rb-Switch-No";
            rbNo.Text = "否";
            rbNo.Attributes["onclick"] = "";

            rbYes.GroupName = "Switch";
            rbNo.GroupName = "Switch";

            bool bSwitch = DataReceiver.getSwitchableFromPaper_Header(strPaperID);

            if (bSwitch == true)
            {
                rbYes.Checked = true;
            }
            else
            {
                rbNo.Checked = true;
            }
        }

        /// <summary>
        /// 建立是否開啟Modify按鈕的Table
        /// </summary>
        /// <returns></returns>
        public void getModifySelectionTable(Table table, string strPaperID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcModifyTitle = new TableCell();
            tr.Cells.Add(tcModifyTitle);
            tcModifyTitle.Text = "是否允許修改選項";

            TableCell tcYes = new TableCell();
            tr.Cells.Add(tcYes);
            tcYes.Style["width"] = "150px";

            RadioButton rbYes = new RadioButton();
            tcYes.Controls.Add(rbYes);
            rbYes.ID = "rb-Modify-Yes";
            rbYes.Text = "是";
            rbYes.Attributes["onclick"] = "";

            TableCell tcNo = new TableCell();
            tr.Cells.Add(tcNo);
            tcNo.Style["width"] = "50px";

            RadioButton rbNo = new RadioButton();
            tcNo.Controls.Add(rbNo);
            rbNo.ID = "rb-Modify-No";
            rbNo.Text = "否";
            rbNo.Attributes["onclick"] = "";

            rbYes.GroupName = "Modify";
            rbNo.GroupName = "Modify";

            bool bModify = DataReceiver.getModifyableFromPaper_Header(strPaperID);

            if (bModify == true)
            {
                rbYes.Checked = true;
            }
            else
            {
                rbNo.Checked = true;
            }
        }

        /// <summary>
        /// 建立是否開啟Mark按鈕的Table
        /// </summary>
        /// <returns></returns>
        public void getMarkSelectionTable(Table table, string strPaperID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcMarkTitle = new TableCell();
            tr.Cells.Add(tcMarkTitle);
            tcMarkTitle.Text = "是否允許做記號";

            TableCell tcYes = new TableCell();
            tr.Cells.Add(tcYes);
            tcYes.Style["width"] = "150px";

            RadioButton rbYes = new RadioButton();
            tcYes.Controls.Add(rbYes);
            rbYes.ID = "rb-Mark-Yes";
            rbYes.Text = "是";
            rbYes.Attributes["onclick"] = "";

            TableCell tcNo = new TableCell();
            tr.Cells.Add(tcNo);
            tcNo.Style["width"] = "50px";

            RadioButton rbNo = new RadioButton();
            tcNo.Controls.Add(rbNo);
            rbNo.ID = "rb-Mark-No";
            rbNo.Text = "否";
            rbNo.Attributes["onclick"] = "";

            rbYes.GroupName = "Mark";
            rbNo.GroupName = "Mark";

            bool bMark = DataReceiver.getMarkableFromPaper_Header(strPaperID);

            if (bMark == true)
            {
                rbYes.Checked = true;
            }
            else
            {
                rbNo.Checked = true;
            }
        }

        /// <summary>
        /// 建立Assign方式的Table
        /// </summary>
        /// <returns></returns>
        public void getAssignSelectionTable(Table table, string strPaperID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcAssignTitle = new TableCell();
            tr.Cells.Add(tcAssignTitle);
            tcAssignTitle.Text = "出題順序是否固定";

            TableCell tcYes = new TableCell();
            tr.Cells.Add(tcYes);
            tcYes.Style["width"] = "150px";

            RadioButton rbYes = new RadioButton();
            tcYes.Controls.Add(rbYes);
            rbYes.ID = "rb-Assign-Yes";
            rbYes.Text = "是";
            rbYes.Attributes["onclick"] = "";

            TableCell tcNo = new TableCell();
            tr.Cells.Add(tcNo);
            tcNo.Style["width"] = "50px";

            RadioButton rbNo = new RadioButton();
            tcNo.Controls.Add(rbNo);
            rbNo.ID = "rb-Assign-No";
            rbNo.Text = "否";
            rbNo.Attributes["onclick"] = "";

            rbYes.GroupName = "Assign";
            rbNo.GroupName = "Assign";

            int intAssignMethod = DataReceiver.getAssignMethodFromPaper_Header(strPaperID);

            if (intAssignMethod == 1)
            {
                rbYes.Checked = true;
            }
            else
            {
                rbNo.Checked = true;
            }
        }

        /// <summary>
        /// 建立PresentMethod方式的Table
        /// </summary>
        /// <returns></returns>
        public void getPresentMethodSelectionTable(Table table, string strPaperID, string strCaseID, int intClinicNum, string strSectionName)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcPresentMethodTitle = new TableCell();
            tr.Cells.Add(tcPresentMethodTitle);
            tcPresentMethodTitle.Text = "呈現方式";

            TableCell tcPresentMethod = new TableCell();
            tr.Cells.Add(tcPresentMethod);
            tcPresentMethod.ColumnSpan = 2;

            DropDownList dlPresent = new DropDownList();
            tcPresentMethod.Controls.Add(dlPresent);
            dlPresent.ID = "dlPresent";
            this.setupDropDownListPresent(dlPresent, strCaseID, intClinicNum, strSectionName);
        }

        /// <summary>
        /// 建立RandomSelection方式的Table
        /// </summary>
        /// <returns></returns>
        public void getRandomSelectionSelectionTable(Table table, string strPaperID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcAssignTitle = new TableCell();
            tr.Cells.Add(tcAssignTitle);
            tcAssignTitle.Text = "選項是否隨機呈現";

            TableCell tcYes = new TableCell();
            tr.Cells.Add(tcYes);
            tcYes.Style["width"] = "150px";

            RadioButton rbYes = new RadioButton();
            tcYes.Controls.Add(rbYes);
            rbYes.ID = "rb-RandomSelection-Yes";
            rbYes.Text = "是";
            rbYes.Attributes["onclick"] = "";

            TableCell tcNo = new TableCell();
            tr.Cells.Add(tcNo);
            tcNo.Style["width"] = "50px";

            RadioButton rbNo = new RadioButton();
            tcNo.Controls.Add(rbNo);
            rbNo.ID = "rb-RandomSelection-No";
            rbNo.Text = "否";
            rbNo.Attributes["onclick"] = "";

            rbYes.GroupName = "RandomSelection";
            rbNo.GroupName = "RandomSelection";

            bool bRandomSelect = Hints.Learning.Question.DataReceiver.getRandomSelectionFromPaper_Header(strPaperID);

            if (bRandomSelect == true)
            {
                rbYes.Checked = true;
            }
            else
            {
                rbNo.Checked = true;
            }
        }

        /// <summary>
        /// 建立選取呈現方式的下拉式選單的內容
        /// </summary>
        /// <param name="dlPresent"></param>
        /// <param name="strCaseID"></param>
        /// <param name="intClinicNum"></param>
        /// <param name="strSectionName"></param>
        public void setupDropDownListPresent(DropDownList dlPresent, string strCaseID, int intClinicNum, string strSectionName)
        {
            dlPresent.Items.Clear();

            string strSQL = "SELECT sWorkType, cDescription	FROM SectionWorkType WHERE (sSectionKind = '301') ORDER BY sWorkType";
            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet ds = myDB.getDataSet(strSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i < 4 || i > 8)
                    {
                        string strWorkType = ds.Tables[0].Rows[i]["sWorkType"].ToString();
                        string strDescription = ds.Tables[0].Rows[i]["cDescription"].ToString();

                        ListItem li = new ListItem(strDescription, strWorkType);
                        dlPresent.Items.Add(li);
                    }
                }
            }
            ds.Dispose();

            //取得此Section目前正在執行的WorkType
            string strSectionWorkType = DataReceiver.getSectionWorkTypeFromUserLevelPresentBySectionName(strCaseID, intClinicNum, strSectionName);
            for (int j = 0; j < dlPresent.Items.Count; j++)
            {
                if (dlPresent.Items[j].Value == strSectionWorkType)
                {
                    dlPresent.Items[j].Selected = true;
                }
            }
        }

        /// <summary>
        /// 建立輸入測驗時間的表格
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public void getTestDurationTable(Table table, string strPaperID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcDurationTitle = new TableCell();
            tr.Cells.Add(tcDurationTitle);
            tcDurationTitle.Text = "作答時間是否有限制";

            TableCell tcYes = new TableCell();
            tr.Cells.Add(tcYes);
            tcYes.Style["width"] = "150px";

            RadioButton rbYes = new RadioButton();
            tcYes.Controls.Add(rbYes);
            rbYes.ID = "rb-Duration-Yes";
            rbYes.Text = "是" + "&nbsp;";
            rbYes.Attributes["onclick"] = "openDlDuration()";

            int intDuration = DataReceiver.getTestDurationFromPaper_Header(strPaperID);

            DropDownList dlDuration = new DropDownList();
            tcYes.Controls.Add(dlDuration);
            dlDuration.ID = "dlTestDuration";
            dlDuration.Style["width"] = "50px";
            this.setupDropDownListDuration(dlDuration, intDuration);

            Label lblMinute = new Label();
            tcYes.Controls.Add(lblMinute);
            lblMinute.Text = "&nbsp;" + "分鐘";

            TableCell tcNo = new TableCell();
            tr.Cells.Add(tcNo);
            tcNo.Style["width"] = "50px";

            RadioButton rbNo = new RadioButton();
            tcNo.Controls.Add(rbNo);
            rbNo.ID = "rb-Duration-No";
            rbNo.Text = "否";
            rbNo.Attributes["onclick"] = "closeDlDuration()";

            rbYes.GroupName = "Duration";
            rbNo.GroupName = "Duration";

            if (intDuration > 0)
            {
                rbYes.Checked = true;
                //dlDuration.Attributes["disabled"] = "";
            }
            else
            {
                rbNo.Checked = true;
                //dlDuration.Attributes["disabled"] = "disabled";
            }
        }

        /// <summary>
        /// 建立輸出是否顯示正確答案的表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strPaperID"></param>
        public void getCorrectAnswerTable(Table table, string strPaperID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcCorrectAnswerTitle = new TableCell();
            tr.Cells.Add(tcCorrectAnswerTitle);
            tcCorrectAnswerTitle.Text = "作答完成是否提供建議答案";

            TableCell tcYes = new TableCell();
            tr.Cells.Add(tcYes);
            tcYes.Style["width"] = "150px";

            RadioButton rbYes = new RadioButton();
            tcYes.Controls.Add(rbYes);
            rbYes.ID = "rb-CorrectAnswer-Yes";
            rbYes.Text = "是";
            rbYes.Attributes["onclick"] = "";

            TableCell tcNo = new TableCell();
            tr.Cells.Add(tcNo);
            tcNo.Style["width"] = "50px";

            RadioButton rbNo = new RadioButton();
            tcNo.Controls.Add(rbNo);
            rbNo.ID = "rb-CorrectAnswer-No";
            rbNo.Text = "否";
            rbNo.Attributes["onclick"] = "";

            rbYes.GroupName = "CorrectAnswer";
            rbNo.GroupName = "CorrectAnswer";

            bool bCorrectAnswer = DataReceiver.getCorrectAnswerableFromPaper_Header(strPaperID);

            if (bCorrectAnswer == true)
            {
                rbYes.Checked = true;
            }
            else
            {
                rbNo.Checked = true;
            }
        }

        /// <summary>
        /// 設定考試時間下拉式選單的內容
        /// </summary>
        /// <param name="dlDuration"></param>
        /// <param name="intDuration"></param>
        public void setupDropDownListDuration(DropDownList dlDuration, int intDuration)
        {
            dlDuration.Items.Clear();

            for (int i = 5; i <= 60; i += 5)
            {
                ListItem li = new ListItem(i.ToString(), i.ToString());
                dlDuration.Items.Add(li);
                if (i == intDuration)
                {
                    li.Selected = true;
                }
            }
        }

        /// <summary>
        /// 建立答案處理機制的表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        public void getAnsProcess(Table table, string strPaperID, string strCaseID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcAssignTitle = new TableCell();
            tr.Cells.Add(tcAssignTitle);
            tcAssignTitle.Text = "答案處理機制";

            TableCell tcTablrDriven = new TableCell();
            tr.Cells.Add(tcTablrDriven);
            tcTablrDriven.ColumnSpan = 2;


            HtmlButton btAddTD = new HtmlButton();
            btAddTD.InnerText = "答案處理機制";
            btAddTD.Attributes.Add("class", "button_continue");
            tcTablrDriven.Controls.Add(btAddTD);
            btAddTD.Attributes.Add("onclick", "OpenAnsProcessTool()");
        }

        /// <summary>
        /// 建立情境題模式選擇的表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        public void getQuestionSituationMode(Table table, string strPaperID, string strCaseID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcSituationModeTitle = new TableCell();
            tr.Cells.Add(tcSituationModeTitle);
            //tcSituationModeTitle.Text = "情境題的模式類型";

            Label lbTitle = new Label();
            lbTitle.Text = "情境題的模式類型";           

            Label lbNote = new Label();
            lbNote.Text = "(若無情境題則此選項不必選擇)";
            lbNote.Font.Size = 9;
            lbNote.ForeColor = System.Drawing.Color.Red;

            tcSituationModeTitle.Controls.Add(lbTitle);
            tcSituationModeTitle.Controls.Add(lbNote);
            
            TableCell tcExam = new TableCell();
            tr.Cells.Add(tcExam);
            //tcExam.Style["width"] = "100px";

            RadioButton rbExam = new RadioButton();
            tcExam.Controls.Add(rbExam);
            rbExam.ID = "rb-SituationMode-Exam";
            rbExam.Text = "考試模式";
            rbExam.Attributes["onclick"] = "";

            TableCell tcPractice = new TableCell();
            tr.Cells.Add(tcPractice);
            //tcPractice.Style["width"] = "100px";

            RadioButton rbPractice = new RadioButton();
            tcPractice.Controls.Add(rbPractice);
            rbPractice.ID = "rb-SituationMode-Practice";
            rbPractice.Text = "教學模式";
            rbPractice.Attributes["onclick"] = "";

            rbExam.GroupName = "SituationMode";
            rbPractice.GroupName = "SituationMode";

            bool IsExamMode = DataReceiver.getIsSituationModeFromPaper_Header(strPaperID);

            if (IsExamMode == true)
            {
                rbExam.Checked = true;
            }
            else
            {
                rbPractice.Checked = true;
            }
        }

        /// <summary>
        /// 建立批改考卷時是否會顯示學生姓名的選項表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        public void getStudentNameVisibleMode(Table table, string strPaperID, string strCaseID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcStudentNameModeTitle = new TableCell();
            tr.Cells.Add(tcStudentNameModeTitle);

            Label lbTitle = new Label();
            lbTitle.Text = "批改考卷時是否顯示學生姓名";

            Label lbNote = new Label();
            lbNote.Text = "(若無須批改考卷則此選項不必選擇)";
            lbNote.Font.Size = 9;
            lbNote.ForeColor = System.Drawing.Color.Red;

            tcStudentNameModeTitle.Controls.Add(lbTitle);
            tcStudentNameModeTitle.Controls.Add(lbNote);

            TableCell tcStudentNameVisible = new TableCell();
            tr.Cells.Add(tcStudentNameVisible);
            //tcExam.Style["width"] = "100px";

            RadioButton rbVisible = new RadioButton();
            tcStudentNameVisible.Controls.Add(rbVisible);
            rbVisible.ID = "rb-Visible";
            rbVisible.Text = "是";
            rbVisible.Attributes["onclick"] = "";

            TableCell tcStudentNameInvisible = new TableCell();
            tr.Cells.Add(tcStudentNameInvisible);
            //tcPractice.Style["width"] = "100px";

            RadioButton rbInvisible = new RadioButton();
            tcStudentNameInvisible.Controls.Add(rbInvisible);
            rbInvisible.ID = "rb-Invisible";
            rbInvisible.Text = "否";
            rbInvisible.Attributes["onclick"] = "";

            rbVisible.GroupName = "StudentNameMode";
            rbInvisible.GroupName = "StudentNameMode";

            bool IsStudentNameVisible = DataReceiver.getIsStudentNameVisibleFromPaper_Header(strPaperID);

            if (IsStudentNameVisible == true)
            {
                rbVisible.Checked = true;
            }
            else
            {
                rbInvisible.Checked = true;
            }
        }

        /// <summary>
        /// 建立TableDriven的Table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strPaperID"></param>
        public void getTableDriven(Table table, string strPaperID, string strCaseID)
        {
            TableRow tr = new TableRow();
            table.Rows.Add(tr);

            TableCell tcAssignTitle = new TableCell();
            tr.Cells.Add(tcAssignTitle);
            tcAssignTitle.Text = "套用Table Driven關係";

            TableCell tcTablrDriven = new TableCell();
            tr.Cells.Add(tcTablrDriven);
            tcTablrDriven.ColumnSpan = 2;

            DataTable dtTD_QandA = Hints.DB.TableDriven.TD_QandA.TD_QandA_SELECT(strPaperID, strCaseID);
            if (dtTD_QandA.Rows.Count > 0)
            {
                Label lbTDTemplate = new Label();
                lbTDTemplate.Text = "Table Driven樣版名稱: " + Hints.DB.TableDriven.clsTableDriven.TDTemplate_SELECT(dtTD_QandA.Rows[0]["cTDTemplateID"].ToString()).Rows[0]["cTDTemplateName"].ToString() + "&nbsp;&nbsp;";
                tcTablrDriven.Controls.Add(lbTDTemplate);

                HtmlButton btModifyTD = new HtmlButton();
                btModifyTD.InnerText = "修改";
                btModifyTD.Attributes.Add("class", "button_continue");
                tcTablrDriven.Controls.Add(btModifyTD);
                btModifyTD.Attributes.Add("onclick", "OpenTDTemplate()");

                Label lbSpace = new Label();
                lbSpace.Text = "&nbsp;&nbsp;";
                tcTablrDriven.Controls.Add(lbSpace);

                Button btDeleteTD = new Button();
                btDeleteTD.Text = "刪除";
                btDeleteTD.Attributes.Add("class", "button_continue");
                tcTablrDriven.Controls.Add(btDeleteTD);
                btDeleteTD.Attributes.Add("onclick", "DeleteTDTemplate()");

            }
            else
            {
                HtmlButton btAddTD = new HtmlButton();
                btAddTD.InnerText = "套用TableDriven關係樣版";
                btAddTD.Attributes.Add("class", "button_continue");
                tcTablrDriven.Controls.Add(btAddTD);
                btAddTD.Attributes.Add("onclick", "OpenTDTemplate()");
            }


        }

        /// <summary>
        /// 建立一個問題表格
        /// </summary>
        public Table setupSuggestedQuestionTableByPaperID(string strPaperID, string strCaseID, int intClinicNum, string strSectionName)
        {
            DataReceiver myReceiver = new DataReceiver();
            clsPresentTable myTable = new clsPresentTable();
            SQLString mySQL = new SQLString();

            //取得此問卷的所有題目(Paper_Content)
            int intContentQuestionCount = myReceiver.getPaperContentQuestionNum(strPaperID);

            //取得此問卷的所有題目(Paper_RandomQuestionNum)
            int intRandomQuestionCount = myReceiver.getTotalQuestionNumFromRandomQuestion(strPaperID);

            //取得此問卷的所有題目總數
            int intTotalQuestion = intContentQuestionCount + intRandomQuestionCount;

            //目前顯示的題號
            int intQuestionIndex = 1;

            //建立問卷的本體表格
            Table table = new Table();
            table.Style.Add("WIDTH", "98%");

            //建立此問卷中的所有問題
            for (int i = 1; i <= intContentQuestionCount; i++)
            {
                TableRow tr = new TableRow();
                table.Rows.Add(tr);

                TableCell tc = new TableCell();
                tr.Cells.Add(tc);

                //取得問題的資料(From Paper_Content)
                DataRow[] drQuestion = myReceiver.getQIDFromPaper_ContentByQuestionIndex(strPaperID, intQuestionIndex);

                if (drQuestion.Length > 0)
                {
                    //取得要顯示問題的QID
                    string strQID = drQuestion[0]["cQID"].ToString();

                    //QuestionType
                    string strQuestionType = drQuestion[0]["cQuestionType"].ToString();

                    //QuestionMode
                    string strQuestionMode = drQuestion[0]["cQuestionMode"].ToString();

                    //問題是選擇題還是問答題?
                    switch (drQuestion[0]["cQuestionType"].ToString())
                    {
                        case "2":
                            //問答題

                            //呼叫建立問答題表格的Function
                            break;
                        default:
                            //選擇題

                            tc.Controls.Add(this.setupSingleSelectionQuestionTableByPlainView(strPaperID, strCaseID, intClinicNum, strSectionName, strQID, intQuestionIndex));
                            break;
                    }
                }

                intQuestionIndex += 1;
            }

            return table;
        }

        /// <summary>
        /// 建立一個可以顯示建議與非建議選項選擇題的表格
        /// </summary>
        /// <returns></returns>
        public Table setupSingleSelectionQuestionTableByPlainView(string strPaperID, string strCaseID, int intClinicNum, string strSectionName, string strQID, int intQuestionIndex)
        {
            DataReceiver myReceiver = new DataReceiver();

            Table table = new Table();

            //題目
            TableRow trQuestion = new TableRow();
            table.Rows.Add(trQuestion);

            //題號
            TableCell tcNum = new TableCell();
            trQuestion.Cells.Add(tcNum);
            tcNum.Text = intQuestionIndex.ToString();
            tcNum.Attributes.Add("align", "center");
            tcNum.Style.Add("WIDTH", "10px");

            //Question
            string strQuestion = myReceiver.getSelectionQuestionContent(strQID);
            TableCell tcQuestion = new TableCell();
            trQuestion.Cells.Add(tcQuestion);
            tcQuestion.Text = strQuestion;

            //設定回饋機制
            TableCell tcRetry = new TableCell();
            trQuestion.Cells.Add(tcRetry);
            tcRetry.Attributes["align"] = "center";
            tcRetry.Style["width"] = "220px";

            HtmlInputButton btnRetry = new HtmlInputButton("button");
            tcRetry.Controls.Add(btnRetry);
            btnRetry.Style["width"] = "150px";
            btnRetry.Style["height"] = "30px";
            btnRetry.Value = "設定回饋機制";
            btnRetry.Attributes.Add("class", "button_continue");
            btnRetry.Attributes["onclick"] = "callRetrySetting('" + strPaperID + "' , '" + strCaseID + "' , '" + intClinicNum + "' , '" + strSectionName + "' , '" + strQID + "')";

            //選項
            SQLString mySQL = new SQLString();
            string strSQL = mySQL.getAllSelections(strQID);

            SqlDB myDB = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            DataSet dsSelection = myDB.getDataSet(strSQL);
            if (dsSelection.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsSelection.Tables[0].Rows.Count; i++)
                {
                    TableRow trSelection = new TableRow();
                    table.Rows.Add(trSelection);

                    string strSelectionID = dsSelection.Tables[0].Rows[i]["cSelectionID"].ToString();
                    int intSeq = Convert.ToInt32(dsSelection.Tables[0].Rows[i]["sSeq"]);
                    string strSelection = dsSelection.Tables[0].Rows[i]["cSelection"].ToString();
                    bool bSuggested = Convert.ToBoolean(dsSelection.Tables[0].Rows[i]["bCaseSelect"]);

                    //顯示建議或是非建議選項的圖片
                    TableCell tcControl = new TableCell();
                    trSelection.Cells.Add(tcControl);
                    tcControl.Attributes.Add("align", "center");
                    tcControl.Style.Add("WIDTH", "10px");

                    //img
                    HtmlImage img = new HtmlImage();
                    tcControl.Controls.Add(img);
                    if (bSuggested == true)
                    {
                        //Suggested
                        img.Src = "/Hints/Summary/Images/smiley4.gif";
                    }
                    else
                    {
                        //Un-Suggested
                        img.Src = "/Hints/Summary/Images/smiley11.gif";
                    }

                    //Selection
                    TableCell tcSelection = new TableCell();
                    trSelection.Cells.Add(tcSelection);
                    tcSelection.Text = strSelection;

                    //NextStep
                    TableCell tcNextStep = new TableCell();
                    trSelection.Cells.Add(tcNextStep);
                    tcNextStep.Style["width"] = "220px";
                    tcNextStep.Controls.Add(this.getNextStepTable(strPaperID, strCaseID, intClinicNum, strSectionName, strQID, strSelectionID, bSuggested, intQuestionIndex));

                }
            }
            dsSelection.Dispose();

            //設定外觀
            TableAttribute.setupTopHeaderTableStyle(table, "TableName", 1, "100%", 5, 0, GridLines.Both, true);
            return table;
        }

        /// <summary>
        /// 建立顯示某問卷隨機出題的清單
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <returns></returns>
        public Table getListTableFromRandomNum(string strPaperID)
        {
            Table table = new Table();

            //取得RandomNum的資料
            DataTable dtRandom = SQLString.getPaperInformationFromPaper_RandomQuestionNumWithGroupName(strPaperID);

            if (dtRandom.Rows.Count > 0)
            {
                //建立標頭欄位
                TableRow trTitle = new TableRow();
                table.Rows.Add(trTitle);

                //QuestionGroupName
                TableCell tcGroupNameTitle = new TableCell();
                trTitle.Cells.Add(tcGroupNameTitle);
                tcGroupNameTitle.Text = "Group name";

                //QustionNum
                TableCell tcQustionNumTitle = new TableCell();
                trTitle.Cells.Add(tcQustionNumTitle);
                tcQustionNumTitle.Text = "Question number";

                for (int i = 0; i < dtRandom.Rows.Count; i++)
                {
                    TableRow tr = new TableRow();
                    table.Rows.Add(tr);

                    //QuestionGroupName
                    TableCell tcGroupName = new TableCell();
                    tr.Cells.Add(tcGroupName);
                    tcGroupName.Text = dtRandom.Rows[i]["cNodeName"].ToString();

                    //QustionNum
                    TableCell tcQuestionNum = new TableCell();
                    tr.Cells.Add(tcQuestionNum);
                    tcQuestionNum.Style["width"] = "180px";
                    tcQuestionNum.Attributes["align"] = "center";
                    tcQuestionNum.Text = dtRandom.Rows[i]["sQuestionNum"].ToString();
                }
            }

            //CSS
            TableAttribute.setupTopHeaderTableStyle(table, "TableName", 1, "60%", 5, 0, GridLines.Both, true);

            return table;
        }

        /// <summary>
        /// 建立設定NextStep的控制項表格
        /// </summary>
        /// <param name="strPaperID"></param>
        /// <param name="strCaseID"></param>
        /// <param name="intClinicNum"></param>
        /// <param name="strSectionName"></param>
        /// <returns></returns>
        public Table getNextStepTable(string strPaperID, string strCaseID, int intClinicNum, string strSectionName, string strQID, string strSelectionID, bool bSuggested, int intQuestionIndex)
        {
            Table table = new Table();

            //Default
            TableRow trDefault = new TableRow();
            table.Rows.Add(trDefault);

            TableCell tcDefaultTitle = new TableCell();
            trDefault.Cells.Add(tcDefaultTitle);
            tcDefaultTitle.ColumnSpan = 2;

            RadioButton rbDefault = new RadioButton();
            tcDefaultTitle.Controls.Add(rbDefault);
            rbDefault.ID = "rbDefault-" + strSelectionID;
            rbDefault.GroupName = strSelectionID;
            rbDefault.Text = "Default: ";
            rbDefault.Attributes["onclick"] = "EnableDlDefaultBySelectionID('" + strSelectionID + "')";

            //題號
            TableRow trIndex = new TableRow();
            table.Rows.Add(trIndex);

            TableCell tcIndexTitle = new TableCell();
            trIndex.Cells.Add(tcIndexTitle);

            RadioButton rbIndex = new RadioButton();
            tcIndexTitle.Controls.Add(rbIndex);
            rbIndex.ID = "rbIndex-" + strSelectionID;
            rbIndex.GroupName = strSelectionID;
            rbIndex.Text = "題號: ";
            rbIndex.Attributes["onclick"] = "EnableDlIndexBySelectionID('" + strSelectionID + "')";

            TableCell tcIndex = new TableCell();
            trIndex.Cells.Add(tcIndex);

            DropDownList dlIndex = new DropDownList();
            this.setupQuestionIndexDropDownList(dlIndex, strPaperID, strQID, strSelectionID, bSuggested, intQuestionIndex);
            tcIndex.Controls.Add(dlIndex);
            dlIndex.ID = "dlSelection-Index-" + strSelectionID;

            //Section
            TableRow trSection = new TableRow();
            table.Rows.Add(trSection);

            TableCell tcSectionTitle = new TableCell();
            trSection.Cells.Add(tcSectionTitle);

            RadioButton rbSection = new RadioButton();
            tcSectionTitle.Controls.Add(rbSection);
            rbSection.ID = "rbSection-" + strSelectionID;
            rbSection.GroupName = strSelectionID;
            rbSection.Text = "Section: ";
            rbSection.Attributes["onclick"] = "EnableDlSectionBySelectionID('" + strSelectionID + "')";

            TableCell tcSection = new TableCell();
            trSection.Cells.Add(tcSection);

            DropDownList dlSection = new DropDownList();
            this.setupSectionList(dlSection, strCaseID, intClinicNum, strSectionName, strPaperID, strQID, strSelectionID);
            tcSection.Controls.Add(dlSection);
            dlSection.ID = "dlSelection-Section-" + strSelectionID;

            //取得此選項的NextStepMethod
            int intNextMethod = DataReceiver.getNextMethodFromPaperContent(strPaperID, strQID, strSelectionID);
            switch (intNextMethod)
            {
                case 0:
                    //結束此Section
                    rbIndex.Checked = true;
                    break;
                case 1:
                    //Section
                    rbSection.Checked = true;
                    break;
                case 2:
                    //題號
                    rbIndex.Checked = true;
                    break;
                default:
                    //Default
                    rbDefault.Checked = true;
                    break;
            }

            if (rbIndex.Checked == true)
            {
                //使用題號

                //關閉Section的下拉式選單
                dlSection.Attributes["disabled"] = "disabled";
            }
            else if (rbSection.Checked == true)
            {
                //使用Section

                //關閉題號的下拉式選單
                dlIndex.Attributes["disabled"] = "disabled";
            }
            else
            {
                //Default

                //關閉題號的下拉式選單
                dlIndex.Attributes["disabled"] = "disabled";

                //關閉Section的下拉式選單
                dlSection.Attributes["disabled"] = "disabled";
            }

            return table;
        }

        /// <summary>
        /// 建立一個Section的下拉式選單
        /// </summary>
        /// <param name="strCaseID"></param>
        /// <param name="intClinicNum"></param>
        /// <param name="strSectionName"></param>
        public void setupSectionList(DropDownList dlSection, string strCaseID, int intClinicNum, string strSectionName, string strPaperID, string strQID, string strSelectionID)
        {
            dlSection.Items.Clear();

            //取得此Section在UserLevelPresent的DivisionID
            string strDivisionID = DataReceiver.getDivisionIDFromUserLevelPresentBySectionName(strCaseID, intClinicNum, strSectionName);

            //取得此選項是否有被Assign某個Section
            string strNextSection = DataReceiver.getNextSectionNameFromNextStepBySelectionID(strPaperID, strQID, strSelectionID);

            SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

            string strSQL = "SELECT cSectionName FROM UserLevelPresent WHERE cCaseID = '" + strCaseID + "' AND sClinicNum = '" + intClinicNum.ToString() + "' AND cDivisionID = '" + strDivisionID + "' ORDER BY sSectionSeq";

            DataSet dsSection = sqldb.getDataSet(strSQL);
            if (dsSection.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsSection.Tables[0].Rows.Count; i++)
                {
                    string strSection = dsSection.Tables[0].Rows[i]["cSectionName"].ToString();
                    ListItem li = new ListItem(strSection, strSection);

                    //不顯示此問卷所屬的Section
                    if (strSection != strSectionName)
                    {
                        dlSection.Items.Add(li);
                    }

                    if (strSection == strNextSection)
                    {
                        li.Selected = true;
                    }
                }
            }
            dsSection.Dispose();
        }

        /// <summary>
        /// 建立一個顯示題號的下拉式選單
        /// </summary>
        /// <param name="dlIndex"></param>
        /// <param name="intQuestionIndex"></param>
        public void setupQuestionIndexDropDownList(DropDownList dlIndex, string strPaperID, string strQID, string strSelectionID, bool bSuggested, int intQuestionIndex)
        {
            dlIndex.Items.Clear();

            DataReceiver myReceiver = new DataReceiver();

            //取得此問卷的所有題目(Paper_Content)
            int intContentQuestionCount = myReceiver.getPaperContentQuestionNum(strPaperID);

            //取得此問卷所有的問題總數
            int intTotalQuestionNum = myReceiver.getPaperAllQuestionCountFromContentAndRandom(strPaperID);

            SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);

            //建立所有提號
            for (int i = 1; i <= intContentQuestionCount; i++)
            {
                ListItem li = new ListItem(i.ToString(), i.ToString());
                dlIndex.Items.Add(li);
            }

            //建立Finish
            ListItem liFinish = new ListItem("Finish", "0");
            dlIndex.Items.Add(liFinish);

            //取得此題目是否有設定下一步的題號
            int intNextIndex = DataReceiver.getNextQuestionSeqFromNextStepBySelectionID(strPaperID, strQID, strSelectionID);

            if (intNextIndex == 999)
            {
                //沒有設定下一步的題號

                //檢查此題目是否最後一題
                if (intQuestionIndex == intTotalQuestionNum)
                {
                    //Yes，顯示Finish
                    dlIndex.Items[dlIndex.Items.Count - 1].Selected = true;

                    //此選項是否為建議選項
                    if (bSuggested == true)
                    {
                        //Disable
                        dlIndex.Attributes["disabled"] = "disabled";
                    }
                }
                else
                {
                    //No，顯示下一題的題號
                    dlIndex.Items[intQuestionIndex].Selected = true;
                }
            }
            else
            {
                //有設定下一步的題號

                //顯示已經有設定的題號
                for (int j = 0; j < dlIndex.Items.Count; j++)
                {
                    if (dlIndex.Items[j].Value == intNextIndex.ToString())
                    {
                        dlIndex.Items[j].Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// 傳回Paper_RandomNum的組別問題數目表格(不做動作)
        /// </summary>
        private Table setupQuestionNumTable(string strPaperID)
        {
            Table table = new Table();
            table.Attributes.Add("Class", "TableName");
            table.Style.Add("WIDTH", "100%");
            table.CellSpacing = 0;
            table.CellPadding = 2;
            table.BorderWidth = Unit.Pixel(1);
            table.BorderStyle = BorderStyle.Solid;
            table.BorderColor = System.Drawing.Color.Black;

            SQLString mySQL = new SQLString();
            DataReceiver myReceiver = new DataReceiver();

            //從Paper_RandomQuestionNum取出此問卷的資料

            SqlDB sqldb = new SqlDB(System.Configuration.ConfigurationSettings.AppSettings["connstr"]);
            string strSQL = mySQL.getPaper_RandomQuestionNum(strPaperID);
            DataSet dsQuestionNum = sqldb.getDataSet(strSQL);

            if (dsQuestionNum.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsQuestionNum.Tables[0].Rows.Count; i++)
                {

                    //設定Table的Style
                    string strStyle = "header1_tr_odd_row";
                    if ((i % 2) != 0)
                    {
                        strStyle = "header1_tr_odd_row";
                    }
                    else
                    {
                        strStyle = "header1_tr_even_row";
                    }

                    TableRow tr = new TableRow();
                    tr.CssClass = strStyle;
                    table.Rows.Add(tr);

                    //get GroupID
                    string strGroupID = "";
                    try
                    {
                        strGroupID = dsQuestionNum.Tables[0].Rows[i]["cQuestionGroupID"].ToString();

                    }
                    catch
                    {
                    }

                    //get Question number
                    int intQuestionNum = 0;
                    try
                    {
                        intQuestionNum = Convert.ToInt32(dsQuestionNum.Tables[0].Rows[i]["sQuestionNum"]);

                    }
                    catch
                    {
                    }

                    //get 此組別全部的問題數目
                    int intQuestionCount = 0;
                    if (strGroupID == "Specific")
                    {
                        strSQL = mySQL.getSpecificSelectionQuestion(strPaperID);
                    }
                    else
                    {
                        strSQL = mySQL.getGroupSelectionQuestion(strGroupID);
                    }

                    DataSet dsQuestionCount = sqldb.getDataSet(strSQL);
                    try
                    {
                        intQuestionCount = dsQuestionCount.Tables[0].Rows.Count;
                    }
                    catch
                    {
                    }
                    dsQuestionCount.Dispose();

                    //組別名稱
                    TableCell tcGroupName = new TableCell();
                    tr.Cells.Add(tcGroupName);

                    string strGroupName = "";
                    if (strGroupID == "Specific")
                    {
                        strGroupName = "Specific questions";
                    }
                    else
                    {
                        strGroupName = mySQL.getQuestionGroupName(strGroupID);
                    }
                    tcGroupName.Text = strGroupName;

                    //問題個數
                    TableCell tcQuestionNum = new TableCell();
                    tr.Cells.Add(tcQuestionNum);

                    TextBox txtQuestionNum = new TextBox();
                    tcQuestionNum.Controls.Add(txtQuestionNum);
                    txtQuestionNum.ID = "txt" + strGroupID;
                    txtQuestionNum.Text = intQuestionNum.ToString();

                    //此組別的問題總數
                    TableCell tcQuestionCount = new TableCell();
                    tr.Cells.Add(tcQuestionCount);

                    tcQuestionCount.Text = intQuestionCount.ToString();
                    tcQuestionCount.Attributes.Add("Align", "Center");

                    //建立Title的TextArea
                    TableRow trQuestionTitle = new TableRow();
                    table.Rows.Add(trQuestionTitle);

                    TableCell tcTitle = new TableCell();
                    trQuestionTitle.Cells.Add(tcTitle);
                    tcTitle.Attributes.Add("align", "right");
                    tcTitle.ColumnSpan = 3;

                    //建立Question title的TextArea
                    HtmlTextArea txtTitle = new HtmlTextArea();
                    tcTitle.Controls.Add(txtTitle);
                    txtTitle.ID = "txtTitle" + strGroupID;
                    txtTitle.Style.Add("WIDTH", "100%");
                    txtTitle.Rows = 5;
                    txtTitle.Style.Add("DISPLAY", "none");

                    //取出此QuestionTitle的內容
                    txtTitle.InnerText = myReceiver.getQuestionTitle(strPaperID, strGroupID);

                    //建立Question title button
                    HtmlInputButton btnTitle = new HtmlInputButton("button");
                    tcTitle.Controls.Add(btnTitle);
                    btnTitle.ID = "btnTitle" + strGroupID;
                    btnTitle.Value = "Add a question title";
                    btnTitle.Attributes.Add("onclick", "showQuestionTitle('" + strGroupID + "')");

                    //建立間隔
                    Label lblCell0 = new Label();
                    lblCell0.Style.Add("WIDTH", "20px");
                    tcTitle.Controls.Add(lblCell0);

                    //建立Empty row
                    if (i < dsQuestionNum.Tables[0].Rows.Count - 1)
                    {
                        TableRow trEmpty = new TableRow();
                        table.Rows.Add(trEmpty);
                        trEmpty.BackColor = System.Drawing.Color.White;

                        TableCell tcEmpty1 = new TableCell();
                        trEmpty.Cells.Add(tcEmpty1);
                        tcEmpty1.Style.Add("Height", "10px");
                        tcEmpty1.ColumnSpan = 3;
                    }
                }

                //加入Table的Title
                TableRow trTitle = new TableRow();
                table.Rows.AddAt(0, trTitle);
                trTitle.Attributes.Add("Class", "header1_table_first_row");

                TableCell tcGroupTitle = new TableCell();
                trTitle.Cells.Add(tcGroupTitle);
                tcGroupTitle.Text = "Question topic";

                TableCell tcQuestionTitle = new TableCell();
                trTitle.Cells.Add(tcQuestionTitle);
                tcQuestionTitle.Text = "Selected questions number";

                TableCell tcQuestionCountTitle = new TableCell();
                trTitle.Cells.Add(tcQuestionCountTitle);
                tcQuestionCountTitle.Text = "Maximum questions count";
            }
            else
            {
                //沒有資料
            }
            dsQuestionNum.Dispose();

            return table;
        }
    }
}
