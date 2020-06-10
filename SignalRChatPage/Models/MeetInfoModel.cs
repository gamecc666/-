using SignalRChatPage.CustomerAttr;
using System;
using System.ComponentModel.DataAnnotations;

namespace SignalRChatPage.Models
{
    public class MeetInfoModel
    {
        [Key]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(6,MinimumLength =6,ErrorMessage ="房间号应为6位数字")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "只能输入数字")]
        public string RoomNo { get; set; }

        [Required(ErrorMessage ="不能为空")]
        public string Theme { get; set; }

        [Required(ErrorMessage = "不能为空")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "不能为空")]
        [CompareData]
        public DateTime EndTime { get; set; }

        //[Required(ErrorMessage = "不能为空")]
        [DataType(DataType.Password)]
        [StringLength(6,MinimumLength =6,ErrorMessage ="注意密码长度为6")]
        public string Password { get; set; }
    }
}
