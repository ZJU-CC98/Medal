using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC98.Medal.Data
{
	/// <summary>
	/// ��ʾѫ�µķ��ࡣ
	/// </summary>
	public class MedalCategory
	{
		/// <summary>
		/// ��ȡ�����ø÷���ı�ʶ��
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// ��ȡ�����ø÷�������ơ�
		/// </summary>
		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		/// <summary>
		/// ��ȡ�����ø÷����µ�ѫ�µļ��ϡ�
		/// </summary>
		[InverseProperty(nameof(Medal.Category))]
		public virtual ICollection<Medal> Medals { get; set; } = new Collection<Medal>();
	}
}