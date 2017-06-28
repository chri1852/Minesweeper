using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Minesweeper
{
	class AboutMenu
	{
		private Form AboutForm;

		public AboutMenu()
		{
			AboutForm = new Form();
			AboutForm.MaximizeBox = false;
			AboutForm.FormBorderStyle = FormBorderStyle.FixedDialog;
			AboutForm.Name = "AboutFrom";
			AboutForm.Text = "Minesweeper";
			AboutForm.ClientSize = new Size(200, 270);
			AboutForm.KeyPreview = true;
			AboutForm.KeyPress += new KeyPressEventHandler(AboutMenuClose);
			AboutForm.Icon = GameResources.GetIconImage();
			
			Label AboutLabel = new Label();
			AboutLabel.Font = new Font(AboutLabel.Font.FontFamily, Single.Parse("14"), FontStyle.Bold);
			AboutLabel.Text = "About";
			AboutLabel.TextAlign = ContentAlignment.MiddleCenter;
			AboutLabel.Size = new Size(190, 30);
			AboutLabel.Location = new Point(5, 5);
			AboutForm.Controls.Add(AboutLabel);
			
			Label AboutLabelLine = new Label();
			AboutLabelLine.Size = new Size(190, 2);
			AboutLabelLine.Location = new Point(5, 37);
			AboutLabelLine.BorderStyle = BorderStyle.FixedSingle;
			AboutForm.Controls.Add(AboutLabelLine);
			
			// The Icon Picture
			PictureBox IconPicture = new PictureBox();
			IconPicture.Size = new Size(128,128);
			IconPicture.Location = new Point(36, 42);
			IconPicture.Image = GameResources.GetIconImageSized(128, 128).ToBitmap();
			AboutForm.Controls.Add(IconPicture);
			
			// The Date Line
			Label DateLable = new Label();
			DateLable.Text = "Created - October 9th 2015";
			DateLable.TextAlign = ContentAlignment.MiddleCenter;
			DateLable.Size = new Size(190, 15);
			DateLable.Location = new Point(5, 175);
			AboutForm.Controls.Add(DateLable);
			
			// The Author Line
			Label AuthorLabel = new Label();
			AuthorLabel.Text = "Author - Alex Christensen";
			AuthorLabel.TextAlign = ContentAlignment.MiddleCenter;
			AuthorLabel.Size = new Size(190, 15);
			AuthorLabel.Location = new Point(5, 195);
			AboutForm.Controls.Add(AuthorLabel);
			
			// The Contact Line
			Label ContactLabel = new Label();
			ContactLabel.Text = "Alex.Christensen@chrobinson.com";
			ContactLabel.TextAlign = ContentAlignment.MiddleCenter;
			ContactLabel.Size = new Size(190, 15);
			ContactLabel.Location = new Point(5, 215);
			AboutForm.Controls.Add(ContactLabel);
			
			// The Okay Button
			NoFocusButton OkayButton = new NoFocusButton();
			OkayButton.Text = "Ok";
			OkayButton.Name = "OkayButton";
			OkayButton.FlatStyle = FlatStyle.Flat;
			OkayButton.Size = new Size(90, 25);
			OkayButton.Location = new Point(55, 240);
			OkayButton.Font = new Font(OkayButton.Font.FontFamily, Single.Parse("10"));
			OkayButton.Click += AboutMenuClose;
			AboutForm.Controls.Add(OkayButton);
		}
	
		public void AboutMenuRun()
		{
			AboutForm.ShowDialog();
		}
		
		private void AboutMenuClose(object sender, EventArgs e)
		{
			AboutForm.Close();
		}
	}
}