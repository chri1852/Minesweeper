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
	class NoFocusButton : System.Windows.Forms.Button
	{
		protected override bool ShowFocusCues
		{
			get
			{
				return false;
			}
		}
	}
}