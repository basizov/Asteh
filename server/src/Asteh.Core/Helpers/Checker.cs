using Asteh.Core.Models;

namespace Asteh.Core.Helpers
{
	internal static class Checker
	{
		public static bool CheckFilterUserModelForNullOrEmpty(this FilterUserModel filter)
		{
			var isBeginDateNullOrWmpty = string.IsNullOrEmpty(filter?.BeginDate ?? null);
			var isEndDateNullOrWmpty = string.IsNullOrEmpty(filter?.EndDate ?? null);
			if (filter == null || (
				string.IsNullOrEmpty(filter.Name) &&
				string.IsNullOrEmpty(filter.TypeName) &&
				isBeginDateNullOrWmpty &&
				isEndDateNullOrWmpty))
			{
				return false;
			}
			return true;
		}

		public static (bool result, DateTime? beginDate, DateTime? endDate)
			CheckFilterUserModelForCorrectDateTime(this FilterUserModel filter)
		{
			var isBeginDateNullOrEmpty = string.IsNullOrEmpty(filter.BeginDate);
			var isEndDateNullOrEmpty = string.IsNullOrEmpty(filter.EndDate);

			var isCorrectBeginDate = DateTime
				.TryParse(filter.BeginDate, out var beginDate) || isBeginDateNullOrEmpty;
			var isCorrectEndDate = DateTime
				.TryParse(filter.EndDate, out var endDate) || isEndDateNullOrEmpty;
			if (!isCorrectBeginDate || !isCorrectEndDate)
			{
				return (false,
					isCorrectBeginDate ? beginDate : null,
					isCorrectEndDate ? endDate : null);
			}
			return (true,
				isBeginDateNullOrEmpty ? null : beginDate,
				isEndDateNullOrEmpty ? null : endDate);
		}
	}
}
