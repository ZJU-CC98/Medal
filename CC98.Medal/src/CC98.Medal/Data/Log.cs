using System;
using System.ComponentModel.DataAnnotations;

namespace CC98.Medal.Data
{
	/// <summary>
	/// 表示一个日志项目。
	/// </summary>
	public class Log
	{
		/// <summary>
		/// 获取或设置该项目的日志。
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// 获取或设置操作的用户名。
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// 获取或设置操作的勋章名。
		/// </summary>
		public string MedalName { get; set; }

		/// <summary>
		/// 获取或设置操作说明。
		/// </summary>
		public string Comment { get; set; }

		/// <summary>
		/// 获取或设置操作的时间。
		/// </summary>
		public DateTimeOffset Time { get; set; }
	}
}