using System.ComponentModel.DataAnnotations;

namespace CC98.Medal.Data
{
	/// <summary>
	/// ��ʾ�������۵��趨��
	/// </summary>
	public class PriceSetting
	{
		/// <summary>
		/// ��ȡ������Ҫ�趨�ļ۸�
		/// </summary>
		[Range(0, int.MaxValue)]
		public int Price { get; set; }

		/// <summary>
		/// ��ȡ������Ҫ�趨����Ч�ڣ�����Ϊ��λ�������������Ϊ null�����ʾ��Ч��Ϊ���ޡ�
		/// </summary>
		[Range(0, int.MaxValue)]
		public int? ExpiryDate { get; set; }
	}
}