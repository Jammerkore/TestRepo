using System;
using System.Data;

using MIDRetail.Data;


namespace MIDRetail.Business
{
	
	public class Filters : MarshalByRefObject
	{
		public  Filters()
		{
		}

		public DataTable Retreive()
		{
			return new Filters().Retreive();	
		}

	}

	public class Attributes : MarshalByRefObject
	{
		public  Attributes()
		{
		}
		public DataTable Retreive()
		{
			return new Attributes().Retreive();	
		}
	}

	public class AttributeSets : MarshalByRefObject
	{
		public  AttributeSets()
		{
		}
		public DataTable Retreive()
		{
			return new AttributeSets().Retreive();	
		}
	}



}
