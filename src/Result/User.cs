using System;

namespace CSharpFunctionalExtensions.Examples.Results
{
	public class User
	{
		public User(UserPrincipalName userPrincipalName, string fullName)
		{
			if (string.IsNullOrWhiteSpace(fullName))
				throw new ArgumentException("can't be null, empty or blank", nameof(fullName));

			UserPrincipalName = userPrincipalName ?? throw new ArgumentNullException(nameof(userPrincipalName));
			FullName = fullName;
		}

		public UserPrincipalName UserPrincipalName { get; }
		public string FullName { get; }

		public override bool Equals(object obj)
		{
			var other = obj as User;

			if (other is null) return false;

			if (ReferenceEquals(this, other)) return true;

			if (GetType() != other.GetType()) return false;

			return UserPrincipalName == other.UserPrincipalName;
		}

		public override int GetHashCode() => (GetType().ToString() + UserPrincipalName).GetHashCode();

		public override string ToString() =>
			$"{nameof(User)} {{ {nameof(UserPrincipalName)}: {UserPrincipalName}, {nameof(FullName)}: {FullName} }}";

		public static bool operator ==(User left, User right)
		{
			if (left is null && right is null) return true;

			if (left is null || right is null) return false;

			return left.Equals(right);
		}

		public static bool operator !=(User left, User right) => !(left == right);
	}
}