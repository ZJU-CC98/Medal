using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC98.Medal.Data
{
	/// <summary>
	/// ��ʾ�û���ѫ�µ����롣
	/// </summary>
	public class UserMedalApply
	{
		/// <summary>
		/// ��ȡ����������ѫ�µ��û��ı�ʶ��
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// ��ȡ�����������ѫ�µı�ʶ��
		/// </summary>
		public int MedalId { get; set; }

		/// <summary>
		/// ��ȡ�����������ѫ�¡�
		/// </summary>
		[ForeignKey(nameof(MedalId))]
		public Medal Medal { get; set; }

		/// <summary>
		/// ��ȡ�����ø������ʱ�䡣
		/// </summary>
		public DateTimeOffset Time { get; set; }
	}
}