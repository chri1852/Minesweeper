using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace Minesweeper
{
	class OptionsMenu
	{
		private Form OptionsForm;
		private OptionsMenuData UserChoices;
		private OptionsMenuData OriginalChoices;
		public event EventHandler<OptionMenuEventArgs> OptionsReturn;
		
		public OptionsMenu() : this(9, 9, 10) {}
		public OptionsMenu(int height, int width) : this(height, width, 10) {}
		public OptionsMenu(int height, int width, int numMines)
		{
			// Set the user choices to the default
			UserChoices = new OptionsMenuData(width, height, numMines);
			OriginalChoices = new OptionsMenuData(width, height, numMines);
			
			// Now build the form
			OptionsForm = new Form();
			OptionsForm.MaximizeBox = false;
			OptionsForm.FormBorderStyle = FormBorderStyle.FixedDialog;
			OptionsForm.Name = "OptionsForm";
			OptionsForm.Text = "Minesweeper";
			OptionsForm.ClientSize = new Size(200, 235);
			OptionsForm.Icon = GameResources.GetIconImage();
			OptionsForm.KeyPreview = true;
			OptionsForm.KeyPress += new KeyPressEventHandler(MainFormKeyStroke);
			
			Label OptionsLabel = new Label();
			OptionsLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("14"), FontStyle.Bold);
			OptionsLabel.Text = "Options";
			OptionsLabel.TextAlign = ContentAlignment.MiddleCenter;
			OptionsLabel.Size = new Size(190, 30);
			OptionsLabel.Location = new Point(5, 5);
			OptionsForm.Controls.Add(OptionsLabel);
			
			Label OptionsLabelLine = new Label();
			OptionsLabelLine.Size = new Size(190, 2);
			OptionsLabelLine.Location = new Point(5, 37);
			OptionsLabelLine.BorderStyle = BorderStyle.FixedSingle;
			OptionsForm.Controls.Add(OptionsLabelLine);
			
			//Add the Easy, Medium, Hard, and Custom Radio Buttons
			
			// Easy
			Label EasyLabel = new Label();
			EasyLabel.Text = "Easy";
			EasyLabel.Size = new Size(60, 25);
			EasyLabel.Location = new Point(45, 43);
			EasyLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(EasyLabel);
			RadioButton EasyRadio = new RadioButton();
			EasyRadio.Name = "EasyRadio";
			EasyRadio.Location = new Point(130, 40);
			EasyRadio.CheckedChanged += EasyRadio_CheckedChanged;
			OptionsForm.Controls.Add(EasyRadio);
			
			// Medium
			Label MediumLabel = new Label();
			MediumLabel.Text = "Medium";
			MediumLabel.Size = new Size(60, 25);
			MediumLabel.Location = new Point(45, 68);
			MediumLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(MediumLabel);
			RadioButton MediumRadio = new RadioButton();
			MediumRadio.Name = "MediumRadio";
			MediumRadio.Location = new Point(130, 65);
			MediumRadio.CheckedChanged += MediumRadio_CheckedChanged;
			OptionsForm.Controls.Add(MediumRadio);
			
			// Hard
			Label HardLabel = new Label();
			HardLabel.Text = "Hard";
			HardLabel.Size = new Size(60, 25);
			HardLabel.Location = new Point(45, 93);
			HardLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(HardLabel);
			RadioButton HardRadio = new RadioButton();
			HardRadio.Name = "HardRadio";
			HardRadio.Location = new Point(130, 90);
			HardRadio.CheckedChanged += HardRadio_CheckedChanged;
			OptionsForm.Controls.Add(HardRadio);
			
			// Custom
			Label CustomLabel = new Label();
			CustomLabel.Text = "Custom";
			CustomLabel.Size = new Size(60, 25);
			CustomLabel.Location = new Point(45, 118);
			CustomLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(CustomLabel);
			RadioButton CustomRadio = new RadioButton();
			CustomRadio.Name = "CustomRadio";
			CustomRadio.Location = new Point(130, 115);
			CustomRadio.CheckedChanged += CustomRadio_CheckedChanged;
			OptionsForm.Controls.Add(CustomRadio);
			
			// Fields line
			Label OptionsLabelLine2 = new Label();
			OptionsLabelLine2.Size = new Size(190, 2);
			OptionsLabelLine2.Location = new Point(5, 145);
			OptionsLabelLine2.BorderStyle = BorderStyle.FixedSingle;
			OptionsForm.Controls.Add(OptionsLabelLine2);
			
			//The Height Field
			Label HeightLabel = new Label();
			HeightLabel.Text = "Height";
			HeightLabel.Size = new Size(50, 20);
			HeightLabel.Location = new Point(14, 152);
			HeightLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(HeightLabel);
			TextBox HeightTextBox = new TextBox();
			HeightTextBox.Name = "HeightTextBox";
			HeightTextBox.Location = new Point(69, 150);
			HeightTextBox.Size = new Size(20,25);
			HeightTextBox.ReadOnly = true;
			HeightTextBox.LostFocus += CalculateCellsMine;
			OptionsForm.Controls.Add(HeightTextBox);
			
			//The Width Field
			Label WidthLabel = new Label();
			WidthLabel.Text = "Width";
			WidthLabel.Size = new Size(40, 20);
			WidthLabel.Location = new Point(94, 152);
			WidthLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(WidthLabel);
			TextBox WidthTextBox = new TextBox();
			WidthTextBox.Name = "WidthTextBox";
			WidthTextBox.Location = new Point(156, 150);
			WidthTextBox.Size = new Size(20,25);
			WidthTextBox.ReadOnly = true;
			WidthTextBox.LostFocus += CalculateCellsMine;
			OptionsForm.Controls.Add(WidthTextBox);
			
			//The Mines Field
			Label MinesLabel = new Label();
			MinesLabel.Text = "Mines";
			MinesLabel.Size = new Size(40, 20);
			MinesLabel.Location = new Point(14, 177);
			MinesLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(MinesLabel);
			TextBox MinesTextBox = new TextBox();
			MinesTextBox.Name = "MinesTextBox";
			MinesTextBox.Location = new Point(54, 175);
			MinesTextBox.Size = new Size(25,25);
			MinesTextBox.ReadOnly = true;
			MinesTextBox.LostFocus += CalculateCellsMine;
			OptionsForm.Controls.Add(MinesTextBox);
			
			//The Cells/Mine Field
			Label CellMineLabel = new Label();
			CellMineLabel.Text = "Cells/Mine";
			CellMineLabel.Size = new Size(70, 20);
			CellMineLabel.Location = new Point(84, 177);
			CellMineLabel.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OptionsForm.Controls.Add(CellMineLabel);
			TextBox CellMineTextBox = new TextBox();
			CellMineTextBox.Name = "CellMineTextBox";
			CellMineTextBox.Location = new Point(156, 175);
			CellMineTextBox.Size = new Size(30,25);
			CellMineTextBox.ReadOnly = true;
			OptionsForm.Controls.Add(CellMineTextBox);
			
			
			// The Okay Button
			NoFocusButton OkayButton = new NoFocusButton();
			OkayButton.Text = "Ok";
			OkayButton.Name = "OkayButton";
			OkayButton.Size = new Size(90, 25);
			OkayButton.Location = new Point(5, 205);
			OkayButton.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			OkayButton.Click += OkayButtonAction;
			OkayButton.FlatStyle = FlatStyle.Flat;
			OptionsForm.Controls.Add(OkayButton);
			
			// The Cancel Button
			NoFocusButton CancelButton = new NoFocusButton();
			CancelButton.Text = "Cancel";
			CancelButton.Name = "CancelButton";
			CancelButton.Size = new Size(90, 25);
			CancelButton.Location = new Point(105, 205);
			CancelButton.Font = new Font(OptionsLabel.Font.FontFamily, Single.Parse("10"));
			CancelButton.Click += CancelButtonAction;
			CancelButton.FlatStyle = FlatStyle.Flat;
			OptionsForm.Controls.Add(CancelButton);
		}
		
		private void SetInitialValues()
		{
			OptionsForm.Controls.Find("HeightTextBox", true)[0].Text = UserChoices.Height.ToString();
			OptionsForm.Controls.Find("WidthTextBox", true)[0].Text = UserChoices.Width.ToString();
			OptionsForm.Controls.Find("MinesTextBox", true)[0].Text = UserChoices.NumberOfMines.ToString();
			
			if(UserChoices.isEasy())
			{
				((RadioButton)OptionsForm.Controls.Find("EasyRadio", true)[0]).Checked = true;
			}
			else if(UserChoices.isMedium())
			{
				((RadioButton)OptionsForm.Controls.Find("MediumRadio", true)[0]).Checked = true;
			}
			else if(UserChoices.isHard())
			{
				((RadioButton)OptionsForm.Controls.Find("HardRadio", true)[0]).Checked = true;
			}
			else
			{
				((RadioButton)OptionsForm.Controls.Find("CustomRadio", true)[0]).Checked = true;
			}
			
			CalculateCellsMine(this, EventArgs.Empty);
		}
		
		public void RunOptionsMenu(int height, int width, int numMines)
		{
			// Set the user choices to the default
			UserChoices = new OptionsMenuData(width, height, numMines);
			OriginalChoices = new OptionsMenuData(width, height, numMines);
			
			SetInitialValues();
			OptionsForm.ShowDialog();
		}
		
		private void EasyRadio_CheckedChanged(object sender, EventArgs e)
		{
			if(((RadioButton)OptionsForm.Controls.Find("EasyRadio", true)[0]).Checked)
			{
				((TextBox)OptionsForm.Controls.Find("HeightTextBox", true)[0]).Text = "9";
				((TextBox)OptionsForm.Controls.Find("WidthTextBox", true)[0]).Text = "9";
				((TextBox)OptionsForm.Controls.Find("MinesTextBox", true)[0]).Text = "10";
			}
			CalculateCellsMine(this, EventArgs.Empty);
		}
		
		private void MediumRadio_CheckedChanged(object sender, EventArgs e)
		{
			if(((RadioButton)OptionsForm.Controls.Find("MediumRadio", true)[0]).Checked)
			{
				((TextBox)OptionsForm.Controls.Find("HeightTextBox", true)[0]).Text = "16";
				((TextBox)OptionsForm.Controls.Find("WidthTextBox", true)[0]).Text = "16";
				((TextBox)OptionsForm.Controls.Find("MinesTextBox", true)[0]).Text = "40";
			}
			CalculateCellsMine(this, EventArgs.Empty);
		}
		
		private void HardRadio_CheckedChanged(object sender, EventArgs e)
		{
			if(((RadioButton)OptionsForm.Controls.Find("HardRadio", true)[0]).Checked)
			{
				((TextBox)OptionsForm.Controls.Find("HeightTextBox", true)[0]).Text = "16";
				((TextBox)OptionsForm.Controls.Find("WidthTextBox", true)[0]).Text = "30";
				((TextBox)OptionsForm.Controls.Find("MinesTextBox", true)[0]).Text = "99";
			}
			CalculateCellsMine(this, EventArgs.Empty);
		}
		
		private void CustomRadio_CheckedChanged(object sender, EventArgs e)
		{
			if(((TextBox)OptionsForm.Controls.Find("HeightTextBox", true)[0]).ReadOnly)
			{
				((TextBox)OptionsForm.Controls.Find("HeightTextBox", true)[0]).ReadOnly = false;
				((TextBox)OptionsForm.Controls.Find("WidthTextBox", true)[0]).ReadOnly = false;
				((TextBox)OptionsForm.Controls.Find("MinesTextBox", true)[0]).ReadOnly = false;
			}
			else
			{
				((TextBox)OptionsForm.Controls.Find("HeightTextBox", true)[0]).ReadOnly = true;
				((TextBox)OptionsForm.Controls.Find("WidthTextBox", true)[0]).ReadOnly = true;
				((TextBox)OptionsForm.Controls.Find("MinesTextBox", true)[0]).ReadOnly = true;
			}
			CalculateCellsMine(this, EventArgs.Empty);
		}
		
		protected virtual void OnOptionsReturn(OptionMenuEventArgs e)
		{
			EventHandler<OptionMenuEventArgs> handler = OptionsReturn;
			if(handler != null)
			{
				handler(this, e);
			}
		}
		
		private void CancelButtonAction(object sender, EventArgs e)
		{
			OptionsForm.Close();
			OptionMenuEventArgs oEA = new OptionMenuEventArgs();
			oEA.Data = OriginalChoices;
			OnOptionsReturn(oEA);
		}
		
		private void OkayButtonAction(object sender, EventArgs e)
		{
			// Grab the new choices
			if(CheckInputs())
			{
				UserChoices.Height = Int32.Parse(OptionsForm.Controls.Find("HeightTextBox", true)[0].Text);
				UserChoices.Width = Int32.Parse(OptionsForm.Controls.Find("WidthTextBox", true)[0].Text);
				UserChoices.NumberOfMines = Int32.Parse(OptionsForm.Controls.Find("MinesTextBox", true)[0].Text);
				
				OptionsForm.Close();
				OptionMenuEventArgs oEA = new OptionMenuEventArgs();
				oEA.Data = UserChoices;
				OnOptionsReturn(oEA);
			}
		}
		
		private bool CheckInputs()
		{
			int temp;
			if(!Int32.TryParse(OptionsForm.Controls.Find("HeightTextBox", true)[0].Text, out temp))
			{
				return false;
			}
			
			if(!Int32.TryParse(OptionsForm.Controls.Find("WidthTextBox", true)[0].Text, out temp))
			{
				return false;
			}
			
			if(!Int32.TryParse(OptionsForm.Controls.Find("MinesTextBox", true)[0].Text, out temp))
			{
				return false;
			}
			
			return true;
		}
		
		private void CalculateCellsMine(object sender, EventArgs e)
		{
			if(CheckInputs())
			{
				double height = Convert.ToDouble(OptionsForm.Controls.Find("HeightTextBox", true)[0].Text);
				double width = Convert.ToDouble(OptionsForm.Controls.Find("WidthTextBox", true)[0].Text);
				double mines = Convert.ToDouble(OptionsForm.Controls.Find("MinesTextBox", true)[0].Text);
				double difficulty = (height * width) / mines;
				OptionsForm.Controls.Find("CellMineTextBox", true)[0].Text = difficulty.ToString();
			}
		}
		
		private void MainFormKeyStroke(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)Keys.Return)
			{
				OkayButtonAction(this, EventArgs.Empty);
			}
		}
	}
}