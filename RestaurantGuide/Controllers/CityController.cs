using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantGuide.Models;

namespace RestaurantGuide.Controllers
{
	/// <summary>
	/// Контроллер городов
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class CityController : ControllerBase
	{
		private ApplicationContext db;
		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="context"></param>
		public CityController(ApplicationContext context)
		{
			db = context;
		}

		/// <summary>
		/// Добавление города по наименованию
		/// </summary>
		/// <param name="cityName">Наименование города</param>
		/// <returns></returns>
		[Route("AddByName")]
		[HttpPost]
		public async Task<ActionResult<City>> AddByName(string cityName)
		{
			if (String.IsNullOrWhiteSpace(cityName))
			{
				return BadRequest("пустое наименование");
			}

			if(db.Cities.FirstOrDefault(x => x.Name == cityName) != null)
			{
				return BadRequest($"город {cityName} уже существует");
			}

			var id = db.Cities.Any() ? db.Cities.Max(p => p.Id) + 1 : 1;
	
			var city = new City
			{
				Id = id,
				Name = cityName
			};

			db.Cities.Add(city);
			await db.SaveChangesAsync();

			return CreatedAtAction(nameof(AddByName), new { id = city.Id }, city);
		}

	}
}
