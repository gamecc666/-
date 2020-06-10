using SignalRChatPage.Models;
using System;
using System.ComponentModel.DataAnnotations;

//请参考MSDN模型验证
namespace SignalRChatPage.CustomerAttr
{
    public class RoomAttribute:ValidationAttribute
    {        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Console.WriteLine($"输出[Room]属性下的value值：{value}");
            var room = (RoomModel)validationContext.ObjectInstance;
            if (string.IsNullOrEmpty(room.NickName))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return "请检查用户名是否为空";
        }
    }
}
