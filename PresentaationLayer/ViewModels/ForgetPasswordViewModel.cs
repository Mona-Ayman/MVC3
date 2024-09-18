namespace PresentationLayer.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[EmailAddress]
        public string Email { get; set; }
    }
}
