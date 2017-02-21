
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Stazis
{
	/// <summary>
	/// Description of AddRecord.
	/// </summary>
	public partial class AddRecord : Form
	{
		public Database DB { get; set; }
		
		public AddRecord()
		{
			InitializeComponent();
		}
		
		static DataTable QueryProcess(DataTable Source)
		{
			DataTable tmp = Source.Clone();
			var query =
				from order in Source.AsEnumerable()
				where order
				select order;
			tmp = query.CopyToDataTable();
			return tmp.Rows.Count != 0 ? tmp : Source;
		}
		
		void ExploreData()
		{
			
		}
	}
}
