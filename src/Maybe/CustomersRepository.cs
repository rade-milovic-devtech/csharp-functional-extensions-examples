using CSharpFunctionalExtensions.Examples.ValueObject;
using System;
using System.Collections.Generic;

namespace CSharpFunctionalExtensions.Examples.Maybe
{
	public class CustomersRepository
	{
		private static readonly IDictionary<CustomerTenantId, Customer> store =
			new Dictionary<CustomerTenantId, Customer>();

		public Maybe<Customer> FindOneBy(CustomerTenantId tenantId)
		{
			if (tenantId == null)
				throw new ArgumentNullException(nameof(tenantId));

			return store.ContainsKey(tenantId)
				? Maybe<Customer>.From(store[tenantId])
				: Maybe<Customer>.None;
		}

		public void Add(Customer customer)
		{
			if (customer == null)
				throw new ArgumentNullException(nameof(customer));

			store.Add(customer.TenantId, customer);
		}
	}
}