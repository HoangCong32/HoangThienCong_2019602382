using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanSach.Models.AccountViewModels
{
	public class RegisterViewModel
	{
        [Required(ErrorMessage = "Họ tên không được để trống", AllowEmptyStrings = false)]
        [Display(Name = "Họ tên")]
        public string FullName { set; get; }

        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDay { set; get; }

        [Required(ErrorMessage = "Email không được để trống", AllowEmptyStrings = false)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự và nhiều nhất {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác thực mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập không khớp với mật khẩu xác thực.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
    }
}
