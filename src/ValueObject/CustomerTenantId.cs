using System;

namespace CSharpFunctionalExtensions.Examples.ValueObject
{
	public class CustomerTenantId : ValueObject<CustomerTenantId>
	{
		private readonly string value;

		public CustomerTenantId(Guid value) : this(value.ToString()) {}

		public CustomerTenantId(string value)
		{
			Validate(value);

			this.value = value;
		}

		private void Validate(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("must not be null, empty or blank", nameof(value));

			Guid guidValue;
			if (!Guid.TryParse(value, out guidValue))
			{
				throw new ArgumentException("must be in GUID format", nameof(value));
			}

			if (guidValue == Guid.Empty)
			{
				throw new ArgumentException("must not be empty GUID", nameof(value));
			}
		}

		protected override bool EqualsCore(CustomerTenantId other) =>
			string.Equals(value, other.value, StringComparison.InvariantCultureIgnoreCase);

		protected override int GetHashCodeCore() => value.GetHashCode();

		public override string ToString() => value;

		public static implicit operator string(CustomerTenantId customerTenantId) => customerTenantId.ToString();

		public static implicit operator Guid(CustomerTenantId customerTenantId) => Guid.Parse(customerTenantId);

		public static explicit operator CustomerTenantId(string value) => new CustomerTenantId(value);

		public static explicit operator CustomerTenantId(Guid value) => new CustomerTenantId(value);
	}
}