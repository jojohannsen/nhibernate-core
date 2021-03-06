﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2392
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnTearDown()
		{
			using (ISession s = Sfi.OpenSession())
			{
				s.Delete("from A");
				s.Flush();
			}
		}

		[Test]
		public async Task CompositeUserTypeSettabilityAsync()
		{
			ISession s = OpenSession();
			try
			{
				await (s.SaveAsync(new A { StringData1 = "first", StringData2 = "second" }));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				A a = (await (s.CreateCriteria<A>().ListAsync<A>())).First();
				a.MyPhone = new PhoneNumber(1, null);
				await (s.SaveAsync(a));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				A a = (await (s.CreateCriteria<A>().ListAsync<A>())).First();
				a.MyPhone = new PhoneNumber(1, "555-1234");
				await (s.SaveAsync(a));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}
		}
	}
}
