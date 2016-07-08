using System;
using System.ComponentModel.DataAnnotations;

namespace CC98.Medal.Data
{
	/// <summary>
	/// ��ʾһ����־��Ŀ��
	/// </summary>
	public class Log
	{
		/// <summary>
		/// ��ȡ�����ø���Ŀ����־��
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// ��ȡ�����ò������û�����
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// ��ȡ�����ò�����ѫ������
		/// </summary>
		public string MedalName { get; set; }

		/// <summary>
		/// ��ȡ�����ò���˵����
		/// </summary>
		public string Comment { get; set; }

		/// <summary>
		/// ��ȡ�����ò�����ʱ�䡣
		/// </summary>
		public DateTimeOffset Time { get; set; }
	}
}