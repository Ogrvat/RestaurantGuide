using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantGuide.Models
{
	/// <summary>
	/// Ресторан
	/// </summary>
	public class Restaurant
	{
		/// <summary>
		/// Id
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }
		
		/// <summary>
		/// Наименование ресторана
		/// </summary>
		[Required(ErrorMessage = "Укажите наименование")]
		public string Name { get; set; }
		
		/// <summary>
		/// Город ресторана
		/// </summary>
		[Required(ErrorMessage = "Укажите город")]
		public City City { get; set; }
	}
}
