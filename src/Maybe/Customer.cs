using CSharpFunctionalExtensions.Examples.ValueObject;
using System;

namespace CSharpFunctionalExtensions.Examples.Maybe
{
	public class Customer
	{
		public Customer(CustomerTenantId tenantId, string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("can't be null, empty or blank", nameof(name));

			TenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
			Name = name;
		}

		public CustomerTenantId TenantId { get; }
		public string Name { get; }

		public override bool Equals(object obj)
		{
			var other = obj as Customer;

			if (other is null) return false;

			if (ReferenceEquals(this, other)) return true;

			if (GetType() != other.GetType()) return false;

			return TenantId == other.TenantId;
		}

		public override int GetHashCode() => (GetType().ToString() + TenantId).GetHashCode();

		public override string ToString() =>
			$"{nameof(Customer)} {{ {nameof(TenantId)}: {TenantId}, {nameof(Name)}: {Name} }}";

		public static bool operator ==(Customer left, Customer right)
		{
			if (left is null && right is null) return true;

			if (left is null || right is null) return false;

			return left.Equals(right);
		}

		public static bool operator !=(Customer left, Customer right) => !(left == right);
	}
}