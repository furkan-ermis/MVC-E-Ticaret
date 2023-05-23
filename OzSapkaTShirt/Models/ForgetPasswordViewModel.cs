using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OzSapkaTShirt.Models
{
	public class ForgetPasswordViewModel
	{
		[DisplayName("Eposta")]
		[Required(ErrorMessage = "Bu alan zorunludur")]
		[EmailAddress(ErrorMessage = "Geçersiz Eposta")]
		public string eMail { get; set; }
	}
}