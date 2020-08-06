using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantGuide.Models
{
	/// <summary>
	/// Город
	/// </summary>
	public class City
	{
		/// <summary>
		/// Id города
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }
		
		/// <summary>
		/// Наименование города
		/// </summary>
		[Required(ErrorMessage = "Укажите наименование")]
		public string Name { get; set; }
	}
}
