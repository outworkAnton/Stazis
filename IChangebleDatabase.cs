using System.Collections.Generic;
using System.Data;

namespace Stazis
{
	public interface IChangebleDatabase
	{
		void AddRecord(DataRow Record);
		int ChangeRecords(int Column, IList<string> InputElements, string OutputElement);
	}
}
