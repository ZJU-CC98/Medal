using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CC98.Identity;
using CC98.Medal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sakura.AspNetCore;
using Sakura.AspNetCore.Authentication;
using Sakura.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CC98.Medal.Controllers
{
	/// <summary>
	/// 提供勋章相关功能。
	/// </summary>
	public class MedalController : Controller
	{
		public MedalController(MedalDataModel dataModel, CC98MainDataModel cc98DataModel, IOperationMessageAccessor messageAccessor, ExternalSignInManager externalSignInManager)
		{
			DataModel = dataModel;
			MessageAccessor = messageAccessor;
			ExternalSignInManager = externalSignInManager;
			CC98DataModel = cc98DataModel;
		}

		#region 注入对象

		/// <summary>
		/// 数据库连接对象。
		/// </summary>
		private MedalDataModel DataModel { get; }

		private CC98MainDataModel CC98DataModel { get; }

		/// <summary>
		/// 操作消息访问器。
		/// </summary>
		private IOperationMessageAccessor MessageAccessor { get; }

		/// <summary>
		/// 外部登录管理器。
		/// </summary>
		private ExternalSignInManager ExternalSignInManager { get; }

		#endregion

		#region 辅助方法

		/// <summary>
		/// 确保获得勋章，否则引发错误。
		/// </summary>
		/// <param name="id">勋章标识。</param>
		/// <returns>操作结果。</returns>
		private async Task<Data.Medal> EnsureGetMedalAsync(int id)
		{
			var item = await (from i in DataModel.Medals
							  where i.Id == id
							  select i).SingleOrDefaultAsync();

			if (item == null)
			{
				throw new ActionResultException(HttpStatusCode.NotFound);
			}

			return item;
		}

		#endregion

		/// <summary>
		/// 显示勋章主页。
		/// </summary>
		/// <returns>操作结果。</returns>
		[AllowAnonymous]
		public IActionResult Index(int page = 1)
		{
			var items = from i in DataModel.Medals.Include(p => p.Category)
						select i;

			return View(items.ToPagedList(20, page));
		}

		/// <summary>
		/// 创建新勋章。
		/// </summary>
		/// <returns>操作结果。</returns>
		[HttpGet]
		[Authorize(Policies.Edit)]
		public IActionResult Create()
		{
			return View();
		}

		/// <summary>
		/// 执行添加勋章操作。
		/// </summary>
		/// <param name="model">数据模型。</param>
		/// <returns>操作结果。</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policies.Edit)]
		public async Task<IActionResult> Create(Data.Medal model)
		{
			HandleModel(model);


			if (ModelState.IsValid)
			{
				try
				{
					DataModel.Medals.Add(model);
					await DataModel.SaveChangesAsync();

					MessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功。",
						string.Format(CultureInfo.CurrentUICulture, "你已经成功添加了勋章 {0}。", model.Name));

					return RedirectToAction("Index", "Medal");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(model);
		}

		/// <summary>
		/// 编辑勋章信息。
		/// </summary>
		/// <param name="id">要编辑的勋章的标识。</param>
		/// <returns>操作结果。</returns>
		[HttpGet]
		[Authorize(Policies.Edit)]
		public async Task<IActionResult> Edit(int id)
		{
			return View(await EnsureGetMedalAsync(id));
		}

		/// <summary>
		/// 执行编辑操作。
		/// </summary>
		/// <param name="model">数据模型。</param>
		/// <returns>操作结果。</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policies.Edit)]
		public async Task<IActionResult> Edit(Data.Medal model)
		{
			HandleModel(model);

			if (ModelState.IsValid)
			{
				try
				{
					DataModel.Medals.Update(model);
					await DataModel.SaveChangesAsync();

					MessageAccessor.Messages.Add(OperationMessageLevel.Success, "操作成功。",
						string.Format(CultureInfo.CurrentUICulture, "你已经成功修改了勋章 {0}。", model.Name));

					return RedirectToAction("Index", "Medal");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(model);
		}

		/// <summary>
		/// 删除模型中重复项目导致的不必要数据更新信息。
		/// </summary>
		/// <param name="model">模型对象</param>
		private void HandleModel(Data.Medal model)
		{
			ModelState.Remove(nameof(model.Category));
		}

		/// <summary>
		/// 显示勋章详细信息。
		/// </summary>
		/// <param name="id">勋章的标识。</param>
		/// <returns>操作结果。</returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Detail(int id)
		{
			return View(await EnsureGetMedalAsync(id));
		}

		/// <summary>
		/// 放弃勋章。
		/// </summary>
		/// <param name="id">要放弃的勋章的标识。</param>
		/// <returns>操作结果。</returns>
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Discard(int id)
		{
			var userId = User.GetId();

			var item = await (from i in DataModel.UserMedalIssues
							  where i.UserId == userId && i.MedalId == id
							  select i).SingleOrDefaultAsync();

			if (item == null)
			{
				return BadRequest("你目前没有获得这个勋章。");
			}


			// 删除并勋章记录
			DataModel.UserMedalIssues.Remove(item);
			await DataModel.SaveChangesAsync();

			return Ok();
		}

		/// <summary>
		/// 申请勋章。
		/// </summary>
		/// <param name="id">要申请的勋章的标识。</param>
		/// <returns>操作结果。</returns>
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Apply(int id)
		{
			var userId = User.GetId();

			// 申请记录
			var hasApplied = await (from i in DataModel.UserMedalApplies
									where i.UserId == userId && i.MedalId == id
									select i).AnyAsync();

			if (hasApplied)
			{
				return BadRequest("你已经申请了这个勋章。");
			}

			// 拥有记录
			var hasOwnedInfinitily = await (from i in DataModel.UserMedalIssues
											where i.UserId == userId && i.MedalId == id && i.ExpireTime == null
											select i).AnyAsync();

			if (hasOwnedInfinitily)
			{
				return BadRequest("你已经无限期拥有该勋章，无需申请勋章。");
			}

			// 申请检查
			var medal = await EnsureGetMedalAsync(id);
			if (!medal.CanApply)
			{
				return BadRequest("管理员不允许申请该勋章。");
			}
			// 保存记录
			var item = new UserMedalApply { MedalId = id, UserId = userId, Time = DateTimeOffset.Now };
			DataModel.UserMedalApplies.Add(item);

			try
			{
				await DataModel.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok();
		}


		/// <summary>
		/// 取消申请勋章。
		/// </summary>
		/// <param name="id">要取消申请的勋章的标识。</param>
		/// <returns>操作结果。</returns>
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Unapply(int id)
		{
			var userId = User.GetId();

			// 申请记录
			var item = await (from i in DataModel.UserMedalApplies
							  where i.UserId == userId && i.MedalId == id
							  select i).SingleOrDefaultAsync();

			// 未申请
			if (item == null)
			{
				return BadRequest("你尚未申请这个勋章。");
			}

			// 删除记录
			DataModel.UserMedalApplies.Remove(item);
			try
			{
				await DataModel.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok();
		}

		/// <summary>
		/// 执行订购操作。
		/// </summary>
		/// <param name="id">勋章名称。</param>
		/// <param name="type">订购类型。</param>
		/// <returns>操作结果。</returns>
		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Order(int id, int type)
		{

			var item = await (from i in DataModel.Medals
							  where i.Id == id
							  select i).SingleOrDefaultAsync();

			if (item == null)
			{
				return BadRequest("勋章不存在。");
			}

			if (item.PriceSettings.Length <= type)
			{
				return BadRequest("订购类型错误。");
			}

			var priceSetting = item.PriceSettings[type];

			var userId = User.GetId();
			var userName = ExternalSignInManager.GetUserName(User);

			// 提取旧版数据库的用户信息，如果无法找到则是一种异常情况
			var cc98UserInfo = await (from i in CC98DataModel.Users
									  where i.Name == userName
									  select i).SingleAsync();

			// 财富不够
			if (cc98UserInfo.Wealth < priceSetting.Price)
			{
				MessageAccessor.Messages.Add(OperationMessageLevel.Error, "购买勋章失败。", "您没有足够的财富值执行本次购买。");
				return RedirectToAction("Index", "Medal", new { id });
			}


			// 减少财富值
			cc98UserInfo.Wealth -= priceSetting.Price;
			await CC98DataModel.SaveChangesAsync();

			// TODO: 事务支持
			var orderItem = await (from i in DataModel.UserMedalIssues
								   where i.UserId == userId && i.MedalId == id
								   select i).SingleOrDefaultAsync();


			// 是否有现有的购买记录
			if (orderItem == null)
			{
				// 无购买记录时，创建一个新的购买记录，过期日期为今天
				orderItem = new UserMedalIssue
				{
					MedalId = id,
					UserId = userId,
					ExpireTime = DateTime.Today
				};
			}

			// 无限期不能继续购买
			if (orderItem.ExpireTime == null)
			{
				MessageAccessor.Messages.Add(OperationMessageLevel.Error, "购买勋章失败。", "您已经无限期拥有该勋章，无法再次购买勋章。");
				return RedirectToAction("Index", "Medal", new { id });
			}

			// 无限期购买
			if (priceSetting.ExpiryDate == null)
			{
				orderItem.ExpireTime = null;
			}
			// 有限期购买
			else
			{
				orderItem.ExpireTime += TimeSpan.FromDays(priceSetting.ExpiryDate.Value);
			}


			// 订购后返回该勋章主页
			return RedirectToAction("Index", "Medal", new { id });
		}
	}
}
