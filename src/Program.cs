using CSharpFunctionalExtensions.Examples.Maybe;
using CSharpFunctionalExtensions.Examples.ValueObject;
using System;

namespace CSharpFunctionalExtensions.Examples
{
	class Program
	{
		static void Main(string[] args)
		{
			RunValueObjectsExamples();

			RunMaybeExamples();
		}

		private static void RunValueObjectsExamples()
		{
			var value = Guid.NewGuid();
			var firstCustomerTenantId = new CustomerTenantId(value);
			var secondCustomerTenantId = new CustomerTenantId(value);

			Console.WriteLine("Value objects examples:");
			Console.WriteLine();

			Console.WriteLine($"Customer tenant ids are equal (using Equals method): {firstCustomerTenantId.Equals(secondCustomerTenantId)}");
			Console.WriteLine($"Customer tenant ids are equal (using equality operator): {firstCustomerTenantId == secondCustomerTenantId}");
			Console.WriteLine($"Customer tenant ids hash codes are also equal: {firstCustomerTenantId.GetHashCode() == secondCustomerTenantId.GetHashCode()}");
			Console.WriteLine();

			Console.WriteLine($"Implicit conversion to string: {firstCustomerTenantId}");
			Guid firstCustomerTenantIdGuid = firstCustomerTenantId;
			Console.WriteLine($"Implicit conversion to GUID: {firstCustomerTenantIdGuid}");
			Console.WriteLine();

			Console.WriteLine($"Explicit conversion from string: {(CustomerTenantId) "85a21b02-b118-4cce-91d1-a37fc8623579"}");
			Console.WriteLine($"Explicit conversion from GUID: {(CustomerTenantId) Guid.NewGuid()}");
			Console.WriteLine();
		}

		private static void RunMaybeExamples()
		{
			var existingCustomerTenantId = new CustomerTenantId(Guid.NewGuid());
			var nonExistingCustomerTenantId = new CustomerTenantId(Guid.NewGuid());

			var customersRepository = new CustomersRepository();
			customersRepository.Add(new Customer(existingCustomerTenantId, "Magenta D.O.O."));

			var foundCustomer = customersRepository.FindOneBy(existingCustomerTenantId);
			var notFoundCustomer = customersRepository.FindOneBy(nonExistingCustomerTenantId);

			Console.WriteLine("Maybe examples:");
			Console.WriteLine();

			Console.WriteLine($"Customer for { nonExistingCustomerTenantId } is not found: {notFoundCustomer.HasNoValue}");
			Console.WriteLine($"Customer for { existingCustomerTenantId } is found: {foundCustomer.HasValue}");
			Console.WriteLine($"Customer for { existingCustomerTenantId } has value of: {foundCustomer.Value}");
			Console.WriteLine();

			var foundCustomerClone = Maybe<Customer>.From(new Customer(existingCustomerTenantId, "Magenta D.O.O."));
			Console.WriteLine($"Same customers are equal (using Equals method): {foundCustomer.Equals(foundCustomerClone)}");
			Console.WriteLine($"Different customers are equal (using Equals method): {foundCustomer.Equals(notFoundCustomer)}");
			Console.WriteLine($"Empty and non-empty customers are equal (using Equals method): {foundCustomer.Equals(Maybe<Customer>.None)}");
			Console.WriteLine($"Same customers are equal (using equality operator): {foundCustomer == foundCustomerClone}");
			Console.WriteLine($"Different customers are equal (using equality operator): {foundCustomer == notFoundCustomer}");
			Console.WriteLine($"Empty and non-empty customers are equal (using equality operator): {foundCustomer == Maybe<Customer>.None}");
			Console.WriteLine($"Same customers hash codes are equal: {foundCustomer.GetHashCode() == foundCustomerClone.GetHashCode()}");
			Console.WriteLine($"Different customers hash codes are equal: {foundCustomer.GetHashCode() == notFoundCustomer.GetHashCode()}");
			Console.WriteLine($"Empty and non-empty customers hash codes are equal: {foundCustomer.GetHashCode() == Maybe<Customer>.None.GetHashCode()}");
			Console.WriteLine();

			Maybe<CustomerTenantId> maybeCustomerTenantId = existingCustomerTenantId;
			Maybe<CustomerTenantId> emptyCustomerTenantId = null;
			Console.WriteLine($"Implicit conversion of object to Maybe: {maybeCustomerTenantId}");
			Console.WriteLine($"Implicit conversion of NULL to Maybe: {emptyCustomerTenantId}");
			Console.WriteLine();

			Console.WriteLine($"Unwrap non-empty customer: {foundCustomer.Unwrap()}");
			Console.WriteLine($"Unwrap empty customer (without fall-back value): {notFoundCustomer.Unwrap()}");
			Console.WriteLine($"Unwrap empty customer (with fall-back value): {notFoundCustomer.Unwrap(new Customer((CustomerTenantId) Guid.NewGuid(), "Non-Existing"))}");
			Console.WriteLine();

			Console.WriteLine($"Unwrap non-empty customer name: {foundCustomer.Unwrap(customer => customer.Name)}");
			Console.WriteLine($"Unwrap empty customer's name (without fall-back value): {notFoundCustomer.Unwrap(customer => customer.Name)}");
			Console.WriteLine($"Unwrap empty customer's name (with fall-back value): {notFoundCustomer.Unwrap(customer => customer.Name, "Non-Existing")}");
			Console.WriteLine();

			Console.WriteLine($"Select non-empty customer's name (value selector): {foundCustomer.Select(customer => customer.Name)}");
			Console.WriteLine($"Select empty customer's name (value selector): {notFoundCustomer.Select(customer => customer.Name)}");
			Console.WriteLine($"Select non-empty customer's name (maybe selector): {foundCustomer.Select(customer => Maybe<string>.From(customer.Name))}");
			Console.WriteLine($"Select empty customer's name (maybe selector): {notFoundCustomer.Select(customer => Maybe<string>.From(customer.Name))}");
			Console.WriteLine();

			Console.WriteLine($"Found customer's name contains 'D.O.O.': {foundCustomer.Where(customer => customer.Name.Contains("D.O.O."))}");
			Console.WriteLine($"Found customer's name contains 'J': {foundCustomer.Where(customer => customer.Name.Contains("J"))}");
			Console.WriteLine($"Empty customer's name contains 'D.O.O.': {notFoundCustomer.Where(customer => customer.Name.Contains("D.O.O."))}");
			Console.WriteLine();

			void printCustomerName(Customer customer) => Console.Write($"Customer's name is {customer.Name}");
			Console.WriteLine($"Action will be executed on found customer.");
			foundCustomer.Execute(printCustomerName);
			notFoundCustomer.Execute(printCustomerName);
			Console.WriteLine();
		}
	}
}