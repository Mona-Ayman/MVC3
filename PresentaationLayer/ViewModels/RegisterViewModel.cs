namespace PresentationLayer.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "FirstName Is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName Is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName Is Required")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Password & Confirm Diden't Match")]
        public string ConfirmPassword { get; set; }
        public bool? IsAgree { get; set; }
    }
}
