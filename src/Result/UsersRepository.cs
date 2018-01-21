using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpFunctionalExtensions.Examples.Results
{
	public class UsersRepository
	{
		private static readonly IDictionary<UserPrincipalName, User> store =
			new Dictionary<UserPrincipalName, User>();

		public async Task<Result> Add(User user)
		{
			if (user == null)
				throw new ArgumentNullException(nameof(user));

			var userDoesNotExist = await UserDoesNotExistFor(user.UserPrincipalName);

			if (userDoesNotExist)
			{
				store.Add(user.UserPrincipalName, user);

				return Result.Ok();
			}

			return Result.Fail($"User with user principal name \"{user.UserPrincipalName}\" already exist.");
		}

		private async Task<bool> UserDoesNotExistFor(UserPrincipalName userPrincipalName)
		{
			var user = await FindOneBy(userPrincipalName);

			return user.HasNoValue;
		}

		private Task<Maybe<User>> FindOneBy(UserPrincipalName userPrincipalName)
		{
			return store.ContainsKey(userPrincipalName)
				? Task.FromResult(Maybe<User>.From(store[userPrincipalName]))
				: Task.FromResult(Maybe<User>.None);
		}
	}
}