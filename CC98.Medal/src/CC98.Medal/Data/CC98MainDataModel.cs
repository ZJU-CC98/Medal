using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CC98.Medal.Data
{
	/// <summary>
	/// 提供 CC98 主数据库的数据模型。
	/// </summary>
	public class CC98MainDataModel : DbContext
	{
		/// <summary>
		/// 初始化一个数据库链接的新实例。
		/// </summary>
		/// <param name="options">数据库上下文选项。</param>
		public CC98MainDataModel(DbContextOptions<CC98MainDataModel> options) : base(options)
		{
		}

		/// <summary>
		/// 获取或数据库中所有用户的集合。
		/// </summary>
		public virtual DbSet<CC98User> Users { get; set; }
	}

	[Table("User")]
	public class CC98User
	{
		/// <summary>
		/// 获取或设置用户的标识。
		/// </summary>
		[Column("UserId")]
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// 获取或设置用户的名称。
		/// </summary>
		[Column("UserName")]
		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		/// <summary>
		/// 获取或设置用户的财富值。
		/// </summary>
		[Column("UserWealth")]
		[Range(0, int.MaxValue)]
		public int Wealth { get; set; }
	}
}
