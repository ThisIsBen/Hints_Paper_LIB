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

namespace PaperSystem
{
	/// <summary>
	/// TableAttribute 的摘要描述。
	/// </summary>
	public class TableAttribute
	{
		public TableAttribute()
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
		}

		public void addHyperLink(TableCell tc)
		{
			HyperLink hLink = new HyperLink();
			hLink.NavigateUrl = "ShowMember.aspx?";
			tc.Controls.Add(hLink);
		}

//-----------------------------此函數為設定表格的外觀--------------------------------
		public static void setupTopHeaderTableStyle(Table table , string strCssStyle , int intBorderWidth , string strTableWidth , int intCellPadding , int intCellSpacing , GridLines styleGridLines,bool bIsHaveHeader)
		{
			table.CssClass = strCssStyle;
			table.BorderWidth = Unit.Pixel(intBorderWidth);
			table.Style.Add("WIDTH",strTableWidth);
			table.CellPadding = intCellPadding;
			table.CellSpacing = intCellSpacing;
			table.GridLines = styleGridLines;
			table.BorderColor = System.Drawing.Color.Black;

			if(table.Rows.Count > 0)
			{
				int startIndex = 0;
				if(bIsHaveHeader)
				{
					table.Rows[0].Attributes.Add("Class","header1_table_first_row");
					startIndex = 1;
				}
				if(table.Rows.Count > 0)
				{
					for(int i=startIndex ; i<table.Rows.Count ; i++)
					{
						if(i%2 == 0)
						{
							table.Rows[i].Attributes.Add("Class","header1_tr_odd_row");	
						}
						else
						{
							table.Rows[i].Attributes.Add("Class","header1_tr_even_row");	
						}
					}
				}
			}
			/*
			if(ta.Rows.Count != 0)
			{
				if(ta.Rows.Count > 0)
				{
					ta.Attributes.Add("width", "100%");
					ta.Attributes.Add("Class","header1_table");
					//----------------------設定表格的顏色-------------------------
					for (int i=1 ; i<ta.Rows.Count ; i++)
					{
						if((i % 2)!=0)
						{
							//ta.Rows[i].BackColor = System.Drawing.Color.LightCyan;
							ta.Rows[i].Attributes.Add("Class","header1_tr_odd_row");	
						}
						else
						{
							ta.Rows[i].Attributes.Add("Class","header1_tr_even_row");	
						}
					}

					if(bTitle == true)
					{
						ta.Rows[0].Attributes.Add("Class","header1_table_first_row");
					}
				}
			}
			*/
		}

		/// <summary>
		/// 設定橫向表格的外觀 (標題 內容 標題 內容)
		/// </summary>
		/// <param name="table"></param>
		public void setupTransverseTableStyle(Table table)
		{
			table.CssClass = "header1_table";
			table.Style.Add("WIDTH","100%");

			bool bOddEven = true;
			for(int i=0 ; i<table.Rows[0].Cells.Count ; i++)
			{
				if(i%2 == 0)
				{
					table.Rows[0].Cells[i].CssClass = "header1_table_first_row";
				}
				else
				{
					if(bOddEven == true)
					{
						table.Rows[0].Cells[i].CssClass = "header1_tr_even_row";
					}
					else
					{
						table.Rows[0].Cells[i].CssClass = "header1_tr_odd_row";
					}

					bOddEven = !bOddEven;
				}
			}
		}

		public void setCellTableStyle(Table ta)
		{
			ta.Attributes.Add("width", "100%");
			ta.Attributes.Add("Class","header1_table");
			//----------------------設定表格的顏色-------------------------
			for (int i=0 ; i<ta.Rows.Count ; i++)
			{
					if((i % 2)!=0)
				{
					ta.Rows[i].Attributes.Add("Class","header1_tr_odd_row");	
				}
					else
				{
					ta.Rows[i].Attributes.Add("Class","header1_tr_even_row");
				}
			}
			
		}

//--設定內嵌表格的外觀
		public void setInsideTableStyle(Table ta)
		{
			if(ta.Rows.Count != 0)
			{
				ta.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
				ta.GridLines = GridLines.Both;
				//ta.Style.Add("text-align","center");//Table row 的text 置中
				ta.CellSpacing = 0;
				ta.CellPadding = 0;
				ta.BorderWidth = 3;
				ta.BorderColor = Color.Black;//FromName("#000000");
				ta.Attributes.Add("width", "99%");
				ta.Rows[0].Font.Bold = true;
				//----------------------設定表格的顏色-------------------------
				for (int i=1 ; i<ta.Rows.Count ; i++)
				{
					if((i % 2)!=0)
					{
						ta.Rows[i].BackColor = System.Drawing.Color.LightCyan;
					}
					else
					{
						ta.Rows[i].BackColor = System.Drawing.Color.MistyRose;
					}
				}
			}
		}

		//-----------------------------此函數為設定DataGrid的外觀--------------------------------
		public void setDataGridStyle(DataGrid ta)
		{
			
			ta.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
			ta.GridLines = GridLines.Both;
			ta.Style.Add("text-align","center");//Table row 的text 置中
			ta.Font.Size = System.Web.UI.WebControls.FontUnit.Larger;
			ta.CellSpacing = 0;
			ta.CellPadding = 0;
			ta.BorderWidth = 3;
			ta.BorderColor = Color.Black;//FromName("#000000");
			
			ta.Attributes.Add("width", "100%");

			//ta.Rows[0].Font.Bold = true;
			//----------------------設定表格的顏色-------------------------
			ta.ItemStyle.BackColor = System.Drawing.Color.LightCyan;
			ta.AlternatingItemStyle.BackColor = System.Drawing.Color.MistyRose;
			//------------------------標題的顏色----------------------------
			ta.HeaderStyle.BackColor = System.Drawing.Color.LightGoldenrodYellow;
		}

		public void addTitle(TableRow tr , string strTitle)
		{
			//--在傳進來的TableRow中加入一個Title--//
			TableCell tc = new TableCell();
			tc.Attributes.Add("Align","Center");
			tc.Text = strTitle;
			tr.Cells.Add(tc);
		}

//---------------------------此函數為在DataRow中新增一個Cell----------------------------

		public void addNewCell(TableRow trNew , DataSet dsNew , string DataSetheader1_table , string ColumnName , int i)
		{
			TableCell tcNew = new TableCell();
			tcNew.Text = dsNew.Tables[0].Rows[i][ColumnName].ToString();
			trNew.Cells.Add(tcNew);
		}

//------------------------此函數為在DataRow中新增一個自訂的Cell------------------------

		public void addEmptyCell(TableRow trEmpty , string strText)
		{
			TableCell tcEmpty = new TableCell();
			tcEmpty.Text = strText;
			trEmpty.Cells.Add(tcEmpty);
		}
//-----------------------此函數為新增一個TableCell輸入為一個字串-----------------------
		public void addCellText(TableRow trNew , string strText)
		{
			TableCell tcNew = new TableCell();
			tcNew.Text = strText;
			trNew.Cells.Add(tcNew);
		}
	}
}
