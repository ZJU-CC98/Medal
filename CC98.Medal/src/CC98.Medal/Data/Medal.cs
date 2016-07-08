using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CC98.Medal.Data
{
	/// <summary>
	/// ��ʾһ��ѫ��ϵͳ��ѫ�¡�
	/// </summary>
	public class Medal
	{
		/// <summary>
		/// ��ȡ�����ø�ѫ�µı�ʶ��
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// ��ȡ�����ø�ѫ�µ����ơ�
		/// </summary>
		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		/// <summary>
		/// ��ȡ�����ø�ѫ�µ�������
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// ��ȡ�����ø�ѫ�µ����ӵ�ַ��
		/// </summary>
		[DataType(DataType.Url)]
		public string Link { get; set; }

		/// <summary>
		/// ��ȡ�����ø�ѫ�µ�ͼƬ�ĵ�ַ��
		/// </summary>
		[Required]
		public string ImageUri { get; set; }

		/// <summary>
		/// ��ȡ������ѫ�µļ۸���ֻ�Կɹ����ѫ����Ч��
		/// </summary>
		[Range(0, int.MaxValue)]
		public int? Price { get; set; }

		/// <summary>
		/// ��ȡ������һ��ֵ��ָʾѫ���Ƿ���������Ա����䷢��
		/// </summary>
		public bool CanApply { get; set; }

		/// <summary>
		/// ��ȡ�����ø�ѫ�µĶ������á�
		/// </summary>
		[NotMapped]
		public PriceSetting[] PriceSettings
		{
			get
			{
				return string.IsNullOrEmpty(PriceSettingsInternal) ? new PriceSetting[0] : JsonConvert.DeserializeObject<PriceSetting[]>(PriceSettingsInternal);
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				PriceSettingsInternal = JsonConvert.SerializeObject(value);
			}
		}

		/// <summary>
		/// ��ȡ�����ô洢�������õ��ڲ��ֶΡ�
		/// </summary>
		[Column(nameof(PriceSettings))]
		public string PriceSettingsInternal { get; set; }

		/// <summary>
		/// ��ȡ�����ø�ѫ�������ķ���ı�ʶ��
		/// </summary>
		public int? CategoryId { get; set; }

		/// <summary>
		/// ��ȡ�����ø�ѫ�������ķ��ࡣ
		/// </summary>
		[ForeignKey(nameof(CategoryId))]
		public MedalCategory Category { get; set; }
	}
}