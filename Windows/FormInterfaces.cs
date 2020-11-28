using System;

namespace MIDRetail.Windows
{
	/// <summary>
	/// IFormBase contains the base requests that all child form must support. 
	/// </summary>
	public interface IFormBase
	{
		void ICut();
		void ICopy();
		void IPaste();
		void IDelete();
		void IClose();
		void ISave();
		void ISaveAs();
		void IRefresh();
		void IFind();
	}
}
