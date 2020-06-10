using SignalRChatPage.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SignalRChatPage.CustomerAttr
{
    //自定义数据校验的属性实现起止时间的比较；确保结束时间大于开始时间
    public class CompareDataAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (MeetInfoModel)validationContext.ObjectInstance;
            if(DateTime.Compare((DateTime)value,model.StartTime)<0)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return "起止时间有误请检查";
        }
    }
}
