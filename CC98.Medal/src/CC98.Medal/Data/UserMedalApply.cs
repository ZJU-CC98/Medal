using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC98.Medal.Data
{
	/// <summary>
	/// 表示用户对勋章的申请。
	/// </summary>
	public class UserMedalApply
	{
		/// <summary>
		/// 获取或设置申请勋章的用户的标识。
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// 获取或设置申请的勋章的标识。
		/// </summary>
		public int MedalId { get; set; }

		/// <summary>
		/// 获取或设置申请的勋章。
		/// </summary>
		[ForeignKey(nameof(MedalId))]
		public Medal Medal { get; set; }

		/// <summary>
		/// 获取或设置该申请的时间。
		/// </summary>
		public DateTimeOffset Time { get; set; }
	}
}