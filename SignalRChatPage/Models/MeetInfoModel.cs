using Microsoft.AspNetCore.Mvc;
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
        [Remote(action:"VerifyRoomNo",controller:"Hall")]
        public string RoomNo { get; set; }

        [Required(ErrorMessage ="不能为空")]
        public string Theme { get; set; }

        [Required(ErrorMessage = "不能为空")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString ="{0:yyyy-MM-ddThh:mm:ss}")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "不能为空")]
        [DataType(DataType.DateTime)]
        [CompareData]
        public DateTime EndTime { get; set; }

        //[Required(ErrorMessage = "不能为空")]
        [DataType(DataType.Password)]
        [StringLength(6,MinimumLength =6,ErrorMessage ="注意密码长度为6")]
        public string Password { get; set; }


        //时间类型的格式化转换（解决前端后台DateTime格式不匹配问题）
        public string ConvertToCustomizeForamtStr(DateTime datetime)
        {
            string _dt = string.Format("{0:yyyy-MM-ddTHH:mm}",datetime);

            return _dt;
        }
    }
}
