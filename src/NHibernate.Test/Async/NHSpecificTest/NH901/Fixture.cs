﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH901
{
	using System.Threading.Tasks;
	[TestFixture]
	public abstract class FixtureBaseAsync : TestCase
	{
		protected override void OnTearDown()
		{
			base.OnTearDown();
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}

		[Test]
		public async Task EmptyValueTypeComponentAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person p = new Person("Jimmy Hendrix");
				await (s.SaveAsync(p));
				await (tx.CommitAsync());
			}

			InterceptorStub interceptor = new InterceptorStub();
			using (ISession s = OpenSession(interceptor))
			using (ITransaction tx = s.BeginTransaction())
			{
				Person jimmy = await (s.GetAsync<Person>("Jimmy Hendrix"));
				interceptor.entityToCheck = jimmy;
				await (tx.CommitAsync());
			}
			Assert.IsFalse(interceptor.entityWasDeemedDirty);

			InterceptorStub interceptor2 = new InterceptorStub();
			using (ISession s = OpenSession(interceptor2))
			using (ITransaction tx = s.BeginTransaction())
			{
				Person jimmy = await (s.GetAsync<Person>("Jimmy Hendrix"));
				jimmy.Address = new Address();
				interceptor.entityToCheck = jimmy;
				await (tx.CommitAsync());
			}
			Assert.IsFalse(interceptor2.entityWasDeemedDirty);
		}

		[Test]
		public async Task ReplaceValueTypeComponentWithSameValueDoesNotMakeItDirtyAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person p = new Person("Jimmy Hendrix");
				Address a = new Address();
				a.Street = "Some Street";
				a.City = "Some City";
				p.Address = a;

				await (s.SaveAsync(p));
				await (tx.CommitAsync());
			}

			InterceptorStub interceptor = new InterceptorStub();
			using (ISession s = OpenSession(interceptor))
			using (ITransaction tx = s.BeginTransaction())
			{
				Person jimmy = await (s.GetAsync<Person>("Jimmy Hendrix"));
				interceptor.entityToCheck = jimmy;

				Address a = new Address();
				a.Street = "Some Street";
				a.City = "Some City";
				jimmy.Address = a;
				Assert.AreNotSame(jimmy.Address, a);

				await (tx.CommitAsync());
			}
			Assert.IsFalse(interceptor.entityWasDeemedDirty);
		}
	}

	[TestFixture]
	public class FixtureAsync : FixtureBaseAsync
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override string[] Mappings
		{
			get { return new[] {"NHSpecificTest.NH901.Mappings.hbm.xml"}; }
		}
	}

	[TestFixture]
	public class FixtureByCodeAsync : FixtureBaseAsync
	{
		protected override string[] Mappings
		{
			get { return Array.Empty<string>(); }
		}

		protected override string MappingsAssembly
		{
			get { return null; }
		}

		protected override void AddMappings(Configuration configuration)
		{
			var mapper = new ModelMapper();
			mapper.Class<Person>(rc =>
			{
				rc.Table("NH901_Person");
				rc.Id(x => x.Name, m => m.Generator(Generators.Assigned));
				rc.Component(x => x.Address, cm =>
				{
					cm.Property(x => x.City);
					cm.Property(x => x.Street);
				});
			});
			configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
		}
	}
}
