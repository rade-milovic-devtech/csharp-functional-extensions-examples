using CSharpFunctionalExtensions.Examples.ValueObject;
using System;

namespace CSharpFunctionalExtensions.Examples
{
	class Program
	{
		static void Main(string[] args)
		{
			RunValueObjectsExamples();
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
		}
	}
}