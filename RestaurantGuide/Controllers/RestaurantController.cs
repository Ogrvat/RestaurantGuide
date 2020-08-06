using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantGuide.Models;

namespace RestaurantGuide.Controllers
{
	/// <summary>
	/// Контроллер ресторанов
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class RestaurantController : ControllerBase
	{
		private ApplicationContext db;
		private int _pageSize = 10;

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="context"></param>
		public RestaurantController(ApplicationContext context)
		{
			db = context;
		}

		/// <summary>
		/// Добавление рестарана 
		/// </summary>
		/// <param name="restaurantName">Наименование ресторана</param>
		/// <param name="cityName">Наименование города</param>
		/// <returns></returns>
		[Route("AddByName")]
		[HttpPost]
		public async Task<ActionResult<Restaurant>> AddByName(string restaurantName, string cityName)
		{
			if (String.IsNullOrWhiteSpace(restaurantName) || String.IsNullOrWhiteSpace(cityName))
			{
				return BadRequest("пустые наименования");
			}

			var city = db.Cities.FirstOrDefault(x => x.Name == cityName);
			if(city == null)
			{
				return BadRequest($"{cityName} - города нет в БД");
			}

			if (db.Restaurants.FirstOrDefault(x => x.Name == restaurantName && x.City.Id == city.Id) != null)
			{
				return BadRequest($"{restaurantName} в {cityName} уже существует");
			}

			var id = db.Restaurants.Any() ? db.Restaurants.Max(p => p.Id) + 1 : 1;
	
			var restaurant = new Restaurant
			{
				Id = id,
				Name = restaurantName,
				City = city
			};

			db.Restaurants.Add(restaurant);
			await db.SaveChangesAsync();

			return CreatedAtAction(nameof(AddByName), new { id = restaurant.Id }, restaurant);
		}

		/// <summary>
		/// Рестораны города
		/// </summary>
		/// <param name="cityName">Наименование города</param>
		/// <param name="page">Номер страницы</param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetRestaurants")]
		public async Task<ActionResult<List<string>>> GetRestaurants(string cityName, int? page = null)
		{
			if ( String.IsNullOrWhiteSpace(cityName))
			{
				return BadRequest("пустое наименованиe города");
			}

			var city = db.Cities.FirstOrDefault(x => x.Name == cityName);
			if (city == null)
			{
				return BadRequest($"{cityName} - города нет в БД");
			}

			var restaurantsName = db.Restaurants
				.Where(x => x.City != null && x.City.Name == cityName)
				.Select(x => x.Name)
				.OrderBy(x => x);
			
			List<string> result;

			if (page.HasValue)
			{
				var restaurantCount = await db.Restaurants.CountAsync();
				if(_pageSize < 1 || ((_pageSize * (page.Value - 1)) > restaurantCount))
				{
					return BadRequest($"page от 1 до {(restaurantCount / _pageSize) + 1}");
				}

				result = await restaurantsName.
					Skip((page.Value - 1) * _pageSize)
					.Take(_pageSize)
					.ToListAsync();

			}
			else
			{
				result = await restaurantsName
					.ToListAsync();
			}

			return Ok(result);
		}
	}
}
