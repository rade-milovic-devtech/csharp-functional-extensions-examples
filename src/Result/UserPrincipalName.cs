using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CSharpFunctionalExtensions.Examples.Results
{
	public class UserPrincipalName : ValueObject<UserPrincipalName>
	{
		private const int MaxLength = 250;
		private const string RequiredDomainSuffix = ".onmicrosoft.com";

		public static Result<UserPrincipalName> Create(Maybe<string> valueOrNothing) =>
			valueOrNothing.ToResult("User principal name should not be empty")
				.OnSuccess(value => value.Trim())
				.Ensure(value => value != string.Empty, "User principal name should not be empty")
				.Ensure(value => value.Length <= MaxLength, "User principal name must be shorter than 250 characters")
				.Ensure(value => Regex.IsMatch(value, @"^(.+)@(.+)$"), "User principal name has invalid format")
				.Ensure(value => value.EndsWith(RequiredDomainSuffix, StringComparison.InvariantCultureIgnoreCase), $"User principal name must end with \"{RequiredDomainSuffix}\"")
				.OnSuccess(value => Debug.WriteLine($"Successfully created the user principal name for value: \"{value}\""))
				.OnFailure(error => Debug.WriteLine($"Failed to create the user principal name. Reason: \"{error}\""))
				.Map(Create);

		private static UserPrincipalName Create(string value)
		{
			var components = value.Split('@');

			return new UserPrincipalName(components[0], components[1]);
		}

		private UserPrincipalName(string userName, string domain)
		{
			UserName = userName;
			DomainPrefix = ExtractDomainPrefixFrom(domain);
			Domain = domain;
		}

		private string ExtractDomainPrefixFrom(string domain)
		{
			int requiredSuffixPosition = domain.IndexOf(RequiredDomainSuffix, StringComparison.InvariantCultureIgnoreCase);

			return domain.Substring(0, requiredSuffixPosition);
		}

		public string UserName { get; }
		public string DomainPrefix { get; }
		public string Domain { get; }

		protected override bool EqualsCore(UserPrincipalName other) =>
			string.Equals(UserName, other.UserName, StringComparison.InvariantCultureIgnoreCase)
				&& string.Equals(Domain, other.Domain, StringComparison.InvariantCultureIgnoreCase);

		protected override int GetHashCodeCore()
		{
			unchecked
			{
				var hashCode = UserName.GetHashCode();
				hashCode = (hashCode * 397) ^ Domain.GetHashCode();

				return hashCode;
			}
		}

		public override string ToString() => $"{UserName}@{Domain}";

		public static implicit operator string(UserPrincipalName userPrincipalName) => userPrincipalName.ToString();
	}
}